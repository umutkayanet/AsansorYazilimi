using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Modul_Personel_Kayit : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
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
        }
        //Güvenlik///////////////////////


        //Personel Kayıt İşleminden Sonra Personel Bilgileri Güncelleme Panelinde,
        //Tekrar Getiriliyor. Tekrar Kayıt Olmaması İçin ve Mesaj Çıkartılıyor.
        if (RouteData.Values["id"] != null)
        {
            if (RouteData.Values["m"] != null)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", 
                Sorgu.mesaj("y_basari", "Şimdi personelin resim ve işe giriş çıkış tarihlerini ekleyebilirsiniz.").ToString(), true);
            }
            //Personelin Bilgilerini Ekrana Getir.
            bilgi_getir();
        }
    }


    //Personelin Detaylı Bilgileri
    public void bilgi_getir()
    {
        try
        {
            //Personel Verilerini SQL' de Bul.
            connect.Open();            
            SqlDataAdapter komut = new SqlDataAdapter("Select * from Personel where Persid='" +  RouteData.Values["id"].ToString() + "'", connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);


            if (!IsPostBack)
            {
                //Resim yükleme, İşe Giriş ve Yetki Butonları Ekranda Göster
                buttonlar_panel.Visible = true;

                //Personel Resmini Yükle
                personel_resim.ImageUrl = Sorgu.GetImageUrl("/Data/personel_resim/kucuk_resim/" + tablo.Rows[0]["Foto"] + "");

                //Personel Bilgilerini Ekrana Getir.
                adi_textbox.Text = tablo.Rows[0]["Pers_Adi"].ToString();
                tc_texbox.Text = tablo.Rows[0]["Pers_Tc"].ToString();
                soyadi_textbox.Text = tablo.Rows[0]["Pers_Soyadi"].ToString();
                dogumtarihi_textbox.Text = Convert.ToDateTime(tablo.Rows[0]["Pers_Dogum_Tarihi"]).ToString("dd.MM.yyyy");
                kullaniciadi_textbox.Text = tablo.Rows[0]["Email"].ToString();
                mezuniyetyili_textbox.Value = tablo.Rows[0]["Mezuniyet_yil"].ToString();
                personel_calisiyor.Checked = Convert.ToBoolean(tablo.Rows[0]["Calisan"]);            
                sirket_gsm_textbox.Text = tablo.Rows[0]["Pers_TelSirketCep"].ToString();

                //Cinsiyet Seçimi Yap
                if (tablo.Rows[0]["Pers_Cinsiyet"].ToString()!=string.Empty)
                {
                    cinsiyet_drop.SelectedValue = tablo.Rows[0]["Pers_Cinsiyet"].ToString();
                }

                //Personel Sisteme Giriş İzni
                if (tablo.Rows[0]["Pers_Sisteme_Giris_Izni"].ToString() != "")
                {
                    sisteme_giris_izni.Checked = Convert.ToBoolean(tablo.Rows[0]["Pers_Sisteme_Giris_Izni"]);
                }
                else
                {
                    sisteme_giris_izni.Checked = true;
                }

                //Görev Seçimi
                gorev_drop.DataBind();
                foreach (ListItem dongu in gorev_drop.Items)
                {
                    if (dongu.Value.ToString() == tablo.Rows[0]["Gorev_id"].ToString())
                    {
                        gorev_drop.SelectedValue = tablo.Rows[0]["Gorev_id"].ToString();
                        break;
                    }
                    else
                    { gorev_drop.SelectedValue = "0"; }
                }


                //Eğitim Seçimi
                egitim_textbox.DataBind();
                foreach (ListItem dongu in egitim_textbox.Items)
                {
                    if (dongu.Value.ToString() == tablo.Rows[0]["Pers_EgitimDurumu"].ToString())
                    {
                        egitim_textbox.SelectedValue = tablo.Rows[0]["Pers_EgitimDurumu"].ToString();
                        break;
                    }
                    else
                    { egitim_textbox.SelectedValue = "0"; }
                }
            }
            //Bağlantıları Kapat
            komut.Dispose();
            connect.Close();
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);
        }
    }


    //Kayıt Güncelleme ve Ekleme Alanı
    SqlCommand komut;
    string organize_deger = "";
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {            
            connect.Open();
            if (RouteData.Values["id"] != null)//Eğer Ekran Bilgi Güncelleme Sayfasında İse
            {
                //Eğer Bilgi Güncelleme Sayfasında İse ve Şifre Alanı Boş Değilse Password Güncelle
                string sifresorgu = "";
                if (sifresi_textbox.Text != "" || sifresi_textbox.Text != string.Empty)
                {
                    sifresorgu = " password=@password,";
                }
                komut = new SqlCommand("update Personel set Pers_Tc=@Pers_Tc, Pers_Adi=@Pers_Adi, Pers_Soyadi=@Pers_Soyadi, Pers_Dogum_Tarihi=@Pers_Dogum_Tarihi, Email=@Email, " + sifresorgu + " Pers_Sisteme_Giris_Izni=@Pers_Sisteme_Giris_Izni, Mezuniyet_yil=@Mezuniyet_yil, Gorev_id=@Gorev_id, Calisan=@Calisan, Pers_EgitimDurumu=@Pers_EgitimDurumu, Pers_Cinsiyet=@Pers_Cinsiyet, Pers_TelSirketCep=@Pers_TelSirketCep where Persid='" + RouteData.Values["id"].ToString() + "'", connect);
            }
            else
            {
                komut = new SqlCommand("insert into Personel (Pers_Tc, Pers_Adi, Pers_Soyadi, Pers_Dogum_Tarihi, Email, password, Pers_Sisteme_Giris_Izni, Mezuniyet_yil, Gorev_id, Pers_EgitimDurumu, Calisan, Pers_Cinsiyet, Pers_TelSirketCep)values(@Pers_Tc, @Pers_Adi, @Pers_Soyadi, @Pers_Dogum_Tarihi, @Email, @password, @Pers_Sisteme_Giris_Izni, @Mezuniyet_yil, @Gorev_id, @Pers_EgitimDurumu, @Calisan, @Pers_Cinsiyet, @Pers_TelSirketCep)", connect);
            }
            
            komut.Parameters.AddWithValue("@Pers_Tc", tc_texbox.Text.Trim());
            komut.Parameters.AddWithValue("@Pers_Adi", adi_textbox.Text.Trim());
            komut.Parameters.AddWithValue("@Pers_Soyadi", soyadi_textbox.Text.Trim());
            komut.Parameters.AddWithValue("@Pers_Dogum_Tarihi", Convert.ToDateTime(dogumtarihi_textbox.Text).ToString("yyyy.MM.dd"));
            komut.Parameters.AddWithValue("@Email", kullaniciadi_textbox.Text.Trim());
            komut.Parameters.AddWithValue("@password", sifresi_textbox.Text.Trim());
            komut.Parameters.AddWithValue("@Pers_Sisteme_Giris_Izni", sisteme_giris_izni.Checked.ToString());
            komut.Parameters.AddWithValue("@Mezuniyet_yil", mezuniyetyili_textbox.Value);
            komut.Parameters.AddWithValue("@Gorev_id", gorev_drop.SelectedItem.Value);
            komut.Parameters.AddWithValue("@Calisan", personel_calisiyor.Checked.ToString());
            komut.Parameters.AddWithValue("@Pers_EgitimDurumu", egitim_textbox.SelectedItem.Value);
            komut.Parameters.AddWithValue("@Pers_Cinsiyet", cinsiyet_drop.SelectedItem.Value);
            komut.Parameters.AddWithValue("@Pers_TelSirketCep", sirket_gsm_textbox.Text);


            //Kayıt İşlemi ve Update İşlemi Burda Başlıyor.
            if (RouteData.Values["id"] != null)//Eğer Ekran Bilgi Güncelleme Sayfasında İse
            {                
                if (Sorgu.kullanici_varmi("Guncelle", RouteData.Values["id"].ToString(), tc_texbox.Text).ToString() == "0")
                {
                    //Yoksa Kayıt İşlemini tamamla
                    komut.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
                }
                else
                {
                    //Varsa Uyarı Ver.
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "TC Kimlik Numarası sistemde kayıtlı. Lütfen kontrol ediniz.").ToString(), true);
                    return;
                }
            }
            else
            {
                //Aynı Tc Kimlik Numarası Sistemde Kayıtlımı Kontrol Et.
                if (Sorgu.kullanici_varmi("Kayit", "", tc_texbox.Text) != "1")
                {
                    //Yoksa Kayıt İşlemini tamamla
                    komut.ExecuteNonQuery();

                    SqlDataAdapter eklenen_personeli_bul = new SqlDataAdapter("Select * from Personel where Pers_TC='" + tc_texbox.Text + "'", connect);
                    DataTable eklenen_personeli_bul_tab = new DataTable();
                    eklenen_personeli_bul.Fill(eklenen_personeli_bul_tab);

                    yetki_kaydet(eklenen_personeli_bul_tab.Rows[0]["Persid"].ToString());

                    Response.Redirect("/Personel-Islem/Guncelle/1/"+ eklenen_personeli_bul_tab.Rows[0]["Persid"] + "");                    
                }
                else
                {
                    //Varsa Uyarı Ver.
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "TC Kimlik Numarası sistemde kayıtlı. Lütfen kontrol ediniz.").ToString(), true);
                    return;
                }
                //Kayıt İşlemi Son 
            }            
            komut.Dispose();
            connect.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);            
        }
    }

    //Kullanıcının yetkisi (İlk Kayıtta) Standar Olarak Veri Kaydı Yapılıyor. 
    public void yetki_kaydet(string persid)
    {
        try
        {
            SqlDataAdapter standartyetki = new SqlDataAdapter("Select * from tblYetkiRol where YR_Standart='True'", connect);
            DataTable standartb = new DataTable();
            standartyetki.Fill(standartb);

            if (standartb.Rows.Count > 0)
            {
                for (int i = 0; i < standartb.Rows.Count; i++)
                {
                    SqlCommand peryetkay = new SqlCommand("INSERT INTO tblYetkiRolPersonel(YRP_Persid, YRP_Rol_Id) VALUES('" + persid + "','" + standartb.Rows[i]["YR_Id"].ToString() + "')", connect);
                    peryetkay.ExecuteNonQuery();
                }
            }
        }
        catch (Exception)
        {
            
        }            
    }
}