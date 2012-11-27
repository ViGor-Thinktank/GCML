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
    <asp:ListBox ID="ListBox1" runat="server" Height="143px" Width="274px" 
        onselectedindexchanged="ListBox1_SelectedIndexChanged">
    </asp:ListBox>
    <br />
    <br />
    <br />
    <asp:TextBox ID="txtEx" runat="server" Height="96px" TextMode="MultiLine" 
        Width="676px"></asp:TextBox>
    </form>
</body>
</html>
