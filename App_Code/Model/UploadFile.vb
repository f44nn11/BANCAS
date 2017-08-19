Imports Microsoft.VisualBasic
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports Newtonsoft.Json

Public Class UploadFile
    <JsonProperty("attachment")> _
    Public Property Attachment() As HttpPostedFileBase
        Get
            Return p_file
        End Get
        Set(value As HttpPostedFileBase)
            p_file = value
        End Set
    End Property
    Private p_file As HttpPostedFileBase
    <JsonProperty("param1")> _
    Public Property param1() As String
        Get
            Return p_PARAM1
        End Get
        Set(value As String)
            p_PARAM1 = value
        End Set
    End Property
    Private p_PARAM1 As String
End Class
