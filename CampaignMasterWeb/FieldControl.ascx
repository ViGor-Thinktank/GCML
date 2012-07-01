<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldControl.ascx.cs" Inherits="CampaignMasterWeb.FieldControl" %>
<style type="text/css">
    .style1
    {
        height: 42px;
    }
</style>
<table style="width: 100%;"  class="playerpanel">
<tr>
<td colspan="2">
Spieler Auswählen:
&nbsp;
    <asp:DropDownList ID="dropDownPlayer" runat="server" OnSelectedIndexChanged="unitSelected">
        <asp:ListItem Value="1">Player 1</asp:ListItem>
        <asp:ListItem Value="2">Player 2</asp:ListItem>
    </asp:DropDownList>&nbsp;
    <asp:Button ID="btnSelectPlayer" runat="server" Text="Spieler wählen" 
        onclick="btnSelectPlayer_Click" />
</td>
</tr>
    <tr>
        <td class="style1">
            <asp:Panel ID="panelPlayer" runat="server" Wrap="true">
            </asp:Panel>
        </td>
        <td class="style1">
            <asp:Panel ID="panelField" runat="server" Wrap="true">
            </asp:Panel>
        </td>
    </tr>
</table>
