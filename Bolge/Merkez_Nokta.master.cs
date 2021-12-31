using System;
using System.Web;

public partial class Merkez_Nokta : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string RequestPath = HttpContext.Current.Request.Url.ToString();
        //if (RequestPath.StartsWith("http://"))
        //{
        //    RequestPath = RequestPath.Replace("http://", "https://");
        //    HttpContext.Current.Response.Redirect(RequestPath, true);
        //}

        //DisableCache();
        if ((HttpContext.Current.Request.Cookies["RcEU"] == null))
        {
            Sorgu.genelyetkikurallari();
            //HttpResponse.RemoveOutputCacheItem("/Modul/Personel_Listesi.aspx");
            HttpContext.Current.Response.Redirect("/Giris");
        }        
    }

    private void DisableCache()
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache); //Cache-Control : no-cache, Pragma : no-cache
        Response.Cache.SetExpires(DateTime.Now.AddDays(-1)); //Expires : date time
        Response.Cache.SetNoStore(); //Cache-Control :  no-store
        Response.Cache.SetProxyMaxAge(new TimeSpan(0, 0, 0)); //Cache-Control: s-maxage=0
        Response.Cache.SetValidUntilExpires(false);
        Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);//Cache-Control:  must-revalidate
    }
}
