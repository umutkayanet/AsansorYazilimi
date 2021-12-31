<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Personel_Profil.aspx.cs" Inherits="Modul_Personel_Profil" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-header">
                <div class="page-title">
                    <h3>Profil<small>Profil bilgilerinizi aşağıda görebilir ve düzenleyebilirsiniz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Profil">Profilim</a></li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->


            <div class="row">
                <div class="col-lg-2">
                    <!-- Sol Blok Bilgileri-->
                    <div class="block">
                        <div class="block">
                            <div class="thumbnail">
                                <div class="thumb">
                                    <div style="margin-bottom: -15px;">
                                        <img src="<%=Sorgu.GetImageUrl("/Data/personel_resim/kucuk_resim/" + Request.Cookies["RcEU"]["Foto"] + "")%> " alt="" />
                                    </div>
                                </div>
                                <div class="caption text-center">
                                    <h6><%=Server.UrlDecode(Request.Cookies["RcEU"]["Kullanici_Adi"].ToString())%> <small><%=Server.UrlDecode(Request.Cookies["RcEU"]["Bolum"])%></small></h6>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <!-- Sağ Blok Bilgileri -->
                <div class="col-lg-10">
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Adınız Soyadınız</label>
                                    <asp:TextBox ID="adi_textbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label>Soyadınız</label>
                                    <asp:TextBox ID="soyadi_textbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Kimlik Numaranız</label>
                                    <asp:TextBox ID="tc_texbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label>Doğum Tarihiniz</label>
                                    <asp:TextBox ID="dogumtarihi_textbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Email Adresiniz (Sisteme giriş kullanıcı adınız)</label>
                                    <asp:TextBox ID="kullaniciadi_textbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label>Şifreniz [ Şifrenizi Değiştirebilirsiniz. ]</label>
                                    <asp:TextBox ID="sifresi_textbox" runat="server" CssClass="form-control" placeholder="**********" TextMode="Password" BorderColor="#ff6e00"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-3">
                                    <label>Mezuniyet Yılınız</label>
                                    <asp:TextBox ID="mezuniyetyili_textbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label>Eğitim Durumunuz</label>
                                    <asp:TextBox ID="egitim_textbox" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    <br />
                                </div>

                                 <div class="col-md-6">
                                    <label>Göreviniz</label>
                                    <asp:TextBox ID="gorev_drop" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-actions text-right" runat="server" id="g_panel">
                            <a href="/Anasayfa/" class="btn btn-danger">İptal</a>
                            <asp:Button ID="kayit" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="Pers_Kayit" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

