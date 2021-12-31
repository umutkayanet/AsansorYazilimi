<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sol_Menu.ascx.cs" Inherits="Blok_Sol_Menu" %>
<script>
    function startTime() {
        var today = new Date();
        var h = today.getHours();        
        var m = today.getMinutes();       
        var s = today.getSeconds();
        m = checkTime(m);
        s = checkTime(s);
        document.getElementById('txt').innerHTML =
        h + ":" + m + ":" + s;
        var t = setTimeout(startTime, 500);
    }
    function checkTime(i) {
        if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
        return i;
    }
</script>
<!-- Son Performans Alanı -->
<div class="sidebar collapse">
    <div class="sidebar-content">
        <!-- Kullanıcı Profil Menüsü -->
        <div class="saat_tarih">
            <div style="float: left;">
                <div id="txt"></div>
            </div>

            <div style="float: right" class="saat">
                <%=DateTime.Now.ToString("dd MMMM dddd")%>
            </div>
        </div>
    </div>
    <!-- Son Kullanıcı Profil Menüsü -->

    <!-- Sol Açılır Menü -->
    <ul class="navigation">
        <!--  class="active" -->
        <li <%=Request.Url.Segments[1].ToString().IndexOf("Anasayfa")!=-1  ? "class='active'": "" %>>
            <asp:LinkButton ID="Ana_Sayfa" runat="server" OnClick="Ana_Sayfa_Click"><span>Ana Sayfa</span> <i class="icon-screen2"></i></asp:LinkButton>
        </li>

        <li id="ik_menu" runat="server">
            <a href="#" class="expand" id='<%=Request.Url.Segments[1].ToString().IndexOf("Personel-Listesi")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Personel-Ekle")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Personel-Islem")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Personel-Izin")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Personel-Giris")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Personel-Resim")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Personel-Yetki")!=-1 ? "second-level": "" %>'>
                <span>İnsan Kaynakları</span>
                <i class="icon-users"></i>
            </a>

            <ul>
                <li runat="server" id="personeli_listesi_buton" class='<%=Request.Url.Segments[1].ToString().IndexOf("Personel-Listesi")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="Personel_Listesi" PostBackUrl="/Personel-Listesi" runat="server">Personel Listesi</asp:LinkButton>
                </li>
            </ul>
        </li>


        <li id="cari_menu" runat="server">
            <a href="#" class="expand" id='<%=Request.Url.Segments[1].ToString().IndexOf("Cari")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Bakim-Ekle")!=-1 || Request.Url.Segments[1].ToString().IndexOf("Revizyon")!=-1 ? "second-level": "" %>'>
                <span>Cari İşlemler</span>
                <i class="icon-tab"></i>
            </a>

            <ul>
                <li runat="server" id="Li2" class='<%=Request.Url.Segments[1].ToString().IndexOf("Cari-SatisListesi")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton1" PostBackUrl="/Cari-Listesi/Satis" runat="server">Satış Listesi</asp:LinkButton>
                </li>

                 <li runat="server" id="Li1" class='<%=Request.Url.Segments[1].ToString().IndexOf("Cari-AlisListesi")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton2" PostBackUrl="/Cari-Listesi/Alis" runat="server">Alış Listesi</asp:LinkButton>
                </li>
            </ul>
        </li>



        <li id="Stok_Menu" runat="server">
            <a href="#" class="expand" id='<%=Request.Url.Segments[1].ToString().IndexOf("Stok")!=-1 ? "second-level": "" %>'>
                <span>Stok İşlemleri</span>
                <i class="icon-table"></i>
            </a>

            <ul>
                <li runat="server" id="Li4" class='<%=Request.Url.Segments[1].ToString().IndexOf("Stok-Listesi")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton3" PostBackUrl="/Stok-Listesi" runat="server">Stok Listesi</asp:LinkButton>
                </li>

                 <li runat="server" id="Li3" class='<%=Request.Url.Segments[1].ToString().IndexOf("Stok-Atama")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton6" PostBackUrl="/Stok-Atama" runat="server">Stok Atama</asp:LinkButton>
                </li>
            </ul>
        </li>


        <li id="Marka_Menu" runat="server">
            <a href="#" class="expand" id='<%=Request.Url.Segments[1].ToString().IndexOf("Marka")!=-1 ? "second-level": "" %>'>
                <span>Marka İşlemleri</span>
                <i class="icon-tag4"></i>
            </a>

            <ul>
                <li runat="server" id="Li7" class='<%=Request.Url.Segments[1].ToString().IndexOf("Marka-Listesi")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton7" PostBackUrl="/Marka-Listesi" runat="server">Marka Listesi</asp:LinkButton>
                </li>
            </ul>
        </li>


        <li id="Servis_Menu" runat="server">
            <a href="#" class="expand" id='<%=Request.Url.Segments[1].ToString().IndexOf("Servis")!=-1 ? "second-level": "" %>'>
                <span>Servis İşlemleri</span>
                <i class="icon-office"></i>
            </a>

            <ul>
                <li runat="server" id="Li5" class='<%=Request.Url.Segments[1].ToString().IndexOf("Stok-Listesi")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton5" PostBackUrl="/Servis-Listesi" runat="server">Servis Listesi</asp:LinkButton>
                </li>
            </ul>
        </li>

        <li <%=Request.Url.Segments[1].ToString().IndexOf("Roller")!=-1  ? "class='active'": "" %>>
            <asp:LinkButton ID="LinkButton4" PostBackUrl="/Roller" runat="server"><span>Yetkiler</span><i class="icon-unlocked2"></i></asp:LinkButton>
        </li>


        <li id="Ayarlar_Menu" runat="server">
            <a href="#" class="expand" id='<%=Request.Url.Segments[1].ToString().IndexOf("Ayarlar")!=-1 ? "second-level": "" %>'>
                <span>Ayarlar</span>
                <i class="icon-settings"></i>
            </a>

            <ul>
                <li runat="server" id="Li9" class='<%=Request.Url.Segments[1].ToString().IndexOf("Bakim-Secenekleri")!=-1  ? "active": "" %>'>
                    <asp:LinkButton ID="LinkButton8" PostBackUrl="/Ayarlar/Bakim-Secenekleri" runat="server">Bakım Seçenekleri</asp:LinkButton>
                </li>
            </ul>
        </li>
    </ul>
    <!-- Son Sol Açılır Menü -->
</div>

