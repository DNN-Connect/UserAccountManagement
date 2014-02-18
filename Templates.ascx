<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountManagement.Templates" Codebehind="Templates.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div id="ConnectAccountManagementTemplates" class="dnnForm dnnClear">

    <div class="dnnFormItem">
        <dnn:label id="plTheme" controlname="drpThemes" runat="server" />
        <asp:DropDownList ID="drpThemes" runat="server" AutoPostBack="true"></asp:DropDownList>
        &nbsp;
        <asp:LinkButton ID="cmdCopySelected" runat="server"></asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="cmdDeleteSelected" runat="server"></asp:LinkButton>
    </div>

    <div class="dnnFormItem" runat="server" id="pnlLocales" visible="false">
        <dnn:label id="plLocale" controlname="drpLocales" runat="server" />
        <asp:DropDownList ID="drpLocales" runat="server" AutoPostBack="true"></asp:DropDownList>
    </div>

    <div class="dnnFormItem">
        <dnn:label id="plUseTheme" controlname="txtTemplateName" runat="server" />
        <asp:CheckBox ID="chkUseTheme" runat="server" />
    </div>       

    <div class="dnnFormItem" runat="server" id="pnlTemplateName" visible="false">
        <dnn:label id="plTemplateName" controlname="txtTemplateName" runat="server" />
        <asp:TextBox ID="txtTemplateName" runat="server"></asp:TextBox>
    </div>

    <ul id="AccountUpdateTemplatesTabs" runat="Server" class="dnnAdminTabNav dnnClear">
        <li><a href="#dvCreateFormTemplate"><asp:Label id="lblCreateFormTemplate" runat="server" resourcekey="lblCreateFormTemplate" /></a></li>
        <li><a href="#dvAccountFormTemplate"><asp:Label id="lblAccountFormTemplate" runat="server" resourcekey="lblAccountFormTemplate" /></a></li>
        <li><a href="#dvProfileFormTemplate"><asp:Label id="lblProfileFormTemplate" runat="server" resourcekey="lblProfileFormTemplate" /></a></li>
        <li><a href="#dvEmailAddedToRole"><asp:Label id="lblEmailAddedToRole" runat="server" resourcekey="lblEmailAddedToRole" /></a></li>
        <li><a href="#dvEmailRemovedFromRole"><asp:Label id="lblEmailRemovedFromRole" runat="server" resourcekey="lblEmailRemovedFromRole" /></a></li>
        <li><a href="#dvEmailPasswordReset"><asp:Label id="lblEmailPasswordReset" runat="server" resourcekey="lblEmailPasswordReset" /></a></li>
        <li><a href="#dvEmailAccountData"><asp:Label id="lblEmailAccountData" runat="server" resourcekey="lblEmailAccountData" /></a></li>
        <li><a href="#dvEmailAccountCreated"><asp:Label id="lblEmailAccountCreated" runat="server" resourcekey="lblEmailAccountCreated" /></a></li>
    </ul>

    <div class="dnnFormItem dnnClear" id="dvCreateFormTemplate">
        <asp:TextBox ID="txtCreateFormTemplate" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvAccountFormTemplate">
        <asp:TextBox ID="txtAccountFormTemplate" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvProfileFormTemplate">
        <asp:TextBox ID="txtProfileFormTemplate" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailAddedToRole">
        <asp:TextBox ID="txtEmailAddedToRole" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailRemovedFromRole">
        <asp:TextBox ID="txtEmailRemovedFromRole" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailPasswordReset">
        <asp:TextBox ID="txtEmailPasswordReset" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailAccountData">
        <asp:TextBox ID="txtEmailAccountData" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailAccountCreated">
        <asp:TextBox ID="txtEmailAccountCreated" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    

    <ul class="dnnActions">
        <li><asp:LinkButton ID="cmdUpdateSettings" runat="server" CssClass="dnnPrimaryAction"></asp:LinkButton></li>
        <li><asp:LinkButton ID="cmdCancel" runat="server" CssClass="dnnSecondaryAction"></asp:LinkButton></li>
    </ul>

</div>

<script language="javascript" type="text/javascript">
/*globals jQuery, window, Sys */
(function ($, Sys) {

    function setupFormSettings() {
        $('#ConnectAccountManagementTemplates').dnnTabs();
    }

    $(document).ready(function () {
        setupFormSettings();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setupFormSettings();
        });
    });

} (jQuery, window.Sys));
</script>

