'***********************************************************************************
'  Connect UsersLibrary
'
'  Copyright (C) 2013-2014 DNN-Connect Association, Philipp Becker
'  http://dnn-connect.org
'  
'  This program is free software; you can redistribute it and/or
'  modify it under the terms of the GNU General Public License
'  as published by the Free Software Foundation; either version 2
'  of the License, or (at your option) any later version.
'  
'  This program is distributed in the hope that it will be useful,
'  but WITHOUT ANY WARRANTY; without even the implied warranty of
'  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'  GNU General Public License for more details.
'  
'  You should have received a copy of the GNU General Public License
'  along with this program; if not, write to the Free Software
'  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
'
'***********************************************************************************
Imports DotNetNuke.Entities.Modules
Imports Connect.Libraries.UserManagement
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership
Imports Telerik.Web.UI
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Web.UI.WebControls
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace Connect.Modules.UserManagement.AccountManagement

    Partial Class View
        Inherits ConnectUsersModuleBase
        Implements IActionable

#Region "Private Members"

        Private _IsReportResult As Boolean = False

#End Region

#Region "Event Handlers"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


            If Request.QueryString("RoleId") Is Nothing Then
                If Me.PreSelectRole <> Null.NullInteger Then
                    Response.Redirect(NavigateURL(TabId, "", "RoleId=" & Me.PreSelectRole.ToString))
                Else
                    Response.Redirect(NavigateURL(TabId, "", "RoleId=" & Me.PortalSettings.RegisteredRoleId.ToString.ToString))
                End If
            End If

            ProcessQuerystring() 'watch out for querystring actions

            JavaScript.RequestRegistration(CommonJs.DnnPlugins)
            JavaScript.RequestRegistration(CommonJs.jQueryUI)

            InitializeForm()

            ProcessFormTemplate(plhUser, GetTemplate(ModuleTheme, Constants.TemplateName_AccountForm, CurrentLocale, False), User)
            ProcessFormTemplate(plhProfile, GetTemplate(ModuleTheme, Constants.TemplateName_ProfileForm, CurrentLocale, False), User)
            ProcessFormTemplate(plhCreate, GetTemplate(ModuleTheme, Constants.TemplateName_CreateForm, CurrentLocale, False), Nothing)

        End Sub

        Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then

                'make sure txtSearch is empty on non-postback requests
                txtSearch.Text = ""

                BindRoles()
                BindSearchColOptions()
                PersonalizeOptions()
                BindReports()

                pnlBackToList.Visible = False

                If Not Request.QueryString("uid") Is Nothing Then
                    If IsNumeric(Request.QueryString("uid")) Then
                        BindUser(Convert.ToInt32(Request.QueryString("uid")))
                    End If
                End If

            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        End Sub

        Private Sub grdUsers_PreRender(sender As Object, e As EventArgs) Handles grdUsers.PreRender
            grdUsers.ClientSettings.Scrolling.AllowScroll = False
        End Sub

        Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click

            _IsReportResult = True

            If drpReports.SelectedIndex > 0 Then
                ctlRoles.UnselectAllNodes()
                grdUsers.Rebind()
            End If

            Session("UserReportsId") = drpReports.SelectedValue

            pnlGrid.Visible = True
            pnlUser.Visible = False
            pnlCreate.Visible = False

        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        End Sub

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        End Sub

        Private Sub grdUsers_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grdUsers.NeedDataSource
            BindUsers()
        End Sub

        Public Function GetStatusText(strStatus As Object) As String
            Select Case strStatus.ToString
                Case "1"
                    Return Localization.GetString("RoleStatusApproved", LocalResourceFile)
                Case "0"
                    Return Localization.GetString("RoleStatusNoStatus", LocalResourceFile)
                Case "-1"
                    Return Localization.GetString("RoleStatusPending", LocalResourceFile)
            End Select
            Return Localization.GetString("RoleStatusUnknown", LocalResourceFile)
        End Function

        Protected Sub grdUsers_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdUsers.ItemDataBound
            If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

                Try

                    Dim dataBoundItem As GridDataItem = e.Item
                    Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
                    Dim intUser As String = Convert.ToInt32(row("UserID"))
                    Dim intRole As Integer = Convert.ToInt32(ctlRoles.SelectedNode.Value)

                    Dim btnHardDelete As HtmlGenericControl = CType(e.Item.FindControl("btnHardDelete"), HtmlGenericControl)
                    If Not btnHardDelete Is Nothing Then
                        btnHardDelete.Visible = False
                        If intRole = -2 Then
                            btnHardDelete.Visible = True
                        End If
                    End If

                    Dim btnRemove As HtmlGenericControl = CType(e.Item.FindControl("btnRemove"), HtmlGenericControl)
                    If Not btnRemove Is Nothing Then
                        btnRemove.Visible = False
                        If intRole <> -2 AndAlso intRole <> PortalSettings.RegisteredRoleId Then
                            btnRemove.Visible = True
                        End If
                    End If

                    Dim btnSetStatus As HtmlGenericControl = CType(e.Item.FindControl("btnSetStatus"), HtmlGenericControl)
                    If Not btnSetStatus Is Nothing Then
                        btnSetStatus.Visible = False
                        If intRole <> -2 AndAlso intRole <> PortalSettings.RegisteredRoleId Then
                            btnSetStatus.Visible = True
                        End If
                    End If

                    Dim btnSetDeleted As HtmlGenericControl = CType(e.Item.FindControl("btnSetDeleted"), HtmlGenericControl)
                    If Not btnSetDeleted Is Nothing Then
                        btnSetDeleted.Visible = False
                        If AllowDelete AndAlso intUser <> PortalSettings.AdministratorId AndAlso intUser <> UserInfo.UserID AndAlso (Not ctlRoles.SelectedNode Is Nothing AndAlso ctlRoles.SelectedNode.Value <> "-2") Then
                            btnSetDeleted.Visible = True
                        End If
                    End If

                    Dim btnRestore As HtmlGenericControl = CType(e.Item.FindControl("btnRestore"), HtmlGenericControl)
                    If Not btnRestore Is Nothing Then
                        btnRestore.Visible = False
                        If (Not ctlRoles.SelectedNode Is Nothing AndAlso ctlRoles.SelectedNode.Value = "-2") Then
                            btnRestore.Visible = True
                        End If
                    End If

                Catch ex As Exception

                End Try

            End If
        End Sub

        Private Sub btnApplyOptions_Click(sender As Object, e As EventArgs) Handles btnApplyOptions.Click

            SaveGridOptions()
            SaveSearchOptions()

            If Not Request.QueryString("RoleId") Is Nothing Then
                Response.Redirect(NavigateURL(TabId, "", "RoleId=" & Request.QueryString("RoleId")))
            Else
                Response.Redirect(NavigateURL(TabId))
            End If

        End Sub

        Private Sub cmdBack_Click(sender As Object, e As EventArgs) Handles cmdBack.Click

            Dim url As String = NavigateURL(TabId)

            If Not Request.QueryString("RoleId") Is Nothing Then
                url = NavigateURL(TabId, "", "RoleId=" & Request.QueryString("RoleId"))
            End If

            Response.Redirect(url)

        End Sub

        Protected Sub grdUserRoles_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdUserRoles.NeedDataSource

            Dim roleController As New RoleController
            Dim userRoles As IList = roleController.GetUserRoles(PortalId, User.Username, "")

            grdUserRoles.DataSource = userRoles

        End Sub

        Protected Sub grdUserRoles_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdUserRoles.ItemDataBound
            If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

                Dim dataBoundItem As GridDataItem = CType(e.Item, GridDataItem)

                Dim expiryDate As Date = Date.Parse(dataBoundItem("col_ExpiryDate").Text)
                If expiryDate = Null.NullDate Then
                    dataBoundItem("col_ExpiryDate").Text = "-"
                Else
                    dataBoundItem("col_ExpiryDate").Text = expiryDate.ToShortDateString
                End If

                Dim effectiveDate As Date = Date.Parse(dataBoundItem("col_EffectiveDate").Text)
                If effectiveDate = Null.NullDate Then
                    dataBoundItem("col_EffectiveDate").Text = User.Membership.CreatedDate.ToShortDateString
                Else
                    dataBoundItem("col_EffectiveDate").Text = effectiveDate.ToShortDateString
                End If

                Dim btnApprove As ImageButton = CType(dataBoundItem("statusCol").FindControl("btnApproveUserRole"), ImageButton)
                btnApprove.ImageUrl = ResolveUrl("~/images/grant.gif")
                Dim currentRoleId As Integer = CType(btnApprove.CommandArgument, Integer)
                Dim roleController As New RoleController
                Dim userRoles As IList = roleController.GetUserRoles(PortalId, User.Username, "")
                For Each userRole As UserRoleInfo In userRoles
                    If userRole.RoleID = currentRoleId Then
                        If userRole.Status = RoleStatus.Approved Then
                            btnApprove.Visible = False
                        End If
                    End If
                Next

                Dim btn As ImageButton = CType(dataBoundItem("removeCol").FindControl("btnDeleteUserRole"), ImageButton)
                btn.ImageUrl = ResolveUrl("~/images/delete.gif")

                If btn.CommandArgument = PortalSettings.RegisteredRoleId.ToString Then
                    btn.Visible = False
                End If

                If btn.CommandArgument = PortalSettings.AdministratorRoleId.ToString Then
                    btn.Visible = (User.UserID <> PortalSettings.AdministratorId)
                End If


            End If
        End Sub

        Private Sub btnAddToRole_Click(sender As Object, e As EventArgs) Handles btnAddToRole.Click

            Dim roleController As New RoleController

            Dim effectiveDate As Date = Date.Now
            If Not ctlRoleDatFrom.DbSelectedDate Is Nothing Then
                effectiveDate = ctlRoleDatFrom.DbSelectedDate
            End If

            Dim expiryDate As Date = Null.NullDate
            If Not ctlRoleDateTo.DbSelectedDate Is Nothing Then
                expiryDate = ctlRoleDateTo.DbSelectedDate
            End If

            Dim roleId As Integer = Null.NullInteger
            If drpRoles.SelectedItem.Value <> "-1" Then
                roleId = Convert.ToInt32(drpRoles.SelectedItem.Value)
            End If

            If roleId <> Null.NullInteger Then

                roleController.AddUserRole(PortalId, User.UserID, roleId, effectiveDate, expiryDate)
                lblRolesNote.Text = Localization.GetString("lblNotificationNote_Roles", LocalResourceFile)
                BindRoleMembershipChangedNotification(drpRoles.SelectedItem.Text, Constants.TemplateName_EmailAddedToRole, effectiveDate, expiryDate)

                DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & roleId.ToString)

                pnlRoleChange_Step1.Visible = False
                pnlRoleChange_Step2.Visible = True

                btnNotifyRole.CommandArgument = "add"
                btnNotifyRoleSkip.CommandArgument = "add"

            End If


        End Sub

        Protected Sub btnDeleteUserRole_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

            Dim roleId As Integer = Convert.ToInt32(CType(sender, ImageButton).CommandArgument)
            Dim roleController As New RoleController
            Dim role As RoleInfo = roleController.GetRole(roleId, PortalId)

            roleController.DeleteUserRole(User, role, PortalSettings, False)

            DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & roleId.ToString)

            Dim strRole As String = roleController.GetRole(roleId, PortalId).RoleName
            lblRolesNote.Text = Localization.GetString("lblNotificationNote_Roles", LocalResourceFile)
            BindRoleMembershipChangedNotification(role.RoleName, Constants.TemplateName_EmailRemovedFromRole, Null.NullDate, Null.NullDate)

            pnlRoleChange_Step1.Visible = False
            pnlRoleChange_Step2.Visible = True
            btnNotifyRole.CommandArgument = "remove"
            btnNotifyRoleSkip.CommandArgument = "remove"

        End Sub

        Protected Sub btnApproveUserRole_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

            Dim roleId As Integer = Convert.ToInt32(CType(sender, ImageButton).CommandArgument)
            Dim roleController As New RoleController
            Dim role As RoleInfo = roleController.GetRole(roleId, PortalId)

            roleController.UpdateUserRole(PortalId, User.UserID, roleId, RoleStatus.Approved, False, False)

            DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & roleId.ToString)

            Dim strRole As String = roleController.GetRole(roleId, PortalId).RoleName
            lblRolesNote.Text = Localization.GetString("lblNotificationNote_Roles", LocalResourceFile)
            BindRoleMembershipChangedNotification(role.RoleName, Constants.TemplateName_EmailRoleStatusChanged, Date.Now, Null.NullDate)

            pnlRoleChange_Step1.Visible = False
            pnlRoleChange_Step2.Visible = True
            btnNotifyRole.CommandArgument = "approve"
            btnNotifyRoleSkip.CommandArgument = "approve"

        End Sub

        Private Sub btnNotifyRoleSkip_Click(sender As Object, e As EventArgs) Handles btnNotifyRoleSkip.Click

            lblRolesNote.Text = Localization.GetString("lblRolesChanged", LocalResourceFile)
            grdUserRoles.Rebind()
            pnlRoleChange_Step1.Visible = True
            pnlRoleChange_Step2.Visible = False

        End Sub

        Private Sub btnNotifyRole_Click(sender As Object, e As EventArgs) Handles btnNotifyRole.Click

            Dim strPassword As String = Localization.GetString("HiddenPassword", LocalResourceFile)
            If MembershipProvider.Instance.PasswordRetrievalEnabled Then
                strPassword = MembershipProvider.Instance().GetPassword(User, "")
            End If

            Dim strBody As String = txtNotifyRoleBody.Content.Replace(Localization.GetString("HiddenPassword", LocalResourceFile), strPassword)
            Dim strSubject As String = txtNotifyRoleSubject.Text
            If strSubject = "" Then
                strSubject = Localization.GetString("txtNotifyRoleSubject", LocalResourceFile)
            End If

            DotNetNuke.Services.Mail.Mail.SendEmail(PortalSettings.Email, User.Email, strSubject, strBody)

            lblRolesNote.Text = Localization.GetString("lblRolesChanged", LocalResourceFile)
            grdUserRoles.Rebind()
            pnlRoleChange_Step1.Visible = True
            pnlRoleChange_Step2.Visible = False

        End Sub

        Private Sub cmdUpdateAccount_Click(sender As Object, e As EventArgs) Handles cmdUpdateAccount.Click
            UpdateAccount()
            ProcessFormTemplate(plhUser, GetTemplate(ModuleTheme, Constants.TemplateName_AccountForm, CurrentLocale, False), User)
        End Sub

        Private Sub cmdUpdateProfile_Click(sender As Object, e As EventArgs) Handles cmdUpdateProfile.Click
            UpdateProfile()
            ProcessFormTemplate(plhProfile, GetTemplate(ModuleTheme, Constants.TemplateName_ProfileForm, CurrentLocale, False), User)
        End Sub

        Private Sub cmdUpdateSites_Click(sender As Object, e As EventArgs) Handles cmdUpdateSites.Click

            Dim uid As Integer = Convert.ToInt32(Request.QueryString("uid"))
            Dim objCurrentUser As UserInfo = UserController.GetUserById(PortalId, uid)
            Dim blnErrorOccured As Boolean = False

            For Each cItem As ListItem In chkUserSites.Items

                Dim pCtrl As New PortalController
                Dim objPortal As PortalInfo = pCtrl.GetPortal(Convert.ToInt32(cItem.Value))
                If Not objPortal Is Nothing Then
                    Dim objPortalUser As UserInfo = UserController.GetUserById(objPortal.PortalID, uid)

                    Try
                        If cItem.Selected Then

                            If cItem.Enabled Then
                                If objPortalUser Is Nothing Then
                                    UserController.CopyUserToPortal(objCurrentUser, objPortal, False, False)
                                End If
                            End If

                        Else

                            If Not objPortalUser Is Nothing Then
                                UserController.RemoveUser(objPortalUser)
                            End If

                        End If
                    Catch ex As Exception
                        blnErrorOccured = True
                    End Try


                End If

            Next

            If blnErrorOccured Then
                lblSitesNote.Text = Localization.GetString("SitesError", LocalResourceFile)
            Else
                lblSitesNote.Text = Localization.GetString("SitesSuccess", LocalResourceFile)
            End If

            BindUserSites(uid)

        End Sub

        Private Sub cmdForcePasswordChange_Click(sender As Object, e As EventArgs) Handles cmdForcePasswordChange.Click
            Dim oUser As UserInfo = User
            oUser.Membership.UpdatePassword = True
            UserController.UpdateUser(PortalId, oUser)
            BindUser(oUser.UserID)
        End Sub

        Private Sub cmdUnlockAccount_Click(sender As Object, e As EventArgs) Handles cmdUnlockAccount.Click
            Dim oUser As UserInfo = User
            oUser.Membership.LockedOut = False
            UserController.UpdateUser(PortalId, oUser)
            BindUser(oUser.UserID)
        End Sub

        Private Sub cmdAuthorizeAccount_Click(sender As Object, e As EventArgs) Handles cmdAuthorizeAccount.Click
            Dim oUser As UserInfo = User
            oUser.Membership.Approved = True
            UserController.UpdateUser(PortalId, oUser)
            BindUser(oUser.UserID)
        End Sub

        Private Sub cmdRestoreAccount_Click(sender As Object, e As EventArgs) Handles cmdRestoreAccount.Click

            If Request.IsAuthenticated = False Then
                Exit Sub
            End If

            Dim TargetUserId As Integer = Null.NullInteger
            If Request.QueryString("uid") Is Nothing Then
                Exit Sub
            Else
                If IsNumeric(Request.QueryString("uid")) Then
                    TargetUserId = Convert.ToInt32(Request.QueryString("uid"))
                End If
            End If

            If TargetUserId = Null.NullInteger Then
                Exit Sub
            End If

            Dim TargetRoleId As Integer = Null.NullInteger
            If Request.QueryString("RoleId") Is Nothing Then
                Exit Sub
            Else
                If IsNumeric(Request.QueryString("RoleId")) Then
                    TargetRoleId = Convert.ToInt32(Request.QueryString("RoleId"))
                End If
            End If

            Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

            If oUser Is Nothing Then
                Exit Sub
            End If

            UserController.RestoreUser(oUser)
            ClearCache()

            Response.Redirect(NavigateURL(TabId, "", "uid=" & oUser.UserID.ToString, "RoleId=" & TargetRoleId.ToString, "Action=Edit"))

        End Sub

        Private Sub cmdDeleteAccount_Click(sender As Object, e As EventArgs) Handles cmdDeleteAccount.Click

            If Request.IsAuthenticated = False Then
                Exit Sub
            End If

            Dim TargetUserId As Integer = Null.NullInteger
            If Request.QueryString("uid") Is Nothing Then
                Exit Sub
            Else
                If IsNumeric(Request.QueryString("uid")) Then
                    TargetUserId = Convert.ToInt32(Request.QueryString("uid"))
                End If
            End If

            If TargetUserId = Null.NullInteger Then
                Exit Sub
            End If

            Dim TargetRoleId As Integer = Null.NullInteger
            If Request.QueryString("RoleId") Is Nothing Then
                Exit Sub
            Else
                If IsNumeric(Request.QueryString("RoleId")) Then
                    TargetRoleId = Convert.ToInt32(Request.QueryString("RoleId"))
                End If
            End If

            Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

            If oUser Is Nothing Then
                Exit Sub
            End If

            If oUser.IsDeleted Then
                UserController.RemoveUser(oUser)
            Else
                UserController.DeleteUser(oUser, False, False)
            End If

            ClearCache()

            Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))

        End Sub

        Private Sub cmdCreateAccount_Click(sender As Object, e As EventArgs) Handles cmdCreateAccount.Click
            BindUserCreateForm()
            pnlCreateAccount.Visible = False
            pnlBackToList.Visible = True
        End Sub

        Private Sub cmdAddAccount_Click(sender As Object, e As EventArgs) Handles cmdAddAccount.Click
            AddAccount()
        End Sub

        Private Sub btnHardDelete_Click(sender As Object, e As EventArgs) Handles btnHardDelete.Click

            UserController.RemoveDeletedUsers(PortalId)
            ClearCache()
            grdUsers.Rebind()

        End Sub

        Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
            Export()
        End Sub

        Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

            If txtSearch.Text.Length > 0 Then

                'Session("Connect_UserSearchTerm") = txtSearch.Text

                SaveSearchOptions()
                pnlGrid.Visible = True
                pnlUser.Visible = False
                pnlCreate.Visible = False
                grdUsers.Rebind()

            End If

        End Sub

        Private Sub cmdCancelCreate_Click(sender As Object, e As EventArgs) Handles cmdCancelCreate.Click
            Response.Redirect(NavigateURL(TabId, "", "RoleId=" & Request.QueryString("RoleId")))
        End Sub

        Private Sub cmdBulkDelete_Click(sender As Object, e As EventArgs) Handles cmdBulkDelete.Click

            If AllowDelete Then

                For i As Integer = 0 To grdUsers.SelectedItems.Count - 1

                    Dim intUser As Integer = Null.NullInteger

                    Try

                        Dim selecteditem As GridItem = grdUsers.SelectedItems(i)
                        Dim selectedvalue As String = selecteditem.OwnerTableView.DataKeyValues(selecteditem.ItemIndex)("UserId")
                        intUser = Convert.ToInt32(selectedvalue)

                        If intUser <> Null.NullInteger Then

                            Dim oUser As UserInfo = UserController.GetUserById(PortalId, intUser)

                            If Not oUser Is Nothing AndAlso Not oUser.IsSuperUser AndAlso Not oUser.UserID = PortalSettings.AdministratorId Then
                                UserController.DeleteUser(oUser, False, False)
                            End If

                        End If
                    Catch
                    End Try

                Next

                ClearCache()
                grdUsers.Rebind()

            End If

        End Sub

        Private Sub cmdBulkRemove_Click(sender As Object, e As EventArgs) Handles cmdBulkRemove.Click

            Dim rc As New RoleController
            Dim intRole As Integer = Convert.ToInt32(Request.QueryString("RoleId"))
            Dim role As RoleInfo = rc.GetRole(intRole, PortalId)

            For i As Integer = 0 To grdUsers.SelectedItems.Count - 1

                Dim intUser As Integer = Null.NullInteger

                Try

                    Dim selecteditem As GridItem = grdUsers.SelectedItems(i)
                    Dim selectedvalue As String = selecteditem.OwnerTableView.DataKeyValues(selecteditem.ItemIndex)("UserId")
                    intUser = Convert.ToInt32(selectedvalue)

                    If intUser <> Null.NullInteger Then

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, intUser)

                        If Not oUser Is Nothing AndAlso Not oUser.IsSuperUser AndAlso Not (oUser.UserID = PortalSettings.AdministratorId And intRole = PortalSettings.AdministratorRoleId) Then
                            RoleController.DeleteUserRole(oUser, role, PortalSettings, False)
                        End If

                    End If

                Catch
                End Try

            Next

            ClearCache()
            grdUsers.Rebind()

        End Sub

#End Region

#Region "Account E-Mail Event handlers"

        Private Sub btnNotifyUser_Click(sender As Object, e As EventArgs) Handles btnNotifyUser.Click

            Dim strPassword As String = Localization.GetString("HiddenPassword", LocalResourceFile)
            If MembershipProvider.Instance.PasswordRetrievalEnabled Then
                strPassword = MembershipProvider.Instance().GetPassword(User, "")
            End If

            Dim strBody As String = txtNotifyUserBody.Content.Replace(Localization.GetString("HiddenPassword", LocalResourceFile), strPassword)
            Dim strSubject As String = txtNotifyUserSubject.Text
            If strSubject = "" Then
                strSubject = Localization.GetString("txtNotifyUserSubject", LocalResourceFile)
            End If

            Try

                DotNetNuke.Services.Mail.Mail.SendEmail(PortalSettings.Email, User.Email, strSubject, strBody)
                lblEmailNote.Text = Localization.GetString("MessageSent", LocalResourceFile)

            Catch ex As Exception

                lblEmailNote.Text = String.Format(Localization.GetString("MessageNotSent", LocalResourceFile), ex.Message)

            End Try

        End Sub

#End Region

#Region "Password Update Event Handlers"

        Private Sub cmdResetPasswordLink_Click(sender As Object, e As EventArgs) Handles cmdResetPasswordLink.Click

            pnlResetButton.Visible = False
            pnlPassword_Step1.Visible = False
            pnlPassword_Step2.Visible = True

            BindPasswordNotification()

        End Sub

        Private Sub cmdUpdatePassword_Click(sender As Object, e As EventArgs) Handles cmdUpdatePassword.Click

            Dim blnProceed As Boolean = True

            'verifiy passwords have been entered and both passwords match

            If txtPassword1.Text = txtPassword2.Text Then
                If UserController.ValidatePassword(txtPassword1.Text) Then
                    blnProceed = True
                Else
                    Me.lblPasswordNote.Text = Localization.GetString("PasswordPoliciesError", LocalResourceFile)
                    blnProceed = False
                End If
            Else
                Me.lblPasswordNote.Text = Localization.GetString("PasswordMatchError", LocalResourceFile)
                blnProceed = False
            End If

            If blnProceed Then

                Dim strPassword As String = ""
                strPassword = txtPassword1.Text

                Try

                    If UserController.ChangePassword(User, "", strPassword) Then

                        lblPasswordNote.Text = Localization.GetString("PasswordSetNotifyQuestion", LocalResourceFile)

                        pnlPassword_Step1.Visible = False
                        pnlPassword_Step2.Visible = True

                        BindPasswordNotification()

                    Else

                        Me.lblPasswordNote.Text = Localization.GetString("PasswordResetError", LocalResourceFile)

                    End If

                Catch ex As Exception

                    Me.lblPasswordNote.Text = Localization.GetString("CannotUsePassword", LocalResourceFile)

                End Try


            End If

        End Sub

        Private Sub btnNotifyPassword_Click(sender As Object, e As EventArgs) Handles btnNotifyPassword.Click

            Dim strPassword As String = Localization.GetString("HiddenPassword", LocalResourceFile)

            If MembershipProvider.Instance.PasswordRetrievalEnabled Then
                strPassword = MembershipProvider.Instance().GetPassword(User, "")
            End If



            Dim strBody As String = txtNotifyPasswordBody.Content.Replace(Localization.GetString("HiddenPassword", LocalResourceFile), strPassword)

            Dim strSubject As String = txtNotifyPasswordSubject.Text
            If strSubject = "" Then
                strSubject = Localization.GetString("txtNotifyPasswordSubject", LocalResourceFile)
            End If

            Try

                DotNetNuke.Services.Mail.Mail.SendEmail(PortalSettings.Email, User.Email, strSubject, strBody)

                Me.lblPasswordNote.Text = Localization.GetString("UserNotifiedPassword", LocalResourceFile)
                If MembershipProvider.Instance.PasswordRetrievalEnabled = False AndAlso MembershipProvider.Instance.PasswordResetEnabled = True Then
                    Me.lblPasswordNote.Text = Localization.GetString("UserNotifiedReset", LocalResourceFile)
                End If


            Catch ex As Exception

                Me.lblPasswordNote.Text = String.Format(Localization.GetString("MessageNotSent.Text", LocalResourceFile), ex.Message)

            End Try

            If MembershipProvider.Instance.PasswordRetrievalEnabled = False AndAlso MembershipProvider.Instance.PasswordResetEnabled = True Then

                pnlResetButton.Visible = True
                pnlPassword_Step1.Visible = False
                pnlPassword_Step2.Visible = False

                txtPassword1.Text = ""
                txtPassword2.Text = ""

            Else

                If MembershipProvider.Instance.PasswordRetrievalEnabled = True Then

                    pnlResetButton.Visible = False
                    pnlPassword_Step1.Visible = True
                    pnlPassword_Step2.Visible = False

                    txtPassword1.Text = ""
                    txtPassword2.Text = ""

                End If

            End If





        End Sub

        Private Sub btnNotifyPasswordSkip_Click(sender As Object, e As EventArgs) Handles btnNotifyPasswordSkip.Click

            Me.lblPasswordNote.Text = Localization.GetString("PasswordSet", LocalResourceFile)

            pnlPassword_Step1.Visible = True
            pnlPassword_Step2.Visible = False

            txtPassword1.Text = ""
            txtPassword2.Text = ""

        End Sub

        Private Sub BindPasswordNotification()

            Dim Locale As String = CurrentLocale

            If Not String.IsNullOrEmpty(User.Profile.PreferredLocale) Then
                Locale = User.Profile.PreferredLocale
            End If

            UserController.ResetPasswordToken(User, 1440)
            Dim reseturl As String = String.Format("http://{0}/default.aspx?ctl=PasswordReset&resetToken={1}", PortalSettings.PortalAlias.HTTPAlias, Server.UrlEncode(User.PasswordResetToken.ToString))


            Dim strTemplate As String = GetTemplate(Me.ModuleTheme, Constants.TemplateName_EmailPasswordReset, Locale, False)

            strTemplate = strTemplate.Replace("[FIRSTNAME]", User.FirstName)
            strTemplate = strTemplate.Replace("[LASTNAME]", User.LastName)
            strTemplate = strTemplate.Replace("[DISPLAYNAME]", User.DisplayName)
            strTemplate = strTemplate.Replace("[PORTALNAME]", PortalSettings.PortalName)
            strTemplate = strTemplate.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
            strTemplate = strTemplate.Replace("[USERNAME]", User.Username)
            strTemplate = strTemplate.Replace("[PASSWORD]", Localization.GetString("HiddenPassword", LocalResourceFile))
            strTemplate = strTemplate.Replace("[RESETLINK]", "<a href=""" & reseturl & """>" & Localization.GetString("ClickToReset", LocalResourceFile) & "</a>")
            strTemplate = strTemplate.Replace("[RESETLINKURL]", reseturl)
            strTemplate = strTemplate.Replace("[RECIPIENTUSERID]", User.UserID.ToString)
            strTemplate = strTemplate.Replace("[USERID]", User.UserID.ToString)

            txtNotifyPasswordBody.Content = strTemplate

        End Sub

#End Region

#Region "Private Methods"

        Private Sub ProcessQuerystring()

            If Request.IsAuthenticated = False Then
                Exit Sub
            End If

            Dim TargetUserId As Integer = Null.NullInteger
            If Request.QueryString("uid") Is Nothing Then
                Exit Sub
            Else
                If IsNumeric(Request.QueryString("uid")) Then
                    TargetUserId = Convert.ToInt32(Request.QueryString("uid"))
                End If
            End If

            If TargetUserId = Null.NullInteger Then
                Exit Sub
            End If

            Dim TargetRoleId As Integer = Null.NullInteger
            If Request.QueryString("RoleId") Is Nothing Then
                Exit Sub
            Else
                If IsNumeric(Request.QueryString("RoleId")) Then
                    TargetRoleId = Convert.ToInt32(Request.QueryString("RoleId"))
                End If
            End If

            If Not Request.QueryString("Action") Is Nothing Then
                Select Case Request.QueryString("Action").ToLower
                    Case "approve"

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)
                        If oUser Is Nothing Then
                            Exit Sub
                        End If

                        Dim roleController As New RoleController
                        Dim role As RoleInfo = roleController.GetRole(TargetRoleId, PortalId)

                        roleController.UpdateUserRole(PortalId, User.UserID, TargetRoleId, RoleStatus.Approved, False, False)

                        DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & TargetRoleId.ToString)

                        If Not Request.QueryString("Notify") Is Nothing Then
                            If Request.QueryString("Notify") = "1" Then
                                Dim strRole As String = roleController.GetRole(TargetRoleId, PortalId).RoleName
                                lblRolesNote.Text = Localization.GetString("lblNotificationNote_Roles", LocalResourceFile)
                                BindRoleMembershipChangedNotification(role.RoleName, Constants.TemplateName_EmailRoleStatusChanged, Date.Now, Null.NullDate)

                                pnlRoleChange_Step1.Visible = False
                                pnlRoleChange_Step2.Visible = True
                                btnNotifyRole.CommandArgument = "approve"
                                btnNotifyRoleSkip.CommandArgument = "approve"
                            Else
                                Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))
                            End If
                        Else
                            Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))
                        End If

                    Case "pending"

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)
                        If oUser Is Nothing Then
                            Exit Sub
                        End If

                        Dim roleController As New RoleController
                        Dim role As RoleInfo = roleController.GetRole(TargetRoleId, PortalId)

                        roleController.UpdateUserRole(PortalId, User.UserID, TargetRoleId, RoleStatus.Pending, False, False)

                        DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & TargetRoleId.ToString)

                        If Not Request.QueryString("Notify") Is Nothing Then
                            If Request.QueryString("Notify") = "1" Then
                                Dim strRole As String = roleController.GetRole(TargetRoleId, PortalId).RoleName
                                lblRolesNote.Text = Localization.GetString("lblNotificationNote_Roles", LocalResourceFile)
                                BindRoleMembershipChangedNotification(role.RoleName, Constants.TemplateName_EmailRoleStatusChanged, Null.NullDate, Null.NullDate)

                                pnlRoleChange_Step1.Visible = False
                                pnlRoleChange_Step2.Visible = True
                                btnNotifyRole.CommandArgument = "pending"
                                btnNotifyRoleSkip.CommandArgument = "pending"
                            Else
                                Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))
                            End If
                        Else
                            Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))
                        End If

                    Case "remove"

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

                        If oUser Is Nothing Then
                            Exit Sub
                        End If

                        If oUser.IsSuperUser Then
                            Exit Sub
                        End If

                        If oUser.UserID = PortalSettings.AdministratorId Then
                            Exit Sub
                        End If

                        Dim rc As New RoleController
                        Dim role As RoleInfo = rc.GetRole(TargetRoleId, PortalId)

                        RoleController.DeleteUserRole(oUser, role, PortalSettings, False)
                        ClearCache()

                        Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))

                    Case "delete"


                        If AllowDelete Then

                            Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

                            If oUser Is Nothing Then
                                Exit Sub
                            End If

                            If oUser.IsSuperUser Then
                                Exit Sub
                            End If

                            If oUser.UserID = PortalSettings.AdministratorId Then
                                Exit Sub
                            End If

                            UserController.DeleteUser(oUser, False, False)

                            ClearCache()
                            Response.Redirect(NavigateURL(TabId, "", "RoleId=" & TargetRoleId.ToString))

                        End If

                    Case "harddelete"

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

                        If oUser Is Nothing Then
                            Exit Sub
                        End If

                        UserController.RemoveUser(oUser)
                        ClearCache()

                        Response.Redirect(NavigateURL(TabId, "", "RoleId=-2"))

                    Case "restore"

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

                        If oUser Is Nothing Then
                            Exit Sub
                        End If

                        UserController.RestoreUser(oUser)
                        ClearCache()

                        Response.Redirect(NavigateURL(TabId, "", "uid=" & oUser.UserID.ToString, "RoleId=" & PortalSettings.RegisteredRoleId.ToString, "Action=Edit"))

                    Case "impersonate"

                        Dim oUser As UserInfo = UserController.GetUserById(PortalId, TargetUserId)

                        If oUser Is Nothing Then
                            Exit Sub
                        End If

                        ImpersonateAccount(oUser)

                End Select
            End If
        End Sub

        Private Sub ImpersonateAccount(ByVal objUser As UserInfo)

            If MembershipProvider.Instance().PasswordRetrievalEnabled = False Then
                Exit Sub
            End If

            If Not objUser Is Nothing Then

                If UserInfo IsNot Nothing Then
                    DataCache.ClearUserCache(Me.PortalSettings.PortalId, Context.User.Identity.Name)
                End If

                Dim objPortalSecurity As New PortalSecurity
                objPortalSecurity.SignOut()

                Dim password As String = UserController.GetPassword(objUser, "")

                Dim status As DotNetNuke.Security.Membership.UserLoginStatus
                UserController.UserLogin(PortalSettings.PortalId, objUser.Username, password, "", PortalSettings.PortalName, Request.UserHostAddress, status, False)


                Response.Redirect(NavigateURL(PortalSettings.HomeTabId))

            End If

        End Sub

        Private Sub BindUserCreateForm()

            pnlGrid.Visible = False
            pnlUser.Visible = False
            pnlCreate.Visible = True

            If ctlRoles.SelectedNode.Value <> PortalSettings.RegisteredRoleId.ToString Then
                If IsNumeric(ctlRoles.SelectedNode.Value) Then
                    Dim roleid As Integer = Convert.ToInt32(ctlRoles.SelectedNode.Value)
                    Try
                        Dim rc As New RoleController
                        Dim role As RoleInfo = rc.GetRole(roleid, PortalId)
                        If Not role Is Nothing Then
                            lblCreateAccountNote.Text = String.Format(Localization.GetString("CreateAccountInRole", LocalResourceFile), rc.GetRole(roleid, PortalId).RoleName)
                        End If
                    Catch
                    End Try
                End If
            End If

        End Sub

        Private Sub AddAccount()

            Dim strMessage As String = ""

            Dim blnUpdateUsername As Boolean = False
            Dim blnUpdateFirstname As Boolean = False
            Dim blnUpdateLastname As Boolean = False
            Dim blnUpdateDisplayname As Boolean = False
            Dim blnUpdatePassword As Boolean = False
            Dim blnUpdateEmail As Boolean = False

            Dim txtUsername As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Username), TextBox)
            blnUpdateUsername = (Not txtUsername Is Nothing)

            If blnUpdateUsername Then
                If Not IsValidUserAttribute(Constants.User_Username, plhCreate) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Username, plhCreate)
                End If
            End If

            Dim txtEmail As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Email), TextBox)
            blnUpdateEmail = (Not txtEmail Is Nothing)

            If blnUpdateEmail Then
                If Not IsValidUserAttribute(Constants.User_Email, plhCreate) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Email, plhCreate)
                End If
            End If

            Dim txtPassword As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Password1), TextBox)
            Dim txtPassword2 As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Password2), TextBox)
            blnUpdatePassword = (Not txtPassword Is Nothing AndAlso Not txtPassword2 Is Nothing)

            If blnUpdatePassword Then
                If Not IsValidUserAttribute(Constants.User_Password1, plhCreate) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Password1, plhCreate)
                End If
                If Not IsValidUserAttribute(Constants.User_Password2, plhCreate) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Password2, plhCreate)
                End If
            End If

            Dim txtFirstName As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Firstname), TextBox)
            blnUpdateFirstname = (Not txtFirstName Is Nothing)

            If blnUpdateFirstname Then
                If Not IsValidUserAttribute(Constants.User_Firstname, plhCreate) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Firstname, plhCreate)
                End If
            End If

            Dim txtLastName As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Lastname), TextBox)
            blnUpdateLastname = (Not txtLastName Is Nothing)

            If blnUpdateLastname Then
                If Not IsValidUserAttribute(Constants.User_Lastname, plhCreate) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Lastname, plhCreate)
                End If
            End If

            Dim txtDisplayName As TextBox = CType(FindControlRecursive(plhCreate, plhCreate.ID & "_" & Constants.ControlId_Displayname), TextBox)
            blnUpdateDisplayname = (Not txtDisplayName Is Nothing)


            If strMessage.Length > 0 Then
                Me.lblCreateAccountNote.Text = Localization.GetString("FieldsRequired", LocalResourceFile)
                Exit Sub
            End If


            Dim oUser As New UserInfo
            oUser.Membership.Approved = True


            oUser.AffiliateID = Null.NullInteger
            oUser.PortalID = PortalSettings.PortalId
            oUser.Username = ""
            oUser.DisplayName = ""
            oUser.Email = ""
            oUser.Membership.Password = ""

            If blnUpdateUsername Then
                oUser.Username = txtUsername.Text.Trim
            End If

            If blnUpdateDisplayname Then
                oUser.DisplayName = txtDisplayName.Text
            End If

            If blnUpdateFirstname Then
                oUser.FirstName = txtFirstName.Text
            End If

            If blnUpdateLastname Then
                oUser.LastName = txtLastName.Text
            End If

            If blnUpdateEmail Then
                oUser.Email = txtEmail.Text
            End If

            If oUser.Username = "" Then
                If blnUpdateEmail Then
                    oUser.Username = txtEmail.Text.Trim
                End If
            End If

            If oUser.DisplayName = "" Then
                If blnUpdateFirstname Then
                    oUser.DisplayName = txtFirstName.Text.Trim
                End If
                If blnUpdateLastname Then
                    If blnUpdateFirstname Then
                        oUser.DisplayName += " "
                    End If
                    oUser.DisplayName += txtLastName.Text.Trim
                End If
            End If


            'try updating password
            If blnUpdatePassword Then

                If txtPassword.Text = txtPassword2.Text Then
                    If Not UserController.ValidatePassword(txtPassword.Text) Then

                        Me.lblCreateAccountNote.Text = Localization.GetString("PasswordPoliciesError", LocalResourceFile)
                        Exit Sub

                    End If
                Else

                    Me.lblCreateAccountNote.Text = Localization.GetString("PasswordMatchError", LocalResourceFile)
                    Exit Sub

                End If

                oUser.Membership.Password = txtPassword.Text

            Else

                oUser.Membership.Password = UserController.GeneratePassword()

            End If

            'try updating displayname

            If oUser.Username = "" Or oUser.Email = "" Or oUser.DisplayName = "" Or oUser.Membership.Password = "" Then
                'template must be setup up wrong
                Me.lblCreateAccountNote.Text = Localization.GetString("TemplateError", LocalResourceFile)
                Exit Sub
            End If

            'set up profile
            oUser.Profile = New UserProfile
            oUser.Profile.InitialiseProfile(PortalSettings.PortalId, True)

            'see if we can create the account
            Dim createStatus As UserCreateStatus = UserController.CreateUser(oUser)
            If createStatus <> UserCreateStatus.Success Then

                Select Case createStatus
                    Case UserCreateStatus.UsernameAlreadyExists
                        strMessage = Localization.GetString("UsernameAlreadyExists", Me.LocalResourceFile)
                    Case Else
                        strMessage = String.Format(Localization.GetString("UserCreateError", Me.LocalResourceFile), createStatus.ToString)
                End Select

                If strMessage <> "" Then
                    strMessage = strMessage
                Else
                    strMessage = createStatus.ToString
                End If

                Me.lblCreateAccountNote.Text = strMessage
                Exit Sub

            End If

            'try updating firstname
            If blnUpdateFirstname Then
                oUser.FirstName = txtFirstName.Text
                oUser.Profile.FirstName = txtFirstName.Text
            End If

            'try updating lastname
            If blnUpdateLastname Then
                oUser.LastName = txtLastName.Text
                oUser.Profile.LastName = txtLastName.Text
            End If

            oUser.Profile.PreferredLocale = PortalSettings.DefaultLanguage
            oUser.Profile.PreferredTimeZone = PortalSettings.TimeZone

            UserController.UpdateUser(PortalId, oUser)

            'add to role
            If ctlRoles.SelectedNode.Value <> PortalSettings.RegisteredRoleId.ToString Then
                If IsNumeric(ctlRoles.SelectedNode.Value) Then
                    Dim roleid As Integer = Convert.ToInt32(ctlRoles.SelectedNode.Value)
                    Try
                        Dim rc As New RoleController
                        Dim role As RoleInfo = rc.GetRole(roleid, PortalId)
                        If Not role Is Nothing Then
                            rc.AddUserRole(PortalId, oUser.UserID, role.RoleID, Null.NullDate)
                        End If
                    Catch
                    End Try
                End If
            End If

            Dim url As String = NavigateURL(TabId, "", "uid=" & oUser.UserID.ToString)

            If ctlRoles.SelectedNode.Value <> PortalSettings.RegisteredRoleId.ToString Then
                If IsNumeric(ctlRoles.SelectedNode.Value) Then
                    Dim roleid As Integer = Convert.ToInt32(ctlRoles.SelectedNode.Value)
                    url = NavigateURL(TabId, "", "RoleId=" & roleid.ToString, "uid=" & oUser.UserID.ToString)
                End If
            End If

            ClearCache()
            UserController.ResetPasswordToken(User, 1440)

            Response.Redirect(url)

        End Sub

        Private Sub Export()

            Dim users As New List(Of UserInfo)
            Dim ds As DataSet = Nothing
            Dim strError As String = ""

            If ctlRoles.SelectedNode Is Nothing Then
                _IsReportResult = True
            End If

            If _IsReportResult Then
                ds = GetReportResult(strError)
            Else
                ds = GetUserList()
            End If

            If Not ds Is Nothing Then
                For Each row As DataRow In ds.Tables(0).Rows
                    Dim oUser As UserInfo = UserController.GetUserById(PortalId, row("UserId"))
                    If Not oUser Is Nothing Then
                        users.Add(oUser)
                    End If
                Next
            End If


            Dim sFilename As String = "Members"

            Try
                sFilename = "Members_" & ctlRoles.SelectedNode.Text
            Catch
            End Try

            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = "application/vnd.ms-excel"
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1")
            Response.AppendHeader("content-disposition", "attachment;filename=" & sFilename & ".xls")


            For Each strHeader As String In ExportFieldHeaders
                Response.Write(strHeader & Microsoft.VisualBasic.ControlChars.Tab)
            Next
            Response.Write(vbCrLf)

            For Each oUser As UserInfo In users
                For Each strField As String In ExportFields
                    Select Case strField.ToLower
                        Case "user_userid"
                            Response.Write(oUser.UserID.ToString & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_username"
                            Response.Write(oUser.Username & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_firstname"
                            Response.Write(oUser.FirstName & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_lastname"
                            Response.Write(oUser.LastName & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_email"
                            Response.Write(oUser.Email & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_createddate"
                            Response.Write(oUser.Membership.CreatedDate.ToShortDateString & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_lastlogindate"
                            Response.Write(oUser.Membership.LastLoginDate.ToShortDateString & Microsoft.VisualBasic.ControlChars.Tab)
                        Case "user_displayname"
                            Response.Write(oUser.DisplayName & Microsoft.VisualBasic.ControlChars.Tab)
                        Case Else
                            Dim strValue As String = GetPropertyValue(strField, oUser)
                            Response.Write(strValue & Microsoft.VisualBasic.ControlChars.Tab)

                    End Select
                Next
                Response.Write(vbCrLf)
            Next

            Response.End()

        End Sub

        Private Function GetPropertyValue(ByVal strProp As String, ByVal oUser As UserInfo) As String
            Dim prop As ProfilePropertyDefinition = ProfileController.GetPropertyDefinitionByName(PortalId, strProp)
            If Not prop Is Nothing Then
                Return GetPropertyValue(prop, oUser)
            Else
                Return ""
            End If
        End Function

        Private Function GetPropertyValue(ByVal objProp As ProfilePropertyDefinition, ByVal objUser As UserInfo) As String

            Dim strValue As String = ""
            Dim strPropertyValue As String = ""
            If Not String.IsNullOrEmpty(objUser.Profile.GetPropertyValue(objProp.PropertyName)) Then
                strPropertyValue = objUser.Profile.GetPropertyValue(objProp.PropertyName)
            End If

            Dim strType As String = ""
            Dim lc As New DotNetNuke.Common.Lists.ListController
            Dim entry As Lists.ListEntryInfo = lc.GetListEntryInfo(objProp.DataType)
            If Not entry Is Nothing Then
                strType = entry.Value
            End If

            Select Case strType.ToLower

                Case "truefalse"

                    If strValue.ToLower = "true" Then
                        Return Localization.GetString("yes", LocalResourceFile)
                    Else
                        Return Localization.GetString("no", LocalResourceFile)
                    End If

                Case "region"

                    Dim strCountry As String = objUser.Profile.GetPropertyValue("Country")

                    Dim country As Lists.ListEntryInfo = Nothing
                    Dim countries As New List(Of ListEntryInfo)
                    countries = GetList("Country")
                    If Not countries Is Nothing Then
                        For Each checkCountry As Lists.ListEntryInfo In countries
                            If checkCountry.Text = strCountry Then
                                country = checkCountry
                                Exit For
                            End If
                            If checkCountry.Value = strCountry Then
                                country = checkCountry
                                Exit For
                            End If
                        Next
                    End If

                    If Not country Is Nothing Then

                        Dim regions As New List(Of ListEntryInfo)
                        regions = GetList("Region", country.Value)
                        If Not regions Is Nothing Then
                            For Each item As Lists.ListEntryInfo In regions
                                If item.Value.ToLower = strPropertyValue.ToLower Then
                                    strValue = item.Text
                                    Exit For
                                End If
                                If item.Text.ToLower = strPropertyValue.ToLower Then
                                    strValue = item.Text
                                    Exit For
                                End If
                            Next
                        End If


                    End If


                Case "country"

                    Dim entries As New List(Of ListEntryInfo)
                    entries = GetList("Country")
                    If Not entries Is Nothing Then
                        For Each item As Lists.ListEntryInfo In entries
                            If item.Value.ToLower = strPropertyValue.ToLower Then
                                strValue = item.Text
                                Exit For
                            End If
                            If item.Text.ToLower = strPropertyValue.ToLower Then
                                strValue = item.Text
                                Exit For
                            End If
                        Next
                    End If


                Case "list"

                    Dim entries As New List(Of ListEntryInfo)
                    entries = GetList(objProp.PropertyName)
                    If Not entries Is Nothing Then
                        For Each item As Lists.ListEntryInfo In entries
                            If item.Value.ToLower = strPropertyValue.ToLower Then
                                strValue = item.Text
                                Exit For
                            End If
                            If item.Text.ToLower = strPropertyValue.ToLower Then
                                strValue = item.Text
                                Exit For
                            End If
                        Next
                    End If


                Case Else

                    strValue = strPropertyValue

            End Select

            Return strValue.Replace(vbCrLf, "").Replace(vbLf, "").Replace(vbNewLine, "").Replace(vbCr, "")

        End Function

        Private Function GetList(strListName As String) As List(Of ListEntryInfo)

            Dim entries As New List(Of ListEntryInfo)
            Dim lc As New DotNetNuke.Common.Lists.ListController

            If Not DataCache.GetCache("PROPLIST_" & strListName) Is Nothing Then
                entries = CType(DataCache.GetCache("PROPLIST_" & strListName), List(Of ListEntryInfo))
            Else
                entries = lc.GetListEntryInfoItems(strListName)
                DataCache.SetCache("PROPLIST_" & strListName, entries)
            End If

            Return entries

        End Function

        Private Function GetList(strListName As String, strValue As String) As List(Of ListEntryInfo)

            Dim entries As New List(Of ListEntryInfo)
            Dim lc As New DotNetNuke.Common.Lists.ListController

            If Not DataCache.GetCache("PROPLIST_" & strListName) Is Nothing Then
                entries = CType(DataCache.GetCache("PROPLIST_" & strListName), List(Of ListEntryInfo))
            Else
                entries = lc.GetListEntryInfoItems(strListName, strValue)
                DataCache.SetCache("PROPLIST_" & strListName, entries)
            End If

            Return entries

        End Function

        Private Sub UpdateAccount()

            Dim strMessage As String = ""

            Dim blnUpdateUsername As Boolean = False
            Dim blnUpdateDisplayname As Boolean = False
            Dim blnUpdateEmail As Boolean = False

            Dim txtUsername As RadTextBox = CType(FindControlRecursive(plhUser, plhUser.ID & "_" & Constants.ControlId_Username), RadTextBox)
            blnUpdateUsername = (Not txtUsername Is Nothing)

            If blnUpdateUsername Then
                If Not IsValidUserAttribute(Constants.User_Username, plhUser) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Username, plhUser)
                Else
                    RemoveErrorIndicator(Constants.User_Username, plhUser, True)
                End If
            End If

            Dim txtEmail As RadTextBox = CType(FindControlRecursive(plhUser, plhUser.ID & "_" & Constants.ControlId_Email), RadTextBox)
            blnUpdateEmail = (Not txtEmail Is Nothing)

            If blnUpdateEmail Then
                If Not IsValidUserAttribute(Constants.User_Email, plhUser) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Email, plhUser)
                Else
                    RemoveErrorIndicator(Constants.User_Email, plhUser, True)
                End If
            End If

            Dim txtDisplayName As RadTextBox = CType(FindControlRecursive(plhUser, plhUser.ID & "_" & Constants.ControlId_Displayname), RadTextBox)
            blnUpdateDisplayname = (Not txtDisplayName Is Nothing)

            If blnUpdateDisplayname Then
                If Not IsValidUserAttribute(Constants.User_Displayname, plhUser) Then
                    strMessage += "Error"
                    AddErrorIndicator(Constants.User_Displayname, plhUser)
                Else
                    RemoveErrorIndicator(Constants.User_Displayname, plhUser, True)
                End If
            End If

            If strMessage.Length > 0 Then
                Me.lblAccountNote.Text = Localization.GetString("FieldsRequired", LocalResourceFile)
                Exit Sub
            End If

            Dim oUser As UserInfo = User
            If blnUpdateDisplayname Then
                oUser.DisplayName = txtDisplayName.Text
            End If

            If blnUpdateEmail Then
                oUser.Email = txtEmail.Text
            End If

            If blnUpdateUsername Then
                oUser.Username = txtUsername.Text
            End If

            UserController.UpdateUser(PortalId, oUser)

            lblAccountNote.Text = Localization.GetString("AccountDataUpdated", LocalResourceFile)

        End Sub

        Private Sub UpdateProfile()

            Dim oUser As UserInfo = User
            Dim strMessage As String = ""

            For Each itemProp As String In GetPropertiesFromTempate(GetTemplate(ModuleTheme, Constants.TemplateName_ProfileForm, CurrentLocale, False))
                Try
                    Dim prop As ProfilePropertyDefinition = ProfileController.GetPropertyDefinitionByName(PortalId, itemProp.Substring(2)) 'itemprop comes in the form U:Propertyname or P:Propertyname
                    If Not prop Is Nothing Then
                        If Not IsValidProperty(oUser, prop, plhProfile) Then
                            strMessage += "Error"
                            AddErrorIndicator(prop.PropertyDefinitionId.ToString, plhProfile)
                        Else
                            RemoveErrorIndicator(prop.PropertyDefinitionId.ToString, plhProfile, prop.Required)
                        End If
                    End If
                Catch
                End Try
            Next

            If strMessage.Length > 0 Then
                Me.lblProfileNote.Text = Localization.GetString("FieldsRequired", LocalResourceFile)
                Exit Sub
            End If

            UserController.UpdateUser(PortalId, oUser)

            Dim propertiesCollection As New ProfilePropertyDefinitionCollection
            UpdateProfileProperties(plhProfile, oUser, propertiesCollection, GetPropertiesFromTempate(GetTemplate(ModuleTheme, Constants.TemplateName_AccountForm, CurrentLocale, False)))
            oUser = ProfileController.UpdateUserProfile(oUser, propertiesCollection)

            lblProfileNote.Text = Localization.GetString("ProfileUpdated", LocalResourceFile)


        End Sub

        Private Sub BindUser(UserId As Integer)

            pnlBackToList.Visible = True
            pnlCreateAccount.Visible = False

            pnlGrid.Visible = False
            pnlUser.Visible = True

            Dim objController As New UserController
            Dim objUser As UserInfo = objController.GetUser(PortalId, UserId)

            If Not objUser Is Nothing Then

                If objUser.Membership.IsOnLine Then
                    lblUserOnlineStatus.Text = Localization.GetString("UserIsOnline", LocalResourceFile)
                Else
                    lblUserOnlineStatus.Text = Localization.GetString("UserIsOffline", LocalResourceFile)
                End If

                If objUser.Membership.Approved Then
                    lblAuthorizedStatus.Text = Localization.GetString("UserIsApproved", LocalResourceFile)
                Else
                    lblAuthorizedStatus.Text = Localization.GetString("UserIsUnApproved", LocalResourceFile)
                End If

                If objUser.Membership.UpdatePassword Then
                    lblForcePasswordChange.Text = Localization.GetString("UserMustUpdatePassword", LocalResourceFile)
                    cmdForcePasswordChange.Visible = False
                Else
                    lblForcePasswordChange.Text = Localization.GetString("UserMustNotUpdatePassword", LocalResourceFile)
                    cmdForcePasswordChange.Visible = True
                End If

                If objUser.Membership.LockedOut Then
                    lblLockoutStatus.Text = Localization.GetString("UserIsLockedOut", LocalResourceFile)
                    cmdUnlockAccount.Visible = True
                Else
                    lblLockoutStatus.Text = Localization.GetString("UserIsNotLockedOut", LocalResourceFile)
                    cmdUnlockAccount.Visible = False
                End If

                lblMemberSince.Text = objUser.Membership.CreatedDate.ToShortDateString & ", " & objUser.Membership.CreatedDate.ToShortTimeString
                lblLastActivity.Text = objUser.Membership.LastActivityDate.ToShortDateString & ", " & objUser.Membership.LastActivityDate.ToShortTimeString
                lblLastLockout.Text = objUser.Membership.LastLockoutDate.ToShortDateString & ", " & objUser.Membership.LastLockoutDate.ToShortTimeString
                lblLastLogin.Text = objUser.Membership.LastLoginDate.ToShortDateString & ", " & objUser.Membership.LastLoginDate.ToShortTimeString
                lblLastPasswordChange.Text = objUser.Membership.LastPasswordChangeDate.ToShortDateString & ", " & objUser.Membership.LastPasswordChangeDate.ToShortTimeString

                If lblLastActivity.Text = "" Then lblLastActivity.Text = Localization.GetString("Never", LocalResourceFile)
                If lblLastLockout.Text = "" Then lblLastLockout.Text = Localization.GetString("Never", LocalResourceFile)
                If lblLastLogin.Text = "" Then lblLastLogin.Text = Localization.GetString("Never", LocalResourceFile)
                If lblLastPasswordChange.Text = "" Then lblLastPasswordChange.Text = Localization.GetString("Never", LocalResourceFile)

                cmdAuthorizeAccount.Visible = (objUser.Membership.Approved = False)
                cmdDeleteAccount.Visible = (objUser.UserID <> PortalSettings.AdministratorId AndAlso AllowDelete AndAlso (objUser.IsSuperUser = False))
                cmdRestoreAccount.Visible = (objUser.IsDeleted = True)

                If objUser.IsDeleted Then
                    cmdDeleteAccount.Text = Localization.GetString("HardDeleteAccount", LocalResourceFile)
                Else
                    cmdDeleteAccount.Text = Localization.GetString("DeleteAccount", LocalResourceFile)
                End If

                If objUser.IsDeleted Then cmdUpdateAccount.Visible = False
                If objUser.IsDeleted Then cmdForcePasswordChange.Visible = False

                If UserInfo.IsSuperUser Then
                    BindUserSites(UserId)
                End If

            End If

        End Sub

        Private Sub BindRoleMembershipChangedNotification(ByVal strRole As String, TemplateName As String, ByVal EffectiveDate As Date, ByVal ExpiryDate As Date)

            Dim Locale As String = CurrentLocale

            If Not String.IsNullOrEmpty(User.Profile.PreferredLocale) Then
                Locale = User.Profile.PreferredLocale
            End If

            Dim strTemplate As String = GetTemplate(Me.ModuleTheme, TemplateName, Locale, False)
            strTemplate = strTemplate.Replace("[FIRSTNAME]", User.FirstName)
            strTemplate = strTemplate.Replace("[LASTNAME]", User.LastName)
            strTemplate = strTemplate.Replace("[DISPLAYNAME]", User.DisplayName)
            strTemplate = strTemplate.Replace("[PORTALNAME]", PortalSettings.PortalName)
            strTemplate = strTemplate.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
            strTemplate = strTemplate.Replace("[USERNAME]", User.Username)
            strTemplate = strTemplate.Replace("[PASSWORD]", Localization.GetString("HiddenPassword", LocalResourceFile))
            strTemplate = strTemplate.Replace("[ROLE]", strRole)
            strTemplate = strTemplate.Replace("[RECIPIENTUSERID]", User.UserID.ToString)
            strTemplate = strTemplate.Replace("[USERID]", User.UserID.ToString)

            If EffectiveDate <> Null.NullDate Then
                strTemplate = strTemplate.Replace("[EFFECTIVEDATE]", EffectiveDate.ToShortDateString)
            Else
                strTemplate = strTemplate.Replace("[EFFECTIVEDATE]", Date.Now.ToShortDateString)
            End If

            If ExpiryDate <> Null.NullDate Then
                strTemplate = strTemplate.Replace("[EXPIRYDATE]", ExpiryDate.ToShortDateString)
            Else
                strTemplate = strTemplate.Replace("[EXPIRYDATE]", "-")
            End If

            txtNotifyRoleBody.Content = strTemplate

        End Sub

        Private Sub BindUserSites(UserId As Integer)

            Dim pCtrl As New PortalController

            chkUserSites.Items.Clear()

            For Each objPortal As PortalInfo In pCtrl.GetPortals()

                Dim cItem As New ListItem(objPortal.PortalName, objPortal.PortalID.ToString)
                cItem.Selected = False
                If Not UserController.GetUserById(objPortal.PortalID, UserId) Is Nothing Then
                    cItem.Selected = True
                End If
                If objPortal.AdministratorId = UserId Then
                    cItem.Enabled = False
                End If
                chkUserSites.Items.Add(cItem)

            Next

            chkUserSites.Items(0).Enabled = False

        End Sub

        Private Sub BindUserNotification()

            Dim Locale As String = CurrentLocale

            If Not String.IsNullOrEmpty(User.Profile.PreferredLocale) Then
                Locale = User.Profile.PreferredLocale
            End If

            Dim reseturl As String = String.Format("http://{0}/default.aspx?ctl=PasswordReset&resetToken={1}", PortalSettings.PortalAlias.HTTPAlias, User.PasswordResetToken)

            Dim strTemplate As String = GetTemplate(Me.ModuleTheme, Constants.TemplateName_EmailAccountData, Locale, False)

            strTemplate = strTemplate.Replace("[FIRSTNAME]", User.FirstName)
            strTemplate = strTemplate.Replace("[LASTNAME]", User.LastName)
            strTemplate = strTemplate.Replace("[DISPLAYNAME]", User.DisplayName)
            strTemplate = strTemplate.Replace("[PORTALNAME]", PortalSettings.PortalName)
            strTemplate = strTemplate.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
            strTemplate = strTemplate.Replace("[USERNAME]", User.Username)
            strTemplate = strTemplate.Replace("[PASSWORD]", Localization.GetString("HiddenPassword", LocalResourceFile))
            strTemplate = strTemplate.Replace("[RESETLINK]", "<a href=""" & reseturl & """>" & Localization.GetString("ClickToSet", LocalResourceFile) & "</a>")
            strTemplate = strTemplate.Replace("[RESETLINKURL]", reseturl)
            strTemplate = strTemplate.Replace("[RECIPIENTUSERID]", User.UserID.ToString)
            strTemplate = strTemplate.Replace("[USERID]", User.UserID.ToString)

            txtNotifyUserBody.Content = strTemplate

        End Sub

        Private Sub InitializeForm()

            cmdBulkDelete.Text = Localization.GetString("cmdBulkDelete", LocalResourceFile)
            cmdBulkRemove.Text = Localization.GetString("cmdBulkRemove", LocalResourceFile)

            Dim reports As New List(Of UserReportInfo)
            Dim ctrlReports As New UserReportsController
            reports = UserReportsController.GetReports(PortalId)
            If reports.Count = 0 Then
                pnlReport.Visible = False
            End If

            Me.txtNotifyRoleBody.ToolsFile = Me.TemplateSourceDirectory & "/Config/Toolsfile.xml"
            Me.txtNotifyUserBody.ToolsFile = Me.TemplateSourceDirectory & "/Config/Toolsfile.xml"
            Me.txtNotifyPasswordBody.ToolsFile = Me.TemplateSourceDirectory & "/Config/Toolsfile.xml"

            txtNotifyRoleSubject.Text = String.Format(Localization.GetString("txtNotifyRoleSubject", LocalResourceFile), PortalSettings.PortalName)
            txtNotifyUserSubject.Text = String.Format(Localization.GetString("txtNotifyUserSubject", LocalResourceFile), PortalSettings.PortalName)
            txtNotifyPasswordSubject.Text = String.Format(Localization.GetString("txtNotifyPasswordSubject", LocalResourceFile), PortalSettings.PortalName)

            BindUserNotification()

            lblPasswordNote.Text = Localization.GetString("lblPasswordNote", LocalResourceFile)
            lblAccountNote.Text = Localization.GetString("lblAccountNote", LocalResourceFile)
            lblProfileNote.Text = Localization.GetString("lblProfileNote", LocalResourceFile)
            lblRolesNote.Text = Localization.GetString("lblRolesNote", LocalResourceFile)
            lblEmailNote.Text = Localization.GetString("lblEmailNote", LocalResourceFile)
            lblSitesNote.Text = Localization.GetString("lblSitesNote", LocalResourceFile)

            tabAccount.Visible = False
            pnlAccountTab.Visible = False
            tabPassword.Visible = False
            pnlPasswordTab.Visible = False
            tabProfile.Visible = False
            pnlProfileTab.Visible = False
            tabRoles.Visible = False
            pnlRolesTab.Visible = False
            tabEmail.Visible = False
            pnlEmailTab.Visible = False
            tabSites.Visible = False
            pnlSitesTab.Visible = False

            For Each strTab As String In ShowUserDetailTabs
                If strTab.ToLower = "account" Then
                    tabAccount.Visible = True
                    pnlAccountTab.Visible = True
                End If
                If strTab.ToLower = "password" Then
                    tabPassword.Visible = True
                    pnlPasswordTab.Visible = True
                End If
                If strTab.ToLower = "profile" Then
                    tabProfile.Visible = True
                    pnlProfileTab.Visible = True
                End If
                If strTab.ToLower = "roles" Then
                    tabRoles.Visible = True
                    pnlRolesTab.Visible = True
                End If
                If strTab.ToLower = "email" Then
                    tabEmail.Visible = True
                    pnlEmailTab.Visible = True
                End If
                If strTab.ToLower = "sites" Then
                    tabSites.Visible = True
                    pnlSitesTab.Visible = True
                End If
            Next

            If AdditionalControls.Length > 0 Then

                Dim strTabname As String = ""
                Dim strControl As String = ""

                For Each objControl As String In AdditionalControls

                    Try
                        strTabname = objControl.Split(Char.Parse(","))(0)
                        strControl = objControl.Split(Char.Parse(","))(1)
                    Catch
                    End Try


                    If strTabname.Length > 0 AndAlso strControl.Length > 0 Then

                        Dim relUrl As String = ResolveUrl("~" & strControl)
                        Dim path As String = Server.MapPath(relUrl)
                        If System.IO.File.Exists(path) AndAlso path.EndsWith(".ascx") Then

                            'ok, we've got a tabname and a valid control to load
                            Dim objModule As PortalModuleBase = CType(Me.LoadControl(relUrl), PortalModuleBase)
                            objModule.ModuleConfiguration = Me.ModuleConfiguration
                            objModule.ID = System.IO.Path.GetFileNameWithoutExtension(relUrl)


                            Dim strTabLiteral As String = "<li id=""" & strTabname.Replace(" ", "") & """>"

                            Dim objPanel As New Panel
                            objPanel.ID = "pnl_" & objModule.ModuleId.ToString
                            objPanel.Controls.Add(objModule)
                            plhAdditonalControls.Controls.Add(objPanel)

                            strTabLiteral += "<a href=""#" & objPanel.ClientID & """>"
                            strTabLiteral += strTabname
                            strTabLiteral += "</a></li>"

                            plhAdditionalTabs.Controls.Add(New LiteralControl(strTabLiteral))

                        End If

                    End If


                Next
            End If


            If MembershipProvider.Instance().RequiresQuestionAndAnswer Then
                tabPassword.Visible = False
                pnlPasswordTab.Visible = False
            End If

            If MembershipProvider.Instance().PasswordRetrievalEnabled = False Then
                pnlPassword_Step1.Visible = False
                pnlResetButton.Visible = True
                lblPasswordNote.Text = Localization.GetString("ResetPasswordNote", LocalResourceFile)
            End If

            pnlCreateAccount.Visible = AllowCreate
            btnExport.Visible = AllowExport

            btnNotifyPassword.Text = Localization.GetString("btnNotifyPassword", LocalResourceFile)
            btnNotifyPasswordSkip.Text = Localization.GetString("btnNotifyPasswordSkip", LocalResourceFile)

            btnNotifyRoleSkip.Text = Localization.GetString("btnNotifyRoleSkip", LocalResourceFile)
            btnNotifyRole.Text = Localization.GetString("btnNotifyRole", LocalResourceFile)

            btnNotifyUser.Text = Localization.GetString("btnNotifyUser", LocalResourceFile)

            btnAddToRole.Text = Localization.GetString("btnAddToRole", LocalResourceFile)
            btnExport.Text = Localization.GetString("btnExport", LocalResourceFile)
            btnHardDelete.Text = Localization.GetString("btnHardDelete", LocalResourceFile)
            btnReport.Text = Localization.GetString("btnReport", LocalResourceFile)
            btnSearch.Text = Localization.GetString("btnSearch", LocalResourceFile)
            btnApplyOptions.Text = Localization.GetString("btnApplyOptions", LocalResourceFile)


            lblGridTab.Text = Localization.GetString("lblGridTab", LocalResourceFile)
            lblPreferencesTab.Text = Localization.GetString("lblPreferencesTab", LocalResourceFile)

            lblGridSetup.Text = Localization.GetString("lblGridSetup", LocalResourceFile)
            lblPageSize.Text = Localization.GetString("lblPageSize", LocalResourceFile)
            lblSearchOptions.Text = Localization.GetString("lblSearchOptions", LocalResourceFile)


            For Each item As ListItem In chkGridOptions.Items

                Dim strText As String = ""
                strText = Localization.GetString("ProfileProperties_" & item.Value & ".Text", ProfileResourcefile)

                If strText = "" OrElse strText.StartsWith("RESX:") Then
                    item.Text = Localization.GetString(item.Value, LocalResourceFile)
                Else
                    item.Text = strText.Replace(":", "")
                End If

            Next

        End Sub

        Private Sub BindSearchColOptions()

            chkSearchCols.Items.Clear()

            chkSearchCols.Items.Add(New ListItem("Username", "Username"))
            chkSearchCols.Items.Add(New ListItem("DisplayName", "DisplayName"))
            chkSearchCols.Items.Add(New ListItem("Email", "Email"))


            Dim props As ProfilePropertyDefinitionCollection = ProfileController.GetPropertyDefinitionsByPortal(PortalSettings.PortalId)
            For Each prop As ProfilePropertyDefinition In props
                Try
                    chkSearchCols.Items.Add(New ListItem(LocalizeProperty(prop).Replace(":", ""), prop.PropertyName))
                Catch ex As Exception
                    chkSearchCols.Items.Add(New ListItem(prop.PropertyName, prop.PropertyName))
                End Try

            Next

        End Sub

        Private Sub PersonalizeOptions()

            For Each item As ListItem In chkSearchCols.Items
                item.Selected = False
            Next

            For Each item As ListItem In chkGridOptions.Items
                item.Selected = False
            Next

            Dim strSearchCols As String = "FirstName,LastName,City,Email,"
            Dim searchcols As Object = DotNetNuke.Services.Personalization.Personalization.GetProfile("dnnWerk_Users_ColOptions", "SearchCols_" & UserId.ToString)
            If Not searchcols Is Nothing Then
                If searchcols.ToString.Length > 0 Then
                    strSearchCols = searchcols.ToString
                End If
            End If

            For Each strSearchCol As String In strSearchCols.Split(Char.Parse(","))
                If strSearchCol.Length > 0 Then
                    For Each item As ListItem In chkSearchCols.Items
                        If item.Value.ToLower = strSearchCol.ToLower Then
                            item.Selected = True
                        End If
                    Next
                End If
            Next

            Dim strGridCols As String = "UserId,DisplayName,Username,Email,Country,CreatedDate"
            Dim gridcols As Object = DotNetNuke.Services.Personalization.Personalization.GetProfile("dnnWerk_Users_ColOptions", "GridCols_" & UserId.ToString)
            If Not gridcols Is Nothing Then
                If gridcols.ToString.Length > 0 Then
                    strGridCols = gridcols.ToString
                End If
            End If

            For Each strGridCol As String In strGridCols.Split(Char.Parse(","))
                If strGridCol.Length > 0 Then
                    For Each item As ListItem In chkGridOptions.Items
                        If item.Value.ToLower = strGridCol.ToLower Then
                            item.Selected = True
                        End If
                    Next
                End If
            Next

            Dim pagesize As String = "25"
            Try
                pagesize = DotNetNuke.Services.Personalization.Personalization.GetProfile("dnnWerk_Users_ColOptions", "GridPageSize_" & UserId.ToString)
            Catch
            End Try
            Try
                drpPageSize.Items.FindByText(pagesize.ToString).Selected = True
            Catch
            End Try

            'set up grid
            For Each strGridCol As String In strGridCols.Split(Char.Parse(","))
                If strGridCol.ToString.Length > 0 Then

                    Dim strCol As String = strGridCol
                    Try
                        grdUsers.Columns.FindByDataField(strCol).Visible = True

                        If strCol.ToLower = "dreateddate" Then
                            grdUsers.Columns.FindByDataField(strCol).Visible = (ctlRoles.SelectedNode.Value = PortalSettings.RegisteredRoleId.ToString)
                        End If

                    Catch
                    End Try

                End If
            Next

            If drpPageSize.SelectedItem.Value = "All" Then
                grdUsers.AllowPaging = False
            Else
                grdUsers.AllowPaging = True
                grdUsers.PageSize = Convert.ToInt32(drpPageSize.SelectedItem.Value)
            End If

            grdUsers.Columns.FindByDataField("CreatedDate").Visible = (ctlRoles.SelectedNode.Value = PortalSettings.RegisteredRoleId.ToString)
            grdUsers.Columns.FindByDataField("CreatedOnDate").Visible = (ctlRoles.SelectedNode.Value <> PortalSettings.RegisteredRoleId.ToString)
            grdUsers.Columns.FindByDataField("Status").Visible = (ctlRoles.SelectedNode.Value <> PortalSettings.RegisteredRoleId.ToString)

        End Sub

        Private Sub SaveSearchOptions()

            Dim strCols As String = ""
            For Each item As ListItem In chkSearchCols.Items
                If item.Selected = True Then
                    strCols += item.Value & ","
                End If
            Next

            DotNetNuke.Services.Personalization.Personalization.SetProfile("dnnWerk_Users_ColOptions", "SearchCols_" & UserId.ToString, strCols)

        End Sub

        Private Sub SaveGridOptions()

            Dim strCols As String = ""
            For Each item As ListItem In chkGridOptions.Items
                If item.Selected = True Then
                    strCols += item.Value & ","
                End If
            Next

            DotNetNuke.Services.Personalization.Personalization.SetProfile("dnnWerk_Users_ColOptions", "GridCols_" & UserId.ToString, strCols)

            DotNetNuke.Services.Personalization.Personalization.SetProfile("dnnWerk_Users_ColOptions", "GridPageSize_" & UserId.ToString, drpPageSize.SelectedItem.Value)

        End Sub

        Private Sub BindRoles()

            Dim objRoleController As New RoleController

            Dim roles As ArrayList = objRoleController.GetPortalRoles(PortalId)
            Dim groups As ArrayList = RoleController.GetRoleGroups(PortalId)

            If groups.Count > 0 Then
                'we have some role groups, add those first
                For Each objGroup As RoleGroupInfo In groups

                    Dim groupnode As New DnnTreeNode()
                    groupnode.Text = objGroup.RoleGroupName
                    groupnode.Value = "-1"
                    groupnode.Attributes.Add("IsGroup", True)
                    groupnode.ImageUrl = Me.TemplateSourceDirectory & "/images/folder.png"
                    groupnode.PostBack = False

                    Dim groupItem As New DnnComboBoxItem
                    groupItem.Text = objGroup.RoleGroupName
                    groupItem.Value = "-1"
                    groupItem.ImageUrl = Me.TemplateSourceDirectory & "/images/folder.png"
                    groupItem.Attributes.Add("IsGroup", True)
                    groupItem.IsSeparator = True

                    drpRoles.Items.Add(groupItem)

                    For Each objRole As RoleInfo In roles
                        If objRole.RoleGroupID = objGroup.RoleGroupID Then

                            If (Not AllowedRoles Is Nothing AndAlso (Array.IndexOf(AllowedRoles, objRole.RoleID.ToString) > -1 Or Array.IndexOf(AllowedRoles, "all") > -1)) OrElse (AllowedRoles Is Nothing) Then

                                Dim rolenode As New DnnTreeNode()
                                rolenode.Text = objRole.RoleName
                                rolenode.Value = objRole.RoleID.ToString
                                rolenode.Text = objRole.RoleName
                                rolenode.Value = objRole.RoleID.ToString
                                rolenode.ImageUrl = Me.TemplateSourceDirectory & "/images/users.png"
                                rolenode.Attributes.Add("IsGroup", False)
                                rolenode.NavigateUrl = NavigateURL(TabId, "", "RoleId=" & objRole.RoleID.ToString)

                                groupnode.Nodes.Add(rolenode)

                                Dim roleItem As New DnnComboBoxItem
                                roleItem.Text = objRole.RoleName
                                roleItem.Value = objRole.RoleID
                                roleItem.ImageUrl = Me.TemplateSourceDirectory & "/images/users.png"
                                roleItem.Attributes.Add("style", "margin-left: 20px;")
                                drpRoles.Items.Add(roleItem)

                            End If

                        End If

                    Next

                    If groupnode.Nodes.Count > 0 Then
                        ctlRoles.Nodes.Add(groupnode)
                    End If


                Next

                For Each objRole As RoleInfo In roles
                    If objRole.RoleGroupID = Null.NullInteger Then

                        If (Not AllowedRoles Is Nothing AndAlso (Array.IndexOf(AllowedRoles, objRole.RoleID.ToString) > -1 Or Array.IndexOf(AllowedRoles, "all") > -1)) OrElse (AllowedRoles Is Nothing) Then

                            Dim rolenode As New DnnTreeNode()
                            rolenode.Text = objRole.RoleName
                            rolenode.Value = objRole.RoleID.ToString
                            rolenode.Attributes.Add("IsGroup", False)
                            rolenode.ImageUrl = Me.TemplateSourceDirectory & "/images/users.png"
                            rolenode.NavigateUrl = NavigateURL(TabId, "", "RoleId=" & objRole.RoleID.ToString)
                            ctlRoles.Nodes.Add(rolenode)

                            If objRole.RoleID <> PortalSettings.RegisteredRoleId Then
                                Dim roleItem As New DnnComboBoxItem
                                roleItem.Text = objRole.RoleName
                                roleItem.Value = objRole.RoleID
                                roleItem.ImageUrl = Me.TemplateSourceDirectory & "/images/users.png"
                                drpRoles.Items.Add(roleItem)
                            End If

                        End If


                    End If

                Next
            Else
                For Each objRole As RoleInfo In roles

                    If (Not AllowedRoles Is Nothing AndAlso (Array.IndexOf(AllowedRoles, objRole.RoleID.ToString) > -1 Or Array.IndexOf(AllowedRoles, "all") > -1)) OrElse (AllowedRoles Is Nothing) Then

                        Dim rolenode As New DnnTreeNode()
                        rolenode.Text = objRole.RoleName
                        rolenode.Value = objRole.RoleID.ToString
                        rolenode.ImageUrl = Me.TemplateSourceDirectory & "/images/users.png"
                        rolenode.Attributes.Add("IsGroup", False)
                        rolenode.NavigateUrl = NavigateURL(TabId, "", "RoleId=" & objRole.RoleID.ToString)
                        ctlRoles.Nodes.Add(rolenode)

                        If objRole.RoleID <> PortalSettings.RegisteredRoleId Then
                            Dim roleItem As New RadComboBoxItem
                            roleItem.Text = objRole.RoleName
                            roleItem.Value = objRole.RoleID
                            roleItem.ImageUrl = Me.TemplateSourceDirectory & "/images/users.png"
                            drpRoles.Items.Add(roleItem)
                        End If

                    End If

                Next
            End If

            If (Not AllowedRoles Is Nothing AndAlso (Array.IndexOf(AllowedRoles, "-2") > -1 Or Array.IndexOf(AllowedRoles, "all") > -1)) OrElse (AllowedRoles Is Nothing) Then
                Dim binnode As New DnnTreeNode()
                binnode.Text = "Deleted Users"
                binnode.Value = "-2"
                binnode.ImageUrl = "~/images/action_delete.gif"
                binnode.Attributes.Add("IsGroup", False)
                binnode.NavigateUrl = NavigateURL(TabId, "", "RoleId=-2")
                ctlRoles.Nodes.Add(binnode)
            End If


            Dim SelectedRole As String = ""
            If Not Request.QueryString("RoleId") Is Nothing Then
                SelectedRole = Request.QueryString("RoleId")
            Else
                If Request.QueryString("RoleId") Is Nothing AndAlso Request.QueryString("ReportsResult") = "true" Then

                    'Me.ctlFilterTabs.SelectedIndex = 1
                    'Me.pvFilters.SelectedIndex = 1

                Else

                    If Me.PreSelectRole <> Null.NullInteger Then
                        SelectedRole = Me.PreSelectRole.ToString
                    Else
                        SelectedRole = PortalSettings.RegisteredRoleId.ToString
                    End If

                End If


            End If

            Try
                ctlRoles.UnselectAllNodes()
                ctlRoles.FindNodeByValue(SelectedRole).Selected = True
                ctlRoles.FindNodeByValue(SelectedRole).ExpandParentNodes()
            Catch
            End Try

            If Convert.ToInt32(SelectedRole) = PortalSettings.RegisteredRoleId Then
                cmdBulkRemove.Visible = False
            End If
            If Convert.ToInt32(SelectedRole) = -2 Then
                cmdBulkRemove.Visible = False
                cmdBulkDelete.Visible = False
            End If

        End Sub

        Private Sub BindUsers()

            If ctlRoles.SelectedNode Is Nothing Then
                _IsReportResult = (Page.IsPostBack Or (Request.QueryString("ReportsResult") = "true"))
            Else
                _IsReportResult = False
            End If

            If _IsReportResult Then

                Dim strError As String = ""
                grdUsers.DataSource = GetReportResult(strError)

                If strError.Length > 0 Then
                    grdUsers.MasterTableView.NoMasterRecordsText = "<p style='padding:10px;'>" & strError & "</p>"
                Else
                    grdUsers.MasterTableView.NoMasterRecordsText = "<p style='padding:10px;'>The report does not return any data.</p>"
                End If

            Else

                'If Not Session("Connect_UserSearchTerm") Is Nothing Then
                '    txtSearch.Text = CType(Session("Connect_UserSearchTerm"), String)
                'End If

                If txtSearch.Text.Length = 0 Then
                    grdUsers.MasterTableView.NoMasterRecordsText = "<p style='padding:10px;'>" & Localization.GetString("NoUsersFoundInRole", LocalResourceFile) & "</p>"
                Else
                    grdUsers.MasterTableView.NoMasterRecordsText = "<p style='padding:10px;'>" & Localization.GetString("NoResultsFoundInRole", LocalResourceFile) & "</p>"
                End If

                grdUsers.DataSource = GetUserList()

            End If


        End Sub

        Private Function GetUserList() As DataSet

            Dim intRole As Integer = Null.NullInteger
            Dim strSearch As String = Null.NullString
            Dim blnShowDeleted As Boolean = False
            Dim blnUseCache As Boolean = True

            If txtSearch.Text.Length > 0 Then

                strSearch = txtSearch.Text.Trim
                intRole = PortalSettings.RegisteredRoleId
                blnUseCache = False

                If ctlRoles.SelectedNode.Value <> PortalSettings.RegisteredRoleId.ToString And Not ctlRoles.SelectedNode.Value = "-2" Then
                    ctlRoles.UnselectAllNodes()
                    Try
                        ctlRoles.FindNodeByValue(PortalSettings.RegisteredRoleId.ToString).Selected = True
                    Catch
                    End Try
                End If

            Else

                If Not ctlRoles.SelectedNode Is Nothing Then
                    If CType(ctlRoles.SelectedNode.Attributes("IsGroup"), Boolean) = False Then
                        Try
                            intRole = Convert.ToInt32(ctlRoles.SelectedNode.Value)
                        Catch
                        End Try
                    End If
                End If

            End If

            If Not ctlRoles.SelectedNode Is Nothing Then
                If ctlRoles.SelectedNode.Value = "-2" Then
                    blnShowDeleted = True
                    blnUseCache = False
                End If
            End If

            Dim ds As DataSet = Nothing
            Dim dr As IDataReader = Nothing

            If intRole <> Null.NullInteger Then

                If blnUseCache Then
                    ds = GetCachedUserList(intRole)
                End If

                If ds Is Nothing Then

                    ds = New DataSet
                    Dim dt As New DataTable

                    If intRole = PortalSettings.RegisteredRoleId Or intRole = -2 Then

                        Dim strCols As String = ""
                        For Each item As ListItem In chkSearchCols.Items
                            If item.Selected = True Then
                                strCols += item.Value & ","
                            End If
                        Next

                        dr = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("Connect_Accounts_GetUsers", intRole, PortalId, DotNetNuke.Data.DataProvider.Instance().GetNull(strSearch), strCols, blnShowDeleted)
                        dt.Load(dr)



                    Else

                        dr = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("Connect_Accounts_GetRoleMembers", intRole, PortalId)
                        dt.Load(dr)

                    End If

                    ds.Tables.Add(dt)


                    If blnShowDeleted = False AndAlso strSearch = Null.NullString Then
                        CacheUserList(intRole, ds)
                    End If

                End If

                If blnShowDeleted = True Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        btnHardDelete.Visible = AllowHardDelete
                    Else
                        btnHardDelete.Visible = False
                    End If
                Else
                    btnHardDelete.Visible = False
                End If

            End If

            Return ds

        End Function

        Private Function GetCachedUserList(ByVal RoleId As Integer) As DataSet
            Dim ds As DataSet = Nothing
            If Not DataCache.GetCache("DNNWERK_USERLIST_ROLEID" & RoleId.ToString) Is Nothing Then
                Return CType(DataCache.GetCache("DNNWERK_USERLIST_ROLEID" & RoleId.ToString), DataSet)
            End If
            Return ds
        End Function

        Private Sub CacheUserList(ByVal RoleId As Integer, ByVal ds As DataSet)
            If Not DataCache.GetCache("DNNWERK_USERLIST_ROLEID" & RoleId.ToString) Is Nothing Then
                DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & RoleId.ToString)
            End If
            DataCache.SetCache("DNNWERK_USERLIST_ROLEID" & RoleId.ToString, ds)
        End Sub

        Private Sub ClearCache()

            Dim objRoleController As New RoleController
            Dim roles As ArrayList = objRoleController.GetPortalRoles(PortalId)
            For Each role As RoleInfo In roles
                If Not DataCache.GetCache("DNNWERK_USERLIST_ROLEID" & role.RoleID.ToString) Is Nothing Then
                    DataCache.RemoveCache("DNNWERK_USERLIST_ROLEID" & role.RoleID.ToString)
                End If
            Next

        End Sub

        Private Sub BindReports()

            drpReports.Items.Clear()
            Dim reports As New List(Of UserReportInfo)
            reports = UserReportsController.GetReports(PortalId)
            For Each report As UserReportInfo In reports
                drpReports.Items.Add(New ListItem(report.FriendlyName, report.ReportId.ToString))
            Next

            Try
                drpReports.SelectedValue = CType(Session("UserReportsId"), String)
            Catch
            End Try

        End Sub

        Private Function GetReportResult(ByRef strError As String) As DataSet

            Dim ds As New DataSet()
            Dim userTable As New DataTable("UsersTable")
            ds.Tables.Add(userTable)
            Try
                Dim sql As String = GetSQL(Convert.ToInt32(drpReports.SelectedValue))
                If Not sql Is Nothing Then
                    If sql.Length > 0 Then
                        Try
                            Dim dr As IDataReader = DotNetNuke.Data.DataProvider.Instance().ExecuteSQL(sql)
                            ds.Load(dr, LoadOption.OverwriteChanges, userTable)
                        Catch ex As Exception
                            'error in sql syntax (most likely)
                            strError = "Your query contains errors, please check the report settings!"
                        End Try
                    End If
                End If
            Catch ex As Exception

            End Try


            Return ds

        End Function

        Private Function GetSQL(ByVal ReportId As Integer) As String

            Dim strSql As String = ""

            strSql = UserReportsController.GetReport(ReportId).Sql

            Return strSql.Replace("@PortalID", PortalSettings.PortalId.ToString)

        End Function

#End Region

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString("ManageTemplates.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ManageTemplates"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, Localization.GetString("Reports.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("Reports"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace


