using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Modul_Personel_Profil : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("Pers_Profil_Yazma") != "1")
        {
            kayit.Visible = false;
            g_panel.Visible = false;
        }
        //Güvenlik///////////////////////

        SqlDataAdapter komut = new SqlDataAdapter("Select Pers_Adi, Pers_Soyadi, Pers_TC, Pers_Dogum_Tarihi, Email, password, Personel_Egitim, Gorev, Foto, Mezuniyet_yil "+
                                                  "from personel ps "+
                                                  "Left Join tblPersonelGorev gr "+
                                                  "on(gr.Gorev_id = ps.Gorev_id) "+
                                                  "Left Join tblPersonelEgitim pe "+
                                                  "on(pe.Personel_EgitimId = ps.Pers_EgitimDurumu)" +
                                                  "where Email='" + Sorgu.Decrypt(Request.Cookies["RcEU"]["QiZrNv"].ToString()) + "'", connect);
        DataTable tablo = new DataTable();
        komut.Fill(tablo);

        if (tablo.Rows.Count>0)
        {
            //Personel Bilgilerini Ekrana Getir.
            tc_texbox.Text = tablo.Rows[0]["Pers_Tc"].ToString();
            adi_textbox.Text = tablo.Rows[0]["Pers_Adi"].ToString();
            soyadi_textbox.Text = tablo.Rows[0]["Pers_Soyadi"].ToString();
            dogumtarihi_textbox.Text = Convert.ToDateTime(tablo.Rows[0]["Pers_Dogum_Tarihi"]).ToString("dd.MM.yyyy");
            kullaniciadi_textbox.Text = tablo.Rows[0]["Email"].ToString();            
            mezuniyetyili_textbox.Text = tablo.Rows[0]["Mezuniyet_yil"].ToString();
            egitim_textbox.Text = tablo.Rows[0]["Personel_Egitim"].ToString();
            gorev_drop.Text = tablo.Rows[0]["Gorev"].ToString();
        }
    }

    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {   
            if (sifresi_textbox.Text != "" || sifresi_textbox.Text != string.Empty)
            {
                connect.Open();
                SqlCommand komut = new SqlCommand("update Personel set password="+sifresi_textbox.Text+" where Email='" + Sorgu.Decrypt(Request.Cookies["RcEU"]["QiZrNv"].ToString()) + "'", connect);
                komut.ExecuteNonQuery();
                komut.Dispose();
                connect.Close();
            }
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_basari", "").ToString(), true);
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", Sorgu.mesaj("y_hata", "").ToString(), true);
        }
    }
}