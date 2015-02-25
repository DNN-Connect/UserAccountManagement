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

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports System.Web.UI
Imports DotNetNuke.Entities.Profile

Namespace Connect.Libraries.UserManagement
    Public Class TemplateController

        Public Shared Function GetTemplate(ByVal strPath As String) As String


            If System.IO.File.Exists(strPath) Then
                Dim templ As String = ""
                Dim sr As New System.IO.StreamReader(strPath)
                templ = sr.ReadToEnd
                sr.Close()
                sr.Dispose()
                Return templ
            Else
                Return "Could not load template, sorry..."
            End If

        End Function

    End Class
End Namespace


