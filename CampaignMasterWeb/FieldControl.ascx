<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FieldControl.ascx.cs" Inherits="CampaignMasterWeb.FieldControl" %>
<%@ Reference Control="SektorControl.ascx" %>

<style type="text/css">
    .style1
    {
        height: 42px;
    }
    .style2
    {
        height: 34px;
    }
</style>
<input type="hidden" runat="server" ID="panelControlSessionId" />
<table style="width: 100%;"  class="playerpanel">
<tr>
<td colspan="2" class="style2">
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
