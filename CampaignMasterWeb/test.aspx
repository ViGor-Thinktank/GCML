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

    <table border="0" width="100%">
    <tr>
        <td>
        Spieler:
    <asp:Label ID="lbPlayer" runat="server" Text="Label"></asp:Label>
    <br />
    Kampagne:
    <asp:Label ID="lbCampaign" runat="server" Text="Label"></asp:Label>
&nbsp;
        </td>
        <td></td>
        <td></td>
        <td>
            <img alt="" class="style1" src="Bilder/gcml_logo_small.png" 
                style="float: right" /></td>
    </tr>
    </table>
      <hr />

    <uc1:FieldControl ID="FieldControl1" runat="server" />
    
  
    </form>

</body>
</html>
