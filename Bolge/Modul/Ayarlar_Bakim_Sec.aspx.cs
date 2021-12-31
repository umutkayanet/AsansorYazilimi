using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AjaxControlToolkit;
using System.IO;
using ClosedXML.Excel;
using System.Collections;

public partial class Modul_Ayarlar_Bakim_Sec : System.Web.UI.Page
{
    string buton_yetki;
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Ayarlar_Okuma") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Ayarlar_Yazma") != "1")
        {
            g_panel.Visible = false;
        }
        //Güvenlik Son


        if (RouteData.Values["id"] != null)
        {
            //Stok Bilgilerini Ekrana Getir.
            bilgi_getir();

            //Güncelleme sayfası açıkken yeni asansör için butonu yarat
            buttonlar_panel.Visible = true;

            //Güncelleme sayfası açıkken buton adını güncelle olarak değiştir.
            kayit.Text = "Güncelle";

            //Düzenleme Sayfasında Başlık Değişimi
            yeni_kayit_buton.Text = "Seçenek Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }

        //Verileri Yükle
        verileri_yukle();
    }

    string sorgu = "";
    string belge_ozeti = "";
    public void verileri_yukle()
    {
        connect.Open();
        
        SqlCommand komut = new SqlCommand("Select * From tblCariBakimSecenekleri", connect);
        SqlDataReader reader;

        reader = komut.ExecuteReader();
        stop_rep.DataSource = reader;
        stop_rep.DataBind();
        connect.Close();
    }

    int sonuc;
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            connect.Open();
            SqlCommand kayit_ekle;

            //Veri Varsa Güncelle Yoksa ekle
            if (RouteData.Values["id"] != null)
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariBakimSecenekleri where BakimSecenekleri_Adi='" + Text1.Text + "' and BakimSecenekler_Id!='" + RouteData.Values["id"] + "')  " +
                                            "update tblCariBakimSecenekleri set BakimSecenekleri_Adi='" + Text1.Text + "' where BakimSecenekler_Id='" + RouteData.Values["id"] + "'", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariBakimSecenekleri where BakimSecenekleri_Adi='" + Text1.Text + "') " +
                                            "Begin insert into tblCariBakimSecenekleri(BakimSecenekleri_Adi) values('" + Text1.Text+"') end", connect);
            }
            sonuc = kayit_ekle.ExecuteNonQuery();
            kayit_ekle.ExecuteNonQuery();

            if (sonuc < 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde benzer seçenek bulundu.").ToString(), true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            connect.Close();
            verileri_yukle();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Kayıt Yapılamadı.").ToString(), true);
            return;
        }
    }


    //Güncelle denildiğinde ekrana duyuru detaylarını yükle
    public void bilgi_getir()
    {
        try
        {
            if (!IsPostBack)
            {
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariBakimSecenekleri where BakimSecenekler_Id='" + RouteData.Values["id"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Text1.Text = tablo.Rows[0]["BakimSecenekleri_Adi"].ToString();
                }
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Bilgiler ekrana getirilemedi.").ToString(), true);
            return;
        }
    }


    protected void rep1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Ayarlar_Yazma") != "1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
        //Güvenlik Son
    }

    protected void stop_rep_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "sil")
        {
            connect.Open();
            int id = Convert.ToInt32(e.CommandArgument.ToString());

            SqlCommand sil = new SqlCommand("delete from tblCariBakimSecenekleri where BakimSecenekler_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["id"] != null)
            {
                if (RouteData.Values["id"].ToString() == id.ToString())
                {
                    Response.Redirect("/Ayarlar/Bakim-Secenekleri");
                    return;
                }
            }
            verileri_yukle();
        }
    }
}