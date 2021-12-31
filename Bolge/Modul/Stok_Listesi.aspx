<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Stok_Listesi.aspx.cs" Inherits="Modul_Stok_Listesi" EnableEventValidation="false" ClientIDMode="AutoID" %>

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
                    <h3>Stok Listesi<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü -->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li class="active">Stok Listesi</li>
                    </ul>
                </div>
                <!-- Son -->
            </div>
            <!-- Son -->


            <!-- Yeni Stok Ekleme Butonu -->
            <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                <a href="/Stok-Listesi/" class="btn btn-success"><i class="icon-plus-circle"></i>Yeni Stok Ekle</a>
            </div>


            <div class="form-horizontal form-bordered">
                <div class="row">
                    <div class="col-md-7">
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
                                                    <div class="form-group" style="margin: 0px 0 -18px 0">
                                                        <div class="col-sm-12">
                                                            <div class="row">
                                                                <div class="col-sm-6">
                                                                    <asp:DropDownList ID="Marka_Arama" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="Marka_Sql" Width="250px" DataTextField="Marka_Adi" DataValueField="Marka_Id">
                                                                        <asp:ListItem Selected="True" Value="0">Marka Listesi</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>

                                                                <div class="col-sm-6 has-feedback">
                                                                    <asp:TextBox ID="Stok_Kodu" runat="server" CssClass="form-control" placeholder="Stok Kodu"></asp:TextBox>
                                                                    <span class="help-block"></span>
                                                                </div>
                                                            </div>

                                                             <div class="row">
                                                                 <div class="col-sm-6 has-feedback">
                                                                    <asp:TextBox ID="Urun_Adi" runat="server" CssClass="form-control" placeholder="Ürün Adı"></asp:TextBox>
                                                                </div>

                                                                <div class="col-sm-6 has-feedback">
                                                                    <asp:TextBox ID="stok" runat="server" CssClass="form-control" placeholder="Stoğu X'den Küçük"></asp:TextBox>
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
                                <h6 class="panel-title"><i class="icon-stack"></i>Stok Listesi</h6>
                            </div>

                            <div class="datatablee">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th style="width: 13%">Stok Kodu</th>
                                            <th>Ürün Adı</th>
                                            <th style="width: 10%">Toplam</th>
                                            <th style="width: 15%">Tipi</th>
                                            <th style="width: 17%">İşlem</th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater ID="stop_rep" runat="server" EnableViewState="True" OnItemCommand="stop_rep_ItemCommand" OnItemDataBound="stop_rep_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("Stok_UrunKodu") %></td>
                                                <td><%#Eval("Stok_UrunAdi") %></td>
                                                <td><%#Eval("KalanAdet") %></td>
                                                <td><%#Eval("StokAdetTipi_adi") %></td>
                                                <td>
                                                    <a href='/Stok-Listesi/<%#Eval("Stok_Id")%>' class="btn btn-default btn-xs btn-icon tip" data-original-title="Düzenle"><i class="icon-pencil3"></i></a>
                                                    <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-xs btn-icon tip" data-original-title="Sil" CommandName="sil" CommandArgument='<%# Eval("Stok_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </div>


                    <!-- Yeni Stok Ekleme / Stok Düzenleme -->
                    <div class="col-md-5">
                        <div class="panel panel-default">
                            <div class="panel-heading" runat="server" id="guncelle_div">
                                <h6 class="panel-title"><i class="icon-stack"></i>
                                    <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Stok Ekle"></asp:Label>
                                </h6>
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Stok Kodu</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="Text1" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Text1_Sorgula" runat="server" ErrorMessage="Lütfen Stok Kodu Alanını Boş Bırakmayınız." ControlToValidate="Text1" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Miktar Tipi</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="Adet_Drop" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="stoktipi_sql" DataTextField="StokAdetTipi_adi" DataValueField="StokAdetTipi_Id">
                                            <asp:ListItem Selected="True" Value="9999">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="Adet_Drop" ErrorMessage="Lütfen adet seçiminizi yapınız" InitialValue="9999" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:SqlDataSource ID="stoktipi_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblStokAdetTipi]"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Ürün Adı</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="Text2" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Text2_Sorgu" runat="server" ErrorMessage="Lütfen Ürün Adı Alanını Boş Bırakmayınız." ControlToValidate="Text2" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Marka</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="Marka_Drop" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="Marka_Sql" DataTextField="Marka_Adi" DataValueField="Marka_Id">
                                            <asp:ListItem Selected="True" Value="9999">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Marka_Drop" ErrorMessage="Lütfen marka seçiminizi yapınız" InitialValue="9999" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:SqlDataSource ID="Marka_Sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblMarkaListesi]"></asp:SqlDataSource>
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
    </asp:UpdatePanel>

    <!-- Yükleniyor İbaresi -->
    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <%=Sorgu.yukleniyor()%>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>


