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

public partial class Modul_Ariza_Ekle : System.Web.UI.Page
{
    string icguvenlik_gorev="1";
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Cari_Ariza_Listeleme") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Cari_Ariza_Yazma") != "1")
        {
            g_panel.Visible = false;
            icguvenlik_gorev = "0";
        }
        //Güvenlik Son


        if (RouteData.Values["m"] != null)
        {
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
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
            yeni_kayit_buton.Text = "Arıza Düzenle";

            //Düzenleme sayfasında tablo renk değişimi
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
        }
        else
        {
            if (!IsPostBack)
            {
                durumu.SelectedIndex = 0;
            }
        }

        //Verileri Yükle
        verileri_yukle();
    }

    string sorgu = "";
    string belge_ozeti = "";
    string select2, select3 = "";
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
            asansor_arama = " and CariAsansorAriza_AsaId = '" + Asansor_Arama.SelectedItem.Value + "'";
            belge_ozeti += "<b>Asansör: </b>" + Asansor_Arama.SelectedItem.Text + ", ";
        }


        string tarih_search = "";
        if (tarih_arama.Text != string.Empty)
        {
            tarih_search = " and CariAsansorAriza_Tarih='" + Convert.ToDateTime(tarih_arama.Text).ToString("yyyy.MM.dd") + "'";
            belge_ozeti += "<b>Arıza Tarihi :</b> " + Convert.ToDateTime(tarih_arama.Text).ToString("dd.MM.yyyy") + ", ";
        }

        string durumu_arama_sql = "";
        if (Durumu_arama.SelectedItem.Value == "0")
        {
            durumu_arama_sql = "";
        }
        else
        {
            durumu_arama_sql = " and CariAsansorAriza_Durumu = '"+ Durumu_arama.SelectedItem.Value + "'";
            belge_ozeti += "<b>Durumu: </b> "+ Durumu_arama.SelectedItem.Text + ", ";
        }

        string not_arama = "";
        if (not_arama_text.Text != string.Empty)
        {
            not_arama = " and CariAsansorAriza_Not like '%" + not_arama_text.Text + "%'";
            belge_ozeti += "<b>Not Anahtar Kelime :</b> " + not_arama_text.Text + ", ";
        }

        
        sorgu = " where CariSU_CariNo='"+ RouteData.Values["id"] + "' " + asansor_arama + tarih_search + durumu_arama_sql + not_arama + "";
        select3 = "WITH songiris AS" +
                  "(Select Count(CariAsansorArizaSec_ArizaId)[Toplam Parça], CariAsansorArizaSec_ArizaId from tblCariAsansorArizaSec group by CariAsansorArizaSec_ArizaId) "+
                  "Select ck.Cari_Unvan, asn.CariSU_Tanimi[Asansör Tanımı], ar.CariAsansorAriza_Aciklama[Arıza Açıklaması], dr.Durumu_Aciklama, " +
                  "dr.Durumu_Style[Durum Style], ar.CariAsansorAriza_Id, sn.[Toplam Parça], ar.CariAsansorAriza_Tarih[Arıza Tarihi] " +
                  "from tblCariAsansorAriza ar  left join tblCariAsansorler asn on(ar.CariAsansorAriza_AsaId = asn.CariSU_Id) "+
                  "left join tblDurumu dr on(dr.Durumu_Id = ar.CariAsansorAriza_Durumu) "+
                  "left join tblCariKayit ck on(ck.Cari_Id = asn.CariSU_CariNo) "+
                  "left join songiris sn on(sn.CariAsansorArizaSec_ArizaId = ar.CariAsansorAriza_Id)" + sorgu;

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
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariAsansorAriza where CariAsansorAriza_AsaId='" + Drop1.SelectedValue + "' and CariAsansorAriza_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "' and CariAsansorAriza_Id!='" + RouteData.Values["idd"] + "')  " +
                                            "update tblCariAsansorAriza set CariAsansorAriza_AsaId='" + Drop1.SelectedValue + "', CariAsansorAriza_Aciklama='" + t9.Text + "', CariAsansorAriza_Not='" + t4.Text + "', CariAsansorAriza_Durumu='" + durumu.SelectedValue + "', CariAsansorAriza_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "' where CariAsansorAriza_Id='" + RouteData.Values["idd"] + "'", connect);


            }
            else
            {
                kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariAsansorAriza where CariAsansorAriza_AsaId='" + Drop1.SelectedValue + "' and CariAsansorAriza_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "') " +
                                            "Begin insert into tblCariAsansorAriza(CariAsansorAriza_AsaId, CariAsansorAriza_Aciklama, CariAsansorAriza_Not, CariAsansorAriza_Durumu, CariAsansorAriza_Tarih, CariAsansorAriza_KayitTarih) values('" + Drop1.SelectedValue + "', '" + t9.Text + "', '" + t4.Text + "', '" + durumu.SelectedValue + "', '" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy.MM.dd HH:mm")) + "') end", connect);
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
                duyuru_bul = new SqlDataAdapter("Select * from tblCariAsansorAriza where CariAsansorAriza_Id='" + RouteData.Values["idd"] + "'", connect);
            }
            else
            {
                duyuru_bul = new SqlDataAdapter("Select * from tblCariAsansorAriza where CariAsansorAriza_AsaId='" + Drop1.SelectedValue + "' and CariAsansorAriza_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "'", connect);
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
                    komut = new SqlCommand("IF NOT Exists (select * from tblCariAsansorArizaGP where CariArizaGP_GorevId='" + duyuru_bultab.Rows[0]["CariAsansorAriza_Id"].ToString() + "' and CariArizaGP_Persid='" + item.Value + "') Begin " +
                                                      "insert into tblCariAsansorArizaGP(CariArizaGP_GorevId, CariArizaGP_Persid)values('" + duyuru_bultab.Rows[0]["CariAsansorAriza_Id"].ToString() + "', '" + item.Value + "') end", connect);
                    komut.ExecuteNonQuery();
                }
                else
                {
                    //Eğer seçilen bölüm kaldırılmışsa sistemden sil
                    komut = new SqlCommand("Delete From tblCariAsansorArizaGP Where CariArizaGP_Persid='" + item.Value + "' and CariArizaGP_GorevId='" + duyuru_bultab.Rows[0]["CariAsansorAriza_Id"].ToString() + "'", connect);
                    komut.ExecuteNonQuery();
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            connect.Close();
            kayit_ekle.Dispose();
            komut.Dispose();
            verileri_yukle();

        }
        catch (Exception EX)
        {
            Label1.Text = EX.Message.ToString();
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariAsansorAriza where CariAsansorAriza_Id='" + RouteData.Values["idd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Drop1.SelectedValue = tablo.Rows[0]["CariAsansorAriza_AsaId"].ToString();
                    t3.Text = Convert.ToDateTime(tablo.Rows[0]["CariAsansorAriza_Tarih"]).ToString("dd.MM.yyyy");
                    t4.Text = tablo.Rows[0]["CariAsansorAriza_Not"].ToString();
                    t9.Text = tablo.Rows[0]["CariAsansorAriza_Aciklama"].ToString();

                    durumu.DataBind();
                    foreach (ListItem li in durumu.Items)
                    {
                        if (li.Value == tablo.Rows[0]["CariAsansorAriza_Durumu"].ToString())
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
                SqlDataAdapter goreve_giden_personel = new SqlDataAdapter("select * from tblCariAsansorArizaGP where CariArizaGP_GorevId='" + RouteData.Values["idd"].ToString() + "'", connect);
                DataTable goreve_giden_personeltb = new DataTable();
                goreve_giden_personel.Fill(goreve_giden_personeltb);

                if (goreve_giden_personeltb.Rows.Count > 0)
                {
                    gorevli_personel.DataBind();
                    for (int i = 0; i < goreve_giden_personeltb.Rows.Count; i++)
                    {
                        foreach (ListItem dongu in gorevli_personel.Items)
                        {
                            if (dongu.Value.ToString() == goreve_giden_personeltb.Rows[i]["CariArizaGP_Persid"].ToString())
                            {
                                dongu.Selected = true;
                            }
                        }
                    }
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

            SqlCommand sil = new SqlCommand("delete from tblCariAsansorAriza where CariAsansorAriza_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();
            sil.Dispose();

            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["idd"] != null)
            {
                if (RouteData.Values["idd"].ToString() == id.ToString())
                {
                    Response.Redirect("/Servis/Ariza-Listesi/" + RouteData.Values["id"] + "");
                    return;
                }
            }
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
                select3 = select3.Replace("ar.CariAsansorAriza_Id,", "");
                select3 = select3.Replace("dr.Durumu_Style[Durum Style],", "");
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
                            var ws = wb.Worksheets.Add(dt, "Arıza Listesi");

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
        if (icguvenlik_gorev != "1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
        //Güvenlik Son
    }
}