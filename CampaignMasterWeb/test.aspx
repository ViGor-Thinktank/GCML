<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="CampaignMasterWeb.GcmlClientPage" %>

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
        
    </style>

</head>

<body>
    <form id="form1" runat="server">

    <asp:DropDownList ID="dropDownPlayer" runat="server">
        <asp:ListItem Value="-">-</asp:ListItem>
        <asp:ListItem Value="1">Player 1</asp:ListItem>
        <asp:ListItem Value="2">Player 2</asp:ListItem>
    </asp:DropDownList>&nbsp;
    <asp:Button ID="btnSelectPlayer" runat="server" Text="Spieler wählen" 
        onclick="btnSelectPlayer_Click" />
        <br />
    <uc1:FieldControl ID="FieldControl1" runat="server" />
    <hr />
    </form>

</body>
</html>
