using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

public partial class Giris_Giris : System.Web.UI.Page
{
    //SQL Bağlantısı
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);

    string[] dizi;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            kullanici_resmi.ImageUrl = "/Desing/Default/images/sy_images/kullanici_giris.png";


            beni_hatirla();        
            //Ana sayfa wallpaper değişimi
            string klasoryolu = MapPath("/Desing/Default/images/sy_images/walpaper/");
            string klasorum = klasoryolu;
            string[] klasordekiler = Directory.GetFiles(klasorum, "*.jpg");

            if (klasordekiler.Count() > 0)
            {
                dizi = new string[klasordekiler.Count()];
                for (int i = 0; i < klasordekiler.Count(); i++)
                {
                    dizi[i] = Path.GetFileName(klasordekiler[i]);
                }
                Random sayi = new Random();
                int no = sayi.Next(0, dizi.Length);
                body.Attributes.Add("style", "background-image: url('/Desing/Default/images/sy_images/walpaper/" + dizi[no] + "'); background-size: cover; background - repeat: no - repeat; background - position: center center; ");
            }
            else
            {
                body.Attributes.Add("style", "background-image: url('/Desing/Default/images/sy_images/walpaper/0.jpg'); background-size: cover; background - repeat: no - repeat; background - position: center center; ");
            }
            //SON
        }
        catch (Exception)
        {
            body.Attributes.Add("style", "background-image: url('/Desing/Default/images/sy_images/walpaper/0.jpg'); background-size: cover; background - repeat: no - repeat; background - position: center center; ");
        }        
    }

    public void beni_hatirla()
    {
        try
        {
            //Enter Butonun Çalışması İçin Gerekli Kod.
            Page.Form.DefaultButton = kullanici_giris.UniqueID;

            //Eğer Kullanıcı Varsa Ana Sayfaya Yönlendir
            if ((Request.Cookies["RcEU"] != null))
            {
                Response.Redirect("/Anasayfa/");
            }


            //Eğer Resim Yüklü İse Varsa 
            if ((Request.Cookies["COF"] != null))
            {
                kullanici_resmi.ImageUrl = Sorgu.GetImageUrl2("/Data/personel_resim/kucuk_resim/" + Sorgu.Decrypt(Request.Cookies["COF"]["F"].ToString()));

                //Eğer Beni Hatırla Aktif İse
                if (Request.Cookies["COF"]["U"].ToString()!="")
                {
                    kullanici.Text = Sorgu.Decrypt(Request.Cookies["COF"]["U"].ToString());
                    benihatirla.Checked=true;
                }
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Profil resminiz yüklenemedi").ToString(), true);
        }
    }

    protected void kullanici_giris_Click(object sender, EventArgs e)
    {
        try
        {
        SqlDataAdapter komut = new SqlDataAdapter("select * from Personel where Email='" + kullanici.Text + "' and Password='" + password.Text + "' and Pers_Sisteme_Giris_Izni='True'", connect);
        DataTable dt = new DataTable();
        komut.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                SqlDataAdapter bolum = new SqlDataAdapter("select * from tblPersonelGorev where Gorev_id='" + dt.Rows[0]["Gorev_id"] + "'", connect);
                DataTable bolum_tablo = new DataTable();
                bolum.Fill(bolum_tablo);

                HttpCookie cerez = new HttpCookie("RcEU");
                cerez.Values.Add("QiZrNv", Sorgu.Encrypt(dt.Rows[0]["Email"].ToString()));
                cerez.Values.Add("YcEsZ", Sorgu.Encrypt(password.Text));
                cerez.Values.Add("TyPsqX", Sorgu.Encrypt(dt.Rows[0]["Persid"].ToString()));
                cerez.Values.Add("Kullanici_Adi", Server.UrlEncode(dt.Rows[0]["Pers_adi"].ToString()) + " " + Server.UrlEncode(dt.Rows[0]["Pers_soyadi"].ToString()));

                if (bolum_tablo.Rows.Count > 0)
                {
                    cerez.Values.Add("Bolum", Server.UrlEncode(bolum_tablo.Rows[0]["Gorev"].ToString()));
                }
                else
                {
                    cerez.Values.Add("Bolum", "Bölüm Belirlenemedi.");
                }

                if (benihatirla.Checked)
                {
                    cerez.Expires = DateTime.Now.AddDays(7);
                }
                else
                {
                    cerez.Expires = DateTime.Now.AddHours(12);
                }
                cerez.Values.Add("Foto", dt.Rows[0]["Foto"].ToString());
                Response.Cookies.Add(cerez);


                HttpCookie cerez_foto = new HttpCookie("COF");
                if (benihatirla.Checked)
                {
                    cerez_foto.Values.Add("U", Sorgu.Encrypt(dt.Rows[0]["Email"].ToString()));
                }
                else
                {
                    cerez_foto.Values.Add("U", "");
                }                   
                cerez_foto.Values.Add("F", Sorgu.Encrypt(dt.Rows[0]["Foto"].ToString()));
                cerez_foto.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cerez_foto);
                Response.Redirect("/Anasayfa/");
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Oturum açmak için kullandığınız kullanıcı adınız ve şifreniz eşleşmiyor.").ToString(), true);
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);
        }
    }

    protected void resmi_unut_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["COF"] != null)
            {
                Response.Cookies["COF"].Expires = DateTime.Now.AddYears(-1);
                Response.Redirect("/Giris");
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);
        }        
    }
}