using System;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Web.Script.Services;
using System.Net;
using System.Reflection.Emit;
using System.Web.UI;

public class Sorgu
{
    public static string genelyetkikurallari()
    {
        string temp = "";
        try
        {
            HttpContext.Current.Session["yetkises"] = null;
            if (HttpContext.Current.Session["yetkises"] == null)
            {
                string kullaniciid = Sorgu.Decrypt(HttpContext.Current.Request.Cookies["RcEU"]["TyPsqX"]);

                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString); connect.Open();
                SqlDataAdapter yetkicommand = new SqlDataAdapter("SELECT YY_Yetki_Kodu[komut], dd.YRPI_Yetki_Durumu[durumu], 'ist'[tablo] from tblYetkiRolPersonelIst dd "+
                                                                 "left join tblYetkiYetki yy on yy.YY_Id = dd.YRPI_YetkiId where dd.YRPI_Persid = '"+kullaniciid+"' "+
                                                                 "UNION all "+
                                                                 "Select DISTINCT YY_Yetki_Kodu, YRY_Yetki_Id[durumu], 'diger'[tablo] From tblYetkiRolYetki yry "+
                                                                 "left join tblYetkiYetki yy on yy.YY_Id = yry.YRY_Yetki_Id " +
                                                                 "where YRY_Rol_Id IN(Select YRP_Rol_Id from tblYetkiRolPersonel where YRP_Persid = '"+ kullaniciid + "') or YRY_Rol_Id=(Select YR_Id from tblYetkiRol where YR_Standart='True')", connect);

                DataTable yetkidb = new DataTable();
                yetkicommand.Fill(yetkidb);
                HttpContext.Current.Session["yetkises"] = yetkidb;
            }
        }
        catch (Exception)
        {
            Sorgu.cookiesil();
        }       
        return temp;
    }

    public static string yetkiteklikontrol(string komut)
    {
        string temp = "";
        try
        {
            HttpContext.Current.Session["yetkises"] = null;
            if (HttpContext.Current.Session["yetkises"] == null)
            {
                genelyetkikurallari();
            }

            DataTable aaa = new DataTable();
            aaa = HttpContext.Current.Session["yetkises"] as DataTable;

            if (aaa.Rows.Count > 0)
            {
                DataRow[] results = aaa.Select("komut = '" + komut.Trim() + "' and tablo='ist'");

                if (results.Length > 0)
                {
                    DataRow[] results2 = aaa.Select("komut = '" + komut.Trim() + "' AND durumu='1' AND tablo='ist'");

                    if (results2.Length > 0)
                    {
                        temp = "1";
                    }
                    else
                    {
                        temp = "0";
                    }
                }
                else
                {
                    DataRow[] results3 = aaa.Select("komut = '" + komut.Trim() + "' and tablo='diger'");

                    if (results3.Length > 0)
                    {
                        temp = "1";
                    }
                    else
                    {
                        temp = "0";
                    }
                }
            }
            else
            {
                temp = "0";
            }
        }
        catch (Exception)
        {
           temp = "0";
        }        
        return temp;
    }
        
    /*Dosya Adı Yaratma İçin Kullanılıyor (Yemek Listesi, Yerleşim Planı ve diğer PDF Dosyaları için)*/
    public static string kodyarat()
    {
        try
        {
            //Dün, Ay, Yıl, Saat, Dk, Sn
            string temp = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM")  + DateTime.Now.ToString("dd")   + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
            return temp.ToString();
        }
        catch (Exception)
        {
            //Rasgele Sayı Üret Gönder
            Random rastgele = new Random();
            int sayi = rastgele.Next(1000000, 9999999);
            return sayi.ToString();
        }
    }
   
    //Sisteme yüklenen dosya ve veri tabanına yazılan bazı verilerin türkçe karekter sorunlarını çözmek için
    public static string dosya_adi_yarat(string baslik)
    {
        string temp = "";
        temp = baslik.ToLower();
        temp = temp.Replace(" ", "_");
        temp = temp.Replace("ü", "u");
        temp = temp.Replace("ş", "s");
        temp = temp.Replace("ç", "c");
        temp = temp.Replace("ö", "o");
        temp = temp.Replace("ı", "i");
        temp = temp.Replace("ğ", "g");
        temp = temp.Replace("/", "");
        temp = temp.Replace("?", "");
        temp = temp.Replace("+", "");
        temp = temp.Replace("&", "");
        temp = temp.Replace("\"", "");
        temp = temp.Replace(".", "-");
        temp = temp.Replace(",", "");
        temp = temp.Replace("%", "");
        temp = temp.Replace("}", "");
        temp = temp.Replace("(", "");
        temp = temp.Replace(")", "");
        temp = temp.Replace("[", "");
        temp = temp.Replace("]", "");
        return temp;
    }
    
    //Karekter Temizle
    public static string karektertemizle(string baslik)
    {
        string temp = "";
        temp = baslik.ToLower();
        temp = temp.Replace(" ", "");
        temp = temp.Replace("ü", "u");
        temp = temp.Replace("ş", "s");
        temp = temp.Replace("ç", "c");
        temp = temp.Replace("ö", "o");
        temp = temp.Replace("ı", "i");
        temp = temp.Replace("ğ", "g");
        temp = temp.Replace("/", "");
        temp = temp.Replace("?", "");
        temp = temp.Replace("+", "");
        temp = temp.Replace("&", "");
        temp = temp.Replace("\"", "");
        temp = temp.Replace(".", "-");
        temp = temp.Replace(",", "");
        temp = temp.Replace("%", "");
        temp = temp.Replace("}", "");
        temp = temp.Replace("(", "");
        temp = temp.Replace(")", "");
        temp = temp.Replace("[", "");
        temp = temp.Replace("(", "");
        temp = temp.Replace(")", "");
        temp = temp.Replace("]", "");
        return temp;
    }


    //Sistemin çalışması esnasında oluşabilecek hata veya başarılı kayıt işlemlerinde çıkan uyarı mesajı
    public static string mesaj(string gelen_mesaj, string ek_mesaj)
    {
        string temp = "";

        if (gelen_mesaj == "hata")
        {
            temp = "<div class='alert alert-danger fade in block-inner' style='border:1px solid red'>" +
                   "<button type='button' class='close' data-dismiss='alert'>×</button>" +
                   "<i class='icon-cancel-circle'></i> ! Hata : İşleminiz sırasında bir hata oluştu. " + ek_mesaj + " </div>";
        }
        else if (gelen_mesaj == "basari")
        {
            temp = "<div class='alert alert-success fade in block-inner'  style='border:1px solid green'>" +
                   "<button type='button' class='close' data-dismiss='alert'>×</button>" +
                   "<i class='icon-checkmark-circle'></i>Başarılı : İşleminiz başarıyla tamamlandı. " + ek_mesaj + " </div>";
        }
        else if (gelen_mesaj == "uyari")
        {
            temp = "<div class='alert alert-warning fade in block-inner'>" +
                   "<button type='button' class='close' data-dismiss='alert'>×</button>" +
                   "<i class='icon-warning'></i>Uyarı : " + ek_mesaj + " </div>";
        }
        else if (gelen_mesaj == "bilgi")
        {
            temp = "<div class='alert alert-info fade in'>" +
                   "<button type='button' class='close' data-dismiss='alert'>×</button>" +
                   "<i class='icon-info'></i>Bilgi : " + ek_mesaj + " </div>";
        }
        else if (gelen_mesaj == "y_basari")
        {
            //Başarılı Yazı Açıklaması. Süre sonu kapanır
            temp="$.jGrowl('İşleminiz Başarıyla Tamamlandı. "+ ek_mesaj + "', { sticky: false, theme: 'growl-success', header: 'Başarılı.', life: 2000} );";
        }
        else if (gelen_mesaj == "y_hata")
        {
            //Hata Mesajı, Süre sonu kapanır
            temp= "$.jGrowl('İşleminiz sırasında bir hata oluştu. " + ek_mesaj+ "', { sticky: true, theme: 'growl-error', header: 'Hata!'});";
        }
        else if (gelen_mesaj == "y_uyari")
        {
            //uyari mesajı, süre sonu kapanır
            temp= "$.jGrowl('"+ek_mesaj+ "', { sticky: true, theme: 'growl-warning', header: 'Uyarı!' });";
        }
        else if (gelen_mesaj == "mesaj")
        {
            //mesaj sistemi. Sabit
            temp= "$.jGrowl('"+ek_mesaj+ "', { sticky: true, header: 'Mesaj' });";
        }
        //<script language=JavaScript></script>
        return temp;
    }


    //Sisteme Girilen Verilerin Şifrelenmesi
    public static string Encrypt(string clearText)
    {
        string EncryptionKey = "Qt?C2dt4!dfg2d3&44vlD5/4XxCp!g7Q1wE";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }


    //Sisteme Girilen Şifreli Verilerin Çözülmesi
    public static string Decrypt(string cipherText)
    {
        string EncryptionKey = "Qt?C2dt4!dfg2d3&44vlD5/4XxCp!g7Q1wE";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    
    public static string cookiesil()
    {
        HttpContext.Current.Response.Cookies["RcEU"].Expires = DateTime.Now.AddDays(-7);
        HttpContext.Current.Response.Cookies["RcEU"].Expires = DateTime.Now.AddHours(-12);
        return "True";
    }
    
    public static string yukleniyor()
    {
        string gonder = "<div class='yukleniyor'><div id='progressBackgroundFilter'></div><div id='processMessage'>İşleminiz devam ediyor,<br/>lütfen bekleyiniz.<br/><img src = '/Desing/Default/images/sy_images/load.gif'/></div></div>";
        return gonder;
    }

    //Sistemde Kullanıcı varmı yokmu kontrolü
    public static string kullanici_varmi(string hangikomut, string id, string tc)
    {
        string temp = "";
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        connect.Open();

        if (hangikomut=="Kayit")
        {
            SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from personel where Pers_Tc='" + tc + "'", connect);
            DataTable tablo2 = new DataTable();
            kullanici_varmi.Fill(tablo2);

            if (tablo2.Rows.Count > 0)
            {
                temp = "1";
            }
            else
            {
                temp = "0";
            }
        }
        else
        {
            //9999 Tc kimlik numarası hem personel_id hemde Yeni olan Pers_Tc de aranıyor ileride Eski Personel_id alanına arama iptal olacak
            SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from personel where Persid!='" + id+"' and (Pers_Tc='" + tc + "')", connect);
            DataTable tablo2 = new DataTable();
            kullanici_varmi.Fill(tablo2);

            if (tablo2.Rows.Count > 0)
            {
                temp = "1";
            }
            else
            {
                temp = "0";
            }
        }
        
        connect.Close();
        return temp;
    }


    //Sistemde Kullanıcı varmı yokmu kontrolü
    public static string kullanici_bilgileri(string persid, string deger)
    {
        string temp = "";
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        connect.Open();

        SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from personel where Persid='" + persid + "' ", connect);
        DataTable tablo2 = new DataTable();
        kullanici_varmi.Fill(tablo2);

        if (tablo2.Rows.Count > 0)
        {
            if (deger=="1")
            {
                temp = tablo2.Rows[0]["Pers_Adi"].ToString();
            }

            if (deger == "2")
            {
                temp = tablo2.Rows[0]["Pers_Adi"].ToString() + " " + tablo2.Rows[0]["Pers_Soyadi"].ToString();
            }

            if (deger == "3")
            {
                temp = tablo2.Rows[0]["bol_id"].ToString();
            }
        }
        else
        {
            temp = "0";
        }
        connect.Close();
        return temp;
    }

    //Personele yüklenen resmin orantılanması
    public static Bitmap Oranla(Bitmap resim, int donusturme)
    {
        int sabit = 250;
        Bitmap oranlanacak = resim;

        using (Bitmap Orjinal = resim)
        {
            double yukseklik = Orjinal.Height;// Resmin Yüksekliğini yukseklik değişkenine atıyoruz.
            double genislik = Orjinal.Width;//Resmin enini genişlik değişkenine atıyoruz.
            double oran = 0;

            if (genislik >= yukseklik && genislik > sabit) // eğer genişlik büyük veya yüksekliğe eşit ise
            {
                oran = genislik / yukseklik; // orana genişlik ile yükseliği böl ve ata
                genislik = sabit;
                yukseklik = sabit / oran; // yükseliğe genişlik ile oranı böl ve ata
            }
            else if (yukseklik >= genislik && yukseklik > sabit) // eğer genişlik büyük ve eşik ise yüksekliğe
            {
                oran = yukseklik / genislik; // orana genişlik ile yükseliği böl ve ata
                yukseklik = sabit;
                genislik = sabit / oran; // yükseliğe genişlik ile oranı böl ve ata
            }

            Size yenidegerler = new Size(Convert.ToInt32(genislik), Convert.ToInt32(yukseklik)); //Resmi yeniden boyutlandırıyoruz.
            Bitmap yeniresim = new Bitmap(Orjinal, yenidegerler);
            oranlanacak = yeniresim;
        }
        return (oranlanacak);
    }


    //Sitenin resim bölümlerinde resim yoksa Resim yok ikonu getiriliyor.
    public static string GetImageUrl(string dbImgURL)
    {
        string temp = "";
        try
        {
            temp = dbImgURL.ToLower();
            if (File.Exists(HttpContext.Current.Server.MapPath(dbImgURL)))
            {
                temp = dbImgURL;
            }
            else
            {
                temp = "/Desing/Default/images/sy_images/resim_yok.png";
            }
            return temp;
        }
        catch (Exception)
        {
            temp = "/Desing/Default/images/sy_images/resim_yok.png";
            return temp;
        }
    }


    //Sitenin resim bölümlerinde resim yoksa Resim yok ikonu getiriliyor.
    public static string GetImageUrl2(string dbImgURL)
    {
        string temp = "";
        try
        {
            temp = dbImgURL.ToLower();
            if (File.Exists(HttpContext.Current.Server.MapPath(dbImgURL)))
            {
                temp = dbImgURL;
            }
            else
            {
                temp = "/Desing/Default/images/sy_images/kullanici_giris.png";
            }
            return temp;
        }
        catch (Exception)
        {
            temp = "/Desing/Default/images/sy_images/kullanici_giris.png";
            return temp;
        }
    }


    //Cari İşlemleri//////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static string cari_varmi(string hangikomut, string id, string cari_kodu, string vergi_no, string turu, string kimlik_no)
    {
        string temp = "";
        string kimlik_vergi_sorgu = "";
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        connect.Open();

        if (kimlik_no!="" && vergi_no=="")
        {
            kimlik_vergi_sorgu = " Cari_KimlikNo='" + kimlik_no + "'";
        }
        else if (kimlik_no == "" && vergi_no != "")
        {
            kimlik_vergi_sorgu = " Cari_VergiNo = '" + vergi_no + "'";
            
        }
        else if (kimlik_no != "" && vergi_no != "")
        {
            kimlik_vergi_sorgu = " Cari_VergiNo = '" + vergi_no + "' or Cari_KimlikNo='" + kimlik_no + "'";
        }

        if (hangikomut == "Kayit")
        {
            SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from tblCariKayit where Cari_Kodu='" + cari_kodu + "' or (Cari_Turu='" + turu + "' and ("+ kimlik_vergi_sorgu + "))", connect);
            DataTable tablo2 = new DataTable();
            kullanici_varmi.Fill(tablo2);

            if (tablo2.Rows.Count > 0)
            {
                temp = "1";
            }
            else
            {
                temp = "0";
            }
        }
        else
        {
            SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from tblCariKayit where Cari_Id!='" + id + "' and Cari_Kodu='" + cari_kodu + "' or (Cari_Id!='"+id+"' and Cari_Turu='" + turu + "' and ("+ kimlik_vergi_sorgu + "))", connect);
            DataTable tablo2 = new DataTable();
            kullanici_varmi.Fill(tablo2);

            if (tablo2.Rows.Count > 0)
            {
                temp = "1";
            }
            else
            {
                temp = "0";
            }
        }

        connect.Close();
        return temp;
    }
    
    public static string cari_kod_uret()
    {
        int cari_kod = 1;

        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        connect.Open();
        SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select top 1 Cari_Kodu from tblCariKayit order by Cari_Kodu desc", connect);
        DataTable tablo2 = new DataTable();
        kullanici_varmi.Fill(tablo2);

        if (tablo2.Rows.Count>0)
        {
            cari_kod = Convert.ToInt32(tablo2.Rows[0]["Cari_Kodu"]) + 1;
        }
        connect.Close();

        return cari_kod.ToString();
    }
    //Cari İşlemleri SON//////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Stok Kodu Üretme
    public static string stok_kod_uret()
    {
        int cari_kod = 1;

        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        connect.Open();
        SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select top 1 Stok_Id from tblStok order by Stok_Id desc", connect);
        DataTable tablo2 = new DataTable();
        kullanici_varmi.Fill(tablo2);

        if (tablo2.Rows.Count > 0)
        {
            cari_kod = Convert.ToInt32(tablo2.Rows[0]["Stok_Id"]) + 1;
        }
        connect.Close();

        return cari_kod.ToString();
    }
    //Stok İşlemleri SON ////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    public static string urun_varmi(string hangikomut, string id, string urun_kodu)
    {
        string temp = "";
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        connect.Open();

        if (hangikomut == "Kayit")
        {
            SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from tblCariAsansorler where CariSU_KimlikNo='" + urun_kodu + "'", connect);
            DataTable tablo2 = new DataTable();
            kullanici_varmi.Fill(tablo2);

            if (tablo2.Rows.Count > 0)
            {
                temp = "1";
            }
            else
            {
                temp = "0";
            }
        }
        else
        {
            SqlDataAdapter kullanici_varmi = new SqlDataAdapter("Select * from tblCariAsansorler where CariSU_Id!='" + id + "' and CariSU_KimlikNo='" + urun_kodu + "'", connect);
            DataTable tablo2 = new DataTable();
            kullanici_varmi.Fill(tablo2);

            if (tablo2.Rows.Count > 0)
            {
                temp = "1";
            }
            else
            {
                temp = "0";
            }
        }

        connect.Close();
        return temp;
    }


    //Dosya Sil
    public static string dosya_sil(string dosya, string dosya_yolu)
    {
        string gonder = "1";
        try
        {
            dosya_yolu = HttpContext.Current.Server.MapPath("/Data/" + dosya_yolu + "/") + "\\" + dosya;
            if (System.IO.File.Exists(dosya_yolu))
            {
                System.IO.File.Delete(dosya_yolu);// Ve sil
            }
        }
        catch (Exception)
        {
            gonder = "0";
        }
        return gonder;
    }


    public static string cari_bul(string cari_id)
    {
        string temp = "";
        try
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString); connect.Open();
            SqlDataAdapter cari_bull = new SqlDataAdapter("Select * from tblCariKayit where Cari_Id='" + cari_id + "'", connect);
            DataTable cari_bulltb = new DataTable();
            cari_bull.Fill(cari_bulltb);

            if (cari_bulltb.Rows.Count>0)
            {
                temp = cari_bulltb.Rows[0]["Cari_Unvan"].ToString();
            }
        }
        catch (Exception)
        {
            cari_id = "0";
        }
        return temp;
    }
}