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
        .style9
        {
            width: 64px;
        }
        
        
        .style10
        {
            width: 148px;
        }
        .auto-style1 {
            width: 151px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" width="100%">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Label">Spielername: </asp:Label>
                <asp:TextBox ID="tbPlayername" runat="server" Width="190px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                <asp:Button ID="btnLogin" runat="server" Text="Anmelden" OnClick="btnLogin_Click"
                    BorderStyle="Solid" BorderWidth="1px" />
                <asp:Button ID="btnLogoff" runat="server" Text="Ausloggen" OnClick="btnLogoff_Click"
                    BorderStyle="Solid" BorderWidth="1px" />
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
                <img alt="" class="style1" src="Bilder/gcml_logo_small.png" />
            </td>
        </tr>
    </table>
    <hr />
    <div>
        <asp:Panel ID="pnPlayerCampaigns" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="style4">
                        <b>Kampagnen:</b>
                    </td>
                    <td>
                        <b>Neue Kampagne anlegen:</b>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:ListBox ID="lbCampaigns" runat="server" Height="115px" Width="301px" 
                            BackColor="#CCCCCC"></asp:ListBox>
                        <br />
                        
                        <asp:Button ID="btnSelectCampaign0" runat="server" BorderStyle="Solid" BorderWidth="1px"
                            Height="36px" OnClick="lbCampaigns_SelectedIndexChanged" Text="Kampagne laden"
                            Width="146px" />
                    </td>
                    <td valign="top">
                        <table width="100%">
                                <tr>
                                <td class="auto-style1">Name</td>
                                <td><asp:TextBox ID="tbCampaignName" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="217px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="tbCampaignName" ErrorMessage="Feld muss ausgefüllt sein!"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr><td class="auto-style1">Breite</td>
                                <td><asp:TextBox ID="tbX" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="75px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                        ErrorMessage="Feld muss ausgefüllt sein!" ControlToValidate="tbX"></asp:RequiredFieldValidator>
                                    </td></tr>
                                <tr><td class="auto-style1">Höhe</td>
                                <td><asp:TextBox ID="tbY" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Width="75px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                        ErrorMessage="Feld muss ausgefüllt sein!" ControlToValidate="tbY"></asp:RequiredFieldValidator></td></tr>
                                <tr>
                                <td class="auto-style1">Anzahl Units:</td>
                                <td><asp:TextBox ID="tbAnzUnits" runat="server" BorderStyle="Solid"
                                    BorderWidth="1px" Width="75px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                        ErrorMessage="Feld muss ausgefüllt sein!" ControlToValidate="tbAnzUnits"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                <td>
                                   
                                    </td>
                                <td>&nbsp;</td>
                                </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="Button1" runat="server" BorderStyle="Solid" BorderWidth="1px" OnClick="BtnNewCampaign_Click" Text="Kampagne erstellen" Width="146px" />
                                    </td>
                                <td>&nbsp;</td>
                                </tr>
                                
                   
                    </table> 
                        </td>
                </tr>
            </table>
            <hr />
            <asp:Panel ID="pnCampaignInfo" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid"
                BorderWidth="1px" Width="415px">
                <table border="0" style="font-size: small">
                    <tr>
                        <td>
                            Id
                        </td>
                        <td class="style9">
                            <asp:Label ID="lbId" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Name
                        </td>
                        <td class="style9">
                            <asp:Label ID="lbName" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            Spieler
                        </td>
                        <td class="style7">
                            <asp:Label ID="lbPlayer" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAddPlayerToCampaign" runat="server" OnClick="btnAddPlayerToCampaign_Click"
                                Text="Spieler hinzufügen:" Width="160px" BorderStyle="Solid" BorderWidth="1px" />
                        </td>
                        <td class="style9">
                            <asp:TextBox ID="tbAddPlayername" runat="server" Width="198px" BorderStyle="Solid"
                                BorderWidth="1px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>

                    <td>
                        <asp:Button ID="btnAddUnit" runat="server" Text="Unit Ressource hinzufügen" Width="198px" BorderStyle="Solid"
                                BorderWidth="1px" onclick="btnAddUnit_Click"/>
                    </td>
                    <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style9">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnLoadCampaign" runat="server" Height="41px" OnClick="btnLoadCampaign_Click"
                                Style="color: #FFFF00; background-color: #333333; font-weight: 700;" Text="Kampagne starten"
                                Width="190px" />
                        </td>
                        <td align="center">
                            <asp:Button ID="btnEditCampaign" runat="server" Height="41px" OnClick="btnEditCampaign_Click"
                                Style="color: #FFFF00; background-color: #333333; font-weight: 700;" Text="Kampagne bearbeiten"
                                Width="190px" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
