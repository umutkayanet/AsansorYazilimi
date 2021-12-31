<%@ Page Title="" Language="C#" ClientIDMode="AutoID" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Roller.aspx.cs" Inherits="Modul_Roller" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server"></asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
                function pageLoad() {
                    $(document).ready(function () {
                        $('.switch1').bootstrapSwitch();
                    });
                }
            </script>

            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Roller<small>Kullanıcılara ait rolleri aşağıda belirleyebilirsiniz.</small></h3>
                </div>
                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Roller">Roller</a></li>
                        <li><asp:Label ID="rol_baslik" runat="server" Text=""></asp:Label></li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->


            <!-- Roller Bölümü -->
            <asp:Panel ID="Panel1" runat="server">
                <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                    <a href="/Roller" class="btn btn-success"><i class="icon-unlocked2"></i>Yeni Rol Ekle</a>
                </div>

                <div class="form-horizontal">
                    <div class="row">
                        <!-- Rol Listesi -->
                        <div class="col-md-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h6 class="panel-title"><i class="icon-table"></i>Roller</h6>
                                </div>

                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Rol Adı</th>
                                            <th style="width: 15%; text-align: center">İşlem</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rep1" runat="server" EnableViewState="True">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("YR_Rol_Adi") %></td>
                                                    <td style="text-align: center">
                                                        <a href="/Roller/Yetki/<%#Eval("YR_Id") %>" class="btn btn-default btn-icon tip" data-original-title="Rol Yetkileri">
                                                            <i class="icon-unlocked2"></i>
                                                        </a>

                                                        <a href="/Roller/Guncelle/<%#Eval("YR_Id") %>" class="btn btn-default btn-icon tip" data-original-title="Düzenle">
                                                            <i class="icon-pencil3"></i>
                                                        </a>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>


                        <!-- Yeni Rol Ekleme / Rol Düzenleme --> 
                        <div class="col-md-6">
                            <div class="panel panel-default">
                                <div class="panel-heading" runat="server" id="guncelle_div">
                                    <h6 class="panel-title"><i class="icon-unlocked2"></i>
                                        <asp:Label ID="rol_ekle_baslik" runat="server" Text="Yeni Rol Ekle"></asp:Label>
                                    </h6>
                                </div>

                                <!-- Adı Soyadı Alanı-->
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Rol Adı</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="rol_adi" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rol_adi_sorgula" runat="server" ErrorMessage="Lütfen adı alanını boş bırakmayınız." ControlToValidate="rol_adi" ValidationGroup="rol_ekle" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Standart Rol</label>
                                        <div class="col-sm-10">
                                            <label class="checkbox-inline">
                                                <input id="standart_rol" runat="server" type="checkbox" class="switch1 switch-mini" data-on-label="<i class='icon-checkmark3'></i>" data-off-label="<i class='icon-cancel'></i>" checked="checked">
                                            </label>
                                        </div>
                                    </div>

                                    <div class="form-actions text-right" runat="server" id="g_panel">
                                        <input type="reset" class="btn btn-danger" value="İptal">
                                        <asp:Button ID="kayit" runat="server" Text="Ekle" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="rol_ekle" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <!-- SON -->
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Yükleniyor İbaresi -->
    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <%=Sorgu.yukleniyor()%>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

