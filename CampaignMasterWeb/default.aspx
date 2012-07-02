<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="CampaignMasterWeb.GcmlClient" %>

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
    <uc1:FieldControl ID="FieldControl1" runat="server" />
    <hr />
    <uc1:FieldControl ID="FieldControl2" runat="server" />
    
    </form>

</body>
</html>
