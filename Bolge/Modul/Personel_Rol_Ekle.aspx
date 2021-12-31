<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Personel_Rol_Ekle.aspx.cs" Inherits="Modul_Personel_Rol_Ekle" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        //İstisna kaldır butona basınca kaldır.
        function test(a) {
            var asd = a;
            document.getElementById("o_" + asd).setAttribute("style", "display:none");
            document.getElementById("s_" + asd).setAttribute("style", "display:none");
            document.getElementById("k_" + asd).setAttribute("style", "");
            document.getElementById('<%= Button1.ClientID %>').click();
        }
        
        function yukle() {
            setTimeout('timeout_trigger()', 500);           
        }

        function timeout_trigger() {
            document.getElementById('<%= Button1.ClientID %>').click();
        }

        //İstisna Eklenince Ekle
        function testt(a, b) {
            var a;
            var b;

            document.getElementById("s_" + b).innerHTML = "<label class='onay'>" +
                "<input id='onaychec' type='checkbox' value='" + a + " - " + b + "' onclick='yetkipersonelistsil(this); test(" + b + ")' class='onay-input' checked />" +
                "<span class='onay-label' data-on='İstisnai [ İptal ]' data-off='Yetki Silindi' title='Sisteme Giriş İzni'></span><span class='onay-handle'></span></label>";
            document.getElementById("s_" + b).setAttribute("style", "");
            document.getElementById("o_" + b).setAttribute("style", "display:none");
            document.getElementById("k_" + b).setAttribute("style", "display:none");
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- İstaitstik kaldır butonuna basınca otomatik yenilenmesi için yerleştirilen kod -->
            <asp:Button ID="Button1" runat="server" Text="Button" style="display:none"/>
            
            <script type="text/javascript">
                function pageLoad() {
                    $(document).ready(function () {
                        $(".select-search4").select2({
                            width: "100%"
                        });
                    });
                }
            </script>


            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Personel Yetki İşlemleri <small>Ana Rol Yetkilerini Yükle</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Personel-Listesi">İnsan Kaynakları</a></li>
                        <li class="active"><a href="/Personel-Islem/Guncelle/<%=RouteData.Values["id"]%>">Personel Kayıt</a></li>
                        <li class="active">Yetki</li>
                    </ul>
                </div>

                <!-- Personel Bilgilerini Ekrana Yaz -->
                <div class="breadcrumb-line" style="margin-right: 5px; font-size: 12px;">
                    <ul class="breadcrumb">
                        <li>
                            <asp:Label ID="persadi_label" runat="server" Text="" ForeColor="#1fa8b7"></asp:Label></li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->

            <br />
            <!-- Rol Ekle Butonu-->
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-sm-12">
                        <div class="row">
                            <!-- Rol Listesi -->
                            <div class="col-sm-5 has-feedback">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h6 class="panel-title"><i class="icon-arrow4"></i>Tanımlı Rol Bilgileri</h6>
                                    </div>
                                    <div class="table-responsive pre-scrollable">
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Rol</th>
                                                    <th style="width: 100px">İşlem</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Rep1" runat="server" OnItemDataBound="Rep1_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%#Eval("YR_Rol_Adi")%><asp:HiddenField ID="YR_ID" runat="server" Value='<%#Eval("YR_Id") %>' />
                                                            </td>
                                                            <td>
                                                                <asp:Panel ID="durum_panel" runat="server">
                                                                    <label class="onoff">
                                                                        <input id="durumu2" type="checkbox" value='<%# String.Format("{0} - {1}", RouteData.Values["id"].ToString(), Eval("YR_Id")) %>' class="onoff-input" onchange="peryetki(this)" onclick="yukle()" runat="server" />
                                                                        <span class="onoff-label" data-on="on" data-off="off" title="Sisteme Giriş İzni"></span>
                                                                        <span class="onoff-handle"></span>
                                                                    </label>
                                                                </asp:Panel>

                                                                <asp:Panel ID="durum_panel_2" runat="server">
                                                                    <asp:Label ID="durum_mesaj" runat="server" Text=""></asp:Label>
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


                            <!-- Rol Giriş Alanı-->
                            <div class="col-sm-7">
                                <div class="panel-group">
                                    <asp:Repeater ID="Rep2" runat="server" OnItemDataBound="Rep2_ItemDataBound">
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
                                                        <asp:Repeater ID="Rep3" runat="server" OnItemDataBound="Rep3_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <div class="datatablee">
                                                                    <tbody>
                                                                        <table class="table">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="width: 25%">Yetki</th>
                                                                                    <th>Açıklama</th>
                                                                                    <th style="width: 30%">Geçerli Rol</th>
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

                                                                        <div id="k_<%#Eval("YY_Id") %>">
                                                                            <asp:Label ID="gecerli_rol" runat="server" Text=""></asp:Label>
                                                                        </div>

                                                                        <div id="s_<%#Eval("YY_Id") %>"></div>

                                                                        <label id="o_<%#Eval("YY_Id") %>">
                                                                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                                                        </label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Panel ID="durum_panel3" runat="server">
                                                                            <label class="onoff">
                                                                                <input id="durumu3" type="checkbox" value='<%# String.Format("{0} - {1}", RouteData.Values["id"].ToString(), Eval("YY_Id")) %>' class="onoff-input" onchange="yetkipersonelist(this)" onclick='<%# String.Format("testt(\"{0}\", \"{1}\");", RouteData.Values["id"].ToString(), Eval("YY_Id")) %>' runat="server" />
                                                                                <span class="onoff-label" data-on="on" data-off="off" title="Sisteme Giriş İzni"></span>
                                                                                <span class="onoff-handle"></span>
                                                                            </label>
                                                                        </asp:Panel>

                                                                        <asp:Panel ID="durum_panel4" runat="server">
                                                                            <asp:Label ID="durum_mesaj4" runat="server" Text=""></asp:Label>
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
            </div>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="rol_ekle" ShowMessageBox="True" ShowSummary="False" />
            <!-- Yükleniyor İbaresi -->
            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                <ProgressTemplate>
                    <%=Sorgu.yukleniyor()%>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

