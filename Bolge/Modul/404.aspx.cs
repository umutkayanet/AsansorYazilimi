using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Modul_404 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Page.RouteData.Values["id"].ToString());
        //<asp:HyperLink ID = "Label5" runat = "server" NavigateUrl = '<%# String.Format("~/Tarifler/{0}/{1}",KodOlustur(Eval("tarifadi").ToString()),Eval("tarifid").ToString()) %>' ><%# Eval("tarifadi") %></asp:HyperLink>
    }
}