using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;

namespace Ajanda
{
    public partial class formKayit : Form
    {
        SQLiteConnection baglanti;

        public formKayit()
        {
            InitializeComponent();
            // SQLite veritabanı bağlantısı
            baglanti = new SQLiteConnection("Data Source=kullanicilar.db;Version=3;");
            baglanti.Open();

            // Kullanıcılar tablosu yoksa oluştur
            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Kullanicilar (ID INTEGER PRIMARY KEY AUTOINCREMENT, KullaniciAdi TEXT, Sifre TEXT, Eposta TEXT)", baglanti);
            cmd.ExecuteNonQuery();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                // MailAddress sınıfı kullanarak e-posta adresini kontrol et
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                // FormatException alınırsa e-posta geçerli değil
                return false;
            }
        }

        private bool IsStrongPassword(string password)
        {
            // En az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool KullaniciKayitOl(string kullaniciAdi, string sifre, string eposta)
        {
            // Kullanıcı adı ve eposta kontrolü
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Kullanicilar WHERE KullaniciAdi=@KullaniciAdi OR Eposta=@Eposta", baglanti);
            cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
            cmd.Parameters.AddWithValue("@Eposta", eposta);

            using (SQLiteDataReader okuyucu = cmd.ExecuteReader())
            {
                if (okuyucu.Read())
                {
                    // Kullanıcı adı veya eposta zaten kayıtlı
                    MessageBox.Show("Bu kullanıcı adı veya eposta adresi zaten kullanımda.");
                    return false;
                }
            }

            // E-posta geçerliliğini kontrol et
            if (!IsValidEmail(eposta))
            {
                MessageBox.Show("Geçersiz e-posta adresi!");
                return false;
            }

            // Şifre güçlü mü kontrol et
            if (!IsStrongPassword(sifre))
            {
                MessageBox.Show("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.");
                return false;
            }

            // Şifreyi hash'le
            string hashedPassword = HashPassword(sifre);

            // Kullanıcı kaydı yap
            cmd = new SQLiteCommand("INSERT INTO Kullanicilar (KullaniciAdi, Sifre, Eposta) VALUES (@kullaniciAdi, @sifre, @eposta)", baglanti);
            cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
            cmd.Parameters.AddWithValue("@sifre", hashedPassword);
            cmd.Parameters.AddWithValue("@eposta", eposta);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Kayıt başarıyla tamamlandı.");
            baglanti.Close();
            this.Close();
            return true;
        }

        private void btnKayitOl_Click_1(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;
            string sifreTekrar = txtSifreTekrar.Text;
            string eposta = txtEposta.Text;

            // Kullanıcı adı ve şifre boş olmamalı
            if (!string.IsNullOrEmpty(kullaniciAdi) && !string.IsNullOrEmpty(sifre))
            {
                // Şifrelerin eşleşip eşleşmediğini kontrol et
                if (sifre == sifreTekrar)
                {
                    // Veritabanına kaydet
                    KullaniciKayitOl(kullaniciAdi, sifre, eposta);
                }
                else
                {
                    MessageBox.Show("Şifreler eşleşmiyor!");
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!");
            }
        }
    }
}
