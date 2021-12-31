using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
public partial class Modul_Roller : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("Y_Yetkiler_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("Y_Yetkiler_Yazma") != "1")
        {
            g_panel.Visible=false;
        }
        //Güvenlik///////////////////////


        if (RouteData.Values["id"] != null)
        {
            //Eğer güncelleme sayfası açıksa tıklanan verinin bilgisini getir.
            bilgi_getir();
         
            //Güncelleme sayfası açıkken yeni rol için buton yarat
            buttonlar_panel.Visible = true;

            //Güncelleme sayfası açıkken buton adını güncelle olarak değiştir.
            kayit.Text = "Güncelle";

            //Düzenleme Sayfasında Başlık Değişimi
            rol_ekle_baslik.Text = "Rol Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }

        //Rolleri Yükle
        rolleri_yukle();
    }


    //Rolleri Yükle
    public void rolleri_yukle()
    {
        connect.Open();
        try
        {
            //Sistemdeki Ana Rolleri Yukle
            SqlDataAdapter komut = new SqlDataAdapter("Select * from tblYetkiRol", connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);

            if (tablo.Rows.Count > 0 )
            {
                rep1.DataSource = tablo;
                rep1.DataBind();
            }
            else
            {
               ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde kayıtlı rol bulunamadı.").ToString(), true);
            }
        }
        catch (Exception)
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Roller Yüklenemedi.").ToString(), true);
        }
        connect.Close();
    }


    //Rolleri güncellemek ve yeni kayıt eklemek için
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            connect.Open();
            SqlCommand kayit_ekle;
            if (RouteData.Values["id"] != null)
            {
                kayit_ekle = new SqlCommand("If Not Exists(select * from tblYetkiRol where YR_Rol_Adi='" + rol_adi.Text + "' and YR_Id!='" + RouteData.Values["id"] + "') " +
                                            "Begin update tblYetkiRol set YR_Rol_Adi='" + rol_adi.Text + "', YR_Standart='"+ standart_rol.Checked.ToString() + "' where YR_Id='" + RouteData.Values["id"] + "' End", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("If Not Exists(select * from tblYetkiRol where YR_Rol_Adi='" + rol_adi.Text + "') " +
                                            "Begin insert into tblYetkiRol(YR_Rol_Adi, YR_Standart) values('" + rol_adi.Text + "', '"+ standart_rol.Checked.ToString() + "') End", connect);
            }
            int sonuc = kayit_ekle.ExecuteNonQuery();
            kayit_ekle.ExecuteNonQuery();

            //Eğer güncelleme veya kaydet sql değeri 0 dan küçükse
            if (sonuc < 0)
            {
               ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde eşleşen rol adı bulundu.").ToString(), true);
            }
            else
            {
               ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            }
            connect.Close();
            rolleri_yukle();
        }
        catch
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Kayıt Yapılamadı.").ToString(), true);
        }
    }


    //Güncelle denildiğinde ekrana rol detaylarını yükle
    public void bilgi_getir()
    {
        try
        {
            if (!IsPostBack)
            {
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblYetkiRol where YR_Id='" + RouteData.Values["id"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    rol_adi.Text = tablo.Rows[0]["YR_Rol_Adi"].ToString();
                    rol_baslik.Text = tablo.Rows[0]["YR_Rol_Adi"].ToString();
                    standart_rol.Checked = Convert.ToBoolean(tablo.Rows[0]["YR_Standart"]);
                }
                else
                {
                   ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Rol bilgileri alınamadım.").ToString(), true);
                }
            }
        }
        catch
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Bilgiler ekrana getirilemedi.").ToString(), true);
        }
    }
}