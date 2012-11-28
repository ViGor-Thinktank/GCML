<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignManager.aspx.cs" Inherits="CampaignMasterWeb.frmCampaignView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmCampaignManager" runat="server">
    <div>
    Manager
    </div>
    <asp:ListBox ID="ListBox1" runat="server" Height="143px" Width="680px">
    </asp:ListBox>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Edit" />
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="Label1" runat="server" Text="Move"></asp:Label>
&nbsp;<br />
    <asp:TextBox ID="TextBox1" runat="server" ontextchanged="TextBox1_TextChanged"></asp:TextBox>
    <br />
    <p>
    <asp:TextBox ID="txtEx" runat="server" Height="96px" TextMode="MultiLine" 
        Width="676px" style="margin-top: 39px"></asp:TextBox>
    </p>
    </form>
</body>
</html>
