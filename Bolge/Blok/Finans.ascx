<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Finans.ascx.cs" Inherits="Blok_Finans" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="panel panel-primary kutugolgefekti">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-stats"></i>Kurlar</h6>
                <div style="float: right">
                    <h6 class="panel-title">
                        <asp:LinkButton ID="yenile" runat="server" data-original-title="Yenile" OnClick="yenile_Click" CssClass="tip"><i class="icon-spinner8"></i></asp:LinkButton>
                    </h6>
                </div>
            </div>

            <div class="panel-body">
                <script type="text/javascript">
                    function pageLoad() {
                        $(function () {
                            $("#W3199").paraceviriciWidget({
                                widget: "boxline",
                                wData: {
                                    category: 0,
                                    currency: "USD-EUR"
                                },
                                wSize: {
                                    wWidth: "100%",
                                    wHeight: 100
                                },
                                wBase: {
                                    rCombine: 1
                                },
                                wFrame: {
                                    wB: 0
                                },
                                wContent: {
                                    cFlag: 1,
                                    pBuy: 1,
                                    pChange: 1
                                },
                                wTop: {
                                    tStat: 2,
                                    tT: "Son Yenileme >>",
                                    tS: 11
                                },
                                wCode: {
                                    cS: 14
                                },
                                wPrice: {
                                    pS: 15
                                },
                                wArrow: {
                                    aS: 7
                                }
                            });
                        });
                    }
                </script>
                <asp:Panel ID="Panel1" runat="server">
                    <div id="W3199" style="z-index: 0;"></div>
                </asp:Panel>
                <br />
                Serbest Piyasa Kurları
                <script type="text/javascript" src="https://paracevirici.com/servis/widget/widget.js"></script>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
