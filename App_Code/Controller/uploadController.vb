Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities
Imports System.IO
Imports System.Threading.Tasks

Public Class uploadController
    Inherits ApiController
    Private sSQL As String
    Private Params() As ParamSQL
    Private Ar1 As ArrayList
    Private Artmp As DataTable
    Private dt As New DataTable
    Private dr As DataRow
    Private script As StringBuilder
    Private ResultTrans As Integer
    Private dToday As Date
    Private sError As String
    Private _session As String
    Private response As HttpResponseMessage
    Private result As String
    Private pStatus As String
    Private pMessage As String
    ' POST api/<controller>
    <ActionName("uploadfile")> _
    <HttpPost> _
    Public Function Post() As HttpResponseMessage
        'Dim files = data.Attachment
        'Dim param1 = data.param1

        Dim result As HttpResponseMessage = Nothing
        Dim httpRequest = HttpContext.Current.Request

        Dim iniparam1 = httpRequest.Form.GetValues("param1")(0)
        Dim iniparam2 = httpRequest.Form.GetValues("param2")(0)
        'If httpRequest.Files.AllKeys(0) = "file" Then
        If httpRequest.Files.Count > 0 Then
            Dim docfiles = New List(Of String)()
            For Each file As String In httpRequest.Files
                Dim postedFile = httpRequest.Files(file)
                'Dim filePath = HttpContext.Current.Server.MapPath("~/upload/" + postedFile.FileName)
                Dim filePath = "D:/DOWNLOAD/" + postedFile.FileName
                postedFile.SaveAs(filePath)

                docfiles.Add(filePath)
            Next


            result = Request.CreateResponse(HttpStatusCode.Created, docfiles)
        Else
            result = Request.CreateResponse(HttpStatusCode.BadRequest)
        End If
        'Else
        'result = Request.CreateResponse(HttpStatusCode.BadRequest)
        'End If
        Return result


        'pStatus = "2"
        'pMessage = "Actiontype not Allowed"
        ''pMessage = file.FileName
        'result = "{""uid"":""" + param1 + ""","
        'result = result + """message"":""" + pMessage + ""","
        'result = result + """status"":""" + pStatus + ""","
        'result = result + """createdt"":""" + DateString + """}"
        'response = New HttpResponseMessage(HttpStatusCode.OK)
        'response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")
        'Return response
    End Function



    '' GET api/<controller>
    'Public Function GetValues() As IEnumerable(Of String)
    '    Return New String() {"value1", "value2"}
    'End Function

    '' GET api/<controller>/5
    'Public Function GetValue(ByVal id As Integer) As String
    '    Return "value"
    'End Function

    '' POST api/<controller>
    'Public Sub PostValue(<FromBody()> ByVal value As String)

    'End Sub

    '' PUT api/<controller>/5
    'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    'End Sub

    '' DELETE api/<controller>/5
    'Public Sub DeleteValue(ByVal id As Integer)

    'End Sub
End Class
