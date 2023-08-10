using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Ionic.Zip;
using ZipPartition;

namespace GridAllFile
{
    /*
     * Bu kısımda herhangi bir dosya ya da klasöre  odaklanıldıktan sonra ekle butonuna basılarak bu ekrana geçilmektedir
     * Bu kısımda ZipPartition adında oluşturulmuş başka bir projeden(dll mantığı kullanılmak için) nesne oluşturulmuştur.
     * Zip Partition da ise thread mantığı kullanılarak aynı anda birden fazla dosya sıkıştırılabilmektedir.   
     */
    public partial class Adding : Form
    {
        string startPath;
        string folderorfile;
        public Adding(string str ,string folderorfile)
        {
            InitializeComponent();
            startPath = str;
            this.folderorfile = folderorfile;
        }

        ZipBuilt built = new ZipBuilt();
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
            {
                MessageBox.Show("Lütfen İlgili alanları tam olarak doldurunuz");
                return;
            }

            string zipPath = textBox2.Text + "\\";
            string name = textBox1.Text;
            //Zipleme İşlemi

            built.ZipingFolder(startPath ,zipPath , name , progressBar , folderorfile);

           // MessageBox.Show("Dosyanız Oluşturulmuştur");
           // Close();

        }


        private void btnFolder_Click(object sender, EventArgs e)
        {
            textBox2.Text = built.Folder();       
        }

   
    }
}
