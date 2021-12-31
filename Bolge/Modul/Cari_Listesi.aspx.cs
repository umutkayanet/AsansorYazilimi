using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using ClosedXML.Excel;

public partial class Modul_Cari_Listesi : System.Web.UI.Page
{
    string icguvenlik_gorev = "1";
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("Cari_Bilgileri_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        if (Sorgu.yetkiteklikontrol("Cari_Bilgileri_Yazma") != "1")
        {
            Cari_Kayit.Visible = false;
            icguvenlik_gorev = "0";
        }
        //Güvenlik///////////////////////


        if (RouteData.Values["gorev"].ToString() == "Satis")
        {
            cari_baslik.Text = "Cari Satış Listesi";
            cari_ana_baslik.Text = "Cari Satış Listesi";
            Cari_Kayit.PostBackUrl = "/Cari-Kayit/Satis";
        }
        else
        {
            cari_baslik.Text = "Cari Alış Listesi";
            cari_ana_baslik.Text = "Cari Alış Listesi";
            Cari_Kayit.PostBackUrl = "/Cari-Kayit/Alis";
        }

        verileri_yukle();
    }

    string sorgu = "";
    string belge_ozeti = "";
    //Personel Listesini Yükle
    public void verileri_yukle()
    {
        try
        {
            string Cari_Kodu_Arama = "";
            if (Cari_Kodu.Text != string.Empty)
            {
                Cari_Kodu_Arama = " and Cari_Kodu LIKE '%" + Cari_Kodu.Text + "%' ";
                belge_ozeti += "<b>Cari Kodu :</b> "+ Cari_Kodu.Text + ", ";
            }

            string Cari_Unvan_Arama = "";
            if (Cari_Unvan.Text != string.Empty)
            {
                Cari_Unvan_Arama = " and Cari_Unvan LIKE '%" + Cari_Unvan.Text + "%' ";
                belge_ozeti += "<b>Ünvan: </b> " + Cari_Unvan.Text + ", ";
            }

            string Cari_Telefon_Arama = "";
            if (Cari_Telefon.Text != string.Empty)
            {
                Cari_Telefon_Arama = " and Cari_Telefon LIKE '" + Cari_Telefon.Text + "' ";
                belge_ozeti += "<b>Telefon: </b> " + Cari_Telefon.Text + ", ";
            }


            string Cari_Yetkili_Arama = "";
            if (Cari_Yetkili.Text != string.Empty)
            {
                Cari_Yetkili_Arama = " and Cari_Yetkili LIKE '%" + Cari_Yetkili.Text + "%'";
                belge_ozeti += "<b>Yetkili Kişi: </b> " + Cari_Yetkili.Text + ", ";
            }


            //Personel Listesini Ekrana Getir.
            connect.Open();
            sorgu = " where Cari_Turu = '" + RouteData.Values["gorev"].ToString() + "' " + Cari_Kodu_Arama + Cari_Unvan_Arama + Cari_Telefon_Arama + Cari_Yetkili_Arama + " order by Cari_Unvan";

            SqlCommand komut = new SqlCommand("Select sh.Sehir_Adi, tblSIlceler_IlceAdi, * From tblCariKayit ck " +
                                              "Left Join tblSehirler sh on(sh.Sehirler_Id = ck.Cari_Sehir)" +
                                              "Left join tblSIlceler il on(il.tblSIlceler_Id=ck.Cari_İlce)" + sorgu + "", connect);

            SqlDataReader reader;
            

            reader = komut.ExecuteReader();
            personel_rep.DataSource = reader;
            personel_rep.DataBind();

            komut.Dispose();
            connect.Close();
           
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "");
        }
    }

    protected void ara_Click(object sender, EventArgs e)
    {
        verileri_yukle();
    }
    protected void excel_export_Click(object sender, EventArgs e)
    {
        export("Excel");
    }

    public void export(string export)
    {
        try
        {
            string connect = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand("Select ck.Cari_Kodu[Cari Kodu], ck.Cari_Turu[Cari Türü], ck.Cari_Unvan[Ünvan], ck.Cari_Telefon[Telefon], sh.Sehir_Adi[Şehir], ck.Cari_İlce[İlçe], " +
                                                       "ck.Cari_Adres, ck.Cari_VergiNo, ck.Cari_VergiDairesi, ck.Cari_Yetkili[Yetkili Kişi], ck.Cari_WebSites[Web Sitesi], ck.Cari_Email[Email Adresi] From tblCariKayit ck " +
                                                       "Left Join tblSehirler sh on(sh.Sehirler_Id = ck.Cari_Sehir)" + sorgu))


                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    if (export == "Excel")
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add(dt, RouteData.Values["gorev"].ToString()+ " Cari Listesi");

                                var listOfStrings = new List<String>();
                                //ws.Cell(1, 6).Value = "Service";
                                //ws.Cell(1, 15).Value = "Invoice";

                                //ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                                //ws.Range("M1:Q1").Style.Fill.BackgroundColor = XLColor.DarkCandyAppleRed;
                                ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.FromHtml("#3A4B55");

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
                                Response.AddHeader("content-disposition", "attachment;filename="+ RouteData.Values["gorev"].ToString() + "_Cari_Listesi_" + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx");
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


                    if (export == "print")
                    {
                        con.Open();
                        DataTable results = new DataTable();
                        results.Load(cmd.ExecuteReader());

                        Repeater1.DataSource = results;
                        Repeater1.DataBind();

                        belge_ozet_label.Text = "<b>Kullanıcı : </b>" + Server.UrlDecode(Request.Cookies["RcEU"]["Kullanici_Adi"].ToString()) + "</br>" + belge_ozeti + "</br><b>Basım Zamanı : </b> " + DateTime.Now.ToString("dd.MM.yyyy H:mm:ss") + " <b>Toplam Kayıt : </b>" + Repeater1.Items.Count.ToString() + "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "print", "printDiv()", true);
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

    protected void print_Click(object sender, EventArgs e)
    {
        export("print");
    }

    protected void personel_rep_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "sil") //Eğer işe giriş çıkış sil komutu tetiklenmişse
            {

                connect.Open();
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                SqlCommand sil = new SqlCommand("delete from tblCariKayit where Cari_Id='" + id + "'", connect);
                sil.ExecuteNonQuery();
                sil.Dispose();
                connect.Close();

                ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("basari", "");
                verileri_yukle();
            }
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "");
        }
    }


    protected void personel_rep_ItemDataBound(object sender, RepeaterItemEventArgs e)
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