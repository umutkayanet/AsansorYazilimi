<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Hava_Durumu.ascx.cs" Inherits="Blok_Hava_Durumu" %>

        
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">           
            !function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];

                if (!d.getElementById(id)) {
                    js = d.createElement(s); js.id = id; js.src = 'https://weatherwidget.io/js/widget.min.js'; fjs.parentNode.insertBefore(js, fjs);
                }
            }
            (document, 'script', 'weatherwidget-io-js');           
        </script>


        <div class="panel panel-primary kutugolgefekti">
            <div class="panel-heading">
                <h6 class="panel-title"><i class="icon-cloud"></i>Hava Durumu</h6>
            </div>
            <div class="panel-body" style="padding: 4px 0 10px 0; overflow-y: scroll;">                                   
                    <a class="weatherwidget-io" href="https://forecast7.com/tr/39d9332d86/ankara/" data-label_1="ANKARA" data-label_2="" data-icons="Climacons Animated" data-theme="pure" >ANKARA WEATHER</a>
                    <!--
                    <div style="text-align:center;">
                    <table style="width:100%">
                        <tr>
                            <td style="width:130px"></td>
                            <td style="text-align:center; height:20px">Son Durum</td>
                            <td style="text-align:center;">1-5 Günlük Tahmin</td>
                        </tr>
                        <tr>
                            <td>Ankara<br />Meteoroloji Genel Müdürlüğü</td>
                            <td>
                                <img src="http://www.mgm.gov.tr/sunum/sondurum-klasik-5070.aspx?m=ANKARA&rC=111&rZ=fff" style="width:100px; height:70px;" alt="ANKARA" />                                
                            </td>
                            <td>
                                <img src="http://www.mgm.gov.tr/sunum/tahmin-klasik-5070.aspx?m=ANKARA&basla=1&bitir=5&rC=111&rZ=fff" style="width:250px; height:70px;" alt="ANKARA" />
                            </td>
                        </tr>
                    </table>
                    </div>
                   -->
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

