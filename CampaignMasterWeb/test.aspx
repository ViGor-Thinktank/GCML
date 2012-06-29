<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="CampaignMasterWeb.CampaignMasterClientTest" %>

<%@ Register src="FieldControl.ascx" tagname="FieldControl" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 46px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
        <td><uc1:FieldControl ID="FieldControl1" runat="server" /></td>
    </tr>
    <tr>
        <td><uc1:FieldControl ID="FieldControl2" runat="server" /></td>
    </tr>
    </table>
    </form>

</body>
</html>
