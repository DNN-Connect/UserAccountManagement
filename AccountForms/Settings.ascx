<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.Accounts.UserForms.Settings" Codebehind="Settings.ascx.vb" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>


<div class="dnnFormItem">    
    <dnn:Label ID="lblUsernameMode" runat="server" Text="Username Mode:"></dnn:Label>
    <asp:DropDownList ID="drpUsernameMode" runat="server" AutoPostBack="false">
        <asp:ListItem Value="DISPLAY" Text="Yes" resourcekey="Username_FreeEnter"></asp:ListItem>
        <asp:ListItem Value="EMAIL" Text="No, use e-mail address" resourcekey="Username_UseEmail"></asp:ListItem>
        <asp:ListItem Value="FIRSTLETTER.LASTNAME" Text="No, use [Firstletter.Lastname]" resourcekey="Username_FirstletterLastname"></asp:ListItem>                
        <asp:ListItem Value="FIRSTNAME.LASTNAME" Text="No, use [Firstname.Lastname]" resourcekey="Username_FirstnameLastname"></asp:ListItem>                
        <asp:ListItem Value="LASTNAME" Text="No, use [Lastname]" resourcekey="Username_Lastname"></asp:ListItem>                
    </asp:DropDownList>    
</div>
<div class="dnnFormItem">
    <dnn:Label ID="lblDisplaynameMode" runat="server" Text="Displayname Mode:"></dnn:Label>
    <asp:DropDownList ID="drpDisplaynameMode" runat="server" AutoPostBack="false">
        <asp:ListItem Value="DISPLAY" Text="Yes" resourcekey="Displayname_FreeEnter"></asp:ListItem>
        <asp:ListItem Value="EMAIL" Text="No, use e-mail address" resourcekey="Displayname_FromEmail"></asp:ListItem>
        <asp:ListItem Value="FIRSTLETTER.LASTNAME" Text="No, use [Firstletter. Lastname]" resourcekey="Displayname_FirstletterLastname"></asp:ListItem>                
        <asp:ListItem Value="FIRSTNAME.LASTNAME" Text="No, use [Firstname Lastname]" resourcekey="Displayname_FirstnameLastname"></asp:ListItem>                
        <asp:ListItem Value="LASTNAME" Text="No, use [Lastname]" resourcekey="Displayname_Lastname"></asp:ListItem>                
    </asp:DropDownList>    
</div>


<div class="dnnFormItem">
    <dnn:Label ID="lblRedirectAfterLogin" runat="server" Text="Redirect after login:"></dnn:Label>
    <asp:DropDownList ID="drpRedirectAfterLogin" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />
</div>

<div class="dnnFormItem">
    <dnn:Label ID="lblRedirectAfterSubmit" runat="server" Text="Redirect after submit:"></dnn:Label>
    <asp:DropDownList ID="drpRedirectAfterSubmit" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />
</div>


<div class="dnnFormItem">
    <dnn:Label ID="lblUsermanagementTab" runat="server" Text="Select Usermanagement Tab:"></dnn:Label> 
    <asp:DropDownList ID="drpUserManagementTab" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />    
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAddToRole" runat="server" Text="Add to role on submit:"></dnn:Label>
     <asp:DropDownList ID="drpAddToRole" runat="server" DataTextField="RoleName" DataValueField="RoleId"></asp:DropDownList>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAddToRoleStatus" runat="server" Text="Role status once submitted:"></dnn:Label>
     <asp:DropDownList ID="drpRoleStatus" runat="server">
         <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
         <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
     </asp:DropDownList>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblNotifyRole" runat="server" Text="Send confirmation e-mail to Role:"></dnn:Label>
     <asp:DropDownList ID="drpNotifyRole" runat="server" DataTextField="RoleName" DataValueField="RoleId"></asp:DropDownList>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblNotifyUser" runat="server" Text="Send confirmation e-mail to user:"></dnn:Label>
     <asp:CheckBox ID="chkNotifyUser" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblInterface" runat="server" Text="External Interface:"></dnn:Label>
     <asp:TextBox ID="txtInterface" runat="server"></asp:TextBox>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblPrivateCaptchaKey" runat="server" Text="External Interface:"></dnn:Label>
     <asp:TextBox ID="txtPrivateCaptchaKey" runat="server"></asp:TextBox>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblCompareFirstNameLastName" runat="server" Text="Compare Firstname and Lastname:"></dnn:Label>
     <asp:CheckBox ID="chkCompareFirstNameLastName" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblValidateEmailThroughRegex" runat="server" Text="Validate e-mail through Regex:"></dnn:Label>
     <asp:CheckBox ID="chkValidateEmailThroughRegex" runat="server" />
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblEmailRegex" runat="server" Text="Regex Validation for E-Mail Adresses:"></dnn:Label>
     <asp:TextBox ID="txtEmailRegex" runat="server"></asp:TextBox>
</div>