using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


public partial class Blok_Servis_Hatirlatma : System.Web.UI.UserControl
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        connect.Open();
        //Cari Bakımlar Haırlatma 
        SqlCommand komut = new SqlCommand("Select cb.CariBakimlar_Id, ca.CariSU_CariNo, ck.Cari_Unvan, ca.CariSU_Tanimi, cb.CariBakimlar_Tarih from tblCariBakimlar cb " +
                                          "left join tblCariAsansorler ca on(ca.CariSU_Id = cb.CariBakimlar_Asansor) "+
                                          "left join tblCariKayit ck on(ck.Cari_Id = ca.CariSU_CariNo) "+
                                          "WHERE  CariBakimlar_Tarih <= getdate()+7 and CariBakimlar_Durumu='6' order by CariBakimlar_Tarih", connect);
        SqlDataReader reader;

        reader = komut.ExecuteReader();
        bakim_hat_rep.DataSource = reader;
        bakim_hat_rep.DataBind();
        bakim_adet.Text = bakim_hat_rep.Items.Count.ToString();


        // CariRevizyonlar_Durumu in (2,3) and 
        komut = new SqlCommand("WITH Max_Tarih AS " +
                                "(SELECT RevEt.CRE_RevizyonId, MAX(RevEt.CRE_EtiketSonTarih)[Tarih] FROM tblCariRevizyonlarEtiket RevEt group by RevEt.CRE_RevizyonId) " +
                                "Select ck.Cari_Id, ck.Cari_Kodu, ck.Cari_Unvan, etk.CRE_RevizyonId, etk.CRE_Id, asn.CariSU_Tanimi, etk.CRE_Etiket, dn.Tarih, etk.CRE_BelgeTarihi from tblCariRevizyonlar Rev " +
                                "left join Max_Tarih dn on (dn.CRE_RevizyonId=Rev.CariRevizyonlar_Id) " +
                                "left join tblCariRevizyonlarEtiket etk on (etk.CRE_RevizyonId=dn.CRE_RevizyonId and etk.CRE_EtiketSonTarih=dn.Tarih) " +
                                "left join tblCariAsansorler asn on(asn.CariSU_Id = Rev.CariRevizyonlar_Asansor) " +
                                "left join tblCariKayit ck on(ck.Cari_Id = asn.CariSU_CariNo) " +
                                "where Tarih is not null and dn.Tarih <= getdate()+7 order by dn.Tarih", connect);
        SqlDataReader reader2;

        reader2 = komut.ExecuteReader();
        revizyon_hat_rep.DataSource = reader2;
        revizyon_hat_rep.DataBind();
        revizyon_adet.Text = revizyon_hat_rep.Items.Count.ToString();



        //
        komut = new SqlCommand("Select cb.CariAsansorAriza_Id, ca.CariSU_CariNo, ck.Cari_Unvan, ca.CariSU_Tanimi, cb.CariAsansorAriza_AsaId, cb.CariAsansorAriza_Tarih, CariAsansorAriza_Aciklama from tblCariAsansorAriza cb " +
                               "left join tblCariAsansorler ca on(ca.CariSU_Id = cb.CariAsansorAriza_AsaId) "+
                               "left join tblCariKayit ck on(ck.Cari_Id = ca.CariSU_CariNo) "+
                               "WHERE CariAsansorAriza_Tarih <= getdate()+7 and cb.CariAsansorAriza_Durumu = '1' order by CariAsansorAriza_Tarih", connect);
        SqlDataReader reader3;

        reader3 = komut.ExecuteReader();
        ariza_hat_rep.DataSource = reader3;
        ariza_hat_rep.DataBind();
        Ariza_Adet.Text = ariza_hat_rep.Items.Count.ToString();



        //
        komut = new SqlCommand("Select CariSozlesmeler_Id, cs.CariSozlesmeler_CariId, ck.Cari_Unvan, cs.CariSozlesmeler_Baslik, cs.CariSozlesmeler_SonTarih, CariSozlesmeler_Bedel from tblCariSozlesmeler cs " +
                               "left join tblCariKayit ck on(ck.Cari_Id = cs.CariSozlesmeler_CariId) "+
                               "WHERE cs.CariSozlesmeler_SonTarih <= getdate() + 7 and cs.CariSozlesmeler_Durumu = '1' order by cs.CariSozlesmeler_SonTarih", connect);
        SqlDataReader reader4;

        reader4 = komut.ExecuteReader();
        sozlesme_hat_rep.DataSource = reader4;
        sozlesme_hat_rep.DataBind();
        sozlesme_adet.Text = sozlesme_hat_rep.Items.Count.ToString();
        connect.Close();
    }
}