using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Modul_Cari_Kayit : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("Cari_Bilgileri_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        //İnsan kaynakları Yazma
        if (Sorgu.yetkiteklikontrol("Cari_Bilgileri_Yazma") != "1")
        {
            g_panel.Visible = false;
        }
        //Güvenlik///////////////////////


        //Personel Kayıt İşleminden Sonra Personel Bilgileri Güncelleme Panelinde,
        //Tekrar Getiriliyor. Tekrar Kayıt Olmaması İçin ve Mesaj Çıkartılıyor.
        if (RouteData.Values["id"] != null)
        {
            //Personelin Bilgilerini Ekrana Getir.
            bilgi_getir();
            Cari_Kodu.Enabled = false;
        }
        else
        {
            if (!IsPostBack)
            {
                Cari_Kodu.Text = Sorgu.cari_kod_uret();
                Cari_Turu_Drop.SelectedValue = RouteData.Values["gorev"].ToString();
                Cari_Turu_Drop.Enabled = false;
            }
        }
    }



    //Personelin Detaylı Bilgileri
    public void bilgi_getir()
    {
        try
        {
            //Personel Verilerini SQL' de Bul.
            connect.Open();            
            SqlDataAdapter komut = new SqlDataAdapter("Select * from tblCariKayit where Cari_Id='" +  RouteData.Values["id"].ToString() + "'", connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);


            if (!IsPostBack)
            {
                //Personel Bilgilerini Ekrana Getir.
                Cari_Kodu.Text = tablo.Rows[0]["Cari_Kodu"].ToString();
                Cari_Turu_Drop.SelectedValue = tablo.Rows[0]["Cari_Turu"].ToString();
                Cari_Unvan.Text = tablo.Rows[0]["Cari_Unvan"].ToString();
                Cari_Yetkili_Kisi.Text = tablo.Rows[0]["Cari_Yetkili"].ToString();
                Cari_Adres.Text = tablo.Rows[0]["Cari_Adres"].ToString();
                Cari_Telefon.Text = tablo.Rows[0]["Cari_Telefon"].ToString();
                Cari_Telefon2.Text = tablo.Rows[0]["Cari_Telefon2"].ToString();

                Cari_VergiDairesi.Text = tablo.Rows[0]["Cari_VergiDairesi"].ToString();
                Cari_VergiNumarasi.Text = tablo.Rows[0]["Cari_VergiNo"].ToString();
                Cari_Web.Text = tablo.Rows[0]["Cari_WebSites"].ToString();
                Cari_EmailAdresi.Text = tablo.Rows[0]["Cari_Email"].ToString();
                Cari_KimlikNumarasi.Text = tablo.Rows[0]["Cari_KimlikNo"].ToString();

                //Personel Sisteme Giriş İzni
                if (tablo.Rows[0]["Cari_Aktif"].ToString() != "")
                {
                    Cari_Aktif.Checked = Convert.ToBoolean(tablo.Rows[0]["Cari_Aktif"]);
                }
                else
                {
                    Cari_Aktif.Checked = true;
                }

                //Görev Seçimi
                sehir_drop.DataBind();
                foreach (ListItem dongu in sehir_drop.Items)
                {
                    if (dongu.Value.ToString() == tablo.Rows[0]["Cari_Sehir"].ToString())
                    {
                        sehir_drop.SelectedValue = tablo.Rows[0]["Cari_Sehir"].ToString();
                        ilce_secimi();
                        Cari_İlce.SelectedValue = tablo.Rows[0]["Cari_İlce"].ToString();
                        break;
                    }
                    else
                    { sehir_drop.SelectedValue = "0"; }
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
            //Kimlik Numarası, Vergi Dairesi, Vergi Numarası Alan Kontrolü
            if (Cari_KimlikNumarasi.Text == "" && Cari_VergiDairesi.Text == "" && Cari_VergiNumarasi.Text == "")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Kimlik Numarası, Vergi Numarası Alanlarından En Az Birisi Boş Bırakılabilir.").ToString(), true);
                return;
            }
            else
            {
                if ((Cari_VergiDairesi.Text != "" && Cari_VergiNumarasi.Text == "") || (Cari_VergiDairesi.Text == "" && Cari_VergiNumarasi.Text != ""))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Veri Dairesi ve Vergi Numarası Alanlarının İkiside Boş Bırakılamaz.").ToString(), true);
                    return;
                }
            }


            connect.Open();
            if (RouteData.Values["id"] != null)//Eğer Ekran Bilgi Güncelleme Sayfasında İse
            {
                komut = new SqlCommand("update tblCariKayit set Cari_Turu=@Cari_Turu, Cari_Kodu=@Cari_Kodu, Cari_Unvan=@Cari_Unvan, Cari_Adres=@Cari_Adres, Cari_Telefon=@Cari_Telefon, Cari_Telefon2=@Cari_Telefon2, Cari_VergiDairesi=@Cari_VergiDairesi, Cari_VergiNo=@Cari_VergiNo, Cari_KimlikNo=@Cari_KimlikNo, Cari_Sehir=@Cari_Sehir, Cari_İlce=@Cari_İlce, Cari_Yetkili=@Cari_Yetkili, Cari_WebSites=@Cari_WebSites, Cari_Email=@Cari_Email, Cari_GuncellemeTarih=@Cari_GuncellemeTarih, Cari_Aktif=@Cari_Aktif where Cari_Id='" + RouteData.Values["id"].ToString() + "'", connect);

            }
            else
            {
                komut = new SqlCommand("insert into tblCariKayit (Cari_Turu, Cari_Kodu, Cari_Unvan, Cari_Adres, Cari_Telefon, Cari_Telefon2, Cari_VergiDairesi, Cari_VergiNo, Cari_KimlikNo, Cari_Sehir, Cari_İlce, Cari_Yetkili, Cari_WebSites, Cari_Email, Cari_KayitTarih, Cari_Aktif)values" +
                                       "(@Cari_Turu, @Cari_Kodu, @Cari_Unvan, @Cari_Adres, @Cari_Telefon, @Cari_Telefon2, @Cari_VergiDairesi, @Cari_VergiNo, @Cari_KimlikNo, @Cari_Sehir, @Cari_İlce, @Cari_Yetkili, @Cari_WebSites, @Cari_Email, @Cari_KayitTarih, @Cari_Aktif)", connect);
            }
            
            komut.Parameters.AddWithValue("@Cari_Turu", Cari_Turu_Drop.SelectedValue);
            komut.Parameters.AddWithValue("@Cari_Kodu", Cari_Kodu.Text);
            komut.Parameters.AddWithValue("@Cari_Unvan", Cari_Unvan.Text);
            komut.Parameters.AddWithValue("@Cari_Adres", Cari_Adres.Text);
            komut.Parameters.AddWithValue("@Cari_Telefon", Cari_Telefon.Text);
            komut.Parameters.AddWithValue("@Cari_Telefon2", Cari_Telefon2.Text);
            komut.Parameters.AddWithValue("@Cari_VergiDairesi", Cari_VergiDairesi.Text);
            komut.Parameters.AddWithValue("@Cari_VergiNo", Cari_VergiNumarasi.Text);
            komut.Parameters.AddWithValue("@Cari_Sehir", sehir_drop.SelectedValue);
            komut.Parameters.AddWithValue("@Cari_İlce", Cari_İlce.Text);
            komut.Parameters.AddWithValue("@Cari_Yetkili", Cari_Yetkili_Kisi.Text);
            komut.Parameters.AddWithValue("@Cari_WebSites", Cari_Web.Text);
            komut.Parameters.AddWithValue("@Cari_Email", Cari_EmailAdresi.Text);
            komut.Parameters.AddWithValue("@Cari_KayitTarih", Convert.ToDateTime(DateTime.Now.ToString("yyyy.MM.dd")));
            komut.Parameters.AddWithValue("@Cari_GuncellemeTarih", Convert.ToDateTime(DateTime.Now.ToString("yyyy.MM.dd")));
            komut.Parameters.AddWithValue("@Cari_Aktif", Cari_Aktif.Checked.ToString());
            komut.Parameters.AddWithValue("@Cari_KimlikNo", Cari_KimlikNumarasi.Text);


            //Kayıt İşlemi ve Update İşlemi Burda Başlıyor.
            if (RouteData.Values["id"] != null)//Eğer Ekran Bilgi Güncelleme Sayfasında İse
            {                
                if (Sorgu.cari_varmi("Guncelle", RouteData.Values["id"].ToString(), Cari_Kodu.Text, Cari_VergiNumarasi.Text, Cari_Turu_Drop.SelectedValue, Cari_KimlikNumarasi.Text).ToString() == "0")
                {
                    //Yoksa Kayıt İşlemini tamamla
                   komut.ExecuteNonQuery();
                   komut.Dispose();
                   connect.Close();
                   ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
                }
                else
                {
                    //Varsa Uyarı Ver.
                   ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari kodu veya vergi numarası sistemde kayıtlı. Lütfen kontrol ediniz.").ToString(), true);
                   return;
                }
            }
            else
            {
                //Aynı Tc Kimlik Numarası Sistemde Kayıtlımı Kontrol Et.
                if (Sorgu.cari_varmi("Kayit", "", Cari_Kodu.Text, Cari_VergiNumarasi.Text, Cari_Turu_Drop.SelectedValue, Cari_KimlikNumarasi.Text) != "1")
                {
                    //Yoksa Kayıt İşlemini tamamla
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    connect.Close();
                    Response.Redirect("/Cari-Listesi/" + RouteData.Values["gorev"] + "");
                }
                else
                {
                    //Varsa Uyarı Ver.
                   ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Cari kodu veya vergi numarası sistemde kayıtlı. Lütfen kontrol ediniz.").ToString(), true);
                   return;
                }
                //Kayıt İşlemi Son 
            }
        }
        catch (Exception)
        {
           ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Lütfen Alanları Kontrol Ediniz.").ToString(), true);            
        }
    }

    protected void urun_ekle_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Cari-Satis-UrunEkle/" + RouteData.Values["gorev"] + "/" + RouteData.Values["id"] + "");
    }

    protected void Stok_Ekle_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Cari-Stok-Kayit/"+ RouteData.Values["gorev"] +"/"+ RouteData.Values["id"] + "");
    }

    protected void Bakim_Ekle_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Bakim-Ekle/"+RouteData.Values["gorev"]+"/" + RouteData.Values["id"] + "");
    }

    protected void Revizyon_Ekle_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Revizyon-Ekle/" + RouteData.Values["gorev"] + "/" + RouteData.Values["id"] + "");
    }

    protected void Ariza_Ekle_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Ariza-Ekle/" + RouteData.Values["gorev"] + "/" + RouteData.Values["id"] + "");
    }

    protected void sehir_drop_SelectedIndexChanged(object sender, EventArgs e)
    {
        ilce_secimi();
    }

    public void ilce_secimi()
    {
        Cari_İlce.Items.Clear();

        if (RouteData.Values["id"] == null)
        {
            Cari_İlce.Items.Add(new ListItem("Seçiminiz", "0"));
            Cari_İlce.SelectedValue = "0";
        }

        ilce_sql.SelectCommand = "Select * from tblSIlceler where tblSIlceler_SehirId='" + sehir_drop.SelectedValue + "'";
        ilce_sql.DataBind();
        Cari_İlce.DataBind();
    }
}