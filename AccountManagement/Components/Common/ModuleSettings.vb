Namespace Common
    Public Class ModuleSettings

#Region "Private Members"

        Private _ModuleId As Integer = Null.NullInteger
        Private _TabId As Integer = Null.NullInteger
        Private _UserId As Integer = Null.NullInteger
        Private _allSettings As Hashtable = Nothing
        Private _userSettings As Hashtable = Nothing
        Private _PersonalizationKey As String = "DNNCONNECT_USERMODULESETTINGS"
        Private _allrolesKey As String = "AllRoles"

#End Region

#Region "Properties "

        Public Property AllColumns As String() = New String() {"Photo", "UserId", "UserName", "FirstName", "LastName", "DisplayName", "CreatedDate", "Email", "PortalId", "PostalCode", "City", "Country", "Telephone", "Cell", "PreferredLocale", "Status", "RoleId"}
        Public Property UserColumns As String() = New String() {"Photo", "UserId", "UserName", "FirstName", "LastName", "CreatedDate"}
        Public Property SearchableColumns As String() = New String() {"UserId", "UserName", "FirstName", "LastName", "DisplayName", "CreatedDate", "Email", "PortalId", "PostalCode", "City", "Country", "Telephone", "Cell", "PreferredLocale", "Status", "RoleId"}
        Public Property PageSize As Integer = 25
        Public Property SortMode As String = "[ 1, 'desc' ]"
        Public Property AllowExport As Boolean = True
        Public Property AllowCreate As Boolean = True
        Public Property AllowReports As Boolean = True
        Public Property AllowDelete As Boolean = True
        Public Property AllowedRoles As String() = New String() {"all"}
        Public Property PreselectRole As Integer = GetPortalSettings.RegisteredRoleId
        Public Property UserTabs As String() = New String() {"Account", "Profile", "Password", "Email", "Roles", "Sites"}
        Public Property AllRoles As Boolean = True
        Public Property ModuleTheme As String = "default"
        Public Property LoadResources As Boolean = True

        Public ReadOnly Property ModuleId As Integer
            Get
                Return _ModuleId
            End Get
        End Property

        Public ReadOnly Property TabId As Integer
            Get
                Return _TabId
            End Get
        End Property

        Public ReadOnly Property UserId As Integer
            Get
                Return _UserId
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByVal ModuleId As Integer, TabId As Integer, UserId As Integer)

            _ModuleId = ModuleId
            _allSettings = (New DotNetNuke.Entities.Modules.ModuleController).GetModule(ModuleId, TabId, True).ModuleSettings

            _allSettings.ReadValue("AllColumns", Me.AllColumns)
            _allSettings.ReadValue("UserColumns", Me.UserColumns)
            _allSettings.ReadValue("SearchableColumns", Me.SearchableColumns)
            _allSettings.ReadValue("PageSize", Me.PageSize)
            _allSettings.ReadValue("DefaultSort", Me.SortMode)
            _allSettings.ReadValue("AllowExport", Me.AllowExport)
            _allSettings.ReadValue("AllowCreate", Me.AllowCreate)
            _allSettings.ReadValue("AllowReports", Me.AllowReports)
            _allSettings.ReadValue("AllowDelete", Me.AllowDelete)
            _allSettings.ReadValue("AllowedRoles", Me.AllowedRoles)
            _allSettings.ReadValue("PreselectRole", Me.PreselectRole)
            _allSettings.ReadValue("UserTabs", Me.UserTabs)
            _allSettings.ReadValue("AllRoles", Me.AllRoles)
            _allSettings.ReadValue("ModuleTheme", Me.ModuleTheme)

            If UserId <> Null.NullInteger Then

                If Not DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "SearchableColumns") Is Nothing Then
                    SearchableColumns = CType(DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "SearchableColumns"), String).Split(Char.Parse(","))
                End If

                If Not DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "UserColumns") Is Nothing Then
                    UserColumns = CType(DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "UserColumns"), String).Split(Char.Parse(","))
                End If

                If Not DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "PageSize") Is Nothing Then
                    PageSize = CType(DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "PageSize"), Integer)
                End If

                If Not DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "SortMode") Is Nothing Then
                    SortMode = CType(DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "SortMode"), String)
                End If

                If Not DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "PreselectRole") Is Nothing Then
                    PreselectRole = CType(DotNetNuke.Services.Personalization.Personalization.GetProfile(_PersonalizationKey, "PreselectRole"), String)
                End If

            End If

        End Sub

#End Region

#Region "Public Methods"

        Public Shared Function GetModuleSettings(ByVal ModuleId As Integer, ByVal TabId As Integer, ByVal UserId As Integer) As ModuleSettings
            Dim modSettings As ModuleSettings = Nothing
            Try
                modSettings = CType(DotNetNuke.Common.Utilities.DataCache.GetCache(CacheKey(ModuleId, TabId, UserId)), ModuleSettings)
            Catch
            End Try
            If modSettings Is Nothing Then
                modSettings = New ModuleSettings(ModuleId, TabId, UserId)
                DotNetNuke.Common.Utilities.DataCache.SetCache(CacheKey(ModuleId, TabId, UserId), modSettings)
            End If
            Return modSettings
        End Function

        Public Overridable Sub UpdateSettings()

            Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
            objModules.UpdateModuleSetting(_ModuleId, "AllColumns", AllColumns.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "UserColumns", UserColumns.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "SearchableColumns", SearchableColumns.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "PageSize", PageSize.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "DefaultSort", SortMode.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "AllowExport", AllowExport.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "AllowCreate", AllowCreate.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "AllowReports", AllowReports.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "AllowDelete", AllowDelete.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "PreselectRole", PreselectRole.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "UserTabs", UserTabs.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "AllRoles", AllRoles.ToString)
            objModules.UpdateModuleSetting(_ModuleId, "ModuleTheme", ModuleTheme)

        End Sub

        Public Sub PersonalizeSettings()

            DotNetNuke.Services.Personalization.Personalization.SetProfile(_PersonalizationKey, "SearchableColumns", SearchableColumns.ToString)
            DotNetNuke.Services.Personalization.Personalization.SetProfile(_PersonalizationKey, "UserColumns", UserColumns.ToString)
            DotNetNuke.Services.Personalization.Personalization.SetProfile(_PersonalizationKey, "PageSize", PageSize.ToString)
            DotNetNuke.Services.Personalization.Personalization.SetProfile(_PersonalizationKey, "SortMode", SortMode.ToString)
            DotNetNuke.Services.Personalization.Personalization.SetProfile(_PersonalizationKey, "PreselectRole", PreselectRole.ToString)

        End Sub

#End Region

#Region "Private Methods"

        Private Shared Function CacheKey(ByVal ModuleId As Integer, ByVal TabId As Integer, ByVal UserId As Integer) As String

            Return "ModuleSettings_" & ModuleId.ToString & "_" & TabId.ToString & "_" & UserId.ToString

        End Function

#End Region


    End Class
End Namespace

