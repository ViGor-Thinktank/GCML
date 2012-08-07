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
    
        <asp:TextBox ID="TextBox1" runat="server" Width="190px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Anmelden" /><br />
		<asp:Button ID="BtnNewCampaign" runat="server" Text="Neue Kampagne" OnClick="BtnNewCampaign_Click" />
    
    </div>
    </form>
</body>
</html>
