Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities

Public Class ModuleController
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
    <ActionName("getdatamodule")> _
    <HttpPost> _
    Public Function Post(modules As Modules) As HttpResponseMessage
        Dim uid = modules.uid
        Dim ugroupid = modules.ugroupid
        Dim actiontype = modules.actiontype

        If (actiontype = "moduleuser") Then
            sSQL = "usp_TblUser"
            ReDim Params(6)
            Params(0) = New ParamSQL("@pParam1", uid)
            Params(1) = New ParamSQL("@pParam2", "")
            Params(2) = New ParamSQL("@pParam3", "")
            Params(3) = New ParamSQL("@pParam4", "")
            Params(4) = New ParamSQL("@pParam5", "")
            Params(5) = New ParamSQL("@pParam6", "")
            Params(6) = New ParamSQL("@pOpt", "5")
            dt = DBTrans.ReadRecordsToDataTable(sSQL, DBConn.Connection, True, Params)
            response = New HttpResponseMessage(HttpStatusCode.OK)
            Dim datajson As String = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None)
            response.Content = New StringContent(datajson, System.Text.Encoding.UTF8, "application/json")
        ElseIf (actiontype = "modulenew") Then
            sSQL = "usp_TblUser"
            ReDim Params(6)
            Params(0) = New ParamSQL("@pParam1", uid)
            Params(1) = New ParamSQL("@pParam2", "")
            Params(2) = New ParamSQL("@pParam3", "")
            Params(3) = New ParamSQL("@pParam4", "")
            Params(4) = New ParamSQL("@pParam5", "")
            Params(5) = New ParamSQL("@pParam6", "")
            Params(6) = New ParamSQL("@pOpt", "9")
            dt = DBTrans.ReadRecordsToDataTable(sSQL, DBConn.Connection, True, Params)
            response = New HttpResponseMessage(HttpStatusCode.OK)
            Dim datajson As String = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None)
            response.Content = New StringContent(datajson, System.Text.Encoding.UTF8, "application/json")
        Else
            pStatus = "2"
            pMessage = "Actiontype not Allowed"
            result = "{""uid"":""" + uid + ""","
            result = result + """message"":""" + pMessage + ""","
            result = result + """status"":""" + pStatus + ""","
            result = result + """createdt"":""" + DateString + """}"
            response = New HttpResponseMessage(HttpStatusCode.OK)
            response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")
        End If

        

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
