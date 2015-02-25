Namespace Common

    Public Module Extensions

        <System.Runtime.CompilerServices.Extension()>
        Public Sub ReadValue(ByRef ValueTable As Hashtable, ValueName As String, ByRef Variable As Integer)
            If Not ValueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(ValueTable.Item(ValueName), Integer)
                Catch ex As Exception
                End Try
            End If
        End Sub

        <System.Runtime.CompilerServices.Extension()>
        Public Sub ReadValue(ByRef ValueTable As Hashtable, ValueName As String, ByRef Variable As String)
            If Not ValueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(ValueTable.Item(ValueName), String)
                Catch ex As Exception
                End Try
            End If
        End Sub

        <System.Runtime.CompilerServices.Extension()>
        Public Sub ReadValue(ByRef ValueTable As Hashtable, ValueName As String, ByRef Variable() As String)
            If Not ValueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(ValueTable.Item(ValueName), String).Split(Char.Parse(","))
                Catch ex As Exception
                End Try
            End If
        End Sub

        <System.Runtime.CompilerServices.Extension()>
        Public Sub ReadValue(ByRef ValueTable As Hashtable, ValueName As String, ByRef Variable As Boolean)
            If Not ValueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(ValueTable.Item(ValueName), Boolean)
                Catch ex As Exception
                End Try
            End If
        End Sub

        <System.Runtime.CompilerServices.Extension()>
        Public Sub ReadValue(ByRef ValueTable As Hashtable, ValueName As String, ByRef Variable As Date)
            If Not ValueTable.Item(ValueName) Is Nothing Then
                Try
                    Variable = CType(ValueTable.Item(ValueName), Date)
                Catch ex As Exception
                End Try
            End If
        End Sub

    End Module

End Namespace

