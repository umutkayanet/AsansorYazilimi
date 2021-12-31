using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;

public partial class Modul_Personel_Kayit_Resim : System.Web.UI.Page
{
    SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
    string sayi = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();

    string kullanici_adi = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Güvenlik///////////////////////
        if (Sorgu.yetkiteklikontrol("YY_IK_Okuma") != "1")
        {
            Response.Redirect("/Anasayfa");
        }

        if (Sorgu.yetkiteklikontrol("YY_IK_Yazma") != "1")
        {
            resmi_sil.Visible = false;
            FileUpload1.Enabled = false;
            g_panel.Visible = false;
        }
        //Güvenlik///////////////////////
        verileri_yukle();
    }

    public void verileri_yukle()
    {
        //Resim Yüklenecek Personeli Bul.
        SqlDataAdapter komut = new SqlDataAdapter("Select * from Personel where Persid='" + RouteData.Values["id"] + "'", connect);
        DataTable tablo = new DataTable();
        komut.Fill(tablo);

        //Eğer Personel Bulunduysa
        if (tablo.Rows.Count > 0)
        {
            persadi_label.Text = tablo.Rows[0]["Pers_adi"].ToString() + " " + tablo.Rows[0]["Pers_Soyadi"].ToString();

            //Personelin ismini resme yazmak için verileri al ve sicil no yu devamına ekle
            kullanici_adi = Sorgu.dosya_adi_yarat(tablo.Rows[0]["Pers_adi"].ToString() + "_" + tablo.Rows[0]["Pers_Soyadi"].ToString());

            //Eğer Resmi Sistemde Varsa Yükle
            personel_resim.ImageUrl = Sorgu.GetImageUrl("/Data/personel_resim/kucuk_resim/" + tablo.Rows[0]["Foto"] + "");
        }
        else
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("uyari", "Personele resim yükleyebilmeniz için öncelikle sisteme kayıt işlemini tamamlamalısınız.");
        }
    }



    SqlCommand komut;
    protected void kayit_Click(object sender, EventArgs e)
    {
        try
        {
            //File Upload dosyasını kontrol et
            if (FileUpload1.HasFile)
            {
                //Yüklenecek dosyanın uzantısını al
                string dosyauzantisi = Path.GetExtension(FileUpload1.PostedFile.FileName);

                //Eğer yüklenecek dosya uzantısında jpg veya png yoksa işlemi iptal et
                if (dosyauzantisi.IndexOf(".jpg") == 0 || dosyauzantisi.IndexOf(".png") == 0)
                {
                    //Eğer Daha öncesinden resim varsa önce eski resmi sil
                    SqlDataAdapter resim_bul = new SqlDataAdapter("Select * from Personel where Persid='" + RouteData.Values["id"] + "'", connect);
                    DataTable db = new DataTable();
                    resim_bul.Fill(db);

                    //Orjinal Resmi Sil
                    string resim1 = db.Rows[0]["Foto"].ToString();// Hafızada olan resim1 al
                    string resim1yol = Server.MapPath("/Data/personel_resim/kucuk_resim/") + "\\" + resim1;// Urunler dosyasına git Resim1 deki kaydı bul
                    if (System.IO.File.Exists(resim1yol))
                    {
                        System.IO.File.Delete(resim1yol);// Ve sil
                    }


                    //Küçük Resmi Sil
                    string resim2 = db.Rows[0]["Foto"].ToString();// Hafızada olan resim1 al
                    string resim2yol = Server.MapPath("/Data/personel_resim/") + "\\" + resim1;// Urunler dosyasına git Resim1 deki kaydı bul
                    if (System.IO.File.Exists(resim2yol))
                    {
                        System.IO.File.Delete(resim2yol);// Ve sil
                    }


                    //dosya uzantısına personelin adını ekle
                    string dosyauzantisi2 = kullanici_adi + Path.GetExtension(FileUpload1.PostedFile.FileName);

                    //Resmin orjinalini yükle
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("/Data/personel_resim/" + dosyauzantisi2));

                    //resmi orantıla ve küçük resmini sisteme koy
                    Bitmap resim = new Bitmap(Server.MapPath("/Data/personel_resim/" + dosyauzantisi2));// Resmi küçült
                    resim = Sorgu.Oranla(resim, 165);
                    resim.Save(Server.MapPath("/Data/personel_resim/kucuk_resim/" + dosyauzantisi2));

                    connect.Open();
                    //Resmin adını sisteme yaz
                    komut = new SqlCommand("update personel set Foto=@Foto where Persid='" + RouteData.Values["id"] + "'", connect);
                    komut.Parameters.AddWithValue("@Foto", dosyauzantisi2);
                    komut.ExecuteNonQuery();
                    connect.Close();
                    ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("basari", "");
                    verileri_yukle();
                }
                else
                {
                    ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("uyari", "Sisteme .jpg ve.png uzantılı resimler ekleyebilirsiniz.");
                }
            }
            else
            {
                ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("uyari", "Resim yükleme alanı boş olamaz.");
            }
        }
        catch (Exception)
        {
            ((Label)((MasterPage)Page.Master).FindControl("mesaj")).Text = Sorgu.mesaj("hata", "");
        }
    }

    protected void resmi_sil_Click(object sender, EventArgs e)
    {
        connect.Open();
        SqlDataAdapter resim_bul = new SqlDataAdapter("Select * from Personel where Persid='" + RouteData.Values["id"] + "'", connect);
        DataTable db = new DataTable();
        resim_bul.Fill(db);

        //Orjinal Resmi Sil
        string resim1 = db.Rows[0]["Foto"].ToString();// Hafızada olan resim1 al
        string resim1yol = Server.MapPath("/Data/personel_resim/kucuk_resim/") + "\\" + resim1;// Urunler dosyasına git Resim1 deki kaydı bul
        if (System.IO.File.Exists(resim1yol))
        {
            System.IO.File.Delete(resim1yol);// Ve sil
        }

        //Küçük Resmi Sil
        string resim2 = db.Rows[0]["Foto"].ToString();// Hafızada olan resim1 al
        string resim2yol = Server.MapPath("/Data/personel_resim/") + "\\" + resim1;// Urunler dosyasına git Resim1 deki kaydı bul
        if (System.IO.File.Exists(resim2yol))
        {
            System.IO.File.Delete(resim2yol);// Ve sil
        }

        komut = new SqlCommand("update personel set Foto=@Foto where Persid='" + RouteData.Values["id"] + "'", connect);
        komut.Parameters.AddWithValue("@Foto", "");
        komut.ExecuteNonQuery();

        verileri_yukle();
        connect.Close();
    }
}