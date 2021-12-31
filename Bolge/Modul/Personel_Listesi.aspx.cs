using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using ClosedXML.Excel;

public partial class Modul_Personel_Listesi : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("YY_IK_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");          
            excel_export.Visible = false;
            print_button.Visible = false;
        }
        else
        {
            excel_export.Visible = true;
            print_button.Visible = true;
        }

        //İnsan kaynakları Yazma
        if (Sorgu.yetkiteklikontrol("YY_IK_Yazma") != "1")
        {
            Personel_Ekle.Visible = false;
        }
        //Güvenlik///////////////////////
        verileri_yukle();
    }

    string sorgu = "";
    string belge_ozeti = "";
    //Personel Listesini Yükle
    public void verileri_yukle()
    {
        try
        {
            string Personel_adi_arama_arama = "";
            if (Personel_adi_arama.Text != string.Empty)
            {
                Personel_adi_arama_arama = " and Pers_Adi LIKE '%" + Personel_adi_arama.Text + "%' ";
                belge_ozeti += "<b>Adı:</b> "+ Personel_adi_arama.Text + ", ";
            }

            string Personel_soyadi_arama_sql = "";
            if (Personel_soyadi_arama.Text != string.Empty)
            {
                Personel_soyadi_arama_sql = " and Pers_Soyadi LIKE '%" + Personel_soyadi_arama.Text + "%' ";
                belge_ozeti += "<b>Soyadı: </b> " + Personel_soyadi_arama.Text + ", ";
            }

            string Personel_tc_arama_sql = "";
            if (Personel_tc_arama.Text != string.Empty)
            {
                Personel_tc_arama_sql = " and Pers_Tc LIKE '" + Personel_tc_arama.Text + "' ";
                belge_ozeti += "<b>Kimlik Numarası: </b> " + Personel_tc_arama.Text + ", ";
            }


            string Personel_email_arama_sql = "";
            if (personel_email_arama.Text != string.Empty)
            {
                Personel_email_arama_sql = " and Email LIKE '%" + personel_email_arama.Text + "%'";
                belge_ozeti += "<b>Email Adresi: </b> " + personel_email_arama.Text + ", ";
            }


            //Personel Listesini Ekrana Getir.
            connect.Open();
            sorgu = " where Persid != '' " + Personel_adi_arama_arama + Personel_soyadi_arama_sql +  Personel_tc_arama_sql +  Personel_email_arama_sql +  " order by Pers_Soyadi";

            SqlCommand komut = new SqlCommand("Select * from Personel" + sorgu + "", connect);

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
                using (SqlCommand cmd = new SqlCommand("WITH songiris AS "+
                                                       "(SELECT GC.IGC_Persid, MAX(GC.IGC_Giris_Tarihi) MaxDate FROM tblIseGirisCikis GC GROUP BY GC.IGC_Persid), "+
                                                       "soncikis AS(SELECT GC.IGC_Persid, MAX(GC.IGC_Cikis_Tarihi) MaxDate FROM tblIseGirisCikis GC GROUP BY GC.IGC_Persid) "+
                                                       "Select Personel.Pers_adi[Adı], Pers_Soyadi[Soyadı], Pers_TC[Kimlik Numarası], Pers_Dogum_Tarihi[Doğum Tarihi], "+
                                                       "CASE WHEN Pers_Cinsiyet = '0' THEN 'Erkek' WHEN Pers_Cinsiyet = '1' THEN 'Kadın' WHEN Pers_Cinsiyet = '2' THEN 'Diğer' end as Cinsiyet, "+
                                                       "DATEDIFF(YEAR, Pers_Dogum_Tarihi, GETDATE()) AS Yaş, Email[Email Adresi], gr.Gorev[Pozisyonu], Mezuniyet_yil[Mezuniyet Yılı], "+
                                                       "Personel_Egitim[Eğitim Durumu], Calisan[Çalışan], md.MaxDate[İşe Giriş Tarihi], sc.MaxDate[İşden Çıkış Tarihi], Pers_TelSirketCep[Şirket Gsm Numarası] "+
                                                       "From Personel "+
                                                       "left join songiris md " +
                                                       "on Personel.Persid = md.IGC_Persid " +
                                                       "left join soncikis sc " +
                                                       "on Personel.Persid = sc.IGC_Persid " +
                                                       "left join tblPersonelGorev gr " +
                                                       "on Personel.Gorev_id = gr.Gorev_id " +
                                                       "left join tblPersonelEgitim pe " +
                                                       "on Personel.Pers_EgitimDurumu = pe.Personel_EgitimId " + sorgu))


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
                                var ws = wb.Worksheets.Add(dt, "Personel Listesi");

                                var listOfStrings = new List<String>();
                                //ws.Cell(1, 6).Value = "Service";
                                //ws.Cell(1, 15).Value = "Invoice";

                                //ws.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                                //ws.Range("M1:Q1").Style.Fill.BackgroundColor = XLColor.DarkCandyAppleRed;
                                ws.Range("A1:N1").Style.Fill.BackgroundColor = XLColor.FromHtml("#3A4B55");

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
                                Response.AddHeader("content-disposition", "attachment;filename=Personel_Listesi_" + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx");
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
                }
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
}