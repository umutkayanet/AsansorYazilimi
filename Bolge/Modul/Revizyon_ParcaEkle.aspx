<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Revizyon_ParcaEkle.aspx.cs" Inherits="Modul_Revizyon_ParcaEkle" EnableEventValidation="false" %>

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
                        $(".select-search5").select2({
                            width: "100%"
                        });
                    });

                    $(document).ready(function () {
                        /*Cv Proje Çalışma Alanı Arama*/
                        $('.multi-select-all-personel').multiselect({
                            buttonClass: 'btn btn-default',
                            includeSelectAllOption: true,
                            nonSelectedText: 'Personel',
                            onChange: function (element, checked) {
                                $.uniform.update();
                            }
                        });
                    });

                    $(document).ready(function () {
                        /*Cv Proje Çalışma Alanı Arama*/
                        $('.multi-select-all-bakimsecenekleri').multiselect({
                            buttonClass: 'btn btn-default',
                            includeSelectAllOption: true,
                            nonSelectedText: 'Bakımlar',
                            onChange: function (element, checked) {
                                $.uniform.update();
                            }
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
                    <h3>Ürün Ekle<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü -->
                <div class="breadcrumb-line">
                     <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Servis-Listesi">Servis Listesi</a></li>
                        <li><a href="/Servis/Revizyon-Ekle/<%=RouteData.Values["id"].ToString()%>">Revizyon Listesi</a></li>
                        <li class="active">Parça Ekle</li>
                    </ul>
                </div>
                <!-- Son -->

                <!-- Cari Bilgilerini Ekrana Yaz -->
                <div class="breadcrumb-line" style="margin-right: 5px; font-size: 12px;">
                    <ul class="breadcrumb">
                        <li><asp:Label ID="Cari_Label" runat="server" Text="" ForeColor="#1fa8b7"></asp:Label></li>
                    </ul>
                </div>
                <!-- Son -->
            </div>
            <!-- Son -->

            <div class="form-horizontal form-bordered">
                <div class="row">

                    <!-- Yeni Rol Ekleme / Rol Düzenleme -->
                    <div class="col-md-5">
                        <div class="panel panel-default">
                            <div class="panel-heading" runat="server" id="guncelle_div">
                                <h6 class="panel-title"><i class="icon-rating3"></i>
                                    <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Ürün Ekle"></asp:Label>
                                </h6>
                            </div>

                            <!-- Adı Soyadı Alanı-->
                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Ürün Listesi</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="Drop1" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="Drop1_sql" Width="250px" DataTextField="Stok_UrunAdi" DataValueField="Stok_Id">
                                            <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="val2" runat="server" ControlToValidate="Drop1" ErrorMessage="Lütfen ürün seçiminizi yapınız" InitialValue="0" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                        <asp:SqlDataSource ID="Drop1_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" SelectCommand="SELECT (ST.Stok_UrunAdi + ' [' + ADT.StokAdetTipi_adi + ']')[Stok_UrunAdi], ST.Stok_Id FROM [tblStok] ST left join tblStokAdetTipi ADT on (ADT.StokAdetTipi_Id=ST.Stok_AdetTipi)"> </asp:SqlDataSource>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Miktar</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="t4" runat="server" CssClass="form-control" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="t4arama" runat="server" ErrorMessage="Lütfen miktar alanını boş bırakmayınız." ControlToValidate="t4" Text="(*)" ForeColor="Red" ValidationGroup="group1"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Tarih</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="t5" runat="server" CssClass="form-control" data-mask="99/99/9999"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen tarih alanını boş bırakmayınız." ControlToValidate="t5" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="t5" Format="dd/MM/yyyy" />
                                    </div>
                                </div>
                                
                                <div class="form-actions text-right" runat="server" id="g_panel">
                                    <input type="reset" class="btn btn-danger" value="İptal">
                                    <asp:Button ID="kayit" runat="server" Text="Ekle" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="group1" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                                </div>
                            </div>
                        </div>
                    </div>


                    <!-- Ürün Listesi -->
                    <div class="col-md-7">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h6 class="panel-title"><i class="icon-rating3"></i>Ürün Listesi</h6>
                            </div>
                            <div class="datatablee">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Ürün Adı</th>
                                            <th style="width: 15%;">Adet</th>
                                            <th style="width: 15%;">Tarih</th>
                                            <th style="width: 15%; text-align: center;">İşlem</th>
                                        </tr>
                                    </thead>

                                    <asp:Repeater ID="rep1" runat="server" EnableViewState="True" OnItemCommand="rep1_ItemCommand" OnItemCreated="rep1_ItemCreated" ClientIDMode="AutoID">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("Stok_UrunAdi") %></td>
                                                <td><%#Eval("CariRevizyoSec_RevizyonAdet") %></td>
                                                <td><%# Convert.ToDateTime(Eval("CariRevizyoSec_Tarih")).ToString("dd.MM.yyyy") %></td>
                                                <td style="text-align: center">
                                                    <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-icon tip" data-original-title="Sil" CommandName="sil" CommandArgument='<%# Eval("CariRevizyoSec_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- SON -->
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="group1" ShowMessageBox="True" ShowSummary="False" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Yükleniyor İbaresi -->
    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <%=Sorgu.yukleniyor()%>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>


