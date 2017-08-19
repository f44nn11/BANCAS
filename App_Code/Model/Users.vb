Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class Users
    <JsonProperty("uid")> _
    Public Property UID() As String
        Get
            Return p_UID
        End Get
        Set(value As String)
            p_UID = value
        End Set
    End Property
    Private p_UID As String
    <JsonProperty("uname")> _
    Public Property UNAME() As String
        Get
            Return p_UNAME
        End Get
        Set(value As String)
            p_UNAME = value
        End Set
    End Property
    Private p_UNAME As String
    <JsonProperty("pass")> _
    Public Property PASS() As String
        Get
            Return p_PASS
        End Get
        Set(value As String)
            p_PASS = value
        End Set
    End Property
    Private p_PASS As String
    <JsonProperty("ugroupid")> _
    Public Property UGROUPID() As Integer
        Get
            Return p_UGROUPID
        End Get
        Set(value As Integer)
            p_UGROUPID = value
        End Set
    End Property
    Private p_UGROUPID As Integer
End Class
