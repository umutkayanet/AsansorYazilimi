using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Modul_RolYetkileri : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string yetki_durum;
    string yetki_rol;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("Y_Yetkiler_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        //İnsan Kaynakları Okuma
        if (Sorgu.yetkiteklikontrol("Y_Yetkiler_Yazma") != "1")
        {
            yetki_durum = "0";
        }
        //Güvenlik///////////////////////
        
        //Ana Rol Bilgileri
        bilgi_getir();

        //Açılan Role Ait Yetki Kategori Başlıkları
        yetkileri_Yukle();
    }


    //Ana Rol bilgileri
    public void bilgi_getir()
    {
        if (!IsPostBack)
        {
            connect.Open();
            SqlCommand komut = new SqlCommand("Select * from tblYetkiRol where YR_Id='" + RouteData.Values["id"] + "'", connect);
            SqlDataReader tablo;
            tablo = komut.ExecuteReader();
            
            if (tablo.HasRows)
            {
                {//Aynı yetkiye bağlı kişiler kendi yetilerinde değişiklik yapamaz.
                    SqlDataAdapter komut2 = new SqlDataAdapter("Select * from tblYetkiRolPersonel where YRP_Persid='" + Sorgu.Decrypt(Request.Cookies["RcEU"]["TyPsqX"].ToString()) + "' and YRP_Rol_Id='" + RouteData.Values["id"] + "'", connect);
                    DataTable personel = new DataTable();
                    komut2.Fill(personel);

                    if (personel.Rows.Count > 0)
                    {
                        //Eğer düzenlemek istediği yetki kendi yetkisi ise
                        yetki_rol = "1";
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_uyari", "Kendi yetkinizde değişiklik yapamazsınız.").ToString(), true);
                    }
                }

                tablo.Read();
                rol_baslik1.Text = tablo["YR_Rol_Adi"].ToString();
                rol_baslik2.Text = tablo["YR_Rol_Adi"].ToString();
                rol_baslik3.Text = tablo["YR_Rol_Adi"].ToString();
            }
            tablo.Close();
            connect.Close();
        }
    }


    //Yetki Kategorilerini Yükle
    public void yetkileri_Yukle()
    {
        if (!IsPostBack)
        {
            connect.Open();
            //Tüm Yetkileri Ekrana Yaz.
            SqlCommand komut = new SqlCommand("Select YY_Rol_Yetki_Kategori from tblYetkiYetki Group BY YY_Rol_Yetki_Kategori", connect);
            SqlDataReader tablo;
            tablo = komut.ExecuteReader();

            if (tablo.HasRows)
            {
                Rep1.DataSource = tablo;
                Rep1.DataBind();
            }
            tablo.Close();
            connect.Close();
        }
    }

    
    //Açılan Role Ait Alt Yetkiler
    protected void Rep1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater Rep2 = (Repeater)e.Item.FindControl("Rep2");
        HiddenField RY_Rol_Yetki_Kategori_id = (HiddenField)e.Item.FindControl("RY_Rol_Yetki_Kategori_id");


        SqlCommand komut = new SqlCommand("Select * from tblYetkiYetki where YY_Rol_Yetki_Kategori='" + RY_Rol_Yetki_Kategori_id.Value + "'", connect);
        SqlDataReader tablo = komut.ExecuteReader();


        if (tablo.HasRows)
        {
            Rep2.DataSource = tablo;
            Rep2.DataBind();
        }
    }

    protected void Rep2_ItemDataBound(object sender, RepeaterItemEventArgs a)
    {
        if (a.Item.ItemType == ListItemType.Item || a.Item.ItemType == ListItemType.AlternatingItem)
        {
            Panel durum_panel = (Panel)a.Item.FindControl("durum_panel");
            Panel durum_panel_2 = (Panel)a.Item.FindControl("durum_panel_2");
            Label durum_mesaj = (Label)a.Item.FindControl("durum_mesaj");
            

            //Eğer düzenlemek istediği yetki kendi yetkisi ise
            if (yetki_rol == "1")
            {
                durum_panel.Visible = false;
                durum_panel_2.Visible = true;
            }
            else
            {
                //Eğer yazma yetkis varsa
                if (yetki_durum == "0")
                {
                    durum_panel.Visible = false;
                    durum_panel_2.Visible = true;
                }
                else
                {
                    durum_panel.Visible = true;
                    durum_panel_2.Visible = false;
                }
            }

            HiddenField yy_id = (HiddenField)a.Item.FindControl("yy_id");
            HtmlInputCheckBox chb = (HtmlInputCheckBox)a.Item.FindControl("durumu2");

            SqlCommand komut = new SqlCommand("Select * from tblYetkiRolYetki where YRY_Rol_Id='" + RouteData.Values["id"] + "' and YRY_Yetki_Id='" + yy_id.Value + "'", connect);
            SqlDataReader tablo = komut.ExecuteReader();

            if (yetki_rol == "1")
            {
                if (tablo.HasRows)
                {
                    chb.Checked = true;
                    durum_mesaj.Text = "Açık";
                    durum_mesaj.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    durum_mesaj.Text = "Kapalı";
                    durum_mesaj.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                if (tablo.HasRows)
                {
                    chb.Checked = true;
                    durum_mesaj.Text = "Açık";
                    durum_mesaj.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    durum_mesaj.Text = "Kapalı";
                    durum_mesaj.ForeColor = System.Drawing.Color.Red;
                }
            }
        }       
    }
}