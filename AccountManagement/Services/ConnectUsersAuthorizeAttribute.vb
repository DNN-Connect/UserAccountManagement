Imports DotNetNuke.Web.Api
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities


Namespace Services

#Region " Security Access Levels "

    Public Enum SecurityAccessLevel As Integer
        Anonymous = 0
        Admin = 1
        ViewModule = 2
        EditModule = 4
        AddUser = 8
        EditUser = 16
        DeleteUser = 32
        ApproveUser = 64
        AddRole = 128
        EditRole = 256
        DeleteRole = 512
        AddToRole = 1024
        RemoveFromRole = 2048
        ApproveRoleMembership = 4096
        Authenticated = 8192
    End Enum

#End Region

    Public Class ConnectUsersAuthorizeAttribute
        Inherits AuthorizeAttributeBase
        Implements IOverrideDefaultAuthLevel

#Region "Properties"

        Public Property AccessLevel As SecurityAccessLevel
        Public Property UserInfo As UserInfo

#End Region

#Region "Constructors"

        Public Sub New()
            AccessLevel = SecurityAccessLevel.Admin
        End Sub

        Public Sub New(accessLevel As SecurityAccessLevel)
            Me.AccessLevel = accessLevel
        End Sub

#End Region

#Region " Public Methods "

        Public Overrides Function IsAuthorized(context As AuthFilterContext) As Boolean

            If AccessLevel = SecurityAccessLevel.Anonymous Then Return True ' save time by not going through the code below


            Dim portalSettings As DotNetNuke.Entities.Portals.PortalSettings = DotNetNuke.Entities.Portals.PortalController.Instance.GetCurrentPortalSettings()
            Dim moduleId As Integer = context.ActionContext.Request.FindModuleId
            Dim tabId As Integer = context.ActionContext.Request.FindTabId
            Dim blnAuthenticated As Boolean = False

            If Not HttpContextSource.Current.Request.IsAuthenticated Then
                blnAuthenticated = False
            Else
                UserInfo = UserController.Instance.GetCurrentUserInfo
                blnAuthenticated = True
            End If


            If blnAuthenticated Then

                If AccessLevel = SecurityAccessLevel.Admin Then

                    Return UserInfo.IsInRole(GetPortalSettings.AdministratorRoleName)

                ElseIf AccessLevel = SecurityAccessLevel.Authenticated Then

                    Return True

                ElseIf AccessLevel = SecurityAccessLevel.ViewModule Then

                    Return DotNetNuke.Security.Permissions.ModulePermissionController.HasModulePermission(context.ActionContext.Request.FindModuleInfo().ModulePermissions, "VIEW")

                ElseIf AccessLevel = SecurityAccessLevel.EditModule Then

                    Return DotNetNuke.Security.Permissions.ModulePermissionController.HasModulePermission(context.ActionContext.Request.FindModuleInfo().ModulePermissions, "EDIT")

                End If

            Else

                If AccessLevel = SecurityAccessLevel.ViewModule Then
                    Return DotNetNuke.Security.Permissions.ModulePermissionController.CanViewModule(context.ActionContext.Request.FindModuleInfo())
                End If

            End If


            Return False

        End Function
#End Region

#Region " Private Methods "
#End Region

    End Class

End Namespace

