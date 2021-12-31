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

public partial class Modul_Marka_Listesi : System.Web.UI.Page
{
    string buton_yetki;
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Marka_Listesi_Okuma") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("MArka_Listesi_Yazma") != "1")
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
            yeni_kayit_buton.Text = "MArka Düzenle";

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

        string Marka_Arama = "";
        if (Marka_Ara.Text != string.Empty)
        {
            Marka_Arama = " Marka_Adi LIKE '%" + Marka_Ara.Text + "%' ";
            sorgu = " where " + Marka_Arama + "";
            belge_ozeti += "<b>Cari Kodu :</b> " + Marka_Ara.Text + ", ";
        }


        
        SqlCommand komut = new SqlCommand("Select * From tblMarkaListesi" + sorgu + "", connect);
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
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblMarkaListesi where Marka_Adi='" + Text1.Text + "' and Marka_Id!='" + RouteData.Values["id"] + "')  " +
                                            "update tblMarkaListesi set Marka_Adi='" + Text1.Text + "' where Marka_Id='" + RouteData.Values["id"] + "'", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblMarkaListesi where Marka_Adi='" + Text1.Text + "') " +
                                            "Begin insert into tblMarkaListesi(Marka_Adi) values('" + Text1.Text+"') end", connect);
            }
            sonuc = kayit_ekle.ExecuteNonQuery();
            kayit_ekle.ExecuteNonQuery();

            if (sonuc < 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde benzer marka bulundu.").ToString(), true);
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblMarkaListesi where Marka_Id='" + RouteData.Values["id"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Text1.Text = tablo.Rows[0]["Marka_Adi"].ToString();
                }
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Bilgiler ekrana getirilemedi.").ToString(), true);
            return;
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
                using (SqlCommand cmd = new SqlCommand("Select ass.CariSU_Tanimi[Asansör Tanımı], ar.CariAsansorAriza_Aciklama[Arıza Açıklama], ar.CariAsansorAriza_Not[Arıza Not], ar.CariAsansorAriza_Durumu[Durumu], ar.CariAsansorAriza_Tarih[Arıza Tarihi], ar.CariAsansorAriza_KayitTarih[Kayıt Tarihi] from tblCariAsansorAriza ar "+
                                                       "left join tblCariAsansorler ass on(ass.CariSU_Id = ar.CariAsansorAriza_AsaId) " + sorgu))


                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt, "Arıza Listesi");

                            //ws.Cell(1, 6).Value = "Service";
                            //ws.Cell(1, 15).Value = "Invoice";

                            //ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            //ws.Range("M1:Q1").Style.Fill.BackgroundColor = XLColor.DarkCandyAppleRed;
                            ws.Range("A1:G1").Style.Fill.BackgroundColor = XLColor.FromHtml("#3A4B55");

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
                            Response.AddHeader("content-disposition", "attachment;filename=Ariza_Listesi" + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx");
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

    protected void rep1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Marka_Listesi_Yazma") != "1")
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

            SqlCommand sil = new SqlCommand("delete from tblMarkaListesi where Marka_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["id"] != null)
            {
                if (RouteData.Values["id"].ToString() == id.ToString())
                {
                    Response.Redirect("/Marka-Listesi");
                    return;
                }
            }
            verileri_yukle();
        }
    }
}