using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySql.Data.Common;
using System;
using System.Management;

namespace RESTORAN
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        MySqlConnection baglanti;
        bool drm_kntrl = false;
        private void Form2_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
            build.Server = "localhost";
            build.UserID = "root";
            build.Database = "youdatabase";
            build.Password = "yourpassword";
            baglanti = new MySqlConnection(build.ToString());

            baglanti.Open();
            //satış sayfası combobox masa çek
            string masacek = "select * from personel";
            MySqlCommand masagetir = new MySqlCommand(masacek, baglanti);
            MySqlDataReader read = masagetir.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read.GetString("kullanici_adi"));
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            string user = comboBox1.Text;
            string pass = textBox1.Text;
            MySqlCommand kontrol = new MySqlCommand();
            kontrol.Connection = baglanti;
            kontrol.CommandText = "SELECT * FROM personel where kullanici_adi='" + comboBox1.Text + "' AND sifre='" + textBox1.Text + "'";
            MySqlDataReader dr = kontrol.ExecuteReader();
            if (dr.Read())
            {
                Form1.kullanici_yetki = dr["yetki"].ToString();
                Form1 yenisayfa = new Form1();
                Visible = false;
                yenisayfa.ShowDialog();
                this.Show();
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Hatalı Kullanıcı Adı veya Şifre Girdiniz");
            }


            baglanti.Close();



           
        }
    }
}
