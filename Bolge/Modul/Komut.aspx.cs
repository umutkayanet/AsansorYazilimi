using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_Default4 : System.Web.UI.Page
{   
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Personel Listesinde Personelin Sisteme Giriş Çıkışını Durdurmak/Başlatmak İçin Yapılan Çalışma
    [System.Web.Services.WebMethod]
    public static string sistemegiris(string id, char durum)
    {
        string sonuc = "";
        try
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
                connect.Open();
                if (durum == '0')
                {
                    SqlCommand personel_giris_izni = new SqlCommand("update Personel set Pers_Sisteme_Giris_Izni='False' where Persid='" + id + "'", connect);
                    personel_giris_izni.ExecuteNonQuery();
                    sonuc = "İşleminiz Başarıyla Gerçekleşti.";
                }
                else if (durum == '1')
                {
                    SqlCommand personel_giris_izni = new SqlCommand("update Personel set Pers_Sisteme_Giris_Izni='True' where Persid='" + id + "'", connect);
                    personel_giris_izni.ExecuteNonQuery();
                    sonuc = "İşleminiz Başarıyla Gerçekleşti.";
                }
                connect.Close();   
            return sonuc;
        }
        catch (Exception)
        {
            return sonuc="Hata";
        }
    }


    //Roller sayfasındaki rollere bağlı yetilerin on off butonları
    [System.Web.Services.WebMethod]
    public static string yetki_yap(string Rolid, string yetkiid, string durum)
    {
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        string sonuc = "";
        try
        {         
            connect.Open();
            if (durum=="1")
            {
                SqlCommand personel_giris_izni = new SqlCommand("If Not Exists(select * from tblYetkiRolYetki where YRY_Rol_Id='" + Rolid + "' and YRY_Yetki_Id='" + yetkiid + "') " +
                                                                "Begin insert into tblYetkiRolYetki(YRY_Rol_Id, YRY_Yetki_Id) values('" + Rolid + "', '" + yetkiid + "') End", connect);
                personel_giris_izni.ExecuteNonQuery();
            }
            else
            {
                SqlCommand personel_giris_izni = new SqlCommand("delete from tblYetkiRolYetki where YRY_Rol_Id='" + Rolid + "' and YRY_Yetki_Id='"+ yetkiid + "'", connect);
                personel_giris_izni.ExecuteNonQuery();
            }
            sonuc = "İşleminiz Başarıyla Gerçekleşti.";
            connect.Close();
            return sonuc;
        }
        catch (Exception)
        {
            return sonuc = "!Hata. Kayıs esnasında bir hata meydana geldi.";
        }
    }



    //Personel Detayında Yetki Ekleme Alanı İçin Kullanılan Komut
    [System.Web.Services.WebMethod]
    public static string peryetki(string perid, string rolid, string durum)
    {
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        string sonuc = "";
        try
        {
            connect.Open();
            if (durum == "1")
            {
                SqlCommand personel_giris_izni = new SqlCommand("If Not Exists(select * from tblYetkiRolPersonel where YRP_Persid='" + perid + "' and YRP_Rol_Id='" + rolid + "') " +
                                                                "Begin insert into tblYetkiRolPersonel(YRP_Persid, YRP_Rol_Id, YRP_ModBy, YRP_ModDate) values('" + perid + "', '" + rolid + "', '"+ Sorgu.Decrypt(HttpContext.Current.Request.Cookies["RcEU"]["TyPsqX"])  +"', '"+ Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))+"') End", connect);
                personel_giris_izni.ExecuteNonQuery();
            }
            else
            {
                SqlCommand personel_giris_izni = new SqlCommand("delete from tblYetkiRolPersonel where YRP_Persid='" + perid + "' and YRP_Rol_Id='" + rolid + "'", connect);
                personel_giris_izni.ExecuteNonQuery();
            }
            sonuc = "İşleminiz Başarıyla Gerçekleşti.";
            connect.Close();
            return sonuc;
        }
        catch (Exception Ex)
        {
            return sonuc = "!Hataaa" + Ex.Message;
        }
    }



    //Personel Detayında Yetki Ekleme Alanında istisnai yetki ekleme için kullanılır.
    [System.Web.Services.WebMethod]
    public static string yetkipersonelist(string perid, string yetkiid, string durum)
    {
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        string sonuc = "";
        try
        {
            connect.Open();
            if (durum == "1")
            {
                SqlCommand personel_giris_izni = new SqlCommand("If Not Exists(select * from tblYetkiRolPersonelIst where YRPI_Persid='" + perid + "' and YRPI_YetkiId='" + yetkiid + "') " +
                                                                "Begin insert into tblYetkiRolPersonelIst(YRPI_Persid, YRPI_YetkiId, YRPI_Yetki_Durumu, YRPI_ModBy, YRPI_ModDate) values('" + perid + "', '" + yetkiid + "', 'True', '" + Sorgu.Decrypt(HttpContext.Current.Request.Cookies["RcEU"]["TyPsqX"]) + "', '" + Convert.ToDateTime(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")) + "') End " +
                                                                "ELSE BEGIN Update tblYetkiRolPersonelIst set YRPI_Yetki_Durumu='True' where YRPI_Persid='" + perid + "' and YRPI_YetkiId='" + yetkiid + "' End", connect);
                personel_giris_izni.ExecuteNonQuery();
            }
            else
            {
                SqlCommand personel_giris_izni = new SqlCommand("If Not Exists(select * from tblYetkiRolPersonelIst where YRPI_Persid='" + perid + "' and YRPI_YetkiId='" + yetkiid + "') " +
                                                                 "Begin insert into tblYetkiRolPersonelIst(YRPI_Persid, YRPI_YetkiId, YRPI_Yetki_Durumu, YRPI_ModBy, YRPI_ModDate) values('" + perid + "', '" + yetkiid + "', 'False', '" + Sorgu.Decrypt(HttpContext.Current.Request.Cookies["RcEU"]["TyPsqX"]) + "', '" + Convert.ToDateTime(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")) + "') End " +
                                                                 "ELSE BEGIN Update tblYetkiRolPersonelIst set YRPI_Yetki_Durumu='False' where YRPI_Persid='" + perid + "' and YRPI_YetkiId='" + yetkiid + "' End", connect);
                personel_giris_izni.ExecuteNonQuery();
            }

            sonuc = "İşleminiz Başarıyla Gerçekleşti.";
            connect.Close();
            return sonuc;
        }
        catch (Exception)
        {
            return sonuc = "!Hata";
        }
    }



    //PErsonel yetki işlemleri menüsünde yetkiyi istisnadan kaldırma ve tekrar ekleme işlemi
    [System.Web.Services.WebMethod]
    public static string yetkipersonelistsil(string perid, string yetkiid, string durum)
    {
        SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        string sonuc = "";
        try
        {
            connect.Open();
            if (durum == "1")
            {
                SqlCommand personel_giris_izni = new SqlCommand("If Not Exists(select * from tblYetkiRolPersonelIst where YRPI_Persid='" + perid + "' and YRPI_YetkiId='" + yetkiid + "') " +
                                                               "Begin insert into tblYetkiRolPersonelIst(YRPI_Persid, YRPI_YetkiId, YRPI_Yetki_Durumu, YRPI_ModBy, YRPI_ModDate) values('" + perid + "', '" + yetkiid + "', 'True', '" + Sorgu.Decrypt(HttpContext.Current.Request.Cookies["RcEU"]["TyPsqX"]) + "', '" + Convert.ToDateTime(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")) + "') END", connect);
                personel_giris_izni.ExecuteNonQuery();
                sonuc = "İstistani Tablosuna Yetki <b>Ekleme</b> İşlemi BAŞARILI.";

            }
            else
            {
                SqlCommand sil = new SqlCommand("delete from tblYetkiRolPersonelIst where YRPI_Persid='" + perid + "' and YRPI_YetkiId='" + yetkiid + "'", connect);
                sil.ExecuteNonQuery();
                sonuc = "İstisnai Tablosundan Yetki <b>Silme</b> İşlemi BAŞARILI.";
            }

           
            connect.Close();
            return sonuc;
        }
        catch (Exception)
        {
            return sonuc = "!Hata";
        }
    }


    //Personel Listesinde Personelin Sisteme Giriş Çıkışını Durdurmak/Başlatmak İçin Yapılan Çalışma
    [System.Web.Services.WebMethod]
    public static string cinsiyet(string id, char durum)
    {
        string sonuc = "";
        try
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
            connect.Open();
            SqlCommand personel_giris_izni = new SqlCommand("update Personel set Pers_Cinsiyet='"+durum+"' where Persid='" + id + "'", connect);
            personel_giris_izni.ExecuteNonQuery();
            sonuc = "İşleminiz Başarıyla Gerçekleşti." + durum + " "+ id;
            
            connect.Close();
            return sonuc;
        }
        catch (Exception)
        {
            return sonuc = "Hata";
        }
    }
}