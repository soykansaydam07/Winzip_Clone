using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using System.Threading;

namespace ZipPartition
{
    public class ZipBuilt
    {
        ProgressBar progressBar;

        public void ZipingFolder (string startPath ,string zipPath, string name , ProgressBar progressBar , string folderorfile)
        {
            this.progressBar = progressBar;

            if (folderorfile.Equals("Dosya Klasörü"))
            {
                if (string.IsNullOrEmpty(zipPath))
                {
                    MessageBox.Show("Please Select Your Folder.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    return;
                }
                // string path = textBox2.Text;
                Thread thread = new Thread(t =>
                {
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        zip.AddDirectory(startPath);
                        zip.SaveProgress += Zip_SaveProgress;

                        zip.Save(string.Format("{0}{1}.zip", zipPath, name));

                    }
                })
                { IsBackground = true };
                thread.Start();
            }
            else
            {
                if (string.IsNullOrEmpty(zipPath))
                {
                    MessageBox.Show("Please Select Your filename.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    return;
                }
                // string fileName = textBox2.Text;
                Thread thread = new Thread(t =>
                {
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        zip.AddFile(startPath);
                        zip.SaveProgress += Zip_SaveFileProgress;
                        zip.Save(string.Format("{0}/{1}.zip", zipPath, name));
                    }
                })
                { IsBackground = true };
                thread.Start();
            }
        }

        private void Zip_SaveFileProgress(object sender, SaveProgressEventArgs e)
        {
            ProcessingFile(e);
        }

        private void Zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            ProcessingFolder(e);
        }

        public void ProcessingFile(SaveProgressEventArgs e)
        {
            if (e.EventType == Ionic.Zip.ZipProgressEventType.Saving_EntryBytesRead)
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

        public void ProcessingFolder(SaveProgressEventArgs e)
        {
            if (e.EventType == Ionic.Zip.ZipProgressEventType.Saving_BeforeWriteEntry)
            {
                {
                    progressBar.Invoke(new MethodInvoker(delegate
                    {
                        progressBar.Maximum = e.EntriesTotal;
                        progressBar.Value = e.EntriesSaved + 1;
                        progressBar.Update();
                    }));
                }
            }
        }

        public string Folder()
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
