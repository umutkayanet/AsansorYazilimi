using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AjaxControlToolkit;

public partial class Modul_Asansor_Ekle : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string icguvenlik_gorev = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Servis_Asansor_Okuma") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Servis_Asansor_Yazma") != "1")
        {
            g_panel.Visible = false;
        }
        //Güvenlik Son


        if (RouteData.Values["m"] != null)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
        }


        if (RouteData.Values["idd"] != null)
        {
            //Duyuru Bilgilerini Ekrana Getir.
            bilgi_getir();

            //Güncelleme sayfası açıkken yeni asansör için butonu yarat
            buttonlar_panel.Visible = true;

            //Güncelleme sayfası açıkken buton adını güncelle olarak değiştir.
            kayit.Text = "Güncelle";

            //Düzenleme Sayfasında Başlık Değişimi
            yeni_kayit_buton.Text = "Asansör Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }
        //Verileri Yükle
        verileri_yukle();
    }

    

    public void verileri_yukle()
    {
        try
        {
            connect.Open();
            Cari_Label.Text = Sorgu.cari_bul(RouteData.Values["id"].ToString());

            //Sistemdeki Duyurları Yukle
            SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariAsansorler ca " +
                                                      "Left join tblAsansorCinsi ast on (ca.CariSU_Cinsi=ast.AsansorCinsi_Id) where CariSU_CariNo='" + RouteData.Values["id"] + "' ", connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);
            rep1.DataSource = tablo;
            rep1.DataBind();

            connect.Close();
            komut.Dispose();
        }
        catch (Exception)
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Asansör Listesi Yüklenemedi.").ToString(), true);
           return;
        }
    }


    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            connect.Open();
            SqlCommand kayit_ekle;

            //Veri Varsa Güncelle Yoksa ekle
            if (RouteData.Values["idd"] != null)
            {
                kayit_ekle = new SqlCommand("update tblCariAsansorler set CariSu_KimlikNo='" + t2.Text + "', CariSu_Tanimi='" + t3.Text + "', CariSU_Cinsi='" + asansor_cinsi.SelectedValue + "', CariSU_Not='"+asansor_not.Text+"' where CariSU_Id='" + RouteData.Values["idd"] + "'", connect);

                if (Sorgu.urun_varmi("Guncelle", RouteData.Values["idd"].ToString(), t2.Text)!="0")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde Kimlik Numarası Kayıtlı Lütfen Kontrol Ediniz.").ToString(), true);
                    return;
                }
            }
            else
            {
                kayit_ekle = new SqlCommand("insert into tblCariAsansorler(CariSu_KimlikNo, CariSu_Tanimi, CariSU_CariNo, CariSU_Tarih, CariSU_Cinsi, CariSU_Not) values('" + t2.Text + "', '" + t3.Text + "', '" + RouteData.Values["id"] + "', '"+Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "', '"+ asansor_cinsi.SelectedValue + "','"+asansor_not.Text+"')", connect);

                if (Sorgu.urun_varmi("Kayit", "", t2.Text) != "0")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde Kimlik Numarası Kayıtlı Lütfen Kontrol Ediniz.").ToString(), true);
                    return;
                }
            }
            kayit_ekle.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            connect.Close();
            kayit_ekle.Dispose();
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
                connect.Open();
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariAsansorler where CariSU_Id='" + RouteData.Values["idd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    t2.Text = tablo.Rows[0]["CariSU_KimlikNo"].ToString();
                    t3.Text = tablo.Rows[0]["CariSU_Tanimi"].ToString();
                    asansor_cinsi.SelectedValue = tablo.Rows[0]["CariSU_Cinsi"].ToString();
                    asansor_not.Text = tablo.Rows[0]["CariSU_Not"].ToString();
                }
                else
                {
                   ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari Stok Bilgileri Alınamadı").ToString(), true);
                }
                connect.Close();
                komut.Dispose();
            }
        }
        catch
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Bilgiler ekrana getirilemedi.").ToString(), true);
           return;
        }
    }

    protected void rep1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "sil")
        {
            connect.Open();
            int id = Convert.ToInt32(e.CommandArgument.ToString());

            SqlCommand sil = new SqlCommand("delete from tblCariAsansorler where CariSU_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["idd"] != null)
            {
                if (RouteData.Values["idd"].ToString() == id.ToString())
                {
                   Response.Redirect("/Servis/Asansor-Ekle/" + RouteData.Values["id"] + "");
                   return;
                }
            }

            connect.Close();
            sil.Dispose();
            verileri_yukle();
        }
    }


    protected void txtDescricao_HtmlEditorExtender_ImageUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
    {
        //get the file name of the posted image
        string imgName = e.FileName;
        // Generate file path
        string filePath = "/Data/" + imgName;

        // Save uploaded file to the file system
        var ajaxFileUpload = (AjaxFileUpload)sender;
        ajaxFileUpload.SaveAs(MapPath(filePath));

        // Update client with saved image path
        e.PostedUrl = Page.ResolveUrl(filePath);

    }


    protected void rep1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (icguvenlik_gorev != "1")
            {
                e.Item.FindControl("sil").Visible = false;
            }
        }
    }
}