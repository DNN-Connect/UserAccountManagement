<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountManagement.View" Codebehind="View.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="connect_managementform dnnClear">

    <asp:Panel ID="pnlError" runat="server" CssClass="dnnFormMessage dnnFormError" Visible="false">
        <asp:Literal id="lblError" runat="server"></asp:Literal>
    </asp:Panel> 

    <asp:Panel ID="pnlSuccess" runat="server" CssClass="dnnFormMessage dnnFormSuccess" Visible="false">
        <asp:Literal id="lblSucess" runat="server"></asp:Literal>
    </asp:Panel>     
         

    <div class="connect_leftcol dnnLeft">

        <asp:Panel ID="pnlCreateAccount" runat="server" CssClass="connect_btnContainer">
            <asp:LinkButton ID="cmdCreateAccount" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdCreateAccount"></asp:LinkButton>
        </asp:Panel>

        <asp:Panel ID="pnlBackToList" runat="server" CssClass="connect_btnContainer">
            <asp:LinkButton ID="cmdBack" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdBack"></asp:LinkButton>
        </asp:Panel>

        <asp:Panel ID="pnlSearch" runat="server" CssClass="connect_btnContainer">
            <div class="dnnClear">
                <div class="connect_searchbox dnnLeft"><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></div>
                <div class="connect_searchbutton dnnLeft"><asp:LinkButton id="btnSearch" runat="server"></asp:LinkButton></div>
            </div>
        </asp:Panel>

        <div class="connect_rolesContainer">
            <dnn:DnnTreeView ID="ctlRoles" runat="server"></dnn:DnnTreeView>
        </div>

        <asp:Panel ID="pnlHardDelete" runat="server" CssClass="connect_btnContainer">
            <asp:Linkbutton ID="btnHardDelete" runat="server" cssClass="dnnSecondaryAction" Visible="false" />
        </asp:Panel>

        <asp:Panel ID="pnlExport" runat="server" CssClass="connect_btnContainer">
            <asp:Linkbutton ID="btnExport" runat="server" CssClass="dnnSecondaryAction" />
        </asp:Panel>

        <asp:Panel ID="pnlReport" runat="server" CssClass="connect_btnContainer">
            <div class="connect_drpContainer dnnFormItem"><asp:DropDownList ID="drpReports" runat="server"></asp:DropDownList></div>
            <asp:Linkbutton ID="btnReport" runat="server" Text="Run Report" CssClass="dnnSecondaryAction" />
        </asp:Panel>

    </div>

    <div class="connect_rightcol dnnLeft">

        <asp:Panel ID="pnlGrid" runat="server">

            <div id="ConnectGridContainer" class="dnnClear">

                <ul id="UserGridTabs" runat="Server" class="dnnAdminTabNav dnnClear">
                    <li><a href="#dvGrid"><asp:Label id="lblGridTab" runat="server" resourcekey="lblFormTemplate" /></a></li>
                    <li><a href="#dvPreferences"><asp:Label id="lblPreferencesTab" runat="server" resourcekey="lblEmailUser" /></a></li>
                </ul>

                <div id="dvGrid">
                    
                    <dnn:dnngrid id="grdUsers" runat="server">
                        <MasterTableView AutoGenerateColumns="false" DataKeyNames="UserId" AllowSorting="true">
                            <Columns>
                                <dnn:dnnGridBoundColumn DataField="UserId" HeaderText="UserID" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="DisplayName" HeaderText="DisplayName" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="FirstName" HeaderText="FirstName" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="LastName" HeaderText="LastName" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="Username" HeaderText="Username" Visible="false"></dnn:dnnGridBoundColumn>                        
                                <dnn:dnnGridBoundColumn DataField="Email" HeaderText="Email" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="PostalCode" HeaderText="PostalCode" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="City" HeaderText="City" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="Country" HeaderText="Country" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="Telephone" HeaderText="Telephone" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="Cell" HeaderText="Cell" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="PreferredLocale" HeaderText="PreferredLocale" Visible="false"></dnn:dnnGridBoundColumn>
                                <dnn:dnnGridBoundColumn DataField="CreatedDate" HeaderText="Created" DataFormatString="{0:dd.MM.yyyy}" Visible="false"></dnn:dnnGridBoundColumn>                         
                                <dnn:dnnGridTemplateColumn HeaderText="Actions">
                                    <ItemTemplate>
                                        <dnn:dnnMenu ID="ctlActionMenu" runat="server" ClickToOpen="true" EnableOverlay="false">
                                            <Items>
                                                <dnn:dnnMenuItem Text="Actions" ImageUrl="~/images/expand.gif" PostBack="false"> 
                                                </dnn:dnnMenuItem>
                                            </Items>
                                        </dnn:dnnMenu>                               
                                
                                    </ItemTemplate>
                                </dnn:dnnGridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false" Selecting-AllowRowSelect="false">
                        </ClientSettings>
                    </dnn:dnngrid> 

                </div>

                <div id="dvPreferences" class="dnnClear">

                    <div class="connect_optionscol dnnLeft">

                        <p><asp:Literal ID="lblSearchOptions" runat="server"></asp:Literal></p>
                        <asp:CheckBoxList ID="chkSearchCols" runat="server"></asp:CheckBoxList>  

                    </div>

                    <div class="connect_optionscol dnnLeft">

                        <p><asp:Literal ID="lblGridSetup" runat="server"></asp:Literal></p>
                        <asp:CheckBoxList ID="chkGridOptions" runat="server">
                            <asp:ListItem Value="UserId"></asp:ListItem>
                            <asp:ListItem Value="DisplayName"></asp:ListItem>
                            <asp:ListItem Value="Username"></asp:ListItem>
                            <asp:ListItem Value="Email"></asp:ListItem>
                            <asp:ListItem Value="Country"></asp:ListItem>
                            <asp:ListItem Value="CreatedDate"></asp:ListItem>
                            <asp:ListItem Value="FirstName"></asp:ListItem>
                            <asp:ListItem Value="LastName"></asp:ListItem>
                            <asp:ListItem Value="PostalCode"></asp:ListItem>
                            <asp:ListItem Value="City"></asp:ListItem>
                            <asp:ListItem Value="Telephone"></asp:ListItem>
                            <asp:ListItem Value="Cell"></asp:ListItem>
                            <asp:ListItem Value="PreferredLocale"></asp:ListItem>
                        </asp:CheckBoxList>

                    </div>

                    <div class="connect_optionscol dnnLeft">

                        <p><asp:Literal ID="lblPageSize" runat="server"></asp:Literal></p>
                        <asp:dropdownlist ID="drpPageSize" runat="server">
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="75" Value="75"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="All" Value="ALL"></asp:ListItem>
                        </asp:dropdownlist> 

                    </div>

                    <div class="dnnClear">
                        <ul class="dnnActions">
                            <li><asp:Linkbutton ID="btnApplyOptions" runat="server" CssClass="dnnPrimaryAction" /></li>
                        </ul>
                    </div>
                    
                </div>  
                                            
            </div>

        </asp:Panel>

        <asp:Panel ID="pnlUser" runat="server" Visible="false">

            <div id="ConnectUserContainer" class="dnnClear">

                <ul id="UserDetailTabs" runat="Server" class="dnnAdminTabNav dnnClear">
                    <li id="tabAccount" runat="server"><a href="#dvAccount"><asp:Label id="lblAccountTab" runat="server" resourcekey="lblAccountTab" /></a></li>
                    <li id="tabPassword" runat="server"><a href="#dvPassword"><asp:Label id="lblPasswordTab" runat="server" resourcekey="lblPasswordTab" /></a></li>
                    <li id="tabProfile" runat="server"><a href="#dvProfile"><asp:Label id="lblProfileTab" runat="server" resourcekey="lblProfileTab" /></a></li>
                    <li id="tabRoles" runat="server"><a href="#dvRoles"><asp:Label id="lblRolesTab" runat="server" resourcekey="lblRolesTab" /></a></li>
                    <li id="tabEmail" runat="server"><a href="#dvEmail"><asp:Label id="lblEmailTab" runat="server" resourcekey="lblEmailTab" /></a></li>
                    <li id="tabSites" runat="server"><a href="#dvSites"><asp:Label id="lblSitesTab" runat="server" resourcekey="lblSitesTab" /></a></li>
                </ul>

                <div id="dvAccount" class="dnnClear">
                    <asp:Panel ID="pnlAccountTab" runat="server">

                    <p class="connect_note"><asp:Literal ID="lblAccountNote" runat="server"></asp:Literal></p>

                    <div class="dnnClear">

                        <div class="connect_col50 dnnLeft">
                            
                            <asp:PlaceHolder ID="plhUser" runat="server"></asp:PlaceHolder>

                        </div>

                        <div class="connect_col50 dnnLeft">

                            <table style="width:100%;" cellpadding="4" cellspacing="0">
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblMemberSinceLabel" runat="server" resourcekey="lblMemberSinceLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblMemberSince" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblLastLoginLabel" runat="server" resourcekey="lblLastLoginLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblLastLogin" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblLastActivityLabel" runat="server" resourcekey="lblLastActivityLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblLastActivity" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblLastPasswordChangeLabel" runat="server" resourcekey="lblLastPasswordChangeLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblLastPasswordChange" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblLastLockoutLabel" runat="server" resourcekey="lblLastLockoutLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblLastLockout" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblUserOnlineStatusLabel" runat="server" resourcekey="lblUserOnlineStatusLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblUserOnlineStatus" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblAuthorizedStatusLabel" runat="server" resourcekey="lblAuthorizedStatusLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblAuthorizedStatus" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblLockoutStatusLabel" runat="server" resourcekey="lblLockoutStatusLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblLockoutStatus" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <td class="connect_cellLabel"><asp:Label id="lblForcePasswordChangeLabel" runat="server" resourcekey="lblForcePasswordChangeLabel" /></td>
                                    <td class="connect_cellValue"><asp:Literal ID="lblForcePasswordChange" runat="server"></asp:Literal></td>
                                </tr>
                            </table>

                        </div>

                    </div>

                    <ul class="dnnActions">
                        <li><asp:LinkButton ID="cmdUpdateAccount" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdateAccount"></asp:LinkButton></li>
                        <li><asp:LinkButton ID="cmdUnlockAccount" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdUnlockAccount"></asp:LinkButton></li>
                        <li><asp:LinkButton ID="cmdAuthorizeAccount" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdAuthorizeAccount"></asp:LinkButton></li>
                        <li><asp:LinkButton ID="cmdForcePasswordChange" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdForcePasswordChange"></asp:LinkButton></li>                        
                        <li><asp:LinkButton ID="cmdDeleteAccount" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdDeleteAccount"></asp:LinkButton></li>
                    </ul>

                    </asp:Panel>
                </div>

                <div id="dvPassword" class="dnnClear">
                    <asp:Panel ID="pnlPasswordTab" runat="server">

                        <p class="connect_note"><asp:Literal ID="lblPasswordNote" runat="server"></asp:Literal></p>

                        <asp:Panel ID="pnlResetButton" runat="server">
                            <asp:LinkButton ID="cmdResetPasswordLink" runat="server" resourcekey="ResetPasswordLink" CssClass="dnnPrimaryAction"></asp:LinkButton>
                        </asp:Panel>

                        <asp:Panel ID="pnlPassword_Step1" runat="server">

                            <div class="dnnForm dnnClear">
	
	                            <div class="dnnFormItem dnnClear">
		                            <dnn:Label id="lblPassword1" runat="server"></dnn:Label>
                                    <asp:TextBox ID="txtPassword1" runat="server"></asp:TextBox>
	                            </div>

	                            <div class="dnnFormItem dnnClear">
		                            <dnn:Label id="lblPassword2" runat="server"></dnn:Label>
                                    <asp:TextBox ID="txtPassword2" runat="server"></asp:TextBox>
	                            </div>
		
                            </div>

                            <ul class="dnnActions">
                                <li><asp:LinkButton ID="cmdUpdatePassword" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdatePassword"></asp:LinkButton></li>
                            </ul>

                        </asp:Panel>

                        <asp:Panel ID="pnlPassword_Step2" runat="server" Visible="false">                                                  
                                                        
                            <div style="padding-bottom:10px;">
                                <dnn:dnnTextBox ID="txtNotifyPasswordSubject" runat="server" Width="598px" Height="25px"></dnn:dnnTextBox>
                            </div> 
                                                    
                            <div style="padding-top:20px;padding-bottom:20px;">
                                <dnn:dnnEditor ID="txtNotifyPasswordBody" runat="server" Width="600px" Height="275px"></dnn:dnnEditor>
                            </div>
                                  
                            <ul class="dnnActions">
                                <li><asp:Button ID="btnNotifyPassword" runat="server" CssClass="dnnPrimaryAction" /></li>
                                <li><asp:Button ID="btnNotifyPasswordSkip" runat="server" CssClass="dnnSecondaryAction" /></li>
                            </ul>
                                                                
                        </asp:Panel> 

                    </asp:Panel>
                </div>

                <div id="dvProfile" class="dnnClear">
                    <asp:Panel ID="pnlProfileTab" runat="server">

                    <p class="connect_note"><asp:Literal ID="lblProfileNote" runat="server"></asp:Literal></p>

                    <asp:PlaceHolder ID="plhProfile" runat="server"></asp:PlaceHolder>

                    <ul class="dnnActions">
                        <li><asp:LinkButton ID="cmdUpdateProfile" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdateProfile"></asp:LinkButton></li>
                    </ul>

                    </asp:Panel>
                </div>

                <div id="dvRoles" class="dnnClear">
                    <asp:Panel ID="pnlRolesTab" runat="server">

                        <p class="connect_note"><asp:Literal ID="lblRolesNote" runat="server"></asp:Literal></p>

                        <asp:Panel ID="pnlRoleChange_Step1" runat="server" Visible="true">
                            <div>
                                
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-bottom:20px;padding-right:25px;vertical-align:top;"><dnn:DNNComboBox ID="drpRoles" runat="server" Width="180px" AllowCustomText="true"></dnn:DNNComboBox></td>
                                        <td style="padding-bottom:20px;padding-right:5px;vertical-align:top;"><dnn:DNNDatePicker ID="ctlRoleDatFrom" runat="server"></dnn:DNNDatePicker></td>
                                        <td style="padding-bottom:20px;padding-right:5px;vertical-align:top;"><dnn:DNNDatePicker ID="ctlRoleDateTo" runat="server"></dnn:DNNDatePicker></td>
                                        <td style="padding-bottom:20px;padding-right:5px;vertical-align:top;"><asp:Button ID="btnAddToRole" runat="server" CssClass="dnnPrimaryAction" /></td>
                                    </tr>
                                </table>
                                
                            </div>
                            
                            <dnn:dnnGrid ID="grdUserRoles" runat="server">
                                <MasterTableView DataKeyNames="RoleId" AutoGenerateColumns="false">
                                    <Columns>
                                        <dnn:dnnGridBoundColumn HeaderText="RoleName" DataField="RoleName" UniqueName="col_RoleName"></dnn:dnnGridBoundColumn>
                                        <dnn:dnnGridBoundColumn HeaderText="EffectiveDate" DataField="EffectiveDate" UniqueName="col_EffectiveDate"></dnn:dnnGridBoundColumn>
                                        <dnn:dnnGridBoundColumn HeaderText="ExpiryDate" DataField="ExpiryDate" UniqueName="col_ExpiryDate"></dnn:dnnGridBoundColumn>
                                        <dnn:dnnGridTemplateColumn UniqueName="removeCol">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDeleteUserRole" runat="server" OnClick="btnDeleteUserRole_Click" CommandArgument='<%# Databinder.Eval(Container.DataItem, "RoleId") %>' />
                                            </ItemTemplate>
                                        </dnn:dnnGridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </dnn:dnnGrid>                        
                        
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlRoleChange_Step2" runat="server" Visible="false">                       
                                                        
                            <div style="padding-bottom:10px;">
                                <dnn:dnnTextBox ID="txtNotifyRoleSubject" runat="server" Width="598px" Height="25px"></dnn:dnnTextBox>
                            </div> 
                                                    
                            <div style="padding-top:20px;padding-bottom:20px;">
                                <dnn:dnnEditor ID="txtNotifyRoleBody" runat="server" Width="600px" Height="275px"></dnn:dnnEditor>
                            </div>
                            
                            
                            <ul class="dnnActions">
                                <li><asp:Button ID="btnNotifyRole" runat="server" Visible="true" CssClass="dnnPrimaryAction" /></li>
                                <li><asp:Button ID="btnNotifyRoleSkip" runat="server" Visible="true" CssClass="dnnSecondaryAction" /></li>
                            </ul>
                                                                    
                        </asp:Panel> 

                    </asp:Panel>
                </div>

                <div id="dvEmail" class="dnnClear">
                    <asp:Panel ID="pnlEmailTab" runat="server">
                    
                        <p class="connect_note"><asp:Literal ID="lblEmailNote" runat="server"></asp:Literal></p>

                        <div style="padding-bottom:10px;">
                            <dnn:dnnTextBox ID="txtNotifyUserSubject" runat="server" Width="598px" Height="25px"></dnn:dnnTextBox>
                        </div> 
                                                    
                        <div style="padding-top:20px;padding-bottom:20px;">
                            <dnn:dnnEditor ID="txtNotifyUserBody" runat="server" Width="600px" Height="275px"></dnn:dnnEditor>
                        </div>
                            
                            
                        <ul class="dnnActions">
                            <li><asp:Button ID="btnNotifyUser" runat="server" Visible="true" CssClass="dnnPrimaryAction" /></li>
                        </ul>

                    </asp:Panel>
                </div>

                <div id="dvSites" class="dnnClear">
                    <asp:Panel ID="pnlSitesTab" runat="server">

                    <p class="connect_note"><asp:Literal ID="lblSitesNote" runat="server"></asp:Literal></p>

                    <div style="padding-bottom:20px;">
                        <asp:CheckBoxList id="chkUserSites" runat="server" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="OrderedList"></asp:CheckBoxList>
                    </div>

                    <ul class="dnnActions">
                        <li><asp:Linkbutton ID="cmdUpdateSites" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdUpdateSites" /></li>
                    </ul>                                


                    </asp:Panel>
                </div>

            </div>

        </asp:Panel>

        <asp:Panel ID="pnlCreate" runat="server" Visible="false">

            <p class="connect_note"><asp:Literal ID="lblCreateAccountNote" runat="server"></asp:Literal></p>
            
            <div class="dnnClear">
                <asp:PlaceHolder ID="plhCreate" runat="server"></asp:PlaceHolder>
            </div>

            <ul class="dnnActions">
                <li><asp:LinkButton ID="cmdAddAccount" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdAddAccount"></asp:LinkButton></li>
                <li><asp:LinkButton ID="cmdCancelCreate" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdCancelCreate"></asp:LinkButton></li>
            </ul>

        </asp:Panel>
    </div>
                     
</div>

<script language="javascript" type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {

        function setupFormSettings() {
            $('#ConnectGridContainer').dnnTabs();
            $('#ConnectUserContainer').dnnTabs();            
        }

        $(document).ready(function () {
            setupFormSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupFormSettings();
            });
        });

    }(jQuery, window.Sys));
</script>



