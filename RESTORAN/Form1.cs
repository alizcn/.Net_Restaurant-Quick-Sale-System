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
using System.Drawing;
using System.Drawing.Printing;
using System.Management;

namespace RESTORAN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MySqlConnection baglanti;
        static public string kullanici_yetki;


        //**********************fonksiyonlar***************************

        //kategori iþlemleri listele
        void kate_islem_listele()
        {
            string kayit = "SELECT kategori_id as Sýra,kategori_adi as Kategori from kategoriler";
            MySqlCommand komut = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            kate_islem_grid.DataSource = dt;
        }

        //masa iþlemleri listele
        void masa_islem_listele()
        {
            string kayit = "SELECT masa_id as Sýra,masa_adi as Masa from masa";
            MySqlCommand komut = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            masa_islem_grid.DataSource = dt;
        }

        //urun iþlemlerinde ürünleri listele
        void urun_islem_urun_listele()
        {
            string getir = "SELECT urun_id as Ürün_No,kategori_id as Kategori_No,urun_adi as Ürün,fiyat as Fiyat from urunler";
            MySqlCommand calis = new MySqlCommand(getir, baglanti);
            MySqlDataAdapter tbl = new MySqlDataAdapter(calis);
            DataTable urun_liste = new DataTable();
            tbl.Fill(urun_liste);
            urun_isl_urun_grid.DataSource = urun_liste;
        }
        //Ürün iþlemlerinde kategori listele
        void urun_islem_kate_listele()
        {
            string kayit = "SELECT kategori_id as Sýra,kategori_adi as Kategori from kategoriler";
            MySqlCommand komut = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            urun_isl_kate_grid.DataSource = dt;
        }

        //satýþ sayfasý masa toplam hesap
        void hesap_toplami()
        {
            float masa_hesabi = 0;

            for (int i = 0; i < satis_gecici_grid.Rows.Count ; i++)
            {
                float toplam = float.Parse(satis_gecici_grid.Rows[i].Cells["Toplam_Fiyat"].Value.ToString());
                masa_hesabi = masa_hesabi + toplam;

            }
            label13.Text = masa_hesabi.ToString() + " TL";
        }

        //personel listeleme
        void personel_listele()
        {
            
            string kayit = "SELECT personel_id as Sýra,personel_tc as TCK_No,personel_ad as Ad,personel_soyad as Soyad,personel_adres as Adres,personel_dog_tar as Doðum_Tarihi,personel_cep_bir as Tel_1,personel_cep_iki as Tel_2,personel_email as E_Mail,kullanici_adi as Kullanýcý_Adý,sifre as Þifre,yetki as Yetki from personel";
            MySqlCommand komut = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            personel_grid.DataSource = dt;
           
        }

        void gecici_kayit_listele()
        {
            string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat from gecici_kayit";
            MySqlCommand list = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(list);
            DataTable dt = new DataTable();
            da.Fill(dt);
            satis_gecici_grid.DataSource = dt;
        }

        //******MAC ADRESÝ ÇEK********
        static string Mac()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
            {
                if ((bool)obj["IPEnabled"])
                {
                    return obj["MacAddress"].ToString();
                }
            }

            return String.Empty;
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            MySqlConnectionStringBuilder build = new MySqlConnectionStringBuilder();
            build.Server = "localhost";
            build.UserID = "root";
            build.Database = "lokanta";
            build.Password = "3044Cisco";
            baglanti = new MySqlConnection(build.ToString());



            baglanti.Open();
            //*******MAC_ADRESSs********
            string cek_mac = "select * from get_adress_mac";
            MySqlCommand get_mac = new MySqlCommand(cek_mac, baglanti);
            MySqlDataReader read_mac = get_mac.ExecuteReader();
            string mac_adress;
            while (read_mac.Read())
            {
                mac_adress=read_mac.GetString("get_adress_mac");
                label28.Text= mac_adress;
            }
            string mac = Mac();;
            if (String.IsNullOrEmpty(mac) || label28.Text != mac)
            {
                MessageBox.Show("...... YAZILIM © 2022 TÜM HAKLARI SAKLIDIR.");
                Environment.Exit(0);
            }
  
            baglanti.Close();



            baglanti.Open();

            //*******MODÜL ÝÞLEMLERÝÝii******
            string cek_modul = "select * from moduls_active";
            MySqlCommand get_modul = new MySqlCommand(cek_modul, baglanti);
            MySqlDataReader read_moduls = get_modul.ExecuteReader();
            string moduls;
            List<string> intermediate_list = new List<string>();
            
            while (read_moduls.Read())
            {
                intermediate_list.Add(read_moduls.GetString("modul_active"));
                
            }
            if (kullanici_yetki == "YÖNETÝCÝ")
            {
                if (intermediate_list.Contains("www.rpt.sys.com"))
                {
                    pictureBox6.Visible = false;
                }
                if (intermediate_list.Contains("www.prsn.sys.com"))
                {
                    pictureBox5.Visible = false;
                }
                if (intermediate_list.Contains("www.tbl.sys.com"))
                {
                    pictureBox4.Visible = false;
                }
                if (intermediate_list.Contains("www.ctg.sys.com"))
                {
                    pictureBox3.Visible = false;
                }
                if (intermediate_list.Contains("www.urn.sys.com"))
                {
                    pictureBox2.Visible = false;
                }
                if (intermediate_list.Contains("www.sell.sys.com"))
                {
                    pictureBox1.Visible = false;
                }
            }
            else if(kullanici_yetki == "PERSONEL")
            {
                if (intermediate_list.Contains("www.sell.sys.com"))
                {
                    pictureBox1.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("YETKÝLENDÝRME HATASI");
            }
    

            baglanti.Close();


            baglanti.Open();

            kate_islem_listele();
            masa_islem_listele();
            urun_islem_kate_listele();
            urun_islem_urun_listele();



            //satýþ sayfasý kategori yükleme
            string kayit = "SELECT kategori_id as Sýra,kategori_adi as Kategori from kategoriler";
            MySqlCommand komut = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            satis_kategori_grid.DataSource = dt;
            baglanti.Close();

            baglanti.Open();
            //satýþ sayfasý combobox masa çek
            string masacek = "select * from masa";
            MySqlCommand masagetir = new MySqlCommand(masacek, baglanti);
            MySqlDataReader read = masagetir.ExecuteReader();
            while (read.Read())
            {
                satis_masa_cmb.Items.Add(read.GetString("masa_adi"));
            }
            baglanti.Close();


            baglanti.Open();
            //satýþ sayfasý personel çek
            string personelcek = "select * from personel";
            MySqlCommand personelgetir = new MySqlCommand(personelcek, baglanti);
            MySqlDataReader oku = personelgetir.ExecuteReader();
            while (oku.Read())
            {
                satis_per_cmb.Items.Add(oku.GetString("kullanici_adi"));

                //satislardaki personel combo içerisi
                satislar_personel_cmb.Items.Add(oku.GetString("kullanici_adi"));
            }

            baglanti.Close();



            baglanti.Open();
            personel_listele();
            baglanti.Close();

        }

        private void kate_islem_ekle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string komut = "insert into kategoriler(kategori_adi) values('" + kategori.Text + "')";
            MySqlCommand kmt = new MySqlCommand(komut, baglanti);
            kmt.ExecuteNonQuery();

            MessageBox.Show("Kategori Eklendi");
            kate_islem_listele();
            baglanti.Close();
            kategori.Text = "";
        }

        private void kate_islem_sil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (kate_islem_grid.Rows.Count == 0)
            {
                MessageBox.Show("Kayýt Bulunamadý");
            }
            else if (kate_islem_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seçim Yapýnýz");
            }
            else
            {
                string sql = "delete from kategoriler where kategori_id = '" + kate_islem_grid.CurrentRow.Cells[0].Value.ToString() + "' ";
                MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
                silinecek.ExecuteNonQuery();
                kate_islem_listele();
            }
            baglanti.Close();
            kategori.Text = "";
        }


        private void kate_islem_guncel_Click(object sender, EventArgs e)
        {
            if (kategori.Text == "")
            {
                MessageBox.Show("Lütfen Deðer Giriniz");
            }
            else
            {
                baglanti.Open();
                string komut = "UPDATE kategoriler SET kategori_adi='" + kategori.Text + "' where kategori_id='" + kate_islem_grid.CurrentRow.Cells[0].Value.ToString() + "'";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();

                MessageBox.Show("Kategori Bilgisi Güncellendi");
                kate_islem_listele();
                baglanti.Close();
                kategori.Text = "";
            }


        }

        private void masa_islem_ekle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string komut = "insert into masa(masa_adi) values('" + masa.Text + "')";
            MySqlCommand kmt = new MySqlCommand(komut, baglanti);
            kmt.ExecuteNonQuery();

            MessageBox.Show("Masa Eklendi");
            masa_islem_listele();
            baglanti.Close();
            masa.Text = "";
        }

        private void masa_islem_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //masa.Text = masa_islem_grid.Rows[e.RowIndex].Cells[1].Value.ToString();
            masa.Text = masa_islem_grid.CurrentRow.Cells[1].Value.ToString();
        }

        private void masa_islem_sil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sql = "delete from masa where masa_id = '" + masa_islem_grid.CurrentRow.Cells[0].Value.ToString() + "' ";
            MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
            silinecek.ExecuteNonQuery();
            masa_islem_listele();

            string sifir_id = "ALTER TABLE masa DROP masa_id;ALTER TABLE masa ADD masa_id int not null auto_increment primary key first;";
            MySqlCommand sifirla_id = new MySqlCommand(sifir_id, baglanti);
            sifirla_id.ExecuteNonQuery();

            baglanti.Close();
            masa.Text = "";
        }

        private void masa_islem_guncel_Click(object sender, EventArgs e)
        {
            if (masa.Text == "")
            {
                MessageBox.Show("Lütfen Deðer Giriniz");
            }
            else
            {
                baglanti.Open();
                string komut = "UPDATE masa SET masa_adi='" + masa.Text + "' where masa_id='" + masa_islem_grid.CurrentRow.Cells[0].Value.ToString() + "'";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();

                MessageBox.Show("Masa Bilgisi Güncellendi");
                masa_islem_listele();
                baglanti.Close();
                masa.Text = "";
            }
        }

        private void kate_islem_grid_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //kategori.Text = kate_islem_grid.Rows[e.RowIndex].Cells[1].Value.ToString();
            kategori.Text = kate_islem_grid.CurrentRow.Cells[1].Value.ToString();
        }

        private void urun_isl_kate_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //urun_isl_kategori.Text = urun_isl_kate_grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            urun_isl_kategori.Text = urun_isl_kate_grid.CurrentRow.Cells[0].Value.ToString();
        }

        private void urun_isl_urun_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*urun_isl_ad.Text = urun_isl_urun_grid.Rows[e.RowIndex].Cells[2].Value.ToString();
            urun_isl_fiyat.Text = urun_isl_urun_grid.Rows[e.RowIndex].Cells[3].Value.ToString();
            urun_isl_kategori.Text = urun_isl_urun_grid.Rows[e.RowIndex].Cells[1].Value.ToString();*/

            urun_isl_ad.Text = urun_isl_urun_grid.CurrentRow.Cells[2].Value.ToString();
            urun_isl_fiyat.Text= urun_isl_urun_grid.CurrentRow.Cells[3].Value.ToString();
            urun_isl_kategori.Text= urun_isl_urun_grid.CurrentRow.Cells[1].Value.ToString();
        }

        private void urun_islem_ekle_Click(object sender, EventArgs e)
        {
            //virgüllü giriþ



            if (urun_isl_ad.Text == "" || urun_isl_fiyat.Text == "" || urun_isl_kategori.Text == "")
            {
                MessageBox.Show("Lütfen Alanlarý Doldurunuz");
            }
            else
            {
                baglanti.Open();
                float num = float.Parse(urun_isl_fiyat.Text);
                string urun_fiyat_float = num.ToString().Replace(',', '.');
                string komut = "insert into urunler(kategori_id,urun_adi,fiyat) values('" + urun_isl_kategori.Text + "','" + urun_isl_ad.Text + "','" + urun_fiyat_float + "')";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();
                MessageBox.Show("Ürün Eklendi");

                urun_islem_kate_listele();
                urun_islem_urun_listele();

                baglanti.Close();

                urun_isl_kategori.Text = "";
                urun_isl_ad.Text = "";
                urun_isl_fiyat.Text = "";



            }
        }

        private void urun_islem_sil_Click(object sender, EventArgs e)
        {

            baglanti.Open();

            if (urun_isl_urun_grid.Rows.Count == 0)
            {
                MessageBox.Show("Kayýt Bulunamadý");
            }

            else
            {
                string sql = "delete from urunler where urun_id = '" + urun_isl_urun_grid.CurrentRow.Cells[0].Value.ToString() + "' ";
                MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
                silinecek.ExecuteNonQuery();


                string sifir_id = "ALTER TABLE urunler DROP urun_id;ALTER TABLE urunler ADD urun_id int not null auto_increment primary key first;";
                MySqlCommand sifirla_id = new MySqlCommand(sifir_id, baglanti);
                sifirla_id.ExecuteNonQuery();

                urun_islem_urun_listele();
                urun_islem_kate_listele();
            }
            baglanti.Close();
            urun_isl_kategori.Text = "";
            urun_isl_ad.Text = "";
            urun_isl_fiyat.Text = "";
        }

        private void urun_islem_guncel_Click(object sender, EventArgs e)
        {
            //virgüllü giriþ


            if (urun_isl_ad.Text == "" || urun_isl_fiyat.Text == "" || urun_isl_kategori.Text == "")
            {
                MessageBox.Show("Lütfen Deðer Giriniz");
            }
            else
            {
                float num = float.Parse(urun_isl_fiyat.Text);
                string urun_fiyat_float = num.ToString().Replace(',', '.');
                baglanti.Open();
                string komut = "UPDATE urunler SET kategori_id='" + urun_isl_kategori.Text + "',fiyat='" + urun_fiyat_float + "',urun_adi='" + urun_isl_ad.Text + "' where urun_id='" + urun_isl_urun_grid.CurrentRow.Cells[0].Value.ToString() + "'";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();
                MessageBox.Show("Ürün Bilgileri Güncellendi");

                urun_islem_urun_listele();
                urun_islem_kate_listele();

                baglanti.Close();
                urun_isl_kategori.Text = "";
                urun_isl_ad.Text = "";
                urun_isl_fiyat.Text = "";
                

            }
        }

        private void urun_ara_tbx_TextChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            string getir = "SELECT urun_id as Ürün_No,kategori_id as Kategori_No,urun_adi as Ürün,fiyat as Fiyat  from urunler WHERE urun_adi LIKE '%" + urun_ara_tbx.Text + "%'";
            MySqlCommand calis = new MySqlCommand(getir, baglanti);
            MySqlDataAdapter tbl = new MySqlDataAdapter(calis);
            DataTable urun_liste = new DataTable();
            tbl.Fill(urun_liste);
            urun_isl_urun_grid.DataSource = urun_liste;
            baglanti.Close();
        }

        private void satis_urun_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            satis_urun_ad.Text = satis_urun_grid.CurrentRow.Cells[0].Value.ToString();
            satis_urun_fiyat.Text = satis_urun_grid.CurrentRow.Cells[1].Value.ToString();
            if (satis_urun_ad.Text == satis_urun_grid.CurrentRow.Cells[0].Value.ToString())
            {
                satis_urun_adet.Value = satis_urun_adet.Value + 1;
            }
            else
            {
                satis_urun_adet.Value = 0;
                satis_urun_adet.Text = "0";
            }
        }

        private void satis_kategori_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            baglanti.Open();
            int secili = satis_kategori_grid.SelectedCells[0].RowIndex;
            string id = satis_kategori_grid.Rows[secili].Cells[0].Value.ToString();
            string kayit = "SELECT urun_adi as Urun,fiyat as Fiyat from urunler where kategori_id=" + id;
            MySqlCommand komut = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            satis_urun_grid.DataSource = dt;
            baglanti.Close();
        }

        private void satis_ekle_Click(object sender, EventArgs e)
        {
            if (satis_urun_fiyat.Text != "" && satis_urun_adet.Text != "")
            {
                float fiyat_cek = float.Parse(satis_urun_fiyat.Text);
                float adet_cek = float.Parse(satis_urun_adet.Text);
                float hesapla = fiyat_cek * adet_cek;
                satis_urun_toplam.Text = hesapla.ToString();

            }
            else
            {
                MessageBox.Show("Seçimleri Kontrol Ediniz");
            }


            float ur_fyt = float.Parse(satis_urun_fiyat.Text);
            string urun_fiyat_float = ur_fyt.ToString().Replace(',', '.');

            float ur_adet = float.Parse(satis_urun_adet.Text);
            string urun_adet_float = ur_adet.ToString().Replace(',', '.');

            float ur_toplam = float.Parse(satis_urun_toplam.Text);
            string urun_toplam_float = ur_toplam.ToString().Replace(',', '.');


            DateTime bugun = DateTime.Now;
            string anlik_tarih = bugun.ToString("yyyy-MM-dd");
            string anlik_saat = bugun.ToLongTimeString();
            if (ur_toplam !=0 && ur_adet!=0 && ur_adet != 0 && satis_per_cmb.Text != "PERSONEL SEÇ" && satis_masa_cmb.Text != "MASA SEÇ")
            {
                baglanti.Open();

                string komut = "insert into gecici_kayit(masa,personel,urun,fiyat,adet,toplam,tarih,saat) values('" + satis_masa_cmb.Text + "','" + satis_per_cmb.Text + "','" + satis_urun_ad.Text + "','" + urun_fiyat_float + "','" + urun_adet_float + "','" + urun_toplam_float + "','" + anlik_tarih + "','" + anlik_saat + "')";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();
                satis_urun_ad.Text = "";
                satis_urun_adet.Text = "0";
                satis_urun_fiyat.Text = "0";
                satis_urun_toplam.Text = "0";

                baglanti.Close();

                baglanti.Open();
                string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat  from gecici_kayit ";
                MySqlCommand list = new MySqlCommand(kayit, baglanti);
                MySqlDataAdapter da = new MySqlDataAdapter(list);
                DataTable dt = new DataTable();
                da.Fill(dt);
                satis_gecici_grid.DataSource = dt;
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Seçimleri Kontrol Ediniz");
            }

        }
        

        private void satis_temizle_Click(object sender, EventArgs e)
        {
            satis_urun_ad.Text = "";
            satis_urun_adet.Text ="0";
            satis_urun_fiyat.Text = "0";
            satis_urun_toplam.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (satis_urun_fiyat.Text != "" && satis_urun_adet.Text != "")
            {
                float fiyat_cek = float.Parse(satis_urun_fiyat.Text);
                float adet_cek = float.Parse(satis_urun_adet.Text);
                float hesapla = fiyat_cek * adet_cek;
                satis_urun_toplam.Text = hesapla.ToString();

            }
            else
            {
                MessageBox.Show("Seçimleri Kontrol Ediniz");
            }


        }

        private void satis_masa_hesap_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat  from gecici_kayit where masa='" + satis_masa_hesap_cmb.Text + "'";
            MySqlCommand list = new MySqlCommand(kayit, baglanti);
            MySqlDataAdapter da = new MySqlDataAdapter(list);
            DataTable dt = new DataTable();
            da.Fill(dt);
            satis_gecici_grid.DataSource = dt;
            baglanti.Close();

            hesap_toplami();


        }

        private void satis_tumsatislar_Click(object sender, EventArgs e)
        {
            
                baglanti.Open();
                string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat from gecici_kayit";
                MySqlCommand list = new MySqlCommand(kayit, baglanti);
                MySqlDataAdapter da = new MySqlDataAdapter(list);
                DataTable dt = new DataTable();
                da.Fill(dt);
                satis_gecici_grid.DataSource = dt;
                baglanti.Close();

                satis_masa_hesap_cmb.Text = "";
                label13.Text = "";
            /*
            baglanti.Open();
            string masa_hesap = "SELECT DISTINCT masa FROM gecici_kayit";
            MySqlCommand hesap_getir = new MySqlCommand(masa_hesap, baglanti);
            MySqlDataReader hesap_cek = hesap_getir.ExecuteReader();
            while (hesap_cek.Read())
            {
                satis_masa_hesap_cmb.Items.Add(hesap_cek.GetString("masa"));

            }
            baglanti.Close();
            for (int i = 1; i < 11; i++)
            {
                Button btn = new Button();
                btn.Name = i.ToString();
                btn.Text = i.ToString();
                btn.Width = 80;
                btn.Height = 50;
                //this.Controls.Add(btn); //bu þekilde form'a ekleme yapýlýrsa tüm butonlar üst üste çýkacaktýr
                panel1.Controls.Add(btn); //oluþan butonlar üstüste binmez
            }

            */


        }

        private void satis_masa_kapat_Click(object sender, EventArgs e)
        {

            if (satis_masa_hesap_cmb.Text == "")
            {
                MessageBox.Show("Masa Seçimi Yapýnýz");
            }
            else if (satis_gecici_grid.Rows.Count==0)
            {
                MessageBox.Show("Kayýt Bulunamadý");
            }
            else
            {
                DateTime bugun = DateTime.Now;
                string anlik_tarih = bugun.ToString("yyyy-MM-dd");
                string anlik_saat = bugun.ToLongTimeString();

                baglanti.Open();
                string anlik_tar = "UPDATE gecici_kayit SET tarih='" + anlik_tarih + "',saat='" + anlik_saat + "' where masa='" + satis_masa_hesap_cmb.Text + "'";
                MySqlCommand tarih_cek = new MySqlCommand(anlik_tar, baglanti);
                tarih_cek.ExecuteNonQuery();

                string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat from gecici_kayit where masa='" + satis_masa_hesap_cmb.Text + "'";
                MySqlCommand list = new MySqlCommand(kayit, baglanti);
                MySqlDataAdapter da = new MySqlDataAdapter(list);
                DataTable dt = new DataTable();
                da.Fill(dt);
                satis_gecici_grid.DataSource = dt;

                baglanti.Close();

                baglanti.Open();
                for (int i = 0; i < satis_gecici_grid.Rows.Count; i++)
                {
                    string iki = satis_gecici_grid.Rows[i].Cells["Personel"].Value.ToString();
                    string uc = satis_gecici_grid.Rows[i].Cells["Ürün"].Value.ToString();
                    string dort = satis_gecici_grid.Rows[i].Cells["Fiyat"].Value.ToString();
                    string bes = satis_gecici_grid.Rows[i].Cells["Adet"].Value.ToString();
                    string alti = satis_gecici_grid.Rows[i].Cells["Toplam_Fiyat"].Value.ToString();
                    string yedi = satis_gecici_grid.Rows[i].Cells["Tarih"].Value.ToString();
                    string sekiz = satis_gecici_grid.Rows[i].Cells["Saat"].Value.ToString();

                    float fiyat = float.Parse(dort);
                    string fiyat_float = fiyat.ToString().Replace(',', '.');

                    float adet = float.Parse(bes);
                    string adet_float = adet.ToString().Replace(',', '.');

                    float toplam = float.Parse(alti);
                    string toplam_float = toplam.ToString().Replace(',', '.');


                    string komut = "insert into kayitlar(kayitlar_personel,kayitlar_urun,kayitlar_fiyat,kayitlar_adet,kayitlar_toplam,kayitlar_tarih,kayitlar_saat) values('" + iki + "','" + uc + "','" + fiyat_float + "','" + adet_float + "','" + toplam_float + "','" + anlik_tarih + "','" + sekiz + "')";
                    MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                    kmt.ExecuteNonQuery();
                    



                }
                yazdir_Click(sender, e);    
                MessageBox.Show("Masa Hesabý Alýndý");
                string sql = "delete from gecici_kayit where masa = '" + satis_masa_hesap_cmb.Text + "' ";
                MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
                silinecek.ExecuteNonQuery();
                

                string sifir_id = "ALTER TABLE gecici_kayit DROP gecici_id;ALTER TABLE gecici_kayit ADD gecici_id int not null auto_increment primary key first;";
                MySqlCommand sifirla_id = new MySqlCommand(sifir_id, baglanti);
                sifirla_id.ExecuteNonQuery();




                gecici_kayit_listele();
                satis_masa_hesap_cmb.Text = "";
                label13.Text = "0 TL";
                baglanti.Close();

            }

            

        }

        private void satis_siparis_iptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            if (satis_gecici_grid.Rows.Count == 0)
            {
                MessageBox.Show("Kayýt Bulunamadý");
            }
            else
            {


                string sql = "delete from gecici_kayit where gecici_id = '" + satis_gecici_grid.CurrentRow.Cells[0].Value.ToString() + "' ";
                MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
                silinecek.ExecuteNonQuery();

                string sifir_id = "ALTER TABLE gecici_kayit DROP gecici_id;ALTER TABLE gecici_kayit ADD gecici_id int not null auto_increment primary key first;";
                MySqlCommand sifirla_id = new MySqlCommand(sifir_id, baglanti);
                sifirla_id.ExecuteNonQuery();
                if(satis_masa_hesap_cmb.Text != "")
                {
                    string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat from gecici_kayit where masa='" + satis_masa_hesap_cmb.Text + "'";
                    MySqlCommand list = new MySqlCommand(kayit, baglanti);
                    MySqlDataAdapter da = new MySqlDataAdapter(list);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    satis_gecici_grid.DataSource = dt;
                    hesap_toplami();
                }
                else
                {
                    string kayit = "SELECT gecici_id as Sýra,masa as Masa_No,personel as Personel,urun as Ürün,fiyat as Fiyat,adet as Adet,toplam as Toplam_Fiyat,tarih as Tarih,saat as Saat from gecici_kayit";
                    MySqlCommand list = new MySqlCommand(kayit, baglanti);
                    MySqlDataAdapter da = new MySqlDataAdapter(list);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    satis_gecici_grid.DataSource = dt;
                }

            }
            
            baglanti.Close();
        }

        private void personel_ekle_Click(object sender, EventArgs e)
        {
            if (personel_tc.Text == "" || personel_ad.Text == "" || personel_soyad.Text == "" || personel_adres.Text == "" || personel_dog_tar.Text == "" || personel_cep_bir.Text == "" || personel_email.Text == "" || kullanici_adi.Text == "" || sifre.Text == "")
            {
                MessageBox.Show("Lütfen Alanlarý Doldurunuz");
            }
            else
            {
                baglanti.Open();
                string komut = "insert into personel(personel_tc,personel_ad,personel_soyad,personel_adres,personel_dog_tar,personel_cep_bir,personel_cep_iki,personel_email,kullanici_adi,sifre,yetki) values('" + personel_tc.Text + "','" + personel_ad.Text + "','" + personel_soyad.Text + "','" + personel_adres.Text + "','" + personel_dog_tar.Text + "','" + personel_cep_bir.Text + "','" + personel_cep_iki.Text + "','" + personel_email.Text + "','" + kullanici_adi.Text + "','" + sifre.Text + "','" + yeki_cmb.Text + "')";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();
                MessageBox.Show("Kayýt Eklendi");

                personel_listele();


                baglanti.Close();
                personel_ad.Text = "";
                personel_soyad.Text = "";
                personel_email.Text = "";
                personel_dog_tar.Text = "";
                personel_cep_iki.Text = "";
                personel_cep_bir.Text = "";
                personel_adres.Text = "";
                personel_tc.Text = "";
                kullanici_adi.Text = "";
                sifre.Text = "";
            }
        }

        private void personel_guncelle_Click(object sender, EventArgs e)
        {
            if (personel_tc.Text == "" || personel_ad.Text == "" || personel_soyad.Text == "" || personel_adres.Text == "" || personel_dog_tar.Text == "" || personel_cep_bir.Text == "" || personel_email.Text == "" || kullanici_adi.Text == "" || sifre.Text == "")
            {
                MessageBox.Show("Lütfen Deðer Giriniz");
            }
            else
            {
                baglanti.Open();
                string komut = "UPDATE personel SET personel_tc='" + personel_tc.Text + "',personel_ad='" + personel_ad.Text + "',personel_soyad='" + personel_soyad.Text + "',personel_adres='" + personel_adres.Text + "',personel_dog_tar='" + personel_dog_tar.Text + "',personel_cep_bir='" + personel_cep_bir.Text + "',personel_cep_iki='" + personel_cep_iki.Text + "',personel_email='" + personel_email.Text + "',kullanici_adi='" + kullanici_adi.Text + "',sifre='" + sifre.Text + "',yetki='" + yeki_cmb.Text + "' where personel_id='" + personel_grid.CurrentRow.Cells[0].Value.ToString() + "'";
                MySqlCommand kmt = new MySqlCommand(komut, baglanti);
                kmt.ExecuteNonQuery();
                MessageBox.Show("Ürün Bilgileri Güncellendi");
                personel_listele();

                baglanti.Close();
                personel_ad.Text = "";
                personel_soyad.Text = "";
                personel_email.Text = "";
                personel_dog_tar.Text = "";
                personel_cep_iki.Text = "";
                personel_cep_bir.Text = "";
                personel_adres.Text = "";
                personel_tc.Text = "";
                kullanici_adi.Text = "";
                sifre.Text = "";

            }
        }

        private void personel_sil_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            if (personel_grid.Rows.Count == 0)
            {
                MessageBox.Show("Kayýt Bulunamadý");
            }
            else if (personel_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seçim Yapýnýz");
            }
            else
            {
                string sql = "delete from personel where personel_id = '" + personel_grid.CurrentRow.Cells[0].Value.ToString() + "' ";
                MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
                silinecek.ExecuteNonQuery();

                string sifir_id = "ALTER TABLE personel DROP personel_id;ALTER TABLE personel ADD personel_id int not null auto_increment primary key first;";
                MySqlCommand sifirla_id = new MySqlCommand(sifir_id, baglanti);
                sifirla_id.ExecuteNonQuery();


                personel_listele();
            }



            baglanti.Close();
        }

        private void personel_ara_tbx_TextChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            string getir = "SELECT personel_id as Sýra,personel_tc as TCK_No,personel_ad as Ad,personel_soyad as Soyad,personel_adres as Adres,personel_dog_tar as Doðum_Tarihi,personel_cep_bir as Tel_1,personel_cep_iki as Tel_2,personel_email as E_Mail,kullanici_adi as Kullanýcý_Adý,sifre as Þifre,yetki as Yetki from personel WHERE personel_soyad LIKE '%" + personel_ara_tbx.Text + "%'";
            MySqlCommand calis = new MySqlCommand(getir, baglanti);
            MySqlDataAdapter tbl = new MySqlDataAdapter(calis);
            DataTable urun_liste = new DataTable();
            tbl.Fill(urun_liste);
            personel_grid.DataSource = urun_liste;
            baglanti.Close();
        }

        private void personel_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            personel_tc.Text = personel_grid.CurrentRow.Cells[1].Value.ToString();
            personel_ad.Text = personel_grid.CurrentRow.Cells[2].Value.ToString();
            personel_soyad.Text = personel_grid.CurrentRow.Cells[3].Value.ToString();
            personel_adres.Text = personel_grid.CurrentRow.Cells[4].Value.ToString();
            personel_dog_tar.Text = personel_grid.CurrentRow.Cells[5].Value.ToString();
            personel_cep_bir.Text = personel_grid.CurrentRow.Cells[6].Value.ToString();
            personel_cep_iki.Text = personel_grid.CurrentRow.Cells[7].Value.ToString();
            personel_email.Text = personel_grid.CurrentRow.Cells[8].Value.ToString();
            kullanici_adi.Text = personel_grid.CurrentRow.Cells[9].Value.ToString();
            sifre.Text = personel_grid.CurrentRow.Cells[10].Value.ToString();
        }

        private void yazdir_Click(object sender, EventArgs e)
        {
            
            
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            pd.Print();
        }




        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            DateTime bugun = DateTime.Now;
            string anlik_tarih = bugun.ToString();
            try
            {
                Font font = new Font("Arial", 7);
                Font font1 = new Font("Calibri", 12);
                Font font2 = new Font("Arial BLack", 7);
                SolidBrush firca = new SolidBrush(Color.Black);
                
                e.Graphics.DrawString("Tarih   : " + bugun, font, firca, 0, 10);
                e.Graphics.DrawString("MEÞHUR ADIYAMAN ÇÝÐKÖFTECÝSÝ", font2, firca, 0, 35);
                e.Graphics.DrawString("--------------------------------------------------------------------------", font, firca, 0, 50);
                e.Graphics.DrawString("Ürün", font, firca, 0, 65);
                e.Graphics.DrawString("Fiyat", font, firca, 70, 65);
                e.Graphics.DrawString("Adet", font, firca, 115, 65);
                e.Graphics.DrawString("Toplam", font, firca, 140, 65);

                int y = 80;
                for (int i = 0; i < satis_gecici_grid.Rows.Count  ; i++)
                {
                    e.Graphics.DrawString(satis_gecici_grid.Rows[i].Cells["Ürün"].Value.ToString(), font, firca, 0, y);
                    e.Graphics.DrawString(satis_gecici_grid.Rows[i].Cells["Fiyat"].Value.ToString() + " TL", font, firca, 70, y);
                    e.Graphics.DrawString("x" + satis_gecici_grid.Rows[i].Cells["Adet"].Value.ToString(), font, firca, 115, y);
                    e.Graphics.DrawString(satis_gecici_grid.Rows[i].Cells["Toplam_Fiyat"].Value.ToString() + " TL", font, firca, 140, y);
                    y = y + 15;
                   
                    e.Graphics.DrawString("--------------------------------------------------------------------------", font, firca, 0, y+5);
                    e.Graphics.DrawString("TOPLAM : " + label13.Text, font1, firca, 0, y + 35);
                    e.Graphics.DrawString("AFÝYET OLSUN.ÝYÝ GÜNLER", font1, firca, 0, y + 100);

                }
            }
            catch
            {

            }
        }

        private void tum_satis_Click(object sender, EventArgs e)
        {
            string ilk_tarih=satislar_ilk_tarih.Value.ToString("yyyy-MM-dd");
            string son_tarih=satislar_ikinci_tarih.Value.ToString("yyyy-MM-dd");
            satis_bilg_grid.DataSource = null;
            baglanti.Open();
            string getir = "SELECT kayitlar_id as Sýra,kayitlar_personel as Personel,kayitlar_urun as Ürün,kayitlar_fiyat as Fiyat,kayitlar_adet as Adet,kayitlar_toplam as Toplam,kayitlar_tarih as Tarih,kayitlar_saat as Saat  from kayitlar WHERE kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih + "'";
            MySqlCommand calis = new MySqlCommand(getir, baglanti);
            MySqlDataAdapter tbl = new MySqlDataAdapter(calis);
            DataTable urun_liste = new DataTable();
            tbl.Fill(urun_liste);
            satis_bilg_grid.DataSource = urun_liste;
            baglanti.Close();


            baglanti.Open();
            string satilan_adet = "SELECT SUM(kayitlar_adet),SUM(kayitlar_toplam),kayitlar_tarih as Tarih,kayitlar_saat as Saat from kayitlar WHERE kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih + "'";
            MySqlCommand cek_satilan_adet = new MySqlCommand(satilan_adet, baglanti);
            MySqlDataReader read_satilan_adet = cek_satilan_adet.ExecuteReader();
            string tum_satis_adet;
            string tum_satis_toplm;
            while (read_satilan_adet.Read())
            {
                try
                {
                    tum_satis_adet = read_satilan_adet.GetString("SUM(kayitlar_adet)");
                    tum_satis_toplm = read_satilan_adet.GetString("SUM(kayitlar_toplam)");
                    label34.Text = tum_satis_adet;
                    label35.Text = tum_satis_toplm;
                }
                catch (Exception)
                {
                    MessageBox.Show("Tarihe Ait Satýþ Kaydý Yoktur");
                    label34.Text = "";
                    label35.Text = "";
                }

            }
            baglanti.Close();
        }

        private void personel_satis_Click(object sender, EventArgs e)
        {
            string ilk_tarih = satislar_ilk_tarih.Value.ToString("yyyy-MM-dd");
            string son_tarih = satislar_ikinci_tarih.Value.ToString("yyyy-MM-dd");

            satis_bilg_grid.DataSource = null;
            if (satislar_personel_cmb.Text == "PERSONEL SEÇ")
            {
                MessageBox.Show("Lütfen Personel Seçimi Yapýnýz");
            }

            else
            { 
            baglanti.Open();
            string per_urun_satis = "SELECT kayitlar_personel as Personel, kayitlar_urun as Ürün,SUM(kayitlar_adet) as Satýlan_Adet FROM kayitlar WHERE (kayitlar_personel='" + satislar_personel_cmb.Text + "') AND (kayitlar_tarih BETWEEN '" + ilk_tarih+ "' AND '" + son_tarih + "') GROUP BY kayitlar_urun,kayitlar_personel";
            MySqlCommand per_urun_bazli = new MySqlCommand(per_urun_satis, baglanti);
            MySqlDataAdapter per_urun_lst = new MySqlDataAdapter(per_urun_bazli);
            DataTable per_ur_lst = new DataTable();
            per_urun_lst.Fill(per_ur_lst);
            satis_bilg_grid.DataSource = per_ur_lst;
            baglanti.Close();


            baglanti.Open();
            string satilan_adet = "SELECT SUM(kayitlar_adet),SUM(kayitlar_toplam) from kayitlar WHERE (kayitlar_personel='" + satislar_personel_cmb.Text + "') AND (kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih + "')";
            MySqlCommand cek_satilan_adet = new MySqlCommand(satilan_adet, baglanti);
            MySqlDataReader read_satilan_adet = cek_satilan_adet.ExecuteReader();
            string satis_per_adet;
            string satis_per_toplm;
            while (read_satilan_adet.Read())
            {
                try
                {
                        satis_per_adet = read_satilan_adet.GetString("SUM(kayitlar_adet)");
                        satis_per_toplm = read_satilan_adet.GetString("SUM(kayitlar_toplam)");
                        label34.Text = satis_per_adet;
                        label35.Text = satis_per_toplm;
                    }
                catch(Exception)
                {
                        MessageBox.Show("Personele Ait Satýþ Kaydý Yoktur");
                        label34.Text = "";
                        label35.Text = "";
                }
            

                
            }
            baglanti.Close();

            }
        }

        private void tarih_urun_satis_Click(object sender, EventArgs e)
        {
            string ilk_tarih = satislar_ilk_tarih.Value.ToString("yyyy-MM-dd");
            string son_tarih = satislar_ikinci_tarih.Value.ToString("yyyy-MM-dd");
            satis_bilg_grid.DataSource = null;
                baglanti.Open();
                string per_urun_satis = "SELECT kayitlar_urun as Ürün,SUM(kayitlar_adet) as Satýlan_Adet FROM kayitlar WHERE kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih + "' GROUP BY kayitlar_urun";
                MySqlCommand per_urun_bazli = new MySqlCommand(per_urun_satis, baglanti);
                MySqlDataAdapter per_urun_lst = new MySqlDataAdapter(per_urun_bazli);
                DataTable per_ur_lst = new DataTable();
                per_urun_lst.Fill(per_ur_lst);
                satis_bilg_grid.DataSource = per_ur_lst;
                baglanti.Close();


                baglanti.Open();
                string satilan_adet = "SELECT SUM(kayitlar_adet),SUM(kayitlar_toplam) from kayitlar WHERE kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih + "'";
                MySqlCommand cek_satilan_adet = new MySqlCommand(satilan_adet, baglanti);
                MySqlDataReader read_satilan_adet = cek_satilan_adet.ExecuteReader();
                string satis_per_adet;
                string satis_per_toplm;
                while (read_satilan_adet.Read())
                {
                    try
                    {
                        satis_per_adet = read_satilan_adet.GetString("SUM(kayitlar_adet)");
                        satis_per_toplm = read_satilan_adet.GetString("SUM(kayitlar_toplam)");
                        label34.Text = satis_per_adet;
                        label35.Text = satis_per_toplm;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Tarihe Ait Satýþ Kaydý Yoktur");
                        label34.Text = "";
                        label35.Text = "";
                    }



                }
                baglanti.Close();
        }

        private void gun_gun_satis_Click(object sender, EventArgs e)
        {
            string ilk_tarih = satislar_ilk_tarih.Value.ToString("yyyy-MM-dd");
            string son_tarih = satislar_ikinci_tarih.Value.ToString("yyyy-MM-dd");
            satis_bilg_grid.DataSource = null;
            baglanti.Open();
            string per_urun_satis = "SELECT SUM(kayitlar_adet) as Satýlan_Adet,SUM(kayitlar_toplam) as Toplam,kayitlar_tarih as Tarih FROM kayitlar WHERE kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih + "' GROUP BY kayitlar_tarih";
            MySqlCommand per_urun_bazli = new MySqlCommand(per_urun_satis, baglanti);
            MySqlDataAdapter per_urun_lst = new MySqlDataAdapter(per_urun_bazli);
            DataTable per_ur_lst = new DataTable();
            per_urun_lst.Fill(per_ur_lst);
            satis_bilg_grid.DataSource = per_ur_lst;
            baglanti.Close();

            baglanti.Open();
            string satilan_adet = "SELECT SUM(kayitlar_adet),SUM(kayitlar_toplam) from kayitlar WHERE kayitlar_tarih BETWEEN '" + ilk_tarih + "' AND '" + son_tarih  + "'";
            MySqlCommand cek_satilan_adet = new MySqlCommand(satilan_adet, baglanti);
            MySqlDataReader read_satilan_adet = cek_satilan_adet.ExecuteReader();
            string satis_per_adet;
            string satis_per_toplm;
            while (read_satilan_adet.Read())
            {
                try
                {
                    satis_per_adet = read_satilan_adet.GetString("SUM(kayitlar_adet)");
                    satis_per_toplm = read_satilan_adet.GetString("SUM(kayitlar_toplam)");
                    label34.Text = satis_per_adet;
                    label35.Text = satis_per_toplm;
                }
                catch (Exception)
                {
                    MessageBox.Show("Tarihe Ait Satýþ Kaydý Yoktur");
                    label34.Text = "";
                    label35.Text = "";
                }



            }
            baglanti.Close();
        }

        private void rapor_sistemi_Click(object sender, EventArgs e)
        {

        }

        private void satis_urun_adet_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            satis_masa_hesap_cmb.Items.Clear();
            baglanti.Open();
            //satýþ sayfasý combobox masa çek
            string masacek = "SELECT DISTINCT masa FROM gecici_kayit";
            MySqlCommand masagetir = new MySqlCommand(masacek, baglanti);
            MySqlDataReader read = masagetir.ExecuteReader();
            while (read.Read())
            {
                satis_masa_hesap_cmb.Items.Add(read.GetString("masa"));
            }
            baglanti.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            string getir = "SELECT kayitlar_id as Sýra,kayitlar_personel as Personel,kayitlar_urun as Ürün,kayitlar_fiyat as Fiyat,kayitlar_adet as Adet,kayitlar_toplam as Toplam,kayitlar_tarih as Tarih from kayitlar WHERE kayitlar_tarih LIKE '%" + kayitlar_ara.Text + "%'";
            MySqlCommand calis = new MySqlCommand(getir, baglanti);
            MySqlDataAdapter tbl = new MySqlDataAdapter(calis);
            DataTable urun_liste = new DataTable();
            tbl.Fill(urun_liste);
            satis_bilg_grid.DataSource = urun_liste;
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (satis_bilg_grid.Rows.Count == 0)
            {
                MessageBox.Show("Kayýt Bulunamadý");
            }
            else
            {

                baglanti.Open();
                string sql = "delete from kayitlar where kayitlar_id = '" + satis_bilg_grid.CurrentRow.Cells[0].Value.ToString() + "' ";
                MySqlCommand silinecek = new MySqlCommand(sql, baglanti);
                silinecek.ExecuteNonQuery();

                string sifir_id = "ALTER TABLE kayitlar DROP kayitlar_id;ALTER TABLE kayitlar ADD kayitlar_id int not null auto_increment primary key first;";
                MySqlCommand sifirla_id = new MySqlCommand(sifir_id, baglanti);
                sifirla_id.ExecuteNonQuery();

                
                if (kayitlar_ara.Text != "")
                {
                    string kayit = "SELECT kayitlar_id as Sýra,kayitlar_personel as Personel,kayitlar_urun as Ürün,kayitlar_fiyat as Fiyat,kayitlar_adet as Adet,kayitlar_toplam as Toplam,kayitlar_tarih as Tarih,kayitlar_saat as Saat from kayitlar where kayitlar_tarih='" + kayitlar_ara.Text + "'";
                    MySqlCommand list = new MySqlCommand(kayit, baglanti);
                    MySqlDataAdapter da = new MySqlDataAdapter(list);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    satis_bilg_grid.DataSource = dt;
                    
                }
                else
                {
                    string kayit = "SELECT kayitlar_id as Sýra,kayitlar_personel as Personel,kayitlar_urun as Ürün,kayitlar_fiyat as Fiyat,kayitlar_adet as Adet,kayitlar_toplam as Toplam,kayitlar_tarih as Tarih,kayitlar_saat as Saat from kayitlar";
                    MySqlCommand list = new MySqlCommand(kayit, baglanti);
                    MySqlDataAdapter da = new MySqlDataAdapter(list);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    satis_bilg_grid.DataSource = dt;
                }
                baglanti.Close();
            }

                
        }

        private void satis_urun_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    }
    
