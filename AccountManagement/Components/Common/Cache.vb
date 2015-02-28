Public Class CacheUtilities

    Public Shared Sub ClearRoleCache(RoleId As Integer)
        Dim keyCache As String = String.Format("DNNCONNECT_USERS_{0}", RoleId.ToString)
        DataCache.RemoveCache(keyCache)
    End Sub

End Class
