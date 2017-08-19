Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities

Public Class UsersController
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


    Private Property Session(p1 As String) As String
        Get
            Return _session
        End Get
        Set(value As String)
            _session = value
        End Set
    End Property

    <ActionName("getdatausers")> _
    <HttpPost> _
    Public Function Post(users As Users) As HttpResponseMessage
        Dim uid = users.UID
        Dim uname = users.UNAME
        Dim pass = users.PASS

        sTmpPass = Utility.Encryption(pass)
        
        sSQL = "usp_TblUser_webapp"
        ReDim Params(4)
        Params(0) = New ParamSQL("@pParam1", uname)
        Params(1) = New ParamSQL("@pParam2", sTmpPass)
        Params(2) = New ParamSQL("@pParam3", "")
        Params(3) = New ParamSQL("@pParam4", "")
        Params(4) = New ParamSQL("@pOpt", "A")
        Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
        If Ar1.Count > 0 Then
            response = New HttpResponseMessage(HttpStatusCode.OK)
            Dim Status As String = "1"
            Dim pMessage As String = "login success"
            Dim pUid = Ar1(0).ToString
            Dim pUname = Ar1(1).ToString
            Dim pGroupid = Ar1(2).ToString
            Dim pGroupnm = Ar1(3).ToString
            Dim dateString1 = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
            result1 = "{""uid"":""" + pUid + ""","
            result1 = result1 + """uname"":""" + pUname + ""","
            result1 = result1 + """ugroupid"":""" + pGroupid + ""","
            result1 = result1 + """ugroupnm"":""" + pGroupnm + ""","
            result1 = result1 + """message"":""" + pMessage + ""","
            result1 = result1 + """status"":""" + Status + ""","
            result1 = result1 + """createdt"":""" + dateString1 + """}"

            response = New HttpResponseMessage(HttpStatusCode.OK)
            response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")
        Else
            Dim Status As String = "2"
            Dim pMessage As String = "Username or password is incorrect"
            Dim pUid = ""
            Dim pUname = ""
            Dim pGroupid = ""
            Dim pGroupnm = ""
            Dim dateString1 = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
            result1 = "{""uid"":""" + pUid + ""","
            result1 = result1 + """uname"":""" + pUname + ""","
            result1 = result1 + """ugroupid"":""" + pGroupid + ""","
            result1 = result1 + """ugroupnm"":""" + pGroupnm + ""","
            result1 = result1 + """message"":""" + pMessage + ""","
            result1 = result1 + """status"":""" + Status + ""","
            result1 = result1 + """createdt"":""" + dateString1 + """}"

            response = New HttpResponseMessage(HttpStatusCode.NotAcceptable)
            response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")
        End If

        Return response

    End Function
    ' GET api/<controller>
    <ActionName("getdatausers")> _
    <HttpGet> _
    Public Function GetValue() As HttpResponseMessage
        sSQL = "usp_TblUser_Input"
        ReDim Params(1)
        Params(0) = New ParamSQL("@pNm", "")
        Params(1) = New ParamSQL("@pOpt", "A")
        dt = DBTrans.ReadRecordsToDataTable(sSQL, DBConn.Connection, True, Params)
        Dim response = New HttpResponseMessage(HttpStatusCode.OK)
        Dim datajson As String = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Formatting.None)
        response.Content = New StringContent(datajson, System.Text.Encoding.UTF8, "application/json")

        Return response
    End Function

    '' GET api/<controller>
    'Public Function GetValues() As IEnumerable(Of String)
    '    Return New String() {"value1", "value2"}
    'End Function

    '' GET api/<controller>/5
    'Public Function GetValue(ByVal id As Integer) As String
    '    Return "value"
    'End Function

    ' POST api/<controller>
    'Public Sub PostValue(<FromBody()> ByVal value As String)

    'End Sub

    '' PUT api/<controller>/5
    'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    'End Sub

    '' DELETE api/<controller>/5
    'Public Sub DeleteValue(ByVal id As Integer)

    'End Sub
End Class
