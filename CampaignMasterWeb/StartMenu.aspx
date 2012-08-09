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
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="Label">Spielername: </asp:Label>
    
        <asp:TextBox ID="tbPlayername" runat="server" Width="190px"></asp:TextBox>
        <asp:Button ID="btnLogin" runat="server" Text="Anmelden" onclick="btnLogin_Click" 
             />
        <asp:Button ID="btnLogoff" runat="server" Text="Ausloggen" 
            onclick="btnLogoff_Click" />
        <br />
        <br />
        <asp:Panel ID="pnPlayerCampaigns" runat="server">
		<asp:Button ID="btnNewCampaign" runat="server" Text="Neue Kampagne" OnClick="BtnNewCampaign_Click" />
    
    &nbsp;<br />
        <br />
        Kampagnen
        <br />
        <asp:ListBox ID="lbCampaigns" runat="server" Height="154px" Width="213px">
        </asp:ListBox>
        <br />
        <br />
        <asp:Button ID="btnLoadCampaign" runat="server" Text="Gewählte Kampagne laden" 
                onclick="btnLoadCampaign_Click" />
        <br />
        <br />
        
        </asp:Panel>
    </div>
    </form>
</body>
</html>
