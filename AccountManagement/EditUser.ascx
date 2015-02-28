<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditUser.ascx.vb" Inherits="Connect.Modules.Accounts.AccountManagement.EditUser" %>

<div class="connect-accounts-editform">
    
    <div class="row">

        <div class="col-lg-12">
            <h2 class="page-header">
                <asp:HyperLink ID="cmdBack_Top" runat="server"><i class="fa fa-chevron-left"></i></asp:HyperLink>
                <asp:Literal ID="lblEditHead" runat="server"></asp:Literal>
                <asp:Image ID="imgUser" runat="server" />
            </h2>
        </div>

    </div>

    <div class="row">

        <div class="col-lg-6 col-md-6">
        
            <div class="panel-group" id="userMain">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userMain" href="#collapseUser" aria-expanded="true" class="">
                                <asp:Literal ID="lblAccountData" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseUser" class="panel-collapse collapse in" aria-expanded="true">
                        <div class="panel-body">

                            <p><asp:Literal ID="lblAccountInfoText" runat="server"></asp:Literal></p>

                            <div class="form-group">
                                <label><asp:Literal ID="lblUsername" runat="server"></asp:Literal></label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label><asp:Literal ID="lblEmail" runat="server"></asp:Literal></label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label><asp:Literal ID="lblFirstname" runat="server"></asp:Literal></label>
                                <asp:TextBox ID="txtFirstname" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label><asp:Literal ID="lblLastname" runat="server"></asp:Literal></label>
                                <asp:TextBox ID="txtLastname" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label><asp:Literal ID="lblDisplayname" runat="server"></asp:Literal></label>
                                <asp:TextBox ID="txtDisplayname" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="col-lg-6 col-md-6">

            <div class="panel-group" id="userInfos">

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userInfos" href="#collapseInfo" aria-expanded="true" class="">
                                <asp:Literal ID="lblInfoHeading" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseInfo" class="panel-collapse collapse in" aria-expanded="true">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover">
                                    <tbody>
                                        <tr>
                                            <td><asp:Literal ID="lblMemberSince" runat="server"></asp:Literal></td>
                                            <td><asp:Literal ID="lblMemberSinceValue" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblLastlogin" runat="server"></asp:Literal></td>
                                            <td><asp:Literal ID="lblLastloginValue" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblLastActivity" runat="server"></asp:Literal></td>
                                            <td><asp:Literal ID="lblLastActivityValue" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblLastLockout" runat="server"></asp:Literal></td>
                                            <td><asp:Literal ID="lblLastLockoutValue" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblCurrentActivity" runat="server"></asp:Literal></td>
                                            <td><asp:Literal ID="lblCurrentActivityValue" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblLockStatus" runat="server"></asp:Literal></td>
                                            <td><asp:Literal ID="lblLockStatusValue" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblAuthorized" runat="server"></asp:Literal></td>
                                            <td><asp:CheckBox ID="chkAuthorized" runat="server" CssClass="chkIsAuthorized" /></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Literal ID="lblForcePasswordChange" runat="server"></asp:Literal></td>
                                            <td><asp:CheckBox ID="chkForcePasswordChange" runat="server" CssClass="chkForcePasswordChange" /></td>
                                        </tr>
                                    </tbody>
                                </table>  
                            </div>                          
                        </div>
                    </div>
                </div>
                                
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userInfos" href="#collapseRoles" aria-expanded="false" class="">
                                <asp:Literal ID="lblRolesHeading" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseRoles" class="panel-collapse collapse" aria-expanded="false">
                        <div class="panel-body">
                            Change Roles
                        </div>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userInfos" href="#collapsePassword" aria-expanded="false" class="">
                                <asp:Literal ID="lblPasswordHeading" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapsePassword" class="panel-collapse collapse" aria-expanded="false">
                        <div class="panel-body">
                            Change Password
                        </div>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userInfos" href="#collapseProfile" aria-expanded="false" class="">
                                <asp:Literal ID="lblProfileHeading" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseProfile" class="panel-collapse collapse" aria-expanded="false">
                        <div class="panel-body">
                            Change Profile
                        </div>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userInfos" href="#collapseSites" aria-expanded="false" class="">
                                <asp:Literal ID="lblSitesHeading" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseSites" class="panel-collapse collapse" aria-expanded="false">
                        <div class="panel-body">
                            Change Sites
                        </div>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#userInfos" href="#collapseMessaging" aria-expanded="false" class="">
                                <asp:Literal ID="lblMessagingHeading" runat="server"></asp:Literal>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseMessaging" class="panel-collapse collapse" aria-expanded="false">
                        <div class="panel-body">
                            Send a message
                        </div>
                    </div>
                </div>


        </div>

    </div>

    </div>

    <div class="row">

        <div class="col-lg-12 col-md-12 actions">
            <asp:LinkButton ID="cmdUpdate" runat="server" CssClass="btn btn-primary"></asp:LinkButton>
            <asp:LinkButton ID="cmdDelete" runat="server" CssClass="btn btn-default"><i class="fa fa-bitbucket"></i></asp:LinkButton>
            <asp:Hyperlink ID="cmdBack_Bottom" runat="server" CssClass="btn btn-default"><i class="fa fa-bitbucket"></i></asp:Hyperlink>
        </div>

        <div class="modal fade" id="mdProcess" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel">Modal title</h4>
                    </div>
                    <div class="modal-body">
                        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>

    </div>



    <script type="text/javascript">

        /*globals jQuery, window, Sys */
        (function ($, Sys) {

            function setupPage() {
                $("#<%= chkAuthorized.ClientId %>").bootstrapSwitch({
                    onText:     '<%= Localization.GetString("yes", LocalResourceFile)%>',
                    offText:    '<%= Localization.GetString("no", LocalResourceFile)%>',
                    size:       'mini',
                    onSwitchChange: setAuthorized
                });
                $("#<%= chkForcePasswordChange.ClientId %>").bootstrapSwitch({
                    onText: '<%= Localization.GetString("yes", LocalResourceFile)%>',
                    offText: '<%= Localization.GetString("no", LocalResourceFile)%>',
                    size: 'mini',
                    onSwitchChange: setForcePasswordChange
                });
            }

            function setAuthorized(event, state) {
                //alert(state);
            }

            function setForcePasswordChange(event, state) {
                //alert(state);
            }

            function updateUser() {
                
                var profile = {
                    firstname: <%= txtFirstname.ClientID%>.val(),
                    lastname: <%= txtLastname.ClientID%>.val(),
                };

                var membership = {
                    approved: <%= chkAuthorized.ClientID%>.val(),
                    forcepasswordchange: <%= chkForcePasswordChange.ClientID%>.val()
                };

                var user = {
                    userid: 1,
                    username: <%= txtUsername.ClientId%>.val(),
                    firstname: <%= txtFirstname.ClientID%>.val(),
                    lastname: <%= txtLastname.ClientID%>.val(),
                    displayname: <%= txtDisplayname.ClientID%>.val(),
                    email: <%= txtEmail.ClientID%>.val(),
                    profile: profile,
                    membership: membership
                };



            }

            function showProcess() {
                $('#mdProcess').modal();
            }

            $(document).ready(function () {
                
                setupPage();

                Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
                    showProcess();
                });

                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    setupPage();
                });

            });

        }(jQuery, window.Sys));

    </script>