Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class UsersManage
    <JsonProperty("uid")> _
    Public Property uid() As String
        Get
            Return p_UID
        End Get
        Set(value As String)
            p_UID = value
        End Set
    End Property
    Private p_UID As String
    <JsonProperty("uname")> _
    Public Property uname() As String
        Get
            Return p_UNAME
        End Get
        Set(value As String)
            p_UNAME = value
        End Set
    End Property
    Private p_UNAME As String
    <JsonProperty("pass")> _
    Public Property pass() As String
        Get
            Return p_PASS
        End Get
        Set(value As String)
            p_PASS = value
        End Set
    End Property
    Private p_PASS As String
    <JsonProperty("ugroupid")> _
    Public Property ugroupid() As String
        Get
            Return p_UGROUPID
        End Get
        Set(value As String)
            p_UGROUPID = value
        End Set
    End Property
    Private p_UGROUPID As String
    <JsonProperty("sessionuser")> _
    Public Property sessionuser() As String
        Get
            Return p_sessionuser
        End Get
        Set(value As String)
            p_sessionuser = value
        End Set
    End Property
    Private p_sessionuser As String
    <JsonProperty("actiontype")> _
    Public Property actiontype() As String
        Get
            Return p_actiontype
        End Get
        Set(value As String)
            p_actiontype = value
        End Set
    End Property
    Private p_actiontype As String
End Class
