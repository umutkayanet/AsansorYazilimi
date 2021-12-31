using System;
using System.Web.UI;

public partial class Blok_Haberler : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void yenile_Click(object sender, EventArgs e)
    {
        Panel1.DataBind();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow",
        Sorgu.mesaj("y_basari", "").ToString(), true);
    }
}