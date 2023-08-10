using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;
using UnzipPartition;

namespace GridAllFile
{
    /*
     * Bu kısımda ise .zip uzantılı bir klasöre odaklanıldıktan sonra dizine çıkar butonuna basılarak bu ekrana geçilmektedir
     * Bu kısımda UnzipPartition adında oluşturulmuş başka bir projeden(dll mantığı kullanılmak için) nesne oluşturulmuştur.
     * Unzip Partition da ise thread mantığı kullanılarak aynı anda birden fazla dosya zipi açılabilmektedir   
     */
    public partial class Unzip : Form
    {
        string zipPath;
        public Unzip(String str )
        {
            InitializeComponent();
            zipPath = str;
        }

        UnzipBuilt built = new UnzipBuilt();

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
            {
                MessageBox.Show("Lütfen İlgili alanları tam olarak doldurunuz");
                return;
            }

            string unzipPath = textBox2.Text + "\\";
            string name = textBox1.Text;

            //UnZipleme İşlemi

            built.Unziping(zipPath, unzipPath , name , progressBar);

           // MessageBox.Show("Dosyanız Oluşturulmuştur");
           // Close();        
        }


        private void btnFile2_Click(object sender, EventArgs e)
        {
            textBox2.Text = built.Folder();
        }
    }
}
