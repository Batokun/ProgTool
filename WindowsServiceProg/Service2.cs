using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Reflection;
using SelectPdf;
using System.IO.Compression;
using System.Security.Cryptography;

namespace WindowsServiceProg
{
    public partial class Service2 : ServiceBase
    {
        string InputDir = @"C:\ftproot\ICPROGSYS\PHX-in\";
        string OutputLocalDir = @"C:\IcraftProgOutput\";
        string LogDir = @"C:\ftproot\ICPROGSYS\log\";
        string OutputDir = @"C:\ftproot\ICPROGSYS\PDF-out\";
        string templatePathBB = @"C:\htmlTemplate\templateBB.html";
        string templatePathSS = @"C:\htmlTemplate\templateSS.html";
        Boolean checkFlagT1 = true;
        Boolean checkFlagT2 = true;
        DateTime timeYomiT1 = DateTime.Now;
        DateTime timeYomiT2 = DateTime.Now;

        StringBuilder LogStringT1 = new StringBuilder();
        StringBuilder ErrorLogStringT1 = new StringBuilder();
        StringBuilder LogStringT2 = new StringBuilder();
        StringBuilder ErrorLogStringT2 = new StringBuilder();
        StringBuilder UnzipTimeLogString = new StringBuilder();
        public Service2()
        {
            InitializeComponent();
           
        }

        protected override void OnStart(string[] args)
        {
            WriteServiceLog("Service is started at " + DateTime.Now);
            start();
        }

        protected override void OnStop()
        {
            WriteServiceLog("Service is stopped at " + DateTime.Now);
            checkFlagT1 = false;
            checkFlagT2 = false;
          
        }

        public void start()
        {
            checkFlagT1 = true;
            checkFlagT2 = true;
            Thread threadload1 = new Thread(new ThreadStart(PollingFuncT1));
            threadload1.Start();

            //Thread.Sleep(threadSleepTime * 1000); //setvalue (sec) * 1000(ms)
            Thread threadload2 = new Thread(new ThreadStart(PollingFuncT2));
            threadload2.Start();
        }
        public void PollingFuncT1()
        {
            try
            {
                while (checkFlagT1)
                {
                    LogStringT1 = new StringBuilder();
                    ErrorLogStringT1 = new StringBuilder();
                    timeYomiT1 = DateTime.Now;

                    importFile("T1", timeYomiT1);
                    writeLog("T1", timeYomiT1);
                    writeErrorLog("T1", timeYomiT1);
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT1.AppendLine("エラーT1：" + ex.Message.ToString());
                writeErrorLog("T1", timeYomiT1);
            }
        }

        public void PollingFuncT2()
        {
            try
            {
                while (checkFlagT2)
                {
                    LogStringT2 = new StringBuilder();
                    ErrorLogStringT2 = new StringBuilder();
                    timeYomiT2 = DateTime.Now;

                    importFile("T2", timeYomiT2);
                    writeLog("T2", timeYomiT2);
                    writeErrorLog("T2", timeYomiT2);
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT2.AppendLine("エラーT2：" + ex.Message.ToString());
                writeErrorLog("T2", timeYomiT2);
            }
        }

        bool checkMD5file(string MD5file, string ZIPfile, string ZIPfileName)
        {
            string zipToMD5 = "";
            string[] inputMD5;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(ZIPfile))
                {
                    byte[] checksum = md5.ComputeHash(stream);
                    zipToMD5 = BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();
                }
            }
            using (var streamReader = new StreamReader(MD5file))
            {
                var line = streamReader.ReadToEnd();
                inputMD5 = line.Split(' ');

            }
            if (inputMD5.Length > 1)
            {
                if (inputMD5[0] == zipToMD5 && inputMD5[1] == "*" + ZIPfileName)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        void unZipFile(string Outpath, string threadName)
        {
            DirectoryInfo dirInput = new DirectoryInfo(InputDir);
            FileInfo[] filesZIP = dirInput.GetFiles("*.zip");
            string OutputLocalDirErr = OutputLocalDir + @"Error\";

            foreach (var zipF in filesZIP)
            {
                if (File.Exists(zipF.FullName))
                {
                    try
                    {
                        if (File.Exists(zipF.FullName + ".md5"))
                        {
                            bool MD5checkFlag;
                            MD5checkFlag = checkMD5file(zipF.FullName + ".md5", zipF.FullName, zipF.Name);
                            if (MD5checkFlag)
                            {
                                FileStream fs = new FileStream(zipF.FullName + ".md5", FileMode.Open, FileAccess.Read, FileShare.None); //Not even reading rights
                                if (!Directory.Exists(Outpath))
                                    Directory.CreateDirectory(Outpath);
                                File.Move(zipF.FullName, Outpath + zipF.Name);
                                fs.Close();
                                File.Move(zipF.FullName + ".md5", Outpath + zipF.Name + ".md5");
                                if (File.Exists(Outpath + zipF.Name))
                                    ZipFile.ExtractToDirectory(Outpath + zipF.Name, Outpath);
                            }
                            else
                            {
                                using (StreamWriter sw = System.IO.File.CreateText(OutputDir + zipF.Name + "-err.txt"))
                                {
                                    sw.WriteLine(zipF.Name + " ファイル、" + zipF.Name + ".md5 ファイルがマッチしませんでした。");
                                }
                                if (!Directory.Exists(OutputLocalDirErr))
                                    Directory.CreateDirectory(OutputLocalDirErr);
                                if (File.Exists(OutputLocalDirErr + zipF.Name))
                                    File.Delete(OutputLocalDirErr + zipF.Name);
                                FileStream fs = new FileStream(zipF.FullName + ".md5", FileMode.Open, FileAccess.Read, FileShare.None); //Not even reading rights
                                File.Move(zipF.FullName, OutputLocalDirErr + zipF.Name);
                                fs.Close();
                                if (File.Exists(OutputLocalDirErr + zipF.Name + ".md5"))
                                    File.Delete(OutputLocalDirErr + zipF.Name + ".md5");
                                File.Move(zipF.FullName + ".md5", OutputLocalDirErr + zipF.Name + ".md5");
                            }
                            if (threadName == "T1")
                                LogStringT1.AppendLine(zipF.Name + "ファイルを読み込みました。");
                            else
                                LogStringT2.AppendLine(zipF.Name + "ファイルを読み込みました。");
                            break;
                        }
                        else
                        {
                            if (zipF.CreationTime.AddSeconds(40) < DateTime.Now)
                            {
                                using (StreamWriter sw = System.IO.File.CreateText(OutputDir + zipF.Name + "-err.txt"))
                                {
                                    sw.WriteLine(zipF.Name + "のMD5ファイルはなかったです。");
                                }
                                if (!Directory.Exists(OutputLocalDirErr))
                                    Directory.CreateDirectory(OutputLocalDirErr);
                                if (File.Exists(OutputLocalDirErr + zipF.Name))
                                    File.Delete(OutputLocalDirErr + zipF.Name);
                                File.Move(zipF.FullName, OutputLocalDirErr + zipF.Name);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (!e.ToString().Contains("別のプロセスで使用されているため、プロセスはファイル"))
                        {
                            if (threadName == "T1")
                                ErrorLogStringT1.AppendLine("unzipエラー：zipファイル名：" + zipF.Name + "　" + e.Message.ToString());
                            else
                                ErrorLogStringT2.AppendLine("unzipエラー：zipファイル名：" + zipF.Name + "　" + e.Message.ToString());
                        }
                    }
                }
            }
        }
        void writeLog(string threadName, DateTime timeYomi)
        {
            if (threadName == "T1")
            {
                if (LogStringT1.Length > 0)
                {
                    if (!Directory.Exists(LogDir))
                        Directory.CreateDirectory(LogDir);
                    using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + threadName + "-ログ.txt"))
                    {
                        sw.WriteLine(LogStringT1);
                    }
                }
            }
            else
            {
                if (LogStringT2.Length > 0)
                {
                    if (!Directory.Exists(LogDir))
                        Directory.CreateDirectory(LogDir);
                    using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + threadName + "-ログ.txt"))
                    {
                        sw.WriteLine(LogStringT2);
                    }
                }
            }
        }
        void writeErrorLog(string threadName, DateTime timeYomi)
        {
            if (threadName == "T1")
            {
                if (ErrorLogStringT1.Length > 0)
                {
                    if (!Directory.Exists(LogDir))
                        Directory.CreateDirectory(LogDir);
                    using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + threadName + "-エラーログ.txt"))
                    {
                        sw.WriteLine(ErrorLogStringT1);
                    }
                }
            }
            else
            {
                if (ErrorLogStringT2.Length > 0)
                {
                    if (!Directory.Exists(LogDir))
                        Directory.CreateDirectory(LogDir);
                    using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + threadName + "-エラーログ.txt"))
                    {
                        sw.WriteLine(ErrorLogStringT2);
                    }
                }
            }
        }

        public void importFile(string threadName, DateTime timeYomi)
        {
            datModel csvData = new datModel();
            try
            {
                string timeYomiString = timeYomi.ToString("yyyyMMddHHmmss") + threadName;
                string nowDoneDir = OutputLocalDir + timeYomiString + "\\";

                unZipFile(nowDoneDir, threadName);
                FileInfo[] filesCSV = new FileInfo[0];
                if (Directory.Exists(nowDoneDir))
                {
                    DirectoryInfo dirInput = new DirectoryInfo(nowDoneDir);
                    filesCSV = dirInput.GetFiles("*.csv");
                }

                if (!Directory.Exists(LogDir))
                    Directory.CreateDirectory(LogDir);

                foreach (var f in filesCSV)
                {
                    try
                    {
                        List<datModel> csvDataList = new List<datModel>();
                        //import csv file to database
                        DataTable dt = new DataTable();

                        using (StreamReader sr = new StreamReader(nowDoneDir + f.Name, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            int counter = new int();
                            string[] headers = sr.ReadLine().Split(',');
                            foreach (string header in headers)
                            {
                                dt.Columns.Add(header.Replace("\"", ""));
                                counter++;
                            }
                            while (!sr.EndOfStream)
                            {
                                string[] rows = sr.ReadLine().Split(',');
                                DataRow dr = dt.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    try { dr[i] = rows[i].Replace("\"", ""); }
                                    catch (IndexOutOfRangeException ind) { dr[i] = ""; }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr != null)
                            {

                                csvData = new datModel();
                                string ColName = string.Empty;

                                foreach (DataColumn column in dt.Columns)
                                {
                                    ColName = column.ColumnName;

                                    //	印刷順			CSV_COL名：	印刷順
                                    if (ColName == "印刷順")
                                    {
                                        csvData.印刷順 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	受験者ID			CSV_COL名：	受験者ID
                                    if (ColName == "受験者ID")
                                    {
                                        csvData.受験者ID = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	社員職員番号			CSV_COL名：	社員／職員番号
                                    if (ColName == "社員／職員番号")
                                    {
                                        csvData.社員職員番号 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	カナ氏名			CSV_COL名：	カナ氏名
                                    if (ColName == "カナ氏名")
                                    {
                                        csvData.カナ氏名 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	顧客名			CSV_COL名：	顧客名
                                    if (ColName == "顧客名")
                                    {
                                        csvData.顧客名 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	結果管理情報			CSV_COL名：	結果管理情報
                                    if (ColName == "結果管理情報")
                                    {
                                        csvData.結果管理情報 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	言語商品区分実施テスト			CSV_COL名：	言語・商品区分・実施テスト
                                    if (ColName == "言語・商品区分・実施テスト")
                                    {
                                        csvData.言語商品区分実施テスト = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	キャリアステージ			CSV_COL名：	キャリアステージ
                                    if (ColName == "Lキャリアステージコード")
                                    {
                                        csvData.Lキャリアステージコード = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	キャリアステージ			CSV_COL名：	キャリアステージ
                                    if (ColName == "Lキャリアステージ")
                                    {
                                        csvData.Lキャリアステージ = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	キャリアステージ			CSV_COL名：	キャリアステージ
                                    if (ColName == "Cキャリアステージコード")
                                    {
                                        csvData.Cキャリアステージコード = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	キャリアステージ			CSV_COL名：	キャリアステージ
                                    if (ColName == "Cキャリアステージ")
                                    {
                                        csvData.Cキャリアステージ = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	採点日			CSV_COL名：	採点日
                                    if (ColName == "採点日")
                                    {
                                        csvData.採点日 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合レベル			CSV_COL名：	リテラシー総合：レベル
                                    if (ColName == "リテラシー総合：レベル")
                                    {
                                        csvData.リテラシー総合レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合強化ポイント			CSV_COL名：	リテラシー総合：強化ポイント
                                    if (ColName == "リテラシー総合：強化ポイント")
                                    {
                                        csvData.リテラシー総合強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合CS平均			CSV_COL名：	リテラシー総合：CS平均
                                    if (ColName == "リテラシー総合：CS平均")
                                    {
                                        csvData.リテラシー総合CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合CS帯1			CSV_COL名：	リテラシー総合：CS帯1
                                    if (ColName == "リテラシー総合：CS帯1")
                                    {
                                        csvData.リテラシー総合CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合CS帯2			CSV_COL名：	リテラシー総合：CS帯2
                                    if (ColName == "リテラシー総合：CS帯2")
                                    {
                                        csvData.リテラシー総合CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合CS帯3			CSV_COL名：	リテラシー総合：CS帯3
                                    if (ColName == "リテラシー総合：CS帯3")
                                    {
                                        csvData.リテラシー総合CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合CS帯4			CSV_COL名：	リテラシー総合：CS帯4
                                    if (ColName == "リテラシー総合：CS帯4")
                                    {
                                        csvData.リテラシー総合CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー総合CS帯5			CSV_COL名：	リテラシー総合：CS帯5
                                    if (ColName == "リテラシー総合：CS帯5")
                                    {
                                        csvData.リテラシー総合CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力レベル			CSV_COL名：	リテラシー情報収集力：レベル
                                    if (ColName == "リテラシー情報収集力：レベル")
                                    {
                                        csvData.リテラシー情報収集力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力レベル			CSV_COL名：	リテラシー情報分析力：レベル
                                    if (ColName == "リテラシー情報分析力：レベル")
                                    {
                                        csvData.リテラシー情報分析力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力レベル			CSV_COL名：	リテラシー課題発見力：レベル
                                    if (ColName == "リテラシー課題発見力：レベル")
                                    {
                                        csvData.リテラシー課題発見力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力レベル			CSV_COL名：	リテラシー構想力：レベル
                                    if (ColName == "リテラシー構想力：レベル")
                                    {
                                        csvData.リテラシー構想力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力強化ポイント			CSV_COL名：	リテラシー情報収集力：強化ポイント
                                    if (ColName == "リテラシー情報収集力：強化ポイント")
                                    {
                                        csvData.リテラシー情報収集力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力強化ポイント			CSV_COL名：	リテラシー情報分析力：強化ポイント
                                    if (ColName == "リテラシー情報分析力：強化ポイント")
                                    {
                                        csvData.リテラシー情報分析力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力強化ポイント			CSV_COL名：	リテラシー課題発見力：強化ポイント
                                    if (ColName == "リテラシー課題発見力：強化ポイント")
                                    {
                                        csvData.リテラシー課題発見力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力強化ポイント			CSV_COL名：	リテラシー構想力：強化ポイント
                                    if (ColName == "リテラシー構想力：強化ポイント")
                                    {
                                        csvData.リテラシー構想力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力CS平均			CSV_COL名：	リテラシー情報収集力：CS平均
                                    if (ColName == "リテラシー情報収集力：CS平均")
                                    {
                                        csvData.リテラシー情報収集力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力CS平均			CSV_COL名：	リテラシー情報分析力：CS平均
                                    if (ColName == "リテラシー情報分析力：CS平均")
                                    {
                                        csvData.リテラシー情報分析力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力CS平均			CSV_COL名：	リテラシー課題発見力：CS平均
                                    if (ColName == "リテラシー課題発見力：CS平均")
                                    {
                                        csvData.リテラシー課題発見力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力CS平均			CSV_COL名：	リテラシー構想力：CS平均
                                    if (ColName == "リテラシー構想力：CS平均")
                                    {
                                        csvData.リテラシー構想力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力CS帯1			CSV_COL名：	リテラシー情報収集力：CS帯1
                                    if (ColName == "リテラシー情報収集力：CS帯1")
                                    {
                                        csvData.リテラシー情報収集力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力CS帯2			CSV_COL名：	リテラシー情報収集力：CS帯2
                                    if (ColName == "リテラシー情報収集力：CS帯2")
                                    {
                                        csvData.リテラシー情報収集力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力CS帯3			CSV_COL名：	リテラシー情報収集力：CS帯3
                                    if (ColName == "リテラシー情報収集力：CS帯3")
                                    {
                                        csvData.リテラシー情報収集力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力CS帯4			CSV_COL名：	リテラシー情報収集力：CS帯4
                                    if (ColName == "リテラシー情報収集力：CS帯4")
                                    {
                                        csvData.リテラシー情報収集力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報収集力CS帯5			CSV_COL名：	リテラシー情報収集力：CS帯5
                                    if (ColName == "リテラシー情報収集力：CS帯5")
                                    {
                                        csvData.リテラシー情報収集力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力CS帯1			CSV_COL名：	リテラシー情報分析力：CS帯1
                                    if (ColName == "リテラシー情報分析力：CS帯1")
                                    {
                                        csvData.リテラシー情報分析力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力CS帯2			CSV_COL名：	リテラシー情報分析力：CS帯2
                                    if (ColName == "リテラシー情報分析力：CS帯2")
                                    {
                                        csvData.リテラシー情報分析力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力CS帯3			CSV_COL名：	リテラシー情報分析力：CS帯3
                                    if (ColName == "リテラシー情報分析力：CS帯3")
                                    {
                                        csvData.リテラシー情報分析力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力CS帯4			CSV_COL名：	リテラシー情報分析力：CS帯4
                                    if (ColName == "リテラシー情報分析力：CS帯4")
                                    {
                                        csvData.リテラシー情報分析力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー情報分析力CS帯5			CSV_COL名：	リテラシー情報分析力：CS帯5
                                    if (ColName == "リテラシー情報分析力：CS帯5")
                                    {
                                        csvData.リテラシー情報分析力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力CS帯1			CSV_COL名：	リテラシー課題発見力：CS帯1
                                    if (ColName == "リテラシー課題発見力：CS帯1")
                                    {
                                        csvData.リテラシー課題発見力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力CS帯2			CSV_COL名：	リテラシー課題発見力：CS帯2
                                    if (ColName == "リテラシー課題発見力：CS帯2")
                                    {
                                        csvData.リテラシー課題発見力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力CS帯3			CSV_COL名：	リテラシー課題発見力：CS帯3
                                    if (ColName == "リテラシー課題発見力：CS帯3")
                                    {
                                        csvData.リテラシー課題発見力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力CS帯4			CSV_COL名：	リテラシー課題発見力：CS帯4
                                    if (ColName == "リテラシー課題発見力：CS帯4")
                                    {
                                        csvData.リテラシー課題発見力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー課題発見力CS帯5			CSV_COL名：	リテラシー課題発見力：CS帯5
                                    if (ColName == "リテラシー課題発見力：CS帯5")
                                    {
                                        csvData.リテラシー課題発見力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力CS帯1			CSV_COL名：	リテラシー構想力：CS帯1
                                    if (ColName == "リテラシー構想力：CS帯1")
                                    {
                                        csvData.リテラシー構想力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力CS帯2			CSV_COL名：	リテラシー構想力：CS帯2
                                    if (ColName == "リテラシー構想力：CS帯2")
                                    {
                                        csvData.リテラシー構想力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力CS帯3			CSV_COL名：	リテラシー構想力：CS帯3
                                    if (ColName == "リテラシー構想力：CS帯3")
                                    {
                                        csvData.リテラシー構想力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力CS帯4			CSV_COL名：	リテラシー構想力：CS帯4
                                    if (ColName == "リテラシー構想力：CS帯4")
                                    {
                                        csvData.リテラシー構想力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リテラシー構想力CS帯5			CSV_COL名：	リテラシー構想力：CS帯5
                                    if (ColName == "リテラシー構想力：CS帯5")
                                    {
                                        csvData.リテラシー構想力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合レベル			CSV_COL名：	コンピテンシー総合：レベル
                                    if (ColName == "コンピテンシー総合：レベル")
                                    {
                                        csvData.コンピテンシー総合レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合強化ポイント			CSV_COL名：	コンピテンシー総合：強化ポイント
                                    if (ColName == "コンピテンシー総合：強化ポイント")
                                    {
                                        csvData.コンピテンシー総合強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合CS平均			CSV_COL名：	コンピテンシー総合：CS平均
                                    if (ColName == "コンピテンシー総合：CS平均")
                                    {
                                        csvData.コンピテンシー総合CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合CS帯1			CSV_COL名：	コンピテンシー総合：CS帯1
                                    if (ColName == "コンピテンシー総合：CS帯1")
                                    {
                                        csvData.コンピテンシー総合CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合CS帯2			CSV_COL名：	コンピテンシー総合：CS帯2
                                    if (ColName == "コンピテンシー総合：CS帯2")
                                    {
                                        csvData.コンピテンシー総合CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合CS帯3			CSV_COL名：	コンピテンシー総合：CS帯3
                                    if (ColName == "コンピテンシー総合：CS帯3")
                                    {
                                        csvData.コンピテンシー総合CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合CS帯4			CSV_COL名：	コンピテンシー総合：CS帯4
                                    if (ColName == "コンピテンシー総合：CS帯4")
                                    {
                                        csvData.コンピテンシー総合CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	コンピテンシー総合CS帯5			CSV_COL名：	コンピテンシー総合：CS帯5
                                    if (ColName == "コンピテンシー総合：CS帯5")
                                    {
                                        csvData.コンピテンシー総合CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力レベル			CSV_COL名：	対人基礎力：レベル
                                    if (ColName == "対人基礎力：レベル")
                                    {
                                        csvData.対人基礎力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力レベル			CSV_COL名：	対自己基礎力：レベル
                                    if (ColName == "対自己基礎力：レベル")
                                    {
                                        csvData.対自己基礎力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力レベル			CSV_COL名：	対課題基礎力：レベル
                                    if (ColName == "対課題基礎力：レベル")
                                    {
                                        csvData.対課題基礎力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力強化ポイント			CSV_COL名：	対人基礎力：強化ポイント
                                    if (ColName == "対人基礎力：強化ポイント")
                                    {
                                        csvData.対人基礎力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力強化ポイント			CSV_COL名：	対自己基礎力：強化ポイント
                                    if (ColName == "対自己基礎力：強化ポイント")
                                    {
                                        csvData.対自己基礎力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力強化ポイント			CSV_COL名：	対課題基礎力：強化ポイント
                                    if (ColName == "対課題基礎力：強化ポイント")
                                    {
                                        csvData.対課題基礎力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力CS平均			CSV_COL名：	対人基礎力：CS平均
                                    if (ColName == "対人基礎力：CS平均")
                                    {
                                        csvData.対人基礎力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力CS平均			CSV_COL名：	対自己基礎力：CS平均
                                    if (ColName == "対自己基礎力：CS平均")
                                    {
                                        csvData.対自己基礎力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力CS平均			CSV_COL名：	対課題基礎力：CS平均
                                    if (ColName == "対課題基礎力：CS平均")
                                    {
                                        csvData.対課題基礎力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力CS帯1			CSV_COL名：	対人基礎力：CS帯1
                                    if (ColName == "対人基礎力：CS帯1")
                                    {
                                        csvData.対人基礎力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力CS帯2			CSV_COL名：	対人基礎力：CS帯2
                                    if (ColName == "対人基礎力：CS帯2")
                                    {
                                        csvData.対人基礎力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力CS帯3			CSV_COL名：	対人基礎力：CS帯3
                                    if (ColName == "対人基礎力：CS帯3")
                                    {
                                        csvData.対人基礎力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力CS帯4			CSV_COL名：	対人基礎力：CS帯4
                                    if (ColName == "対人基礎力：CS帯4")
                                    {
                                        csvData.対人基礎力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人基礎力CS帯5			CSV_COL名：	対人基礎力：CS帯5
                                    if (ColName == "対人基礎力：CS帯5")
                                    {
                                        csvData.対人基礎力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力CS帯1			CSV_COL名：	対自己基礎力：CS帯1
                                    if (ColName == "対自己基礎力：CS帯1")
                                    {
                                        csvData.対自己基礎力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力CS帯2			CSV_COL名：	対自己基礎力：CS帯2
                                    if (ColName == "対自己基礎力：CS帯2")
                                    {
                                        csvData.対自己基礎力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力CS帯3			CSV_COL名：	対自己基礎力：CS帯3
                                    if (ColName == "対自己基礎力：CS帯3")
                                    {
                                        csvData.対自己基礎力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力CS帯4			CSV_COL名：	対自己基礎力：CS帯4
                                    if (ColName == "対自己基礎力：CS帯4")
                                    {
                                        csvData.対自己基礎力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対自己基礎力CS帯5			CSV_COL名：	対自己基礎力：CS帯5
                                    if (ColName == "対自己基礎力：CS帯5")
                                    {
                                        csvData.対自己基礎力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力CS帯1			CSV_COL名：	対課題基礎力：CS帯1
                                    if (ColName == "対課題基礎力：CS帯1")
                                    {
                                        csvData.対課題基礎力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力CS帯2			CSV_COL名：	対課題基礎力：CS帯2
                                    if (ColName == "対課題基礎力：CS帯2")
                                    {
                                        csvData.対課題基礎力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力CS帯3			CSV_COL名：	対課題基礎力：CS帯3
                                    if (ColName == "対課題基礎力：CS帯3")
                                    {
                                        csvData.対課題基礎力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力CS帯4			CSV_COL名：	対課題基礎力：CS帯4
                                    if (ColName == "対課題基礎力：CS帯4")
                                    {
                                        csvData.対課題基礎力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対課題基礎力CS帯5			CSV_COL名：	対課題基礎力：CS帯5
                                    if (ColName == "対課題基礎力：CS帯5")
                                    {
                                        csvData.対課題基礎力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力レベル			CSV_COL名：	親和力：レベル
                                    if (ColName == "親和力：レベル")
                                    {
                                        csvData.親和力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力レベル			CSV_COL名：	協働力：レベル
                                    if (ColName == "協働力：レベル")
                                    {
                                        csvData.協働力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力レベル			CSV_COL名：	統率力：レベル
                                    if (ColName == "統率力：レベル")
                                    {
                                        csvData.統率力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力レベル			CSV_COL名：	感情制御力：レベル
                                    if (ColName == "感情制御力：レベル")
                                    {
                                        csvData.感情制御力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力レベル			CSV_COL名：	自信創出力：レベル
                                    if (ColName == "自信創出力：レベル")
                                    {
                                        csvData.自信創出力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力レベル			CSV_COL名：	行動持続力：レベル
                                    if (ColName == "行動持続力：レベル")
                                    {
                                        csvData.行動持続力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力レベル			CSV_COL名：	課題発見力：レベル
                                    if (ColName == "課題発見力：レベル")
                                    {
                                        csvData.課題発見力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力レベル			CSV_COL名：	計画立案力：レベル
                                    if (ColName == "計画立案力：レベル")
                                    {
                                        csvData.計画立案力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力レベル			CSV_COL名：	実践力：レベル
                                    if (ColName == "実践力：レベル")
                                    {
                                        csvData.実践力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力強化ポイント			CSV_COL名：	親和力：強化ポイント
                                    if (ColName == "親和力：強化ポイント")
                                    {
                                        csvData.親和力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力強化ポイント			CSV_COL名：	協働力：強化ポイント
                                    if (ColName == "協働力：強化ポイント")
                                    {
                                        csvData.協働力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力強化ポイント			CSV_COL名：	統率力：強化ポイント
                                    if (ColName == "統率力：強化ポイント")
                                    {
                                        csvData.統率力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力強化ポイント			CSV_COL名：	感情制御力：強化ポイント
                                    if (ColName == "感情制御力：強化ポイント")
                                    {
                                        csvData.感情制御力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力強化ポイント			CSV_COL名：	自信創出力：強化ポイント
                                    if (ColName == "自信創出力：強化ポイント")
                                    {
                                        csvData.自信創出力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力強化ポイント			CSV_COL名：	行動持続力：強化ポイント
                                    if (ColName == "行動持続力：強化ポイント")
                                    {
                                        csvData.行動持続力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力強化ポイント			CSV_COL名：	課題発見力：強化ポイント
                                    if (ColName == "課題発見力：強化ポイント")
                                    {
                                        csvData.課題発見力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力強化ポイント			CSV_COL名：	計画立案力：強化ポイント
                                    if (ColName == "計画立案力：強化ポイント")
                                    {
                                        csvData.計画立案力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力強化ポイント			CSV_COL名：	実践力：強化ポイント
                                    if (ColName == "実践力：強化ポイント")
                                    {
                                        csvData.実践力強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力CS平均			CSV_COL名：	親和力：CS平均
                                    if (ColName == "親和力：CS平均")
                                    {
                                        csvData.親和力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力CS平均			CSV_COL名：	協働力：CS平均
                                    if (ColName == "協働力：CS平均")
                                    {
                                        csvData.協働力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力CS平均			CSV_COL名：	統率力：CS平均
                                    if (ColName == "統率力：CS平均")
                                    {
                                        csvData.統率力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力CS平均			CSV_COL名：	感情制御力：CS平均
                                    if (ColName == "感情制御力：CS平均")
                                    {
                                        csvData.感情制御力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力CS平均			CSV_COL名：	自信創出力：CS平均
                                    if (ColName == "自信創出力：CS平均")
                                    {
                                        csvData.自信創出力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力CS平均			CSV_COL名：	行動持続力：CS平均
                                    if (ColName == "行動持続力：CS平均")
                                    {
                                        csvData.行動持続力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力CS平均			CSV_COL名：	課題発見力：CS平均
                                    if (ColName == "課題発見力：CS平均")
                                    {
                                        csvData.課題発見力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力CS平均			CSV_COL名：	計画立案力：CS平均
                                    if (ColName == "計画立案力：CS平均")
                                    {
                                        csvData.計画立案力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力CS平均			CSV_COL名：	実践力：CS平均
                                    if (ColName == "実践力：CS平均")
                                    {
                                        csvData.実践力CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力CS帯1			CSV_COL名：	親和力：CS帯1
                                    if (ColName == "親和力：CS帯1")
                                    {
                                        csvData.親和力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力CS帯2			CSV_COL名：	親和力：CS帯2
                                    if (ColName == "親和力：CS帯2")
                                    {
                                        csvData.親和力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力CS帯3			CSV_COL名：	親和力：CS帯3
                                    if (ColName == "親和力：CS帯3")
                                    {
                                        csvData.親和力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力CS帯4			CSV_COL名：	親和力：CS帯4
                                    if (ColName == "親和力：CS帯4")
                                    {
                                        csvData.親和力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親和力CS帯5			CSV_COL名：	親和力：CS帯5
                                    if (ColName == "親和力：CS帯5")
                                    {
                                        csvData.親和力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力CS帯1			CSV_COL名：	協働力：CS帯1
                                    if (ColName == "協働力：CS帯1")
                                    {
                                        csvData.協働力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力CS帯2			CSV_COL名：	協働力：CS帯2
                                    if (ColName == "協働力：CS帯2")
                                    {
                                        csvData.協働力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力CS帯3			CSV_COL名：	協働力：CS帯3
                                    if (ColName == "協働力：CS帯3")
                                    {
                                        csvData.協働力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力CS帯4			CSV_COL名：	協働力：CS帯4
                                    if (ColName == "協働力：CS帯4")
                                    {
                                        csvData.協働力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	協働力CS帯5			CSV_COL名：	協働力：CS帯5
                                    if (ColName == "協働力：CS帯5")
                                    {
                                        csvData.協働力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力CS帯1			CSV_COL名：	統率力：CS帯1
                                    if (ColName == "統率力：CS帯1")
                                    {
                                        csvData.統率力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力CS帯2			CSV_COL名：	統率力：CS帯2
                                    if (ColName == "統率力：CS帯2")
                                    {
                                        csvData.統率力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力CS帯3			CSV_COL名：	統率力：CS帯3
                                    if (ColName == "統率力：CS帯3")
                                    {
                                        csvData.統率力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力CS帯4			CSV_COL名：	統率力：CS帯4
                                    if (ColName == "統率力：CS帯4")
                                    {
                                        csvData.統率力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	統率力CS帯5			CSV_COL名：	統率力：CS帯5
                                    if (ColName == "統率力：CS帯5")
                                    {
                                        csvData.統率力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力CS帯1			CSV_COL名：	感情制御力：CS帯1
                                    if (ColName == "感情制御力：CS帯1")
                                    {
                                        csvData.感情制御力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力CS帯2			CSV_COL名：	感情制御力：CS帯2
                                    if (ColName == "感情制御力：CS帯2")
                                    {
                                        csvData.感情制御力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力CS帯3			CSV_COL名：	感情制御力：CS帯3
                                    if (ColName == "感情制御力：CS帯3")
                                    {
                                        csvData.感情制御力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力CS帯4			CSV_COL名：	感情制御力：CS帯4
                                    if (ColName == "感情制御力：CS帯4")
                                    {
                                        csvData.感情制御力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	感情制御力CS帯5			CSV_COL名：	感情制御力：CS帯5
                                    if (ColName == "感情制御力：CS帯5")
                                    {
                                        csvData.感情制御力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力CS帯1			CSV_COL名：	自信創出力：CS帯1
                                    if (ColName == "自信創出力：CS帯1")
                                    {
                                        csvData.自信創出力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力CS帯2			CSV_COL名：	自信創出力：CS帯2
                                    if (ColName == "自信創出力：CS帯2")
                                    {
                                        csvData.自信創出力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力CS帯3			CSV_COL名：	自信創出力：CS帯3
                                    if (ColName == "自信創出力：CS帯3")
                                    {
                                        csvData.自信創出力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力CS帯4			CSV_COL名：	自信創出力：CS帯4
                                    if (ColName == "自信創出力：CS帯4")
                                    {
                                        csvData.自信創出力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自信創出力CS帯5			CSV_COL名：	自信創出力：CS帯5
                                    if (ColName == "自信創出力：CS帯5")
                                    {
                                        csvData.自信創出力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力CS帯1			CSV_COL名：	行動持続力：CS帯1
                                    if (ColName == "行動持続力：CS帯1")
                                    {
                                        csvData.行動持続力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力CS帯2			CSV_COL名：	行動持続力：CS帯2
                                    if (ColName == "行動持続力：CS帯2")
                                    {
                                        csvData.行動持続力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力CS帯3			CSV_COL名：	行動持続力：CS帯3
                                    if (ColName == "行動持続力：CS帯3")
                                    {
                                        csvData.行動持続力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力CS帯4			CSV_COL名：	行動持続力：CS帯4
                                    if (ColName == "行動持続力：CS帯4")
                                    {
                                        csvData.行動持続力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	行動持続力CS帯5			CSV_COL名：	行動持続力：CS帯5
                                    if (ColName == "行動持続力：CS帯5")
                                    {
                                        csvData.行動持続力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力CS帯1			CSV_COL名：	課題発見力：CS帯1
                                    if (ColName == "課題発見力：CS帯1")
                                    {
                                        csvData.課題発見力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力CS帯2			CSV_COL名：	課題発見力：CS帯2
                                    if (ColName == "課題発見力：CS帯2")
                                    {
                                        csvData.課題発見力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力CS帯3			CSV_COL名：	課題発見力：CS帯3
                                    if (ColName == "課題発見力：CS帯3")
                                    {
                                        csvData.課題発見力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力CS帯4			CSV_COL名：	課題発見力：CS帯4
                                    if (ColName == "課題発見力：CS帯4")
                                    {
                                        csvData.課題発見力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	課題発見力CS帯5			CSV_COL名：	課題発見力：CS帯5
                                    if (ColName == "課題発見力：CS帯5")
                                    {
                                        csvData.課題発見力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力CS帯1			CSV_COL名：	計画立案力：CS帯1
                                    if (ColName == "計画立案力：CS帯1")
                                    {
                                        csvData.計画立案力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力CS帯2			CSV_COL名：	計画立案力：CS帯2
                                    if (ColName == "計画立案力：CS帯2")
                                    {
                                        csvData.計画立案力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力CS帯3			CSV_COL名：	計画立案力：CS帯3
                                    if (ColName == "計画立案力：CS帯3")
                                    {
                                        csvData.計画立案力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力CS帯4			CSV_COL名：	計画立案力：CS帯4
                                    if (ColName == "計画立案力：CS帯4")
                                    {
                                        csvData.計画立案力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画立案力CS帯5			CSV_COL名：	計画立案力：CS帯5
                                    if (ColName == "計画立案力：CS帯5")
                                    {
                                        csvData.計画立案力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力CS帯1			CSV_COL名：	実践力：CS帯1
                                    if (ColName == "実践力：CS帯1")
                                    {
                                        csvData.実践力CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力CS帯2			CSV_COL名：	実践力：CS帯2
                                    if (ColName == "実践力：CS帯2")
                                    {
                                        csvData.実践力CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力CS帯3			CSV_COL名：	実践力：CS帯3
                                    if (ColName == "実践力：CS帯3")
                                    {
                                        csvData.実践力CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力CS帯4			CSV_COL名：	実践力：CS帯4
                                    if (ColName == "実践力：CS帯4")
                                    {
                                        csvData.実践力CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践力CS帯5			CSV_COL名：	実践力：CS帯5
                                    if (ColName == "実践力：CS帯5")
                                    {
                                        csvData.実践力CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさレベル			CSV_COL名：	親しみやすさ：レベル
                                    if (ColName == "親しみやすさ：レベル")
                                    {
                                        csvData.親しみやすさレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りレベル			CSV_COL名：	気配り：レベル
                                    if (ColName == "気配り：レベル")
                                    {
                                        csvData.気配りレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容レベル			CSV_COL名：	対人興味／共感・受容：レベル
                                    if (ColName == "対人興味／共感・受容：レベル")
                                    {
                                        csvData.対人興味共感受容レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解レベル			CSV_COL名：	多様性理解：レベル
                                    if (ColName == "多様性理解：レベル")
                                    {
                                        csvData.多様性理解レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成レベル			CSV_COL名：	人脈形成：レベル
                                    if (ColName == "人脈形成：レベル")
                                    {
                                        csvData.人脈形成レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築レベル			CSV_COL名：	信頼構築：レベル
                                    if (ColName == "信頼構築：レベル")
                                    {
                                        csvData.信頼構築レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動レベル			CSV_COL名：	役割理解・連携行動：レベル
                                    if (ColName == "役割理解・連携行動：レベル")
                                    {
                                        csvData.役割理解連携行動レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有レベル			CSV_COL名：	情報共有：レベル
                                    if (ColName == "情報共有：レベル")
                                    {
                                        csvData.情報共有レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援レベル			CSV_COL名：	相互支援：レベル
                                    if (ColName == "相互支援：レベル")
                                    {
                                        csvData.相互支援レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけレベル			CSV_COL名：	相談・指導・他者の動機づけ：レベル
                                    if (ColName == "相談・指導・他者の動機づけ：レベル")
                                    {
                                        csvData.相談指導他者の動機づけレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうレベル			CSV_COL名：	話しあう：レベル
                                    if (ColName == "話しあう：レベル")
                                    {
                                        csvData.話しあうレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するレベル			CSV_COL名：	意見を主張する：レベル
                                    if (ColName == "意見を主張する：レベル")
                                    {
                                        csvData.意見を主張するレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議レベル			CSV_COL名：	建設的・創造的な討議：レベル
                                    if (ColName == "建設的・創造的な討議：レベル")
                                    {
                                        csvData.建設的創造的な討議レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得レベル			CSV_COL名：	意見の調整、交渉、説得：レベル
                                    if (ColName == "意見の調整、交渉、説得：レベル")
                                    {
                                        csvData.意見の調整交渉説得レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスレベル			CSV_COL名：	セルフアウェアネス：レベル
                                    if (ColName == "セルフアウェアネス：レベル")
                                    {
                                        csvData.セルフアウェアネスレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングレベル			CSV_COL名：	ストレスコーピング：レベル
                                    if (ColName == "ストレスコーピング：レベル")
                                    {
                                        csvData.ストレスコーピングレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントレベル			CSV_COL名：	ストレスマネジメント：レベル
                                    if (ColName == "ストレスマネジメント：レベル")
                                    {
                                        csvData.ストレスマネジメントレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解レベル			CSV_COL名：	独自性理解：レベル
                                    if (ColName == "独自性理解：レベル")
                                    {
                                        csvData.独自性理解レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性レベル			CSV_COL名：	自己効力感／楽観性：レベル
                                    if (ColName == "自己効力感／楽観性：レベル")
                                    {
                                        csvData.自己効力感楽観性レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革レベル			CSV_COL名：	学習視点・機会による自己変革：レベル
                                    if (ColName == "学習視点・機会による自己変革：レベル")
                                    {
                                        csvData.学習視点機会による自己変革レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動レベル			CSV_COL名：	主体的行動：レベル
                                    if (ColName == "主体的行動：レベル")
                                    {
                                        csvData.主体的行動レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂レベル			CSV_COL名：	完遂：レベル
                                    if (ColName == "完遂：レベル")
                                    {
                                        csvData.完遂レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化レベル			CSV_COL名：	良い行動の習慣化：レベル
                                    if (ColName == "良い行動の習慣化：レベル")
                                    {
                                        csvData.良い行動の習慣化レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集レベル			CSV_COL名：	情報収集：レベル
                                    if (ColName == "情報収集：レベル")
                                    {
                                        csvData.情報収集レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解レベル			CSV_COL名：	本質理解：レベル
                                    if (ColName == "本質理解：レベル")
                                    {
                                        csvData.本質理解レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究レベル			CSV_COL名：	原因追究：レベル
                                    if (ColName == "原因追究：レベル")
                                    {
                                        csvData.原因追究レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定レベル			CSV_COL名：	目標設定：レベル
                                    if (ColName == "目標設定：レベル")
                                    {
                                        csvData.目標設定レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築レベル			CSV_COL名：	シナリオ構築：レベル
                                    if (ColName == "シナリオ構築：レベル")
                                    {
                                        csvData.シナリオ構築レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価レベル			CSV_COL名：	計画評価：レベル
                                    if (ColName == "計画評価：レベル")
                                    {
                                        csvData.計画評価レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析レベル			CSV_COL名：	リスク分析：レベル
                                    if (ColName == "リスク分析：レベル")
                                    {
                                        csvData.リスク分析レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動レベル			CSV_COL名：	実践行動：レベル
                                    if (ColName == "実践行動：レベル")
                                    {
                                        csvData.実践行動レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整レベル			CSV_COL名：	修正／調整：レベル
                                    if (ColName == "修正／調整：レベル")
                                    {
                                        csvData.修正調整レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善レベル			CSV_COL名：	検証／改善：レベル
                                    if (ColName == "検証／改善：レベル")
                                    {
                                        csvData.検証改善レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさ強化ポイント			CSV_COL名：	親しみやすさ：強化ポイント
                                    if (ColName == "親しみやすさ：強化ポイント")
                                    {
                                        csvData.親しみやすさ強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配り強化ポイント			CSV_COL名：	気配り：強化ポイント
                                    if (ColName == "気配り：強化ポイント")
                                    {
                                        csvData.気配り強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容強化ポイント			CSV_COL名：	対人興味／共感・受容：強化ポイント
                                    if (ColName == "対人興味／共感・受容：強化ポイント")
                                    {
                                        csvData.対人興味共感受容強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解強化ポイント			CSV_COL名：	多様性理解：強化ポイント
                                    if (ColName == "多様性理解：強化ポイント")
                                    {
                                        csvData.多様性理解強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成強化ポイント			CSV_COL名：	人脈形成：強化ポイント
                                    if (ColName == "人脈形成：強化ポイント")
                                    {
                                        csvData.人脈形成強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築強化ポイント			CSV_COL名：	信頼構築：強化ポイント
                                    if (ColName == "信頼構築：強化ポイント")
                                    {
                                        csvData.信頼構築強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動強化ポイント			CSV_COL名：	役割理解・連携行動：強化ポイント
                                    if (ColName == "役割理解・連携行動：強化ポイント")
                                    {
                                        csvData.役割理解連携行動強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有強化ポイント			CSV_COL名：	情報共有：強化ポイント
                                    if (ColName == "情報共有：強化ポイント")
                                    {
                                        csvData.情報共有強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援強化ポイント			CSV_COL名：	相互支援：強化ポイント
                                    if (ColName == "相互支援：強化ポイント")
                                    {
                                        csvData.相互支援強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけ強化ポイント			CSV_COL名：	相談・指導・他者の動機づけ：強化ポイント
                                    if (ColName == "相談・指導・他者の動機づけ：強化ポイント")
                                    {
                                        csvData.相談指導他者の動機づけ強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあう強化ポイント			CSV_COL名：	話しあう：強化ポイント
                                    if (ColName == "話しあう：強化ポイント")
                                    {
                                        csvData.話しあう強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張する強化ポイント			CSV_COL名：	意見を主張する：強化ポイント
                                    if (ColName == "意見を主張する：強化ポイント")
                                    {
                                        csvData.意見を主張する強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議強化ポイント			CSV_COL名：	建設的・創造的な討議：強化ポイント
                                    if (ColName == "建設的・創造的な討議：強化ポイント")
                                    {
                                        csvData.建設的創造的な討議強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得強化ポイント			CSV_COL名：	意見の調整、交渉、説得：強化ポイント
                                    if (ColName == "意見の調整、交渉、説得：強化ポイント")
                                    {
                                        csvData.意見の調整交渉説得強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネス強化ポイント			CSV_COL名：	セルフアウェアネス：強化ポイント
                                    if (ColName == "セルフアウェアネス：強化ポイント")
                                    {
                                        csvData.セルフアウェアネス強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピング強化ポイント			CSV_COL名：	ストレスコーピング：強化ポイント
                                    if (ColName == "ストレスコーピング：強化ポイント")
                                    {
                                        csvData.ストレスコーピング強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメント強化ポイント			CSV_COL名：	ストレスマネジメント：強化ポイント
                                    if (ColName == "ストレスマネジメント：強化ポイント")
                                    {
                                        csvData.ストレスマネジメント強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解強化ポイント			CSV_COL名：	独自性理解：強化ポイント
                                    if (ColName == "独自性理解：強化ポイント")
                                    {
                                        csvData.独自性理解強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性強化ポイント			CSV_COL名：	自己効力感／楽観性：強化ポイント
                                    if (ColName == "自己効力感／楽観性：強化ポイント")
                                    {
                                        csvData.自己効力感楽観性強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革強化ポイント			CSV_COL名：	学習視点・機会による自己変革：強化ポイント
                                    if (ColName == "学習視点・機会による自己変革：強化ポイント")
                                    {
                                        csvData.学習視点機会による自己変革強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動強化ポイント			CSV_COL名：	主体的行動：強化ポイント
                                    if (ColName == "主体的行動：強化ポイント")
                                    {
                                        csvData.主体的行動強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂強化ポイント			CSV_COL名：	完遂：強化ポイント
                                    if (ColName == "完遂：強化ポイント")
                                    {
                                        csvData.完遂強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化強化ポイント			CSV_COL名：	良い行動の習慣化：強化ポイント
                                    if (ColName == "良い行動の習慣化：強化ポイント")
                                    {
                                        csvData.良い行動の習慣化強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集強化ポイント			CSV_COL名：	情報収集：強化ポイント
                                    if (ColName == "情報収集：強化ポイント")
                                    {
                                        csvData.情報収集強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解強化ポイント			CSV_COL名：	本質理解：強化ポイント
                                    if (ColName == "本質理解：強化ポイント")
                                    {
                                        csvData.本質理解強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究強化ポイント			CSV_COL名：	原因追究：強化ポイント
                                    if (ColName == "原因追究：強化ポイント")
                                    {
                                        csvData.原因追究強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定強化ポイント			CSV_COL名：	目標設定：強化ポイント
                                    if (ColName == "目標設定：強化ポイント")
                                    {
                                        csvData.目標設定強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築強化ポイント			CSV_COL名：	シナリオ構築：強化ポイント
                                    if (ColName == "シナリオ構築：強化ポイント")
                                    {
                                        csvData.シナリオ構築強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価強化ポイント			CSV_COL名：	計画評価：強化ポイント
                                    if (ColName == "計画評価：強化ポイント")
                                    {
                                        csvData.計画評価強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析強化ポイント			CSV_COL名：	リスク分析：強化ポイント
                                    if (ColName == "リスク分析：強化ポイント")
                                    {
                                        csvData.リスク分析強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動強化ポイント			CSV_COL名：	実践行動：強化ポイント
                                    if (ColName == "実践行動：強化ポイント")
                                    {
                                        csvData.実践行動強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整強化ポイント			CSV_COL名：	修正／調整：強化ポイント
                                    if (ColName == "修正／調整：強化ポイント")
                                    {
                                        csvData.修正調整強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善強化ポイント			CSV_COL名：	検証／改善：強化ポイント
                                    if (ColName == "検証／改善：強化ポイント")
                                    {
                                        csvData.検証改善強化ポイント = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさCS平均			CSV_COL名：	親しみやすさ：CS平均
                                    if (ColName == "親しみやすさ：CS平均")
                                    {
                                        csvData.親しみやすさCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りCS平均			CSV_COL名：	気配り：CS平均
                                    if (ColName == "気配り：CS平均")
                                    {
                                        csvData.気配りCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容CS平均			CSV_COL名：	対人興味／共感・受容：CS平均
                                    if (ColName == "対人興味／共感・受容：CS平均")
                                    {
                                        csvData.対人興味共感受容CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解CS平均			CSV_COL名：	多様性理解：CS平均
                                    if (ColName == "多様性理解：CS平均")
                                    {
                                        csvData.多様性理解CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成CS平均			CSV_COL名：	人脈形成：CS平均
                                    if (ColName == "人脈形成：CS平均")
                                    {
                                        csvData.人脈形成CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築CS平均			CSV_COL名：	信頼構築：CS平均
                                    if (ColName == "信頼構築：CS平均")
                                    {
                                        csvData.信頼構築CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動CS平均			CSV_COL名：	役割理解・連携行動：CS平均
                                    if (ColName == "役割理解・連携行動：CS平均")
                                    {
                                        csvData.役割理解連携行動CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有CS平均			CSV_COL名：	情報共有：CS平均
                                    if (ColName == "情報共有：CS平均")
                                    {
                                        csvData.情報共有CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援CS平均			CSV_COL名：	相互支援：CS平均
                                    if (ColName == "相互支援：CS平均")
                                    {
                                        csvData.相互支援CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけCS平均			CSV_COL名：	相談・指導・他者の動機づけ：CS平均
                                    if (ColName == "相談・指導・他者の動機づけ：CS平均")
                                    {
                                        csvData.相談指導他者の動機づけCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうCS平均			CSV_COL名：	話しあう：CS平均
                                    if (ColName == "話しあう：CS平均")
                                    {
                                        csvData.話しあうCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するCS平均			CSV_COL名：	意見を主張する：CS平均
                                    if (ColName == "意見を主張する：CS平均")
                                    {
                                        csvData.意見を主張するCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議CS平均			CSV_COL名：	建設的・創造的な討議：CS平均
                                    if (ColName == "建設的・創造的な討議：CS平均")
                                    {
                                        csvData.建設的創造的な討議CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得CS平均			CSV_COL名：	意見の調整、交渉、説得：CS平均
                                    if (ColName == "意見の調整、交渉、説得：CS平均")
                                    {
                                        csvData.意見の調整交渉説得CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスCS平均			CSV_COL名：	セルフアウェアネス：CS平均
                                    if (ColName == "セルフアウェアネス：CS平均")
                                    {
                                        csvData.セルフアウェアネスCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングCS平均			CSV_COL名：	ストレスコーピング：CS平均
                                    if (ColName == "ストレスコーピング：CS平均")
                                    {
                                        csvData.ストレスコーピングCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントCS平均			CSV_COL名：	ストレスマネジメント：CS平均
                                    if (ColName == "ストレスマネジメント：CS平均")
                                    {
                                        csvData.ストレスマネジメントCS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解CS平均			CSV_COL名：	独自性理解：CS平均
                                    if (ColName == "独自性理解：CS平均")
                                    {
                                        csvData.独自性理解CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性CS平均			CSV_COL名：	自己効力感／楽観性：CS平均
                                    if (ColName == "自己効力感／楽観性：CS平均")
                                    {
                                        csvData.自己効力感楽観性CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革CS平均			CSV_COL名：	学習視点・機会による自己変革：CS平均
                                    if (ColName == "学習視点・機会による自己変革：CS平均")
                                    {
                                        csvData.学習視点機会による自己変革CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動CS平均			CSV_COL名：	主体的行動：CS平均
                                    if (ColName == "主体的行動：CS平均")
                                    {
                                        csvData.主体的行動CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂CS平均			CSV_COL名：	完遂：CS平均
                                    if (ColName == "完遂：CS平均")
                                    {
                                        csvData.完遂CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化CS平均			CSV_COL名：	良い行動の習慣化：CS平均
                                    if (ColName == "良い行動の習慣化：CS平均")
                                    {
                                        csvData.良い行動の習慣化CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集CS平均			CSV_COL名：	情報収集：CS平均
                                    if (ColName == "情報収集：CS平均")
                                    {
                                        csvData.情報収集CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解CS平均			CSV_COL名：	本質理解：CS平均
                                    if (ColName == "本質理解：CS平均")
                                    {
                                        csvData.本質理解CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究CS平均			CSV_COL名：	原因追究：CS平均
                                    if (ColName == "原因追究：CS平均")
                                    {
                                        csvData.原因追究CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定CS平均			CSV_COL名：	目標設定：CS平均
                                    if (ColName == "目標設定：CS平均")
                                    {
                                        csvData.目標設定CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築CS平均			CSV_COL名：	シナリオ構築：CS平均
                                    if (ColName == "シナリオ構築：CS平均")
                                    {
                                        csvData.シナリオ構築CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価CS平均			CSV_COL名：	計画評価：CS平均
                                    if (ColName == "計画評価：CS平均")
                                    {
                                        csvData.計画評価CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析CS平均			CSV_COL名：	リスク分析：CS平均
                                    if (ColName == "リスク分析：CS平均")
                                    {
                                        csvData.リスク分析CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動CS平均			CSV_COL名：	実践行動：CS平均
                                    if (ColName == "実践行動：CS平均")
                                    {
                                        csvData.実践行動CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整CS平均			CSV_COL名：	修正／調整：CS平均
                                    if (ColName == "修正／調整：CS平均")
                                    {
                                        csvData.修正調整CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善CS平均			CSV_COL名：	検証／改善：CS平均
                                    if (ColName == "検証／改善：CS平均")
                                    {
                                        csvData.検証改善CS平均 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさCS帯1			CSV_COL名：	親しみやすさ：CS帯1
                                    if (ColName == "親しみやすさ：CS帯1")
                                    {
                                        csvData.親しみやすさCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさCS帯2			CSV_COL名：	親しみやすさ：CS帯2
                                    if (ColName == "親しみやすさ：CS帯2")
                                    {
                                        csvData.親しみやすさCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさCS帯3			CSV_COL名：	親しみやすさ：CS帯3
                                    if (ColName == "親しみやすさ：CS帯3")
                                    {
                                        csvData.親しみやすさCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさCS帯4			CSV_COL名：	親しみやすさ：CS帯4
                                    if (ColName == "親しみやすさ：CS帯4")
                                    {
                                        csvData.親しみやすさCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	親しみやすさCS帯5			CSV_COL名：	親しみやすさ：CS帯5
                                    if (ColName == "親しみやすさ：CS帯5")
                                    {
                                        csvData.親しみやすさCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りCS帯1			CSV_COL名：	気配り：CS帯1
                                    if (ColName == "気配り：CS帯1")
                                    {
                                        csvData.気配りCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りCS帯2			CSV_COL名：	気配り：CS帯2
                                    if (ColName == "気配り：CS帯2")
                                    {
                                        csvData.気配りCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りCS帯3			CSV_COL名：	気配り：CS帯3
                                    if (ColName == "気配り：CS帯3")
                                    {
                                        csvData.気配りCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りCS帯4			CSV_COL名：	気配り：CS帯4
                                    if (ColName == "気配り：CS帯4")
                                    {
                                        csvData.気配りCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	気配りCS帯5			CSV_COL名：	気配り：CS帯5
                                    if (ColName == "気配り：CS帯5")
                                    {
                                        csvData.気配りCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容CS帯1			CSV_COL名：	対人興味／共感・受容：CS帯1
                                    if (ColName == "対人興味／共感・受容：CS帯1")
                                    {
                                        csvData.対人興味共感受容CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容CS帯2			CSV_COL名：	対人興味／共感・受容：CS帯2
                                    if (ColName == "対人興味／共感・受容：CS帯2")
                                    {
                                        csvData.対人興味共感受容CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容CS帯3			CSV_COL名：	対人興味／共感・受容：CS帯3
                                    if (ColName == "対人興味／共感・受容：CS帯3")
                                    {
                                        csvData.対人興味共感受容CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容CS帯4			CSV_COL名：	対人興味／共感・受容：CS帯4
                                    if (ColName == "対人興味／共感・受容：CS帯4")
                                    {
                                        csvData.対人興味共感受容CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	対人興味共感受容CS帯5			CSV_COL名：	対人興味／共感・受容：CS帯5
                                    if (ColName == "対人興味／共感・受容：CS帯5")
                                    {
                                        csvData.対人興味共感受容CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解CS帯1			CSV_COL名：	多様性理解：CS帯1
                                    if (ColName == "多様性理解：CS帯1")
                                    {
                                        csvData.多様性理解CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解CS帯2			CSV_COL名：	多様性理解：CS帯2
                                    if (ColName == "多様性理解：CS帯2")
                                    {
                                        csvData.多様性理解CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解CS帯3			CSV_COL名：	多様性理解：CS帯3
                                    if (ColName == "多様性理解：CS帯3")
                                    {
                                        csvData.多様性理解CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解CS帯4			CSV_COL名：	多様性理解：CS帯4
                                    if (ColName == "多様性理解：CS帯4")
                                    {
                                        csvData.多様性理解CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	多様性理解CS帯5			CSV_COL名：	多様性理解：CS帯5
                                    if (ColName == "多様性理解：CS帯5")
                                    {
                                        csvData.多様性理解CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成CS帯1			CSV_COL名：	人脈形成：CS帯1
                                    if (ColName == "人脈形成：CS帯1")
                                    {
                                        csvData.人脈形成CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成CS帯2			CSV_COL名：	人脈形成：CS帯2
                                    if (ColName == "人脈形成：CS帯2")
                                    {
                                        csvData.人脈形成CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成CS帯3			CSV_COL名：	人脈形成：CS帯3
                                    if (ColName == "人脈形成：CS帯3")
                                    {
                                        csvData.人脈形成CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成CS帯4			CSV_COL名：	人脈形成：CS帯4
                                    if (ColName == "人脈形成：CS帯4")
                                    {
                                        csvData.人脈形成CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	人脈形成CS帯5			CSV_COL名：	人脈形成：CS帯5
                                    if (ColName == "人脈形成：CS帯5")
                                    {
                                        csvData.人脈形成CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築CS帯1			CSV_COL名：	信頼構築：CS帯1
                                    if (ColName == "信頼構築：CS帯1")
                                    {
                                        csvData.信頼構築CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築CS帯2			CSV_COL名：	信頼構築：CS帯2
                                    if (ColName == "信頼構築：CS帯2")
                                    {
                                        csvData.信頼構築CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築CS帯3			CSV_COL名：	信頼構築：CS帯3
                                    if (ColName == "信頼構築：CS帯3")
                                    {
                                        csvData.信頼構築CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築CS帯4			CSV_COL名：	信頼構築：CS帯4
                                    if (ColName == "信頼構築：CS帯4")
                                    {
                                        csvData.信頼構築CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	信頼構築CS帯5			CSV_COL名：	信頼構築：CS帯5
                                    if (ColName == "信頼構築：CS帯5")
                                    {
                                        csvData.信頼構築CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動CS帯1			CSV_COL名：	役割理解・連携行動：CS帯1
                                    if (ColName == "役割理解・連携行動：CS帯1")
                                    {
                                        csvData.役割理解連携行動CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動CS帯2			CSV_COL名：	役割理解・連携行動：CS帯2
                                    if (ColName == "役割理解・連携行動：CS帯2")
                                    {
                                        csvData.役割理解連携行動CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動CS帯3			CSV_COL名：	役割理解・連携行動：CS帯3
                                    if (ColName == "役割理解・連携行動：CS帯3")
                                    {
                                        csvData.役割理解連携行動CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動CS帯4			CSV_COL名：	役割理解・連携行動：CS帯4
                                    if (ColName == "役割理解・連携行動：CS帯4")
                                    {
                                        csvData.役割理解連携行動CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	役割理解連携行動CS帯5			CSV_COL名：	役割理解・連携行動：CS帯5
                                    if (ColName == "役割理解・連携行動：CS帯5")
                                    {
                                        csvData.役割理解連携行動CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有CS帯1			CSV_COL名：	情報共有：CS帯1
                                    if (ColName == "情報共有：CS帯1")
                                    {
                                        csvData.情報共有CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有CS帯2			CSV_COL名：	情報共有：CS帯2
                                    if (ColName == "情報共有：CS帯2")
                                    {
                                        csvData.情報共有CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有CS帯3			CSV_COL名：	情報共有：CS帯3
                                    if (ColName == "情報共有：CS帯3")
                                    {
                                        csvData.情報共有CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有CS帯4			CSV_COL名：	情報共有：CS帯4
                                    if (ColName == "情報共有：CS帯4")
                                    {
                                        csvData.情報共有CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報共有CS帯5			CSV_COL名：	情報共有：CS帯5
                                    if (ColName == "情報共有：CS帯5")
                                    {
                                        csvData.情報共有CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援CS帯1			CSV_COL名：	相互支援：CS帯1
                                    if (ColName == "相互支援：CS帯1")
                                    {
                                        csvData.相互支援CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援CS帯2			CSV_COL名：	相互支援：CS帯2
                                    if (ColName == "相互支援：CS帯2")
                                    {
                                        csvData.相互支援CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援CS帯3			CSV_COL名：	相互支援：CS帯3
                                    if (ColName == "相互支援：CS帯3")
                                    {
                                        csvData.相互支援CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援CS帯4			CSV_COL名：	相互支援：CS帯4
                                    if (ColName == "相互支援：CS帯4")
                                    {
                                        csvData.相互支援CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相互支援CS帯5			CSV_COL名：	相互支援：CS帯5
                                    if (ColName == "相互支援：CS帯5")
                                    {
                                        csvData.相互支援CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけCS帯1			CSV_COL名：	相談・指導・他者の動機づけ：CS帯1
                                    if (ColName == "相談・指導・他者の動機づけ：CS帯1")
                                    {
                                        csvData.相談指導他者の動機づけCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけCS帯2			CSV_COL名：	相談・指導・他者の動機づけ：CS帯2
                                    if (ColName == "相談・指導・他者の動機づけ：CS帯2")
                                    {
                                        csvData.相談指導他者の動機づけCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけCS帯3			CSV_COL名：	相談・指導・他者の動機づけ：CS帯3
                                    if (ColName == "相談・指導・他者の動機づけ：CS帯3")
                                    {
                                        csvData.相談指導他者の動機づけCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけCS帯4			CSV_COL名：	相談・指導・他者の動機づけ：CS帯4
                                    if (ColName == "相談・指導・他者の動機づけ：CS帯4")
                                    {
                                        csvData.相談指導他者の動機づけCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	相談指導他者の動機づけCS帯5			CSV_COL名：	相談・指導・他者の動機づけ：CS帯5
                                    if (ColName == "相談・指導・他者の動機づけ：CS帯5")
                                    {
                                        csvData.相談指導他者の動機づけCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうCS帯1			CSV_COL名：	話しあう：CS帯1
                                    if (ColName == "話しあう：CS帯1")
                                    {
                                        csvData.話しあうCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうCS帯2			CSV_COL名：	話しあう：CS帯2
                                    if (ColName == "話しあう：CS帯2")
                                    {
                                        csvData.話しあうCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうCS帯3			CSV_COL名：	話しあう：CS帯3
                                    if (ColName == "話しあう：CS帯3")
                                    {
                                        csvData.話しあうCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうCS帯4			CSV_COL名：	話しあう：CS帯4
                                    if (ColName == "話しあう：CS帯4")
                                    {
                                        csvData.話しあうCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	話しあうCS帯5			CSV_COL名：	話しあう：CS帯5
                                    if (ColName == "話しあう：CS帯5")
                                    {
                                        csvData.話しあうCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するCS帯1			CSV_COL名：	意見を主張する：CS帯1
                                    if (ColName == "意見を主張する：CS帯1")
                                    {
                                        csvData.意見を主張するCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するCS帯2			CSV_COL名：	意見を主張する：CS帯2
                                    if (ColName == "意見を主張する：CS帯2")
                                    {
                                        csvData.意見を主張するCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するCS帯3			CSV_COL名：	意見を主張する：CS帯3
                                    if (ColName == "意見を主張する：CS帯3")
                                    {
                                        csvData.意見を主張するCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するCS帯4			CSV_COL名：	意見を主張する：CS帯4
                                    if (ColName == "意見を主張する：CS帯4")
                                    {
                                        csvData.意見を主張するCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見を主張するCS帯5			CSV_COL名：	意見を主張する：CS帯5
                                    if (ColName == "意見を主張する：CS帯5")
                                    {
                                        csvData.意見を主張するCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議CS帯1			CSV_COL名：	建設的・創造的な討議：CS帯1
                                    if (ColName == "建設的・創造的な討議：CS帯1")
                                    {
                                        csvData.建設的創造的な討議CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議CS帯2			CSV_COL名：	建設的・創造的な討議：CS帯2
                                    if (ColName == "建設的・創造的な討議：CS帯2")
                                    {
                                        csvData.建設的創造的な討議CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議CS帯3			CSV_COL名：	建設的・創造的な討議：CS帯3
                                    if (ColName == "建設的・創造的な討議：CS帯3")
                                    {
                                        csvData.建設的創造的な討議CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議CS帯4			CSV_COL名：	建設的・創造的な討議：CS帯4
                                    if (ColName == "建設的・創造的な討議：CS帯4")
                                    {
                                        csvData.建設的創造的な討議CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	建設的創造的な討議CS帯5			CSV_COL名：	建設的・創造的な討議：CS帯5
                                    if (ColName == "建設的・創造的な討議：CS帯5")
                                    {
                                        csvData.建設的創造的な討議CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得CS帯1			CSV_COL名：	意見の調整、交渉、説得：CS帯1
                                    if (ColName == "意見の調整、交渉、説得：CS帯1")
                                    {
                                        csvData.意見の調整交渉説得CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得CS帯2			CSV_COL名：	意見の調整、交渉、説得：CS帯2
                                    if (ColName == "意見の調整、交渉、説得：CS帯2")
                                    {
                                        csvData.意見の調整交渉説得CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得CS帯3			CSV_COL名：	意見の調整、交渉、説得：CS帯3
                                    if (ColName == "意見の調整、交渉、説得：CS帯3")
                                    {
                                        csvData.意見の調整交渉説得CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得CS帯4			CSV_COL名：	意見の調整、交渉、説得：CS帯4
                                    if (ColName == "意見の調整、交渉、説得：CS帯4")
                                    {
                                        csvData.意見の調整交渉説得CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	意見の調整交渉説得CS帯5			CSV_COL名：	意見の調整、交渉、説得：CS帯5
                                    if (ColName == "意見の調整、交渉、説得：CS帯5")
                                    {
                                        csvData.意見の調整交渉説得CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスCS帯1			CSV_COL名：	セルフアウェアネス：CS帯1
                                    if (ColName == "セルフアウェアネス：CS帯1")
                                    {
                                        csvData.セルフアウェアネスCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスCS帯2			CSV_COL名：	セルフアウェアネス：CS帯2
                                    if (ColName == "セルフアウェアネス：CS帯2")
                                    {
                                        csvData.セルフアウェアネスCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスCS帯3			CSV_COL名：	セルフアウェアネス：CS帯3
                                    if (ColName == "セルフアウェアネス：CS帯3")
                                    {
                                        csvData.セルフアウェアネスCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスCS帯4			CSV_COL名：	セルフアウェアネス：CS帯4
                                    if (ColName == "セルフアウェアネス：CS帯4")
                                    {
                                        csvData.セルフアウェアネスCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	セルフアウェアネスCS帯5			CSV_COL名：	セルフアウェアネス：CS帯5
                                    if (ColName == "セルフアウェアネス：CS帯5")
                                    {
                                        csvData.セルフアウェアネスCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングCS帯1			CSV_COL名：	ストレスコーピング：CS帯1
                                    if (ColName == "ストレスコーピング：CS帯1")
                                    {
                                        csvData.ストレスコーピングCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングCS帯2			CSV_COL名：	ストレスコーピング：CS帯2
                                    if (ColName == "ストレスコーピング：CS帯2")
                                    {
                                        csvData.ストレスコーピングCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングCS帯3			CSV_COL名：	ストレスコーピング：CS帯3
                                    if (ColName == "ストレスコーピング：CS帯3")
                                    {
                                        csvData.ストレスコーピングCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングCS帯4			CSV_COL名：	ストレスコーピング：CS帯4
                                    if (ColName == "ストレスコーピング：CS帯4")
                                    {
                                        csvData.ストレスコーピングCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスコーピングCS帯5			CSV_COL名：	ストレスコーピング：CS帯5
                                    if (ColName == "ストレスコーピング：CS帯5")
                                    {
                                        csvData.ストレスコーピングCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントCS帯1			CSV_COL名：	ストレスマネジメント：CS帯1
                                    if (ColName == "ストレスマネジメント：CS帯1")
                                    {
                                        csvData.ストレスマネジメントCS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントCS帯2			CSV_COL名：	ストレスマネジメント：CS帯2
                                    if (ColName == "ストレスマネジメント：CS帯2")
                                    {
                                        csvData.ストレスマネジメントCS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントCS帯3			CSV_COL名：	ストレスマネジメント：CS帯3
                                    if (ColName == "ストレスマネジメント：CS帯3")
                                    {
                                        csvData.ストレスマネジメントCS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントCS帯4			CSV_COL名：	ストレスマネジメント：CS帯4
                                    if (ColName == "ストレスマネジメント：CS帯4")
                                    {
                                        csvData.ストレスマネジメントCS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	ストレスマネジメントCS帯5			CSV_COL名：	ストレスマネジメント：CS帯5
                                    if (ColName == "ストレスマネジメント：CS帯5")
                                    {
                                        csvData.ストレスマネジメントCS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解CS帯1			CSV_COL名：	独自性理解：CS帯1
                                    if (ColName == "独自性理解：CS帯1")
                                    {
                                        csvData.独自性理解CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解CS帯2			CSV_COL名：	独自性理解：CS帯2
                                    if (ColName == "独自性理解：CS帯2")
                                    {
                                        csvData.独自性理解CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解CS帯3			CSV_COL名：	独自性理解：CS帯3
                                    if (ColName == "独自性理解：CS帯3")
                                    {
                                        csvData.独自性理解CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解CS帯4			CSV_COL名：	独自性理解：CS帯4
                                    if (ColName == "独自性理解：CS帯4")
                                    {
                                        csvData.独自性理解CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	独自性理解CS帯5			CSV_COL名：	独自性理解：CS帯5
                                    if (ColName == "独自性理解：CS帯5")
                                    {
                                        csvData.独自性理解CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性CS帯1			CSV_COL名：	自己効力感／楽観性：CS帯1
                                    if (ColName == "自己効力感／楽観性：CS帯1")
                                    {
                                        csvData.自己効力感楽観性CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性CS帯2			CSV_COL名：	自己効力感／楽観性：CS帯2
                                    if (ColName == "自己効力感／楽観性：CS帯2")
                                    {
                                        csvData.自己効力感楽観性CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性CS帯3			CSV_COL名：	自己効力感／楽観性：CS帯3
                                    if (ColName == "自己効力感／楽観性：CS帯3")
                                    {
                                        csvData.自己効力感楽観性CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性CS帯4			CSV_COL名：	自己効力感／楽観性：CS帯4
                                    if (ColName == "自己効力感／楽観性：CS帯4")
                                    {
                                        csvData.自己効力感楽観性CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	自己効力感楽観性CS帯5			CSV_COL名：	自己効力感／楽観性：CS帯5
                                    if (ColName == "自己効力感／楽観性：CS帯5")
                                    {
                                        csvData.自己効力感楽観性CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革CS帯1			CSV_COL名：	学習視点・機会による自己変革：CS帯1
                                    if (ColName == "学習視点・機会による自己変革：CS帯1")
                                    {
                                        csvData.学習視点機会による自己変革CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革CS帯2			CSV_COL名：	学習視点・機会による自己変革：CS帯2
                                    if (ColName == "学習視点・機会による自己変革：CS帯2")
                                    {
                                        csvData.学習視点機会による自己変革CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革CS帯3			CSV_COL名：	学習視点・機会による自己変革：CS帯3
                                    if (ColName == "学習視点・機会による自己変革：CS帯3")
                                    {
                                        csvData.学習視点機会による自己変革CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革CS帯4			CSV_COL名：	学習視点・機会による自己変革：CS帯4
                                    if (ColName == "学習視点・機会による自己変革：CS帯4")
                                    {
                                        csvData.学習視点機会による自己変革CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	学習視点機会による自己変革CS帯5			CSV_COL名：	学習視点・機会による自己変革：CS帯5
                                    if (ColName == "学習視点・機会による自己変革：CS帯5")
                                    {
                                        csvData.学習視点機会による自己変革CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動CS帯1			CSV_COL名：	主体的行動：CS帯1
                                    if (ColName == "主体的行動：CS帯1")
                                    {
                                        csvData.主体的行動CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動CS帯2			CSV_COL名：	主体的行動：CS帯2
                                    if (ColName == "主体的行動：CS帯2")
                                    {
                                        csvData.主体的行動CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動CS帯3			CSV_COL名：	主体的行動：CS帯3
                                    if (ColName == "主体的行動：CS帯3")
                                    {
                                        csvData.主体的行動CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動CS帯4			CSV_COL名：	主体的行動：CS帯4
                                    if (ColName == "主体的行動：CS帯4")
                                    {
                                        csvData.主体的行動CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	主体的行動CS帯5			CSV_COL名：	主体的行動：CS帯5
                                    if (ColName == "主体的行動：CS帯5")
                                    {
                                        csvData.主体的行動CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂CS帯1			CSV_COL名：	完遂：CS帯1
                                    if (ColName == "完遂：CS帯1")
                                    {
                                        csvData.完遂CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂CS帯2			CSV_COL名：	完遂：CS帯2
                                    if (ColName == "完遂：CS帯2")
                                    {
                                        csvData.完遂CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂CS帯3			CSV_COL名：	完遂：CS帯3
                                    if (ColName == "完遂：CS帯3")
                                    {
                                        csvData.完遂CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂CS帯4			CSV_COL名：	完遂：CS帯4
                                    if (ColName == "完遂：CS帯4")
                                    {
                                        csvData.完遂CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	完遂CS帯5			CSV_COL名：	完遂：CS帯5
                                    if (ColName == "完遂：CS帯5")
                                    {
                                        csvData.完遂CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化CS帯1			CSV_COL名：	良い行動の習慣化：CS帯1
                                    if (ColName == "良い行動の習慣化：CS帯1")
                                    {
                                        csvData.良い行動の習慣化CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化CS帯2			CSV_COL名：	良い行動の習慣化：CS帯2
                                    if (ColName == "良い行動の習慣化：CS帯2")
                                    {
                                        csvData.良い行動の習慣化CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化CS帯3			CSV_COL名：	良い行動の習慣化：CS帯3
                                    if (ColName == "良い行動の習慣化：CS帯3")
                                    {
                                        csvData.良い行動の習慣化CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化CS帯4			CSV_COL名：	良い行動の習慣化：CS帯4
                                    if (ColName == "良い行動の習慣化：CS帯4")
                                    {
                                        csvData.良い行動の習慣化CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	良い行動の習慣化CS帯5			CSV_COL名：	良い行動の習慣化：CS帯5
                                    if (ColName == "良い行動の習慣化：CS帯5")
                                    {
                                        csvData.良い行動の習慣化CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集CS帯1			CSV_COL名：	情報収集：CS帯1
                                    if (ColName == "情報収集：CS帯1")
                                    {
                                        csvData.情報収集CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集CS帯2			CSV_COL名：	情報収集：CS帯2
                                    if (ColName == "情報収集：CS帯2")
                                    {
                                        csvData.情報収集CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集CS帯3			CSV_COL名：	情報収集：CS帯3
                                    if (ColName == "情報収集：CS帯3")
                                    {
                                        csvData.情報収集CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集CS帯4			CSV_COL名：	情報収集：CS帯4
                                    if (ColName == "情報収集：CS帯4")
                                    {
                                        csvData.情報収集CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	情報収集CS帯5			CSV_COL名：	情報収集：CS帯5
                                    if (ColName == "情報収集：CS帯5")
                                    {
                                        csvData.情報収集CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解CS帯1			CSV_COL名：	本質理解：CS帯1
                                    if (ColName == "本質理解：CS帯1")
                                    {
                                        csvData.本質理解CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解CS帯2			CSV_COL名：	本質理解：CS帯2
                                    if (ColName == "本質理解：CS帯2")
                                    {
                                        csvData.本質理解CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解CS帯3			CSV_COL名：	本質理解：CS帯3
                                    if (ColName == "本質理解：CS帯3")
                                    {
                                        csvData.本質理解CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解CS帯4			CSV_COL名：	本質理解：CS帯4
                                    if (ColName == "本質理解：CS帯4")
                                    {
                                        csvData.本質理解CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	本質理解CS帯5			CSV_COL名：	本質理解：CS帯5
                                    if (ColName == "本質理解：CS帯5")
                                    {
                                        csvData.本質理解CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究CS帯1			CSV_COL名：	原因追究：CS帯1
                                    if (ColName == "原因追究：CS帯1")
                                    {
                                        csvData.原因追究CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究CS帯2			CSV_COL名：	原因追究：CS帯2
                                    if (ColName == "原因追究：CS帯2")
                                    {
                                        csvData.原因追究CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究CS帯3			CSV_COL名：	原因追究：CS帯3
                                    if (ColName == "原因追究：CS帯3")
                                    {
                                        csvData.原因追究CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究CS帯4			CSV_COL名：	原因追究：CS帯4
                                    if (ColName == "原因追究：CS帯4")
                                    {
                                        csvData.原因追究CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	原因追究CS帯5			CSV_COL名：	原因追究：CS帯5
                                    if (ColName == "原因追究：CS帯5")
                                    {
                                        csvData.原因追究CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定CS帯1			CSV_COL名：	目標設定：CS帯1
                                    if (ColName == "目標設定：CS帯1")
                                    {
                                        csvData.目標設定CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定CS帯2			CSV_COL名：	目標設定：CS帯2
                                    if (ColName == "目標設定：CS帯2")
                                    {
                                        csvData.目標設定CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定CS帯3			CSV_COL名：	目標設定：CS帯3
                                    if (ColName == "目標設定：CS帯3")
                                    {
                                        csvData.目標設定CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定CS帯4			CSV_COL名：	目標設定：CS帯4
                                    if (ColName == "目標設定：CS帯4")
                                    {
                                        csvData.目標設定CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	目標設定CS帯5			CSV_COL名：	目標設定：CS帯5
                                    if (ColName == "目標設定：CS帯5")
                                    {
                                        csvData.目標設定CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築CS帯1			CSV_COL名：	シナリオ構築：CS帯1
                                    if (ColName == "シナリオ構築：CS帯1")
                                    {
                                        csvData.シナリオ構築CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築CS帯2			CSV_COL名：	シナリオ構築：CS帯2
                                    if (ColName == "シナリオ構築：CS帯2")
                                    {
                                        csvData.シナリオ構築CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築CS帯3			CSV_COL名：	シナリオ構築：CS帯3
                                    if (ColName == "シナリオ構築：CS帯3")
                                    {
                                        csvData.シナリオ構築CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築CS帯4			CSV_COL名：	シナリオ構築：CS帯4
                                    if (ColName == "シナリオ構築：CS帯4")
                                    {
                                        csvData.シナリオ構築CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	シナリオ構築CS帯5			CSV_COL名：	シナリオ構築：CS帯5
                                    if (ColName == "シナリオ構築：CS帯5")
                                    {
                                        csvData.シナリオ構築CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価CS帯1			CSV_COL名：	計画評価：CS帯1
                                    if (ColName == "計画評価：CS帯1")
                                    {
                                        csvData.計画評価CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価CS帯2			CSV_COL名：	計画評価：CS帯2
                                    if (ColName == "計画評価：CS帯2")
                                    {
                                        csvData.計画評価CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価CS帯3			CSV_COL名：	計画評価：CS帯3
                                    if (ColName == "計画評価：CS帯3")
                                    {
                                        csvData.計画評価CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価CS帯4			CSV_COL名：	計画評価：CS帯4
                                    if (ColName == "計画評価：CS帯4")
                                    {
                                        csvData.計画評価CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	計画評価CS帯5			CSV_COL名：	計画評価：CS帯5
                                    if (ColName == "計画評価：CS帯5")
                                    {
                                        csvData.計画評価CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析CS帯1			CSV_COL名：	リスク分析：CS帯1
                                    if (ColName == "リスク分析：CS帯1")
                                    {
                                        csvData.リスク分析CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析CS帯2			CSV_COL名：	リスク分析：CS帯2
                                    if (ColName == "リスク分析：CS帯2")
                                    {
                                        csvData.リスク分析CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析CS帯3			CSV_COL名：	リスク分析：CS帯3
                                    if (ColName == "リスク分析：CS帯3")
                                    {
                                        csvData.リスク分析CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析CS帯4			CSV_COL名：	リスク分析：CS帯4
                                    if (ColName == "リスク分析：CS帯4")
                                    {
                                        csvData.リスク分析CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	リスク分析CS帯5			CSV_COL名：	リスク分析：CS帯5
                                    if (ColName == "リスク分析：CS帯5")
                                    {
                                        csvData.リスク分析CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動CS帯1			CSV_COL名：	実践行動：CS帯1
                                    if (ColName == "実践行動：CS帯1")
                                    {
                                        csvData.実践行動CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動CS帯2			CSV_COL名：	実践行動：CS帯2
                                    if (ColName == "実践行動：CS帯2")
                                    {
                                        csvData.実践行動CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動CS帯3			CSV_COL名：	実践行動：CS帯3
                                    if (ColName == "実践行動：CS帯3")
                                    {
                                        csvData.実践行動CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動CS帯4			CSV_COL名：	実践行動：CS帯4
                                    if (ColName == "実践行動：CS帯4")
                                    {
                                        csvData.実践行動CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	実践行動CS帯5			CSV_COL名：	実践行動：CS帯5
                                    if (ColName == "実践行動：CS帯5")
                                    {
                                        csvData.実践行動CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整CS帯1			CSV_COL名：	修正／調整：CS帯1
                                    if (ColName == "修正／調整：CS帯1")
                                    {
                                        csvData.修正調整CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整CS帯2			CSV_COL名：	修正／調整：CS帯2
                                    if (ColName == "修正／調整：CS帯2")
                                    {
                                        csvData.修正調整CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整CS帯3			CSV_COL名：	修正／調整：CS帯3
                                    if (ColName == "修正／調整：CS帯3")
                                    {
                                        csvData.修正調整CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整CS帯4			CSV_COL名：	修正／調整：CS帯4
                                    if (ColName == "修正／調整：CS帯4")
                                    {
                                        csvData.修正調整CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	修正調整CS帯5			CSV_COL名：	修正／調整：CS帯5
                                    if (ColName == "修正／調整：CS帯5")
                                    {
                                        csvData.修正調整CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善CS帯1			CSV_COL名：	検証／改善：CS帯1
                                    if (ColName == "検証／改善：CS帯1")
                                    {
                                        csvData.検証改善CS帯1 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善CS帯2			CSV_COL名：	検証／改善：CS帯2
                                    if (ColName == "検証／改善：CS帯2")
                                    {
                                        csvData.検証改善CS帯2 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善CS帯3			CSV_COL名：	検証／改善：CS帯3
                                    if (ColName == "検証／改善：CS帯3")
                                    {
                                        csvData.検証改善CS帯3 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善CS帯4			CSV_COL名：	検証／改善：CS帯4
                                    if (ColName == "検証／改善：CS帯4")
                                    {
                                        csvData.検証改善CS帯4 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	検証改善CS帯5			CSV_COL名：	検証／改善：CS帯5
                                    if (ColName == "検証／改善：CS帯5")
                                    {
                                        csvData.検証改善CS帯5 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回受験者ID			CSV_COL名：	前回受験者ID
                                    if (ColName == "前回受験者ID")
                                    {
                                        csvData.前回受験者ID = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回採点日			CSV_COL名：	[前回]採点日
                                    if (ColName == "[前回]採点日")
                                    {
                                        csvData.前回採点日 = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回リテラシー総合レベル			CSV_COL名：	[前回]リテラシー総合：レベル
                                    if (ColName == "[前回]リテラシー総合：レベル")
                                    {
                                        csvData.前回リテラシー総合レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回リテラシー情報収集力レベル			CSV_COL名：	[前回]リテラシー情報収集力：レベル
                                    if (ColName == "[前回]リテラシー情報収集力：レベル")
                                    {
                                        csvData.前回リテラシー情報収集力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回リテラシー情報分析力レベル			CSV_COL名：	[前回]リテラシー情報分析力：レベル
                                    if (ColName == "[前回]リテラシー情報分析力：レベル")
                                    {
                                        csvData.前回リテラシー情報分析力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回リテラシー課題発見力レベル			CSV_COL名：	[前回]リテラシー課題発見力：レベル
                                    if (ColName == "[前回]リテラシー課題発見力：レベル")
                                    {
                                        csvData.前回リテラシー課題発見力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回リテラシー構想力レベル			CSV_COL名：	[前回]リテラシー構想力：レベル
                                    if (ColName == "[前回]リテラシー構想力：レベル")
                                    {
                                        csvData.前回リテラシー構想力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回コンピテンシー総合レベル			CSV_COL名：	[前回]コンピテンシー総合：レベル
                                    if (ColName == "[前回]コンピテンシー総合：レベル")
                                    {
                                        csvData.前回コンピテンシー総合レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回対人基礎力レベル			CSV_COL名：	[前回]対人基礎力：レベル
                                    if (ColName == "[前回]対人基礎力：レベル")
                                    {
                                        csvData.前回対人基礎力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回対自己基礎力レベル			CSV_COL名：	[前回]対自己基礎力：レベル
                                    if (ColName == "[前回]対自己基礎力：レベル")
                                    {
                                        csvData.前回対自己基礎力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回対課題基礎力レベル			CSV_COL名：	[前回]対課題基礎力：レベル
                                    if (ColName == "[前回]対課題基礎力：レベル")
                                    {
                                        csvData.前回対課題基礎力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回親和力レベル			CSV_COL名：	[前回]親和力：レベル
                                    if (ColName == "[前回]親和力：レベル")
                                    {
                                        csvData.前回親和力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回協働力レベル			CSV_COL名：	[前回]協働力：レベル
                                    if (ColName == "[前回]協働力：レベル")
                                    {
                                        csvData.前回協働力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回統率力レベル			CSV_COL名：	[前回]統率力：レベル
                                    if (ColName == "[前回]統率力：レベル")
                                    {
                                        csvData.前回統率力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回感情制御力レベル			CSV_COL名：	[前回]感情制御力：レベル
                                    if (ColName == "[前回]感情制御力：レベル")
                                    {
                                        csvData.前回感情制御力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回自信創出力レベル			CSV_COL名：	[前回]自信創出力：レベル
                                    if (ColName == "[前回]自信創出力：レベル")
                                    {
                                        csvData.前回自信創出力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回行動持続力レベル			CSV_COL名：	[前回]行動持続力：レベル
                                    if (ColName == "[前回]行動持続力：レベル")
                                    {
                                        csvData.前回行動持続力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回課題発見力レベル			CSV_COL名：	[前回]課題発見力：レベル
                                    if (ColName == "[前回]課題発見力：レベル")
                                    {
                                        csvData.前回課題発見力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回計画立案力レベル			CSV_COL名：	[前回]計画立案力：レベル
                                    if (ColName == "[前回]計画立案力：レベル")
                                    {
                                        csvData.前回計画立案力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回実践力レベル			CSV_COL名：	[前回]実践力：レベル
                                    if (ColName == "[前回]実践力：レベル")
                                    {
                                        csvData.前回実践力レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回親しみやすさレベル			CSV_COL名：	[前回]親しみやすさ：レベル
                                    if (ColName == "[前回]親しみやすさ：レベル")
                                    {
                                        csvData.前回親しみやすさレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回気配りレベル			CSV_COL名：	[前回]気配り：レベル
                                    if (ColName == "[前回]気配り：レベル")
                                    {
                                        csvData.前回気配りレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回対人興味共感受容レベル			CSV_COL名：	[前回]対人興味／共感・受容：レベル
                                    if (ColName == "[前回]対人興味／共感・受容：レベル")
                                    {
                                        csvData.前回対人興味共感受容レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回多様性理解レベル			CSV_COL名：	[前回]多様性理解：レベル
                                    if (ColName == "[前回]多様性理解：レベル")
                                    {
                                        csvData.前回多様性理解レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回人脈形成レベル			CSV_COL名：	[前回]人脈形成：レベル
                                    if (ColName == "[前回]人脈形成：レベル")
                                    {
                                        csvData.前回人脈形成レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回信頼構築レベル			CSV_COL名：	[前回]信頼構築：レベル
                                    if (ColName == "[前回]信頼構築：レベル")
                                    {
                                        csvData.前回信頼構築レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回役割理解連携行動レベル			CSV_COL名：	[前回]役割理解・連携行動：レベル
                                    if (ColName == "[前回]役割理解・連携行動：レベル")
                                    {
                                        csvData.前回役割理解連携行動レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回情報共有レベル			CSV_COL名：	[前回]情報共有：レベル
                                    if (ColName == "[前回]情報共有：レベル")
                                    {
                                        csvData.前回情報共有レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回相互支援レベル			CSV_COL名：	[前回]相互支援：レベル
                                    if (ColName == "[前回]相互支援：レベル")
                                    {
                                        csvData.前回相互支援レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回相談指導他者の動機づけレベル			CSV_COL名：	[前回]相談・指導・他者の動機づけ：レベル
                                    if (ColName == "[前回]相談・指導・他者の動機づけ：レベル")
                                    {
                                        csvData.前回相談指導他者の動機づけレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回話しあうレベル			CSV_COL名：	[前回]話しあう：レベル
                                    if (ColName == "[前回]話しあう：レベル")
                                    {
                                        csvData.前回話しあうレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回意見を主張するレベル			CSV_COL名：	[前回]意見を主張する：レベル
                                    if (ColName == "[前回]意見を主張する：レベル")
                                    {
                                        csvData.前回意見を主張するレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回建設的創造的な討議レベル			CSV_COL名：	[前回]建設的・創造的な討議：レベル
                                    if (ColName == "[前回]建設的・創造的な討議：レベル")
                                    {
                                        csvData.前回建設的創造的な討議レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回意見の調整交渉説得レベル			CSV_COL名：	[前回]意見の調整、交渉、説得：レベル
                                    if (ColName == "[前回]意見の調整、交渉、説得：レベル")
                                    {
                                        csvData.前回意見の調整交渉説得レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回セルフアウェアネスレベル			CSV_COL名：	[前回]セルフアウェアネス：レベル
                                    if (ColName == "[前回]セルフアウェアネス：レベル")
                                    {
                                        csvData.前回セルフアウェアネスレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回ストレスコーピングレベル			CSV_COL名：	[前回]ストレスコーピング：レベル
                                    if (ColName == "[前回]ストレスコーピング：レベル")
                                    {
                                        csvData.前回ストレスコーピングレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回ストレスマネジメントレベル			CSV_COL名：	[前回]ストレスマネジメント：レベル
                                    if (ColName == "[前回]ストレスマネジメント：レベル")
                                    {
                                        csvData.前回ストレスマネジメントレベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回独自性理解レベル			CSV_COL名：	[前回]独自性理解：レベル
                                    if (ColName == "[前回]独自性理解：レベル")
                                    {
                                        csvData.前回独自性理解レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回自己効力感楽観性レベル			CSV_COL名：	[前回]自己効力感／楽観性：レベル
                                    if (ColName == "[前回]自己効力感／楽観性：レベル")
                                    {
                                        csvData.前回自己効力感楽観性レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回学習視点機会による自己変革レベル			CSV_COL名：	[前回]学習視点・機会による自己変革：レベル
                                    if (ColName == "[前回]学習視点・機会による自己変革：レベル")
                                    {
                                        csvData.前回学習視点機会による自己変革レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回主体的行動レベル			CSV_COL名：	[前回]主体的行動：レベル
                                    if (ColName == "[前回]主体的行動：レベル")
                                    {
                                        csvData.前回主体的行動レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回完遂レベル			CSV_COL名：	[前回]完遂：レベル
                                    if (ColName == "[前回]完遂：レベル")
                                    {
                                        csvData.前回完遂レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回良い行動の習慣化レベル			CSV_COL名：	[前回]良い行動の習慣化：レベル
                                    if (ColName == "[前回]良い行動の習慣化：レベル")
                                    {
                                        csvData.前回良い行動の習慣化レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回情報収集レベル			CSV_COL名：	[前回]情報収集：レベル
                                    if (ColName == "[前回]情報収集：レベル")
                                    {
                                        csvData.前回情報収集レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回本質理解レベル			CSV_COL名：	[前回]本質理解：レベル
                                    if (ColName == "[前回]本質理解：レベル")
                                    {
                                        csvData.前回本質理解レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回原因追究レベル			CSV_COL名：	[前回]原因追究：レベル
                                    if (ColName == "[前回]原因追究：レベル")
                                    {
                                        csvData.前回原因追究レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回目標設定レベル			CSV_COL名：	[前回]目標設定：レベル
                                    if (ColName == "[前回]目標設定：レベル")
                                    {
                                        csvData.前回目標設定レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回シナリオ構築レベル			CSV_COL名：	[前回]シナリオ構築：レベル
                                    if (ColName == "[前回]シナリオ構築：レベル")
                                    {
                                        csvData.前回シナリオ構築レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回計画評価レベル			CSV_COL名：	[前回]計画評価：レベル
                                    if (ColName == "[前回]計画評価：レベル")
                                    {
                                        csvData.前回計画評価レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回リスク分析レベル			CSV_COL名：	[前回]リスク分析：レベル
                                    if (ColName == "[前回]リスク分析：レベル")
                                    {
                                        csvData.前回リスク分析レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回実践行動レベル			CSV_COL名：	[前回]実践行動：レベル
                                    if (ColName == "[前回]実践行動：レベル")
                                    {
                                        csvData.前回実践行動レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回修正調整レベル			CSV_COL名：	[前回]修正／調整：レベル
                                    if (ColName == "[前回]修正／調整：レベル")
                                    {
                                        csvData.前回修正調整レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                    //	前回検証改善レベル			CSV_COL名：	[前回]検証／改善：レベル
                                    if (ColName == "[前回]検証／改善：レベル")
                                    {
                                        csvData.前回検証改善レベル = dr.Field<string>(dt.Columns.IndexOf(ColName));
                                        continue;
                                    }
                                }
                            }
                            csvDataList.Add(csvData);
                        }
                        if (threadName == "T1")
                            LogStringT1.AppendLine(f.Name + "ファイル：" + csvDataList.Count + "行のデータを読み込みました。" + timeYomi);
                        else
                            LogStringT2.AppendLine(f.Name + "ファイル：" + csvDataList.Count + "行のデータを読み込みました。" + timeYomi);

                        //List<datModel> csvDataList1 = csvDataList.GetRange(0, csvDataList.Count / 2);
                        //int start_index = csvDataList1.Count;
                        //int count = csvDataList.Count - csvDataList1.Count;
                        //List<datModel> csvDataList2 = csvDataList.GetRange(start_index, count);

                        //Thread thr1 = new Thread(createAllHtml(csvDataList1, nowDoneDir));
                        //Thread thr2 = new Thread(createAllHtml(csvDataList2, nowDoneDir));
                        //thr1.Start();
                        //thr2.Start();

                        if (csvDataList.Count > 0)
                            createAllHtml(csvDataList, nowDoneDir, threadName, timeYomi);
                    }
                    catch (Exception e)
                    {
                        if (threadName == "T1")
                            ErrorLogStringT1.AppendLine("csvファイル読み込みエラー：ファイル名：" + f.Name + " エラー：" + e.Message.ToString());
                        else
                            ErrorLogStringT2.AppendLine("csvファイル読み込みエラー：ファイル名：" + f.Name + " エラー：" + e.Message.ToString());
                        
                    }
                }
            }
            catch (Exception ex)
            {
                if (threadName == "T1")
                    ErrorLogStringT1.AppendLine("csvファイルのエラー：" + ex.Message.ToString());
                else
                    ErrorLogStringT2.AppendLine("csvファイルのエラー：" + ex.Message.ToString());
            }
        }
        public void zipFile(string PDFpath, string threadName, DateTime timeYomi)
        {
            try
            {
                string root = PDFpath;
                if (Directory.Exists(root))
                {
                    DirectoryInfo dirInput = new DirectoryInfo(root);
                    FileInfo[] filesPDF = dirInput.GetFiles("*.pdf");

                    if (!Directory.Exists(OutputDir))
                        Directory.CreateDirectory(OutputDir);

                    string zipPath = OutputDir + @"report_pdf_" + timeYomi.ToString("yyyyMMddHHmmss") + threadName + @".zip";
                    ZipFile.CreateFromDirectory(root, zipPath);
                    CreateMD5(zipPath, @"report_pdf_" + timeYomi.ToString("yyyyMMddHHmmss") + threadName + @".zip");
                    if (threadName == "T1")
                        LogStringT1.AppendLine("出力PDF：" + filesPDF.Length.ToString() + "件数");
                    else
                        LogStringT2.AppendLine("出力PDF：" + filesPDF.Length.ToString() + "件数");
                }
            }
            catch (Exception e)
            {
                if (threadName == "T1")
                    ErrorLogStringT1.AppendLine("PDFファイルzipのエラー：" + e.Message.ToString());
                else
                    ErrorLogStringT2.AppendLine("PDFファイルzipのエラー：" + e.Message.ToString());
            }

        }

        void CreateMD5(string filePath, string filename)
        {
            string output;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] checksum = md5.ComputeHash(stream);
                    output = BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();

                    using (StreamWriter sw = System.IO.File.CreateText(OutputDir + filename + ".md5"))
                    {
                        sw.Write(output + " " + filename);
                    }
                }
            }
        }
        public void createAllHtml(List<datModel> newDat, string Outputdir, string threadName, DateTime YomiTimeT)
        {
            double scale = 1;
            if (newDat != null)
            {
                foreach (var a in newDat)
                {
                    try
                    {
                        datModel chosenOne = a;
                        string temp = string.Empty;

                        DateTime now = DateTime.Now;
                        string pdfPath = Outputdir + "PDF\\" + chosenOne.社員職員番号 + @"@" + chosenOne.カナ氏名 + @"_" + chosenOne.受験者ID + @"_H" + @"" + @".pdf";
                        string htmlPath = Outputdir + "HTML\\" + chosenOne.社員職員番号 + @"@" + chosenOne.カナ氏名 + @"_" + chosenOne.受験者ID + @"_H" + @"" + @".html";
                        if (a.言語商品区分実施テスト == "151")
                        {
                            //string htmlPath = @"C:\testHtmlExport\" + DateTime.Now.ToString("HH_d") + @".html";
                            temp += "<div id=\"printableArea\" Style=\"transform: scale(" + scale.ToString() + "); transform-origin: top left;\"> ";

                            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(templatePathSS)))
                            {
                                temp = temp + sr.ReadToEnd() + "</div>";
                            }

                            foreach (PropertyInfo prop in typeof(datModel).GetProperties())
                            {
                                //temp = temp.Replace("Change:" + prop.Name, chosenOne.GetType().GetProperty(prop.Name).GetValue(chosenOne).ToString());
                                temp = temp.Replace("i." + prop.Name, chosenOne.GetType().GetProperty(prop.Name).GetValue(chosenOne).ToString());
                            }
                        }
                        else if (a.言語商品区分実施テスト == "153")
                        {
                            temp += "<div id=\"printableArea\" Style=\"transform: scale(" + scale.ToString() + "); transform-origin: top left;\"> ";

                            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(templatePathBB)))
                            {
                                temp = temp + sr.ReadToEnd() + "</div>";
                            }

                            foreach (PropertyInfo prop in typeof(datModel).GetProperties())
                            {
                                //temp = temp.Replace("Change:" + prop.Name, chosenOne.GetType().GetProperty(prop.Name).GetValue(chosenOne).ToString());
                                temp = temp.Replace("i." + prop.Name, chosenOne.GetType().GetProperty(prop.Name).GetValue(chosenOne).ToString());
                            }
                        }

                        if (!Directory.Exists(Outputdir + "HTML\\"))
                            Directory.CreateDirectory(Outputdir + "HTML\\");

                        StreamWriter swriter = new StreamWriter(htmlPath);
                        swriter.WriteLine(temp);
                        //Log.Error($"html is created: " + chosenOne.受験者ID + " " + chosenOne.カナ氏名);
                        swriter.Close();

                        HtmlToPdf converter = new HtmlToPdf();
                        converter.Options.PdfPageSize = PdfPageSize.A3;
                        converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                        converter.Options.MarginLeft = 10;
                        converter.Options.MarginRight = 10;
                        converter.Options.MarginTop = 10;
                        converter.Options.MarginBottom = 10;
                        PdfDocument doc = converter.ConvertUrl(htmlPath);
                        doc.RemovePageAt(2);
                        // save pdf document
                        doc.Save(pdfPath);

                        // close pdf document
                        doc.Close();

                        //Log.Error($"pdf is created: " + chosenOne.受験者ID + " " + chosenOne.カナ氏名);
                    }
                    catch (Exception e)
                    {
                        if (threadName == "T1")
                            ErrorLogStringT1.AppendLine("PDF作成エラー：" + e.Message.ToString());
                        else
                            ErrorLogStringT2.AppendLine("PDF作成エラー：" + e.Message.ToString());
                    }
                }
            }
            zipFile(Outputdir + "PDF\\", threadName, YomiTimeT);
        }

        public void WriteServiceLog(string Message)
        {
            try
            {
                string path = LogDir;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = path + "ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch(Exception ex){ }
        }
    }
}
