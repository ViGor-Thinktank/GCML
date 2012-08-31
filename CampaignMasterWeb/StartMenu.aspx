﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartMenu.aspx.cs" Inherits="CampaignMasterWeb.StartMenu" %>

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
        
               
        .style4
        {
            width: 219px;
        }
        
               
        .style5
        {
            height: 17px;
        }
        .style7
        {
            height: 17px;
            width: 64px;
        }
        .style8
        {
            width: 37px;
        }
        .style9
        {
            width: 64px;
        }
        
               
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" width="100%">
    <tr>
        <td><asp:Label ID="Label1" runat="server" Text="Label">Spielername: </asp:Label>
    
        <asp:TextBox ID="tbPlayername" runat="server" Width="190px" BorderStyle="Solid" 
                BorderWidth="1px"></asp:TextBox>
        <asp:Button ID="btnLogin" runat="server" Text="Anmelden" onclick="btnLogin_Click" 
                BorderStyle="Solid" BorderWidth="1px" 
             />
        <asp:Button ID="btnLogoff" runat="server" Text="Ausloggen" 
            onclick="btnLogoff_Click" BorderStyle="Solid" BorderWidth="1px" /></td>
        <td></td>
        <td></td>
        <td>
            <img alt="" class="style1" src="Bilder/gcml_logo_small.png" /></td>
    </tr>
    </table>
    <hr />

    <div>
    
        
        <asp:Panel ID="pnPlayerCampaigns" runat="server">
    
  
            <table style="width:100%;">
           
                <tr>
                    <td class="style4">
                        Kampagnen</td>
                    <td>
                        
                    </td>
                   
                </tr>
                <tr>
                    <td class="style4">
                        <asp:ListBox ID="lbCampaigns" runat="server" Height="115px" Width="301px" 
                            BackColor="#CCCCCC">
                        </asp:ListBox>
                    </td>
                    <td valign="top">
                    <asp:Button ID="btnSelectCampaign" runat="server" Text="Kampagne auswählen" 
                            Width="146px" BorderStyle="Solid" BorderWidth="1px" Height="36px" 
                            onclick="lbCampaigns_SelectedIndexChanged" />
                        <br />
                        <br />
                            <asp:Button ID="btnNewCampaign0" runat="server" BorderStyle="Solid" 
                    BorderWidth="1px" OnClick="BtnNewCampaign_Click" Text="Neue Kampagne" 
                    Width="146px" />
                &nbsp;<asp:TextBox ID="tbCampaignName" runat="server" BorderStyle="Solid" 
                    BorderWidth="1px" Width="217px"></asp:TextBox>
                            
                        </table>
                    </td>
                </tr>
            </table>
            <br />
         <asp:Panel ID="pnCampaignInfo" runat="server" BorderColor="#CCCCCC" 
                            BorderStyle="Solid" BorderWidth="1px" Width="415px">
                        <table border="0" style="font-size: x-small">
                        <tr>
                        <td>Id</td>
                        <td class="style9">
                            <asp:Label ID="lbId" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <td>Name</td><td class="style9">
                            <asp:Label ID="lbName" runat="server" Text="Label"></asp:Label>
                            </td></tr>
                        <tr><td class="style5">Spieler</td><td class="style7">
                            <asp:Label ID="lbPlayer" runat="server" Text="Label"></asp:Label>
                            </td></tr>
                       
                            <tr>
                            <td>
                            <asp:Button ID="btnAddPlayerToCampaign" runat="server" 
                            onclick="btnAddPlayerToCampaign_Click" 
                            Text="Spieler hinzufügen:" Width="160px" BorderStyle="Solid" BorderWidth="1px" />
                            </td>
                            <td class="style9">
                            <asp:TextBox ID="tbAddPlayername" runat="server" Width="198px" BorderStyle="Solid" 
                                    BorderWidth="1px"></asp:TextBox>
                            </td>
                            </tr>
                             <tr>
                        <td>&nbsp;</td>
                        <td class="style9">&nbsp;</td>
                        </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnLoadCampaign" runat="server" Height="41px" 
                                        onclick="btnLoadCampaign_Click" 
                                        style="color: #FFFF00; background-color: #333333" Text="Kampagne starten" 
                                        Width="190px" />
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                        
        </asp:Panel>
    </div>
    </form>
</body>
</html>
