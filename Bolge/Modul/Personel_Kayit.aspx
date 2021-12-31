<%@ Page Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Personel_Kayit.aspx.cs" Inherits="Modul_Personel_Kayit" %>

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
                    <h3>Personel Kayıt <small>Lütfen aşağıdaki alanları eksiksiz doldurmaya özen gösteriniz.</small></h3>
                </div>

                <!-- Üst Sayfa Bilgilendirici Menü-->
                <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="javascript:window.close();">İnsan Kaynakları</a></li>
                        <li class="active">Personel Kayıt</li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->

            <!-- Resim vs Yükleme Butonları -->
            <div class="resimortala" style="width: 8%; height: 100%; display: table; margin: 0 0 0 0px">
                <div style="margin-bottom: -15px;">
                    <asp:Image ID="personel_resim" runat="server" /></div>
            </div>
            <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                <button type="button" class="btn btn-xs btn-success" onclick="window.location.href='/Personel-Resim/<%=RouteData.Values["id"]%>'"><i class="icon-camera2"></i>Resim Yükle</button>
                <button type="button" class="btn btn-xs btn-info" onclick="window.location.href='/Personel-Giris/<%=RouteData.Values["id"]%>'"><i class="icon-loop"></i>İşe Giriş / Çıkış Ekle</button>
                <button type="button" class="btn btn-xs btn-danger" onclick="window.location.href='/Personel-Yetki/<%=RouteData.Values["id"]%>'"><i class="icon-unlocked"></i>Yetki İşlemleri</button>
            </div>

            <div class="form-horizontal form-bordered">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h6 class="panel-title"><i class="icon-users"></i>Personel Bilgileri</h6>
                    </div>

                    <!-- Adı Soyadı Alanı-->
                    <div class="panel-body">
                        <div class="form-group">                           
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="adi_textbox" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        <span class="help-block">Adı
                                        <asp:RequiredFieldValidator ID="adi_sorgula" runat="server" ErrorMessage="Lütfen adı alanını boş bırakmayınız." ControlToValidate="adi_textbox" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>

                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="soyadi_textbox" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        <span class="help-block">Soyadı
                                        <asp:RequiredFieldValidator ID="soyadi_sorgu" runat="server" ErrorMessage="Lütfen soyadı alanını boş bırakmayınız." ControlToValidate="soyadi_textbox" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <!-- Kimlik Numarası Doğum Tarihi Cinsiyet -->
                        <div class="form-group">                           
                            <div class="col-sm-12">
                                <div class="row">
                                     <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="tc_texbox" runat="server" CssClass="form-control" MaxLength="11" data-mask="99999999999"></asp:TextBox>
                                        <span class="help-block">TC
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen TC Kimlik No Alanı Boş Bırakılamaz" ControlToValidate="tc_texbox" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="tc_texbox" ID="RegularExpressionValidator3" ValidationExpression="^[\s\S]{11,11}$" runat="server" ErrorMessage="TC Kimlik Numarası 11 Karakter Olmalıdır" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RegularExpressionValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^\d+$" ControlToValidate="tc_texbox" runat="server" ErrorMessage="TC Kimlik No Alanı Tam Sayı Olmalıdır." Display="Static" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RegularExpressionValidator></span>
                                    </div>

                                    <div class="col-sm-3 has-feedback">
                                        <asp:TextBox ID="dogumtarihi_textbox" runat="server" CssClass="form-control" data-mask="99/99/9999"></asp:TextBox>
                                        <span class="help-block">Doğum Tarihi : Gün/Ay/Yıl
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen doğum tarihi alanını boş bırakmayınız." ControlToValidate="dogumtarihi_textbox" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="dogumtarihi_textbox" ErrorMessage="Lütfen Doğrum Tarihi Alanını Kontrol Ediniz" Type="Date" MinimumValue="01/01/1930" MaximumValue="01/01/2000" Display="Dynamic" Text="(*)" ForeColor="Red"></asp:RangeValidator></span>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dogumtarihi_textbox" Format="dd/MM/yyyy" />
                                    </div>

                                    <div class="col-sm-3 has-feedback">
                                         <asp:DropDownList ID="cinsiyet_drop" runat="server" CssClass="select-search4">
                                             <asp:ListItem Selected="True" Value="9999">Seçiminiz</asp:ListItem>
                                             <asp:ListItem Value="0">Erkek</asp:ListItem>
                                             <asp:ListItem Value="1">Kadın</asp:ListItem>
                                             <asp:ListItem Value="2">Diğer</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="help-block">Cinsiyet
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="cinsiyet_drop" ErrorMessage="Lütfen cinsiyet seçiminizi yapınız" InitialValue="9999" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <!-- Mezuniyet Yılı, Eğitim, Gorev Alanı -->
                        <div class="form-group">
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <input type="text" id="mezuniyetyili_textbox" runat="server" class="form-control" data-mask="9999">
                                        <span class="help-block">Mezuniyet Yılı
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Lütfen mezuniyet yılı alanını boş bırakmayınız." ControlToValidate="mezuniyetyili_textbox" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>

                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="egitim_textbox" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="egitim_sql" DataTextField="Personel_Egitim" DataValueField="Personel_EgitimId" Width="250px">
                                            <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="egitim_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblPersonelEgitim]"></asp:SqlDataSource>

                                        <span class="help-block">Eğitim Durumu
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="egitim_textbox" ErrorMessage="Lütfen eğitim seçiminizi yapınız" InitialValue="0" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                    </div>


                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="gorev_drop" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="gorev_sql" DataTextField="Gorev" DataValueField="Gorev_id" Width="250px">
                                            <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="help-block">Görev
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="gorev_drop" ErrorMessage="Lütfen görev seçiminizi yapınız" InitialValue="0" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator></span>
                                        <asp:SqlDataSource ID="gorev_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblPersonelGorev]"></asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>
                        </div>


                        
                        <!-- Sisteme Giriş Bilgileri, Telefon -->
                        <div class="form-group">
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="kullaniciadi_textbox" runat="server" CssClass="form-control" MaxLength="49"></asp:TextBox>
                                        <span class="help-block">Kullanıcı Adı (Email Adresi)
                                        <asp:RegularExpressionValidator ID="kullaniciadi_sorgu" runat="server" ErrorMessage="Lütfen kullanıcı adı alanını email adresi yazınız." ControlToValidate="kullaniciadi_textbox" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Pers_Kayit" Text="(*)" ForeColor="Red"></asp:RegularExpressionValidator></span>
                                    </div>

                                    <div class="col-sm-3">
                                        <asp:TextBox ID="sifresi_textbox" runat="server" CssClass="form-control" MaxLength="16"></asp:TextBox>
                                        <span class="help-block">Şifresi</span>
                                    </div>

                                    <div class="col-sm-6 has-feedback">
                                        <asp:TextBox ID="sirket_gsm_textbox" runat="server" CssClass="form-control"  MaxLength="10" data-mask="9999999999"></asp:TextBox>
                                        <span class="help-block">Şirket Gsm Numarası : (başında sıfır olmadan)
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                ControlToValidate="sirket_gsm_textbox" ValidationGroup="Pers_Kayit"
                                                ErrorMessage="Lütfen gsm numarası alanını kontrol ediniz." ValidationExpression="[0-9]{10}"
                                                Text="(*)" ForeColor="Red"></asp:RegularExpressionValidator>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        

                        <!-- Personel Seçeneği -->
                        <div class="form-group">
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="checkbox-inline">
                                            <input id="sisteme_giris_izni" runat="server" type="checkbox" class="switch1 switch-small" data-on-label="<i class='icon-checkmark3'></i>" data-off-label="<i class='icon-cancel'></i>" checked="checked">
                                        </label>
                                        <span class="help-block">Sisteme Giriş İzni</span>
                                    </div>


                                    <div class="col-sm-2">
                                        <label class="checkbox-inline">
                                            <input id="personel_calisiyor" runat="server" type="checkbox" class="switch1 switch-small" data-on-label="<i class='icon-checkmark3'></i>" data-off-label="<i class='icon-cancel'></i>" checked="checked">
                                        </label>
                                        <span class="help-block">Personel Çalışıyor</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel ID="g_panel" runat="server">
                            <div class="form-actions text-right">
                                <a href="/Anasayfa/" class="btn btn-danger">İptal</a>
                                <asp:Button ID="kayit" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="kayit_Click" ValidationGroup="Pers_Kayit" OnClientClick="javascript:if(!confirm('Kayıt işlemini yapmak istediğinizden emin misiniz?'))return false;" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <!-- Son Kayıt Formu-->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Pers_Kayit" ShowMessageBox="True" ShowSummary="False" />


            <!-- Kayıt Esnasında Çıkan Uyarı Mesajı-->
            <div id="small_modal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title"><i class="icon-warning"></i>Uyarı!</h4>
                        </div>
                        <div class="modal-body with-padding">
                            <p>Kayıt işlemini tamamlamak istediğinizden eminmisiniz.</p>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-warning" data-dismiss="modal">İptal</button>
                            <a data-toggle="modal" role="button" href="#small_modal" class="btn btn-primary">Kaydet</a>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Son -->

            <!-- Yükleniyor İbaresi -->
            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                <ProgressTemplate>
                    <%=Sorgu.yukleniyor()%>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

