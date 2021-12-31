<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

    public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        //Giriş
        routes.MapPageRoute("Giris", "Giris/", "~/Modul/Giris.aspx");
        

        // Anasayfa kelimesi ile tek nokta dönüş sağlama
        routes.MapPageRoute("Anasayfa", "Anasayfa/", "~/Default.aspx");
        routes.MapPageRoute("404", "404/{id}/", "~/Modul/404.aspx");

        //Cari Satış
        routes.MapPageRoute("CariListesi", "Cari-Listesi/{gorev}", "~/Modul/Cari_Listesi.aspx");
        routes.MapPageRoute("CariKayit", "Cari-Kayit/{gorev}", "~/Modul/Cari_Kayit.aspx");
        routes.MapPageRoute("CariKayit2", "Cari-Kayit/{gorev}/{id}", "~/Modul/Cari_Kayit.aspx");
        routes.MapPageRoute("CariKayit3", "Cari-Kayit/{gorev}/{m}/{id}", "~/Modul/Cari_Kayit.aspx");

        //İnsan Kaynakları
        routes.MapPageRoute("Personelkayit", "Personel-Ekle", "~/Modul/Personel_Kayit.aspx");
        routes.MapPageRoute("Personelkayit_g", "Personel-Islem/Guncelle/{id}", "~/Modul/Personel_Kayit.aspx");
        routes.MapPageRoute("Personelkayit_g2", "Personel-Islem/Guncelle/{m}/{id}", "~/Modul/Personel_Kayit.aspx");
        routes.MapPageRoute("PersonelListesi", "Personel-Listesi", "~/Modul/Personel_Listesi.aspx");
        routes.MapPageRoute("PersonelResim", "Personel-Resim/{id}", "~/Modul/Personel_Kayit_Resim.aspx");
        routes.MapPageRoute("PersonelGiris", "Personel-Giris/{id}", "~/Modul/Personel_Ise_Giris_Cikis.aspx");
        routes.MapPageRoute("PersonelGiris2", "Personel-Giris/Guncelle/{id}/{veriid}", "~/Modul/Personel_Ise_Giris_Cikis.aspx");
        routes.MapPageRoute("PersonelYillikİzin", "Personel-Izin/{id}", "~/Modul/Personel_Yillik_Izin.aspx");
        routes.MapPageRoute("PersonelYetki", "Personel-Yetki/{id}", "~/Modul/Personel_Rol_Ekle.aspx");
        routes.MapPageRoute("PersonelYetki2", "Personel-Yetki/Guncelle/{id}/{veriid}", "~/Modul/Personel_Rol_Ekle.aspx");

        //Servis Listesi
        routes.MapPageRoute("Servis", "Servis-Listesi", "~/Modul/Servis_Listesi.aspx");

        //Servis Asansör Listesi
        routes.MapPageRoute("AsansorEkle", "Servis/Asansor-Ekle/{id}", "~/Modul/Asansor_Ekle.aspx");
        routes.MapPageRoute("AsansorEkleG", "Servis/Asansor-Ekle/{id}/{idd}", "~/Modul/Asansor_Ekle.aspx");
        
        //Servis Bakım Listesi
        routes.MapPageRoute("BakimEkle", "Servis/Bakim-Ekle/{id}", "~/Modul/Bakim_Ekle.aspx");
        routes.MapPageRoute("BakimEkleG", "Servis/Bakim-Ekle/{id}/{idd}", "~/Modul/Bakim_Ekle.aspx");
        
        //Servis Revizyon Listesi
        routes.MapPageRoute("RevizyonEkle", "Servis/Revizyon-Ekle/{id}", "~/Modul/Revizyon_Ekle.aspx");
        routes.MapPageRoute("RevizyonEkleG", "Servis/Revizyon-Ekle/{id}/{idd}", "~/Modul/Revizyon_Ekle.aspx");
        routes.MapPageRoute("RevizyonRenkEkle", "Servis/Revizyon-RenkEkle/{id}/{idd}", "~/Modul/Revizyon_RenkEkle.aspx");
        routes.MapPageRoute("RevizyonRenkEkleG", "Servis/Revizyon-RenkEkle/{id}/{idd}/{iddd}", "~/Modul/Revizyon_RenkEkle.aspx");
        routes.MapPageRoute("RevizyonEkleParça", "Servis/Revizyon-ParcaEkle/{id}/{idd}", "~/Modul/Revizyon_ParcaEkle.aspx");
        
        //Servis Arıza Listesi
        routes.MapPageRoute("ArızaEkle", "Servis/Ariza-Listesi/{id}", "~/Modul/Ariza_Ekle.aspx");
        routes.MapPageRoute("ArızaEkleG", "Servis/Ariza-Listesi/{id}/{idd}", "~/Modul/Ariza_Ekle.aspx");
        routes.MapPageRoute("ArizaEkleParca", "Servis/Ariza-ParcaEkle/{id}/{idd}", "~/Modul/Ariza_ParcaEkle.aspx");

        //Profil
        routes.MapPageRoute("Profil", "Profil/", "~/Modul/Personel_Profil.aspx");

        //Roller
        routes.MapPageRoute("Roller", "Roller/", "~/Modul/Roller.aspx");
        routes.MapPageRoute("RollerGuncelle", "Roller/Guncelle/{id}", "~/Modul/Roller.aspx");
        routes.MapPageRoute("RollerYetki", "Roller/Yetki/{id}", "~/Modul/RolYetkileri.aspx");


        //Stok Listesi
        routes.MapPageRoute("StokListesi", "Stok-Listesi", "~/Modul/Stok_Listesi.aspx");
        routes.MapPageRoute("StokListesiG", "Stok-Listesi/{id}", "~/Modul/Stok_Listesi.aspx");
        routes.MapPageRoute("StokListesiAt", "Stok-Atama/", "~/Modul/Stok_Atama.aspx");
        routes.MapPageRoute("StokListesiAtG", "Stok-Atama/{idd}", "~/Modul/Stok_Atama.aspx");

        //Marka Listesi
        routes.MapPageRoute("MarkaListesi", "Marka-Listesi", "~/Modul/Marka_Listesi.aspx");
        routes.MapPageRoute("MarkaListesiG", "Marka-Listesi/{id}", "~/Modul/Marka_Listesi.aspx");


        //Ayarlar
        routes.MapPageRoute("BakimSec", "Ayarlar/Bakim-Secenekleri", "~/Modul/Ayarlar_Bakim_Sec.aspx");
        routes.MapPageRoute("BakimSecG", "Ayarlar/Bakim-Secenekleri/{id}", "~/Modul/Ayarlar_Bakim_Sec.aspx");

        //Cari Sözleşmeler
        routes.MapPageRoute("Sozlesmeler", "Servis/Cari-Sozlesmeler/{id}", "~/Modul/Cari_Sozlesmeler.aspx");
        routes.MapPageRoute("SozlesmelerG", "Servis/Cari-Sozlesmeler/{id}/{idd}", "~/Modul/Cari_Sozlesmeler.aspx");
    }
</script>
