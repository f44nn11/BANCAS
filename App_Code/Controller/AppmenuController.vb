Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities

Public Class AppmenuController
    Inherits ApiController
    Private sSQL As String
    Private sSQL1 As String
    Private sSQL2 As String
    Private Params() As ParamSQL
    Private Params2() As ParamSQL
    Private Ar1 As ArrayList
    Private Artmp As DataTable
    Private dt As New DataTable
    Private dr As DataRow
    Private script As StringBuilder
    Private ResultTrans As Integer
    Private dToday As Date
    Private sError As String
    Private _session As String
    Private sTmpPass As String
    Private response As HttpResponseMessage
    Private result1 As String
    <ActionName("getappmenu")> _
    <HttpPost> _
    Public Function Post(appmenu As Appmenu) As HttpResponseMessage
        Dim id = appmenu.ID
        Dim name = appmenu.NAME
        Dim url = appmenu.URL
        Dim parentid = appmenu.PARENTID

        sSQL = "usp_TblUser_appmenu"
        ReDim Params(4)
        Params(0) = New ParamSQL("@pParam1", id)
        Params(1) = New ParamSQL("@pParam2", "")
        Params(2) = New ParamSQL("@pParam3", "")
        Params(3) = New ParamSQL("@pParam4", "")
        Params(4) = New ParamSQL("@pOpt", "A")
        dt = DBTrans.ReadRecordsToDataTable(sSQL, DBConn.Connection, True, Params)
        Dim response = New HttpResponseMessage(HttpStatusCode.OK)
        Dim datajson As String = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None)
        response.Content = New StringContent(datajson, System.Text.Encoding.UTF8, "application/json")

        Return response

    End Function
End Class
