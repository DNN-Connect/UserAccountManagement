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
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership
Imports Telerik.Web.UI
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Web.UI.WebControls
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Framework.JavaScriptLibraries

Public Class EditUser
    Inherits AccountManagementBase

#Region "Private Members"



#End Region

#Region "Event Handlers"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        SetupPage()
        RegisterResources()

    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            BindUser()
        End If

    End Sub

    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click

        System.Threading.Thread.Sleep(5000)
        UpdateUser()

    End Sub

#End Region

#Region "Private Methods"

    Private Sub SetupPage()

        cmdBack_Top.NavigateUrl = NavigateURL(TabId, "", "RoleId=" & Request.QueryString("RoleId"))
        cmdBack_Bottom.NavigateUrl = NavigateURL(TabId, "", "RoleId=" & Request.QueryString("RoleId"))

        cmdBack_Bottom.Text = Localization.GetString("cmdCancel", LocalResourceFile)
        cmdDelete.Text = Localization.GetString("cmdDelete", LocalResourceFile)
        cmdUpdate.Text = Localization.GetString("cmdUpdate", LocalResourceFile)

        lblUsername.Text = Localization.GetString("lblUsername", LocalResourceFile)
        lblEmail.Text = Localization.GetString("lblEmail", LocalResourceFile)
        lblFirstname.Text = Localization.GetString("lblFirstname", LocalResourceFile)
        lblLastname.Text = Localization.GetString("lblLastname", LocalResourceFile)
        lblDisplayname.Text = Localization.GetString("lblDisplayname", LocalResourceFile)
        lblMessagingHeading.Text = Localization.GetString("lblMessagingHeading", LocalResourceFile)
        lblPasswordHeading.Text = Localization.GetString("lblPasswordHeading", LocalResourceFile)
        lblProfileHeading.Text = Localization.GetString("lblProfileHeading", LocalResourceFile)
        lblRolesHeading.Text = Localization.GetString("lblRolesHeading", LocalResourceFile)
        lblSitesHeading.Text = Localization.GetString("lblSitesHeading", LocalResourceFile)
        lblInfoHeading.Text = Localization.GetString("lblInfoHeading", LocalResourceFile)
        lblAccountData.Text = Localization.GetString("lblAccountData", LocalResourceFile)
        lblAccountInfoText.Text = Localization.GetString("lblAccountInfoText", LocalResourceFile)
        lblMemberSince.Text = Localization.GetString("lblMemberSince", LocalResourceFile)
        lblLastlogin.Text = Localization.GetString("lblLastlogin", LocalResourceFile)
        lblLastActivity.Text = Localization.GetString("lblLastActivity", LocalResourceFile)
        lblLastLockout.Text = Localization.GetString("lblLastLockout", LocalResourceFile)
        lblCurrentActivity.Text = Localization.GetString("lblCurrentActivity", LocalResourceFile)
        lblAuthorized.Text = Localization.GetString("lblAuthorized", LocalResourceFile)
        lblLockStatus.Text = Localization.GetString("lblLockStatus", LocalResourceFile)
        lblForcePasswordChange.Text = Localization.GetString("lblForcePasswordChange", LocalResourceFile)

    End Sub

    Private Sub BindUser()

        Dim objUser As UserInfo = EditableUser
        If Not objUser Is Nothing Then

            lblEditHead.Text = objUser.DisplayName
            cmdDelete.Visible = True

            txtUsername.Text = objUser.Username
            txtEmail.Text = objUser.Email
            txtFirstname.Text = objUser.FirstName
            txtLastname.Text = objUser.LastName
            txtDisplayname.Text = objUser.DisplayName

            lblMemberSinceValue.Text = objUser.CreatedOnDate.ToShortDateString & ", " & objUser.CreatedOnDate.ToShortTimeString
            lblLastActivityValue.Text = objUser.Membership.LastActivityDate.ToShortDateString & ", " & objUser.Membership.LastActivityDate.ToShortTimeString
            lblLastLockoutValue.Text = objUser.Membership.LastLockoutDate.ToShortDateString & ", " & objUser.Membership.LastLockoutDate.ToShortTimeString
            lblLastloginValue.Text = objUser.Membership.LastLoginDate.ToShortDateString & ", " & objUser.Membership.LastLoginDate.ToShortTimeString
            lblCurrentActivityValue.Text = IIf(objUser.Membership.IsOnLine, Localization.GetString("online", LocalResourceFile), Localization.GetString("offline", LocalResourceFile))
            lblLockStatusValue.Text = IIf(objUser.Membership.LockedOut, Localization.GetString("yes", LocalResourceFile), Localization.GetString("no", LocalResourceFile))

            chkForcePasswordChange.Checked = IIf(objUser.Membership.UpdatePassword, True, False)
            chkAuthorized.Checked = IIf(objUser.Membership.Approved, True, False)

            imgUser.ImageUrl = String.Format(DotNetNuke.Common.Globals.UserProfilePicRelativeUrl(), objUser.UserID, 64, 64)

        Else

            lblEditHead.Text = Localization.GetString("lblCreateHead", LocalResourceFile)
            cmdDelete.Visible = False
            cmdUpdate.Text = Localization.GetString("cmdCreate", LocalResourceFile)

        End If

    End Sub

    Private Sub RegisterResources()

        JavaScript.RequestRegistration(CommonJs.DnnPlugins)
        JavaScript.RequestRegistration(CommonJs.jQueryUI)

    End Sub

    Private Function InitializeUser() As UserInfo
        Return New UserInfo
    End Function

    Private Sub UpdateUser()

        Dim objUser As UserInfo = EditableUser
        Dim blnCreate As Boolean = False
        Dim blnUsernameFromEmail As Boolean = PortalController.GetPortalSettingAsBoolean("Registration_UseEmailAsUserName", PortalId, False)

        If objUser Is Nothing Then

            'initialize for account creation
            objUser = InitializeUser()
            blnCreate = True

            If blnUsernameFromEmail Then
                objUser.Username = txtEmail.Text
            Else
                objUser.Username = txtUsername.Text
            End If

        Else

            If blnUsernameFromEmail Then
                If txtEmail.Text.Trim.ToLower <> EditableUser.Email.ToLower Then
                    UserController.ChangeUsername(objUser.UserID, txtEmail.Text)
                    objUser = UserController.Instance.GetUserById(PortalId, EditableUser.UserID)
                End If
            Else
                If txtUsername.Text.Trim.ToLower <> EditableUser.Username.ToLower Then
                    UserController.ChangeUsername(objUser.UserID, txtUsername.Text)
                    objUser = UserController.Instance.GetUserById(PortalId, EditableUser.UserID)
                End If
            End If

        End If

        objUser.Membership.UpdatePassword = chkForcePasswordChange.Checked
        objUser.FirstName = txtFirstname.Text
        objUser.LastName = txtLastname.Text
        objUser.DisplayName = txtDisplayname.Text
        objUser.Email = txtEmail.Text

        If blnCreate Then

            Dim createStatus As UserCreateStatus = UserController.CreateUser(objUser)
            Select Case createStatus
                Case UserCreateStatus.AddUser

                Case UserCreateStatus.Success

            End Select

        Else
            UserController.UpdateUser(PortalId, objUser)
        End If


        If objUser.Membership.Approved = False Then
            If chkAuthorized.Checked Then
                ManagementController.SetUserAuthorized(objUser, PortalId)
            End If
        Else
            If Not chkAuthorized.Checked Then
                ManagementController.SetUserUnAuthorized(objUser, PortalId)
            End If
        End If

    End Sub

#End Region

#Region "Public Functions"



#End Region

End Class