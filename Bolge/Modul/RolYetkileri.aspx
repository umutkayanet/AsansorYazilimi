<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="RolYetkileri.aspx.cs" Inherits="Modul_RolYetkileri" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Rol Yetkileri<small>Aşağıda
                <asp:Label ID="rol_baslik1" runat="server" Text=""></asp:Label>
                        rolüne bağlı yetkileri düzenleyebilirsiniz.</small></h3>
                </div>
                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Roller">Roller</a></li>
                        <li><asp:Label ID="rol_baslik2" runat="server" Text=""></asp:Label></li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <asp:Label ID="rol_baslik3" runat="server" Text=""></asp:Label>
                                Yetkileri
                            </h6>
                        </div>
                        <div class="panel-body">
                            <div class="panel-group">
                                <asp:Repeater ID="Rep1" runat="server" OnItemDataBound="Rep1_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="panel panel-primary">
                                            <a data-toggle="collapse" href="#<%#Sorgu.dosya_adi_yarat(Eval("YY_Rol_Yetki_Kategori").ToString())%>">
                                                <div class="panel-heading accordionbaslik">
                                                    <h6 class="panel-title">
                                                        <asp:HiddenField ID="RY_Rol_Yetki_Kategori_id" runat="server" Value='<%#Eval("YY_Rol_Yetki_Kategori")%>' />
                                                        <div><%#Eval("YY_Rol_Yetki_Kategori")%></div>
                                                    </h6>
                                                </div>
                                            </a>
                                            <div id="<%#Sorgu.dosya_adi_yarat(Eval("YY_Rol_Yetki_Kategori").ToString())%>" class="panel-collapse collapse in" style="height: auto;">
                                                <div class="panel-body">
                                                    <asp:Repeater ID="Rep2" runat="server" OnItemDataBound="Rep2_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <div class="datatablee">
                                                                <tbody>
                                                                    <table class="table">
                                                                        <thead>
                                                                            <tr>
                                                                                <th style="width: 20%">Yetki</th>
                                                                                <th>Açıklama</th>
                                                                                <th style="width: 10%">İşlem</th>
                                                                            </tr>
                                                                        </thead>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="yy_id" runat="server" Value='<%#Eval("YY_Id") %>' />
                                                                    <%#Eval("YY_Yetki_Adi") %></td>
                                                                <td><%#Eval("YY_Yetki_Aciklama") %></td>
                                                                <td>
                                                                    <asp:Panel ID="durum_panel" runat="server">
                                                                        <label class="onoff">
                                                                            <input id="durumu2" type="checkbox" value='<%# String.Format("{0} - {1}", RouteData.Values["id"].ToString(), Eval("YY_Id")) %>' class="onoff-input" onchange="yetki(this)" runat="server" />
                                                                            <span class="onoff-label" data-on="on" data-off="off" title="Sisteme Giriş İzni"></span>
                                                                            <span class="onoff-handle"></span>
                                                                        </label>
                                                                    </asp:Panel>

                                                                    <asp:Panel ID="durum_panel_2" runat="server">
                                                                        <asp:Label ID="durum_mesaj" runat="server" Text="Label"></asp:Label>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                            </table></tbody></div>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Yükleniyor İbaresi -->
            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                <ProgressTemplate>
                    <%=Sorgu.yukleniyor()%>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


