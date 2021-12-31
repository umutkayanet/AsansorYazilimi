<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Revizyon_RenkEkle.aspx.cs" Inherits="Modul_Revizyon_RenkEkle" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $(document).ready(function () {
                        $(".styled, .multiselect-container input").uniform({ radioClass: 'choice', selectAutoWidth: false });
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
                    <h3>Asansör Renk Etiketi<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü -->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Servis-Listesi">Servis Listesi</a></li>
                        <li><a href="/Servis/Revizyon-Ekle/<%=RouteData.Values["id"].ToString()%>">Revizyon Listesi</a></li>
                        <li class="active">Renk Etiketi</li>
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
                <a href="/Servis/Revizyon-RenkEkle/<%=RouteData.Values["id"]%>/<%=RouteData.Values["idd"]%>" class="btn btn-success"><i class="icon-plus-circle"></i>Yeni Renk Etiketi Ekle</a>
            </div>


            <div class="form-horizontal form-bordered">
                <div class="row">

                    <!-- Yeni Rol Ekleme / Rol Düzenleme -->
                    <div class="col-md-5">
                        <div class="panel panel-default">
                            <div class="panel-heading" runat="server" id="guncelle_div">
                                <h6 class="panel-title"><i class="icon-rating3"></i>
                                    <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Renk Etiketi Ekle"></asp:Label>
                                </h6>
                            </div>

                            <!-- Adı Soyadı Alanı-->
                            <div class="panel-body">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Renk</label>
                                    <div class="col-sm-9">
                                        <asp:RadioButtonList ID="Drop1" runat="server" RepeatDirection="Horizontal" CssClass="renk_radio" RepeatColumns="2">
                                            <asp:ListItem Value="1" Selected="True"><span class="label label-danger">Kırmızı Etiket</span></asp:ListItem>
                                            <asp:ListItem Value="2"><span class="label label-success">&nbsp;Yeşil Etiket&nbsp;</span></asp:ListItem>
                                            <asp:ListItem Value="3"><span class="label label-info">&nbsp;Mavi Etiket&nbsp;</span></asp:ListItem>
                                            <asp:ListItem Value="4"><span class="label label-warning">&nbsp;Sarı Etiket&nbsp;</span></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Belge Tarihi</label>
                                    <div class="col-sm-9">
                                       <asp:TextBox ID="belge_tarihi" runat="server" CssClass="form-control" MaxLength="11" data-mask="99/99/9999" PlaceHolder="Belge Tarihi"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen belge tarihi alanını boş bırakmayınız." ControlToValidate="belge_tarihi" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator> 
                                    </div>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="belge_tarihi" Format="dd/MM/yyyy" />
                                </div>


                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Son Etiket Tarihi</label>
                                    <div class="col-sm-9">
                                       <asp:TextBox ID="sonetikettarihi" runat="server" CssClass="form-control" MaxLength="11" data-mask="99/99/9999" PlaceHolder="Son Etiket Tarihi"></asp:TextBox>
                                    </div>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="sonetikettarihi" Format="dd/MM/yyyy" />
                                </div>


                                <div class="form-group">
                                    <label class="col-sm-3 control-label">Belge Yükle</label>
                                    <div class="col-sm-9">
                                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="styled" Style="user-select: none;" />
                                        <asp:RequiredFieldValidator ID="FileUpload1_kontrol" runat="server" ErrorMessage="Lütfen dosya seç alanını boş bırakmayınız." Text="(*)" ForeColor="Red" ValidationGroup="group1" ControlToValidate="FileUpload1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Dosya pdf yada resim olmalıdır." ValidationExpression="^.+(.pdf|.PDF|.png|.PNG|.jpg|.JPG)$" ControlToValidate="FileUpload1" ValidationGroup="group1" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        <asp:HiddenField ID="yuklu_belge" runat="server" Value="" />
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
                                <h6 class="panel-title"><i class="icon-rating3"></i>Etiket Listesi</h6>
                            </div>
                            <div class="datatablee">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Etiket Rengi</th>
                                            <th style="width: 15%;">Belge Tarihi</th>
                                            <th style="width: 15%;">Son Tarihi</th>
                                            <th style="width: 15%; text-align: center;">İşlem</th>
                                        </tr>
                                    </thead>

                                    <asp:Repeater ID="rep1" runat="server" EnableViewState="True" OnItemCommand="rep1_ItemCommand" OnItemCreated="rep1_ItemCreated" ClientIDMode="AutoID">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%#Eval("renk") %>
                                                    <asp:HiddenField ID="repeat_yuklu_belge" runat="server" Value='<%#Eval("CRE_Dosya") %>' />
                                                </td>
                                                <td><%#Convert.ToDateTime(Eval("CRE_BelgeTarihi")).ToString("dd.MM.yyyy") %></td>
                                                <td><%#Convert.ToDateTime(Eval("CRE_EtiketSonTarih")).ToString("dd.MM.yyyy") %></td>
                                                <td style="text-align: center">
                                                    
                                                    <a href="/Data/Asansor_RB/<%#Eval("CRE_Dosya") %>" target="_blank" class="btn btn-default btn-icon tip" data-original-title="Dosya Aç">
                                                        <i class="icon-screen"></i>
                                                    </a>
                                                    
                                                    <a href="/Servis/Revizyon-RenkEkle/<%=RouteData.Values["id"]%>/<%#RouteData.Values["idd"]%>/<%#Eval("CRE_Id") %>" title="Düzenle" class="btn btn-default btn-icon tip" data-original-title="Düzenle">
                                                        <i class="icon-pencil3"></i>
                                                    </a>

                                                    <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-icon tip" data-original-title="Sil" CommandName="sil" CommandArgument='<%# Eval("CRE_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
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

                <!-- Yükleniyor İbaresi -->
                <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                    <ProgressTemplate>
                        <%=Sorgu.yukleniyor()%>
                    </ProgressTemplate>
                </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="kayit" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


