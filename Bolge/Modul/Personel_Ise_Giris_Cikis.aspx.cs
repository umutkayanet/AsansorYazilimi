using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Modul_Personel_Ise_Giris_Cikis : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string buton_yetki = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("YY_IK_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        if (Sorgu.yetkiteklikontrol("YY_IK_Yazma") != "1")
        {
            g_panel.Visible = false;
            buton_yetki = "0";
        }
        //Güvenlik///////////////////////




        //İşe giriş çıkış butonu tıklantığında personel seçili değilse
        if (RouteData.Values["id"] != null)
        {
            //Eğer veri güncelleme sayfasında ise
            if (RouteData.Values["veriid"] != null)
            {
                //Butonların adını değiştir.
                kayit.Text = "Kaydet";

                //Çıkış alanı bölümünü gizle
                Panel2.Visible = true;

                //Eğer tarih güncelleme ekranında ise verileri doldur.
                giris_cikis_bilgilerini_getir();
            }
            //Giriş çıkış verilerini yükle ve kontrol et.
            verileri_yukle();
        }
    }

    //Personelin bilgilerini yükle
    public void verileri_yukle()
    {
       connect.Open();
        try
        {
            //Personel tablosundan personelin adını al ve ekrana yansıt.
            SqlCommand personel_bul = new SqlCommand("Select * from Personel where Persid='" + RouteData.Values["id"] + "'", connect);
            SqlDataReader reader = personel_bul.ExecuteReader();
            reader.Read();

            persadi_label.Text =reader["Pers_Adi"].ToString() + " " + reader["Pers_Soyadi"].ToString();
            personelin_adi.Text = reader["Pers_Adi"].ToString() + " " + reader["Pers_Soyadi"].ToString();
            reader.Close();

            //Personelin tarihlerini ekranda repeatera yazdır.
            SqlDataAdapter komut = new SqlDataAdapter("SELECT IGC_Id, IGC_Giris_Tarihi, IGC_Cikis_Tarihi, "+
            "CASE "+
            "WHEN IGC_Aciklama_Id = '1'  THEN 'Sağlık Sorunları' "+
            "WHEN IGC_Aciklama_Id = '2'   THEN 'İstifa' "+
            "WHEN IGC_Aciklama_Id = '3'  THEN 'Proje Bitimi' "+
            "WHEN IGC_Aciklama_Id = '4' THEN 'Sözleşme Sonu' "+
            "WHEN IGC_Aciklama_Id = '5' THEN 'Denem Süresi' "+
            "WHEN IGC_Aciklama_Id = '6' THEN 'Emeklilik' "+
            "WHEN IGC_Aciklama_Id = '7' THEN 'Malulen Emeklilik' "+
            "WHEN IGC_Aciklama_Id = '8' THEN 'Ölüm' "+
            "WHEN IGC_Aciklama_Id = '9' THEN 'İş Kazası' "+
            "WHEN IGC_Aciklama_Id = '10' THEN 'Askerlik' "+
            "WHEN IGC_Aciklama_Id = '11' THEN 'Diğer Sebepler' "+
            "ELSE 'Diğer' "+ "END AS IGCA_Aciklama FROM tblIseGirisCikis where IGC_Persid = '" + RouteData.Values["id"] + "'", connect);
            DataTable table = new DataTable();
            komut.Fill(table);

            Repeater1.DataSource = table;
            Repeater1.DataBind();


            {
                //Veri güncellemede çıkış tarihi olmadan giriş tarihi eklemesi yapılamaz.
                int bos_tarih = 0;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["IGC_Cikis_Tarihi"].ToString() == DBNull.Value.ToString())
                    {
                        bos_tarih += 1;
                    }
                }

                if (bos_tarih > 0)
                {
                    if (Panel2.Visible != true)
                    {
                        ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("uyari", "Personele işe giriş tarihi ekleyemezsiniz. Sistemde kayıtlı çıkış tarihi olmayan giriş tarihi bulundu.");
                        kayit.Enabled = false;
                    }
                }
            }
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "");
        }        
        connect.Close();
    }


    //Personelin Güncellenebilir Giriş Çıkış Bilgilerini Getir.
    public void giris_cikis_bilgilerini_getir()
    {
        connect.Open();
        SqlDataAdapter komut = new SqlDataAdapter("Select * From tblIseGirisCikis where IGC_Id='" + RouteData.Values["veriid"] + "' order by IGC_Id DESC ", connect);
        DataTable table = new DataTable();
        komut.Fill(table);
        if (!IsPostBack)
        {
            giristarihi_textbox.Value = Convert.ToDateTime(table.Rows[0]["IGC_Giris_Tarihi"]).ToString("dd.MM.yyyy");
            if (table.Rows[0]["IGC_Cikis_Tarihi"].ToString() != "")
            {               
                cikistarihi_textbox.Value = Convert.ToDateTime(table.Rows[0]["IGC_Cikis_Tarihi"]).ToString("dd.MM.yyyy");

                //Tür Dropdown

                //Tür Dropdown
                cikma_sebebi_drop.DataBind();
                foreach (ListItem dongu in cikma_sebebi_drop.Items)
                {
                    if (dongu.Value.ToString() == table.Rows[0]["IGC_Aciklama_Id"].ToString())
                    {
                        cikma_sebebi_drop.DataBind();
                        cikma_sebebi_drop.SelectedValue = table.Rows[0]["IGC_Aciklama_Id"].ToString();
                        break;
                    }
                    else
                    {
                        cikma_sebebi_drop.SelectedValue = "0";
                    }
                }
            }
        }

        if (table.Rows.Count>0)
        {
            if (table.Rows[0]["IGC_Cikis_Tarihi"].ToString() != "")
            {
                buttonlar_panel.Visible = true;
            }
        }        
        connect.Close();
    }

   



    SqlCommand komut, komut2;
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            if (cikistarihi_textbox.Value != "")
            {
                if (cikma_sebebi_drop.SelectedValue == "0")
                {
                    ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("uyari", "Lütfen çıkış nedeni alanını boş bırakmayınız.");
                    return;
                }
            }

            connect.Open();

            //Eğer Ekran Bilgi Güncelleme Sayfasında İse
            if (RouteData.Values["veriid"] != null)
            {
                komut = new SqlCommand("update tblIseGirisCikis set IGC_Persid=@IGC_Persid, IGC_Giris_Tarihi=@IGC_Giris_Tarihi, IGC_Cikis_Tarihi=@IGC_Cikis_Tarihi, IGC_Aciklama_Id=@IGC_Aciklama_Id where IGC_Id='" + RouteData.Values["veriid"] + "'", connect);
            }
            else
            {   
                komut = new SqlCommand("insert into tblIseGirisCikis (IGC_Persid, IGC_Giris_Tarihi, IGC_Aciklama_Id)values(@IGC_Persid, @IGC_Giris_Tarihi, @IGC_Aciklama_Id)", connect);
                komut.Parameters.AddWithValue("@IGC_Cikis_Tarihi", DBNull.Value);           
            }

            if (cikma_sebebi_drop.SelectedItem.Value == "0")
            {
                komut.Parameters.AddWithValue("@IGC_Aciklama_Id", DBNull.Value);
            }
            else
            {
                komut.Parameters.AddWithValue("@IGC_Aciklama_Id", cikma_sebebi_drop.SelectedValue);
            }
            

            komut.Parameters.AddWithValue("@IGC_Persid", RouteData.Values["id"]);
            komut.Parameters.AddWithValue("@IGC_Giris_Tarihi", Convert.ToDateTime(giristarihi_textbox.Value).ToString("yyyy.MM.dd"));            
            
            if (RouteData.Values["veriid"] != null)
            {
                if (cikistarihi_textbox.Value!="")
                {
                    komut.Parameters.AddWithValue("@IGC_Cikis_Tarihi", Convert.ToDateTime(cikistarihi_textbox.Value).ToString("yyyy.MM.dd"));
                }
                else
                {
                    komut.Parameters.AddWithValue("@IGC_Cikis_Tarihi", DBNull.Value);
                }
            }
            komut.ExecuteNonQuery();
            connect.Close();
            giris_cikis_bilgilerini_getir();
            verileri_yukle();
            

            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("basari", "");
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "");
        }
    }


    //Eğer işe giriş çıkış sil komutu gönderilirse
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "sil") //Eğer işe giriş çıkış sil komutu tetiklenmişse
            {
                connect.Open();
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                SqlCommand sil = new SqlCommand("delete from tblIseGirisCikis where IGC_Id='" + id + "'", connect);
                sil.ExecuteNonQuery();
                connect.Close();

                ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("basari", "");
                Response.Redirect("/Personel-Giris/" + RouteData.Values["id"] + "");
            }
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "");
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Personel-Giris/" + RouteData.Values["id"] + "");
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs a)
    {
        if (a.Item.ItemType == ListItemType.Item || a.Item.ItemType == ListItemType.AlternatingItem)
        {
            Panel button_panel = (Panel)a.Item.FindControl("button_panel");

            if (buton_yetki=="0")
            {
                button_panel.Visible = false;
            }
        }
    }
}