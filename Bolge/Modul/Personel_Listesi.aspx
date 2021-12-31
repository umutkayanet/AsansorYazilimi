<%@ Page Title="Personel Listesi" Language="C#" MasterPageFile="~/Merkez_Nokta.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Personel_Listesi.aspx.cs" Inherits="Modul_Personel_Listesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Başlık -->
    <div class="page-header">
        <div class="page-title">
            <h3>Personel Listesi <small>Aşağıda tüm personel listesini görebilirsiniz.</small></h3>
        </div>

        <!-- Üst Sayfa Bilgilendirici Menü-->
        <div class="breadcrumb-line">
            <ul class="breadcrumb">
                <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                <li><a href="#">İnsan Kaynakları</a></li>
                <li class="active">Personel Listesi</li>
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
                    <h6 class="panel-title"><i class="icon-users"></i>Ara<span class="caret"></span></h6>
                </div>
            </a>
            <div id="collapseOne" class="panel-collapse collapse">
                <!-- Pro Arama -->
                <div class="panel-body" style="padding: 15px 15px 5px 0px">
                    <div class="form-group" style="margin: 5px 0 -12px 0">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Personel_adi_arama" runat="server" CssClass="form-control" placeholder="Personel Adı"></asp:TextBox>
                                    <span class="help-block"></span>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Personel_soyadi_arama" runat="server" CssClass="form-control" placeholder="Personel Soyadı"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="Personel_tc_arama" runat="server" CssClass="form-control" placeholder="Kimlik Numarası"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="personel_email_arama" runat="server" CssClass="form-control" placeholder="Email Adresi"></asp:TextBox>
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

    <!-- Personel Liste Tablosu -->
    <div class="tab-content" style="border: 0px solid #ddd;">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-table"></i>Personel Listesi</h6>
                <!-- Menü Buton -->
                <div class="btn-group pull-right" id="popmen" runat="server">
                    <button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><i class="icon-grid"></i><span class="caret"></span></button>
                    <ul class="dropdown-menu dropdown-menu-right icons-right">
                        <li>
                            <asp:LinkButton ID="Personel_Ekle" runat="server" PostBackUrl="/Personel-Ekle/"><i class="icon-cogs"></i>Personel Ekle</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="excel_export" runat="server" OnClick="excel_export_Click"><i class="icon-file-excel"></i>Excel'e Aktar</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="print_button" runat="server" OnClick="print_Click"><i class="icon-print2"></i>Yazdır</asp:LinkButton></li>
                    </ul>
                </div>
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
                            <asp:Repeater ID="personel_rep" runat="server" EnableViewState="True">
                                <HeaderTemplate>
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th style="width: 2%"><i class="icon-camera"></i></th>
                                                <th style="width: 13%">Adı</th>
                                                <th>Soyadı</th>                                            
                                                <th style="width: 18%">İşlem</th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div style="display:none"><%#Eval("Foto") %></div>  
                                            <ajaxToolkit:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="urun_resim_panel" TargetControlID="tt" OffsetX="30" OffsetY="0"></ajaxToolkit:HoverMenuExtender>
                                            <a href="<%# Sorgu.GetImageUrl("/Data/personel_resim/" + Eval("Foto")) %>" class="lightbox" style="color: #333333;">
                                                <i class='icon-camera' id="tt" runat="server"></i>
                                            </a>                                           
                                            <asp:Panel ID="urun_resim_panel" runat="server" Style="display: none">
                                                <div class="personel_resim_honer">
                                                    <img src="<%# Sorgu.GetImageUrl("/Data/personel_resim/kucuk_resim/" + Eval("Foto")) %>" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                        <td><%#Eval("Pers_Adi") %></td>
                                        <td><%#Eval("Pers_Soyadi") %></td>                                       
                                        <td>
                                            <asp:Panel ID="guvenlik_panel" runat="server">
                                                <a href='<%# string.Format("/Personel-Islem/Guncelle/{0}", Eval("Persid")) %>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Düzenle"><i class="icon-pencil3"></i></a>
                                                <a href='<%# string.Format("/Personel-Giris/{0}", Eval("Persid")) %>' class="btn btn-default btn-xs btn-icon tip" data-original-title="İşe Giriş Çıkış"><i class="icon-loop"></i></a>
                                                <a href='<%# string.Format("/Personel-Resim/{0}", Eval("Persid")) %>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Resim"><i class="icon-camera"></i></a>
                                                <a href='<%# string.Format("/Personel-Izin/{0}", Eval("Persid")) %>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Yıllık İzin"><i class="icon-sun"></i></a>
                                                <a href='<%# string.Format("/Personel-Yetki/{0}", Eval("Persid")) %>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Yetki"><i class="icon-unlocked"></i></a>
                                            </asp:Panel>
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
                            <th>Adı Soyadı</th>
                            <th style="width: 13%">Kimlik Numarası</th>
                            <th style="width: 13%">Doğum Tarihi</th>
                            <th style="width: 7%">Cinsiyet</th>
                            <th style="width: 7%">Yaş</th>
                            <th style="width: 13%">Email Adresi</th>
                            <th style="width: 13%">Telefon Numarası</th>
                        </tr>

                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("Adı") %> <%#Eval("Soyadı") %></td>
                                    <td><%#Eval("Kimlik Numarası") %></td>
                                    <td><%#Eval("Doğum Tarihi") %></td>
                                    <td><%#Eval("Cinsiyet") %></td>
                                    <td><%#Eval("Yaş") %></td>
                                    <td><%#Eval("Email Adresi") %></td>
                                    <td><%#Eval("Şirket Gsm Numarası") %></td>
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

