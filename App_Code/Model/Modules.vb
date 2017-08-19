Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class Modules
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
    <JsonProperty("appregno")> _
    Public Property appregno() As Integer
        Get
            Return p_APPREGNO
        End Get
        Set(value As Integer)
            p_APPREGNO = value
        End Set
    End Property
    Private p_APPREGNO As Integer
    <JsonProperty("appdesc")> _
    Public Property appdesc() As String
        Get
            Return p_APPDESC
        End Get
        Set(value As String)
            p_APPDESC = value
        End Set
    End Property
    Private p_APPDESC As String
    <JsonProperty("actiontype")> _
    Public Property actiontype() As String
        Get
            Return p_ACTIONTYPE
        End Get
        Set(value As String)
            p_ACTIONTYPE = value
        End Set
    End Property
    Private p_ACTIONTYPE As String
End Class
