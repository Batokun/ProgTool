using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CheckTool
{
    public partial class frmMain : Form
    {
        private const double V = 3;

        public List<string> InputFolderFileNamesT1 = new List<string>();
        public List<string> InputFolderFileNamesBkT1 = new List<string>();
        public DateTime CheckToolTime;

        public List<string> InputFolderFileNamesT2 = new List<string>();
        public List<string> InputFolderFileNamesBkT2 = new List<string>();
        public DateTime CheckToolTimeT2;

        string InputDir = @"C:\ftproot\ICPROGSYS\PHX-in\";
        string OutputIcraftDir = @"C:\IcraftProgOutput\";
        string LogDir = @"C:\PROGCHECKTOOL\LOG\";
        string OutputDir = @"C:\ftproot\ICPROGSYS\PDF-out\";
        string TimeLogDir = @"C:\PROGCHECKTOOL\LOG\TIMELOG\";
        string TempDir = @"C:\PROGCHECKTOOL\TEMPLATE\";

        //int fileSize = 0;
        //long totalSizes = 0;

        //Boolean checkFlag = true;
        //DateTime timeYomi = DateTime.Now;

        //StringBuilder LogCheckString = new StringBuilder();
        //StringBuilder ErrorLogCheckString = new StringBuilder();

        Boolean checkFlagT1 = true;
        Boolean checkFlagT2 = true;
        DateTime timeYomiT1 = DateTime.Now;
        DateTime timeYomiT2 = DateTime.Now;

        StringBuilder LogStringT1 = new StringBuilder();
        StringBuilder ErrorLogStringT1 = new StringBuilder();
        StringBuilder LogStringT2 = new StringBuilder();
        StringBuilder ErrorLogStringT2 = new StringBuilder();
        Boolean stop = false;
        public frmMain()
        {
            InitializeComponent();
            this.label1.ForeColor = Color.Blue;
            start();
        }

        public void start()
        {
            Thread threadload1 = new Thread(new ThreadStart(PollingFuncT1));
            threadload1.Start();

            //Thread.Sleep(threadSleepTime * 1000);
            Thread threadload2 = new Thread(new ThreadStart(PollingFuncT2));
            threadload2.Start();
        }

        public void PollingFuncT1()
        {
            while (checkFlagT1)
            {
                try
                {
                    LogStringT1 = new StringBuilder();
                    ErrorLogStringT1 = new StringBuilder();
                    timeYomiT1 = DateTime.Now;
                    stop = false;

                    if (!Directory.Exists(TimeLogDir))
                    {
                        Directory.CreateDirectory(TimeLogDir);
                    }
                    if (!File.Exists(TimeLogDir + "before_start_timeT1.txt"))
                    {
                        using (StreamWriter sw = File.CreateText(TimeLogDir + "before_start_timeT1.txt"))
                        {
                            sw.WriteLine("1900/01/01 00:00:01");
                        }
                    }

                    CheckInputFolderT1(timeYomiT1);
                    List<string> OutDirList = CheckIcraftOutputFolderT1(timeYomiT1);

                    if (OutDirList.Count > 0)
                    {
                        CheckCSV_WaitTimeT1(OutDirList);
                        CheckOutputFileT1(OutDirList);
                        writeLogT1(timeYomiT1);
                    }

                    writeErrorLogT1(timeYomiT1);
                    using (StreamWriter sw = File.CreateText(TimeLogDir + "before_start_timeT1.txt"))
                    {
                        sw.WriteLine(timeYomiT1.AddMinutes(-1));
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    ErrorLogStringT1.AppendLine("エラーT1：" + ex.Message.ToString());
                    writeErrorLogT1(timeYomiT1);
                }
            }
        }
        public void PollingFuncT2()
        {
            while (checkFlagT2)
             {
                try
                {
                    LogStringT2 = new StringBuilder();
                    ErrorLogStringT2 = new StringBuilder();
                    timeYomiT2 = DateTime.Now;
                    stop = false;

                    if (!Directory.Exists(TimeLogDir))
                    {
                        Directory.CreateDirectory(TimeLogDir);
                    }
                    if (!File.Exists(TimeLogDir + "before_start_timeT2.txt"))
                    {
                        using (StreamWriter sw = File.CreateText(TimeLogDir + "before_start_timeT2.txt"))
                        {
                            sw.WriteLine("1900/01/01 00:00:01");
                        }
                    }

                    CheckInputFolderT2(timeYomiT2);
                    List<string> OutDirList = CheckIcraftOutputFolderT2(timeYomiT2);

                    if (OutDirList.Count > 0)
                    {
                        CheckCSV_WaitTimeT2(OutDirList);
                        CheckOutputFileT2(OutDirList);
                        writeLogT2(timeYomiT2);
                    }
                    else
                    {
                        //CheckInputFolderT2(timeYomiT2);
                    }
                    writeErrorLogT2(timeYomiT2);

                    using (StreamWriter sw = File.CreateText(TimeLogDir + "before_start_timeT2.txt"))
                    {
                        sw.WriteLine(timeYomiT2.AddMinutes(-1));
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    ErrorLogStringT2.AppendLine("エラーT2：" + ex.Message.ToString());
                    writeErrorLogT2(timeYomiT1);
                }
            }
        }
        void writeLogT1(DateTime timeYomi)
        {
            if (LogStringT1.Length > 0)
            {
                if (!Directory.Exists(LogDir))
                    Directory.CreateDirectory(LogDir);
                using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + "T1-ログ.txt"))
                {
                    sw.WriteLine(LogStringT1);
                }
            }
        }
        void writeLogT2(DateTime timeYomi)
        {
            if (LogStringT2.Length > 0)
            {
                if (!Directory.Exists(LogDir))
                    Directory.CreateDirectory(LogDir);
                using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + "T2-ログ.txt"))
                {
                    sw.WriteLine(LogStringT2);
                }
            }
        }
        void writeErrorLogT1(DateTime timeYomi)
        {
            if (!Directory.Exists(TimeLogDir))
                Directory.CreateDirectory(TimeLogDir);
            if (!Directory.Exists(LogDir))
                Directory.CreateDirectory(LogDir);

            if (ErrorLogStringT1.Length > 0)
            {
                if (stop == false)
                {
                    using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + "T1-エラーログ.txt"))
                    {
                        sw.WriteLine(ErrorLogStringT1);
                    }
                    SendMail(ErrorLogStringT1);
                }
                else
                {

                    if (!File.Exists(TimeLogDir + "last_mail_time1.txt"))
                    {
                        using (StreamWriter sw = File.CreateText(TimeLogDir + "last_mail_time1.txt"))
                        {
                            sw.WriteLine("1900/01/01 00:00:01");
                        }
                    }

                    string txtTime = File.ReadAllText(TimeLogDir + "last_mail_time1.txt");
                    DateTime lastMailSent = Convert.ToDateTime(txtTime);

                    if (lastMailSent.AddDays(1) < DateTime.Now)
                    {
                        if (!Directory.Exists(LogDir))
                            Directory.CreateDirectory(LogDir);
                        using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + "T1-エラーログ.txt"))
                        {
                            sw.WriteLine(ErrorLogStringT1);
                        }
                        SendMail(ErrorLogStringT1);

                        using (StreamWriter sw = File.CreateText(TimeLogDir + "last_mail_time1.txt"))
                        {
                            sw.WriteLine(DateTime.Now);
                        }
                    }
                }
            }
        }
        void writeErrorLogT2(DateTime timeYomi)
        {
            if (!Directory.Exists(TimeLogDir))
                Directory.CreateDirectory(TimeLogDir);
            if (!Directory.Exists(LogDir))
                Directory.CreateDirectory(LogDir);

            if (ErrorLogStringT2.Length > 0)
            {
                if (stop == false)
                {
                    using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + "T2-エラーログ.txt"))
                    {
                        sw.WriteLine(ErrorLogStringT2);
                    }
                    SendMail(ErrorLogStringT2);
                }
                else
                {
                    if (!File.Exists(TimeLogDir + "last_mail_time2.txt"))
                    {
                        using (StreamWriter sw = File.CreateText(TimeLogDir + "last_mail_time2.txt"))
                        {
                            sw.WriteLine("1900/01/01 00:00:01");
                        }
                    }

                    string txtTime = File.ReadAllText(TimeLogDir + "last_mail_time2.txt");
                    DateTime lastMailSent = Convert.ToDateTime(txtTime);

                    if (lastMailSent.AddDays(1) < DateTime.Now)
                    {
                        if (!Directory.Exists(LogDir))
                            Directory.CreateDirectory(LogDir);
                        using (StreamWriter sw = System.IO.File.CreateText(LogDir + timeYomi.ToString("yyyyMMddHHmmss") + "T2-エラーログ.txt"))
                        {
                            sw.WriteLine(ErrorLogStringT2);
                        }
                        SendMail(ErrorLogStringT2);
                        using (StreamWriter sw = File.CreateText(TimeLogDir + "last_mail_time2.txt"))
                        {
                            sw.WriteLine(DateTime.Now);
                        }
                    }
                }
            }
            
        }
        public void CheckInputFolderT1(DateTime yomi1)
        {
            try
            {
                string[] zipfiles = Directory.GetFiles(InputDir, "*.zip");
                string[] md5files = Directory.GetFiles(InputDir, "*.md5");
                DateTime createDate;

                if (InputFolderFileNamesT1.Count > 1)
                {
                    if (InputFolderFileNamesBkT1.Count > 1)
                    {
                        int cntold = 0;
                        foreach (string oldfile in InputFolderFileNamesT1)
                        {
                            foreach (string file in zipfiles)
                            {
                                if (file == oldfile)
                                {
                                    //InputFolderFileNamesBkT1.Add(oldfile);
                                    cntold++;
                                }
                            }
                        }

                        if (InputFolderFileNamesBkT1.Count == cntold)
                        {
                            stop = true;
                            ErrorLogStringT1.AppendLine("エラー：" + DateTime.Now.ToString() + "：" + "PROGTOOLでエラーが発生したか、停止している恐れがあります。確認ください。</br>");
                        }
                        InputFolderFileNamesBkT1.Clear();
                    }
                    else
                    {
                        foreach (string file in zipfiles)
                        {
                            foreach (string oldfile in InputFolderFileNamesT1)
                            {
                                if (file == oldfile)
                                {
                                    InputFolderFileNamesBkT1.Add(oldfile);
                                }
                            }
                        }
                    }

                    InputFolderFileNamesT1.Clear();
                    foreach (string file in zipfiles)
                    {
                        createDate = Directory.GetLastAccessTime(file);
                        if (createDate.AddMinutes(1) < yomi1)
                        {
                            foreach (string md5file in md5files)
                            {
                                if (md5file == file + ".md5")
                                {
                                    InputFolderFileNamesT1.Add(file);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (string file in zipfiles)
                    {
                        createDate = Directory.GetLastAccessTime(file);
                        if (createDate.AddMinutes(1) < yomi1)
                        {
                            foreach (string md5file in md5files)
                            {
                                if (md5file == file + ".md5")
                                {
                                    InputFolderFileNamesT1.Add(file);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT1.AppendLine("エラーT1：" + ex.Message.ToString() + "</br>");
            }
        }
        public void CheckInputFolderT2(DateTime yomi2)
        {
            try
            {
                string[] files = Directory.GetFiles(InputDir);
                DateTime createDate;
                if (InputFolderFileNamesT2.Count > 1)
                {
                    if (InputFolderFileNamesBkT2.Count > 1)
                    {
                        int cntold = 0;
                        foreach (string oldfile in InputFolderFileNamesT2)
                        {
                            foreach (string file in files)
                            {
                                if (file == oldfile)
                                {
                                    //InputFolderFileNamesBkT1.Add(oldfile);
                                    cntold++;
                                }
                            }
                        }

                        if (InputFolderFileNamesBkT2.Count == cntold)
                        {
                            stop = true;
                            ErrorLogStringT2.AppendLine("エラー：" + DateTime.Now.ToString() + "：" + "PROGTOOLでエラーが発生したか、停止している恐れがあります。確認ください。</br>");
                        }
                        InputFolderFileNamesBkT2.Clear();
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                            foreach (string oldfile in InputFolderFileNamesT2)
                            {
                                if (file == oldfile)
                                {
                                    InputFolderFileNamesBkT2.Add(oldfile);
                                }
                            }
                        }
                    }

                    InputFolderFileNamesT2.Clear();
                    foreach (string file in files)
                    {
                        createDate = Directory.GetLastAccessTime(file);
                        if (createDate.AddMinutes(1) < yomi2)
                        {
                            InputFolderFileNamesT2.Add(file);
                        }
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        createDate = Directory.GetLastAccessTime(file);
                        if (createDate.AddMinutes(1) < yomi2)
                        {
                            InputFolderFileNamesT2.Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT2.AppendLine("エラーT2：" + ex.Message.ToString() + "</br>");
            }
        }
        public List<string> CheckIcraftOutputFolderT1(DateTime timeYomiT11)
        {
            List<string> dirs = new List<string>();
            DateTime createDate;
            try
            {
                string txtTime = File.ReadAllText(TimeLogDir + "before_start_timeT1.txt");
                DateTime lastStart = Convert.ToDateTime(txtTime);
                string[] alldirs = Directory.GetDirectories(OutputIcraftDir, "*", SearchOption.AllDirectories);
                               
                foreach (string dir in alldirs)
                {
                    createDate = Directory.GetCreationTime(dir);
                    if (createDate.AddMinutes(1) < timeYomiT11)
                    {
                        if (createDate >= lastStart)
                        {
                            if (dir.Substring(dir.LastIndexOf("\\") + 1, dir.Length - dir.LastIndexOf("\\") - 1) != "Error")
                            {
                                if (dir.Substring(dir.Length - 2, 2) == "T1")
                                {
                                    dirs.Add(dir);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT1.AppendLine("エラーT1：" + ex.Message.ToString() + "</br>");
            }

            return dirs;
        }
        public List<string> CheckIcraftOutputFolderT2(DateTime timeYomiT22)
        {
            List<string> dirs = new List<string>();
            DateTime createDate;
            try
            {
                string txtTime = File.ReadAllText(TimeLogDir + "before_start_timeT2.txt");
                DateTime lastStart = Convert.ToDateTime(txtTime);
                string[] alldirs = Directory.GetDirectories(OutputIcraftDir, "*", SearchOption.TopDirectoryOnly);
                
                foreach (string dir in alldirs)
                {
                    createDate = Directory.GetCreationTime(dir);
                    if (createDate.AddMinutes(1) < timeYomiT22)
                    {
                        if (createDate >= lastStart)
                        {
                            if (dir.Substring(dir.LastIndexOf("\\") + 1, dir.Length - dir.LastIndexOf("\\") - 1) != "Error")
                            {
                                if (dir.Substring(dir.Length - 2, 2) == "T2")
                                {
                                    dirs.Add(dir);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT2.AppendLine("エラーT2：" + ex.Message.ToString() + "</br>");
            }
            return dirs;
        }
        public void CheckCSV_WaitTimeT1(List<string> OutDirList)
        {
            try
            {
                string trd = "T1";
                double rowcnt = 0;
                double total_rowcnt = 0;

                if (!Directory.Exists(TempDir + trd))
                {
                    Directory.CreateDirectory(TempDir + trd);
                }

                DirectoryInfo di = new DirectoryInfo(TempDir + trd);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                foreach (string outdir in OutDirList)
                {
                    string[] zipFiles = Directory.GetFiles(outdir, "*.zip");

                    foreach (string zippath in zipFiles)
                    {
                        string fName = Path.GetFileName(zippath);
                        File.Copy(Path.Combine(outdir, fName), Path.Combine(TempDir + trd, fName));
                        ZipFile.ExtractToDirectory(TempDir + trd + "\\" + fName, TempDir + trd);
                    }

                    string[] csvFiles = Directory.GetFiles(TempDir + trd, "*.csv");

                    foreach (string csvpath in csvFiles)
                    {
                        var size = File.ReadAllLines(csvpath);
                        rowcnt = size.Length - 1;
                        total_rowcnt = total_rowcnt + rowcnt;
                    }

                    string[] tmpFiless = Directory.GetFiles(TempDir + trd);
                    foreach (string f in tmpFiless)
                    {
                        File.Delete(f);
                    }
                }

                if (total_rowcnt > 0)
                {
                    Thread.Sleep((int)(total_rowcnt * V * 1000));
                }

            }
            catch (Exception ex)
            {
                 ErrorLogStringT1.AppendLine("エラーT1vbvb：" + ex.Message.ToString() + "</br>");
            }
        }
        public void CheckCSV_WaitTimeT2(List<string> OutDirList)
        {
            try
            {
                string trd = "T2";
                double rowcnt = 0;
                double total_rowcnt = 0;

                if (!Directory.Exists(TempDir + trd))
                {
                    Directory.CreateDirectory(TempDir + trd);
                }

                DirectoryInfo di = new DirectoryInfo(TempDir + trd);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                foreach (string outdir in OutDirList)
                {
                    string[] zipFiles = Directory.GetFiles(outdir, "*.zip");

                    foreach (string zippath in zipFiles)
                    {
                        string fName = Path.GetFileName(zippath);
                        File.Copy(Path.Combine(outdir, fName), Path.Combine(TempDir + trd, fName));
                        ZipFile.ExtractToDirectory(TempDir + trd + "\\" + fName, TempDir + trd);
                    }

                    string[] csvFiles = Directory.GetFiles(TempDir + trd, "*.csv");

                    foreach (string csvpath in csvFiles)
                    {
                        var size = File.ReadAllLines(csvpath);
                        rowcnt = size.Length - 1;
                        total_rowcnt = total_rowcnt + rowcnt;
                    }

                    string[] tmpFiless = Directory.GetFiles(TempDir + trd);
                    foreach (string f in tmpFiless)
                    {
                        File.Delete(f);
                    }
                }

                if (total_rowcnt > 0)
                {
                    Thread.Sleep((int)(total_rowcnt * V * 1000));
                }
              
            }
            catch (Exception ex)
            {
                ErrorLogStringT2.AppendLine("エラーT2：" + ex.Message.ToString() + "</br>");
            }
        }
        public void CheckOutputFileT1(List<string> OutDirLis)
        {
            try
            {
                //string trd = "T1";
                string check_dir = "";
                string check_file = "";
                foreach (string dir in OutDirLis)
                {
                    check_dir = dir.Substring(dir.LastIndexOf("\\") + 1, dir.Length - dir.LastIndexOf("\\") - 1);
                    check_file = OutputDir + "report_pdf_" + check_dir + ".zip";

                    //input *.zip file.iin ner oloh
                    string[] zipFiles = Directory.GetFiles(dir, "*.zip");
                    if (File.Exists(check_file) == true)
                    {
                        foreach (string zippath in zipFiles)
                        {
                            string fName = Path.GetFileName(zippath);
                            LogStringT1.Append("「" + fName + "」のファイルが出力出来ています。" + "</br>");
                        }
                    }
                    else
                    {
                        foreach (string zippath in zipFiles)
                        {
                            string fName = Path.GetFileName(zippath);
                            ErrorLogStringT1.Append("「" + fName + "」のファイルが出力出来ていません。" +
                                "入力フォルダや" + "「" + dir + "」" + "を確認ください。" + "</br>");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLogStringT1.AppendLine("エラーT1：" + ex.Message.ToString() + "</br>");
            }
        }
        public void CheckOutputFileT2(List<string> OutDirLis)
        {
            try
            {
                string check_dir = "";
                string check_file = "";
                foreach (string dir in OutDirLis)
                {
                    check_dir = dir.Substring(dir.LastIndexOf("\\") + 1, dir.Length - dir.LastIndexOf("\\") - 1);
                    check_file = OutputDir + "report_pdf_" + check_dir + ".zip";

                    //input *.zip file.iin ner oloh
                    string[] zipFiles = Directory.GetFiles(dir, "*.zip");
                    if (File.Exists(check_file) == true)
                    {
                        foreach (string zippath in zipFiles)
                        {
                            string fName = Path.GetFileName(zippath);
                            LogStringT2.Append("「" + fName + "」のファイルが出力出来ています。" + "</br>");
                        }
                    }
                    else
                    {
                        foreach (string zippath in zipFiles)
                        {
                            string fName = Path.GetFileName(zippath);
                            ErrorLogStringT2.Append("「" + fName + "」のファイルが出力出来ていません。" +
                                "入力フォルダや" + "「" + dir + "」" + "を確認ください。" + "</br>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogStringT2.AppendLine("エラーT2：" + ex.Message.ToString() + "</br>");
            }
        }
        private void btnTeishi_Click(object sender, EventArgs e)
        {
            if (checkFlagT1)
            {
                this.label1.Text = "停止中";
                this.label1.ForeColor = Color.Red;
                checkFlagT1 = false;
                checkFlagT2 = false;
                this.btnTeishi.Text = "スタート";
            }
            else
            {
                this.label1.Text = "実行中";
                this.label1.ForeColor = Color.Blue;
                this.btnTeishi.Text = "停止";
                checkFlagT1 = true;
                checkFlagT2 = true;
                Thread threadloadT1 = new Thread(new ThreadStart(PollingFuncT1));
                threadloadT1.Start();

                //Thread.Sleep(threadSleepTime * 1000); //setvalue (sec) * 1000(ms)
                Thread threadloadT2 = new Thread(new ThreadStart(PollingFuncT2));
                threadloadT2.Start();
            }
        }
        public void SendMail(StringBuilder ErrorLogCheckString)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("progsystemmail@gmail.com");
                mail.To.Add("tsendayush.j@icraft.jp");
                mail.Subject = "PROGシステム：エラー";
                string str1 = "<h1> PROGTOOLでエラーが発生しました。</h1>";
                //string str2 = "エラー：" + DateTime.Now.ToString() + "：" + "PROGTOOLでエラーが発生したか、停止している恐れがあります。確認ください。";
                mail.Body = str1 + ErrorLogCheckString.ToString();
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("progsystemmail@gmail.com", "Prog2021!");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
