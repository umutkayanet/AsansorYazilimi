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

public partial class Modul_Stok_Listesi : System.Web.UI.Page
{
    string buton_yetki;
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Stok_Listesi_Okuma") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Stok_Listesi_Yazma") != "1")
        {
            g_panel.Visible = false;
            buton_yetki = "0";
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
            yeni_kayit_buton.Text = "Stok Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }
        else
        {
            if (!IsPostBack)
            {
                Text1.Text = Sorgu.stok_kod_uret();
            }
        }
        //Verileri Yükle
        verileri_yukle();
    }

    string sorgu = "";
    string belge_ozeti = "";
    public void verileri_yukle()
    {
        connect.Open();

        string Stok_Kodu_Arama = "";
        if (Stok_Kodu.Text != string.Empty)
        {
            Stok_Kodu_Arama = " and Stok_UrunKodu LIKE '%" + Stok_Kodu.Text + "%' ";
            belge_ozeti += "<b>Cari Kodu :</b> " + Stok_Kodu.Text + ", ";
        }

        string Urun_Adi_Arama = "";
        if (Urun_Adi.Text != string.Empty)
        {
            Urun_Adi_Arama = " and Stok_UrunAdi LIKE '%" + Urun_Adi.Text + "%' ";
            belge_ozeti += "<b>Ünvan: </b> " + Urun_Adi.Text + ", ";
        }

        string stok_adeti_arama = "";
        if (stok.Text != string.Empty)
        {
            stok_adeti_arama = " and adet < '" + stok.Text + "' ";
            belge_ozeti += "<b>Adet: </b> " + Urun_Adi.Text + ", ";
        }

        string marka_arama = "";
        if (Marka_Arama.SelectedItem.Value == "0")
        {
            marka_arama = "";
        }
        else
        {
            marka_arama = " and Stok_MarkasiId = '" + Marka_Arama.SelectedItem.Value + "'";
            belge_ozeti += "<b>MArka: </b>" + Marka_Arama.SelectedItem.Text + ", ";
        }

        sorgu = " where Stok_Id != '' " + Stok_Kodu_Arama + Urun_Adi_Arama + stok_adeti_arama + marka_arama + " order by Stok_UrunAdi";

        SqlCommand komut = new SqlCommand("WITH KullanilanStok AS " +
                                          "( " +
                                          "Select SEC.CariRevizyoSec_Revizyon, " +
                                                  "SUM(SEC.CariRevizyoSec_RevizyonAdet)[RevizyonAdet] " +
                                          "from tblCariRevizyonSec SEC group by SEC.CariRevizyoSec_Revizyon, SEC.CariRevizyoSec_RevizyonAdet " +
                                          "), " +

                                          "songiris AS " +
                                          "( " +
                                          "Select CariStokKayit_UrunKodu, SUM(CariStokKayit_Adet) as ToplamAdet " +
                                          "from tblCariStokKayit " +
                                          "Left join tblStok st on (st.Stok_Id = CariStokKayit_UrunKodu) " +
                                          "group by CariStokKayit_UrunKodu" +
                                          "), " +

                                          "ArizaEklenenParca AS" +
                                          "( " +
                                          "Select SEC.CariAsansorArizaSec_Parca," +
                                          "SUM(SEC.CariAsansorArizaSec_ParcaAdet)[ArizaAdet] " +
                                          "from tblCariAsansorArizaSec SEC group by SEC.CariAsansorArizaSec_Parca, SEC.CariAsansorArizaSec_ParcaAdet " +
                                          ") " +

                                          "Select (" +
                                                "CASE WHEN sn.ToplamAdet is null THEN 0  ELSE sn.ToplamAdet END -" +
                                                "CASE WHEN KS.RevizyonAdet is null THEN 0  ELSE KS.RevizyonAdet END - " +
                                                "CASE WHEN AEP.ArizaAdet is null THEN 0  ELSE AEP.ArizaAdet END)[KalanAdet], " +
                                                "Stok_UrunKodu, " +
                                                "Stok_UrunAdi, " +
                                                "StokAdetTipi_adi, " +
                                                "st.Stok_Id " +
                                          "From tblStok st " +
                                          "left join songiris sn on(sn.CariStokKayit_UrunKodu = st.Stok_Id) " +
                                          "left join tblStokAdetTipi stt on (stt.StokAdetTipi_Id=st.Stok_AdetTipi) " +
                                          "left join KullanilanStok KS on (KS.CariRevizyoSec_Revizyon=St.Stok_Id) " +
                                          "left join ArizaEklenenParca AEP on (AEP.CariAsansorArizaSec_Parca=St.Stok_Id) " + sorgu + "", connect);

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
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblStok where Stok_UrunKodu='" + Text1.Text + "' and Stok_UrunAdi='" + Text2.Text+ "' and Stok_Id!='" + RouteData.Values["id"] + "')  " +
                                            "update tblStok set Stok_UrunKodu='" + Text1.Text + "', Stok_AdetTipi='" + Adet_Drop.SelectedValue + "', Stok_UrunAdi='" + Text2.Text + "', Stok_MarkasiId='"+Marka_Drop.SelectedValue+"' where Stok_Id='" + RouteData.Values["id"] + "'", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblStok where Stok_UrunKodu='" + Text1.Text + "' and Stok_UrunAdi='" + Text2.Text + "') " +
                                            "Begin insert into tblStok(Stok_UrunKodu, Stok_AdetTipi, Stok_UrunAdi, Stok_MarkasiId) values('" + Text1.Text+"', '"+Adet_Drop.SelectedValue+"', '"+Text2.Text+"', '"+Marka_Drop.SelectedValue+"') end", connect);
            }
            sonuc = kayit_ekle.ExecuteNonQuery();
            kayit_ekle.ExecuteNonQuery();

            if (sonuc < 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde benzer stok bulundu.").ToString(), true);
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblStok where Stok_Id='" + RouteData.Values["id"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Adet_Drop.SelectedValue = tablo.Rows[0]["Stok_AdetTipi"].ToString();
                    Text1.Text = tablo.Rows[0]["Stok_UrunKodu"].ToString();
                    Text2.Text = tablo.Rows[0]["Stok_UrunAdi"].ToString();
                    Marka_Drop.SelectedValue = tablo.Rows[0]["Stok_MarkasiId"].ToString();
                }
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

            SqlCommand sil = new SqlCommand("delete from tblStok where Stok_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["id"] != null)
            {
                if (RouteData.Values["id"].ToString() == id.ToString())
                {
                    Response.Redirect("/Stok-Listesi/");
                    return;
                }
            }
            verileri_yukle();
        }
    }

    protected void ara_Click(object sender, EventArgs e)
    {
        verileri_yukle();
    }


    protected void rep1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Stok_Listesi_Yazma") != "1")
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

            SqlCommand sil = new SqlCommand("delete from tblStok where Stok_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["id"] != null)
            {
                if (RouteData.Values["id"].ToString() == id.ToString())
                {
                    Response.Redirect("/Stok-Listesi");
                    return;
                }
            }
            verileri_yukle();
        }
    }

    protected void stop_rep_ItemDataBound(object sender, RepeaterItemEventArgs a)
    {
        if (a.Item.ItemType == ListItemType.Item || a.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton button_panel = (LinkButton)a.Item.FindControl("sil");

            if (buton_yetki == "0")
            {
                button_panel.Visible = false;
            }
        }
    }
}