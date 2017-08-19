Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities

Public Class GroupidController
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
    Private result1 As String
    ' GET api/<controller>
    <ActionName("getdatagroupid")> _
    <HttpGet> _
    Public Function GetValues() As HttpResponseMessage
        sSQL = "usp_TblUser_Input"
        ReDim Params(1)
        Params(0) = New ParamSQL("@pNm", "")
        Params(1) = New ParamSQL("@pOpt", "B")
        dt = DBTrans.ReadRecordsToDataTable(sSQL, DBConn.Connection, True, Params)
        Dim response = New HttpResponseMessage(HttpStatusCode.OK)
        Dim datajson As String = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None)
        response.Content = New StringContent(datajson, System.Text.Encoding.UTF8, "application/json")

        Return response
    End Function

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
