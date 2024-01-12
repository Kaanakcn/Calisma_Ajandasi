// SifreSifirlaForm.cs

using System;
using System.Data.SQLite;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Ajanda
{
    public partial class SifreSifirlaForm : Form
    {
        private string eposta;

        public SifreSifirlaForm()
        {
            InitializeComponent();
        }

        private bool EpostaKontrol(string eposta)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection("Data Source=kullanicilar.db;Version=3;"))
            {
                baglanti.Open();

                using (SQLiteCommand komut = baglanti.CreateCommand())
                {
                    komut.CommandText = "SELECT COUNT(*) FROM Kullanicilar WHERE Eposta=@eposta";
                    komut.Parameters.AddWithValue("@eposta", eposta);

                    int kullaniciSayisi = Convert.ToInt32(komut.ExecuteScalar());

                    baglanti.Close();

                    return kullaniciSayisi > 0;
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        }

        private bool SifreyiGuncelle(string eposta, string yeniSifre)
        {
            try
            {
                using (SQLiteConnection baglanti = new SQLiteConnection("Data Source=kullanicilar.db;Version=3;"))
                {
                    baglanti.Open();

                    using (SQLiteCommand komut = baglanti.CreateCommand())
                    {
                        // Parolanın hash değerini hesapla
                        string hashedPassword = HashPassword(yeniSifre);

                        komut.CommandText = "UPDATE Kullanicilar SET Sifre=@sifre WHERE Eposta=@eposta";
                        komut.Parameters.AddWithValue("@sifre", hashedPassword);
                        komut.Parameters.AddWithValue("@eposta", eposta);

                        komut.ExecuteNonQuery();
                    }

                    baglanti.Close();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void btnSifreSifirla_Click(object sender, EventArgs e)
        {
            eposta = txtEposta.Text;

            if (IsValidEmail(eposta))
            {
                if (EpostaKontrol(eposta))
                {
                    string yeniSifre = txtYeniSifre.Text;
                    string yeniSifreTekrar = txtYeniSifreTekrar.Text;

                    if (IsStrongPassword(yeniSifre) && yeniSifre == yeniSifreTekrar)
                    {
                        if (SifreyiGuncelle(eposta, yeniSifre))
                        {
                            MessageBox.Show("Şifre başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Şifre güncelleme sırasında bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Şifre güvenlik kurallarını karşılamıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Girilen e-posta adresi sistemde bulunmamaktadır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Geçersiz e-posta adresi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
