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

public partial class Modul_Bakim_Ekle : System.Web.UI.Page
{
    string icguvenlik_gorev = "1";
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Bakim_Listeleme") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Bakim_Yazma") != "1")
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

            //Güncelleme sayfasında bakım planı değişiliği yaptırma
            bakim_plan_panel.Visible = false;

            //Güncelleme sayfası açıkken yeni asansör için butonu yarat
            buttonlar_panel.Visible = true;

            //Güncelleme sayfası açıkken buton adını güncelle olarak değiştir.
            kayit.Text = "Güncelle";

            //Düzenleme Sayfasında Başlık Değişimi
            yeni_kayit_buton.Text = "Bakım Düzenle";

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
    string select1, select2, select3;
    public void verileri_yukle()
    {
        try
        {
            connect.Open();
            string asansor_arama = "";
            if (Asansor_Arama.SelectedItem.Value == "0")
            {
                asansor_arama = "";
            }
            else
            {
                asansor_arama = " and CariBakimlar_Asansor = '" + Asansor_Arama.SelectedItem.Value + "'";
                belge_ozeti += "<b>Asansör: </b>" + Asansor_Arama.SelectedItem.Text + ", ";
            }

            string durumu_arama_sql = "";
            if (Durumu_arama.SelectedItem.Value == "0")
            {
                durumu_arama_sql = "";
            }
            else if (Durumu_arama.SelectedItem.Value == "True")
            {
                durumu_arama_sql = " and CariBakimlar_Durumu = '0'";
                belge_ozeti += "<b>Durumu: </b> Açık Bakımlar, ";
            }
            else if (Durumu_arama.SelectedItem.Value == "False")
            {
                durumu_arama_sql = " and CariBakimlar_Durumu = '1'";
                belge_ozeti += "<b>Durumu: </b> Kapalı Bakımlar, ";
            }


            string tarih_search = "";
            if (tarih_arama.Text != string.Empty)
            {
                tarih_search = " and CariBakimlar_Tarih='"+ Convert.ToDateTime(tarih_arama.Text).ToString("yyyy.MM.dd") + "'";
                belge_ozeti += "<b>Tarih:</b> " + Convert.ToDateTime(tarih_arama.Text).ToString("dd.MM.yyyy") + ", ";
            }


            string not_arama = "";
            if (not_arama_text.Text != string.Empty)
            {
                not_arama = " and CariBakimlar_Not like '%" + not_arama_text.Text + "%'";
                belge_ozeti += "<b>Not Anahtar Kelime :</b> " + not_arama_text.Text + ", ";
            }


            sorgu = " where CariSU_CariNo='"+ RouteData.Values["id"] + "' " + asansor_arama + durumu_arama_sql + tarih_search + not_arama + "";
            select1 = "Select ck.Cari_Unvan, cbk.CariBakimlar_Id, cas.CariSU_CariNo, cbk.CariBakimlar_Asansor, cbk.CariBakimlar_Tarih, cas.CariSU_Tanimi ";
            select2 = "Select ck.Cari_Unvan[Ünvan], cas.CariSU_Tanimi[Asansör Tanımı], cbk.CariBakimlar_Tarih[Bakım Tarihi], cbk.CariBakimlar_Not[Bakım Notu] ";
            select3 = "From tblCariBakimlar cbk " +
                      "Left join tblCariAsansorler cas on(cbk.CariBakimlar_Asansor = cas.CariSU_Id)  " +
                      "left join tblCariKayit ck on(ck.Cari_Id=cas.CariSU_CariNo)" + sorgu + "";

            SqlCommand cari_bul = new SqlCommand("Select * from tblCariKayit where Cari_Id='"+ RouteData.Values["id"] + "'", connect);
            SqlDataReader reader;
            reader = cari_bul.ExecuteReader();
            reader.Read();
            Cari_Label.Text = reader["Cari_Unvan"].ToString();


            //Sistemdeki Bakımları Yukle
            SqlDataAdapter komut = new SqlDataAdapter(select1 + select3, connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);
            rep1.DataSource = tablo;
            rep1.DataBind();

            connect.Close();
            cari_bul.Dispose();
            komut.Dispose();
        }
        catch (Exception)
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Asansör Listesi Yüklenemedi.").ToString(), true);
           return;
        }
       
    }

    int sonuc;
    string para;
    SqlCommand komut;
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            connect.Open();
            int dongu_adet = 1;
            SqlCommand kayit_ekle;
            DateTime umut = Convert.ToDateTime(t3.Text).AddMonths(-1);

            if (RouteData.Values["idd"] != null)
            {
                dongu_adet = 1;
            }
            else
            {
                dongu_adet = Convert.ToInt32(bakim_plan_drop.SelectedValue);
            }
            for (int i = 0; i < dongu_adet ; i++)
            {
                umut = umut.AddMonths(1);
                string bakim_ucreti = "";
                bakim_ucreti= t5.Text.Replace(".", "");
                bakim_ucreti = bakim_ucreti.Replace(",", ".");

                //Veri Varsa Güncelle Yoksa ekle
                if (RouteData.Values["idd"] != null)
                {
                    kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariBakimlar where CariBakimlar_Asansor='" + Drop1.SelectedValue + "' and CariBakimlar_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "' and CariBakimlar_Id!='" + RouteData.Values["idd"] + "')  " +
                                                "update tblCariBakimlar set CariBakimlar_Asansor='" + Drop1.SelectedValue + "', CariBakimlar_Tarih='" + Convert.ToDateTime(t3.Text).ToString("yyyy.MM.dd") + "', CariBakimlar_Durumu='" + durumu.SelectedValue + "', CariBakimlar_Not='" + t4.Text + "', CariBakimlar_Bedel='" + String.Format("{0:c}", bakim_ucreti) + "' where CariBakimlar_Id='" + RouteData.Values["idd"] + "'", connect);
                }
                else
                {
                    kayit_ekle = new SqlCommand("IF Not Exists (select * from tblCariBakimlar where CariBakimlar_Asansor='" + Drop1.SelectedValue + "' and CariBakimlar_Tarih='" + umut.ToString("yyyy.MM.dd") + "') " +
                                                "Begin insert into tblCariBakimlar(CariBakimlar_Asansor, CariBakimlar_Tarih, CariBakimlar_Durumu, CariBakimlar_Not, CariBakimlar_Bedel) values('" + Drop1.SelectedValue + "', '" + umut.ToString("yyyy.MM.dd") + "', '" + durumu.SelectedValue + "', '" + t4.Text + "', '" + String.Format("{0:c}", bakim_ucreti) + "') end", connect);
                }
                sonuc = kayit_ekle.ExecuteNonQuery();
                kayit_ekle.ExecuteNonQuery();


                if (sonuc < 0)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sistemde aynı tarihli bakım bulundu.").ToString(), true);
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
                    duyuru_bul = new SqlDataAdapter("Select * from tblCariBakimlar where CariBakimlar_Id='" + RouteData.Values["idd"] + "'", connect);
                }
                else
                {
                    duyuru_bul = new SqlDataAdapter("Select * from tblCariBakimlar where CariBakimlar_Asansor='" + Drop1.SelectedValue + "' and CariBakimlar_Tarih='" + umut.ToString("yyyy.MM.dd") + "'", connect);
                }

                DataTable duyuru_bultab = new DataTable();
                duyuru_bul.Fill(duyuru_bultab);


                //Bakım Seçenekleri Döngüsünü başlat
                foreach (ListItem item in bakim_secenekleri.Items)
                {
                    //Eğer Bakım seçili ise
                    if (item.Selected)
                    {
                        //Sistemde varsa dokunma yoksa ekle
                        SqlCommand komut = new SqlCommand("IF NOT Exists (select * from tblCariBakimSec where CariBakimSec_Bakim='" + item.Value + "' and CariBakimSec_BakimId='" + duyuru_bultab.Rows[0]["CariBakimlar_Id"].ToString() + "') Begin " +
                                                          "insert into tblCariBakimSec(CariBakimSec_Bakim, CariBakimSec_BakimId)values('" + item.Value + "', '" + duyuru_bultab.Rows[0]["CariBakimlar_Id"].ToString() + "') end", connect);
                        komut.ExecuteNonQuery();
                    }
                    else
                    {
                        //Eğer seçilen bölüm kaldırılmışsa sistemden sil
                        SqlCommand komut = new SqlCommand("Delete From tblCariBakimSec Where CariBakimSec_Bakim='" + item.Value + "' and CariBakimSec_BakimId='" + duyuru_bultab.Rows[0]["CariBakimlar_Id"].ToString() + "'", connect);
                        komut.ExecuteNonQuery();
                    }
                }


                //Personel Seçenekleri Döngüsünü başlat
                foreach (ListItem item in gorevli_personel.Items)
                {
                    //Eğer Bakım seçili ise
                    if (item.Selected)
                    {
                        //Sistemde varsa dokunma yoksa ekle
                        SqlCommand komut = new SqlCommand("IF NOT Exists (select * from tblCariBakimlarGP where CariBakimlarGP_GorevId='" + duyuru_bultab.Rows[0]["CariBakimlar_Id"].ToString() + "' and CariBakimlarGP_Persid='" + item.Value + "') Begin " +
                                                          "insert into tblCariBakimlarGP (CariBakimlarGP_GorevId, CariBakimlarGP_Persid)values('" + duyuru_bultab.Rows[0]["CariBakimlar_Id"].ToString() + "', '" + item.Value + "') end", connect);
                        komut.ExecuteNonQuery();
                    }
                    else
                    {
                        //Eğer seçilen bölüm kaldırılmışsa sistemden sil
                        SqlCommand komut = new SqlCommand("Delete From tblCariBakimlarGP Where CariBakimlarGP_Persid='" + item.Value + "' and CariBakimlarGP_GorevId='" + duyuru_bultab.Rows[0]["CariBakimlar_Id"].ToString() + "'", connect);
                        komut.ExecuteNonQuery();
                    }
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            connect.Close();
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariBakimlar where CariBakimlar_Id='" + RouteData.Values["idd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Drop1.SelectedValue = tablo.Rows[0]["CariBakimlar_Asansor"].ToString();
                    t3.Text = Convert.ToDateTime(tablo.Rows[0]["CariBakimlar_Tarih"]).ToString("dd.MM.yyyy");                    
                    t4.Text = tablo.Rows[0]["CariBakimlar_Not"].ToString();
                    t5.Text = Convert.ToDecimal(tablo.Rows[0]["CariBakimlar_Bedel"]).ToString("N");

                    durumu.DataBind();
                    foreach (ListItem li in durumu.Items)
                    {
                        if (li.Value == tablo.Rows[0]["CariBakimlar_Durumu"].ToString())
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
                SqlDataAdapter dil_bul = new SqlDataAdapter("select * from tblCariBakimSec where CariBakimSec_BakimId='" + RouteData.Values["idd"].ToString() + "'", connect);
                DataTable dil_bul_table = new DataTable();
                dil_bul.Fill(dil_bul_table);

                if (dil_bul_table.Rows.Count>0)
                {
                    bakim_secenekleri.DataBind();
                    for (int i = 0; i < dil_bul_table.Rows.Count; i++)
                    {
                        foreach (ListItem dongu in bakim_secenekleri.Items)
                        {
                            if (dongu.Value.ToString() == dil_bul_table.Rows[i]["CariBakimSec_Bakim"].ToString())
                            {
                                dongu.Selected = true;
                            }
                        }
                    }
                }




                //Bakım İşaretlemesini Yağ
                SqlDataAdapter goreve_giden_personel = new SqlDataAdapter("select * from tblCariBakimlarGP where CariBakimlarGP_GorevId='" + RouteData.Values["idd"].ToString() + "'", connect);
                DataTable goreve_giden_personeltb = new DataTable();
                goreve_giden_personel.Fill(goreve_giden_personeltb);

                if (goreve_giden_personeltb.Rows.Count > 0)
                {
                    gorevli_personel.DataBind();
                    for (int i = 0; i < goreve_giden_personeltb.Rows.Count; i++)
                    {
                        foreach (ListItem dongu in gorevli_personel.Items)
                        {
                            if (dongu.Value.ToString() == goreve_giden_personeltb.Rows[i]["CariBakimlarGP_Persid"].ToString())
                            {
                                dongu.Selected = true;
                            }
                        }
                    }
                }
                connect.Close();
                dil_bul.Dispose();
                goreve_giden_personel.Dispose();
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

            SqlCommand sil = new SqlCommand("delete from tblCariBakimlar where CariBakimlar_Id='" + id + "'", connect);
            sil.ExecuteNonQuery();
            connect.Close();

            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);

            //Eğer ekranda güncellenecen veri açıksa ve silinmişse duyuruları tekrar yükleki silinen veriyi güncelleme yapamasın.
            if (RouteData.Values["idd"] != null)
            {
                if (RouteData.Values["idd"].ToString() == id.ToString())
                {
                   Response.Redirect("/Servis/Bakim-Ekle/" + RouteData.Values["id"] + "");
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
                using (SqlCommand cmd = new SqlCommand(select2 + select3))


                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt, "Bakım Listesi");

                            //ws.Cell(1, 6).Value = "Service";
                            //ws.Cell(1, 15).Value = "Invoice";

                            //ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            //ws.Range("M1:Q1").Style.Fill.BackgroundColor = XLColor.DarkCandyAppleRed;
                            ws.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.FromHtml("#3A4B55");

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
        //Güvenlik///////////////////////
        if (icguvenlik_gorev != "1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
        //Güvenlik///////////////////////
    }
}