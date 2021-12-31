using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Modul_Personel_Rol_Ekle : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string yetki_durum;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("YY_IK_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("YY_IK_Yazma") != "1")
        {
            yetki_durum = "0";
        }
        //Güvenlik///////////////////////


        

        //Ana Rolleri ve Yetkileri Yükle
        roller();
        yetkileri_Yukle();        
    }

    
    //Ana Rolleri Yükle
    public void roller()
    {
        connect.Open();

        SqlCommand komut = new SqlCommand("Select * from tblYetkiRol", connect);
        SqlDataReader rollertb;
        rollertb = komut.ExecuteReader();

        if (rollertb.HasRows)
        {
            //while (rollertb.Read())
            {
                Rep1.DataSource = rollertb;
                Rep1.DataBind();
            }
        }

        //Personel tablosundan personelin adını al ve ekrana yansıt.
        SqlCommand personel_bul = new SqlCommand("Select * from Personel where Persid='" + RouteData.Values["id"] + "'", connect);
        SqlDataReader reader = personel_bul.ExecuteReader();
        reader.Read();
        persadi_label.Text =reader["Pers_Adi"].ToString() + " " + reader["Pers_Soyadi"].ToString();
        reader.Close();
        connect.Close();
    }


    //Ana Rollerde Personel Hangi Rollere Bağlıysa Kontrol Et ve İşaretle
    string rol_id_topla;
    protected void Rep1_ItemDataBound(object sender, RepeaterItemEventArgs a)
    {
        try
        {
            if (a.Item.ItemType == ListItemType.Item || a.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Ana Roller On/OFF Butonlarını Kapsayan Panel
                Panel durum_panel = (Panel)a.Item.FindControl("durum_panel");

                //Ana Rollerde Yetki Durumuna Göre  Düzenlemek istendiğinde çıkarılan panel 2
                Panel durum_panel_2 = (Panel)a.Item.FindControl("durum_panel_2");

                //Ana Rollerde Yetki Durumuna Göre  Butonları Gizlendikten Sonra Çıkarılan Mesaj
                Label durum_mesaj = (Label)a.Item.FindControl("durum_mesaj");
                
                //Ana Rollerin Id ileri Alınıyor.
                HiddenField YR_ID = (HiddenField)a.Item.FindControl("YR_Id");

                //On Tuşları Bulunuyor
                HtmlInputCheckBox chb = (HtmlInputCheckBox)a.Item.FindControl("durumu2");

                //Kullanıcının Yazma Yetkisi varsa
                if (yetki_durum == "0")
                {
                    //On Tuşlarını Aç
                    durum_panel.Visible = false;
                    durum_panel_2.Visible = true;
                }
                else
                {
                    //On Tuşlarını Kapat
                    durum_panel.Visible = true;
                    durum_panel_2.Visible = false;
                }

                
                //Kullanıcıda Hangi Roller İşaretli Bulunuyor.
                SqlDataAdapter komut = new SqlDataAdapter("Select * from tblYetkiRolPersonel where YRP_Persid='" + RouteData.Values["id"] + "' and YRP_Rol_Id='" + YR_ID.Value + "'", connect);
                DataTable tablo = new DataTable();
                komut.Fill(tablo);

                //Eğer verilen komutlara göre kayıt varsa
                if (tablo.Rows.Count > 0)
                {
                    //on tuşlarını aktif et
                    chb.Checked = true;

                    //ve eğer tuşlar aktif olmasa açık yaz
                    durum_mesaj.Text = "Açık";

                    //Açık yazısının rengini yeşil yap
                    durum_mesaj.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    //Kayıt yoksa Kapalı Yaz
                    durum_mesaj.Text = "Kapalı";

                    //Rengini Kırmızı Yap
                    durum_mesaj.ForeColor = System.Drawing.Color.Red;
                }

                //Rol idleri bir değişkene topla
                rol_id_topla += "'"+tablo.Rows[0]["YRP_Rol_Id"].ToString()+"'"+",";               
            }
        }
        catch (Exception)
        {
        }
    }



    //Yetki Başlıklarını Kategorilerini Yükle
    public void yetkileri_Yukle()
    {
        connect.Open();
        //Tüm Yetkileri Ekrana Yaz.
        SqlCommand komut = new SqlCommand("Select YY_Rol_Yetki_Kategori from tblYetkiYetki Group BY YY_Rol_Yetki_Kategori", connect);
        SqlDataReader tablo;
        tablo = komut.ExecuteReader();
        
        if (tablo.HasRows)
        {
            Rep2.DataSource = tablo;
            Rep2.DataBind();
        }
        tablo.Close();
        connect.Close();
    }



    //Açılan Yetki Başlıklarına Ait Alt Yetkiler
    protected void Rep2_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //Alt yetilerin döngüsü için repeater bul
        Repeater Rep3 = (Repeater)e.Item.FindControl("Rep3");

        //Yetkilerin ana kategorisi başlıkları bul.
        HiddenField RY_Rol_Yetki_Kategori_id = (HiddenField)e.Item.FindControl("RY_Rol_Yetki_Kategori_id");

        //Alt Yetkileri bul ve ekrana yaz.
        SqlCommand komut = new SqlCommand("Select * from tblYetkiYetki where YY_Rol_Yetki_Kategori='" + RY_Rol_Yetki_Kategori_id.Value + "'", connect);
        SqlDataReader tablo;
        tablo = komut.ExecuteReader();
        
        if (tablo.HasRows)
        {
            Rep3.DataSource = tablo;
            Rep3.DataBind();
        }
    }

    protected void Rep3_ItemDataBound(object sender, RepeaterItemEventArgs a)
    {
        try
        {
            
            if (a.Item.ItemType == ListItemType.Item || a.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Sayfa ilk yüklendiğinde istatistik bunlarını yerleştirmel için gerekli kod
                Label Label1 = (Label)a.Item.FindControl("Label1");                

                //Alt yetkilerde on of butonları paneli
                Panel durum_panel3 = (Panel)a.Item.FindControl("durum_panel3");

                //Alt yetkilerde yetilere göre çıkarılan açık kapalı mesajları için panel
                Panel durum_panel4 = (Panel)a.Item.FindControl("durum_panel4");
                Label durum_mesaj4 = (Label)a.Item.FindControl("durum_mesaj4");

                //Ekrana yetkideki işaretli buton hangi rolden geliyor onu yazmak için
                Label gecerli_rol = (Label)a.Item.FindControl("gecerli_rol");

                //Eğer yazma yetkisi varsa
                if (yetki_durum == "0")
                {
                    durum_panel3.Visible = false;
                    durum_panel4.Visible = true;
                }
                else
                {
                    durum_panel3.Visible = true;
                    durum_panel4.Visible = false;
                }

                //Yetki id sini sistemden bul
                HiddenField yy_id = (HiddenField)a.Item.FindControl("yy_id");

                //off butonları için butonları bul
                HtmlInputCheckBox chb = (HtmlInputCheckBox)a.Item.FindControl("durumu3");


                //Önelikle seçili rollerdeki seçekleri tablo olarak getiriyor. Sonrada İstisnai tablosundaki verileri altına ekliyor
                SqlDataAdapter komut2 = new SqlDataAdapter("Select CONVERT(varchar, YR_Rol_Adi)[tt] from tblYetkiRolYetki wq " +
                                                            "left join tblYetkiRol yr on(wq.YRY_Rol_Id = yr.YR_Id) " +
                                                            "where wq.YRY_Rol_Id in (" + rol_id_topla.Substring(0, rol_id_topla.Length - 1) + ") and YRY_Yetki_Id = '" + yy_id.Value + "' " +
                                                            "union all select  CONVERT(varchar, YRPI_Yetki_Durumu)[tt] from " +
                                                            "tblYetkiRolPersonelIst tt where YRPI_YetkiId = '" + yy_id.Value + "' and YRPI_Persid = '" + RouteData.Values["id"] + "'", connect);

                DataTable tablo2 = new DataTable();
                komut2.Fill(tablo2);


                if (tablo2.Rows.Count > 0)
                {
                    //Eğer buluna rol fazla ise döngüye başla 
                    for (int i = 0; i < tablo2.Rows.Count; i++)
                    {
                        //Eğer gelen veri istisnati tablosundan gelmişse
                        if (tablo2.Rows[i]["tt"].ToString() == "0" || tablo2.Rows[i]["tt"].ToString() == "1")
                        {
                            //Geçerli Rolleri yazma
                            gecerli_rol.Text = "";

                           
                            Label1.Text = "<label class='onay'>"+
                                          "<input id='onaychec' type='checkbox' value='" + RouteData.Values["id"].ToString() + " - "+ yy_id.Value + "' onclick='yetkipersonelistsil(this); test("+yy_id.Value+ ")' class='onay-input' checked />" +
                                          "<span class='onay-label' data-on='İstisnai [ İptal ]' data-off='Yetki Silindi' title='Sisteme Giriş İzni'></span><span class='onay-handle'></span></label>";

                            //Eğer istisnai tablosunda 1 ise işaretle ve döngüden çık
                            if (tablo2.Rows[i]["tt"].ToString() == "1")
                            {
                                chb.Checked = true;
                            }
                            else
                            {
                                chb.Checked = false;
                            }
                            break;
                        }
                        else
                        {
                            //eğer istisnai tablosunda veri yoksa rollarin yetkilerini denetle
                            gecerli_rol.Text += "<span class='label label-success'>" + tablo2.Rows[i]["tt"].ToString() + "</span> ";

                            if (tablo2.Rows[i]["tt"].ToString().Length>0)
                            {
                                chb.Checked = true;
                            }
                            else
                            {
                                chb.Checked = false;
                            }
                            
                        }
                    }
                }
                else
                {
                    gecerli_rol.Text = "";
                }
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow",
            Sorgu.mesaj("y_hata", "Veya kullanıcıda aktif rol bulunamadı.").ToString(), true);
        }        
    }
}