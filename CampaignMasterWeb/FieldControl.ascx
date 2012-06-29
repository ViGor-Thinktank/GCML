<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldControl.ascx.cs" Inherits="CampaignMasterWeb.FieldControl" %>
<style type="text/css">
    .style1
    {
        height: 42px;
    }
</style>
<table style="width: 100%;">
<tr>
<td>
Spieler Auswählen:
</td>
<td>
    <asp:DropDownList ID="dropDownPlayer" runat="server">
        <asp:ListItem Value="1">Player 1</asp:ListItem>
        <asp:ListItem Value="2">Player 2</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnSelectPlayer" runat="server" Text="Spieler wählen" 
        onclick="btnSelectPlayer_Click" />
</td>
</tr>
    <tr>
        <td class="style1">
            <asp:Panel ID="panelPlayer" runat="server">
            </asp:Panel>
        </td>
        <td class="style1">
            &nbsp;
        </td>
        <td class="style1">
            <asp:Panel ID="panelField" runat="server">
            </asp:Panel>
        </td>
    </tr>
</table>
