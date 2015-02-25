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
Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace Connect.Modules.UserManagement.AccountRegistration
    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    BindPages()
                    BindRoles()

                    If (settings.Contains("ExternalInterface")) Then txtInterface.Text = settings("ExternalInterface").ToString()
                    If (settings.Contains("ShowUserName")) Then drpUsernameMode.SelectedValue = settings("ShowUserName").ToString()
                    If (settings.Contains("ShowDisplayName")) Then drpDisplaynameMode.SelectedValue = settings("ShowDisplayName").ToString()
                    If (Settings.Contains("RedirectAfterSubmit")) Then drpRedirectAfterSubmit.SelectedValue = Settings("RedirectAfterSubmit").ToString()
                    If (Settings.Contains("RedirectAfterLogin")) Then drpRedirectAfterLogin.SelectedValue = Settings("RedirectAfterLogin").ToString()
                    If (settings.Contains("UsermanagementTab")) Then drpUserManagementTab.SelectedValue = settings("UsermanagementTab").ToString()
                    If (settings.Contains("AddToRoleOnSubmit")) Then drpAddToRole.SelectedValue = settings("AddToRoleOnSubmit").ToString()
                    If (settings.Contains("NotifyRole")) Then drpNotifyRole.Items.FindByText(settings("NotifyRole").ToString()).Selected = True
                    If (settings.Contains("NotifyUser")) Then chkNotifyUser.Checked = CType(settings("NotifyUser"), Boolean)
                    If (settings.Contains("AddToRoleStatus")) Then drpRoleStatus.SelectedValue = CType(settings("AddToRoleStatus"), String)
                    If (settings.Contains("ReCaptchaKey")) Then txtPrivateCaptchaKey.Text = CType(settings("ReCaptchaKey"), String)
                    If (Settings.Contains("CompareFirstNameLastName")) Then chkCompareFirstNameLastName.Checked = CType(Settings("CompareFirstNameLastName"), Boolean)
                    If (Settings.Contains("ValidateEmailThroughRegex")) Then chkValidateEmailThroughRegex.Checked = CType(Settings("ValidateEmailThroughRegex"), Boolean)
                    If Settings.Contains("EmailRegex") Then
                        txtEmailRegex.Text = CType(Settings("EmailRegex"), String)
                    Else
                        Try
                            txtEmailRegex.Text = UserController.GetUserSettings(PortalId)("Security_EmailValidation")
                        Catch
                        End Try                        
                    End If                    

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                objModules.UpdateTabModuleSetting(TabModuleId, "ReCaptchaKey", txtPrivateCaptchaKey.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "ExternalInterface", txtInterface.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowUserName", drpUsernameMode.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowDisplayName", drpDisplaynameMode.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "RedirectAfterSubmit", drpRedirectAfterSubmit.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "RedirectAfterLogin", drpRedirectAfterLogin.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "UsermanagementTab", drpUserManagementTab.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "AddToRoleOnSubmit", drpAddToRole.SelectedValue)
                'we need the rolename for sending mails to users, therefor store here the rolename rather than the id!
                objModules.UpdateTabModuleSetting(TabModuleId, "NotifyRole", drpNotifyRole.SelectedItem.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "NotifyUser", chkNotifyUser.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "AddToRoleStatus", drpRoleStatus.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "CompareFirstNameLastName", chkCompareFirstNameLastName.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "ValidateEmailThroughRegex", chkValidateEmailThroughRegex.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "EmailRegex", txtEmailRegex.Text)

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub BindPages()

            Dim tabs As System.Collections.Generic.List(Of Entities.Tabs.TabInfo) = TabController.GetPortalTabs(PortalId, Null.NullInteger, True, True, False, False)

            drpRedirectAfterSubmit.DataSource = tabs
            drpRedirectAfterSubmit.DataBind()

            drpRedirectAfterLogin.DataSource = tabs
            drpRedirectAfterLogin.DataBind()

            drpUserManagementTab.DataSource = tabs
            drpUserManagementTab.DataBind()

        End Sub

        Private Sub BindRoles()

            Dim rc As New Security.Roles.RoleController
            Dim roles As ArrayList = rc.GetPortalRoles(PortalId)

            drpAddToRole.DataSource = roles
            drpAddToRole.DataBind()
            drpAddToRole.Items.Insert(0, New ListItem("---", "-1"))

            drpNotifyRole.DataSource = roles
            drpNotifyRole.DataBind()
            drpNotifyRole.Items.Insert(0, New ListItem("---", "-1"))

        End Sub


#End Region

    End Class
End Namespace


