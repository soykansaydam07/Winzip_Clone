using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using System.Threading;

namespace UnzipPartition
{
    public class UnzipBuilt
    {

        ProgressBar progressBar;


        public void Unziping(string zipPath , string unzipPath , string name, ProgressBar progressBar)
        {
            //UnZipleme İşlemi

            this.progressBar = progressBar;

            if (string.IsNullOrEmpty(unzipPath))
            {
                MessageBox.Show("Please Select Your filename.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Thread thread = new Thread(t =>
            {
                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipPath))
                {
                    zip.ExtractProgress += Zip_ExtractProgress;
                    zip.ExtractAll(string.Format("{0}{1}", unzipPath, name), Ionic.Zip.ExtractExistingFileAction.DoNotOverwrite);
                }
            })
            { IsBackground = true };
            thread.Start();
        }

        private void Zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            Progressing(e);
        }

        public void Progressing(ExtractProgressEventArgs e)      // dll içinde  
        {

            if (e.EventType == Ionic.Zip.ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                {
                    progressBar.Invoke(new MethodInvoker(delegate
                    {
                        progressBar.Maximum = 100;
                        progressBar.Value = (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer);
                        progressBar.Update();
                    }));
                }
            }
        }

        public  string Folder()
        {
            string str = null;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select your path";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                str = fbd.SelectedPath;
            }

            return str;
        }


    }
}
