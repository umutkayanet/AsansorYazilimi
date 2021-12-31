<%@ Page Title="" Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Asansor_Ekle.aspx.cs" Inherits="Modul_Asansor_Ekle" %>

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
                }
            </script>

            <!-- Başlık -->
            <div class="page-header">
                <div class="page-title">
                    <h3>Asansör Listesi<small>Lütfen aşağıdaki alanları eksiksiz doldurunuz.</small></h3>
                </div>
                <!-- Üst Sayfa Bilgilendirici Menü-->
                 <div class="breadcrumb-line">
                    <ul class="breadcrumb">
                        <li><a href="/Anasayfa/">Ana Sayfa</a></li>
                        <li><a href="/Servis-Listesi">Servis Listesi</a></li>
                        <li class="active">Asansör Listesi</li>
                    </ul>
                </div>


                <!-- Cari Bilgilerini Ekrana Yaz -->
                <div class="breadcrumb-line" style="margin-right: 5px; font-size: 12px;">
                    <ul class="breadcrumb">
                        <li><asp:Label ID="Cari_Label" runat="server" Text="" ForeColor="#1fa8b7"></asp:Label></li>
                    </ul>
                </div>
                <!-- Son Üst Sayfa Bilgilendirici Menü-->
            </div>
            <!-- Son Başlık -->


            <!-- Roller Bölümü -->
            <asp:Panel ID="Panel1" runat="server">
                <div class="tablo_uzeri_buton" runat="server" id="buttonlar_panel" visible="false">
                    <a href="/Servis/Asansor-Ekle/<%=RouteData.Values["id"]%>" class="btn btn-success"><i class="icon-plus-circle"></i>Yeni Asansör Ekle</a>
                </div>

                <div class="form-horizontal form-bordered">
                    <div class="row">
                        <!-- Rol Listesi -->
                        <div class="col-md-7">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h6 class="panel-title"><i class="icon-sort"></i>Asansör Listesi</h6>
                                </div>


                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th style="width: 14%;">Kimlik No</th>
                                            <th>Tanımı</th>
                                            <th style="width: 22%;">Cinsi</th>
                                            <th style="width: 17%;">Kayıt Tarihi</th>
                                            <th style="width: 15%; text-align: center;">İşlem</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rep1" runat="server" EnableViewState="True" OnItemCommand="rep1_ItemCommand" OnItemDataBound="rep1_ItemDataBound" ClientIDMode="AutoID">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="duyuruid" runat="server" Value='<%#Eval("CariSU_Id") %>' />
                                                        <%#Eval("CariSu_KimlikNo") %>
                                                    </td>
                                                    <td><%#Eval("CariSu_Tanimi") %></td>
                                                    <td><%#Eval("AsansorCinsi_Detay") %></td>
                                                    <td><%#Convert.ToDateTime(Eval("CariSU_Tarih")).ToString("dd.MM.yyyy") %></td>
                                                    <td style="text-align: center">
                                                        <a href="/Servis/Asansor-Ekle/<%=RouteData.Values["id"]%>/<%#Eval("CariSU_Id") %>" title="Düzenle" class="btn btn-default btn-icon tip" data-original-title="Düzenle">
                                                            <i class="icon-pencil3"></i>
                                                        </a>
                                                        <asp:LinkButton ID="sil" runat="server" CssClass="btn btn-default btn-icon tip" data-original-title="Sil"  CommandName="sil" CommandArgument='<%# Eval("CariSU_Id") %>' OnClientClick="javascript:if(!confirm('Kaydı silmek istediğinizden eminmisiniz.'))return false;"><i class="icon-remove3"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>


                        <!-- Yeni Rol Ekleme / Rol Düzenleme -->
                        <div class="col-md-5">
                            <div class="panel panel-default">
                                <div class="panel-heading" runat="server" id="guncelle_div">
                                    <h6 class="panel-title"><i class="icon-plus-circle"></i>
                                        <asp:Label ID="yeni_kayit_buton" runat="server" Text="Yeni Asansör Ekle"></asp:Label>
                                    </h6>
                                </div>

                                <!-- Adı Soyadı Alanı-->
                                <div class="panel-body">

                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Kimlik Numarası</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="t2" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen kimlik numarası alanını boş bırakmayınız." ControlToValidate="t2" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Asansör Tanımı</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="t3" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Lütfen asansör tanımı alanını boş bırakmayınız." ControlToValidate="t3" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Asansör Cinsi</label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList ID="asansor_cinsi" runat="server" CssClass="select-search4" TabIndex="2" AppendDataBoundItems="True" DataSourceID="asansorcinsi_sql" DataTextField="AsansorCinsi_Detay" DataValueField="AsansorCinsi_Id" Width="250px">
                                                <asp:ListItem Selected="True" Value="0">Seçiminiz</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="asansor_cinsi" ErrorMessage="Lütfen asansör tipi seçiminizi yapınız" InitialValue="0" ValidationGroup="group1" Text="(*)" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:SqlDataSource ID="asansorcinsi_sql" runat="server" ConnectionString="<%$ ConnectionStrings:connect %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [tblAsansorCinsi]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    

                                    <div class="form-group">
                                        <label class="col-sm-3 control-label">Not</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="asansor_not" runat="server" CssClass="form-control" MaxLength="200" TextMode="MultiLine"></asp:TextBox>                                            
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
    </asp:UpdatePanel>

    <!-- Yükleniyor İbaresi -->
    <asp:UpdateProgress ID="UpdateProgress3" runat="server">
        <ProgressTemplate>
            <%=Sorgu.yukleniyor()%>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>


