<%@ Page Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Cari_Kayit.aspx.cs" Inherits="Modul_Cari_Kayit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $(document).ready(function () {
                        $(".select-search4").select2({
                            width: "100%"
                        });
                    });

                    $(document).ready(function () {
                        $('.switch1').bootstrapSwitch();
                    });
                }
            </script>
            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Cari Kayıt <small>Lütfen aşağıdaki alanları eksiksiz doldurmaya özen gösteriniz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Cari-Listesi/<%=RouteData.Values["gorev"].ToString()%>">Cari Listesi</a></li>
                        <li class="active">Cari Bilgileri</li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->


            <div class="form-horizontal form-bordered">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6 class="panel-title"><i class="icon-users"></i>Cari Bilgileri</h6>
                    </div>

                    <!-- Adı Soyadı Alanı-->
                    <div class="panel-body">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Cari Kodu / Türü</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_Kodu" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        <span class="help-block">Cari Kodu
                                        <asp:RequiredFieldValidator ID="Cari_sorgula" runat="server" ErrorMessage="Lütfen Cari Kodu Alanını Boş Bırakmayınız." ControlToValidate="Cari_Kodu" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>

                                     <div class="col-sm-6 has-feedback">
                                         <asp:DropDownList ID="Cari_Turu_Drop" runat="server" CssClass="select-search4">
                                             <asp:ListItem Selected="True" Value="9999">Seçiminiz</asp:ListItem>
                                             <asp:ListItem Value="Alis">Alış</asp:ListItem>
                                             <asp:ListItem Value="Satis">Satış</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="help-block">Cari Türü
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="Cari_Turu_Drop" ErrorMessage="Lütfen Cari Türü Seçiminizi Yapınız" InitialValue="9999" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <!-- Kimlik Numarası Doğum Tarihi Cinsiyet -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Ünvan / Yetkili Kişi</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_Unvan" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                        <span class="help-block">Ünvan
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen Ünvan Alanını Boş Bırakmayınız." ControlToValidate="Cari_Unvan" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>

                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_Yetkili_Kisi" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                        <span class="help-block">Yetkili Kişi
                                        <asp:RequiredFieldValidator ID="soyadi_sorgu" runat="server" ErrorMessage="Lütfen Yetkili Kişi Alanını Boş Bırakmayınız." ControlToValidate="Cari_Yetkili_Kisi" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <!-- Kimlik Numarası Doğum Tarihi Cinsiyet -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Adres / Telefon</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_Adres" runat="server" CssClass="form-control" MaxLength="120"></asp:TextBox>
                                        <span class="help-block">Adres
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen Adres Alanını Boş Bırakmayınız." ControlToValidate="Cari_Adres" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>

                                    <div class="col-sm-3 has-feedback">
                                        <asp:TextBox ID="Cari_Telefon" runat="server" CssClass="form-control" MaxLength="11" data-mask="9999999999"></asp:TextBox>
                                        <span class="help-block">Telefon
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Lütfen Telefon Alanını Boş Bırakmayınız." ControlToValidate="Cari_Telefon" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>

                                    <div class="col-sm-3 has-feedback">
                                        <asp:TextBox ID="Cari_Telefon2" runat="server" CssClass="form-control" MaxLength="11" data-mask="9999999999"></asp:TextBox>
                                        <span class="help-block">Telefon 2</span>
                                    </div>
                                </div>
                            </div>
                        </div>


                         <!-- Kimlik Numarası Doğum Tarihi Cinsiyet -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Şehir / İlçe</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="sehir_drop" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="sehir_sql" DataTextField="Sehir_Adi" DataValueField="Sehirler_Id" Width="250px" OnSelectedIndexChanged="sehir_drop_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="help-block">Şehir
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="sehir_drop" ErrorMessage="Lütfen şehir seçiminizi yapınız" InitialValue="0" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                        <asp:SqlDataSource ID="sehir_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblSehirler]"></asp:SqlDataSource>
                                    </div>

                                    <div class="col-sm-6 has-feedback">
                                         <asp:DropDownList ID="Cari_İlce" runat="server" CssClass="select-search4" TabIndex="2" DataSourceID="ilce_sql" DataTextField="tblSIlceler_IlceAdi" DataValueField="tblSIlceler_Id" Width="250px" AppendDataBoundItems="True">
                                         <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                         </asp:DropDownList>
                                        <span class="help-block">İlçe
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Lütfen İlçe Alanını Boş Bırakmayınız." InitialValue="0" ControlToValidate="Cari_İlce" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                        <asp:SqlDataSource ID="ilce_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="form-group">
                            <label class="col-sm-2 control-label">Vergi Dairesi / Numarası</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_VergiDairesi" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                        <span class="help-block">Vergi Dairesi</span>                                        
                                    </div>

                                    <div class="col-sm-3 has-feedback">
                                        <asp:TextBox ID="Cari_VergiNumarasi" runat="server" CssClass="form-control" MaxLength="10" data-mask="9999999999"></asp:TextBox>
                                        <span class="help-block">Vergi Numarası   </span>                                     
                                    </div>

                                     <div class="col-sm-3 has-feedback">
                                        <asp:TextBox ID="Cari_KimlikNumarasi" runat="server" CssClass="form-control" MaxLength="10" data-mask="99999999999"></asp:TextBox>
                                        <span class="help-block">Kimlik Numarası</span>                                        
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label">Web Sitesi / Email</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_Web" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        <span class="help-block">Web Sitesi                           
                                    </div>

                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="Cari_EmailAdresi" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        <span class="help-block">Email Adresi 
                                        <asp:RegularExpressionValidator ID="kullaniciadi_sorgu" runat="server" ErrorMessage="Lütfen email adresi alanını kontrol ediniz." ControlToValidate="Cari_EmailAdresi" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Cari_Kayit" Text="(*)" ForeColor="Red"></asp:RegularExpressionValidator></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Cari Seçeneği -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Cari Durumu</label>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="checkbox-inline">
                                            <input id="Cari_Aktif" runat="server" type="checkbox" class="switch1 switch-small" data-on-label="<i class='icon-checkmark3'></i>" data-off-label="<i class='icon-cancel'></i>" checked="checked">
                                        </label>
                                        <span class="help-block">Cari Aktif</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="g_panel" runat="server">
                            <div class="form-actions text-right">
                                <a href="/Anasayfa/" class="btn btn-danger">İptal</a>
                                <asp:Button ID="kayit" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="Cari_Kayit" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <!-- Son Kayıt Formu-->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Cari_Kayit" ShowMessageBox="True" ShowSummary="False" />
            
            <!-- Yükleniyor İbaresi -->
            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                <ProgressTemplate>
                    <%=Sorgu.yukleniyor()%>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

