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
Imports DotNetNuke.Security.Roles


Namespace Connect.Modules.UserManagement.AccountManagement
    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    LocalizeForm()
                    BindRoles()

                    If (Settings.Contains("AllowReports")) Then chkAllowReports.Checked = CType(Settings("AllowReports"), Boolean)
                    If (Settings.Contains("AllowCreate")) Then chkAllowCreate.Checked = CType(Settings("AllowCreate"), Boolean)
                    If (Settings.Contains("AllowDelete")) Then chkAllowDelete.Checked = CType(Settings("AllowDelete"), Boolean)
                    If (Settings.Contains("AllowHardDelete")) Then chkAllowHardDelete.Checked = CType(Settings("AllowHardDelete"), Boolean)
                    If (Settings.Contains("AllowExport")) Then chkAllowExport.Checked = CType(Settings("AllowExport"), Boolean)

                    If Settings.Contains("AllowedRoles") Then
                        For Each allowedrole As String In CType(Settings("AllowedRoles"), String).Split(Char.Parse(";"))
                            For Each item As ListItem In chkAllowedRoles.Items
                                If item.Value.ToLower = allowedrole.ToLower Then
                                    item.Selected = True
                                End If
                            Next
                        Next
                    End If

                    If Settings.Contains("PreSelectRole") Then drpPreselectRole.SelectedValue = CType(Settings("PreSelectRole"), String)

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
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowReports", chkAllowReports.Checked.ToString)
                Dim strAllowedRoles As String = ""
                For Each item As ListItem In chkAllowedRoles.Items
                    If item.Selected Then
                        strAllowedRoles += item.Value & ";"
                    End If
                Next
                objModules.UpdateTabModuleSetting(TabModuleId, "AllowedRoles", strAllowedRoles)
                objModules.UpdateTabModuleSetting(TabModuleId, "PreSelectRole", drpPreselectRole.SelectedValue)
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

        Private Sub BindRoles()

            Dim rc As New RoleController
            Dim roles As List(Of RoleInfo) = rc.GetRoles(PortalId)

            drpPreselectRole.DataSource = roles
            drpPreselectRole.DataBind()
            drpPreselectRole.Items.Insert(0, New ListItem("---", ""))
            drpPreselectRole.Items.Add(New ListItem(Localization.GetString("DeletedAccounts", LocalResourceFile), "-2"))

            chkAllowedRoles.DataSource = roles
            chkAllowedRoles.DataBind()
            chkAllowedRoles.Items.Add(New ListItem(Localization.GetString("DeletedAccounts", LocalResourceFile), "-2"))
            chkAllowedRoles.Items.Add(New ListItem(Localization.GetString("AllRoles", LocalResourceFile), "all"))

        End Sub

        Private Sub LocalizeForm()
            For Each item As ListItem In chkUserTabs.Items
                item.Text = Localization.GetString("tab" & item.Value, LocalResourceFile)
            Next
        End Sub

#End Region

    End Class
End Namespace


