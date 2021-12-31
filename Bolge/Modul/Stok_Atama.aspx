<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Stok_Atama.aspx.cs" Inherits="Modul_Stok_Atama" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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

                    $(function () {
                        $("#tblCustomers [id*=chkHeader]").click(function () {
                            if ($(this).is(":checked")) {
                                $("#tblCustomers [id*=chkRow]").attr("checked", "checked");
                            } else {
                                $("#tblCustomers [id*=chkRow]").removeAttr("checked");
                            }
                        });
                        $("#tblCustomers [id*=chkRow]").click(function () {
                            if ($("#tblCustomers [id*=chkRow]").length == $("#tblCustomers [id*=chkRow]:checked").length) {
                                $("#tblCustomers [id*=chkHeader]").attr("checked", "checked");
                            } else {
                                $("#tblCustomers [id*=chkHeader]").removeAttr("checked");
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
                    <h3>Cari Stok Atama<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>

                        <li><a href="#">Cari Stok Atama</a></li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->


            <!-- Roller Bölümü -->
            <asp:Panel ID="Panel1" runat="server">
                <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                    <a href="/Stok-Atama" class="btn btn-success"><i class="icon-unlocked2"></i>Yeni Stok Ekle</a>
                </div>

                <div class="form-horizontal form-bordered">
                    <div class="row">
                        <!-- Rol Listesi -->
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
                                                                    <div class="col-sm-3 has-feedback">
                                                                        <asp:DropDownList ID="ara_unvan" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="unvan_sql" DataTextField="Cari_Unvan" DataValueField="Cari_Id" Width="250px">
                                                                            <asp:ListItem Selected="True" Value="0">Ünvan</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <span class="help-block"></span>
                                                                    </div>


                                                                    <div class="col-sm-3 has-feedback">
                                                                        <asp:DropDownList ID="urun_adi_ara" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="tblStok_sql" DataTextField="Stok_UrunAdi" DataValueField="Stok_Id" Width="250px">
                                                                            <asp:ListItem Selected="True" Value="0">Ürün Adı</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>

                                                                </div>
                                                                <div class="form-actions text-right" style="margin-top: 10px">
                                                                    <a href="/Anasayfa/" class="btn btn-danger">İptal</a>
                                                                    <asp:Button ID="ara" runat="server" Text="Ara" CssClass="btn btn-primary" OnClick="ara_Click"/>
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

                                    <div class="btn-group pull-right" id="popmen" runat="server">
                                        <button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><i class="icon-grid"></i><span class="caret"></span></button>
                                        <ul class="dropdown-menu dropdown-menu-right icons-right">
                                            <li><asp:LinkButton ID="excel_export" runat="server" OnClick="excel_export_Click"><i class="icon-file-excel"></i>Excel'e Aktar</asp:LinkButton></li>
                                        </ul>
                                    </div>
                                </div>

                                <div class="datatablee">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th style="width: 20%;">Ünvan</th>
                                                <th>Ürün Adı
                                                </th>
                                                <th style="width: 10%;">Miktar</th>
                                                <th style="width: 10%;">Adet Tipi</th>
                                                <th style="width: 10%;">Birim Fiyat</th>
                                                <th style="width: 10%;">Tarih</th>
                                                <th style="width: 12%; text-align: center;">İşlem</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rep1" runat="server" EnableViewState="True" OnItemCommand="rep1_ItemCommand" OnItemDataBound="rep1_ItemDataBound" ClientIDMode="AutoID">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%#Eval("Cari_Unvan") %></td>
                                                        <td><%#Eval("Stok_UrunAdi") %></td>
                                                        <td><%#Eval("CariStokKayit_Adet") %></td>
                                                        <td><%#Eval("StokAdetTipi_adi") %></td>
                                                        <td><%# Convert.ToDecimal(Eval("CariStokKayit_Fiyat") ).ToString("N")%></td>
                                                        <td><%#Convert.ToDateTime(Eval("CariStokKayit_KayitTarih")).ToString("dd.MM.yyyy") %></td>
                                                        <td style="text-align: center">
                                                            <a href="/Stok-Atama/<%=RouteData.Values["id"]%>/<%#Eval("CariStokKayit_Id") %>" title="Düzenle" class="btn btn-default btn-icon tip" data-original-title="Düzenle">
                                                                <i class="icon-pencil3"></i>
                                                            </a>
                                                            <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-icon tip" data-original-title="Sil" CommandName="sil" CommandArgument='<%# Eval("CariStokKayit_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>


                        <!-- Yeni Rol Ekleme / Rol Düzenleme -->
                        <div class="col-md-5">
                            <div class="panel panel-default">
                                <div class="panel-heading" runat="server" id="guncelle_div">
                                    <h6 class="panel-title"><i class="icon-plus-circle"></i>
                                        <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Ürün Ekle"></asp:Label>
                                    </h6>
                                </div>

                                <!-- Adı Soyadı Alanı-->
                                <div class="panel-body">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Ünvan</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="unvan_drop" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="unvan_sql" DataTextField="Cari_Unvan" DataValueField="Cari_Id" Width="250px">
                                                <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="unvan_drop" ErrorMessage="Lütfen ünvan seçiminizi yapınız" InitialValue="0" ValidationGroup="Group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                            <asp:SqlDataSource ID="unvan_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="Select Cari_Id, Cari_Unvan from tblCariKayit where Cari_Turu='Alis'"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Ürün Adı</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="urun_drop" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="tblStok_sql" DataTextField="Stok_UrunAdi" DataValueField="Stok_Id" Width="250px">
                                                <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="urun_drop" ErrorMessage="Lütfen ürün seçiminizi yapınız" InitialValue="0" ValidationGroup="Group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                            <asp:SqlDataSource ID="tblStok_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblStok]"></asp:SqlDataSource>
                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Miktar</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="t2" runat="server" CssClass="form-control" MaxLength="50" onkeypress="return alphakont(event, numbers)" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen adet alanını boş bırakmayınız." ControlToValidate="t2" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-2 control-label">Birim Fiyat</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="t3" runat="server" CssClass="form-control" MaxLength="50" onkeypress="return alphakont(event, money)"></asp:TextBox>

                                            <p></p>
                                            <div class="callout callout-danger fade in">
                                                <button type="button" class="close" data-dismiss="alert">×</button>
                                                <p>Örnek Kullanımlar : </p>  160,30 - 10.125,50 - 125.850,25
                                            </div>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen birim fiyat alanını boş bırakmayınız." ControlToValidate="t3" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
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
                </div>
            </asp:Panel>
            <!-- SON -->
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


