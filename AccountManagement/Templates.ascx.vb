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
Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Framework.JavaScriptLibraries



Partial Class Templates
    Inherits AccountManagementBase


    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init

        JavaScript.RequestRegistration(CommonJs.DnnPlugins)

    End Sub

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        LocalizeForm()

        If Not Page.IsPostBack Then

            BindThemes()
            BindLocales()

            SelectTheme(Settings.ModuleTheme)

            BindSelectedTheme()
            VerifyPasswordSettings()

        End If

    End Sub

    Private Sub drpThemes_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles drpThemes.SelectedIndexChanged
        BindSelectedTheme()
    End Sub

    Private Sub cmdUpdate_Click(sender As Object, e As System.EventArgs) Handles cmdUpdate.Click

        Dim blnSucess As Boolean = False

        SaveTemplates(blnSucess)

        If blnSucess Then
            UpdateSettings()
        End If

    End Sub

    Private Sub cmdUpdateExit_Click(sender As Object, e As System.EventArgs) Handles cmdUpdateExit.Click

        Dim blnSucess As Boolean = False

        SaveTemplates(blnSucess)

        If blnSucess Then
            UpdateSettings()
        End If

        Response.Redirect(NavigateURL())

    End Sub

    Private Sub cmdDeleteSelected_Click(sender As Object, e As System.EventArgs) Handles cmdDeleteSelected.Click
        Try
            DeleteTheme()
        Catch ex As Exception
            DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblDeleteThemeError", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect(NavigateURL())
    End Sub

    Private Sub drpLocales_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles drpLocales.SelectedIndexChanged
        BindSelectedTheme()
    End Sub

    Private Sub cmdCopySelected_Click(sender As Object, e As System.EventArgs) Handles cmdCopySelected.Click
        pnlTemplateName.Visible = True
    End Sub

    Private Sub VerifyPasswordSettings()

        If MembershipProvider.Instance().PasswordRetrievalEnabled = False Then

            Dim strNote As String = Localization.GetString("lblPasswordRetrievalDisabled", LocalResourceFile)
            If MembershipProvider.Instance().RequiresQuestionAndAnswer Then
                strNote += Localization.GetString("lblRequiresQuestionAndAnswer", LocalResourceFile)
            End If
            DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, strNote, ModuleMessage.ModuleMessageType.BlueInfo)

        End If

    End Sub

    Private Sub LocalizeForm()

        cmdCancel.Text = Localization.GetString("cmdCancel", LocalResourceFile)
        cmdUpdateExit.Text = Localization.GetString("cmdUpdateExit", LocalResourceFile)
        cmdUpdate.Text = Localization.GetString("cmdUpdate", LocalResourceFile)
        cmdCopySelected.Text = Localization.GetString("cmdCopySelected", LocalResourceFile)
        cmdDeleteSelected.Text = Localization.GetString("cmdDeleteSelected", LocalResourceFile)

    End Sub

    Private Sub BindThemes()

        drpThemes.Items.Clear()
        Dim basepath As String = Server.MapPath(Me.TemplateSourceDirectory & "/templates/")

        For Each folder As String In System.IO.Directory.GetDirectories(basepath)
            Dim foldername As String = folder.Substring(folder.LastIndexOf("\") + 1)

            drpThemes.Items.Add(New ListItem(foldername, folder))

        Next

    End Sub

    Private Sub SelectTheme(ThemeName As String)
        drpThemes.Items.FindByText(ThemeName).Selected = True
    End Sub

    Private Sub BindSelectedTheme()

        cmdDeleteSelected.Visible = (drpThemes.SelectedIndex <> 0)

        If Settings.ModuleTheme = drpThemes.SelectedItem.Text Then
            chkUseTheme.Checked = True
            DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDeleteSelected, Localization.GetSafeJSString(Localization.GetString("lblThemeInUse", LocalResourceFile)))
        Else
            chkUseTheme.Checked = False
            DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDeleteSelected, Localization.GetSafeJSString(Localization.GetString("lblConfirmDelete", LocalResourceFile)))
        End If

        Dim path As String = drpThemes.SelectedValue

        For Each file As String In System.IO.Directory.GetFiles(path)

            'If file.EndsWith(Constants.TemplateName_EmailToAdmin) Then
            '    txtEmailAdminTemplate.Text = GetTemplate(drpThemes.SelectedItem.Value, Constants.TemplateName_EmailToAdmin, drpLocales.SelectedValue, True)
            'End If
            'If file.EndsWith(Constants.TemplateName_EmailToUser_Private) Then
            '    txtEmailUserPrivate.Text = GetTemplate(drpThemes.SelectedItem.Value, Constants.TemplateName_EmailToUser_Private, drpLocales.SelectedValue, True)
            'End If
            'If file.EndsWith(Constants.TemplateName_EmailToUser_Verified) Then
            '    txtEmailUserVerified.Text = GetTemplate(drpThemes.SelectedItem.Value, Constants.TemplateName_EmailToUser_Verified, drpLocales.SelectedValue, True)
            'End If
            'If file.EndsWith(Constants.TemplateName_EmailToUser) Then
            '    txtEmailUser.Text = GetTemplate(drpThemes.SelectedItem.Value, Constants.TemplateName_EmailToUser, drpLocales.SelectedValue, True)
            'End If
            'If file.EndsWith(Constants.TemplateName_Form) Then
            '    txtFormTemplate.Text = GetTemplate(drpThemes.SelectedItem.Value, Constants.TemplateName_Form, drpLocales.SelectedValue, True)
            'End If

        Next


    End Sub

    Private Sub SaveTemplate(SelectedTheme As String, TemplateName As String, Locale As String)

        'Dim path As String = SelectedTheme & "\" & TemplateName.Replace(Constants.TemplateName_Extension, "." & Locale & Constants.TemplateName_Extension)

        'If (PortalSettings.DefaultLanguage.ToLower = Locale.ToLower) Or String.IsNullOrEmpty(Locale) Then
        '    path = SelectedTheme & "\" & TemplateName
        'End If

        'Dim sw As New System.IO.StreamWriter(path, False)

        ''If TemplateName = Constants.TemplateName_EmailToAdmin Then
        ''    sw.Write(txtEmailAdminTemplate.Text)
        ''End If
        ''If TemplateName = Constants.TemplateName_EmailToUser_Private Then
        ''    sw.Write(txtEmailUserPrivate.Text)
        ''End If
        ''If TemplateName = Constants.TemplateName_EmailToUser_Verified Then
        ''    sw.Write(txtEmailUserVerified.Text)
        ''End If
        ''If TemplateName = Constants.TemplateName_EmailToUser Then
        ''    sw.Write(txtEmailUser.Text)
        ''End If
        ''If TemplateName = Constants.TemplateName_Form Then
        ''    sw.Write(txtFormTemplate.Text)
        ''End If

        'sw.Close()
        'sw.Dispose()

    End Sub

    Private Sub SaveTemplates(ByRef blnSucess As Boolean)

        Dim basepath As String = drpThemes.SelectedValue

        If pnlTemplateName.Visible Then

            If String.IsNullOrEmpty(txtTemplateName.Text) Then
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblMustEnterTemplateName", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
                blnSucess = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtEmailAdminTemplate.Text) Or String.IsNullOrEmpty(txtEmailUser.Text) Or String.IsNullOrEmpty(txtEmailUserPrivate.Text) Or String.IsNullOrEmpty(txtEmailUserVerified.Text) Or String.IsNullOrEmpty(txtFormTemplate.Text) Then
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblMustEnterTemplate", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
                blnSucess = False
                Exit Sub
            End If

            Dim newpath As String = Server.MapPath(Me.TemplateSourceDirectory & "/templates/") & txtTemplateName.Text
            Try
                System.IO.Directory.CreateDirectory(newpath)
            Catch
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblInvalidFolderName", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
                blnSucess = False
                Exit Sub
            End Try

            Try
                For Each file As String In System.IO.Directory.GetFiles(basepath)
                    Dim destinationpath As String = newpath & "\" & file.Substring(file.LastIndexOf("\") + 1)
                    System.IO.File.Copy(file, destinationpath)
                Next
                basepath = newpath
            Catch ex As Exception
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblCouldNotCopyTheme", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
                blnSucess = False
                Exit Sub
            End Try

            pnlTemplateName.Visible = False
            BindThemes()
            SelectTheme(txtTemplateName.Text)
            cmdDeleteSelected.Visible = True

        End If

        Try

            For Each file As String In System.IO.Directory.GetFiles(basepath)

                'If file.EndsWith(Constants.TemplateName_EmailToAdmin) Then
                '    SaveTemplate(drpThemes.SelectedValue, Constants.TemplateName_EmailToAdmin, drpLocales.SelectedValue)
                'End If
                'If file.EndsWith(Constants.TemplateName_EmailToUser_Private) Then
                '    SaveTemplate(drpThemes.SelectedValue, Constants.TemplateName_EmailToUser_Private, drpLocales.SelectedValue)
                'End If
                'If file.EndsWith(Constants.TemplateName_EmailToUser_Verified) Then
                '    SaveTemplate(drpThemes.SelectedValue, Constants.TemplateName_EmailToUser_Verified, drpLocales.SelectedValue)
                'End If
                'If file.EndsWith(Constants.TemplateName_EmailToUser) Then
                '    SaveTemplate(drpThemes.SelectedValue, Constants.TemplateName_EmailToUser, drpLocales.SelectedValue)
                'End If
                'If file.EndsWith(Constants.TemplateName_Form) Then
                '    SaveTemplate(drpThemes.SelectedValue, Constants.TemplateName_Form, drpLocales.SelectedValue)
                'End If

            Next

        Catch ex As Exception
            DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblCouldNotWriteTheme", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
            blnSucess = False
            Exit Sub
        End Try


        blnSucess = True

    End Sub

    Private Sub UpdateSettings()

        Dim ctrl As New ModuleController
        ctrl.UpdateTabModuleSetting(TabModuleId, "ModuleTheme", drpThemes.SelectedItem.Text)

    End Sub

    Private Sub DeleteTheme()
        Dim basepath As String = drpThemes.SelectedValue
        For Each file As String In System.IO.Directory.GetFiles(basepath)
            System.IO.File.Delete(file)
        Next
        System.IO.Directory.Delete(basepath)
        BindThemes()
        UpdateSettings()
        BindSelectedTheme()

    End Sub

    Private Sub BindLocales()

        Dim dicLocales As Dictionary(Of String, DotNetNuke.Services.Localization.Locale) = LocaleController.Instance().GetLocales(PortalId)

        If dicLocales.Count > 1 Then
            pnlLocales.Visible = True
        End If

        For Each objLocale As DotNetNuke.Services.Localization.Locale In dicLocales.Values

            Dim item As New ListItem
            item.Text = objLocale.Text
            item.Value = objLocale.Code

            Me.drpLocales.Items.Add(item)

        Next

        Try
            drpLocales.Items(0).Selected = True
        Catch
        End Try


    End Sub

End Class



