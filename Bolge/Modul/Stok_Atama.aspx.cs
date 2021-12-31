using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AjaxControlToolkit;
using ClosedXML.Excel;
using System.IO;

public partial class Modul_Stok_Atama : System.Web.UI.Page
{
    string buton_yetki;
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (RouteData.Values["m"] != null)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
        }

        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Stok_Atama_Okuma") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Stok_Atama_Yazma") != "1")
        {
            buton_yetki = "0";
            buttonlar_panel.Visible = false;
            g_panel.Visible = false;
        }
        //Güvenlik Son
        

        if (RouteData.Values["idd"] != null)
        {
            //Duyuru Bilgilerini Ekrana Getir.
            bilgi_getir();

            //Güncelleme sayfası açıkken yeni asansör için butonu yarat
            buttonlar_panel.Visible = true;

            //Güncelleme sayfası açıkken buton adını güncelle olarak değiştir.
            kayit.Text = "Güncelle";

            //Düzenleme Sayfasında Başlık Değişimi
            yeni_kayit_buton.Text = "Duyuru Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }

        //Verileri Yükle
        verileri_yukle();
    }


    string sorgu = "";
    string select = "";
    string belge_ozeti = "";
    public void verileri_yukle()
    {
        connect.Open();
        try
        {
            string unvan_ara = "";
            if (ara_unvan.SelectedValue != "0")
            {
                unvan_ara = " and CariStokKayit_CariId = '" + ara_unvan.SelectedValue + "' ";
                belge_ozeti += "<b>Ünvan :</b> " + ara_unvan.SelectedItem.Text + ", ";
            }

            string urun_ara = "";
            if (urun_adi_ara.SelectedValue != "0")
            {
                urun_ara = " and Stok_Id = '" + urun_adi_ara.SelectedValue + "' ";
                belge_ozeti += "<b>Ürün :</b> " + urun_adi_ara.SelectedItem.Text + ", ";
            }

            sorgu = " where CariStokKayit_Id != '' " + unvan_ara + urun_ara + " order by CariStokKayit_Id";
            select="Select sk.CariStokKayit_Id, ck.Cari_Unvan, st.Stok_Id, st.Stok_UrunKodu, st.Stok_UrunAdi, sk.CariStokKayit_Adet, adt.StokAdetTipi_adi, sk.CariStokKayit_Fiyat, sk.CariStokKayit_KayitTarih from tblCariStokKayit sk " +
                   "left join tblStok st on(sk.CariStokKayit_UrunKodu = st.Stok_Id) " +
                   "Left Join tblStokAdetTipi adt on(adt.StokAdetTipi_Id = st.Stok_AdetTipi) " +
                   "Left Join tblCariKayit ck on(ck.Cari_Id = sk.CariStokKayit_CariId) " + sorgu + " ";


            //Sistemdeki Duyurları Yukle
            SqlDataAdapter komut = new SqlDataAdapter(select, connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);
            rep1.DataSource = tablo;
            rep1.DataBind();            
        }
        catch (Exception)
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Asansör Listesi Yüklenemedi.").ToString(), true);
           return;
        }
        connect.Close();
    }


    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            connect.Open();

            string bakim_ucreti = "";
            bakim_ucreti = t3.Text.Replace(".", "");
            bakim_ucreti = bakim_ucreti.Replace(",", ".");

            SqlCommand kayit_ekle;
            //Veri Varsa Güncelle Yoksa ekle
            if (RouteData.Values["idd"] != null)
            {
                kayit_ekle = new SqlCommand("update tblCariStokKayit set CariStokKayit_UrunKodu='" + urun_drop.SelectedValue + "', CariStokKayit_Adet='" + t2.Text + "', CariStokKayit_Fiyat='" + String.Format("{0:c}", bakim_ucreti) + "', CariStokKayit_CariId='" + unvan_drop.SelectedValue + "' where CariStokKayit_Id='" + RouteData.Values["idd"] + "'", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("insert into tblCariStokKayit(CariStokKayit_UrunKodu, CariStokKayit_Adet, CariStokKayit_CariId, CariStokKayit_KayitTarih, CariStokKayit_Fiyat) values('" + urun_drop.SelectedValue + "', '" + t2.Text + "', '" + unvan_drop.SelectedValue + "', '" + DateTime.Now.ToString("yyyy.MM.dd") + "', '" + String.Format("{0:c}", bakim_ucreti) + "')", connect);
            }
            kayit_ekle.ExecuteNonQuery();
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariStokKayit where CariStokKayit_Id='" + RouteData.Values["idd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    unvan_drop.SelectedValue = tablo.Rows[0]["CariStokKayit_CariId"].ToString();
                    urun_drop.SelectedValue = tablo.Rows[0]["CariStokKayit_UrunKodu"].ToString();
                    t2.Text = tablo.Rows[0]["CariStokKayit_Adet"].ToString();
                    t3.Text = Convert.ToDecimal(tablo.Rows[0]["CariStokKayit_Fiyat"]).ToString("N");
                }
                else
                {
                   ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari Stok Bilgileri Alınamadı").ToString(), true);
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

            SqlCommand sil = new SqlCommand("delete from tblCariStokKayit where CariStokKayit_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["idd"] != null)
            {
                if (RouteData.Values["idd"].ToString() == id.ToString())
                {
                   Response.Redirect("/Cari-Stok-Kayit/" +RouteData.Values["gorev"] +"/"+ RouteData.Values["id"] + "");
                   return;
                }
            }
            verileri_yukle();
        }
    }

    protected void rep1_ItemDataBound(object sender, RepeaterItemEventArgs a)
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

    protected void ara_Click(object sender, EventArgs e)
    {
        verileri_yukle();
    }


    protected void excel_export_Click(object sender, EventArgs e)
    {
        try
        {
            string connect = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(select))


                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt, "Cari Stok Listesi");

                            //ws.Cell(1, 6).Value = "Service";
                            //ws.Cell(1, 15).Value = "Invoice";

                            //ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            //ws.Range("M1:Q1").Style.Fill.BackgroundColor = XLColor.DarkCandyAppleRed;
                            ws.Range("A1:AC1").Style.Fill.BackgroundColor = XLColor.FromHtml("#3A4B55");

                            int son_kayit = dt.Rows.Count + 4;
                            ws.Cell(son_kayit, 1).Value = "Toplam Kayıt";
                            ws.Cell(son_kayit, 1).Style.Font.Bold = true;
                            ws.Cell(son_kayit, 2).Value = dt.Rows.Count.ToString();

                            ws.Cell(son_kayit + 1, 1).Value = "Oluşturulma Zamanı";
                            ws.Cell(son_kayit + 1, 1).Style.Font.Bold = true;
                            ws.Cell(son_kayit + 1, 2).Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                            ws.Cell(son_kayit + 2, 1).Value = "Kullanıcı";
                            ws.Cell(son_kayit + 2, 1).Style.Font.Bold = true;
                            ws.Cell(son_kayit + 2, 2).Value = Server.UrlDecode(Request.Cookies["RcEU"]["Kullanici_Adi"]);

                            ws.Cell(son_kayit + 3, 1).Value = "Belge Özeti";
                            ws.Cell(son_kayit + 3, 1).Style.Font.Bold = true;
                            belge_ozeti = belge_ozeti.Replace("<b>", "");
                            belge_ozeti = belge_ozeti.Replace("</br>", "");
                            belge_ozeti = belge_ozeti.Replace("</b>", "");
                            ws.Cell(son_kayit + 3, 2).Value = belge_ozeti;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=Bakim_Listesi" + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                }
            }
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("basari", "");
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "Aktarım Tamamlanamadı.");
        }
    }
}