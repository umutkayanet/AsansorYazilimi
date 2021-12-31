<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Personel_Ise_Giris_Cikis.aspx.cs" Inherits="Modul_Personel_Ise_Giris_Cikis" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Başlık -->
    <div class="page-header">
        <div class="page-title">
            <h3>Personel İşe Giriş / Çıkış Bilgileri <small>Lütfen aşağıdaki alanları eksiksiz doldurmaya özen gösteriniz.</small></h3>
        </div>

        <!-- Üst Sayfa Bilgilendirici Menü-->
        <div class="breadcrumb-line">
            <ul class="breadcrumb">
                <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                <li><a href="/Personel-Listesi">İnsan Kaynakları</a></li>
                <li class="active"><a href="/Personel-Islem/Guncelle/<%=RouteData.Values["id"]%>">Personel Kayıt</a></li>
                <li class="active">İşe Giriş Çıkış</li>
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

    <br />
    <!-- Tarih Ekle Butonu-->
    <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
        <asp:LinkButton ID="LinkButton1" CssClass="btn btn-success" runat="server" OnClick="LinkButton1_Click"><i class="icon-calendar"></i>Yeni İşe Giriş Tarihi Ekle</asp:LinkButton>
    </div>

    <div class="form-horizontal form-bordered">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-users"></i>Personel İşyeri Bilgileri</h6>
            </div>

            <div class="panel-body">
                <div class="form-group">
                    <div class="col-sm-12">
                        <div class="row">
                            <!-- İşe Giriş-Çıkış Liste Alanı-->
                            <div class="col-sm-8 has-feedback">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h6 class="panel-title"><i class="icon-arrow4"></i>İşe Giriş Çıkış Bilgileri</h6>
                                    </div>
                                    <div class="table-responsive pre-scrollable">
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th style="width: 30%">Giriş Tarihi</th>
                                                    <th style="width: 30%">Çıkış Tarihi</th>
                                                    <th>İşten Ayrılış Nedeni</th>
                                                    <th style="width: 100px">İşlem</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" OnItemDataBound="Repeater1_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%#Convert.ToDateTime(Eval("IGC_Giris_Tarihi")).ToString("dd.MM.yyyy") %></td>
                                                            <td><%#Eval("IGC_Cikis_Tarihi").ToString() == ""  ? "Personel Çalışıyor" : Convert.ToDateTime(Eval("IGC_Cikis_Tarihi")).ToString("dd.MM.yyyy")%></td>
                                                            <td><%#Eval("IGCA_Aciklama") %></td>
                                                            <td>
                                                                <asp:Panel ID="button_panel" runat="server">
                                                                <a href="/Personel-Giris/Guncelle/<%=RouteData.Values["id"] %>/<%#Eval("IGC_Id") %>" class="btn btn-default btn-xs btn-icon tip" data-original-title="Düzenle">
                                                                    <i class="icon-pencil2"></i>
                                                                </a>
                                                                <asp:LinkButton ID="sil" runat="server" CommandName="sil" CommandArgument='<%# Eval("IGC_Id") %>' CssClass="btn btn-default btn-xs btn-icon tip"  data-original-title="Sil" OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;" >
                                                                <i class="icon-remove3"></i>
                                                                </asp:LinkButton>
                                                                </asp:Panel>                                                                
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>


                            <!-- İşe Giriş Alanı-->
                            <div class="col-sm-4 has-feedback" style="border-left: 1px dotted #cecece; padding: 0px 15px 15px 15px; margin: 0 0 0 0px">
                                <h6 style="padding: 0px 5px 0px 0px">
                                    <asp:Label ID="personelin_adi" runat="server" Text=""></asp:Label>
                                </h6>
                                <asp:Panel ID="Panel1" runat="server">
                                    <input type="text" id="giristarihi_textbox" runat="server" class="form-control" data-mask="99/99/9999">
                                    <span class="help-block">Giriş Tarihi : Gün/Ay/Yıl
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen giriş tarihi alanını boş bırakmayınız." ControlToValidate="giristarihi_textbox" ValidationGroup="isegiriscikis" ForeColor="Red">(*)</asp:RequiredFieldValidator></span>
                                    <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="giristarihi_textbox" ErrorMessage="Lütfen İşe Giriş Tarihi Alanını Kontrol Ediniz" Type="Date" MinimumValue="01/01/1963" MaximumValue="01/01/2030" Display="Dynamic" ValidationGroup="isegiriscikis" ForeColor="Red">(*)</asp:RangeValidator>
                                </asp:Panel>


                                <!-- İşden Çıkış Alanı-->
                                <asp:Panel ID="Panel2" runat="server" Visible="false">
                                    <input type="text" id="cikistarihi_textbox" runat="server" class="form-control" data-mask="99/99/9999">
                                    <span class="help-block">Çıkış Tarihi : Gün/Ay/Yıl
                                        <asp:CompareValidator ID="CompareValidator1" ValidationGroup="isegiriscikis" ForeColor="Red" runat="server" ControlToValidate="giristarihi_textbox" ControlToCompare="cikistarihi_textbox" Operator="LessThan" Type="Date" ErrorMessage="Giriş Tarihi/Çıkış Tarihinden Büyük Olamaz">(*)</asp:CompareValidator>
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="cikistarihi_textbox" ErrorMessage="Lütfen Çıkış Tarihi Alanını Kontrol Ediniz" Type="Date" MinimumValue="01/01/1963" MaximumValue="01/01/2030" Display="Dynamic" ValidationGroup="isegiriscikis" ForeColor="Red">(*)</asp:RangeValidator></span>

                                    <asp:DropDownList ID="cikma_sebebi_drop" runat="server" CssClass="select-full" AppendDataBoundItems="True">
                                        <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                        <asp:ListItem Value="1">Sağlık Sorunları</asp:ListItem>
                                        <asp:ListItem Value="2">İstifa</asp:ListItem>
                                        <asp:ListItem Value="3">Proje Bitimi</asp:ListItem>
                                        <asp:ListItem Value="4">Sözleşme Sonu</asp:ListItem>
                                        <asp:ListItem Value="5">Denem Süresi</asp:ListItem>
                                        <asp:ListItem Value="6">Emeklilik</asp:ListItem>
                                        <asp:ListItem Value="7">Malulen Emeklilik</asp:ListItem>
                                        <asp:ListItem Value="8">Ölüm</asp:ListItem>
                                        <asp:ListItem Value="9">İş Kazası</asp:ListItem>
                                        <asp:ListItem Value="10">Askerlik</asp:ListItem>
                                        <asp:ListItem Value="11">Diğer Sebepler</asp:ListItem>                                        
                                    </asp:DropDownList>
                                    <span class="help-block">Çıkış Nedeni</span>                                    
                                </asp:Panel>
                                <asp:Panel ID="g_panel" runat="server">
                                    <div class="form-actions text-right">
                                        <a href="/Personel-Islem/Guncelle/<%=RouteData.Values["id"]%>" class="btn btn-danger">İptal</a>
                                        <asp:Button ID="kayit" runat="server" Text="Ekle" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="isegiriscikis" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="isegiriscikis" ShowMessageBox="True" ShowSummary="False" />
</asp:Content>

