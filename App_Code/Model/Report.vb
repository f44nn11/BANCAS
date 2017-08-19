Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class Report
    <JsonProperty("param1")> _
    Public Property param1() As String
        Get
            Return p_param1
        End Get
        Set(value As String)
            p_param1 = value
        End Set
    End Property
    Private p_param1 As String
    <JsonProperty("param2")> _
    Public Property param2() As String
        Get
            Return p_param2
        End Get
        Set(value As String)
            p_param2 = value
        End Set
    End Property
    Private p_param2 As String
    <JsonProperty("param3")> _
    Public Property param3() As String
        Get
            Return p_param3
        End Get
        Set(value As String)
            p_param3 = value
        End Set
    End Property
    Private p_param3 As String
    <JsonProperty("param4")> _
    Public Property param4() As String
        Get
            Return p_param4
        End Get
        Set(value As String)
            p_param4 = value
        End Set
    End Property
    Private p_param4 As String
    <JsonProperty("param5")> _
    Public Property param5() As String
        Get
            Return p_param5
        End Get
        Set(value As String)
            p_param5 = value
        End Set
    End Property
    Private p_param5 As String
    <JsonProperty("param6")> _
    Public Property param6() As String
        Get
            Return p_param6
        End Get
        Set(value As String)
            p_param6 = value
        End Set
    End Property
    Private p_param6 As String
    <JsonProperty("param7")> _
    Public Property param7() As String
        Get
            Return p_param7
        End Get
        Set(value As String)
            p_param7 = value
        End Set
    End Property
    Private p_param7 As String
    <JsonProperty("param8")> _
    Public Property param8() As String
        Get
            Return p_param8
        End Get
        Set(value As String)
            p_param8 = value
        End Set
    End Property
    Private p_param8 As String
End Class
