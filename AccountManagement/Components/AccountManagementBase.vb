Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports Connect.Modules.Accounts.AccountManagement.Common

Public Class AccountManagementBase
    Inherits PortalModuleBase

#Region "Private Members"

    Private _rolecontroller As RoleController
    Private _settings As ModuleSettings
    Private _EditableUser As UserInfo = Nothing

#End Region

#Region "Public Members"

    Public ReadOnly Property SelectedRole As Integer
        Get
            Dim roleid As Integer = Settings.PreselectRole
            If Not Request.QueryString("RoleId") Is Nothing Then
                If IsNumeric(Request.QueryString("RoleId")) Then
                    roleid = Convert.ToInt32(Request.QueryString("RoleId"))
                End If
            End If
            Return roleid
        End Get
    End Property

    Public ReadOnly Property EditableUser As UserInfo
        Get
            If Not Request.QueryString("UserId") Is Nothing Then
                If IsNumeric(Request.QueryString("UserId")) Then
                    _EditableUser = UserController.Instance.GetUserById(PortalId, Convert.ToInt32(Request.QueryString("UserId")))
                End If
            End If
            Return _EditableUser
        End Get
    End Property

    Public ReadOnly Property ModuleView() As ViewControl
        Get
            If Not Request.QueryString("Action") Is Nothing Then

                Dim strAction As String = Request.QueryString("Action").ToLower
                Select Case strAction
                    Case "editaccount"
                        Return ViewControl.EditUser
                    Case Else
                        Return ViewControl.ListUsers
                End Select
            End If
            Return ViewControl.ListUsers
        End Get
    End Property

    Public ReadOnly Property RoleController As RoleController
        Get
            If _rolecontroller Is Nothing Then
                _rolecontroller = New RoleController
            End If
            Return _rolecontroller
        End Get
    End Property

    Public Shadows Property Settings() As ModuleSettings
        Get
            If _settings Is Nothing Then
                _settings = ModuleSettings.GetModuleSettings(ModuleId, TabId, UserId)
            End If
            Return _settings
        End Get
        Set(ByVal value As ModuleSettings)
            _settings = value
        End Set
    End Property

#End Region

#Region "Public Methods"



#End Region

End Class
