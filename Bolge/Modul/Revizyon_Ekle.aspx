<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Revizyon_Ekle.aspx.cs" Inherits="Modul_Revizyon_Ekle" EnableEventValidation="false" %>

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

            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Revizyon Listesi<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü -->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Servis-Listesi">Servis Listesi</a></li>
                        <li class="active">Revizyon Listesi</li>
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


            <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                <a href="/Servis/Revizyon-Ekle/<%=RouteData.Values["id"]%>" class="btn btn-success"><i class="icon-plus-circle"></i>Yeni Revizyon Ekle</a>
            </div>

            <div class="form-horizontal form-bordered">
                <div class="row">
                    <!-- Revizyon Listesi -->
                    <div class="col-md-6">
                        <!-- Profesyonel Arama -->
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
                                                                    <asp:DropDownList ID="Asansor_Arama" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="Drop1_sql" Width="250px" DataTextField="CariSU_Tanimi" DataValueField="CariSU_Id">
                                                                        <asp:ListItem Selected="True" Value="0">Asansör Listesi</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>

                                                                <div class="col-sm-6">
                                                                    <asp:DropDownList ID="Durumu_arama" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" Width="250px" DataSourceID="durumu_sql" DataValueField="Durumu_Id" DataTextField="Durumu_Aciklama">
                                                                        <asp:ListItem Selected="True" Value="0">Durumu</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <span class="help-block"></span>
                                                                </div>
                                                                </div>
                                                                <div class="row">
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="tarih_arama" runat="server" CssClass="form-control" MaxLength="11" data-mask="99/99/9999" PlaceHolder="Revizyon Tarihi"></asp:TextBox>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="not_arama_text" runat="server" CssClass="form-control" MaxLength="11" PlaceHolder="Not"></asp:TextBox>
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
                        <!-- Profesyonel Arama -->


                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h6 class="panel-title"><i class="icon-cog"></i>Revizyon Listesi</h6>
                                <!-- Menü Buton -->
                                <div class="btn-group pull-right" id="popmen" runat="server">
                                    <button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><i class="icon-grid"></i><span class="caret"></span></button>
                                    <ul class="dropdown-menu dropdown-menu-right icons-right">
                                        <li>
                                            <asp:LinkButton ID="excel_export" runat="server" OnClick="excel_export_Click"><i class="icon-file-excel"></i>Excel'e Aktar</asp:LinkButton></li>
                                    </ul>
                                </div>
                                <!-- SON Menü Buton -->
                            </div>
                            <div class="datatablee">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Asansör</th>
                                            <th style="width: 15%;">Toplam Parça</th>
                                            <th style="width: 17%;">Durumu</th>
                                            <th style="width: 12%;">Tarih</th>
                                            <th style="width: 22%; text-align: center;">İşlem</th>
                                        </tr>
                                    </thead>

                                    <asp:Repeater ID="rep1" runat="server" EnableViewState="True" OnItemCommand="rep1_ItemCommand" OnItemDataBound="rep1_ItemDataBound" ClientIDMode="AutoID">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("Asansör Tanımı") %></td>
                                                <td><%#Eval("Toplam Parça") %></td>
                                                <td><%#Eval("Durumu_Style") %></td>
                                                <td><%#Convert.ToDateTime(Eval("Tarih")).ToString("dd.MM.yyyy") %></td>
                                                <td style="text-align: center">
                                                    <a href="/Servis/Revizyon-RenkEkle/<%=RouteData.Values["id"]%>/<%#Eval("CariRevizyonlar_Id") %>" title="Renk Ekle" class="btn btn-default btn-icon tip" data-original-title="Renk Ekle">
                                                        <i class="icon-tag3"></i>
                                                    </a>

                                                    <a href="/Servis/Revizyon-ParcaEkle/<%=RouteData.Values["id"]%>/<%#Eval("CariRevizyonlar_Id") %>" title="Parça Ekle" class="btn btn-default btn-icon tip" data-original-title="Parça Ekle">
                                                        <i class="icon-cog4"></i>
                                                    </a>
                                                    
                                                    <a href="/Servis/Revizyon-Ekle/<%=RouteData.Values["id"]%>/<%#Eval("CariRevizyonlar_Id") %>" title="Düzenle" class="btn btn-default btn-icon tip" data-original-title="Düzenle">
                                                        <i class="icon-pencil3"></i>
                                                    </a>
                                                    <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-icon tip" data-original-title="Sil" CommandName="sil" CommandArgument='<%# Eval("CariRevizyonlar_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </div>


                    <!-- Yeni Rol Ekleme / Rol Düzenleme -->
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading" runat="server" id="guncelle_div">
                                <h6 class="panel-title"><i class="icon-cog"></i>
                                    <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Revizyon Ekle"></asp:Label>
                                </h6>
                            </div>

                            <!-- Adı Soyadı Alanı-->
                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Asansör</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList ID="Drop1" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="Drop1_sql" Width="250px" DataTextField="CariSU_Tanimi" DataValueField="CariSU_Id">
                                            <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="val2" runat="server" ControlToValidate="Drop1" ErrorMessage="Lütfen asansör seçiminizi yapınız" InitialValue="0" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                        <asp:SqlDataSource ID="Drop1_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" SelectCommand="SELECT * FROM [tblCariAsansorler] WHERE ([CariSU_CariNo] = @CariSU_CariNo)">
                                            <SelectParameters>
                                                <asp:RouteParameter Name="CariSU_CariNo" RouteKey="id" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Revizyon Yapan Personel</label>
                                    <div class="col-sm-9">
                                        <asp:ListBox runat="server" ID="gorevli_personel" SelectionMode="Multiple" CssClass="multi-select-all-personel" DataSourceID="sql1" DataValueField="Persid" DataTextField="adsoyad"></asp:ListBox>
                                        <asp:SqlDataSource ID="sql1" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="<%$ ConnectionStrings:connect.ProviderName %>" SelectCommand="SELECT Persid, Pers_Soyadi, Pers_Adi, Pers_Adi + ' '+Pers_Soyadi+''[adsoyad] FROM [Personel]"></asp:SqlDataSource>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Revizyon Tarihi</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="t3" runat="server" CssClass="form-control" data-mask="99/99/9999"></asp:TextBox>
                                        <span class="help-block">Gün/Ay/Yıl
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen tarih alanını boş bırakmayınız." ControlToValidate="t3" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </span>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="t3" Format="dd/MM/yyyy" />
                                    </div>
                                </div>

                         
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Revizyon Notu</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="t4" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                        <br />Not: Kolay Filtreleme İçin #(hashtag) kullanmalısınız. Örnek #gidilemedi #ücret alınamadı
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Durumu</label>
                                    <div class="col-sm-9">
                                         <asp:RadioButtonList ID="revizyon_durumu" runat="server" CssClass="renk_radio" DataSourceID="durumu_sql" DataTextField="Durumu_Style" DataValueField="Durumu_Id" RepeatDirection="Horizontal"></asp:RadioButtonList>
                                         <asp:SqlDataSource ID="durumu_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="<%$ ConnectionStrings:connect.ProviderName %>" SelectCommand="SELECT * FROM [tblDurumu] where Durumu_Kategori='Revizyon' order by Durumu_Sira"></asp:SqlDataSource>                                        
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


