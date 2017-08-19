Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities
Imports System.IO


Public Class SendReportController
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
    Private responses As HttpResponseMessage
    Private result1 As String
    Private dateTodaystr As String

    <ActionName("datareport")> _
    <HttpPost> _
    Public Function Post(report As Report) As HttpResponseMessage
        Dim param1 = report.param1
        Dim param2 = report.param2
        Dim param3 = report.param3
        Dim param4 = report.param4
        Dim param5 = report.param5
        Dim param6 = report.param6
        Dim param7 = report.param7
        Dim param8 = report.param8
        dateTodaystr = DateTime.Now.ToString("yyyyMMddHmmss")
        Dim ToEmailAdd As String
        Dim FrEmailAdd As String
        Dim sBody As String
        Dim sTargetURL As String
        Dim sProcess As String
        Dim path As String = "C://Download/Reportwebapi/"
        Dim str As String() = param1.Split("&")
        Dim filenm = str(0) + "_" + dateTodaystr
        If param8.ToLower = "excel" Then
            param8 = "xls"
        End If
        Dim filename As String = filenm + "." + param8
        sProcess = path + filename
        AppUtils.GetReportService(param1, path, filename, param8)


        'Dim strReportUser As String = "javasdev"
        'Dim strReportUserPW As String = "p@ssw0rd0908"
        'Dim strReportUserDomain As String = "JADEV"
        ''render format pdf,excel,csv

        'sTargetURL = "http://localhost/ReportServer?/ReportWeb/" + param1 + "&rs:Command=Render&rs:ClearSession=true&rc:Toolbar=false&rs:Format=" + param8
        'Dim req As HttpWebRequest = DirectCast(WebRequest.Create(sTargetURL), HttpWebRequest)
        'req.PreAuthenticate = True
        'req.Credentials = New System.Net.NetworkCredential(strReportUser, strReportUserPW, strReportUserDomain)

        'Dim HttpWResp As HttpWebResponse = DirectCast(req.GetResponse(), HttpWebResponse)
        'Dim fStream As Stream = HttpWResp.GetResponseStream()

        'Dim fileBytes As Byte() = ReadFully(fStream)
        ''Save to local


        'If Not Directory.Exists(path) Then
        '    Directory.CreateDirectory(path)
        'End If
        'sProcess = path + filename
        'File.WriteAllBytes(sProcess, fileBytes) '-- if the file exists on the server 
        'HttpWResp.Close()

        'sSQL = "usp_TblUser_webapp"
        'ReDim Params(4)
        'Params(0) = New ParamSQL("@pParam1", uname)
        'Params(1) = New ParamSQL("@pParam2", "")
        'Params(2) = New ParamSQL("@pParam3", "")
        'Params(3) = New ParamSQL("@pParam4", "")
        'Params(4) = New ParamSQL("@pOpt", "A")
        'Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
        'If Ar1.Count > 0 Then
        '    response = New HttpResponseMessage(HttpStatusCode.OK)
        '    Dim Status As String = "1"
        '    Dim pMessage As String = "login success"
        '    Dim pUid = Ar1(0).ToString
        '    Dim pUname = Ar1(1).ToString
        '    Dim pGroupid = Ar1(2).ToString
        '    Dim dateString1 = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
        '    result1 = "{""uid"":""" + pUid + ""","
        '    result1 = result1 + """uname"":""" + pUname + ""","
        '    result1 = result1 + """ugroupid"":""" + pGroupid + ""","
        '    result1 = result1 + """message"":""" + pMessage + ""","
        '    result1 = result1 + """status"":""" + Status + ""","
        '    result1 = result1 + """createdt"":""" + dateString1 + """}"

        '    response = New HttpResponseMessage(HttpStatusCode.OK)
        '    response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")
        'Else
        '    Dim Status As String = "2"
        '    Dim pMessage As String = "Username or password is incorrect"
        '    Dim pUid = ""
        '    Dim pUname = ""
        '    Dim pGroupid = ""
        '    Dim dateString1 = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
        '    result1 = "{""uid"":""" + pUid + ""","
        '    result1 = result1 + """uname"":""" + pUname + ""","
        '    result1 = result1 + """ugroupid"":""" + pGroupid + ""","
        '    result1 = result1 + """message"":""" + pMessage + ""","
        '    result1 = result1 + """status"":""" + Status + ""","
        '    result1 = result1 + """createdt"":""" + dateString1 + """}"

        '    response = New HttpResponseMessage(HttpStatusCode.NotAcceptable)
        '    response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")
        'End If
        'sProcess = "http://localhost/ReportServer?/ReportWeb/Report1&rs:Command=Render&rs:ClearSession=true&rc:Toolbar=false&rs:Format=PDF"
        SendEmail_Report(sProcess)

        Dim Status As String = "1"
        Dim pMessage As String = "Testing Report Via WebAPI"
        Dim pUid = filename
        Dim pUname = ""
        Dim pGroupid = ""
        dateTodaystr = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
        result1 = "{""uid"":""" + pUid + ""","
        result1 = result1 + """uname"":""" + pUname + ""","
        result1 = result1 + """ugroupid"":""" + pGroupid + ""","
        result1 = result1 + """message"":""" + pMessage + ""","
        result1 = result1 + """status"":""" + Status + ""","
        result1 = result1 + """createdt"":""" + dateTodaystr + """}"

        responses = New HttpResponseMessage(HttpStatusCode.OK)
        responses.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")


        Return responses

    End Function
    Public Shared Function ReadFully(input As Stream) As Byte()
        Using ms As New MemoryStream()
            input.CopyTo(ms)
            Return ms.ToArray()
        End Using
    End Function
    Private Sub SendEmail_Report(ByVal sProcess As String)
        Dim ToEmailAdd As String
        Dim FrEmailAdd As String
        Dim sBody As String

        Try
            FrEmailAdd = "noreplyhli@gmail.com"
            ToEmailAdd = "f44nn1178@gmail.com"

            'Email For Report
            sBody = ""
            sBody = "Dear Team, <br/><br/>"
            sBody = sBody + "Harap segera menyelesaikan permintaan data ataupun informasi data <br/><br/>"
            sBody = sBody + "<table width=100% border=0 cellspacing=0 cellpadding=0>"
            sBody = sBody + "<tr><td width=5%></td><td width=23%>No request<td width=2%>:</td><td width=75%>" + "testing.text" + "</td></tr>"
            sBody = sBody + "<tr><td></td><td>Sumber Data</td><td>:</td><td>" + "123456789" + "-" + "43333" + "</td></tr>"
            sBody = sBody + "<tr><td></td><td>Tanggal Request</td><td>:</td><td>" + "date" + "</td></tr>"
            sBody = sBody + "<tr><td></td><td>Jam Request</td><td>:</td><td>" + "date" + "</td></tr>"
            sBody = sBody + "<tr><td></td><td style=vertical-align:top>Request</td><td style=vertical-align:top>:</td><td style=vertical-align:top><em>" + "testing issue" + "</em></td></tr>"
            sBody = sBody + "</table><br/>"
            sBody = sBody + "<br/><br/><br/>" + "Regards," + "<br/>"
            AppUtils.SendMailHtmlModule(57, FrEmailAdd, ToEmailAdd, , "[TESTING : " + "REPORT" + "] " + "VIA WEBAPI", sBody, sProcess)

        Catch ex As Exception

        End Try
    End Sub

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
