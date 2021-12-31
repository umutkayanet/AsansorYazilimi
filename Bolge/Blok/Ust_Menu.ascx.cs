using System;

public partial class Blok_Ust_Menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void cikis_Click(object sender, EventArgs e)
    {
        Sorgu.cookiesil();
        Session.Abandon();
        Response.Redirect("/Giris");
    }
}