using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateMD5fileTool
{
    public partial class CreateMD5 : Form
    {
        public CreateMD5()
        {
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var fileContent = string.Empty;
                var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "zip files (*.zip)|*.zip|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = openFileDialog.FileName;

                        string zipToMD5;
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(filePath))
                            {
                                byte[] checksum = md5.ComputeHash(stream);
                                zipToMD5 = BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();
                            }
                        }

                        string zipFileName = Path.GetFileName(filePath);
                        using (StreamWriter sw = System.IO.File.CreateText(filePath + ".md5"))
                        {
                            sw.Write(zipToMD5 + " *" + zipFileName);
                        }
                        MessageBox.Show("md5ファイルを作成しました");
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("エラーが発生しました：" + ex.Message.ToString());
            }
           
        }
    }
}
