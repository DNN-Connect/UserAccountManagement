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
Imports System.Net
Imports System.IO
Imports DotNetNuke.Entities.Portals

Namespace Connect.Libraries.UserManagement
    Public Class ReCaptcha

        Public Shared Sub Validate(PrivateKey As String, ByRef IsValid As Boolean, ByRef Result As String)

            Dim strPrivateKey As String = PrivateKey
            Dim strRemoteIp As String = HttpContext.Current.Request.UserHostAddress
            Dim strChallenge As String = HttpContext.Current.Request.Form("recaptcha_challenge_field")
            Dim strResponse As String = HttpContext.Current.Request.Form("recaptcha_response_field")

            Dim postData As String = String.Format("privatekey={0}&remoteip={1}&challenge={2}&response={3}", strPrivateKey, strRemoteIp, strChallenge, strResponse)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)

            Dim strProtocoll As String = "http"
            If PortalSettings.Current.ActiveTab.IsSecure Then
                strProtocoll = "https"
            End If

            Dim request As WebRequest = WebRequest.Create(String.Format("{0}://www.google.com/recaptcha/api/verify", strProtocoll))
            request.Method = "POST"
            request.ContentLength = byteArray.Length
            request.ContentType = "application/x-www-form-urlencoded"
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim lstResult As New List(Of String)
            While reader.Peek >= 0
                lstResult.Add(reader.ReadLine)
            End While

            reader.Close()
            dataStream.Close()
            response.Close()

            If lstResult.Count > 0 Then
                Try
                    IsValid = Boolean.Parse(lstResult(0))
                Catch
                End Try
                Try
                    Result = lstResult(1)
                Catch
                End Try
            End If

        End Sub

    End Class
End Namespace


