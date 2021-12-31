<%@ Page Title="Personel Listesi" Language="C#" MasterPageFile="~/Merkez_Nokta.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Servis_Listesi.aspx.cs" Inherits="Modul_Servis_Listesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Başlık -->
    <div class="page-header">
        <div class="page-title">
            <h3>Servis Listesi<small>Aşağıda servis listesini görebilirsiniz.</small></h3>
        </div>

        <!-- Üst Sayfa Bilgilendirici Menü-->
        <div class="breadcrumb-line">
            <ul class="breadcrumb">
                <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                <li class="active">Servis Listesi</li>
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

                                <div class="col-sm-6 has-feedback">
                                    <asp:TextBox ID="Asansor_Kimlik_NoAra" runat="server" CssClass="form-control" placeholder="Asansör Kimlik No"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="toplam_asansor_ara" runat="server" CssClass="form-control" placeholder="Toplam Asansör X'den Büyük Kayıtlar"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="toplam_bakim_ara" runat="server" CssClass="form-control" placeholder="Toplam Bakımı X'den Büyük Kayıtlar"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="toplam_revizyon_ara" runat="server" CssClass="form-control" placeholder="Toplam Revizyon X'den Büyük Kayıtlar"></asp:TextBox>
                                </div>

                                <div class="col-sm-3 has-feedback">
                                    <asp:TextBox ID="toplam_ariza_ara" runat="server" CssClass="form-control" placeholder="Toplam Ariza X'den Büyük Kayıtlar"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-actions text-right" style="margin-top: 10px">
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
                <h6 class="panel-title"><i class="icon-office"></i>
                    Servis Listesi
                </h6>
                <!-- Menü Buton -->
                <asp:Panel ID="Menu_Button" runat="server">
                    <div class="btn-group pull-right" id="popmen" runat="server">
                        <button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><i class="icon-grid"></i><span class="caret"></span></button>
                        <ul class="dropdown-menu dropdown-menu-right icons-right">
                            <li><asp:LinkButton ID="excel_export" runat="server" OnClick="excel_export_Click"><i class="icon-file-excel"></i>Excel'e Aktar</asp:LinkButton></li>
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
                                                <th style="width: 8%">Toplam Asansör</th>
                                                <th style="width: 8%">Toplam Bakım</th>
                                                <th style="width: 8%">Toplam Revizyon</th>
                                                <th style="width: 8%">Toplam Arıza</th>
                                                <th style="width: 10%">Toplam Sözleşme</th>
                                                <th style="width: 25%; min-width:25%">İşlem</th>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("Cari Kodu") %></td>
                                        <td><%#Eval("Ünvan") %></td>
                                        <td><span class="label label-success"><%#Eval("Toplam Asansor").ToString()=="" ? "0" : Eval("Toplam Asansor") %></span></td>
                                        <td><span class="label label-info"><%#Eval("Toplam Bakım").ToString()=="" ? "0" : Eval("Toplam Bakım") %></span></td>
                                        <td><span class="label label-warning"><%#Eval("Toplam Revizyon").ToString()=="" ? "0" : Eval("Toplam Revizyon") %></span> </td>
                                        <td><span class="label label-danger"><%#Eval("Toplam Ariza").ToString()=="" ? "0" : Eval("Toplam Ariza") %></span></td>
                                       <td><span class="label label-primary"><%#Eval("Toplam Sozlesme").ToString()=="" ? "0" : Eval("Toplam Sozlesme") %></span></td>
                                        <td>
                                            <div runat="server" id="Asansor_Ekle_P" style="float:left; margin-right:5px;">
                                            <a href="/Servis/Asansor-Ekle/<%#Eval("Cari_Id") %>" class="btn btn-icon btn-success tip" data-original-title="Asansör Ekle"><i class="icon-sort"></i></a>
                                            </div>

                                            <div ID="Bakim_Ekle_P" runat="server" style="float:left; margin-right:5px;">
                                            <a href="/Servis/Bakim-Ekle/<%#Eval("Cari_Id") %>" class="btn btn-icon btn-info tip" data-original-title="Bakım Listesi"><i class="icon-wave"></i></a>
                                            </div>

                                            <div ID="Revizyon_Ekle_P" runat="server" style="float:left; margin-right:5px;">
                                            <a href="/Servis/Revizyon-Ekle/<%#Eval("Cari_Id") %>" class="btn btn-icon btn-warning tip" data-original-title="Revizyon Listesi"><i class="icon-cog"></i></a>
                                            </div>

                                            <div ID="Ariza_Ekle_P" runat="server" style="float:left; margin-right:5px;">
                                            <a href="/Servis/Ariza-Listesi/<%#Eval("Cari_Id") %>" class="btn btn-icon btn-danger tip" data-original-title="Arıza Listesi"><i class="icon-tools"></i></a>
                                            </div>

                                            <div ID="Sozlesme_Ekle_P" runat="server" style="float:left; margin-right:5px;">
                                            <a href="/Servis/Cari-Sozlesmeler/<%#Eval("Cari_Id") %>" class="btn btn-icon btn-primary tip" data-original-title="Sözleşmeler"><i class="icon-file6"></i></a>
                                           </div>
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
</asp:Content>
