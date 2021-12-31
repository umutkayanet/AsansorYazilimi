<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Haberler.ascx.cs" Inherits="Blok_Haberler" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="panel panel-primary kutugolgefekti">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-tv"></i>Haberler</h6>
                <div style="float: right">
                    <h6 class="panel-title">
                        <asp:LinkButton ID="yenile" runat="server" data-original-title="Yenile" OnClick="yenile_Click" CssClass="tip"><i class="icon-spinner8"></i></asp:LinkButton>
                    </h6>
                </div>
            </div>
            <div class="panel-body">
                <!-- BAŞLA: TRT Haber Sitene Ekle -->
                <div style="font: normal 12px Arial; width: 100%; background: #fff; border-radius: 3px;">
                    <asp:Panel ID="Panel1" runat="server">
                    <iframe frameborder="0" width="100%" height="300" src="https://www.trthaber.com/sitene-ekle/ekonomi-7/?haberSay=20&renk=a&baslik=0&resimler=1"></iframe>
                    </asp:Panel>
                </div>
                <!-- BİTİŞ: TRT Haber Sitene Ekle -->
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
