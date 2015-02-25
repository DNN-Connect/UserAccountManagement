<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.Accounts.UserForms.View" Codebehind="View.ascx.vb" %>                      
        
<div class="connect_registerform">

    <asp:Panel ID="pnlError" runat="server" CssClass="dnnFormMessage dnnFormError" Visible="false">
        <asp:Literal id="lblError" runat="server"></asp:Literal>
    </asp:Panel> 
    <asp:Panel ID="pnlSuccess" runat="server" CssClass="dnnFormMessage dnnFormSuccess" Visible="false">
        <asp:Literal id="lblSucess" runat="server"></asp:Literal>
    </asp:Panel> 

    <asp:PlaceHolder ID="plhRegister" runat="server"></asp:PlaceHolder>   
        
</div>
            
         



