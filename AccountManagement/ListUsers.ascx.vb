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

Public Class ListUsers
    Inherits AccountManagementBase

#Region "Private Members"

    Private _IsReportResult As Boolean = False
    Private _displaycolumns As String()

#End Region

#Region "Event Handlers"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        SetRoleItems()
        BindUserRoles()

    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


    End Sub

#End Region

#Region "Private Methods"

    

#End Region

#Region "Public Functions"

    Private Sub BindUserRoles()

        chkUserRoles.DataSource = RoleController.GetRoles(PortalId)
        chkUserRoles.DataBind()

    End Sub

    Private Sub SetRoleItems()

        Dim objRoleController As New RoleController
        Dim roles As List(Of RoleInfo) = objRoleController.GetRoles(PortalId)
        Dim groups As ArrayList = RoleController.GetRoleGroups(PortalId)

        Dim strSelect As String = "<select class=""connect-accounts-roleselect"">"

        If groups.Count > 0 Then

            Dim PreselectedGroupId As Integer = Null.NullInteger
            If Settings.PreselectRole <> PortalSettings.RegisteredRoleId Then
                Dim objSelectedRole As RoleInfo = objRoleController.GetRoleById(PortalId, Settings.PreselectRole)
                If Not objSelectedRole Is Nothing Then
                    If objSelectedRole.RoleGroupID > 0 Then
                        PreselectedGroupId = objSelectedRole.RoleGroupID
                    End If
                End If
            End If

            'we have some role groups, add those first
            For Each objGroup As RoleGroupInfo In groups

                strSelect += "<optgroup label=""" & objGroup.RoleGroupName & """ data-class=""ui-icon-folder"">"

                For Each objRole As RoleInfo In roles
                    If objRole.RoleGroupID = objGroup.RoleGroupID Then

                        If (Not Settings.AllowedRoles Is Nothing AndAlso (Array.IndexOf(Settings.AllowedRoles, objRole.RoleID.ToString) > -1 Or Array.IndexOf(Settings.AllowedRoles, "all") > -1)) OrElse (Settings.AllowedRoles Is Nothing) Then
                            If SelectedRole = objRole.RoleID Then
                                strSelect += "<option value=""" & objRole.RoleID.ToString & """ selected>" & objRole.RoleName.ToString & "</option>"
                            Else
                                strSelect += "<option value=""" & objRole.RoleID.ToString & """>" & objRole.RoleName.ToString & "</option>"
                            End If
                        End If

                    End If

                Next
                strSelect += "</optgroup>"

            Next

            For Each objRole As RoleInfo In roles
                If objRole.RoleGroupID = Null.NullInteger Then

                    If (Not Settings.AllowedRoles Is Nothing AndAlso (Array.IndexOf(Settings.AllowedRoles, objRole.RoleID.ToString) > -1 Or Array.IndexOf(Settings.AllowedRoles, "all") > -1)) OrElse (Settings.AllowedRoles Is Nothing) Then



                        If SelectedRole = objRole.RoleID Then
                            strSelect += "<option value=""" & objRole.RoleID.ToString & """ selected>" & objRole.RoleName.ToString & "</option>"
                        Else
                            strSelect += "<option value=""" & objRole.RoleID.ToString & """>" & objRole.RoleName.ToString & "</option>"
                        End If

                    End If


                End If

            Next
        Else
            For Each objRole As RoleInfo In roles

                If SelectedRole = objRole.RoleID Then
                    strSelect += "<option value=""" & objRole.RoleID.ToString & """ selected>" & objRole.RoleName.ToString & "</option>"
                Else
                    strSelect += "<option value=""" & objRole.RoleID.ToString & """>" & objRole.RoleName.ToString & "</option>"
                End If

            Next
        End If

        If (Not Settings.AllowedRoles Is Nothing AndAlso (Array.IndexOf(Settings.AllowedRoles, "-2") > -1 Or Array.IndexOf(Settings.AllowedRoles, "all") > -1)) OrElse (Settings.AllowedRoles Is Nothing) Then

        End If
        strSelect += "<option value=""-2"">Deleted Users</option>"
        strSelect += "</select>"
        ctlRolesContainer.InnerHtml = strSelect

    End Sub

    Public Function GetEditUrl()
        Return NavigateURL(TabId, "", "Action=EditAccount")
    End Function

    Public Function GetDefaultPageSize() As String
        Return Settings.PageSize
    End Function

    Public Function GetPreSelectedRole() As String
        Return Settings.PreselectRole.ToString
    End Function

    Public Function GetDefaultSort() As String
        Return Settings.SortMode
    End Function

    Public Function GetSearchColumns() As String

        Dim strColums As String() = Settings.SearchableColumns
        Dim strValue As String = "'"
        For i As Integer = 0 To strColums.Length - 1
            If Not String.IsNullOrEmpty(strColums(i)) Then
                If i > 0 Then
                    strValue += ","
                End If
                strValue += strColums(i)
            End If
        Next
        strValue += "'"

        Return strValue

    End Function

    Public Function GetDisplayColumns() As String

        Dim strColums As String() = Settings.UserColumns
        Dim strValue As String = "[" & vbNewLine
        For i As Integer = 0 To strColums.Length - 1
            If Not String.IsNullOrEmpty(strColums(i)) Then
                If i > 0 Then
                    strValue += "," & vbNewLine & vbTab & vbTab
                End If
                strValue += "{ ""data"": """ & strColums(i) & """ }"

            End If
        Next

        'make sure we alway have a status column
        strValue += "," & vbNewLine & vbTab & vbTab & " { ""data"": ""Status"", ""render"": function ( data, type, full, meta ) { return getStatusLinks(full); } }"

        'make sure we alway have an edit column
        strValue += "," & vbNewLine & vbTab & vbTab & " { ""data"": ""UserId"", ""render"": function ( data, type, full, meta ) { return getActionLinks(full); } }"

        strValue += "]"


        Return strValue

    End Function

    Public Function GetColumnsHTML() As String

        Dim strColums As String() = Settings.UserColumns
        Dim strValue As String = ""
        For i As Integer = 0 To strColums.Length - 1
            If Not String.IsNullOrEmpty(strColums(i)) Then
                Dim strHead As String = Localization.GetString(strColums(i) & ".Head", LocalResourceFile)
                If String.IsNullOrEmpty(strHead) Then
                    strHead = strColums(i)
                End If
                strValue += "<th>" & strHead & "</th>"
            End If
        Next
        strValue += "<th>" & Localization.GetString("Status.Head", LocalResourceFile) & "</th>"
        strValue += "<th>" & Localization.GetString("Edit.Head", LocalResourceFile) & "</th>"
        Return strValue

    End Function

    Public Function GetUnverifiedUsersRole() As String

        Dim role As RoleInfo = RoleController.GetRoleByName(PortalId, "Unverified Users")
        If Not role Is Nothing Then
            Return role.RoleID
        End If

        Return 4

    End Function

#End Region

End Class