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

public partial class Modul_Cari_Sozlesmeler : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string guvenlik = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Servis_Sozlesmeler_Okuma") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Servis_Sozlesmeler_Yazma") != "1")
        {
            guvenlik = "0";
            g_panel.Visible = false;
        }
        //Güvenlik Son


        //Verileri Yükle
        verileri_yukle();

        if (RouteData.Values["idd"] != null)
        {
            bilgi_getir();
            buttonlar_panel.Visible = true;
        }
    }


    public void bilgi_getir()
    {
        try
        {
            if (!IsPostBack)
            {
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariSozlesmeler where CariSozlesmeler_Id='" + RouteData.Values["idd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Drop1.DataBind();
                    foreach (ListItem li in Drop1.Items)
                    {
                        if (li.Value == tablo.Rows[0]["CariSozlesmeler_Durumu"].ToString())
                        {
                            li.Selected = true;
                            break;
                        }
                    }

                    yuklu_belge.Value = tablo.Rows[0]["CariSozlesmeler_Dosya"].ToString();
                    t1.Text = tablo.Rows[0]["CariSozlesmeler_Baslik"].ToString();
                    belge_tarihi.Text = Convert.ToDateTime(tablo.Rows[0]["CariSozlesmeler_Tarih"]).ToString("dd.MM.yyyy");
                    t3.Text = tablo.Rows[0]["CariSozlesmeler_Not"].ToString();
                    FileUpload1_kontrol.Enabled = false;
                    soztarih.Text = Convert.ToDateTime(tablo.Rows[0]["CariSozlesmeler_SonTarih"]).ToString("dd.MM.yyyy");
                    t5.Text = tablo.Rows[0]["CariSozlesmeler_Bedel"].ToString() != ""? Convert.ToDecimal(tablo.Rows[0]["CariSozlesmeler_Bedel"]).ToString("N"):"";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari Dosya Bilgileri Alınamadı").ToString(), true);
                }
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Bilgiler ekrana getirilemedi.").ToString(), true);
            return;
        }
    }

    public void verileri_yukle()
    {
        connect.Open();

        //Sistemdeki Revizyonda Takilan Ürünlerini Yukle
        SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariSozlesmeler where CariSozlesmeler_CariId='" + RouteData.Values["id"] + "' ", connect);
        DataTable tablo = new DataTable();
        komut.Fill(tablo);
        rep1.DataSource = tablo;
        rep1.DataBind();

        Cari_Label.Text = Sorgu.cari_bul(RouteData.Values["id"].ToString());
        connect.Close();
    }

    SqlCommand komut;
    string dosya_sorgu;
    protected void kayit_Click(object sender, EventArgs e)
    {
       try
        {
            //Yüklenecek dosyanın uzantısını al
            string dosyauzantisi = Path.GetExtension(FileUpload1.PostedFile.FileName);

            //dosya uzantısına personelin adını ekle
            dosyauzantisi = Sorgu.dosya_adi_yarat(Cari_Label.Text) + "_" + Sorgu.kodyarat() + Path.GetExtension(FileUpload1.PostedFile.FileName);

            string SozlesmeBedeli = "";
            SozlesmeBedeli = t5.Text.Replace(".", "");
            SozlesmeBedeli = SozlesmeBedeli.Replace(",", ".");

            connect.Open();
            if (RouteData.Values["idd"] != null)
            {
                //Güncelleme sayfası açıkken dosya yükleme doluysa
                if (FileUpload1.HasFile)
                {
                    dosya_sorgu = " ,CariSozlesmeler_Dosya=@CariSozlesmeler_Dosya";
                }
                komut = new SqlCommand("update tblCariSozlesmeler set CariSozlesmeler_Baslik=@CariSozlesmeler_Baslik, CariSozlesmeler_Not=@CariSozlesmeler_Not, " +
                                       "CariSozlesmeler_Tarih=@CariSozlesmeler_Tarih, CariSozlesmeler_SonTarih=@CariSozlesmeler_SonTarih, CariSozlesmeler_Durumu=@CariSozlesmeler_Durumu, CariSozlesmeler_Bedel=@CariSozlesmeler_Bedel " + dosya_sorgu + " " +
                                       "where CariSozlesmeler_Id='" + RouteData.Values["idd"] + "'", connect);
            }
            else
            {
                komut = new SqlCommand("insert into tblCariSozlesmeler" +
                                       "(CariSozlesmeler_CariId, CariSozlesmeler_Baslik, CariSozlesmeler_Dosya, CariSozlesmeler_Tarih, CariSozlesmeler_SonTarih, CariSozlesmeler_Not, CariSozlesmeler_Durumu, CariSozlesmeler_Bedel)values " +
                                       "(@CariSozlesmeler_CariId, @CariSozlesmeler_Baslik, @CariSozlesmeler_Dosya, @CariSozlesmeler_Tarih, @CariSozlesmeler_SonTarih, @CariSozlesmeler_Not, @CariSozlesmeler_Durumu, @CariSozlesmeler_Bedel)", connect);
            }
            komut.Parameters.AddWithValue("@CariSozlesmeler_CariId", RouteData.Values["id"]);
            komut.Parameters.AddWithValue("@CariSozlesmeler_Baslik", t1.Text);
            komut.Parameters.AddWithValue("@CariSozlesmeler_Dosya", dosyauzantisi);
            komut.Parameters.AddWithValue("@CariSozlesmeler_Tarih", Convert.ToDateTime(belge_tarihi.Text).ToString("yyyy.MM.dd"));
            komut.Parameters.AddWithValue("@CariSozlesmeler_Not", t3.Text);
            komut.Parameters.AddWithValue("@CariSozlesmeler_Durumu", Drop1.SelectedValue);
            komut.Parameters.AddWithValue("@CariSozlesmeler_Bedel", String.Format("{0:c}", SozlesmeBedeli));

            if (soztarih.Text!="")
            {
                komut.Parameters.AddWithValue("@CariSozlesmeler_SonTarih", Convert.ToDateTime(soztarih.Text).ToString("yyyy.MM.dd"));
            }
            else
            {
                //+ 1 Yıl Ekle
                DateTime theDate = Convert.ToDateTime(belge_tarihi.Text);
                DateTime yearInTheFuture = theDate.AddYears(1);
                komut.Parameters.AddWithValue("@CariSozlesmeler_SonTarih", yearInTheFuture.ToString("yyyy.MM.dd"));
            }

            //eğer güncelleme sayfası açıksa
            if (RouteData.Values["idd"] != null)
            {
                //Dosya kontrollü yap yükleme alanı doluysa
                if (FileUpload1.HasFile)
                {
                    //Yüklenen dosyanın uzantılarına bak
                    if (dosyauzantisi != "0")
                    {
                        //dosyayı yükle
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("/Data/sozlesmeler/" + dosyauzantisi));

                        //Orjinal Resmi Sil                       
                        string dosya_yolu = yuklu_belge.Value;
                        dosya_yolu = Server.MapPath("/Data/sozlesmeler/") + "\\" + dosya_yolu;
                        if (System.IO.File.Exists(dosya_yolu))
                        {
                            System.IO.File.Delete(dosya_yolu);// Ve sil
                        }
                        //SON
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sisteme .pdf uzantılı dosyalar ekleyebilirsiniz.").ToString(), true);
                        return;
                    }
                }
            }
            else//eğer ekleme sayfası açıksa
            {
                //dosya yükle alanına bak dolumu
                if (FileUpload1.HasFile)
                {
                    //dosya uzantısını kontrol et
                    if (dosyauzantisi != "0")
                    {
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("/Data/sozlesmeler/" + dosyauzantisi));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Sisteme .pdf uzantılı dosyalar ekleyebilirsiniz.").ToString(), true);
                        return;
                    }
                }
            }


            komut.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
            connect.Close();
            verileri_yukle();
            yuklu_belge.Value = dosyauzantisi;
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
            HiddenField repeat_yuklu_belge = e.Item.FindControl("repeat_yuklu_belge") as HiddenField;

            if (Sorgu.dosya_sil(repeat_yuklu_belge.Value, "sozlesmeler")!="0")
            {
                SqlCommand sil = new SqlCommand("delete from tblCariSozlesmeler where CariSozlesmeler_Id='" + id + "'", connect);
                sil.ExecuteNonQuery();
                connect.Close();
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
                verileri_yukle();
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Dosya Silinemedi.").ToString(), true);
                return;
            }
        }
    }


    protected void rep1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (guvenlik!="1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
    }


    public static string dosya_uzantisi(string dosyauzantisi)
    {
        string gonder = "0";
        try
        {
            if (dosyauzantisi.IndexOf(".pdf") != -1 || dosyauzantisi.IndexOf(".PDF") != -1 || dosyauzantisi.IndexOf(".doc") != -1 || dosyauzantisi.IndexOf(".doc") != -1 || dosyauzantisi.IndexOf(".docx") != -1 || dosyauzantisi.IndexOf(".DOCX") != -1)
            {
                gonder = "1";
            }
        }
        catch (Exception)
        {
            gonder = "0";
        }
        return gonder;
    }
}