<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartMenu.aspx.cs" Inherits="CampaignMasterWeb.StartMenu" %>

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
        
               
        .style1
        {
            float: right;
        }
        
               
        .style2
        {
            width: 236px;
        }
        .style3
        {
            width: 277px;
        }
        .style4
        {
            width: 219px;
        }
        
               
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" width="100%">
    <tr>
        <td><asp:Label ID="Label1" runat="server" Text="Label">Spielername: </asp:Label>
    
        <asp:TextBox ID="tbPlayername" runat="server" Width="190px"></asp:TextBox>
        <asp:Button ID="btnLogin" runat="server" Text="Anmelden" onclick="btnLogin_Click" 
             />
        <asp:Button ID="btnLogoff" runat="server" Text="Ausloggen" 
            onclick="btnLogoff_Click" /></td>
        <td></td>
        <td></td>
        <td>
            <img alt="" class="style1" src="Bilder/gcml_logo_small.png" /></td>
    </tr>
    </table>
    <div>
    
        
        <br />
        <br />
        <asp:Panel ID="pnPlayerCampaigns" runat="server">
    
  
            <table style="width:100%;">
                <tr>
                    <td class="style4">
                        Kampagnen</td>
                    <td>
                        &nbsp;</td>
                   
                </tr>
                <tr>
                    <td class="style4">
                        <asp:ListBox ID="lbCampaigns" runat="server" Height="154px" Width="213px" 
                            AutoPostBack="True" onselectedindexchanged="lbCampaigns_SelectedIndexChanged">
                        </asp:ListBox>
                    </td>
                    <td>
                        <asp:Panel ID="pnCampaignInfo" runat="server">
                            <asp:Button ID="btnLoadCampaign" runat="server" 
                                onclick="btnLoadCampaign_Click" Text="Kampagne starten" Width="190px" />
                        </asp:Panel>
                    </td>
                    
                </tr>
               
            </table>
            <br />
            <table style="width:100%;">
                <tr>
                    <td class="style2">
                        <asp:Button ID="btnNewCampaign" runat="server" OnClick="BtnNewCampaign_Click" 
                            Text="Neue Kampagne" Width="190px" />
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        <asp:Button ID="btnAddPlayerToCampaign" runat="server" 
                            onclick="btnAddPlayerToCampaign_Click" 
                            Text="Spieler hinzufügen:" Width="190px" />
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="tbAddPlayername" runat="server" Width="249px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <br />
        <br />
        
        </asp:Panel>
    </div>
    </form>
</body>
</html>
