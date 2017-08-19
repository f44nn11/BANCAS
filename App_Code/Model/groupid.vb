Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class Groupid
    <JsonProperty("ugroupid")> _
    Public Property ugroupid() As Integer
        Get
            Return p_UGROUPID
        End Get
        Set(value As Integer)
            p_UGROUPID = value
        End Set
    End Property
    Private p_UGROUPID As Integer
    <JsonProperty("ugroupnm")> _
    Public Property ugroupnm() As String
        Get
            Return p_UGROUPNM
        End Get
        Set(value As String)
            p_UGROUPNM = value
        End Set
    End Property
    Private p_UGROUPNM As String

End Class
