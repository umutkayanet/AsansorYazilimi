<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Servis_Hatirlatma.ascx.cs" Inherits="Blok_Servis_Hatirlatma" %>

<style type="text/css">
    .datatable-scroll {
        height: 250px;
    }

    .panel .dataTables_filter {
        width: 100%;
    }

    .datatable-header label {
        width: 100%;
    }

    .dataTables_filter label > input[type=text] {
        width: 100%;
    }
</style>



<div class="row">
    <div class="panel panel-default kutugolgefekti">
        <div class="panel-heading">
            <h6 class="panel-title"><i class="icon-clock3"></i>Hatırlatmalar</h6>
        </div>
        <div class="panel-body">
            <div class="tabbable">
                <ul class="nav nav-pills nav-justified">
                    <li class="active">
                        <a href="#panel-pill1" data-toggle="tab">
                            <i class="icon-wave"></i>Bakım 
                            <span class="label label-danger">
                                <asp:Label ID="bakim_adet" runat="server" Text="0"></asp:Label>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#panel-pill2" data-toggle="tab">
                            <i class="icon-cog"></i>Revizyon 
                            <span class="label label-danger">
                                <asp:Label ID="revizyon_adet" runat="server" Text="0"></asp:Label>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#panel-pill3" data-toggle="tab">
                            <i class="icon-tools"></i>Arıza 
                            <span class="label label-danger">
                                <asp:Label ID="Ariza_Adet" runat="server" Text="0"></asp:Label>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#panel-pill4" data-toggle="tab">
                            <i class="icon-file6"></i>Sözleşme 
                            <span class="label label-danger">
                                <asp:Label ID="sozlesme_adet" runat="server" Text="0"></asp:Label>
                            </span>
                        </a>
                    </li>
                </ul>



                <div class="tab-content pill-content ">

                    <!-- Bakım Hatırlatmaları-->
                    <div class="tab-pane fade in active" id="panel-pill1">
                        <div class="servishatirlatma">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>Ünvan</th>
                                        <th style="width: 300px">Bakım Türü</th>
                                        <th style="width: 100px">Tarihi</th>
                                    </tr>
                                </thead>

                                <asp:Repeater ID="bakim_hat_rep" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="/Servis/Bakim-Ekle/<%#Eval("CariSU_CariNo") %>/<%#Eval("CariBakimlar_Id") %>" onclick="window.open(this.href, '_blank');return false;" class="tip" data-original-title="<%#Eval("CariSu_Tanimi") %>"><%#Eval("Cari_Unvan") %></a></td>
                                            <td><%#Eval("CariSU_Tanimi")%></td>
                                            <td><%#Convert.ToDateTime(Eval("CariBakimlar_Tarih")).ToString("dd.MM.yyyy") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>


                    <!-- Revizyon Hatırlatmaları -->
                    <div class="tab-pane fade" id="panel-pill2">
                        <div class="servishatirlatma">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>Ünvan</th>
                                        <th style="width: 19%">Renk Etiketi</th>
                                        <th style="width: 19%">Belge Tarihi</th>
                                        <th style="width: 19%">Son Etiket Tarihi</th>
                                    </tr>
                                </thead>

                                <asp:Repeater ID="revizyon_hat_rep" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <a href="/Servis/Revizyon-Ekle/<%#Eval("Cari_Id") %>/<%#Eval("CRE_RevizyonId") %>" onclick="window.open(this.href, '_blank');return false;" class="tip" data-original-title="<%#Eval("CariSu_Tanimi") %>">
                                                    <%#Eval("Cari_Unvan") %>
                                                </a>
                                            </td>
                                            <td>
                                                <a href="/Servis/Revizyon-RenkEkle/<%#Eval("Cari_Id") %>/<%#Eval("CRE_RevizyonId") %>/<%#Eval("CRE_Id") %>" onclick="window.open(this.href, '_blank');return false;" class="tip" data-original-title="<%#Eval("CariSu_Tanimi") %>">
                                                <%# Eval("CRE_Etiket").ToString() == "1" ? "<div style='width: 50%; height:20px; background-color: #D65C4F;'></div>" : Eval("CRE_Etiket").ToString() == "2" ? "<div style='width: 50%; height:10px; background-color: #65B688;'></div>" : "" %>
                                                <%# Eval("CRE_Etiket").ToString() == "3" ? "<div style='width: 50%; height:20px; background-color: #3CA2BB;'></div>" : Eval("CRE_Etiket").ToString() == "4" ? "<div style='width: 50%; height:10px; background-color: #E7804F;'></div>" : "" %>
                                                </a>
                                            </td>

                                            <td><%#Convert.ToDateTime(Eval("CRE_BelgeTarihi")).ToString("dd.MM.yyyy") %></td>
                                            <td><%#Convert.ToDateTime(Eval("tarih")).ToString("dd.MM.yyyy") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>


                    <!-- Arıza Hatırlatmaları-->
                    <div class="tab-pane fade" id="panel-pill3">
                        <div class="servishatirlatma">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>Ünvan</th>
                                        <th style="width: 50%">Arıza Açıklaması</th>
                                        <th style="width: 100px">Tarihi</th>
                                    </tr>
                                </thead>

                                <asp:Repeater ID="ariza_hat_rep" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="/Servis/Ariza-Listesi/<%#Eval("CariSU_CariNo") %>/<%#Eval("CariAsansorAriza_Id") %>" class="tip" onclick="window.open(this.href, '_blank');return false;" data-original-title="<%#Eval("CariSu_Tanimi") %>"><%#Eval("Cari_Unvan") %></a></td>
                                            <td><%#Eval("CariAsansorAriza_Aciklama") %></td>
                                            <td><%#Convert.ToDateTime(Eval("CariAsansorAriza_Tarih")).ToString("dd.MM.yyyy") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>


                    <!-- Sözleşme Hatırlatmaları-->
                    <div class="tab-pane fade" id="panel-pill4">
                        <div class="servishatirlatma">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>Ünvan</th>
                                        <th style="width: 160px">Başlık</th>
                                        <th style="width: 135px">Sözleşme Bedeli</th>
                                        <th style="width: 110px">Son Tarihi</th>
                                    </tr>
                                </thead>

                                <asp:Repeater ID="sozlesme_hat_rep" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a href="/Servis/Cari-Sozlesmeler/<%#Eval("CariSozlesmeler_CariId") %>/<%#Eval("CariSozlesmeler_Id") %>" class="tip" onclick="window.open(this.href, '_blank');return false;"><%#Eval("Cari_Unvan")%></a></td>
                                            <td><%#Eval("CariSozlesmeler_Baslik") %></td>
                                            <td><%#Eval("CariSozlesmeler_Bedel").ToString() !=""? Convert.ToDecimal(Eval("CariSozlesmeler_Bedel")).ToString("N") : "" %></td>
                                            <td><%#Convert.ToDateTime(Eval("CariSozlesmeler_SonTarih")).ToString("dd.MM.yyyy") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="callout callout-info fade in">
    <p>
        <span style="margin-top: -2px; margin-right: 5px; float: left;">
            <i class="icon-arrow-up10"></i></span>
        Yukarıda zamanı geçmiş ve önümüzdeki 7 gün içerisinde onaylanmayan servis hatırlatmalarını görebilirsiniz.
    </p>
</div>
