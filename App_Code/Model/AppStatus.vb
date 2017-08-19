Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class AppStatus
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
    <JsonProperty("appregno")> _
    Public Property appregno() As String
        Get
            Return p_APPREGNO
        End Get
        Set(value As String)
            p_APPREGNO = value
        End Set
    End Property
    Private p_APPREGNO As String
    <JsonProperty("sessionuser")> _
    Public Property sessionuser() As String
        Get
            Return p_SESSIONUSER
        End Get
        Set(value As String)
            p_SESSIONUSER = value
        End Set
    End Property
    Private p_SESSIONUSER As String
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
    Public Property statuss() As Statuss()
        Get
            Return p_STATUSS
        End Get
        Set(value As Statuss())
            p_STATUSS = value
        End Set
    End Property
    Private p_STATUSS As Statuss()
End Class
Public Class Modul
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

End Class
Public Class Statuss
    <JsonProperty("status")> _
    Public Property status() As String
        Get
            Return p_STATUS
        End Get
        Set(value As String)
            p_STATUS = value
        End Set
    End Property
    Private p_STATUS As String
End Class

