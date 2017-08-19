Imports System.Net
Imports System.Web.Http
Imports System.Runtime.Remoting.Services
Imports System.Net.Http
Imports System.Data
Imports Newtonsoft.Json
Imports System.Web.Providers.Entities

Public Class UsermanageController
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
    Private pStatus As String
    Private pMessage As String
    Private sTmpPass As String
    Private response As HttpResponseMessage
    Private result1 As String
    Private result As String
    <ActionName("datausersmanage")> _
    <HttpPost> _
    Public Function Post(usersmanage As UsersManage) As HttpResponseMessage
        Dim uid = usersmanage.uid
        Dim uname = usersmanage.uname
        Dim pass = usersmanage.pass
        Dim ugroupid = usersmanage.ugroupid
        Dim sessionuser = usersmanage.sessionuser
        Dim actiontype = usersmanage.actiontype
        Dim dateString1 = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")
        Dim EncryptPass = Utility.Encryption(pass)
        Try
            If (actiontype Is Nothing Or actiontype = "") Then
                pStatus = "2"
                pMessage = "actiontype is null"
                result1 = "{""uid"":""" + uid + ""","
                result1 = result1 + """message"":""" + pMessage + ""","
                result1 = result1 + """status"":""" + pStatus + ""","
                result1 = result1 + """createdt"":""" + dateString1 + """}"

                response = New HttpResponseMessage(HttpStatusCode.OK)
                response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")

                Return response
            End If
            

            If (actiontype.ToLower = "update") Then

                If (sessionuser Is Nothing Or sessionuser = "") Then
                    pStatus = "2"
                    pMessage = "sessionuser is null"
                    result1 = "{""uid"":""" + uid + ""","
                    result1 = result1 + """message"":""" + pMessage + ""","
                    result1 = result1 + """status"":""" + pStatus + ""","
                    result1 = result1 + """createdt"":""" + dateString1 + """}"

                    response = New HttpResponseMessage(HttpStatusCode.OK)
                    response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")

                    Return response
                End If



                sSQL = "usp_TblUser"
                ReDim Params(6)
                Params(0) = New ParamSQL("@pParam1", CInt(uid))
                Params(1) = New ParamSQL("@pParam2", uname)
                Params(2) = New ParamSQL("@pParam3", ugroupid)
                Params(3) = New ParamSQL("@pParam4", "")
                Params(4) = New ParamSQL("@pParam5", sessionuser)
                Params(5) = New ParamSQL("@pParam6", "")
                Params(6) = New ParamSQL("@pOpt", "3")
                ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                If ResultTrans > 0 Then
                    pStatus = "1"
                    pMessage = "Update Success"
                Else
                    pStatus = "2"
                    pMessage = "Update Failed"
                End If
            ElseIf (actiontype.ToLower = "checkname") Then
                sSQL = "usp_TblUser"
                ReDim Params(6)
                Params(0) = New ParamSQL("@pParam1", "")
                Params(1) = New ParamSQL("@pParam2", uname)
                Params(2) = New ParamSQL("@pParam3", "")
                Params(3) = New ParamSQL("@pParam4", "")
                Params(4) = New ParamSQL("@pParam5", "")
                Params(5) = New ParamSQL("@pParam6", "")
                Params(6) = New ParamSQL("@pOpt", "2")
                Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                If Ar1.Count > 0 Then
                    pStatus = "2"
                    pMessage = "Username already exist"
                Else
                    pStatus = "1"
                    pMessage = "Username valid"
                End If

            ElseIf (actiontype.ToLower = "addnewuser") Then
                sSQL = "usp_TblUserLastTransactionID"
                ReDim Params(2)
                Params(0) = New ParamSQL("@pID", 9)
                Params(1) = New ParamSQL("@pLAST_VALUE", 0)
                Params(2) = New ParamSQL("@pOpt", "0")
                Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                If Ar1.Count <= 0 Then
                    Throw New Exception("Client id can't generated ...")
                End If
                Params(0) = New ParamSQL("@pID", 9)
                Params(1) = New ParamSQL("@pLAST_VALUE", Ar1(0))
                Params(2) = New ParamSQL("@pOpt", "1")
                ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                If ResultTrans <= 0 Then
                    Throw New Exception("Client id can't generated ...")
                End If

                sSQL = "usp_TblUser"
                ReDim Params(6)
                Params(0) = New ParamSQL("@pParam1", Ar1(0))
                Params(1) = New ParamSQL("@pParam2", uname)
                Params(2) = New ParamSQL("@pParam3", ugroupid)
                Params(3) = New ParamSQL("@pParam4", EncryptPass)
                Params(4) = New ParamSQL("@pParam5", sessionuser)
                Params(5) = New ParamSQL("@pParam6", "")
                Params(6) = New ParamSQL("@pOpt", "1") 'Insert Addnew User
                ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                If ResultTrans > 0 Then
                    pStatus = "1"
                    pMessage = "Add New User Success"
                Else
                    pStatus = "2"
                    pMessage = "Add New User Failed"
                End If
            ElseIf (actiontype.ToLower = "copyuser") Then
                sSQL = "usp_TblUserToAnotheruser"
                ReDim Params(3)
                Params(0) = New ParamSQL("@pUIdSrc", uname)
                Params(1) = New ParamSQL("@pUIdDest", uid)
                Params(2) = New ParamSQL("@pCreateBy", sessionuser)
                Params(3) = New ParamSQL("@pOpt", "1") 'Copy Module User
                ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                If ResultTrans > 0 Then
                    pStatus = "1"
                    pMessage = "Copy Module User Success"
                Else
                    pStatus = "2"
                    pMessage = "Copy Module Failed"
                End If
            ElseIf (actiontype.ToLower = "appenduser") Then

                sSQL = "usp_TblUserToAnotheruser"
                ReDim Params(3)
                Params(0) = New ParamSQL("@pUIdSrc", uname)
                Params(1) = New ParamSQL("@pUIdDest", uid)
                Params(2) = New ParamSQL("@pCreateBy", sessionuser)
                Params(3) = New ParamSQL("@pOPT", "2") 'Append Module User
                ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
                If ResultTrans > 0 Then
                    pStatus = "1"
                    pMessage = "Append Module User Success"
                Else
                    pStatus = "2"
                    pMessage = "Append Module Failed"
                End If
            Else
                pStatus = "2"
                pMessage = "Actiontype not Allowed"
            End If


            Dim pUid = uid
            Dim pUname = uname
            Dim pGroupid = ugroupid
            result1 = "{""uid"":""" + pUid + ""","
            result1 = result1 + """message"":""" + pMessage + ""","
            result1 = result1 + """status"":""" + pStatus + ""","
            result1 = result1 + """createdt"":""" + dateString1 + """}"

            response = New HttpResponseMessage(HttpStatusCode.OK)
            response.Content = New StringContent(result1, System.Text.Encoding.UTF8, "application/json")

            Return response
        Catch ex As Exception
            pStatus = "2"
            pMessage = "Error"
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
