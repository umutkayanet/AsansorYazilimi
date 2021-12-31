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

public partial class Modul_Revizyon_Ekle : System.Web.UI.Page
{
    string buton_yetki="1";
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (RouteData.Values["m"] != null)
        {
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
        }


        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Revizyon_Listelemee") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Revizyon_Yazma") != "1")
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
            yeni_kayit_buton.Text = "Revizyon Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }
        else
        {
            if (!IsPostBack)
            {
                revizyon_durumu.SelectedIndex = 0;
            }
        }

        //Verileri Yükle
        verileri_yukle();
    }

    string sorgu = "";
    string belge_ozeti = "";
    string select3 = "";
    public void verileri_yukle()
    {
        connect.Open();
        string asansor_arama = "";
        if (Asansor_Arama.SelectedItem.Value == "0")
        {
            asansor_arama = "";
        }
        else
        {
            asansor_arama = " and CariRevizyonlar_Asansor = '" + Asansor_Arama.SelectedItem.Value + "'";
            belge_ozeti += "<b>Asansör: </b>" + Asansor_Arama.SelectedItem.Text + ", ";
        }

        string durumu_arama_sql = "";
        if (Durumu_arama.SelectedItem.Value == "0")
        {
            durumu_arama_sql = "";
        }
        else
        {
            durumu_arama_sql = " and CariRevizyonlar_Durumu = '" + Durumu_arama.SelectedItem.Value + "'";
            belge_ozeti += "<b>Revizyon Durumu: </b> " + Durumu_arama.SelectedItem.Text + " ";
        }


        string tarih_search = "";
        if (tarih_arama.Text != string.Empty)
        {
            tarih_search = " and CariRevizyonlar_Tarih='" + Convert.ToDateTime(tarih_arama.Text).ToString("yyyy.MM.dd") + "'";
            belge_ozeti += "<b>Tarih:</b> " + Convert.ToDateTime(tarih_arama.Text).ToString("dd.MM.yyyy") + ", ";
        }


        string not_arama = "";
        if (not_arama_text.Text != string.Empty)
        {
            not_arama = " and CariRevizyonlar_Not like '%" + not_arama_text.Text + "%'";
            belge_ozeti += "<b>Not Anahtar Kelime :</b> " + not_arama_text.Text + ", ";
        }

        sorgu = " where CariSU_CariNo='" + RouteData.Values["id"] + "' " + asansor_arama + durumu_arama_sql + tarih_search + not_arama + "";
        select3 = "WITH " +
                  "songiris AS(Select Count(CariRevizyoSec_RevizyonId)[Toplam Parça], CariRevizyoSec_RevizyonId from tblCariRevizyonSec group by CariRevizyoSec_RevizyonId) " +
                  "Select ck.Cari_Unvan[Ünvan], cas.CariSU_Tanimi[Asansör Tanımı], drm.Durumu_Aciklama, cbk.CariRevizyonlar_Id, cbk.CariRevizyonlar_Tarih[Tarih], " +
                  " drm.Durumu_Style, sn.[Toplam Parça] " +
                  "From tblCariRevizyonlar cbk " +
                  "Left join tblCariAsansorler cas on(cbk.CariRevizyonlar_Asansor = cas.CariSU_Id) " +
                  "left join tblCariKayit ck on(ck.Cari_Id = cas.CariSU_CariNo) " +
                  "left join tblDurumu drm on(cbk.CariRevizyonlar_Durumu = drm.Durumu_Id) " +
                  "left join songiris sn on(sn.CariRevizyoSec_RevizyonId = cbk.CariRevizyonlar_Id)" + sorgu;


        SqlCommand cari_bul = new SqlCommand("Select * from tblCariKayit where Cari_Id='" + RouteData.Values["id"] + "'", connect);
        SqlDataReader reader;
        reader = cari_bul.ExecuteReader();
        reader.Read();
        Cari_Label.Text = reader["Cari_Unvan"].ToString();


        //Sistemdeki Duyurları Yukle
        SqlDataAdapter komut = new SqlDataAdapter(select3, connect);
        DataTable tablo = new DataTable();
        komut.Fill(tablo);
        rep1.DataSource = tablo;
        rep1.DataBind();

        connect.Close();
        cari_bul.Dispose();
        komut.Dispose();
    }

    int sonuc;
    SqlCommand komut;
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {

            connect.Open();
            SqlCommand kayit_ekle;

            //Veri Varsa Güncelle Yoksa ekle
            if (RouteData.Values["idd"] != null)
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariRevizyonlar where CariRevizyonlar_Asansor='" + Drop1.SelectedValue + "' and CariRevizyonlar_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "' and CariRevizyonlar_Id!='" + RouteData.Values["idd"] + "')  " +
                                            "update tblCariRevizyonlar set CariRevizyonlar_Asansor='" + Drop1.SelectedValue + "', CariRevizyonlar_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "', CariRevizyonlar_Durumu='" + revizyon_durumu.SelectedValue + "', CariRevizyonlar_Not='" + t4.Text + "' where CariRevizyonlar_Id='" + RouteData.Values["idd"] + "'", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariRevizyonlar where CariRevizyonlar_Asansor='" + Drop1.SelectedValue + "' and CariRevizyonlar_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "') " +
                                            "Begin insert into tblCariRevizyonlar(CariRevizyonlar_Asansor, CariRevizyonlar_Tarih, CariRevizyonlar_Durumu, CariRevizyonlar_Not) values('" + Drop1.SelectedValue + "', '" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "', '" + revizyon_durumu.SelectedItem.Value + "', '" + t4.Text + "') end", connect);
            }
            sonuc = kayit_ekle.ExecuteNonQuery();
            kayit_ekle.ExecuteNonQuery();


            if (sonuc < 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde aynı tarihli revizyon bulundu.").ToString(), true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            }

            //Kayıt işleminden sonra eklenen bakımları id değerini bul
            SqlDataAdapter duyuru_bul;
            if (RouteData.Values["idd"] != null)
            {
                duyuru_bul = new SqlDataAdapter("Select * from tblCariRevizyonlar where CariRevizyonlar_Id='" + RouteData.Values["idd"] + "'", connect);
            }
            else
            {
                duyuru_bul = new SqlDataAdapter("Select * from tblCariRevizyonlar where CariRevizyonlar_Asansor='" + Drop1.SelectedValue + "' and CariRevizyonlar_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "'", connect);
            }

            DataTable duyuru_bultab = new DataTable();
            duyuru_bul.Fill(duyuru_bultab);


            //Personel Seçenekleri Döngüsünü başlat
            
            foreach (ListItem item in gorevli_personel.Items)
            {
                //Eğer Bakım seçili ise
                if (item.Selected)
                {
                    //Sistemde varsa dokunma yoksa ekle
                    komut = new SqlCommand("IF NOT Exists (select * from tblCariRevizyonGP where CariRevizyonGP_GorevId='" + duyuru_bultab.Rows[0]["CariRevizyonlar_Id"].ToString() + "' and CariRevizyonGP_Persid='" + item.Value + "') Begin " +
                                                      "insert into tblCariRevizyonGP(CariRevizyonGP_GorevId, CariRevizyonGP_Persid)values('" + duyuru_bultab.Rows[0]["CariRevizyonlar_Id"].ToString() + "', '" + item.Value + "') end", connect);
                    komut.ExecuteNonQuery();
                }
                else
                {
                    //Eğer seçilen bölüm kaldırılmışsa sistemden sil
                    komut = new SqlCommand("Delete From tblCariRevizyonGP Where CariRevizyonGP_Persid='" + item.Value + "' and CariRevizyonGP_GorevId='" + duyuru_bultab.Rows[0]["CariRevizyonlar_Id"].ToString() + "'", connect);
                    komut.ExecuteNonQuery();
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            connect.Close();
            kayit_ekle.Dispose();
            duyuru_bul.Dispose();
            komut.Dispose();
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariRevizyonlar where CariRevizyonlar_Id='" + RouteData.Values["idd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Drop1.SelectedValue = tablo.Rows[0]["CariRevizyonlar_Asansor"].ToString();
                    t3.Text = Convert.ToDateTime(tablo.Rows[0]["CariRevizyonlar_Tarih"]).ToString("dd.MM.yyyy");
                    t4.Text = tablo.Rows[0]["CariRevizyonlar_Not"].ToString();


                    revizyon_durumu.DataBind();
                    foreach (ListItem li in revizyon_durumu.Items)
                    {
                        if (li.Value == tablo.Rows[0]["CariRevizyonlar_Durumu"].ToString())
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari Stok Bilgileri Alınamadı").ToString(), true);
                }


                //Bakım İşaretlemesini Yağ
                SqlDataAdapter goreve_giden_personel = new SqlDataAdapter("select * from tblCariRevizyonGP where CariRevizyonGP_GorevId='" + RouteData.Values["idd"].ToString() + "'", connect);
                DataTable goreve_giden_personeltb = new DataTable();
                goreve_giden_personel.Fill(goreve_giden_personeltb);

                if (goreve_giden_personeltb.Rows.Count > 0)
                {
                    gorevli_personel.DataBind();
                    for (int i = 0; i < goreve_giden_personeltb.Rows.Count; i++)
                    {
                        foreach (ListItem dongu in gorevli_personel.Items)
                        {
                            if (dongu.Value.ToString() == goreve_giden_personeltb.Rows[i]["CariRevizyonGP_Persid"].ToString())
                            {
                                dongu.Selected = true;
                            }
                        }
                    }
                }
                connect.Close();
                komut.Dispose();
                goreve_giden_personel.Dispose();
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

            SqlCommand sil = new SqlCommand("delete from tblCariRevizyonlar where CariRevizyonlar_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["idd"] != null)
            {
                if (RouteData.Values["idd"].ToString() == id.ToString())
                {
                    Response.Redirect("/Servis/Revizyon-Ekle/" + RouteData.Values["id"] + "");
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
                select3 = select3.Replace("cbk.CariRevizyonlar_Id,", "");
                select3 = select3.Replace("drm.Durumu_Style,", "");
                select3 = select3.Replace("cbk.CariRevizyonlar_Durumu,", "");
                
                using (SqlCommand cmd = new SqlCommand(select3))

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt, "Revizyon Listesi");

                            //ws.Cell(1, 6).Value = "Service";
                            //ws.Cell(1, 15).Value = "Invoice";

                            //ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            //ws.Range("M1:Q1").Style.Fill.BackgroundColor = XLColor.DarkCandyAppleRed;
                            ws.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.FromHtml("#3A4B55");

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
                            Response.AddHeader("content-disposition", "attachment;filename=Revizyon_Listesi" + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                    cmd.Dispose();
                }
                con.Close();
            }
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("basari", "");
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "Aktarım Tamamlanamadı.");
        }
    }


    protected void rep1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //Güvenlik
        if (buton_yetki != "1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
        //Güvenlik Son
    }
}