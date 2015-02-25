<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountManagement.Settings" Codebehind="Settings.ascx.vb" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>


<div class="dnnFormItem">
     <dnn:Label ID="lblAllowReports" runat="server" resourcekey="lblAllowReports"></dnn:Label>
     <asp:CheckBox ID="chkAllowReports" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAllowCreate" runat="server" resourcekey="lblAllowCreate"></dnn:Label>
     <asp:CheckBox ID="chkAllowCreate" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAllowDelete" runat="server" resourcekey="lblAllowDelete"></dnn:Label>
     <asp:CheckBox ID="chkAllowDelete" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAllowHardDelete" runat="server" resourcekey="lblAllowHardDelete"></dnn:Label>
     <asp:CheckBox ID="chkAllowHardDelete" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAllowExport" runat="server" resourcekey="lblAllowExport"></dnn:Label>
     <asp:CheckBox ID="chkAllowExport" runat="server" />
</div>

<div class="dnnFormItem">
    <dnn:Label ID="lblAllowedRoles" runat="server" resourcekey="lblAllowedRoles"></dnn:Label>
    <asp:CheckBoxList ID="chkAllowedRoles" runat="server" RepeatColumns="2" DataTextField="RoleName" DataValueField="RoleId"></asp:CheckBoxList>  
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblPreselectRole" runat="server" resourcekey="lblPreselectRole"></dnn:Label>
     <asp:DropDownList ID="drpPreselectRole" runat="server" DataTextField="RoleName" DataValueField="RoleId"></asp:DropDownList>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblUserTabs" runat="server" resourcekey="lblUserTabs"></dnn:Label>
     <asp:CheckBoxList ID="chkUserTabs" runat="server" RepeatColumns="2">
         <asp:ListItem Text="Account" Value="account"></asp:ListItem>
         <asp:ListItem Text="Password" Value="password"></asp:ListItem>
         <asp:ListItem Text="Profile" Value="profile"></asp:ListItem>
         <asp:ListItem Text="Roles" Value="roles"></asp:ListItem>
         <asp:ListItem Text="E-Mail" Value="email"></asp:ListItem>         
         <asp:ListItem Text="Sites" Value="sites"></asp:ListItem>
     </asp:CheckBoxList> 
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAdditionalControls" runat="server" resourcekey="lblAdditionalControls"></dnn:Label>
     <asp:TextBox ID="txtAditionalControls" TextMode="MultiLine" runat="server"></asp:TextBox>
</div>

