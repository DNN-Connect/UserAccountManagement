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
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Security.Roles

Namespace Connect.Modules.UserManagement.AccountManagement

    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    LocalizeForm()
                    BindAllowedRoles()
                    BindPreselectedRole()

                    If (Settings.Contains("AllowReports")) Then chkAllowReports.Checked = CType(Settings("AllowReports"), Boolean)
                    If (Settings.Contains("AllowCreate")) Then chkAllowCreate.Checked = CType(Settings("AllowCreate"), Boolean)
                    If (Settings.Contains("AllowDelete")) Then chkAllowDelete.Checked = CType(Settings("AllowDelete"), Boolean)
                    If (Settings.Contains("AllowHardDelete")) Then chkAllowHardDelete.Checked = CType(Settings("AllowHardDelete"), Boolean)
                    If (Settings.Contains("AllowExport")) Then

                        Dim blnAllowExport As Boolean = CType(Settings("AllowExport"), Boolean)
                        If blnAllowExport Then
                            chkAllowExport.Checked = True
                            dvExportFields.Visible = True
                            BindExportFields()
                        Else
                            chkAllowExport.Checked = False
                            dvExportFields.Visible = False
                        End If

                    End If



                    If (Settings.Contains("AllowMessageUsers")) Then chkAllowSendMessages.Checked = CType(Settings("AllowMessageUsers"), Boolean)

                    If Settings.Contains("ShowUserDetailTabs") Then
                        For Each detailtab As String In CType(Settings("ShowUserDetailTabs"), String).Split(Char.Parse(","))
                            For Each item As ListItem In chkUserTabs.Items
                                If item.Value.ToLower = detailtab.ToLower Then
                                    item.Selected = True
                                End If
                            Next
                        Next
                    End If

                    If Settings.Contains("AdditionalControls") Then txtAditionalControls.Text = CType(Settings("AdditionalControls"), String)

                End If

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                objModules.UpdateTabModuleSetting(TabModuleId, "AllowReports", chkAllowReports.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowCreate", chkAllowCreate.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowDelete", chkAllowDelete.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowHardDelete", chkAllowHardDelete.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowExport", chkAllowExport.Checked.ToString)

                If chkAllowExport.Checked Then
                    objModules.UpdateTabModuleSetting(TabModuleId, "ExportFields", txtExportFields.Text)
                Else
                    objModules.DeleteTabModuleSetting(TabModuleId, "ExportFields")
                End If

                objModules.UpdateTabModuleSetting(TabModuleId, "AllowMessageUsers", chkAllowSendMessages.Checked.ToString)
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowReports", chkAllowReports.Checked.ToString)
                Dim strAllowedRoles As String = ""
                For Each item As ListItem In chkAllowedRoles.Items
                    If item.Selected Then
                        strAllowedRoles += item.Value & ";"
                    End If
                Next
                If strAllowedRoles.Length = 0 Then
                    strAllowedRoles = "all;"
                End If
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowedRoles", strAllowedRoles)

                If String.IsNullOrEmpty(drpPreselectRole.SelectedValue) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, "PreSelectRole", PortalSettings.RegisteredRoleId.ToString)
                Else
                    objModules.UpdateTabModuleSetting(TabModuleId, "PreSelectRole", drpPreselectRole.SelectedValue)
                End If

                Dim strShowUserDetailTabs As String = ""
                For Each item As ListItem In chkUserTabs.Items
                    If item.Selected Then
                        strShowUserDetailTabs += item.Value & ","
                    End If
                Next
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowUserDetailTabs", strShowUserDetailTabs)
                objModules.UpdateTabModuleSetting(TabModuleId, "AdditionalControls", txtAditionalControls.Text)

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub BindAllowedRoles()

            Dim rc As New Security.Roles.RoleController
            Dim roles As ArrayList = rc.GetPortalRoles(PortalId)

            chkAllowedRoles.DataSource = roles
            chkAllowedRoles.DataBind()
            chkAllowedRoles.Items.Add(New ListItem(Localization.GetString("DeletedAccounts", LocalResourceFile), "-2"))
            chkAllowedRoles.Items.Add(New ListItem(Localization.GetString("AllRoles", LocalResourceFile), "all"))

            If Settings.Contains("AllowedRoles") Then
                For Each allowedrole As String In CType(Settings("AllowedRoles"), String).Split(Char.Parse(";"))
                    For Each item As ListItem In chkAllowedRoles.Items
                        If item.Value.ToLower = allowedrole.ToLower Then
                            item.Selected = True
                        End If
                    Next
                Next
            End If

            If chkAllowedRoles.SelectedIndex = -1 Then
                chkAllowedRoles.Items.FindByValue("all").Selected = True
            End If

        End Sub


        Private Sub BindPreselectedRole()

            Dim rc As New RoleController
            Dim roles As ArrayList = rc.GetPortalRoles(PortalId)
            Dim preselectRoles As New List(Of RoleInfo)

            If chkAllowedRoles.Items.FindByValue("all").Selected Then
                For Each role As RoleInfo In roles
                    preselectRoles.Add(role)
                Next
            Else
                For Each role As RoleInfo In roles
                    If chkAllowedRoles.Items.FindByValue(role.RoleID.ToString).Selected Then
                        preselectRoles.Add(role)
                    End If
                Next
            End If

            drpPreselectRole.DataSource = preselectRoles
            drpPreselectRole.DataBind()

            If chkAllowedRoles.Items.FindByValue("all").Selected Or chkAllowedRoles.Items.FindByValue("-2").Selected Then
                drpPreselectRole.Items.Add(New ListItem(Localization.GetString("DeletedAccounts", LocalResourceFile), "-2"))
            End If

            If Settings.Contains("PreSelectRole") Then
                Try
                    drpPreselectRole.SelectedValue = CType(Settings("PreSelectRole"), String)
                Catch
                End Try
            End If


        End Sub

        Private Sub BindExportFields()

            Dim strExportFields As String = ""

            If (Settings.Contains("ExportFields")) Then
                strExportFields = Convert.ToString(Settings("ExportFields"))
            End If

            If String.IsNullOrEmpty(strExportFields.Trim) Then

                strExportFields = "User_UserId,User_Username,User_Firstname,User_Lastname,User_Email,User_CreatedDate,User_LastLoginDate,"

                Dim props As ProfilePropertyDefinitionCollection = ProfileController.GetPropertyDefinitionsByPortal(PortalSettings.PortalId)
                For Each prop As ProfilePropertyDefinition In props
                    strExportFields += prop.PropertyName & ","
                Next

            End If

            txtExportFields.Text = strExportFields

        End Sub

        Private Sub LocalizeForm()
            For Each item As ListItem In chkUserTabs.Items
                item.Text = Localization.GetString("tab" & item.Value, LocalResourceFile)
            Next
        End Sub

#End Region

        Private Sub chkAllowExport_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllowExport.CheckedChanged
            dvExportFields.Visible = chkAllowExport.Checked
        End Sub

        Private Sub chkAllowedRoles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkAllowedRoles.SelectedIndexChanged
            BindPreselectedRole()
        End Sub

    End Class
End Namespace


