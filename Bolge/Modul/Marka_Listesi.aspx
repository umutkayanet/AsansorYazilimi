<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Marka_Listesi.aspx.cs" Inherits="Modul_Marka_Listesi" EnableEventValidation="false" ClientIDMode="AutoID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $(document).ready(function () {
                        $('.switch1').bootstrapSwitch();
                    });

                    $(document).ready(function () {
                        $(".select-search4").select2({
                            width: "100%"
                        });
                    });

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



            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Marka Listesi<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü -->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li class="active">Marka Listesi</li>
                    </ul>
                </div>
                <!-- Son -->
            </div>
            <!-- Son -->


            <!-- Yeni Marka Ekleme Butonu -->
            <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                <a href="/Marka-Listesi/" class="btn btn-success"><i class="icon-plus-circle"></i>Yeni Marka Ekle</a>
            </div>


            <div class="form-horizontal form-bordered">
                <div class="row">
                    <div class="col-md-6">
                        <div class="panel-group block" id="accordion">
                            <div class="panel panel-default">
                                <ajaxToolkit:Accordion ID="MyAccordion" runat="server" FramesPerSecond="150" TransitionDuration="500" AutoSize="None" RequireOpenedPane="False" Width="100%" EnableTheming="False">
                                    <Panes>
                                        <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                                            <Header>
                                                <div class="panel-heading" style="background-color: #fafafa;">
                                                    <h6 class="panel-title"><i class="icon-search3"></i>Ara<span class="caret"></span></h6>
                                                </div>
                                            </Header>
                                            <Content>
                                                <div class="panel-body" style="padding: 15px 15px 5px 0px">
                                                    <div class="form-group" style="margin: 0px 0 12px 0">
                                                        <div class="col-sm-12">
                                                            <div class="row">
                                                                <div class="col-sm-12 has-feedback">
                                                                    <asp:TextBox ID="Marka_Ara" runat="server" CssClass="form-control" placeholder="Marka"></asp:TextBox>
                                                                    <span class="help-block"></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-actions text-right" style="margin-top: 10px">
                                                                <a href="/Anasayfa/" class="btn btn-danger">İptal</a>
                                                                <asp:Button ID="ara" runat="server" Text="Ara" CssClass="btn btn-primary" OnClick="ara_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </Content>
                                        </ajaxToolkit:AccordionPane>
                                    </Panes>
                                </ajaxToolkit:Accordion>
                            </div>
                        </div>



                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h6 class="panel-title"><i class="icon-tag4"></i>Marka Listesi</h6>
                                <div class="btn-group pull-right" id="popmen" runat="server">
                                    <button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><i class="icon-grid"></i><span class="caret"></span></button>
                                    <ul class="dropdown-menu dropdown-menu-right icons-right">
                                        <li>
                                            <asp:LinkButton ID="excel_export" runat="server" OnClick="excel_export_Click"><i class="icon-file-excel"></i>Excel'e Aktar</asp:LinkButton></li>
                                    </ul>
                                </div>
                            </div>

                            <div class="datatablee">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Marka</th>
                                            <th style="width: 18%">İşlem</th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater ID="stop_rep" runat="server" EnableViewState="True" OnItemCommand="stop_rep_ItemCommand" OnItemCreated="rep1_ItemCreated">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("Marka_Adi") %></td>
                                                <td>
                                                    <a href='/Marka-Listesi/<%#Eval("Marka_Id")%>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Düzenle"><i class="icon-pencil3"></i></a>
                                                    <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-xs btn-icon tip" data-original-title="Sil" CommandName="sil" CommandArgument='<%# Eval("Marka_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </div>


                    <!-- Yeni Marka Ekleme / Marka Düzenleme -->
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading" runat="server" id="guncelle_div">
                                <h6 class="panel-title"><i class="icon-tag4"></i>
                                    <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Marka Ekle"></asp:Label>
                                </h6>
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Marka Adı</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="Text1" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Text1_Sorgula" runat="server" ErrorMessage="Lütfen Marka Kodu Alanını Boş Bırakmayınız." ControlToValidate="Text1" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>
                                </div>

                                <div class="form-actions text-right" runat="server" id="g_panel">
                                    <input type="reset" class="btn btn-danger" value="İptal">
                                    <asp:Button ID="kayit" runat="server" Text="Ekle" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="group1" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="group1" ShowMessageBox="True" ShowSummary="False" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="excel_export" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Yükleniyor İbaresi -->
    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <%=Sorgu.yukleniyor()%>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>


