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

public partial class Modul_Revizyon_RenkEkle : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Revizyon_Listelemee") != "1")
        {
            Response.Redirect("/Giris");
        }

        if (Sorgu.yetkiteklikontrol("Revizyon_Yazma") != "1")
        {
            g_panel.Visible = false;
        }
        //Güvenlik Son


        //Verileri Yükle
        verileri_yukle();

        if (RouteData.Values["iddd"] != null)
        {
            yeni_kayit_buton.Text = "Renk Etiketini Düzenle";
            guncelle_div.Attributes.Add("Style", "background-color: #ffefd7;");
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
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariRevizyonlarEtiket where CRE_Id='" + RouteData.Values["iddd"] + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                if (tablo.Rows.Count > 0)
                {
                    Drop1.SelectedValue = tablo.Rows[0]["CRE_Etiket"].ToString();
                    belge_tarihi.Text = Convert.ToDateTime(tablo.Rows[0]["CRE_BelgeTarihi"]).ToString("dd.MM.yyyy");
                    sonetikettarihi.Text = Convert.ToDateTime(tablo.Rows[0]["CRE_EtiketSonTarih"]).ToString("dd.MM.yyyy");
                    yuklu_belge.Value = tablo.Rows[0]["CRE_Dosya"].ToString();

                    FileUpload1_kontrol.Enabled = false;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari Stok Bilgileri Alınamadı").ToString(), true);
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

        Cari_Label.Text = Sorgu.cari_bul(RouteData.Values["id"].ToString());

        //Sistemdeki Revizyonda Takilan Ürünlerini Yukle
        SqlDataAdapter komut = new SqlDataAdapter("Select et.CRE_Id, et.CRE_BelgeTarihi, et.CRE_EtiketSonTarih, asa.CariSU_Tanimi, et.CRE_Dosya, " +
                                                  "CASE "+
                                                  "WHEN CRE_Etiket = '1' THEN '<span class=''label label-danger''>Kırmızı Etiket</span>' "+
                                                  "WHEN CRE_Etiket = '2' THEN '<span class=''label label-success''>Yeşil Etiket</span>' "+
                                                  "WHEN CRE_Etiket = '3' THEN '<span class=''label label-info''>Mavi Etiket</span>' "+
                                                  "WHEN CRE_Etiket = '4' THEN '<span class=''label label-warning''>Sarı Etiket</span>' "+
                                                  "END as renk "+
                                                  "from tblCariRevizyonlarEtiket et " +
                                                  "left join tblCariAsansorler asa on(et.CRE_RevizyonId = asa.CariSU_Id) " +
                                                  "where et.CRE_RevizyonId='" + RouteData.Values["idd"] + "' ", connect);
        DataTable tablo = new DataTable();
        komut.Fill(tablo);
        rep1.DataSource = tablo;
        rep1.DataBind();
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

            connect.Open();
            if (RouteData.Values["iddd"] != null)
            {
                //Güncelleme sayfası açıkken dosya yükleme doluysa
                if (FileUpload1.HasFile)
                {
                    dosya_sorgu = " ,CRE_Dosya=@CRE_Dosya";
                }

                komut = new SqlCommand("update tblCariRevizyonlarEtiket set " +
                                        "CRE_Etiket=@CRE_Etiket, CRE_RevizyonId=@CRE_RevizyonId, CRE_Belge=@CRE_RevizyonId, " +
                                        "CRE_BelgeTarihi=@CRE_BelgeTarihi, CRE_EtiketSonTarih=@CRE_EtiketSonTarih "+ dosya_sorgu + " " +
                                        "where CRE_Id='"+ RouteData.Values["iddd"] + "'", connect);
            }
            else
            {
                komut = new SqlCommand("insert into tblCariRevizyonlarEtiket" +
                                        "(CRE_Etiket, CRE_RevizyonId, CRE_Belge, CRE_BelgeTarihi, CRE_EtiketSonTarih, CRE_Dosya)values " +
                                        "(@CRE_Etiket, @CRE_RevizyonId, @CRE_Belge, @CRE_BelgeTarihi, @CRE_EtiketSonTarih, @CRE_Dosya)", connect);
            }

            komut.Parameters.AddWithValue("@CRE_Etiket", Drop1.SelectedValue);
            komut.Parameters.AddWithValue("@CRE_RevizyonId", RouteData.Values["idd"]);
            komut.Parameters.AddWithValue("@CRE_Belge", "");
            komut.Parameters.AddWithValue("@CRE_BelgeTarihi", Convert.ToDateTime(belge_tarihi.Text).ToString("yyyy.MM.dd"));
            komut.Parameters.AddWithValue("@CRE_Dosya", dosyauzantisi);

            if (sonetikettarihi.Text!="")
            {
                komut.Parameters.AddWithValue("@CRE_EtiketSonTarih", Convert.ToDateTime(sonetikettarihi.Text).ToString("yyyy.MM.dd"));
            }
            else
            {
                komut.Parameters.AddWithValue("@CRE_EtiketSonTarih", Convert.ToDateTime(belge_tarihi.Text).ToString("yyyy.MM.dd"));
            }


            //eğer güncelleme sayfası açıksa
            if (RouteData.Values["iddd"] != null)
            {
                //Dosya kontrollü yap yükleme alanı doluysa
                if (FileUpload1.HasFile)
                {
                    //Yüklenen dosyanın uzantılarına bak
                    if (dosyauzantisi != "0")
                    {
                        //dosyayı yükle
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("/Data/Asansor_RB/" + dosyauzantisi));

                        //Orjinal Resmi Sil                       
                        string dosya_yolu = yuklu_belge.Value;
                        dosya_yolu = Server.MapPath("/Data/Asansor_RB/") + "\\" + dosya_yolu;
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
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("/Data/Asansor_RB/" + dosyauzantisi));
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

            if (Sorgu.dosya_sil(repeat_yuklu_belge.Value, "Asansor_RB")!="0")
            {
                SqlCommand sil = new SqlCommand("delete from tblCariRevizyonlarEtiket where CRE_Id='" + id + "'", connect);
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

    protected void rep1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        //Güvenlik
        if (Sorgu.yetkiteklikontrol("Revizyon_Yazma") != "1")
        {
            e.Item.FindControl("sil").Visible = false;
        }
        //Güvenlik Son
    }


    public static string dosya_uzantisi(string dosyauzantisi)
    {
        string gonder = "0";
        try
        {
            if (dosyauzantisi.IndexOf(".pdf") != -1 || dosyauzantisi.IndexOf(".PDF") != -1 || dosyauzantisi.IndexOf(".jpg") != -1 || dosyauzantisi.IndexOf(".JPG") != -1 || dosyauzantisi.IndexOf(".png") != -1 || dosyauzantisi.IndexOf(".PNG") != -1)
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