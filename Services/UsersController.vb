Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Web.Api
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web
Imports System.Web.Http

Namespace Connect.Modules.UserManagement.AccountManagement.Services
    Public Class UsersController
        Inherits DnnApiController

        <AllowAnonymous> _
        <HttpPost> _
        Public Function AutoSuggestResult(ByVal postData As RequestDTO) As System.Net.Http.HttpResponseMessage

            Dim httpResponseMessage As System.Net.Http.HttpResponseMessage

            Try

                Dim resultList As New List(Of AutoSuggestResponseDTO)
                Dim dr As IDataReader = Nothing
                If postData.RoleId = PortalSettings.RegisteredRoleId Then
                    dr = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("Connect_Accounts_SearchRegisteredUsers", postData.RoleId, postData.PortalId, postData.SearchText, postData.SearchCols)
                ElseIf postData.RoleId = -2 Then
                    dr = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("Connect_Accounts_SearchDeletedUsers", postData.RoleId, postData.PortalId, postData.SearchText, postData.SearchCols)
                Else
                    dr = DotNetNuke.Data.DataProvider.Instance().ExecuteReader("Connect_Accounts_SearchRoleMembers", postData.RoleId, postData.SearchText, postData.SearchCols)
                End If

                If Not dr Is Nothing Then
                    While dr.Read
                        Dim result As New AutoSuggestResponseDTO
                        result.EntryIcon = ""
                        result.EntryName = Convert.ToString(dr("Displayname"))
                        result.EntryUrl = NavigateURL(postData.TabId, "", "uid=" & Convert.ToString(dr("UserId")), "RoleId=" & postData.RoleId.ToString, "Action=edit")
                        resultList.Add(result)
                    End While
                    dr.Close()
                    dr.Dispose()
                End If

                httpResponseMessage = Me.Request.CreateResponse(Of List(Of AutoSuggestResponseDTO))(HttpStatusCode.OK, resultList)

            Catch ex As System.Exception
                httpResponseMessage = Me.Request.CreateResponse(Of String)(HttpStatusCode.OK, ex.Message)
            End Try

            Return httpResponseMessage

        End Function

    End Class

    Public Class RequestDTO
        Public Property TabId As Integer
        Public Property PortalId As Integer
        Public Property RoleId As Integer
        Public Property SearchText As String
        Public Property SearchCols As String
    End Class

    Public Class AutoSuggestResponseDTO
        Public Property EntryName As String
        Public Property EntryUrl As String
        Public Property EntryIcon As String
    End Class

End Namespace