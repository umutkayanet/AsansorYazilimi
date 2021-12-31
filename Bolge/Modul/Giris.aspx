<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Giris.aspx.cs" Inherits="Giris_Giris" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- js, css, tag dosyaları -->
    <title>Bölge Grup Yönetim Sistemi</title>
    <meta charset="utf-8" />
    <meta name="ROBOTS" content="NOINDEX, FOLLOW" />
    <meta name="ROBOTS" content="INDEX, NOFOLLOW" />
    <meta name="ROBOTS" content="NOINDEX, NOFOLLOW" />
    <meta name="googlebot" content="noindex" />
    <meta name="googlebot-news" content="nosnippet" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="/Desing/Default/css/bootstrap.min.css?v=8.0" rel="stylesheet" type="text/css" />
    <link href="/Desing/Default/css/londinium-theme.min.css?v=8.0" rel="stylesheet" type="text/css" />
    <link href="/Desing/Default/css/styles.min.css?v=8.0" rel="stylesheet" type="text/css" />
    <link href="/Desing/Default/css/icons.min.css?v=8.0" rel="stylesheet" type="text/css" />
    <link href="/Desing/Default/css/css.css?family=Open+Sans:400,300,600,700&amp;subset=latin,cyrillic-ext" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Desing/Default/js/jquery.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/jquery-ui.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/charts/sparkline.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/uniform.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/select2.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/inputmask.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/autosize.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/inputlimit.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/listbox.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/multiselect.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/validate.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/tags.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/switch.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/uploader/plupload.full.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/uploader/plupload.queue.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/wysihtml5/wysihtml5.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/forms/wysihtml5/toolbar.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/daterangepicker.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/fancybox.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/moment.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/jgrowl.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/datatables.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/colorpicker.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/fullcalendar.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/timepicker.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/plugins/interface/collapsible.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/bootstrap.min.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/application.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Default/js/rufdisable.js?v=8.0"></script>
    <script type="text/javascript" src="/Desing/Paket/kar_paket/kar.js?v=8.0"></script>
</head>
<body class="full-width page-condensed" id="body" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Üst Menü -->
        <div class="navbar navbar-inverse" style="background-color: rgba(0, 0, 0, 0.3);" role="navigation">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-right"><span class="sr-only">Toggle navbar</span><i class="icon-grid3"></i></button>
                <a class="navbar-brand" href="#">
                    <img src="/Desing/Default/images/sy_images/logo/logo1_png.png" alt="Su-Yapı" width="245" />
                </a>
            </div>
            <ul class="nav navbar-nav navbar-right collapse">
                <li><a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="icon-cogs"></i></a>
                    <ul class="dropdown-menu icons-right dropdown-menu-right">
                        <li><asp:LinkButton ID="resmi_unut" runat="server" OnClick="resmi_unut_Click" ValidationGroup="resmiunutgrup"><i class="icon-camera2"></i>Beni Unut</asp:LinkButton></li>
                        <li><a href="#"><i class="icon-cogs"></i>Şifre İste</a></li>

                    </ul>
                </li>

            </ul>
        </div>
        <!-- Son Üst Menü -->


        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!-- Giriş Alanı -->
                <div class="login-wrapper">
                    <div class="well2">
                        <div class="thumbnail">
                            <div class="thumb">
                                <asp:Image ID="kullanici_resmi" runat="server" />
                            </div>
                            <div class="caption text-center">
                                <h6>KULLANICI GİRİŞİ <small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h6>
                            </div>
                        </div>

                        <!-- Kullanıcı Formu -->
                        <div class="form-group has-feedback has-feedback-no-label">
                            <asp:TextBox ID="kullanici" runat="server" CssClass="form-control" placeholder="Kullanıcı Adınız"></asp:TextBox>
                            <i class="icon-user3 form-control-feedback"></i>
                            <asp:RequiredFieldValidator ID="kullanici_adi_sorgu" runat="server" ErrorMessage="Lütfen kullanıcı adı alanını boş bırakmayınız." ControlToValidate="kullanici" Text="(*)" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group has-feedback has-feedback-no-label">
                            <asp:TextBox ID="password" runat="server" CssClass="form-control" placeholder="Şifreniz" TextMode="Password"></asp:TextBox>
                            <i class="icon-lock form-control-feedback"></i>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen şifre alanını boş bırakmayınız." ControlToValidate="password" Text="(*)" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="row form-actions">
                            <div class="col-xs-6">
                                <div class="checkbox checkbox-success">
                                    <label>
                                        <asp:CheckBox ID="benihatirla" runat="server" CssClass="styled" Text="Beni Hatırla" Font-Size="Larger" />
                                    </label>
                                </div>
                            </div>
                            <div class="col-xs-6">
                                <asp:LinkButton ID="kullanici_giris" runat="server" CssClass="btn btn-danger pull-right" OnClick="kullanici_giris_Click"><i class="icon-menu2"></i>Giriş</asp:LinkButton>
                            </div>
                        </div>
                        <!-- Son Kullanıcı Formu -->
                    </div>
                </div>
                <div style="color: white; width: 100%; font-weight: bold; text-align: right; margin: 0 0 15px -20px; position: absolute; bottom: 0; display: block">8.0</div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
                <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                    <ProgressTemplate>
                        <%=Sorgu.yukleniyor()%>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
