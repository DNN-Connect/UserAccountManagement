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

Imports System.Collections.Specialized
Imports System.Linq
Imports System.Web.UI

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.WebControls

Namespace Connect.Libraries.UserManagement

    ''' <summary>
    ''' Provides a CheckboxList editor for the profile properties.
    ''' Works the same as a list control but will display the list as checkboxes
    ''' instead of a dropdown.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CheckboxListEditControl
        Inherits DNNListEditControl

        Private _columns As Integer = 3

        ''' <summary>
        ''' Number of columns used to display the list
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Columns As Integer
            Get
                Return _columns
            End Get
            Set(value As Integer)
                _columns = value
            End Set
        End Property

        Protected Overrides Sub RenderViewMode(ByVal writer As System.Web.UI.HtmlTextWriter)

            ControlStyle.AddAttributesToRender(writer)
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            writer.Write(MyBase.StringValue.Replace("|", "; "))
            writer.RenderEndTag()

        End Sub

        Protected Overrides Sub RenderEditMode(ByVal writer As System.Web.UI.HtmlTextWriter)
            'Render the Select Tag
            ControlStyle.AddAttributesToRender(writer)
            writer.AddAttribute(HtmlTextWriterAttribute.Name, Me.UniqueID)
            If AutoPostBack Then
                writer.AddAttribute(HtmlTextWriterAttribute.Onchange, page.GetPostBackEventReference(Me))
            End If

            writer.RenderBeginTag(HtmlTextWriterTag.Table)

            Dim itemIndex As Integer = 0
            Dim lastIndex As Integer = ListEntries.Count - 1
            Dim arrValues As String() = MyBase.StringValue.Split("|")

            While itemIndex <= lastIndex
                writer.RenderBeginTag(HtmlTextWriterTag.Tr)
                For columnIndex As Integer = 0 To Me.Columns - 1
                    If (itemIndex <= lastIndex) Then
                        Dim item As ListEntryInfo = List.Item(itemIndex)
                        Dim itemValue As String = GetItemValue(item)
                        Dim itemText As String = GetItemText(item)
                        Dim itemName As String = Me.UniqueID

                        writer.RenderBeginTag(HtmlTextWriterTag.Td)

                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox")
                        writer.AddAttribute(HtmlTextWriterAttribute.Value, itemValue)
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, itemName)

                        For Each val As String In arrValues
                            If itemValue = val Then
                                writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked")
                                Exit For
                            End If
                        Next
                        writer.RenderBeginTag(HtmlTextWriterTag.Input)
                        writer.Write(itemText)
                        writer.RenderEndTag()

                        writer.RenderEndTag()
                    End If
                    itemIndex = itemIndex + 1
                Next
                writer.RenderEndTag()

            End While

            'Close Table Tag
            writer.RenderEndTag()
        End Sub

        Private Function GetItemValue(ByVal item As ListEntryInfo) As String
            'Add the Value Attribute
            Select Case Me.ValueField
                Case ListBoundField.Id
                    Return item.EntryID.ToString
                Case ListBoundField.Text
                    Return item.Text
                Case ListBoundField.Value
                    Return item.Value
                Case Else
                    Return Null.NullString
            End Select

        End Function

        Private Function GetItemText(ByVal item As ListEntryInfo) As String
            'Add the Value Attribute
            Select Case Me.TextField
                Case ListBoundField.Id
                    Return item.EntryID.ToString
                Case ListBoundField.Text
                    Return item.Text
                Case ListBoundField.Value
                    Return item.Value
                Case Else
                    Return Null.NullString
            End Select

        End Function

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            MyBase.OnPreRender(e)
            If Not Page Is Nothing And Me.EditMode = PropertyEditorMode.Edit Then
                Me.Page.RegisterRequiresPostBack(Me)
            End If
        End Sub
    End Class

End Namespace



