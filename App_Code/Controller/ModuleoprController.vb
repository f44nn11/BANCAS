Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities

Public Class ModuleoprController
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
    Public Function Post(appstatus As AppStatus()) As HttpResponseMessage
        Try
            Dim dateString = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
            'Dim Data = Newtonsoft.Json.JsonConvert.SerializeObject(appstatus)
            Dim dataModules = appstatus
            For Each itemModule In dataModules
                If itemModule Is Nothing Then
                Else
                    Dim uid = itemModule.uid
                    Dim appregno = itemModule.appregno
                    Dim actiontype = itemModule.actiontype
                    Dim sessionuser = itemModule.sessionuser
                    Dim datastatus = itemModule.statuss


                    If (actiontype = "select") Then
                        sSQL = "usp_TblUser"
                        ReDim Params(6)
                        Params(0) = New ParamSQL("@pParam1", CInt(uid))
                        Params(1) = New ParamSQL("@pParam2", "")
                        Params(2) = New ParamSQL("@pParam3", "")
                        Params(3) = New ParamSQL("@pParam4", "")
                        Params(4) = New ParamSQL("@pParam5", "")
                        Params(5) = New ParamSQL("@pParam6", appregno)
                        Params(6) = New ParamSQL("@pOpt", "6")
                        dt = DBTrans.ReadRecordsToDataTable(sSQL, DBConn.Connection, True, Params)
                        response = New HttpResponseMessage(HttpStatusCode.OK)
                        Dim datajson As String = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None)
                        response.Content = New StringContent(datajson, System.Text.Encoding.UTF8, "application/json")
                    ElseIf (actiontype = "modulestatusupd") Then
                        Dim datastatuslenght = datastatus.Length
                        sSQL = "usp_TblUser"
                        ReDim Params(6)
                        Params(0) = New ParamSQL("@pParam1", CInt(uid))
                        Params(1) = New ParamSQL("@pParam2", "")
                        Params(2) = New ParamSQL("@pParam3", "")
                        Params(3) = New ParamSQL("@pParam4", "")
                        Params(4) = New ParamSQL("@pParam5", sessionuser)
                        Params(5) = New ParamSQL("@pParam6", appregno)
                        Params(6) = New ParamSQL("@pOpt", "7") 'Delete APPStatus User
                        ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                        'If ResultTrans > 0 Then
                        For Each itemStatus In datastatus
                            If itemStatus Is Nothing Then
                            Else
                                sSQL = "usp_TblUser"
                                ReDim Params(6)
                                Params(0) = New ParamSQL("@pParam1", CInt(uid))
                                Params(1) = New ParamSQL("@pParam2", "")
                                Params(2) = New ParamSQL("@pParam3", "")
                                Params(3) = New ParamSQL("@pParam4", itemStatus.status)
                                Params(4) = New ParamSQL("@pParam5", sessionuser)
                                Params(5) = New ParamSQL("@pParam6", appregno)
                                Params(6) = New ParamSQL("@pOpt", "8") 'Insert APPStatus User
                                ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                            End If
                        Next
                        pStatus = "1"
                        pMessage = "Update AppStatus Success"
                        result = "{""uid"":""" + uid + ""","
                        result = result + """message"":""" + pMessage + ""","
                        result = result + """status"":""" + pStatus + ""","
                        result = result + """createdt"":""" + dateString + """}"
                        response = New HttpResponseMessage(HttpStatusCode.OK)
                        response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")
                        'Else
                        'pStatus = "2"
                        'pMessage = "Delete AppStatus User Failed"
                        'result = "{""uid"":""" + uid + ""","
                        'result = result + """message"":""" + pMessage + ""","
                        'result = result + """status"":""" + pStatus + ""","
                        'result = result + """createdt"":""" + dateString + """}"
                        'response = New HttpResponseMessage(HttpStatusCode.OK)
                        'response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")
                        'End If
                    ElseIf (actiontype = "modulesinsert") Then
                        sSQL = "usp_TblUser"
                        ReDim Params(6)
                        Params(0) = New ParamSQL("@pParam1", CInt(uid))
                        Params(1) = New ParamSQL("@pParam2", "")
                        Params(2) = New ParamSQL("@pParam3", "")
                        Params(3) = New ParamSQL("@pParam4", "")
                        Params(4) = New ParamSQL("@pParam5", sessionuser)
                        Params(5) = New ParamSQL("@pParam6", appregno)
                        Params(6) = New ParamSQL("@pOpt", "10") 'Insert USERS_APP
                        ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                        If (ResultTrans > 0) Then
                            For Each itemStatus In datastatus
                                If itemStatus Is Nothing Then
                                Else
                                    sSQL = "usp_TblUser"
                                    ReDim Params(6)
                                    Params(0) = New ParamSQL("@pParam1", CInt(uid))
                                    Params(1) = New ParamSQL("@pParam2", "")
                                    Params(2) = New ParamSQL("@pParam3", "")
                                    Params(3) = New ParamSQL("@pParam4", itemStatus.status)
                                    Params(4) = New ParamSQL("@pParam5", sessionuser)
                                    Params(5) = New ParamSQL("@pParam6", appregno)
                                    Params(6) = New ParamSQL("@pOpt", "11") 'Insert USERS_APP_STATUS
                                    ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                                End If
                            Next
                        End If

                        pStatus = "1"
                        pMessage = "Insert AppStatus Success"
                        result = "{""uid"":""" + uid + ""","
                        result = result + """message"":""" + pMessage + ""","
                        result = result + """status"":""" + pStatus + ""","
                        result = result + """createdt"":""" + dateString + """}"
                        response = New HttpResponseMessage(HttpStatusCode.OK)
                        response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")

                    Else
                        pStatus = "2"
                        pMessage = "Actiontype not Allowed"
                        result = "{""uid"":""" + uid + ""","
                        result = result + """message"":""" + pMessage + ""","
                        result = result + """status"":""" + pStatus + ""","
                        result = result + """createdt"":""" + dateString + """}"
                        response = New HttpResponseMessage(HttpStatusCode.OK)
                        response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")
                    End If
                End If
            Next

            Return response
        Catch ex As Exception
            pStatus = "2"
            pMessage = "Actiontype not Allowed"
            result = "{""uid"":""" + "" + ""","
            result = result + """message"":""" + ex.Message + ""","
            result = result + """status"":""" + pStatus + ""","
            result = result + """createdt"":""" + DateString + """}"
            response = New HttpResponseMessage(HttpStatusCode.OK)
            response.Content = New StringContent(result, System.Text.Encoding.UTF8, "application/json")
            Return response
        End Try
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
