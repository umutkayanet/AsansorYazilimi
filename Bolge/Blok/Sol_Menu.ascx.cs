using System;
using System.Data.SqlClient;
using System.Configuration;

public partial class Blok_Sol_Menu : System.Web.UI.UserControl
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("YY_IK_Okuma") == "1" || Sorgu.yetkiteklikontrol("YY_IK_Cv_Okuma") == "1")
        {
            ik_menu.Attributes.Add("Style", "");
        }
        else
        {
            ik_menu.Attributes.Add("Style", "Display:none");
        }



        if (Sorgu.yetkiteklikontrol("YY_IK_Okuma") == "1")
        {
            personeli_listesi_buton.Visible = true;
        }
        else
        {
            personeli_listesi_buton.Visible = false;
        }


        if (Sorgu.yetkiteklikontrol("Cari_Bilgileri_Okuma") == "1")
        {
            cari_menu.Visible = true;
        }
        else
        {
            cari_menu.Visible = false;
        }


        if (Sorgu.yetkiteklikontrol("Stok_Listesi_Okuma") == "1" || Sorgu.yetkiteklikontrol("Stok_Atama_Okuma") == "1")
        {
            Stok_Menu.Visible = true;


            if (Sorgu.yetkiteklikontrol("Stok_Listesi_Okuma") != "1")
            {
                Li4.Visible = false;
            }

            if (Sorgu.yetkiteklikontrol("Stok_Atama_Okuma") != "1")
            {
                Li3.Visible = false;
            }
        }
        else
        {
            Stok_Menu.Visible = false;
        }


        if (Sorgu.yetkiteklikontrol("Marka_Listesi_Okuma") == "1")
        {
            Marka_Menu.Attributes.Add("Style", "");
        }
        else
        {
            Marka_Menu.Attributes.Add("Style", "Display:none");
        }


        if (Sorgu.yetkiteklikontrol("Servis_Listesi_Okuma") == "1")
        {
            Servis_Menu.Attributes.Add("Style", "");
        }
        else
        {
            Servis_Menu.Attributes.Add("Style", "Display:none");
        }


        if (Sorgu.yetkiteklikontrol("Y_Yetkiler_Okuma") == "1" || Sorgu.yetkiteklikontrol("Y_Yetkiler_Yazma") == "1")
        {
            LinkButton4.Attributes.Add("Style", "");
        }
        else
        {
            LinkButton4.Attributes.Add("Style", "Display:none");
        }


        if (Sorgu.yetkiteklikontrol("Ayarlar_Okuma") == "1")
        {
            Ayarlar_Menu.Attributes.Add("Style", "");
        }
        else
        {
            Ayarlar_Menu.Attributes.Add("Style", "Display:none");
        }
        //Güvenlik///////////////////////        
    }

    protected void Ana_Sayfa_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Anasayfa");
    }
}