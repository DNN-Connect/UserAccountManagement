Imports System
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Web.Api
Imports DotNetNuke.Services.Localization
Imports System.IO
Imports DotNetNuke.Security
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports System.Runtime.Serialization
Imports DotNetNuke.Common.Utilities.Json
Imports DotNetNuke.Entities.Modules
Imports System.Globalization
Imports DotNetNuke.Entities.Portals

Namespace Services

    Public Class UserAccountsController
        Inherits DnnApiController

        Public Class ActionResultDTO
            Public Property Success As Boolean = False
            Public Property Response As String = Null.NullString
        End Class

        Public Class ActionRequestDTO
            Public Property RoleId As Integer = Null.NullInteger
            Public Property PortalId As Integer = Null.NullInteger
            Public Property UserId As Integer = Null.NullInteger
        End Class

        Public Class RequestDTO
            Public Property RoleId As Integer = Null.NullInteger
            Public Property PortalId As Integer = Null.NullInteger
            Public Property SearchPattern As String = Null.NullString
            Public Property SearchColumns As String = Null.NullString
            Public Property Status As Integer = Null.NullInteger
        End Class

        Public Class DataTableRequestDTO
            Public Property draw As Integer = Null.NullInteger
            Public Property start As Integer = Null.NullInteger
            Public Property length As Integer = Null.NullInteger
            Public Property roleId As Integer = Null.NullInteger
            Public Property portalId As Integer = Null.NullInteger
            Public Property searchPattern As String = Null.NullString
            Public Property searchColumns As String = Null.NullString
            Public Property orderPattern As String = Null.NullString
        End Class

        Public Class DataTableResponseDTO
            Public Property draw As Integer = Null.NullInteger
            Public Property recordsTotal As Integer = Null.NullInteger
            Public Property recordsFiltered As Integer = Null.NullInteger
            Public Property data As Object = Nothing
            Public Property [error] As String = Null.NullString
        End Class

        Public Class UserDTO
            Implements IHydratable

            Public Property UserId As Integer
            Public Property UserName As String
            Public Property FirstName As String
            Public Property LastName As String
            Public Property DisplayName As String
            Public Property CreatedDate As String
            Public Property Email As String
            Public Property PortalId As String
            Public Property PostalCode As String
            Public Property City As String
            Public Property Country As String
            Public Property Telephone As String
            Public Property Cell As String
            Public Property PreferredLocale As String
            Public Property Status As String
            Public Property RoleId As String
            Public Property Authorised As String
            Public Property Photo As String

            Public Sub Fill(dr As IDataReader) Implements IHydratable.Fill


                UserId = Convert.ToInt32(dr("UserId"))
                UserName = Convert.ToString(dr("UserName"))
                FirstName = Convert.ToString(dr("FirstName"))
                LastName = Convert.ToString(dr("LastName"))
                DisplayName = Convert.ToString(dr("DisplayName"))
                CreatedDate = Convert.ToDateTime(dr("CreatedDate")).ToShortDateString
                Email = Convert.ToString(dr("Email"))
                PortalId = Convert.ToString(dr("PortalId"))
                PostalCode = Convert.ToString(dr("UserId"))
                City = Convert.ToString(dr("City"))
                Country = Convert.ToString(dr("Country"))
                Telephone = Convert.ToString(dr("Telephone"))
                Cell = Convert.ToString(dr("Cell"))
                Status = Convert.ToString(dr("Status"))
                Authorised = Convert.ToString(dr("Authorised"))
                RoleId = Convert.ToString(dr("RoleId"))
                PreferredLocale = Convert.ToString(dr("PreferredLocale"))

                If Not String.IsNullOrEmpty(Convert.ToString(dr("Photo"))) Then
                    Photo = "<img src=""" & PortalController.Instance.GetCurrentPortalSettings.HomeDirectory & Convert.ToString(dr("Photo")) & """ />"
                Else
                    Photo = ""
                End If

            End Sub

            Public Property KeyID As Integer Implements IHydratable.KeyID

        End Class

        Public Class RoleDTO
            Public Property RoleId As Integer
        End Class

        Public Class UserRoleDTO
            Public Property RoleId As Integer
            Public Property RoleName As String
        End Class

        Public Class SetUserRolesDTO
            Public Property UserId As Integer
            Public Property PortalId As Integer
            Public Property Roles As String()
        End Class


#Region "Private Members"

        Private _User As UserInfo = Nothing
        Private _Role As RoleInfo = Nothing

#End Region

#Region "Service Methods "

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetUserRoles(postData As SetUserRolesDTO) As HttpResponseMessage

            Dim rolesDTO As New List(Of UserRoleDTO)

            Dim user As UserInfo = UserController.Instance.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                Dim rc As New RoleController
                Dim roles As IList(Of UserRoleInfo) = RoleController.Instance.GetUserRoles(user, True)
                For Each role As UserRoleInfo In roles

                    Dim blnIsStillInRole As Boolean = False
                    For Each newRoleId As String In postData.Roles
                        If Convert.ToInt32(newRoleId) = role.RoleID Then
                            blnIsStillInRole = True
                            Exit For
                        End If
                    Next

                    If Not blnIsStillInRole Then
                        RoleController.Instance.UpdateUserRole(postData.PortalId, postData.UserId, role.RoleID, RoleStatus.Disabled, False, True)
                    End If

                Next

                For Each newRoleId As String In postData.Roles
                    RoleController.Instance.AddUserRole(postData.PortalId, postData.UserId, Convert.ToInt32(newRoleId), RoleStatus.Approved, False, Date.Now, Null.NullDate)
                Next

            End If

            Return Request.CreateResponse(HttpStatusCode.OK)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function GetUserRoles(postData As ActionRequestDTO) As HttpResponseMessage

            Dim rolesDTO As New List(Of UserRoleDTO)

            Dim user As UserInfo = UserController.Instance.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                Dim roles As IList(Of UserRoleInfo) = RoleController.Instance.GetUserRoles(user, True)
                For Each role As UserRoleInfo In roles
                    Dim roleDTO As New UserRoleDTO
                    roleDTO.RoleId = role.RoleID
                    roleDTO.RoleName = role.RoleName
                    rolesDTO.Add(roleDTO)
                Next
            End If

            Dim strJson As Object = Serialize(Of List(Of UserRoleDTO))(rolesDTO)

            Return Request.CreateResponse(HttpStatusCode.OK, rolesDTO)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function ProcessDataTablesRequest(postData As DataTableRequestDTO) As HttpResponseMessage

            Dim users As List(Of UserDTO) = GetUsers(postData.roleId, postData.portalId, postData.searchPattern, postData.searchColumns)

            For Each Pattern As String In postData.orderPattern.Split(Char.Parse("|"))
                users = SortUsers(users, Pattern)
            Next

            Dim filtered As New List(Of UserDTO)

            Dim iStart As Integer = postData.start
            Dim iEnd As Integer = iStart + postData.length - 1

            For i As Integer = iStart To iEnd
                If i + 1 <= users.Count Then
                    filtered.Add(users(i))
                End If
            Next

            Dim objResponse As New DataTableResponseDTO
            objResponse.draw = postData.draw
            objResponse.recordsTotal = users.Count
            objResponse.recordsFiltered = users.Count
            objResponse.data = filtered
            objResponse.error = Nothing

            Dim strJson As Object = Serialize(Of DataTableResponseDTO)(objResponse)

            Return Request.CreateResponse(HttpStatusCode.OK, objResponse)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetRoleMembershipApproved(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                Try

                    Dim roleController As New RoleController
                    Dim role As RoleInfo = roleController.GetRoleById(postData.PortalId, postData.RoleId)
                    If Not role Is Nothing Then
                        roleController.UpdateUserRole(postData.PortalId, postData.UserId, postData.RoleId, RoleStatus.Approved, False, False)
                        CacheUtilities.ClearRoleCache(postData.RoleId)
                    End If

                    result.Success = True
                    result.Response = ""
                Catch ex As Exception
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_SET_USERROLE_APPROVED"
                End Try
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetRoleMembershipPending(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                Try

                    Dim roleController As New RoleController
                    Dim role As RoleInfo = roleController.GetRoleById(postData.PortalId, postData.RoleId)
                    If Not role Is Nothing Then
                        roleController.UpdateUserRole(postData.PortalId, postData.UserId, postData.RoleId, RoleStatus.Pending, False, False)
                        CacheUtilities.ClearRoleCache(postData.RoleId)
                    End If

                    result.Success = True
                    result.Response = ""
                Catch ex As Exception
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_SET_USERROLE_PENDING"
                End Try
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetRoleMembershipDeleted(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing AndAlso Not user.IsInRole(PortalSettings.AdministratorRoleName) Then
                Try

                    Dim rc As New RoleController
                    Dim role As RoleInfo = rc.GetRoleById(postData.PortalId, postData.RoleId)
                    If Not role Is Nothing Then
                        RoleController.DeleteUserRole(user, role, PortalSettings, False)
                        CacheUtilities.ClearRoleCache(postData.RoleId)
                    End If

                    result.Success = True
                    result.Response = ""
                Catch ex As Exception
                    result.Success = False
                    result.Response = "ERROR_COULD_DELETE_USERROLE"
                End Try
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetUserUnAuthorized(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                Try
                    ManagementController.SetUserUnAuthorized(user, postData.PortalId)
                    result.Success = True
                    result.Response = ""
                Catch ex As Exception
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_SET_USER_UNAUTHORIZED"
                End Try
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetUserAuthorized(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                Try
                    ManagementController.SetUserAuthorized(user, postData.PortalId)
                    result.Success = True
                    result.Response = ""
                Catch ex As Exception
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_SET_USER_AUTHORIZED"
                End Try
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetUserDeleted(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)
            If Not user Is Nothing Then
                If ManagementController.SetUserDeleted(user) Then
                    CacheUtilities.ClearRoleCache(PortalSettings.RegisteredRoleId)
                    CacheUtilities.ClearRoleCache(-2)
                    result.Success = True
                    result.Response = ""
                Else
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_SET_USER_DELETED"
                End If
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetUserRestored(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)

            If Not user Is Nothing Then
                If DotNetNuke.Entities.Users.UserController.RestoreUser(user) Then
                    CacheUtilities.ClearRoleCache(PortalSettings.RegisteredRoleId)
                    CacheUtilities.ClearRoleCache(-2)
                    result.Success = True
                    result.Response = ""
                Else
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_RESTORE_USER"
                End If
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

        <ConnectUsersAuthorize(SecurityAccessLevel.ViewModule)>
        <HttpPost>
        Public Function SetUserRemoved(postData As ActionRequestDTO) As HttpResponseMessage

            Dim result As New ActionResultDTO
            Dim user As UserInfo = UserController.GetUserById(postData.PortalId, postData.UserId)

            If Not user Is Nothing Then
                If DotNetNuke.Entities.Users.UserController.RemoveUser(user) Then
                    CacheUtilities.ClearRoleCache(PortalSettings.RegisteredRoleId)
                    CacheUtilities.ClearRoleCache(-2)
                    result.Success = True
                    result.Response = ""
                Else
                    result.Success = False
                    result.Response = "ERROR_COULD_NOT_REMOVE_USER"
                End If
            Else
                result.Success = False
                result.Response = "ERROR_USER_NOT_FOUND"
            End If

            Dim strJson As Object = Serialize(Of ActionResultDTO)(result)
            Return Request.CreateResponse(HttpStatusCode.OK, strJson)

        End Function

#End Region

#Region "Private Methods"

        Private Sub SetUser(data As UserDTO)

        End Sub

        '=======================================================
        'Service provided by Telerik (www.telerik.com)
        'Conversion powered by NRefactory.
        'Twitter: @telerik
        'Facebook: facebook.com/telerik
        '=======================================================


        Private Sub SetRole(data As RoleDTO)
            Dim rc As New RoleController
            _Role = rc.GetRoleById(data.RoleId, PortalSettings.PortalId)
        End Sub

        Private Function SortUsers(Users As List(Of UserDTO), Pattern As String) As List(Of UserDTO)

            Select Case Pattern.ToLower
                Case "userid-asc"
                    Users.Sort(Function(x, y) (x.UserId < y.UserId))
                Case "userid-desc"
                    Users.Sort(Function(x, y) (x.UserId > y.UserId))
                Case "username-asc"
                    Users.Sort(Function(x, y) (x.UserName < y.UserName))
                Case "username-desc"
                    Users.Sort(Function(x, y) (x.UserName > y.UserName))
                Case "firstname-asc"
                    Users.Sort(Function(x, y) (x.FirstName < y.FirstName))
                Case "firstname-desc"
                    Users.Sort(Function(x, y) (x.FirstName > y.FirstName))
                Case "lastname-asc"
                    Users.Sort(Function(x, y) (x.LastName < y.LastName))
                Case "lastname-desc"
                    Users.Sort(Function(x, y) (x.LastName > y.LastName))
                Case "displayname-asc"
                    Users.Sort(Function(x, y) (x.DisplayName < y.DisplayName))
                Case "displayname-desc"
                    Users.Sort(Function(x, y) (x.DisplayName > y.DisplayName))
                Case "createddate-asc"
                    Users.Sort(Function(x, y) (x.CreatedDate < y.CreatedDate))
                Case "createddate-desc"
                    Users.Sort(Function(x, y) (x.CreatedDate > y.CreatedDate))
                Case "email-asc"
                    Users.Sort(Function(x, y) (x.Email < y.Email))
                Case "email-desc"
                    Users.Sort(Function(x, y) (x.Email > y.Email))
                Case "postalcode-asc"
                    Users.Sort(Function(x, y) (x.PostalCode < y.PostalCode))
                Case "postalcode-desc"
                    Users.Sort(Function(x, y) (x.PostalCode > y.PostalCode))
                Case "country-asc"
                    Users.Sort(Function(x, y) (x.Country < y.Country))
                Case "country-desc"
                    Users.Sort(Function(x, y) (x.Country > y.Country))
                Case "city-asc"
                    Users.Sort(Function(x, y) (x.City < y.City))
                Case "city-desc"
                    Users.Sort(Function(x, y) (x.City > y.City))
                Case "telephone-asc"
                    Users.Sort(Function(x, y) (x.Telephone < y.Telephone))
                Case "telephone-desc"
                    Users.Sort(Function(x, y) (x.Telephone > y.Telephone))
                Case "cell-asc"
                    Users.Sort(Function(x, y) (x.Cell < y.Cell))
                Case "cell-desc"
                    Users.Sort(Function(x, y) (x.Cell > y.Cell))
                Case "preferredlocale-asc"
                    Users.Sort(Function(x, y) (x.PreferredLocale < y.PreferredLocale))
                Case "preferredlocale-desc"
                    Users.Sort(Function(x, y) (x.PreferredLocale > y.PreferredLocale))
                Case "status-asc"
                    Users.Sort(Function(x, y) (x.Status < y.Status))
                Case "status-desc"
                    Users.Sort(Function(x, y) (x.Status > y.Status))
            End Select

            Return Users

        End Function

        Private Function GetUsers(RoleId As Integer, PortalId As Integer, SearchPattern As String, SearchColumns As String) As IEnumerable(Of UserDTO)

            Dim blnTryFromCache As Boolean = True
            Dim lstUsers As New List(Of UserDTO)
            Dim keyCache As String = String.Format("DNNCONNECT_USERS_{0}", RoleId.ToString)

            If Not String.IsNullOrEmpty(SearchPattern) AndAlso Not String.IsNullOrEmpty(SearchColumns) Then
                blnTryFromCache = False
            End If

            If blnTryFromCache Then
                If Not DataCache.GetCache(keyCache) Is Nothing Then
                    Try
                        lstUsers = CType(DataCache.GetCache(keyCache), List(Of UserDTO))
                    Catch
                    End Try
                End If
            End If

            If lstUsers Is Nothing OrElse lstUsers.Count = 0 Then
                lstUsers = CBO.FillCollection(Of UserDTO)(CType(DotNetNuke.Data.DataProvider.Instance().ExecuteReader("Connect_Accounts_GetUsers", RoleId, PortalId, DotNetNuke.Data.DataProvider.Instance().GetNull(SearchPattern), SearchColumns), IDataReader))
                If lstUsers.Count > 0 AndAlso String.IsNullOrEmpty(SearchPattern) Then
                    DataCache.SetCache(keyCache, lstUsers)
                End If
            End If

            Return lstUsers

        End Function

#End Region

    End Class

End Namespace

