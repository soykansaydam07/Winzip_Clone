using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridAllFile
{

    /*
     * Projede bu kısım anaform olarak kullanılıp sistem winrar daki arayüz sistemine benzetilmeye çalısılmıştır
     * Kullanım olarak bilgisayardaki oluşturulmuş tüm klasör ve dosyaların gride eklenilmesi sağlanmış, bunun yanında dosyalarda geçiş yapılmak istenirse klasör türünde olan dosyalara çift tıklamak klasörün içindeki 
     * diğer dosyalara ulaşmaya  ,  griddeki elemanlardan olan (...) textine sahip elemana çift tıklamak ise bir önceki dizine geçmesi sağlanmıştır .Klasör olamyıp , dosya elemanlarında çift tıklamak dosyaların 
     * açılmasını sağlıyor. Gridde bir elemana odaklanıldıktan sonra zip ve bazı işlemler için üst kısımdaki butonlar kullanılmaktadır.     
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] files = System.IO.Directory.GetLogicalDrives();
            comboBox1.Items.AddRange(files);
            comboBox1.Text = comboBox1.Items[0].ToString();

            textBox1.Text = comboBox1.Text;
            PathFiles(comboBox1.Text , false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // String str = textBox1.Text;
           // PathFiles(str , false);
        }

        public void PathFiles(String path , bool choose )
        {

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataTable dataTable = new DataTable();
            DataRow dataRow;

            String[] strFiles;
            String[] strDirectories;

            try
            {
                strFiles = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
                strDirectories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Bu Dosya sadece okunabilir olduğundan giriş izni verilmemektedir");
                string[] words = textBox1.Text.Split('\\');

                textBox1.Text = "";

                for (int i = 0; i <= words.Length - 3; i++)
                {
                    textBox1.Text = textBox1.Text + words[i] + "\\";
                }
                return;
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Böyle bir aygıt bulunmamaktadır.");
                comboBox1.Text = comboBox1.Items[0].ToString();
                return;
            }


            dataTable.Columns.Add("File_Name");
            dataTable.Columns.Add("File_Size");
            dataTable.Columns.Add("File_Type");
            dataTable.Columns.Add("LastWrite_Date");

            if (choose)
            {
                dataRow = dataTable.NewRow();

                dataRow["File_Name"] = "...";

                dataTable.Rows.Add(dataRow);
            }

            long size = 0;

            for (int i = 0; i < strDirectories.Length; i++)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(strDirectories[i]);

                dataRow = dataTable.NewRow();

                dataRow["File_Name"] = directoryInfo.Name;
                dataRow["File_Type"] = "Dosya Klasörü";
                dataRow["LastWrite_Date"] = directoryInfo.LastWriteTime.Date.ToString("dd/MM/yyyy");

                dataTable.Rows.Add(dataRow);

            }

            for (int i = 0; i < strFiles.Length; i++)
            {

                FileInfo fileInfo = new FileInfo(strFiles[i]);
                FileSystemInfo fileSystemInfo = new FileInfo(strFiles[i]);


                dataRow = dataTable.NewRow();

                dataRow["File_Name"] = fileSystemInfo.Name;
                dataRow["File_Size"] = string.Format("{0:#,##0}", double.Parse((fileInfo.Length).ToString())); 
                dataRow["File_Type"] = fileSystemInfo.Extension;
                dataRow["LastWrite_Date"] = fileSystemInfo.LastWriteTime.Date.ToString("dd/MM/yyyy");

                size = size + Convert.ToInt64(fileInfo.Length); 
                dataTable.Rows.Add(dataRow);
            }

            if (dataTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = dataTable;
                dataGridView1.ClearSelection();
            }

            textBox7.Text = "Toplam " + strDirectories.Length + " klasör ve " + string.Format("{0:#,##0}", double.Parse(size.ToString())) + " bayt " + strFiles.Length + " dosya";

        }

        public void GridPath()
        {
            if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString().Equals("Dosya Klasörü") || textBox5.Text == "...")
            {
                bool counter =  true;
            
                textBox2.Text = (dataGridView1.CurrentRow.Index + 1).ToString();

                if (dataGridView1.Rows.Count > -1)
                {
                    textBox3.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                }
                // textBox3.Text = dataGridView1.SelectedRows[dataGridView1.CurrentRow.Index + 1].Cells[0].Value.ToString();

                    textBox1.Text = textBox1.Text + textBox3.Text + "\\";
            
                if (textBox3.Text.Equals("..."))
                {

                    string[] words = textBox1.Text.Split('\\');
                
                    textBox1.Text = "";

                    for (int i = 0; i <= words.Length-4; i++)
                    {
                        textBox1.Text = textBox1.Text +  words[i] +"\\" ; 
                    }

                }

                    if (textBox1.Text.Equals("C:\\") || textBox1.Text.Equals("D:\\") || textBox1.Text.Equals("E:\\") || textBox1.Text.Equals("F:\\") || textBox1.Text.Equals("G:\\"))
                    {
                         counter = false;
                    }
                

                PathFiles(textBox1.Text , counter);
            }
            else
            {
                String strExecute = textBox1.Text + textBox5.Text;
                System.Diagnostics.Process.Start(strExecute);    
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            GridPath();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Equals("...") || textBox5.Text.Equals("") || dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString().Equals(".zip"))
            {
                MessageBox.Show("Böyle bir işlem yapılamamaktadır");
                return;
            }
            
            String str = textBox1.Text + textBox5.Text;
            Adding add = new Adding(str, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
            add.Show();                     
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBox4.Text = (dataGridView1.CurrentRow.Index + 1).ToString();

            if (dataGridView1.Rows.Count > -1)
            {
                textBox5.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            }

            if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString().Equals("Dosya Klasörü"))
            {
                textBox6.Text = "Seçili 1 Klasör";
            }
            else
            {
                textBox6.Text = "Seçili " + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString() + " bayt 1 Dosya";
            }
            
        }

        //public string startPath { get { return textBox1.Text + textBox5.Text + "\\" ; } }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox5.Text.Equals("...") || textBox5.Text.Equals("")|| !dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString().Equals(".zip"))
            {
                MessageBox.Show("Böyle bir işlem yapılamamaktadır");
                return;
            }
            String str = textBox1.Text + textBox5.Text;
            Unzip unzip = new Unzip(str);
            unzip.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String[] strFiles;
            String[] strDirectories;

            if (textBox5.Text.Equals("...") || textBox5.Text.Equals(""))
            {
                MessageBox.Show("Böyle bir işlem yapılamamktadır");
                return;
            }
            try
            {

                if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString().Equals("Dosya Klasörü"))
                {
                    strDirectories = Directory.GetDirectories(textBox1.Text,textBox5.Text, SearchOption.TopDirectoryOnly);

                    Directory.Delete(strDirectories[0],true);
                }
                else
                {
                    strFiles = Directory.GetFiles(textBox1.Text,textBox5.Text, SearchOption.TopDirectoryOnly);

                    File.Delete(strFiles[0]);     
                }

            }
            catch (IOException)
            {
                MessageBox.Show("Bu Dosya sadece okunabilir olduğundan silinememektedir");
            }

            bool back = true;

            if (textBox1.Text.Equals("C:\\") || textBox1.Text.Equals("D:\\") || textBox1.Text.Equals("E:\\") || textBox1.Text.Equals("F:\\") || textBox1.Text.Equals("G:\\"))
            {
                back = false;
            }

            PathFiles(textBox1.Text, back);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GridPath();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Text;
            PathFiles(comboBox1.Text, false);
        }
    }
}
