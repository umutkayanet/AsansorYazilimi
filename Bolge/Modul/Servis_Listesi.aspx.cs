using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Web.UI.HtmlControls;

public partial class Modul_Servis_Listesi : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string Asansor_Ekle_G, Bakim_Ekle_G, Revizyon_Ekle_G, Ariza_Ekle_G, Sozlesme_Ekle_G;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("Servis_Listesi_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }
        else
        {
            if (Sorgu.yetkiteklikontrol("Servis_Asansor_Okuma") != "1")
            {
                Asansor_Ekle_G = "0";
            }

            if (Sorgu.yetkiteklikontrol("Bakim_Listeleme") != "1")
            {
                Bakim_Ekle_G = "0";
            }

            if (Sorgu.yetkiteklikontrol("Revizyon_Listelemee") != "1")
            {
                Revizyon_Ekle_G = "0";
            }

            if (Sorgu.yetkiteklikontrol("Cari_Ariza_Listeleme") != "1")
            {
                Ariza_Ekle_G = "0";
            }

            if (Sorgu.yetkiteklikontrol("Servis_Sozlesmeler_Okuma") != "1")
            {
                Sozlesme_Ekle_G = "0";
            }
        }
        verileri_yukle();
    }

    string sorgu_where = "";
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
                belge_ozeti += "<b>Cari Kodu :</b> " + Cari_Kodu.Text + ", ";
            }

            string Cari_Unvan_Arama = "";
            if (Cari_Unvan.Text != string.Empty)
            {
                Cari_Unvan_Arama = " and [Ünvan] LIKE '%" + Cari_Unvan.Text + "%' ";
                belge_ozeti += "<b>Ünvan: </b> " + Cari_Unvan.Text + ", ";
            }

            string Toplam_Asansor_Deg = "";
            if (toplam_asansor_ara.Text != string.Empty)
            {
                Toplam_Asansor_Deg = " and [Toplam Asansor] >= '" + Convert.ToInt32(toplam_asansor_ara.Text) + "'";
                belge_ozeti += "<b>Toplam Asansör: </b> " + toplam_asansor_ara.Text + ", ";
            }


            string Toplam_Bakim_Deg = "";
            if (toplam_bakim_ara.Text != string.Empty)
            {
                Toplam_Bakim_Deg = " and [Toplam Bakım] >= '" + Convert.ToInt32(toplam_bakim_ara.Text) + "'";
                belge_ozeti += "<b>Toplam Bakım: </b> " + toplam_bakim_ara.Text + ", ";
            }


            string Toplam_Revizyon_Deg = "";
            if (toplam_revizyon_ara.Text != string.Empty)
            {
                Toplam_Revizyon_Deg = " and [Toplam Revizyon] >= '" + Convert.ToInt32(toplam_revizyon_ara.Text) + "'";
                belge_ozeti += "<b>Toplam Revizyon: </b> " + toplam_revizyon_ara.Text + ", ";
            }


            string Toplam_Ariza_Deg = "";
            if (toplam_ariza_ara.Text != string.Empty)
            {
                Toplam_Ariza_Deg = " and [Toplam Ariza] >= '" + Convert.ToInt32(toplam_ariza_ara.Text) + "'";
                belge_ozeti += "<b>Toplam Arıza: </b> " + toplam_ariza_ara.Text + ", ";
            }

            string AsansorKimlikNo_Deg = "";
            if (Asansor_Kimlik_NoAra.Text != string.Empty)
            {
                AsansorKimlikNo_Deg = " and CK.Cari_Id = (Select CariSU_CariNo from tblCariAsansorler where CariSU_KimlikNo = '" + Asansor_Kimlik_NoAra.Text + "')";
                belge_ozeti += "<b>Asansör Kimlik No : </b> " + Asansor_Kimlik_NoAra.Text + ", ";
            }


            //Personel Listesini Ekrana Getir.
            connect.Open();
            sorgu_where = "where Cari_Turu = 'Satis' " + Cari_Kodu_Arama + Cari_Unvan_Arama + Toplam_Asansor_Deg + Toplam_Bakim_Deg + Toplam_Revizyon_Deg + Toplam_Ariza_Deg + AsansorKimlikNo_Deg + " order by  CK.Cari_Unvan";
            sorgu = "WITH asansor AS " +
                    "(SELECT COUNT(CK.Cari_Id)[Toplam Asansor], CK.Cari_Id from tblCariAsansorler CA " +
                    "left join tblCariKayit CK on (CA.CariSU_CariNo=CK.Cari_Id) group by CK.Cari_Id), " +
                    "bakim AS(SELECT COUNT(CA.CariSU_CariNo)[Toplam Bakım], CA.CariSU_CariNo from tblCariBakimlar CB " +
                    "left join tblCariAsansorler CA on(CA.CariSU_Id=CB.CariBakimlar_Asansor) group by CA.CariSU_CariNo), " +
                    "ariza AS(SELECT COUNT(CA.CariSU_CariNo)[Toplam Ariza], CA.CariSU_CariNo from tblCariAsansorAriza CB " +
                    "left join tblCariAsansorler CA on(CA.CariSU_Id=CB.CariAsansorAriza_AsaId) group by CA.CariSU_CariNo), " +
                    "revizyon AS(SELECT COUNT(CA.CariSU_CariNo)[Toplam Revizyon], CA.CariSU_CariNo from tblCariRevizyonlar CR " +
                    "left join tblCariAsansorler CA on(CA.CariSU_Id=CR.CariRevizyonlar_Asansor) group by CA.CariSU_CariNo), " +
                    "sozlesme AS(SELECT COUNT(CA.Cari_Kodu)[Toplam Sozlesme], CA.Cari_Kodu from tblCariSozlesmeler SZ " +
                    "left join tblCariKayit CA on(CA.Cari_Id=SZ.CariSozlesmeler_CariId) group by CA.Cari_Kodu) " +
                    "Select CK.Cari_Id, CK.Cari_Kodu[Cari Kodu], ck.Cari_Turu[Cari Türü], CK.Cari_Unvan[Ünvan], ASN.[Toplam Asansor], BK.[Toplam Bakım], AR.[Toplam Ariza], RE.[Toplam Revizyon], SZ.[Toplam Sozlesme] from tblCariKayit CK " +
                    "left join asansor ASN on (ASN.Cari_Id=CK.Cari_Id) " +
                    "left join bakim BK on (BK.CariSU_CariNo=CK.Cari_Id) " +
                    "left join ariza AR on (AR.CariSU_CariNo=CK.Cari_Id) " +
                    "left join revizyon RE on (RE.CariSU_CariNo=CK.Cari_Id) " +
                    "left join sozlesme SZ on(SZ.Cari_Kodu=CK.Cari_Kodu) ";

            SqlCommand komut = new SqlCommand(sorgu + sorgu_where, connect);

            SqlDataReader reader;


            reader = komut.ExecuteReader();
            personel_rep.DataSource = reader;
            personel_rep.DataBind();

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
                using (SqlCommand cmd = new SqlCommand(sorgu + sorgu_where))

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt, "Toplam Servis Listesi");

                            var listOfStrings = new List<String>();
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
                            Response.AddHeader("content-disposition", "attachment;filename=Toplam_Servis_Listesi_" + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx");
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
    protected void personel_rep_ItemDataBound(object sender, RepeaterItemEventArgs a)
    {
        if (a.Item.ItemType == ListItemType.Item || a.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (Asansor_Ekle_G=="0")
            {
                HtmlGenericControl Asansor_Ekle_P = a.Item.FindControl("Asansor_Ekle_P") as HtmlGenericControl;
                Asansor_Ekle_P.Visible = false;
            }


            if (Bakim_Ekle_G == "0")
            {
                HtmlGenericControl Bakim_Ekle_P = a.Item.FindControl("Bakim_Ekle_P") as HtmlGenericControl;
                Bakim_Ekle_P.Visible = false;
            }


            if (Revizyon_Ekle_G == "0")
            {
                HtmlGenericControl Revizyon_Ekle_P = a.Item.FindControl("Revizyon_Ekle_P") as HtmlGenericControl;
                Revizyon_Ekle_P.Visible = false;
            }

            if (Ariza_Ekle_G == "0")
            {
                HtmlGenericControl Ariza_Ekle_P = a.Item.FindControl("Ariza_Ekle_P") as HtmlGenericControl;
                Ariza_Ekle_P.Visible = false;
            }

            if (Sozlesme_Ekle_G == "0")
            {
                HtmlGenericControl Sozlesme_Ekle_P = a.Item.FindControl("Sozlesme_Ekle_P") as HtmlGenericControl;
                Sozlesme_Ekle_P.Visible = false;
            }
        }
    }
}