using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Ajanda
{
    public partial class Form1 : Form
    {
        private SQLiteConnection baglanti;
        private SQLiteCommand komut;

        public Form1()
        {
            InitializeComponent();

            // SQLite veritabanı bağlantısı
            baglanti = new SQLiteConnection("Data Source=kullanicilar.db;Version=3;");
            baglanti.Open();

            // Kullanıcılar tablosu oluşturuluyor.
            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Kullanicilar (ID INTEGER PRIMARY KEY AUTOINCREMENT, KullaniciAdi TEXT, Sifre TEXT, Eposta TEXT)", baglanti);
            cmd.ExecuteNonQuery();
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            // Kullanıcı adı ve şifre kontrolü
            if (KullaniciGirisDogrula(kullaniciAdi, sifre))
            {
                MessageBox.Show("Giriş Başarılı!");
            }
            else
            {
                MessageBox.Show("Hatalı Kullanıcı Adı veya Şifre!");
            }
        }

        private bool KullaniciGirisDogrula(string kullaniciAdi, string sifre)
        {
            baglanti.Open();
            komut = baglanti.CreateCommand();
            komut.CommandText = "SELECT * FROM Kullanicilar WHERE KullaniciAdi=@KullaniciAdi";
            komut.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

            using (SQLiteDataReader okuyucu = komut.ExecuteReader())
            {
                if (okuyucu.Read())
                {
                    // Veritabanından çekilen hash
                    string hashliSifre = okuyucu["Sifre"].ToString();

                    // Kullanıcının girdiği şifreyi hash'le
                    string girilenSifreHash = HashPassword(sifre);

                    // Hash'leri karşılaştır
                    if (string.Equals(girilenSifreHash, hashliSifre))
                    {
                        // Giriş başarılı, kullanıcı bulundu
                        baglanti.Close();
                        return true;
                    }
                }
            }

            // Kullanıcı bulunamadı veya şifre uyuşmuyor
            baglanti.Close();
            return false;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
            formKayit formKayit = new formKayit();

            // Yeni formu göster
            formKayit.Show();
        }

        private void btnSifreniUnuttum_Click(object sender, EventArgs e)
        {
            // Şifre sıfırlama formunu göster
            SifreSifirlaForm sifreSifirlaForm = new SifreSifirlaForm();
            sifreSifirlaForm.Show();
        }
    }
}
