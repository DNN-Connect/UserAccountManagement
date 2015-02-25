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

#End Region

#Region "Public Members"

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
