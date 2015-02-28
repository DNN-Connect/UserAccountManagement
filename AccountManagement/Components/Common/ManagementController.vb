Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Portals

Public Class ManagementController

    Public Shared Sub SetUserAuthorized(ByVal objUser As UserInfo, ByVal PortalId As Integer)

        objUser.Membership.Approved = True
        DotNetNuke.Entities.Users.UserController.UpdateUser(PortalId, objUser)
        DotNetNuke.Entities.Users.UserController.ApproveUser(objUser)
        Dim role As RoleInfo = RoleController.Instance.GetRoleByName(PortalId, "Unverified Users")
        If Not role Is Nothing Then
            CacheUtilities.ClearRoleCache(role.RoleID)
        End If
        CacheUtilities.ClearRoleCache(PortalSettings.Current.RegisteredRoleId)

    End Sub

    Public Shared Sub SetUserUnAuthorized(ByVal objUser As UserInfo, ByVal PortalId As Integer)

        objUser.Membership.Approved = False
        DotNetNuke.Entities.Users.UserController.UpdateUser(PortalId, objUser)
        CacheUtilities.ClearRoleCache(PortalSettings.Current.RegisteredRoleId)

    End Sub

    Public Shared Function SetUserDeleted(ByVal objUser As UserInfo)

        If DotNetNuke.Entities.Users.UserController.DeleteUser(objUser, False, True) Then
            CacheUtilities.ClearRoleCache(PortalSettings.Current.RegisteredRoleId)
            CacheUtilities.ClearRoleCache(-2)
            Return True
        Else
            Return False
        End If

    End Function

End Class
