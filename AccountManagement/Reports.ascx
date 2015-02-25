<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountManagement.Reports" Codebehind="Reports.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>



                        
<asp:Repeater ID="rptReports" runat="server">
    <HeaderTemplate><table style="width:100%"></HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="width:100%;white-space:nowrap;"><%#DataBinder.Eval(Container.DataItem, "FriendlyName")%></td>
            <td style="padding-left:30px; white-space:nowrap;">
                <asp:LinkButton ID="cmdEdit" runat="server" Text="Edit" OnClick="cmdEditReportFromList_Click" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ReportId")%>' resourcekey="cmdEdit"></asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="cmdDelete" runat="server" Text="Delete" OnClick="cmdDeleteReportFromList_Click" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ReportId")%>' resourcekey="cmdDelete"></asp:LinkButton>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate></table></FooterTemplate>
</asp:Repeater>
                        
<ul class="dnnActions">
    <li><asp:LinkButton ID="cmdNewReport" runat="server" Text="Add Report" resourcekey="cmdNewReport" CssClass="dnnPrimaryAction"></asp:LinkButton></li>
    <li><asp:LinkButton ID="cmdBack" runat="server" Text="Back to Module" resourcekey="cmdBack" CssClass="dnnSecondaryAction"></asp:LinkButton></li>
</ul>
                        
<asp:Panel ID="pnlReportForm" runat="server" Visible="false" style="padding-top:15px;">
                            
    <div class="dnnForm dnnClear">

        <div class="dnnFormItem">
            <dnn:Label ID="plReportName" runat="server" Text="Report Name:" resourcekey="plReportName"></dnn:Label>
            <asp:Textbox ID="txtReportName" runat="server" Width="300"></asp:Textbox>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="plReportSql" runat="server" Text="Report SQL" resourcekey="plReportSql"></dnn:Label>
            <asp:Textbox ID="txtReportSql" runat="server" TextMode="MultiLine" Width="300" Height="200"></asp:Textbox>
        </div>

    </div>

    <ul class="dnnActions">
        <li><asp:LinkButton ID="cmdAddReport" runat="server" Text="Update" resourcekey="cmdAddReport" CssClass="dnnPrimaryAction"></asp:LinkButton></li>
        <li><asp:LinkButton ID="cmdUpdateReport" runat="server" Text="Update" Visible="false" resourcekey="cmdUpdateReport" CssClass="dnnPrimaryAction"></asp:LinkButton></li>
        <li><asp:LinkButton ID="cmdDeleteReport" runat="server" Text="Delete" Visible="false" resourcekey="cmdDeleteReport" CssClass="dnnSecondaryAction"></asp:LinkButton></li>
        <li><asp:LinkButton ID="cmdCancelReport" runat="server" Text="Cancel" Visible="true" resourcekey="cmdCancelReport" CssClass="dnnSecondaryAction"></asp:LinkButton></li>
    </ul>
                                                        
</asp:Panel>
