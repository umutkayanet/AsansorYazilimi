<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Personel_Yillik_Izin.aspx.cs" Inherits="Modul_Personel_Yillik_Izin" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script>
                function aaa() {
                    //popgunsayisi
                    var txtMaintCost = $('input[id=<%=popgunsayisi.ClientID%>]').val();
                    var n = txtMaintCost.indexOf("-");
                    if (txtMaintCost.length >= 1) {

                        if (n != "-1") {
                            document.getElementById('<%=bbb.ClientID %>').style.color = "red";
                            document.getElementById('<%=bbb.ClientID %>').innerHTML = 'Gün sayısı toplam izin gün sayından düşülecek.';
                        }
                        else {
                            document.getElementById('<%=bbb.ClientID %>').style.color = "green";
                            document.getElementById('<%=bbb.ClientID %>').innerHTML = 'Gün sayısı toplam izin gün sayısına eklenecek.';
                        }
                    }
                    else {
                        document.getElementById('<%=bbb.ClientID %>').innerHTML = '';
                    }
                }
            </script>
            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Yıllık İzin<small>Personelin yıllık, ücretli, ücretsiz, rapor vs tüm izin işlemlerini aşağıda düzenleyebilirsiniz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Personel-Listesi">Personel Listesi</a></li>
                        <li>Yıllık İzin</li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->


            <!-- Kullanıcı Bilgi Menüsü -->
            <div class="row">
                <div class="col-md-6">
                    <div class="form-horizontal form-separate" role="form">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h6 class="panel-title"><i class="icon-users"></i>Personel Bilgileri</h6>
                            </div>

                            <div class="panel-body">
                                <div class="form-group" style="padding-bottom: 4px; padding-top: 8px; border: 0px">
                                    <!-- Adı Soyadı Başlık Alanı -->
                                    <label class="col-md-2 control-label" style="padding: 0 0 0 10px">
                                        <b>Adı Soyadı</b>
                                    </label>
                                    <div class="col-md-4">
                                        <asp:Label ID="adi_soyadi_label" runat="server" Text=""></asp:Label>
                                    </div>

                                    <!-- Doğum tarihi Başlık alanı -->
                                    <label class="col-md-2 control-label" style="padding: 0 0 0 10px; border: 0px">
                                        <b>Doğrum Tarihi</b>
                                    </label>
                                    <div class="col-md-4">
                                        <asp:Label ID="dogum_tarihi_label" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                                <br /><br /><br /><br />

                                <div class="form-group" style="padding-bottom: 4px; padding-top: 8px; border: 0px">
                                    <!-- Yaş Başlık Alanı -->
                                    <label class="col-md-2 control-label" style="padding: 0 0 0 10px">
                                        <b>Yaşı</b>
                                    </label>
                                    <div class="col-md-4">
                                        <asp:Label ID="yas_label" runat="server" Text=""></asp:Label>
                                    </div>

                                    <!-- İşe Giriş Tarihi Başlık alanı -->
                                    <label class="col-md-2 control-label" style="padding: 0 0 0 10px">
                                        <b>İşe Giriş Tarihi</b>
                                    </label>
                                    <div class="col-md-4">
                                        <asp:Label ID="ise_giris_tarihi_label" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Personel Resim Alanı-->
                <div class="col-md-2">
                    <div class="form-horizontal form-separate" role="form">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h6 class="panel-title"><i class="icon-camera2"></i>Personel Resim</h6>
                            </div>

                            <div class="panel-body">
                                <div class="resimortala">
                                    <asp:Image ID="personel_resim" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- SON Personel Resim Alanı-->
            </div>
            <!-- Kullanıcı Bilgi Menüsü Son -->


            <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel">
                <asp:LinkButton ID="yeni_kayit" runat="server" CssClass="btn btn-xs btn-success" OnClick="yeni_kayit_Click"><i class="icon-sun"></i>Yeni Kayıt</asp:LinkButton>
            </div>


            <!-- Personel İzin Listesi -->
            <div class="tab-content" style="border: 0px solid red;">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6 class="panel-title"><i class="icon-table"></i>Personel İzin Çizelgesi</h6>
                    </div>
                    <!-- asp.net update panelin düzgün çalışabilmesi için gerekli kod -->
                    <script type="text/javascript">
                        function pageLoad() {
                            $(document).ready(function () {
                                $(document).ready(function () {
                                    $(".select-search2").select2({
                                        width: "100%"
                                    });

                                    $(".select-search").select2({
                                        width: "100%"
                                    });
                                });

                                oTable = $('.datatablee table').dataTable({
                                    "aLengthMenu": [[100, 200, -1], [100, 200, "Tümünü Listele"]],
                                    "iDisplayLength": 100,
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
                        <table class="table">
                            <thead>
                                <tr>
                                    <th style="width: 15%">Başlama Tarihi</th>
                                    <th style="width: 15%">Bitiş Tarihi</th>
                                    <th style="width: 15%">Açıklama</th>
                                    <th style="width: 15%">Gün Sayısı</th>
                                    <th style="width: 15%">Kalan İzin</th>
                                    <th>Not</th>
                                    <th style="width: 8.5%">İşlem</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="personel_izin_rep" runat="server" EnableViewState="True" OnItemCommand="Repeater1_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Convert.ToDateTime(Eval("YillikIzin_Baslama_Tarihi")).ToString("dd.MM.yyyy") %>
                                            </td>
                                            <td>
                                                <%#Eval("YillikIzin_Bitis_Tarihi").ToString() == ""  ? "" : Convert.ToDateTime(Eval("YillikIzin_Bitis_Tarihi")).ToString("dd.MM.yyyy")%>
                                            </td>
                                            <td>
                                                <asp:Label ID="Aciklama" runat="server" Text='<%#Eval("YillikIzinAc_Aciklama") %>'></asp:Label>
                                                <asp:HiddenField ID="Aciklama_kod" runat="server" Value='<%#Eval("YillikIzin_Aciklama_Id") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="Gun_Sayisi" runat="server" Text='<%#Eval("YillikIzin_Gun_Sayisi") %>'></asp:Label>
                                                <asp:HiddenField ID="Gun_Sayma" runat="server" Value='<%#Eval("YillikIzinAc_Gun_Sayma") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="kalan_izin_label" runat="server" Text=""></asp:Label></td>
                                            <td>
                                                <%#Eval("YillikIzin_Not") %>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName='Duzenle' CommandArgument='<%#Eval("YillikIzin_Id") %>'>Düzenle</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <!-- Personel İzin Listesi Son -->




            <!-- Açılır Pop Menü -->
            <div style="display: none">
                <asp:Button ID="Button1" runat="server" Text="" />
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button1" BackgroundCssClass="modalBackground" PopupControlID="Panel1" CancelControlID="ccc"></ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="Panel1" runat="server">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title"><i class="icon-paragraph-justify2"></i>Personel İzin Bilgileri</h4>
                        </div>


                        <div class="modal-body with-padding">
                            <!-- Başlangıç Bitiş Tarihleri -->
                            <div class="form-group">
                                <asp:Label ID="pop_mesaj" runat="server" Text="" ForeColor="Red"></asp:Label>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <label>Başlama Tarihi</label>
                                        <asp:HiddenField ID="guncelle_id" runat="server" />
                                        <asp:TextBox ID="popbaslamatarihi" runat="server" CssClass="form-control" data-mask="99/99/9999"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Lütfen bitiş tarihi alanını boş bırakmayınız." ControlToValidate="popbaslamatarihi" ValidationGroup="yillik_izin_guncelle" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="popbaslamatarihi" ErrorMessage="Lütfen Başlama Tarihi Alanını Kontrol Ediniz" Type="Date" ValidationGroup="yillik_izin_guncelle" MinimumValue="01/01/1964" MaximumValue="01/01/2050" Display="Dynamic" Text="(*)" ForeColor="Red"></asp:RangeValidator></span>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="control-label">Bitiş Tarihi</label>
                                        <asp:TextBox ID="popbitistarihi" runat="server" CssClass="form-control" data-mask="99/99/9999"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <!-- Diğer bilgileri -->
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <label>Açıklama</label>
                                        <div style="z-index: 9999; padding: 7px 0 0 0px">
                                            <asp:DropDownList ID="DropDownList1" runat="server" TabIndex="2" AppendDataBoundItems="True" DataSourceID="izin_aciklama_sql" DataTextField="YillikIzinAc_Aciklama" DataValueField="YillikIzinAc_Id" Width="100%">
                                                <asp:ListItem Value="0" Selected="True">Seçiminiz</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="izin_aciklama_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" SelectCommand="SELECT [YillikIzinAc_Id], [YillikIzinAc_Aciklama] FROM [tblYillikIzinAciklama] ORDER BY [YillikIzinAc_Sira]"></asp:SqlDataSource>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="DropDownList1" ErrorMessage="Lütfen açıklama seçiminizi yapınız." InitialValue="0" ValidationGroup="yillik_izin_guncelle" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Gün Sayısı</label>
                                        <asp:TextBox ID="popgunsayisi" runat="server" CssClass="form-control" autocomplete="off" onKeyDown="aaa()" onKeyPress="aaa()" onKeyUp="aaa()"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="popgunsayisi" ErrorMessage="Gün sayısı alanı boş bırakılamaz" ForeColor="Red" ValidationGroup="yillik_izin_guncelle">(*)</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RangeValidator1" runat="server" ControlToValidate="popgunsayisi" Display="Dynamic" ErrorMessage="(*)" ForeColor="Red"
                                            ValidationExpression="^[+-]?\d{1,18}(\.\d{1,2})?$"
                                            SetFocusOnError="True" ValidationGroup="yillik_izin_guncelle"></asp:RegularExpressionValidator>
                                        <br />
                                        <asp:Label ID="bbb" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label>Not</label>
                                            <asp:TextBox ID="popnot" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="border: 0px solid black">
                                <button class="btn btn-warning" data-dismiss="modal" id="ccc">Kapat</button>
                                <asp:Button ID="Duzenle_Button" runat="server" Text="Kayıt" CssClass="btn btn-primary" OnClick="Duzenle_Button_Click" ValidationGroup="yillik_izin_guncelle" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <!-- Açılır Pop Menü Son -->

            <!-- Yükleniyor İbaresi -->
            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                <ProgressTemplate>
                    <%=Sorgu.yukleniyor()%>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="margin: 0 0 200px 0px"></div>
</asp:Content>


