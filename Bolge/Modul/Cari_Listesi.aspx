<%@ Page Title="Cari Listesi" Language="C#" MasterPageFile="~/Merkez_Nokta.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Cari_Listesi.aspx.cs" Inherits="Modul_Cari_Listesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Başlık -->
    <div class="page-header">
        <div class="page-title">
            <h3><asp:Label ID="cari_ana_baslik" runat="server" Text=""></asp:Label> <small>Aşağıda cari listesini görebilirsiniz.</small></h3>
        </div>

        <!-- Üst Sayfa Bilgilendirici Menü-->
        <div class="breadcrumb-line">
            <ul class="breadcrumb">
                <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                <li class="active">Cari Listesi</li>
            </ul>
        </div>
        <!-- Son Üst Sayfa Bilgilendirici Menü-->
    </div>
    <!-- Başlık Son -->



    <!-- Acordion Menü -->
    <div class="panel-group block" id="accordion">
        <div class="panel panel-default">
            <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                <div class="panel-heading" style="background-color: #fafafa;">
                    <h6 class="panel-title"><i class="icon-search3"></i>Ara<span class="caret"></span></h6>
                </div>
            </a>
            <div id="collapseOne" class="panel-collapse collapse">
                <!-- Pro Arama -->
                <div class="panel-body" style="padding: 15px 15px 5px 0px">
                    <div class="form-group" style="margin: 5px 0 -12px 0">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Cari_Kodu" runat="server" CssClass="form-control" placeholder="Cari Kodu"></asp:TextBox>
                                    <span class="help-block"></span>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Cari_Unvan" runat="server" CssClass="form-control" placeholder="Ünvanı"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Cari_Telefon" runat="server" CssClass="form-control" placeholder="Telefon"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Cari_Yetkili" runat="server" CssClass="form-control" placeholder="Yetkili Kişi"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-actions text-right" style="margin-top:10px">
                                <a href="/Anasayfa/" class="btn btn-danger">İptal</a>
                                <asp:Button ID="ara" runat="server" Text="Ara" CssClass="btn btn-primary" OnClick="ara_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Son Pro Arama -->
            </div>
        </div>
    </div>
    <!-- Acordion Menü -->

    <!-- Cari Liste Tablosu -->
    <div class="tab-content" style="border: 0px solid #ddd;">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-tab"></i>
                    <asp:Label ID="cari_baslik" runat="server" Text=""></asp:Label>
                </h6>
                <!-- Menü Buton -->
                <asp:Panel ID="Menu_Button" runat="server">
                <div class="btn-group pull-right" id="popmen" runat="server">
                    <button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><i class="icon-grid"></i><span class="caret"></span></button>
                    <ul class="dropdown-menu dropdown-menu-right icons-right">
                        <li><asp:LinkButton ID="Cari_Kayit" runat="server" PostBackUrl='/Cari-Kayit/Satis'><i class="icon-cogs"></i>Yeni Kayıt</asp:LinkButton></li>
                        <li><asp:LinkButton ID="excel_export" runat="server" OnClick="excel_export_Click"><i class="icon-file-excel"></i>Excel'e Aktar</asp:LinkButton></li>
                        <li><asp:LinkButton ID="print_button" runat="server" OnClick="print_Click"><i class="icon-print2"></i>Yazdır</asp:LinkButton></li>
                    </ul>
                </div>
                </asp:Panel>
                <!-- SON Menü Buton -->
            </div>


            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <!-- Yükleniyor İbaresi -->
                    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                        <ProgressTemplate>
                            <%=Sorgu.yukleniyor()%>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <!-- asp.net update panelin düzgün çalışabilmesi için gerekli kod -->
                    <script type="text/javascript">
                        function pageLoad() {
                            $(document).ready(function () {
                                oTable = $('.datatablee table').dataTable({
                                    "aaSorting": [],
                                    "bJQueryUI": false,
                                    "bAutoWidth": false,
                                    "sPaginationType": "full_numbers",
                                    "sDom": '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
                                    "oLanguage": {
                                        "sSearch": "<span>Hızlı Filtreleme:</span> _INPUT_",
                                        "sLengthMenu": "<span>Kayıt Sayısı : _MENU_</span>",
                                        "oPaginate": { "sFirst": "Geri", "sLast": "İleri", "sNext": ">", "sPrevious": "<" }
                                    }
                                });
                            });
                        }
                    </script>


                    <div class="datatablee">
                        <tbody>
                            <asp:Repeater ID="personel_rep" runat="server" EnableViewState="True" OnItemCommand="personel_rep_ItemCommand" OnItemDataBound="personel_rep_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th style="width: 10%">Cari Kodu</th>
                                                <th>Ünvan</th>
                                                <th style="width: 10%">Telefon</th>
                                                <th style="width: 12%">Vergi Dairesi</th>
                                                <th style="width: 10%">Şehir</th>
                                                <th style="width: 10%">İlçe</th>                                             
                                                <th style="width: 10%">İşlem</th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("Cari_Kodu") %></td>
                                        <td><%#Eval("Cari_Unvan") %></td>
                                        <td><%#Eval("Cari_Telefon") %></td>
                                        <td><%#Eval("Cari_VergiDairesi") %></td>
                                        <td><%#Eval("Sehir_Adi") %></td>
                                        <td><%#Eval("tblSIlceler_IlceAdi") %></td>
                                        <td>
                                            <a href='<%# string.Format("/Cari-Kayit/{1}/{0}", Eval("Cari_Id"), RouteData.Values["gorev"]) %>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Düzenle"><i class="icon-pencil3"></i></a>
                                            <asp:LinkButton ID="sil" runat="server" CommandName="sil" CommandArgument='<%# Eval("Cari_Id") %>' CssClass="btn btn-default btn-xs btn-icon tip" data-original-title="Sil" OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;">
                                            <i class="icon-remove3"></i>
                                            </asp:LinkButton>
                                        </td>                                        
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                </table>
                                </FooterTemplate>
                            </asp:Repeater>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ara" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Son Personel Liste Tablosu -->



    <div style="display: none">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    function printDiv() {
                        var divToPrint = document.getElementById('print').outerHTML;
                        //var newWin = window.open('', 'Print-Window','');
                        var newWin = window.open('', '', 'toolbar=yes,scrollbars=yes,resizable=yes,top=0,left=0,width=600,height=600');
                        newWin.document.open();
                        newWin.document.write('<html><head><title>Personel Listesi</title>');
                        newWin.document.write('<link rel="stylesheet" href="/Desing/Default/css/bootstrap.min.css" type="text/css" />');
                        newWin.document.write('<link rel="stylesheet" href="/Desing/Paket/Print/style.css" type="text/css" />');
                        newWin.document.write('</head><body  onload="window.print()">');
                        newWin.document.write(divToPrint);
                        newWin.document.write('</body></html>');
                        newWin.document.close();
                    }
                </script>

                <div id="print" class="print">
                    <div class="print_baslik">
                        <b>Belge Özeti</b></br>
                    <asp:Label ID="belge_ozet_label" runat="server" Text=""></asp:Label>
                    </div>

                    <table class="table">
                        <tr>
                            <th style="width: 9%">Cari Kodu</th>
                            <th style="width: 9%">Cari Türü</th>
                            <th>Ünvan</th>
                            <th style="width: 9%">Telefon</th>
                            <th style="width: 13%">Yetkişi Kişi</th>                            
                        </tr>

                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("Cari Kodu") %></td>
                                    <td><%#Eval("Cari Türü") %></td>
                                    <td><%#Eval("Ünvan") %></td>
                                    <td><%#Eval("Telefon") %></td>
                                    <td><%#Eval("Yetkili Kişi") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="print_button" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

