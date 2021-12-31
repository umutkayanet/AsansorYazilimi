<%@ Page Language="C#" MasterPageFile="~/Merkez_Nokta.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="~/Blok/Haberler.ascx" TagPrefix="uc1" TagName="Haberler" %>
<%@ Register Src="~/Blok/Finans.ascx" TagPrefix="uc1" TagName="Finans" %>
<%@ Register Src="~/Blok/Hava_Durumu.ascx" TagPrefix="uc1" TagName="Hava_Durumu" %>
<%@ Register Src="~/Blok/Servis_Hatirlatma.ascx" TagPrefix="uc1" TagName="Servis_Hatirlatma" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row" style="margin-top: 50px;  background-size: 65%; background-repeat: no-repeat; background-position: unset">
        <div class="col-md-8">
            <uc1:Servis_Hatirlatma runat="server" ID="Servis_Hatirlatma" />
        </div>

       
        <div class="col-lg-4">
            <!--Finans -->
            <uc1:Finans runat="server" ID="Finans" />

            <!-- Hava Durumu -->
            <uc1:Hava_Durumu runat="server" ID="Hava_Durumu" />

            <!-- Assigned users -->
            <uc1:Haberler runat="server" ID="Haberler" />
        </div>
    </div>
</asp:Content>
