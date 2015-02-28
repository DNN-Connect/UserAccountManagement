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

Partial Class View
    Inherits AccountManagementBase
    Implements IActionable

#Region "Private Members"

    Private _controltoload As String = ""

#End Region

#Region "Event Handlers"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        RegisterResources()

        _controltoload = Me.TemplateSourceDirectory & "/" & ModuleView.ToString() & ".ascx"

        LoadControlType()

    End Sub

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

#End Region

#Region "Private Methods"

    Private Sub RegisterResources()

        JavaScript.RequestRegistration(CommonJs.DnnPlugins)
        JavaScript.RequestRegistration(CommonJs.jQueryUI)

        If Settings.LoadResources Then

            Dim basepath As String = Me.TemplateSourceDirectory & "/Resources/"
            Dim resourcepath As String = ""

            'bootstrap
            resourcepath = basepath & "bootstrap/css/bootstrap.min.css"
            AddCss(resourcepath)
            resourcepath = ResolveClientUrl(basepath & "bootstrap/js/bootstrap.min.js")
            AddJs(resourcepath)

            'bs-switch
            resourcepath = basepath & "bootstrap-switch/css/bootstrap-switch.min.css"
            AddCss(resourcepath)
            resourcepath = ResolveClientUrl(basepath & "bootstrap-switch/js/bootstrap-switch.min.js")
            AddJs(resourcepath)

            'bootstrap-social
            resourcepath = basepath & "bootstrap-social/bootstrap-social.css"
            AddCss(resourcepath)

            'chosen
            resourcepath = basepath & "chosen/chosen.min.css"
            AddCss(resourcepath)
            resourcepath = ResolveClientUrl(basepath & "chosen/chosen.jquery.min.js")
            AddJs(resourcepath)

            'datatables
            resourcepath = basepath & "datatables/css/jquery.dataTables.min.css"
            AddCss(resourcepath)
            resourcepath = ResolveClientUrl(basepath & "datatables/js/jquery.dataTables.min.js")
            AddJs(resourcepath)

            'table-tools
            resourcepath = basepath & "datatables/extensions/tabletools/css/dataTables.tableTools.min.css"
            AddCss(resourcepath)
            resourcepath = ResolveClientUrl(basepath & "datatables/extensions/tabletools/js/dataTables.tableTools.min.js")
            AddJs(resourcepath)

            'responsive tables
            resourcepath = basepath & "datatables/extensions/responsive/css/dataTables.responsive.css"
            AddCss(resourcepath)

            'jstree
            resourcepath = basepath & "jstree/themes/default/style.min.css"
            AddCss(resourcepath)
            resourcepath = ResolveClientUrl(basepath & "jstree/jstree.min.js")
            AddJs(resourcepath)

            'sb-admin
            resourcepath = basepath & "sb-admin/css/sb-admin-2.css"
            AddCss(resourcepath)

            'font-awesome
            resourcepath = basepath & "font-awesome/css/font-awesome.min.css"
            AddCss(resourcepath)

            'connect custom
            resourcepath = ResolveClientUrl(basepath & "connect-accounts/connect-accounts.js")
            AddJs(resourcepath)

        End If

    End Sub

    Private Sub AddJs(ByVal jsPath As String)
        Dim key As String = "Connect" & jsPath.Substring(jsPath.LastIndexOf("/") + 1).Replace(".js", "").Replace(".min", "")
        Me.Page.ClientScript.RegisterClientScriptInclude(key, jsPath)
    End Sub

    Private Sub AddCss(ByVal cssPath As String)

        Dim cssFilename As String = cssPath.Substring(cssPath.LastIndexOf("/") + 1)

        Dim blnAlreadyRegistered As Boolean = False
        For Each ctrl As Control In Me.Page.Header.Controls

            If TypeOf (ctrl) Is HtmlLink Then

                Dim ctrlCss As HtmlLink = CType(ctrl, HtmlLink)

                If ctrlCss.Href.ToLower.EndsWith(cssFilename.ToLower) Then
                    blnAlreadyRegistered = True
                    Exit For
                End If

                If ctrlCss.Href.ToLower.EndsWith(cssFilename.ToLower.Replace(".min", "")) Then
                    blnAlreadyRegistered = True
                    Exit For
                End If

                If ctrlCss.Href.ToLower.Replace(".css", ".min.css").EndsWith(cssFilename.ToLower) Then
                    blnAlreadyRegistered = True
                    Exit For
                End If

            End If

        Next



        If Not blnAlreadyRegistered Then

            Dim ctrlLink As New HtmlLink
            ctrlLink.Href = cssPath
            ctrlLink.Attributes.Add("rel", "stylesheet")
            ctrlLink.Attributes.Add("type", "text/css")
            ctrlLink.Attributes.Add("media", "screen")

            Me.Page.Header.Controls.Add(ctrlLink)

        End If

    End Sub

    Private Sub LoadControlType()

        Dim objPortalModuleBase As AccountManagementBase = CType(Me.LoadControl(_controltoload), AccountManagementBase)
        objPortalModuleBase.ModuleConfiguration = Me.ModuleConfiguration
        objPortalModuleBase.ID = System.IO.Path.GetFileNameWithoutExtension(_controltoload)
        plhControls.Controls.Add(objPortalModuleBase)

    End Sub

#End Region



#Region "Optional Interfaces"

    Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
        Get
            Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
            Actions.Add(GetNextActionID, Localization.GetString("ManageTemplates.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ManageTemplates"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
            Actions.Add(GetNextActionID, Localization.GetString("Reports.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("Reports"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
            Return Actions
        End Get
    End Property

#End Region

End Class

