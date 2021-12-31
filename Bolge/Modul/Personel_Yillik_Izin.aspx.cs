using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Modul_Personel_Yillik_Izin : System.Web.UI.Page
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
            yeni_kayit.Visible = false;
            Duzenle_Button.Visible = false;
        }
        //Güvenlik///////////////////////

        //Pop Açılan Mesajı Temizle
        pop_mesaj.Text = "";
        connect.Open();
        try
        {
            if (!IsPostBack)
            {
                //Personel Bilgileri Alınıyor.
                SqlCommand komut = new SqlCommand("Select Pers_Adi, Pers_Soyadi, Pers_Dogum_tarihi, Foto from personel where Persid='" + RouteData.Values["id"] + "'", connect);
                SqlDataReader reader;
                reader = komut.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    //Eğer Personel Bilgileri Varsa
                    if (reader.HasRows)
                    {
                        //Adı,soyadı ve doğrum tarihini ekrana yazdır.
                        adi_soyadi_label.Text = reader["Pers_Adi"].ToString() + " " + reader["Pers_Soyadi"].ToString();

                        //Eğer personel doğum tarihi boş değilse
                        if (reader["Pers_Dogum_tarihi"].ToString() != "")
                        {
                            dogum_tarihi_label.Text = Convert.ToDateTime(reader["Pers_Dogum_tarihi"]).ToString("dd.MM.yyyy");

                            //Yaşı Hesapla
                            int tarih1 = 0;
                            int tarih2 = 0;
                            tarih1 = Convert.ToInt32(Convert.ToDateTime(reader["Pers_Dogum_tarihi"]).ToString("yyyy"));
                            tarih2 = DateTime.Now.Year;
                            yas_label.Text = Convert.ToString(tarih2 - tarih1);
                        }
                        else
                        {
                            dogum_tarihi_label.Text = "Doğum Tarihi Bulunamadı";
                            yas_label.Text = "Doğum Tarihi Bulunamadı.";
                        }
                    }

                    //Personel resmini sisteme yükle
                    personel_resim.ImageUrl = Sorgu.GetImageUrl("/Data/personel_resim/kucuk_resim/" + reader["Foto"] + "");
                }

                //Personel İşe Giriş Çıkış Bilgileri Alınıyor.
                SqlCommand ise_giris_bul = new SqlCommand("Select IGC_Giris_Tarihi from tblIseGirisCikis where IGC_Persid='" + RouteData.Values["id"] + "' order by IGC_Giris_Tarihi desc ", connect);
                SqlDataReader reader2;
                reader2 = ise_giris_bul.ExecuteReader();

                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        ise_giris_tarihi_label.Text += Convert.ToDateTime(reader2["IGC_Giris_Tarihi"]).ToString("dd.MM.yyyy") + ", ";
                    }    
                    //Sıralana işe giriş çıkış tarihlerindeki son karekteri sil.
                    string son_kerekter_sil = ise_giris_tarihi_label.Text.Substring(0, ise_giris_tarihi_label.Text.Length - 2).ToString();
                    ise_giris_tarihi_label.Text = son_kerekter_sil;
                }
            }
        }
        catch (Exception)
        {
          ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);
        }
        connect.Close();
        izinlistesiyukle();
    }


    //Personel Açılır açılmaz izin listesi yükleniyor.
    public void izinlistesiyukle()
    {
        try
        {
            decimal toplam_izin = 0;
            decimal umut = 0;
            decimal gun_sayisi_deg;

            connect.Open();
            SqlDataAdapter komut = new SqlDataAdapter("Select tblYillikIzin.YillikIzin_Id, tblYillikIzin.YillikIzin_Persid, tblYillikIzin.YillikIzin_Baslama_Tarihi,tblYillikIzin.YillikIzin_Aciklama_Id," +
                                                      "tblYillikIzin.YillikIzin_Bitis_Tarihi, tblYillikIzin.YillikIzin_Gun_Sayisi, tblYillikIzinAciklama.YillikIzinAc_Aciklama, tblYillikIzin.YillikIzin_Not, tblYillikIzinAciklama.YillikIzinAc_Gun_Sayma " +
                                                      "From tblYillikIzinAciklama right Join  tblYillikIzin " +
                                                      "on(tblYillikIzin.YillikIzin_Aciklama_Id = tblYillikIzinAciklama.YillikIzinAc_Id) " +
                                                      "where YillikIzin_Persid ='" + RouteData.Values["id"] + "' order by YillikIzin_Baslama_Tarihi", connect);
            DataTable tablo = new DataTable();
            komut.Fill(tablo);


            //Eğer Personel Sistemde Varsa
            if (tablo.Rows.Count > 0)
            {
                personel_izin_rep.DataSource = tablo;
                personel_izin_rep.DataBind();

                foreach (RepeaterItem item in personel_izin_rep.Items)
                {
                    //Verileri Listeden Al
                    Label kalan_izin_label = (Label)item.FindControl("kalan_izin_label");

                    Label Aciklama = (Label)item.FindControl("Aciklama");
                    HiddenField Aciklama_kod = (HiddenField)item.FindControl("Aciklama_kod");

                    Label Gun_Sayisi = (Label)item.FindControl("Gun_Sayisi");
                    HiddenField Gun_Sayma = (HiddenField)item.FindControl("Gun_Sayma");

                    //Eğer eklene izinde eklenen gün sayıl sayılırsa
                    if (Gun_Sayma.Value.ToLower() == "false")
                    {
                        gun_sayisi_deg = 0;
                    }
                    else
                    {
                        //Sayılmazsa direk gün sayısını ekrana yaz
                        gun_sayisi_deg = Convert.ToDecimal(Gun_Sayisi.Text);
                    }

                    
                    //Eğer eklenen gün sayılmasın isterse
                    if (Gun_Sayma.Value.ToLower() != "false")
                    {
                        umut = Convert.ToDecimal(toplam_izin) + gun_sayisi_deg;
                        toplam_izin = toplam_izin + gun_sayisi_deg;

                        if (umut < 0)
                        {
                            kalan_izin_label.Text = "<span class='label label-danger'>" + umut.ToString() + "</span>";
                        }
                        else
                        {
                            kalan_izin_label.Text = umut.ToString();
                        }
                    }
                    else
                    {
                        Aciklama.Text = Aciklama.Text + "";
                        toplam_izin = toplam_izin + gun_sayisi_deg;
                        kalan_izin_label.Text = toplam_izin.ToString();
                    }


                    if (Convert.ToDecimal(Gun_Sayisi.Text)<=0)
                    {
                        Gun_Sayisi.Text = "<span class='label label-danger'>" + Gun_Sayisi.Text + "</span>";
                    }
                    else
                    {
                        Gun_Sayisi.Text = "<span class='label label-success'>" + Gun_Sayisi.Text + "</span>";
                    }
                }
            }
        }
        catch (Exception)
        {
          ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Personel yıllık izin listesi yüklenemedi.").ToString(), true);
        }
        connect.Close();
    }

    //Personel İzin Güncelle Sayfasında ise
    protected void Duzenle_Button_Click(object sender, EventArgs e)
    {
        kaydet_guncelle();
    }

    //Eğer Yeni Kayıt Eklenecekse
    protected void yeni_kayit_Click(object sender, EventArgs e)
    {
        guncelle_id.Value = "";
        popbaslamatarihi.Text = "";
        popbitistarihi.Text = "";
        popgunsayisi.Text = "";
        popnot.Text = "";
        DropDownList1.SelectedValue = "0";
        ModalPopupExtender1.Show();
        popbaslamatarihi.Focus();
    }

    //Bu değişken sayfadaki güncelleme ve ekleme işlemlerini tek modülde yapmak için eklendi. 1-0 durumuna göre ekle veya
    SqlCommand kayit_ekle;
    public void kaydet_guncelle()
    {
        try
        {
            connect.Open();
            string ara_drop, ara_bitis, ara_baslan, ara_not, ara_gun = "";

            //Verileri Değişkenlere Ata
            ara_drop = DropDownList1.SelectedItem.Value;
            ara_baslan = popbaslamatarihi.Text;
            ara_bitis = popbitistarihi.Text;
            ara_not = popnot.Text;
            ara_gun = popgunsayisi.Text;
            ModalPopupExtender1.Show();



            //Bitiş Tarihi Zorunlu Olmayan Açıklaların Bitiş Tarihini Sil
            string bitis_tarihi_durumu;
            if (izinvarmi(ara_drop) == "False")
            {
                ara_bitis = "";
                bitis_tarihi_durumu = "False";
            }
            else
            {
                bitis_tarihi_durumu = "True";
            }


            //Tarih ve Açılış Seçeneği Sistem Kontrolü
            string kontrol_mesaj = iki_tarih_arasi_kontrol(ara_baslan, ara_bitis, guncelle_id.Value, DropDownList1.SelectedValue);
            if (kontrol_mesaj != "")
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", kontrol_mesaj).ToString(), true);
                return;
            }
            

            //eğer güncelleme emri geldi ise
            if (guncelle_id.Value != "")
            {
                kayit_ekle = new SqlCommand("update tblYillikIzin set YillikIzin_Baslama_Tarihi=@YillikIzin_Baslama_Tarihi, YillikIzin_Bitis_Tarihi=@YillikIzin_Bitis_Tarihi, YillikIzin_Aciklama_Id=@YillikIzin_Aciklama_Id, YillikIzin_Gun_Sayisi=@YillikIzin_Gun_Sayisi, YillikIzin_Not=@YillikIzin_Not where YillikIzin_Id='" + guncelle_id.Value + "' and YillikIzin_Persid='" + RouteData.Values["id"] + "' ", connect);
            }
            else
            {
                kayit_ekle = new SqlCommand("insert into tblYillikIzin (YillikIzin_Persid, YillikIzin_Baslama_Tarihi, YillikIzin_Bitis_Tarihi, YillikIzin_Aciklama_Id, YillikIzin_Gun_Sayisi, YillikIzin_Not)values(@YillikIzin_Persid, @YillikIzin_Baslama_Tarihi, @YillikIzin_Bitis_Tarihi, @YillikIzin_Aciklama_Id, @YillikIzin_Gun_Sayisi, @YillikIzin_Not)", connect);
                kayit_ekle.Parameters.AddWithValue("@YillikIzin_Persid", RouteData.Values["id"]);
            }

            kayit_ekle.Parameters.AddWithValue("@YillikIzin_Baslama_Tarihi", Convert.ToDateTime(ara_baslan).ToString("yyyy.MM.dd"));
            kayit_ekle.Parameters.AddWithValue("@YillikIzin_Aciklama_Id", ara_drop);
            kayit_ekle.Parameters.AddWithValue("@YillikIzin_Gun_Sayisi", ara_gun);
            kayit_ekle.Parameters.AddWithValue("@YillikIzin_Not", ara_not);

            //Eğer güncellenecek ve eklenecek verinin bitiş tarihi zorunlu
            if (bitis_tarihi_durumu == "True" && ara_bitis != string.Empty)
            {

                kayit_ekle.Parameters.AddWithValue("@YillikIzin_Bitis_Tarihi", Convert.ToDateTime(ara_bitis).ToString("yyyy.MM.dd"));
            }
            else if (bitis_tarihi_durumu == "True" && ara_bitis == string.Empty)
            {
                pop_mesaj.Text = "Lütfen bitiş tarihi alanını boş bırakmayınız.";
                return;
            }
            else if (bitis_tarihi_durumu == "False")
            {
                kayit_ekle.Parameters.AddWithValue("@YillikIzin_Bitis_Tarihi", DBNull.Value);
            }
            kayit_ekle.ExecuteNonQuery();
            connect.Close();
            ModalPopupExtender1.Hide();
            izinlistesiyukle();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
        }
        catch (Exception)
        {   
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);
        }       
    }

    //Personel İzin Güncellemede Pop Menü ve Veri Okuma Alanı
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Duzenle") //Eğer işe giriş çıkış sil komutu tetiklenmişse
            {
                //Değişkeni al
                int id = Convert.ToInt32(e.CommandArgument.ToString());

                //ve personel verisini oku
                SqlDataAdapter komut = new SqlDataAdapter("Select * From tblYillikIzin where YillikIzin_Id='" + id + "'", connect);
                DataTable table = new DataTable();
                komut.Fill(table);

                //Pencereyi Aç
                ModalPopupExtender1.Show();
                popbaslamatarihi.Focus();

                //Popup penceresini ilk açılışta temizle
                popbaslamatarihi.Text = "";
                popbitistarihi.Text = "";
                pop_mesaj.Text = "";

                guncelle_id.Value = table.Rows[0]["YillikIzin_Id"].ToString();

                if (table.Rows[0]["YillikIzin_Baslama_Tarihi"].ToString() != DBNull.Value.ToString())
                {
                    popbaslamatarihi.Text = Convert.ToDateTime(table.Rows[0]["YillikIzin_Baslama_Tarihi"]).ToString("dd.MM.yyyy");
                }

                if (table.Rows[0]["YillikIzin_Bitis_Tarihi"].ToString() != DBNull.Value.ToString())
                {
                    popbitistarihi.Text = Convert.ToDateTime(table.Rows[0]["YillikIzin_Bitis_Tarihi"]).ToString("dd.MM.yyyy");
                }

                popgunsayisi.Text = table.Rows[0]["YillikIzin_Gun_Sayisi"].ToString();
                popnot.Text = table.Rows[0]["YillikIzin_Not"].ToString();
                DropDownList1.SelectedValue = table.Rows[0]["YillikIzin_Aciklama_Id"].ToString();
            }
        }
        catch (Exception)
        {
          ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "Personel bilgileri alınamadı.").ToString(), true);
        }
    }

    //Tarihleri eklerken bitiş tarihi zorunlu kontrol et.
    public string izinvarmi(string id)
    {
        string temp = "";
        SqlDataAdapter sanal_tablo = new SqlDataAdapter("Select YillikIzinAc_Bitis_Tarihi From tblYillikIzinAciklama where YillikIzinAc_Id='" + id + "'", connect);
        DataTable sanal_aciklama_tablo = new DataTable();
        sanal_tablo.Fill(sanal_aciklama_tablo);

        temp = sanal_aciklama_tablo.Rows[0]["YillikIzinAc_Bitis_Tarihi"].ToString();       
        return temp;
    }


    public string iki_tarih_arasi_kontrol(string t1, string t2, string neyapalim, string drop)
    {        
        string temp = "";
        
        SqlDataAdapter tarih_kontrol = new SqlDataAdapter("Select * From tblYillikIzin where YillikIzin_Persid='" + RouteData.Values["id"] + "'", connect);
        DataTable td = new DataTable();
        tarih_kontrol.Fill(td);
                

        for (int i = 0; i < td.Rows.Count; i++)
        {
            if (guncelle_id.Value != "" && td.Rows[i]["YillikIzin_Id"].ToString() != guncelle_id.Value || guncelle_id.Value == "")
            {              
                //Eğer Açılış Seçili İse ve Sistemde Varsa Eklenemez
                if (drop == "1" && drop == td.Rows[i]["YillikIzin_Aciklama_Id"].ToString())
                {
                    temp = "Sadece 1 Adet Açılış Eklenebilir.";
                    break;
                }
                //Eğer Açılış Değeri Sistemdeki Tarihlerden büyükse
                else if (drop == "1" && Convert.ToDateTime(t1) >= Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]))
                {
                    temp = "Açılış Tarihi En Küçük Tarih Olmalıdır.";
                    break;
                }
                //Eğer eklenen veri sistemdeki açılıştan eski ise
                else if (drop != "1" && td.Rows[i]["YillikIzin_Aciklama_Id"].ToString()=="1" && Convert.ToDateTime(t1) <= Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]))
                {
                    temp = "Açılış Tarihinden Önce Veri Eklenemez.";
                    break;
                }


                //Tek Tarih Kontrolü
                if (Convert.ToDateTime(t1).ToString("yyyy.MM.dd") == Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]).ToString("yyyy.MM.dd") && drop == td.Rows[i]["YillikIzin_Aciklama_Id"].ToString())
                {
                    temp = "Sistemde Başlangıç Tarihi ve Açıklaması Aynı Kayıt Bulundu.";
                    break;
                }
                else if (Convert.ToDateTime(t1).ToString("yyyy.MM.dd") == Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]).ToString("yyyy.MM.dd"))
                {
                    temp = "Sistemde Başlangıç Tarihi Aynı Kayıt Bulundu.";
                    break;
                }
                //else if (td.Rows[i]["YillikIzin_Bitis_Tarihi"]!= DBNull.Value && Convert.ToDateTime(t1).ToString("yyyy.MM.dd") == Convert.ToDateTime(td.Rows[i]["YillikIzin_Bitis_Tarihi"]).ToString("yyyy.MM.dd"))
                {
                    //    temp = "Sistemde Bitiş Tarihi Aynı Kayıt Bulundu.";
                    //    break;
                }



                //İki Tarih Kontrolü
                //Bitiş Tarihi Olmayanları Dahil Etme Sistem Arıza Vermesin.
                if (td.Rows[i]["YillikIzin_Bitis_Tarihi"].ToString() != "" && t2 != "")
                {
                    //Eğer Ba.Tarihi>=S.Ba.T ve B.Tarihi <= Si.Bi.Ta. (Arasındaysa)
                    if (Convert.ToDateTime(t1) >= Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]) && Convert.ToDateTime(t1) < Convert.ToDateTime(td.Rows[i]["YillikIzin_Bitis_Tarihi"]))
                    {
                        temp = "Tarih Çakışması.Lütfen Sistemdeki Eski Tarihleri Kontrol Ediniz.";
                        break;
                    }
                    //Eğer Ba.Tarihi>=Si.Ba.Ta. ve B.Tarihi<=Si.Bi.Ta. (Sağa Kaymışsa)
                    else if (Convert.ToDateTime(t2) >= Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]) && Convert.ToDateTime(t2) <= Convert.ToDateTime(td.Rows[i]["YillikIzin_Bitis_Tarihi"]))
                    {
                        temp = "Tarih Çakışması.Lütfen Sistemdeki Eski Tarihleri Kontrol Ediniz.";
                        break;
                    }
                    //Eğer Ba.Tarihi <= Si.Ba.Ta. ve Bi>=Si.Bi.Ta.
                    else if (Convert.ToDateTime(t1) <= Convert.ToDateTime(td.Rows[i]["YillikIzin_Baslama_Tarihi"]) && Convert.ToDateTime(t2) >= Convert.ToDateTime(td.Rows[i]["YillikIzin_Bitis_Tarihi"]))
                    {
                        temp = "Tarih Çakışması.Lütfen Sistemdeki Eski Tarihleri Kontrol Ediniz.";
                        break;
                    }                    
                }
            }                        
        }   
        return temp;
    }
}