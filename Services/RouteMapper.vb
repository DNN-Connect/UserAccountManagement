Imports DotNetNuke.Web.Api
Imports System

Namespace Connect.Modules.UserManagement.AccountManagement.Services
    Public Class RouteMapper
        Implements IServiceRouteMapper

        Public Sub RegisterRoutes(ByVal mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes

            mapRouteManager.MapHttpRoute("ConnectAccounts", "default", "{controller}/{action}", New String() {"Connect.Modules.UserManagement.AccountManagement.Services"})

        End Sub
    End Class
End Namespace