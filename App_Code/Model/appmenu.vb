Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class Appmenu
    <JsonProperty("id")> _
    Public Property ID() As Integer
        Get
            Return p_ID
        End Get
        Set(value As Integer)
            p_ID = value
        End Set
    End Property
    Private p_ID As Integer
    <JsonProperty("name")> _
    Public Property NAME() As String
        Get
            Return p_NAME
        End Get
        Set(value As String)
            p_NAME = value
        End Set
    End Property
    Private p_NAME As String
    <JsonProperty("url")> _
    Public Property URL() As String
        Get
            Return p_URL
        End Get
        Set(value As String)
            p_URL = value
        End Set
    End Property
    Private p_URL As String
    <JsonProperty("parentid")> _
    Public Property PARENTID() As Integer
        Get
            Return p_PARENTID
        End Get
        Set(value As Integer)
            p_PARENTID = value
        End Set
    End Property
    Private p_PARENTID As Integer
End Class
