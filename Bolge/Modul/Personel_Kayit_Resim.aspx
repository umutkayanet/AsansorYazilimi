<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Personel_Kayit_Resim.aspx.cs" Inherits="Modul_Personel_Kayit_Resim" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Başlık -->
    <div class="page-header">
        <div class="page-title">
            <h3>Personel Resim  <small>Lütfen aşağıdaki alanları eksiksiz doldurmaya özen gösteriniz.</small></h3>
        </div>

        <!-- Üst Sayfa Bilgilendirici Menü-->
        <div class="breadcrumb-line">
            <ul class="breadcrumb">
                <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                <li><a href="/Personel-Listesi">İnsan Kaynakları</a></li>
                <li class="active"><a href="/Personel-Islem/Guncelle/<%=RouteData.Values["id"] %>">Personel Kayıt</a></li>
                <li class="active">Resim Yükleme</li>
            </ul>
        </div>

        <!-- Personel Bilgilerini Ekrana Yaz -->
        <div class="breadcrumb-line" style="margin-right:5px; font-size:12px;">
            <ul class="breadcrumb">
                <li><asp:Label ID="persadi_label" runat="server" Text="" ForeColor="#1fa8b7"></asp:Label></li>
            </ul>
        </div>        
        <!-- Son Üst Sayfa Bilgilendirici Menü-->
    </div>
    <!-- Son Başlık -->


    <div class="form-horizontal form-bordered">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-users"></i>Personel Resim</h6>
            </div>

            <!-- Resim Yükleme Alanı -->
            <div class="panel-body">
                <div class="form-group">
                    <label class="col-sm-2 control-label">Resim Yükle</label>
                    <div class="col-sm-10">
                        <div class="row">
                            <div class="col-sm-6 has-feedback">
                                <asp:FileUpload ID="FileUpload1" runat="server" ViewStateMode="Enabled" />
                            </div>

                            <div class="col-sm-6 has-feedback">
                                <div style="border: 1px solid #808080;  width: 172px; padding: 5px 5px 5px 5px">
                                    <div class="resimortala">
                                    <asp:Image ID="personel_resim" runat="server" /><br />
                                    </div>
                                </div>

                                <div style="margin: 10px 0 0 0px">
                                    <asp:LinkButton ID="resmi_sil" runat="server" CssClass="btn btn-icon btn-danger" OnClick="resmi_sil_Click"><i class="icon-remove3"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="g_panel" runat="server">
                <div class="form-actions text-right">
                    <a href="/Personel-Islem/Guncelle/<%=RouteData.Values["id"] %>" class="btn btn-danger">İptal</a>
                    <asp:Button ID="kayit" runat="server" Text="Yükle" CssClass="btn btn-primary" OnClick="kayit_Click" />
                </div>
               </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>

