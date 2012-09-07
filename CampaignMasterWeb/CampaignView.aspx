<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignView.aspx.cs" Inherits="CampaignMasterWeb.GcmlClientPage" %>

<%@ Register src="FieldControl.ascx" tagname="FieldControl" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        body
        {
            font-family: Arial, Helvetica, Sans-Serif;
        }
        
        .playerpanel
        {
            border-width: 1px;
            background-color: #CCCCCC;
        }
        
        .style3
        {
            font-size: small;
        }
        
    </style>

</head>

<body>
    <form id="form1" runat="server">

    <table border="0" width="100%">
    <tr>
        <td>
            <span class="style3">Spieler:
    </span>
    <asp:Label ID="lbPlayer" runat="server" Text="Label" CssClass="style3"></asp:Label>
    <br class="style3" />
            <span class="style3">Kampagne:
    </span>
    <asp:Label ID="lbCampaign" runat="server" Text="Label" CssClass="style3"></asp:Label>
            <span class="style3">&nbsp;
            </span>
            <br class="style3" />
            <br class="style3" />
            <a href="StartMenu.aspx"><span class="style3">Zurück zum Kampagnenmenü</span></a>
        </td>
        <td><asp:Button ID="btnEndRound" runat="server" Height="41px" 
                                        
                style="color: #FFFF00; background-color: #333333; font-weight: 700;" Text="Runde beenden" 
                                        Width="190px" onclick="btnEndRound_Click" /></td>
        <td></td>
        <td>
            <img alt="" src="Bilder/gcml_logo_small.png" 
                style="float: right" /></td>
    </tr>
    </table>
      <hr />

    <uc1:FieldControl ID="FieldControl1" runat="server" />
    
  
    </form>

</body>
</html>
