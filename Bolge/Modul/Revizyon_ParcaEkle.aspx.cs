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

public partial class Modul_Revizyon_ParcaEkle : System.Web.UI.Page
{
    string buton_yetki;
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Revizyon_Listelemee") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Revizyon_Yazma") != "1")
        {
            g_panel.Visible = false;
        }
        //Güvenlik Son


        //Verileri Yükle
        verileri_yukle();
    }

    public void verileri_yukle()
    {
        connect.Open();

        Cari_Label.Text = Sorgu.cari_bul(RouteData.Values["id"].ToString());


        //Sistemdeki Revizyonda Takilan Ürünlerini Yukle
        SqlDataAdapter komut = new SqlDataAdapter("Select rs.CariRevizyoSec_Id, st.Stok_Id, st.Stok_UrunAdi, rs.CariRevizyoSec_RevizyonAdet, rs.CariRevizyoSec_Tarih  from tblCariRevizyonSec rs " +
                                                  "left join tblStok st on(rs.CariRevizyoSec_Revizyon = st.Stok_Id) where rs.CariRevizyoSec_RevizyonId='"+ RouteData.Values["idd"] + "' ", connect);
        DataTable tablo = new DataTable();
        komut.Fill(tablo);
        rep1.DataSource = tablo;
        rep1.DataBind();
        connect.Close();
    }

    protected void kayit_Click(object sender, EventArgs e)
    {
       try
        {
            connect.Open();
            SqlCommand komut = new SqlCommand("insert into tblCariRevizyonSec(CariRevizyoSec_Revizyon, CariRevizyoSec_RevizyonAdet, CariRevizyoSec_RevizyonId, CariRevizyoSec_Tarih)values('" + Drop1.SelectedValue + "', '"+t4.Text+"', '" + RouteData.Values["idd"] + "', '" + Convert.ToDateTime(t5.Text).ToString("yyyy.MM.dd") + "')", connect);
            komut.ExecuteNonQuery();
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

    protected void rep1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "sil")
        {
            connect.Open();
            int id = Convert.ToInt32(e.CommandArgument.ToString());

            SqlCommand sil = new SqlCommand("delete from tblCariRevizyonSec where CariRevizyoSec_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            verileri_yukle();
        }
    }

    protected void rep1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Revizyon_Yazma") != "1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
        //Güvenlik Son
    }
}