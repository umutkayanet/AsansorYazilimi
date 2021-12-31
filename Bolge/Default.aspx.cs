using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((HttpContext.Current.Request.Cookies["RcEU"] == null))
        {
            HttpContext.Current.Response.Redirect("/Giris");
        }
    }

    [WebMethod]
    public static List<object> GetChartData()
    {
        string query = "SET LANGUAGE 'Turkish' Select COUNT(CariAsansorAriza_KayitTarih)[Toplam], "+
                       "DATENAME(month, CariAsansorAriza_KayitTarih) AS 'Ay' "+
                       "From tblCariAsansorAriza where Year(CariAsansorAriza_KayitTarih)= YEAR(GETDATE()) group by DATENAME(month, CariAsansorAriza_KayitTarih)";
       
        string constr = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
        List<object> chartData = new List<object>();
        chartData.Add(new object[]
        {
        "ShipCity", "Toplam : "
        });
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        chartData.Add(new object[]
                        {
                           sdr["Ay"], sdr["Toplam"]
                        });
                    }
                }
                con.Close();
                return chartData;
            }
        }
    }
}


