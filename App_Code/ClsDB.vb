Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Xml
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web.Mail
Imports System.Math
Imports System.Data.OleDb
Imports System.IO.Packaging
Imports ICSharpCode.SharpZipLib.Zip
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.Collections
Imports System.ComponentModel
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportAppServer.ClientDoc
Imports System.Security.Principal
Imports System.Net

Public Class Security
    Private Shared LOGON32_LOGON_INTERACTIVE As Integer = 9
    Private Shared LOGON32_PROVIDER_DEFAULT As Integer = 0

    Private Shared impersonationContext As WindowsImpersonationContext

    Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String, _
                            ByVal lpszDomain As String, _
                            ByVal lpszPassword As String, _
                            ByVal dwLogonType As Integer, _
                            ByVal dwLogonProvider As Integer, _
                            ByRef phToken As IntPtr) As Integer

    Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
                            ByVal ExistingTokenHandle As IntPtr, _
                            ByVal ImpersonationLevel As Integer, _
                            ByRef DuplicateTokenHandle As IntPtr) As Integer

    Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long

    Public Shared Function impersonateValidUser(ByVal userName As String, _
    ByVal domain As String, ByVal password As String) As Boolean

        Dim tempWindowsIdentity As WindowsIdentity
        Dim token As IntPtr = IntPtr.Zero
        Dim tokenDuplicate As IntPtr = IntPtr.Zero
        impersonateValidUser = False

        If RevertToSelf() Then
            If LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                    tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                    impersonationContext = tempWindowsIdentity.Impersonate()
                    If Not impersonationContext Is Nothing Then
                        impersonateValidUser = True
                    End If
                End If
            End If
        End If
        If Not tokenDuplicate.Equals(IntPtr.Zero) Then
            CloseHandle(tokenDuplicate)
        End If
        If Not token.Equals(IntPtr.Zero) Then
            CloseHandle(token)
        End If
    End Function

    Public Shared Sub undoImpersonation()
        impersonationContext.Undo()
    End Sub

    Public Shared Function HTMLEncodeSpecialChars(ByVal text As String) As String
        Dim sb As New System.Text.StringBuilder()
        Try
            For Each c As Char In text
                If Asc(c) > 127 Then
                    ' special chars
                    sb.Append([String].Format("&#{0};", AscW(c)))
                Else
                    sb.Append(c)
                End If
            Next
            Return sb.ToString()
        Catch
            Throw
        End Try
    End Function

    Public Shared Function EncryptString(ByVal strText As String) As String
        Dim strEncrKey = "&<:<@!?*/1n5"
        Dim byKey() As Byte = {}

        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Dim lLens As Long
            Dim lLen As Long
            Dim sToEnkrip As String
            Dim sData As String
            Dim sPart As String

            sToEnkrip = ""
            sData = Convert.ToBase64String(ms.ToArray())
            lLens = Len(Convert.ToBase64String(ms.ToArray()))
            For lLen = 1 To lLens
                sPart = CStr(Asc(Mid(sData, lLen, 1)))
                sToEnkrip = sToEnkrip & Len(sPart).ToString & sPart
            Next
            Return sToEnkrip
        Catch
            Throw
        End Try
    End Function

    'The function used to decrypt the text
    Public Shared Function DecryptString(ByVal strText As String) As String
        Dim sDecrKey As String = "&<:<@!?*/1n5"
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte
        Dim sToDekrip As String
        Dim lCol As Long
        Dim lLen As Long
        Dim lPart As Long
        Dim sDekrip As String


        Try
            sDekrip = ""
            sToDekrip = ""
            sToDekrip = strText
            lLen = Len(sToDekrip)
            lCol = 1
            Do While lCol < lLen
                lPart = CInt(Mid$(sToDekrip, lCol, 1))
                lCol = lCol + 1
                sDekrip = sDekrip & CStr(Chr((Mid$(sToDekrip, lCol, lPart))))
                lCol = lCol + lPart
            Loop

            strText = sDekrip
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch
            Throw
        End Try
    End Function

End Class

Public Class Utility

    Public Shared Function SkipHolidays(ByVal Ar1 As ArrayList, _
                                 ByVal strField As String, ByRef dtmTemp As Date, ByVal intIncrement As Integer) _
                                 As Date
        Try
            ' Skip weekend days, and holidays in the recordset referred to by rst.
            'Dim strCriteria As String
            Dim i As Integer
            Dim x As Integer
            ' Move up to the first Monday/last Friday if the first/last
            ' of the month was a weekend date. Then skip holidays.
            ' Repeat this entire process until you get to a weekday.
            ' Unless rst contains a row for every day in the year (!)
            ' this should finally converge on a weekday.
            i = 1
            Do While i < intIncrement
                If IsWeekend(dtmTemp) Then
                    'dtmTemp = dtmTemp + 1
                    'i = i + 1
                End If
                If Not IsNothing(Ar1) Then
                    For x = 0 To Ar1.Count - 1
                        If Convert.ToDateTime(Ar1(x)(0).ToString) = dtmTemp Then
                            'dtmTemp = dtmTemp + 1
                            dtmTemp = Convert.ToDateTime(dtmTemp).AddDays(1) 'DateAdd("d", 1, dtmTemp)
                            'i = i + 1
                            Exit For
                        End If
                        x = x + 1
                    Next x
                End If
                'dtmTemp = dtmTemp + 1
                dtmTemp = Convert.ToDateTime(dtmTemp).AddDays(1) ' DateAdd("d", 1, dtmTemp)
                If IsWeekend(dtmTemp) Then
                    'dtmTemp = dtmTemp + 1
                    'i = i + 1
                End If
                i = i + 1
            Loop

            SkipHolidays = dtmTemp

        Catch
            Throw
        End Try
    End Function

    Public Shared Function IsWeekend(ByRef dtmTemp As Date) As Boolean
        Try
            ' Trace tanggal berikutnya bukan di hari Sabtu dan Minggu
            ' To make sure the next date isn't Saturday or Sunday
            Select Case Weekday(dtmTemp)
                Case vbSaturday
                    'dtmTemp = dtmTemp + 2
                    dtmTemp = Convert.ToDateTime(dtmTemp).AddDays(2) 'DateAdd("d", 2, dtmTemp)
                    IsWeekend = True
                Case vbSunday
                    'dtmTemp = dtmTemp + 1
                    dtmTemp = Convert.ToDateTime(dtmTemp).AddDays(1) 'DateAdd("d", 1, dtmTemp)
                    IsWeekend = True
                Case Else
                    IsWeekend = False
            End Select
        Catch
            Throw
        End Try
    End Function

    Public Shared Function RoundEx(ByVal Angkanya As Double, ByVal Digitnya As Short) As Double
        Try
            Dim Misalnya As Double
            If Right(CStr(Angkanya), 1) = 5 Then
                Misalnya = Angkanya + 0.0000000001
            Else
                Misalnya = Angkanya
            End If

            Return Math.Round(Misalnya, Digitnya)
        Catch
            Throw
        End Try
    End Function
    Public Shared Function Encryption(ByVal sToEnkrip As String) As String
        Try
            Dim iLen As Integer
            Dim i As Integer
            Dim sEnkrip As String
            Dim sPart As String
            sEnkrip = ""
            iLen = Len(sToEnkrip)
            For i = 1 To iLen

                sPart = CStr(Asc(CChar(Mid(sToEnkrip, i, 1))) * 9 + 41 + i)
                sEnkrip = sEnkrip & Len(sPart).ToString & sPart
            Next i
            Return sEnkrip
        Catch
            Throw
        End Try
    End Function

    Public Shared Function Decryption(ByVal sToDekrip As String) As String
        Try
            Dim iLen As Integer
            Dim i As Integer
            Dim j As Integer
            iLen = Len(sToDekrip)
            Dim sDekrip As String
            Dim iPart As Integer

            i = 1
            j = 1
            sDekrip = ""
            Do While i < iLen
                iPart = CInt(Mid$(sToDekrip, i, 1))
                i = i + 1
                sDekrip = sDekrip & CStr(Chr((Mid$(sToDekrip, i, iPart) - 41 - j) / 9))
                i = i + iPart
                j = j + 1
            Loop
            Return sDekrip
        Catch
            Throw
        End Try
    End Function

    Public Shared Function ShowMessageBox(ByVal sError As String) As String
        Dim sb As StringBuilder
        sb = New StringBuilder("<SCRIPT>")
        sb = sb.Append("alert('")
        sb = sb.Append(sError.Replace("'", ""))
        sb = sb.Append("')")
        sb = sb.Append("</SCRIPT>")

        Return sb.ToString()
    End Function

    Public Shared Function ReplaceMessage(ByVal ErrMsg As String) As String
        Dim sTmpErrMsg As String
        sTmpErrMsg = ErrMsg.Replace(vbCrLf, " ")
        sTmpErrMsg = sTmpErrMsg.Replace("'", "")
        Return sTmpErrMsg
    End Function


    Public Shared Function Terbilang(ByVal Number As Double) As String
        Try
            Dim sAngka As String
            Dim sTerbilang As String
            Dim sTigaDigit As String

            sTerbilang = ""
            If Number <= 19 Then
                Select Case Number
                    Case 1 : sTerbilang = "Satu"
                    Case 2 : sTerbilang = "Dua"
                    Case 3 : sTerbilang = "Tiga"
                    Case 4 : sTerbilang = "Empat"
                    Case 5 : sTerbilang = "Lima"
                    Case 6 : sTerbilang = "Enam"
                    Case 7 : sTerbilang = "Tujuh"
                    Case 8 : sTerbilang = "Delapan"
                    Case 9 : sTerbilang = "Sembilan"
                    Case 10 : sTerbilang = "Sepuluh"
                    Case 11 : sTerbilang = "Sebelas"
                    Case 12 : sTerbilang = "Dua Belas"
                    Case 13 : sTerbilang = "Tiga Belas"
                    Case 14 : sTerbilang = "Empat Belas"
                    Case 15 : sTerbilang = "Lima Belas"
                    Case 16 : sTerbilang = "Enam Belas"
                    Case 17 : sTerbilang = "Tujuh Belas"
                    Case 18 : sTerbilang = "Delapan Belas"
                    Case 19 : sTerbilang = "Sembilan Belas"
                    Case 0 : sTerbilang = ""
                End Select
            ElseIf Number <= 99 Then
                sTerbilang = Terbilang(Left$(Number, 1)) & " Puluh " & Terbilang(Right$(Number, 1))
            ElseIf Number <= 999 Then
                If Left$(Number, 1) = "1" Then
                    sTerbilang = "Seratus " & Terbilang(Right$(Number, 2))
                Else
                    sTerbilang = Terbilang(Left$(Number, 1)) & " Ratus " & Terbilang(Right$(Number, 2))
                End If
            ElseIf Number <= 999999 Then
                sAngka = Format(Number, "0#####")
                If Left$(sAngka, 3) = "001" Then
                    sTerbilang = "Seribu " & Terbilang(Right$(sAngka, 3))
                Else
                    sTerbilang = Terbilang(Left$(sAngka, 3)) & " Ribu " & Terbilang(Right$(sAngka, 3))
                End If
            Else
                sAngka = Format(Number, "0##############")
                sTerbilang = Terbilang(Right$(sAngka, 6))

                sTigaDigit = Terbilang(Mid$(sAngka, 7, 3))
                If sTigaDigit <> "" Then sTerbilang = sTigaDigit & " Juta " & sTerbilang

                sTigaDigit = Terbilang(Mid$(sAngka, 4, 3))
                If sTigaDigit <> "" Then sTerbilang = sTigaDigit & " Miliar " & sTerbilang

                sTigaDigit = Terbilang(Left$(sAngka, 3))
                If sTigaDigit <> "" Then sTerbilang = sTigaDigit & " Triliun " & sTerbilang

            End If
            Return sTerbilang
        Catch
            Throw
        End Try
    End Function

    Public Shared Sub EnableAplicationField(ByVal Value As Boolean, ByVal FormName As Object)
        Try
            Dim oForm As HtmlForm
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList
            Dim oCommandBtn As Button
            Dim iRow As Integer
            '****"Form1" is the Id of the Form found in the HTML tab &lt;FORM language=javascript id=Form1
            oForm = CType(FormName, HtmlForm)

            For Each oCtl In oForm.Controls
                Select Case oCtl.GetType.ToString
                    Case "System.Web.UI.WebControls.TextBox"
                        oTextBoxCtl = CType(oCtl, TextBox)
                        oTextBoxCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.DropDownList"
                        oDropDownListCtl = CType(oCtl, DropDownList)
                        oDropDownListCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.CheckBox"
                        oCheckBoxCtl = CType(oCtl, CheckBox)
                        oCheckBoxCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.CheckBoxList"
                        oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                        oCheckBoxListCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.RadioButton"
                        oRadioButtonCtl = CType(oCtl, RadioButton)
                        oRadioButtonCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                        For iRow = 0 To oRadioButtonListCtl.Items.Count - 1
                            oRadioButtonListCtl.Items(iRow).Enabled = Value
                        Next
                    Case "System.Web.UI.WebControls.Button"
                        oCommandBtn = CType(oCtl, Button)
                        oCommandBtn.Enabled = Value
                    Case "System.Web.UI.WebControls.MultiView"
                        Dim oCtlView As System.Web.UI.WebControls.View
                        For Each oCtlView In oCtl.Controls
                            Select Case oCtlView.GetType.ToString
                                Case "System.Web.UI.WebControls.View"
                                    If oCtlView.Visible Then
                                        EnableViewAplicationField(Value, oCtlView)
                                    End If
                            End Select
                        Next
                End Select
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub EnableViewAplicationField(ByVal Value As Boolean, ByVal oView As System.Web.UI.WebControls.View)
        Try
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList
            Dim oCommandBtn As Button
            Dim oImageButton As ImageButton
            '****"Form1" is the Id of the Form found in the HTML tab &lt;FORM language=javascript id=Form1
            For Each oCtl In oView.Controls
                Select Case oCtl.GetType.ToString
                    Case "System.Web.UI.WebControls.TextBox"
                        oTextBoxCtl = CType(oCtl, TextBox)
                        If InStr(1, oTextBoxCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) = 0 Then
                            oTextBoxCtl.Enabled = Value
                        End If
                    Case "System.Web.UI.WebControls.DropDownList"
                        oDropDownListCtl = CType(oCtl, DropDownList)
                        oDropDownListCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.CheckBox"
                        oCheckBoxCtl = CType(oCtl, CheckBox)
                        oCheckBoxCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.CheckBoxList"
                        oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                        oCheckBoxListCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.RadioButton"
                        oRadioButtonCtl = CType(oCtl, RadioButton)
                        oRadioButtonCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                        For iRow = 0 To oRadioButtonListCtl.Items.Count - 1
                            oRadioButtonListCtl.Items(iRow).Enabled = Value
                        Next
                    Case "System.Web.UI.WebControls.Button"
                        oCommandBtn = CType(oCtl, Button)
                        If InStr(1, oCommandBtn.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                            oCommandBtn.Enabled = Value
                        End If
                    Case "System.Web.UI.WebControls.ImageButton"
                        oImageButton = CType(oCtl, ImageButton)
                        If InStr(1, oImageButton.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                            oImageButton.Enabled = Value
                        End If
                End Select
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub ClearAplicationField(ByVal FormName As Object)
        Try
            Dim oForm As HtmlForm
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList
            '****"Form1" is the Id of the Form found in the HTML tab &lt;FORM language=javascript id=Form1
            oForm = CType(FormName, HtmlForm)

            For Each oCtl In oForm.Controls
                Select Case oCtl.GetType.ToString
                    Case "System.Web.UI.WebControls.TextBox"
                        oTextBoxCtl = CType(oCtl, TextBox)
                        oTextBoxCtl.Text = String.Empty
                    Case "System.Web.UI.WebControls.DropDownList"
                        oDropDownListCtl = CType(oCtl, DropDownList)
                        oDropDownListCtl.ClearSelection()
                        oDropDownListCtl.SelectedIndex = -1
                    Case "System.Web.UI.WebControls.CheckBox"
                        oCheckBoxCtl = CType(oCtl, CheckBox)
                        oCheckBoxCtl.Checked = False
                    Case "System.Web.UI.WebControls.CheckBoxList"
                        oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                        oCheckBoxListCtl.Items.Clear()
                    Case "System.Web.UI.WebControls.RadioButton"
                        oRadioButtonCtl = CType(oCtl, RadioButton)
                        oRadioButtonCtl.Checked = False
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                        oRadioButtonListCtl.Items.Clear()
                    Case "System.Web.UI.WebControls.MultiView"
                        Dim oCtlView As System.Web.UI.WebControls.View
                        For Each oCtlView In oCtl.Controls
                            Select Case oCtlView.GetType.ToString
                                Case "System.Web.UI.WebControls.View"
                                    If oCtlView.Visible Then
                                        EnableViewAplicationField(String.Empty, oCtlView)
                                    End If
                            End Select
                        Next
                End Select
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub ClearViewAplicationField(ByVal oView As System.Web.UI.WebControls.View)
        Try
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList

            '****"Form1" is the Id of the Form found in the HTML tab &lt;FORM language=javascript id=Form1
            For Each oCtl In oView.Controls
                Select Case oCtl.GetType.ToString
                    Case "System.Web.UI.WebControls.TextBox"
                        If InStr(1, oCtl.ID.Trim, "_NC_") = 0 Then
                            oTextBoxCtl = CType(oCtl, TextBox)
                            oTextBoxCtl.Text = String.Empty
                        End If
                    Case "System.Web.UI.WebControls.DropDownList"
                        oDropDownListCtl = CType(oCtl, DropDownList)
                        oDropDownListCtl.ClearSelection()
                        oDropDownListCtl.SelectedIndex = -1
                    Case "System.Web.UI.WebControls.CheckBox"
                        oCheckBoxCtl = CType(oCtl, CheckBox)
                        If oCheckBoxCtl.ID = "" Then
                            oCheckBoxCtl.Checked = False
                        End If
                    Case "System.Web.UI.WebControls.CheckBoxList"
                        oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                        oCheckBoxListCtl.Items.Clear()
                    Case "System.Web.UI.WebControls.RadioButton"
                        oRadioButtonCtl = CType(oCtl, RadioButton)
                        oRadioButtonCtl.Checked = False
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                        oRadioButtonListCtl.Items.Clear()
                End Select
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Function UserAppStatus(ByVal status As String, ByVal UserId As Integer, ByVal AppRegNo As Integer) As Boolean
        Try
            Dim sSQL As String
            Dim Params() As ParamSQL
            Dim Ar1 As ArrayList

            UserAppStatus = False

            sSQL = "usp_TblUsersAppStatusFind_ByUIDAppRegNo"

            ReDim Params(1)

            Params(0) = New ParamSQL("@pUID", UserId)
            Params(1) = New ParamSQL("@pAPPREGNO", AppRegNo)

            Ar1 = DBTrans.ReadRecordsToArrayList(sSQL, DBConn.Connection, True, Params)
            If Ar1.Count > 0 Then

                For iRow As Integer = 0 To Ar1.Count - 1
                    If Ar1(iRow)(0) = status Then
                        UserAppStatus = True
                        Exit For
                    End If
                Next
            End If
        Catch
            Throw
        End Try
    End Function

    Public Shared Sub ReplaceGridSpace(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            Dim oText As TextBox
            Dim oLabel As Label
            Dim iCol As Integer

            For iCol = 0 To e.Row.Cells.Count - 1
                If e.Row.Cells(iCol).Controls.Count > 1 Then
                    Select Case e.Row.Cells(iCol).Controls(1).GetType.ToString
                        Case "System.Web.UI.WebControls.TextBox"
                            oText = DirectCast(e.Row.Cells(iCol).Controls(1), TextBox)
                            If oText.Text = "&nbsp;" Then
                                oText.Text = oText.Text.Replace("&nbsp;", "")
                            End If
                        Case "System.Web.UI.WebControls.Label"
                            oLabel = DirectCast(e.Row.Cells(iCol).Controls(1), Label)
                            If oLabel.Text = "&nbsp;" Then
                                oLabel.Text = oLabel.Text.Replace("&nbsp;", "")
                            End If
                    End Select
                Else
                    If e.Row.Cells(iCol).Text = "&nbsp;" Then
                        e.Row.Cells(iCol).Text = e.Row.Cells(iCol).Text.Replace("&nbsp;", "")
                    End If
                End If
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub DefaultState(ByVal UserId As Integer, ByVal AppRegNo As Integer, Optional ByVal State As Boolean = True, Optional ByVal FormName As Object = Nothing, Optional ByVal oView As System.Web.UI.WebControls.View = Nothing, Optional ByVal GridView As System.Web.UI.WebControls.GridView = Nothing)
        Try
            Dim oImageBtn As ImageButton
            Dim icol As Integer

            If Not IsNothing(oView) Then
                For Each oCtl In oView.Controls
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.ImageButton"
                            oImageBtn = CType(oCtl, ImageButton)
                            If Left(oImageBtn.ID, 6) = "btnAdd" Then
                                oImageBtn.Visible = State And UserAppStatus("ADD", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 7) = "btnEdit" Then
                                oImageBtn.Visible = State And UserAppStatus("EDIT", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 9) = "btnDelete" Then
                                oImageBtn.Visible = State And UserAppStatus("DELETE", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 8) = "btnPrint" Then
                                oImageBtn.Visible = State And UserAppStatus("PRINT", UserId, AppRegNo)
                            End If

                            If Left(oImageBtn.ID, 7) = "btnSave" Then
                                oImageBtn.Visible = Not State
                            End If
                            If Left(oImageBtn.ID, 7) = "btnUndo" Then
                                oImageBtn.Visible = Not State
                            End If
                            If Left(oImageBtn.ID, 7) = "btnFind" Then
                                oImageBtn.Visible = State
                            End If
                    End Select
                Next
            End If

            If Not IsNothing(FormName) Then
                Dim oForm As HtmlForm
                oForm = CType(FormName, HtmlForm)

                For Each oCtl In oForm.Controls
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.ImageButton"
                            oImageBtn = CType(oCtl, ImageButton)
                            If Left(oImageBtn.ID, 6) = "btnAdd" Then
                                oImageBtn.Visible = State And UserAppStatus("ADD", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 7) = "btnEdit" Then
                                oImageBtn.Visible = State And UserAppStatus("EDIT", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 9) = "btnDelete" Then
                                oImageBtn.Visible = State And UserAppStatus("DELETE", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 8) = "btnPrint" Then
                                oImageBtn.Visible = State And UserAppStatus("PRINT", UserId, AppRegNo)
                            End If
                            If Left(oImageBtn.ID, 7) = "btnSave" Then
                                oImageBtn.Visible = Not State
                            End If
                            If Left(oImageBtn.ID, 7) = "btnUndo" Then
                                oImageBtn.Visible = Not State
                            End If
                            If Left(oImageBtn.ID, 7) = "btnFind" Then
                                oImageBtn.Visible = State
                            End If
                    End Select
                Next
            End If

            If Not IsNothing(GridView) Then
                For Each row As GridViewRow In GridView.Rows
                    For icol = 0 To row.Cells.Count - 1
                        If row.Cells(icol).Controls.Count > 0 Then
                            If row.Cells(icol).Text = "" Then
                                Select Case row.Cells(icol).Controls(1).GetType.ToString
                                    Case "System.Web.UI.WebControls.ImageButton"
                                        If Not IsNothing(CType(row.Cells(icol).Controls(1), ImageButton)) Then
                                            oImageBtn = CType(row.Cells(icol).Controls(1), ImageButton)
                                            If Left(oImageBtn.ID, 7) = "btnEdit" Then
                                                oImageBtn.Visible = State And UserAppStatus("EDIT", UserId, AppRegNo)
                                                row.Cells(icol).Visible = State And UserAppStatus("EDIT", UserId, AppRegNo)
                                                GridView.HeaderRow.Cells(icol).Visible = State And UserAppStatus("EDIT", UserId, AppRegNo)
                                            End If
                                            If Left(oImageBtn.ID, 9) = "btnDelete" Then
                                                oImageBtn.Enabled = State And UserAppStatus("DELETE", UserId, AppRegNo)
                                                row.Cells(icol).Visible = State And UserAppStatus("DELETE", UserId, AppRegNo)
                                                GridView.HeaderRow.Cells(icol).Visible = State And UserAppStatus("DELETE", UserId, AppRegNo)
                                            End If
                                        End If
                                End Select
                            End If
                        End If
                    Next
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub DisabledMenuField(Optional ByVal param As System.Web.UI.WebControls.TreeNode = Nothing, Optional ByVal View As System.Web.UI.WebControls.View = Nothing, Optional ByVal oSiteMapNode As SiteMapNode = Nothing, Optional ByVal oTree As TreeView = Nothing)
        Try
            Dim xmnuName As String
            Dim sSQL As String
            Dim Ar1 As ArrayList
            Dim Params() As ParamSQL
            Dim oImageBtn As ImageButton
            Dim oLinkButton As LinkButton

            If Not IsNothing(param) Then
                If param.ChildNodes.Count = 0 Then
                    Exit Sub
                End If
                Dim obj As TreeNode
                For Each obj In param.ChildNodes
                    If TypeOf obj Is TreeNode Then
                        'select authorized menu for user log in
                        xmnuName = obj.Value
                        sSQL = "usp_tbl_AppObj_Find"
                        ReDim Params(0)
                        Params(0) = New ParamSQL("@pAPPREGNM", xmnuName)
                        Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                        If Ar1.Count > 0 Then
                            obj.SelectAction = TreeNodeSelectAction.None
                        End If
                    End If
                    DisabledMenuField(obj)
                Next obj
            End If

            'If Not IsNothing(param) Then
            '    If param.ChildNodes.Count = 0 Then
            '        param.Expanded = False
            '        Exit Sub
            '    End If
            '    Dim obj As TreeNode
            '    For Each obj In param.ChildNodes
            '        If TypeOf obj Is TreeNode Then
            '            'select authorized menu for user log in
            '            xmnuName = obj.Value
            '            sSQL = "usp_tbl_AppObj_Find"
            '            ReDim Params(0)
            '            Params(0) = New ParamSQL("@pAPPREGNM", xmnuName)
            '            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
            '            If Ar1.Count > 0 Then
            '                obj.SelectAction = TreeNodeSelectAction.None
            '            End If
            '        End If
            '        DisabledMenuField(obj)
            '    Next obj
            'End If

            'Dim oSiteMap As System.Web.SiteMapNode
            'If Not IsNothing(oSiteMapNode) Then
            '    Dim oTreeNode As System.Web.UI.WebControls.TreeNode
            '    Dim obj As SiteMapNode
            '    For Each obj In oSiteMapNode.ChildNodes
            '        If TypeOf obj Is SiteMapNode Then
            '            'select authorized menu for user log in
            '            xmnuName = obj.Description
            '            sSQL = "usp_tbl_AppObj_Find"
            '            ReDim Params(0)
            '            Params(0) = New ParamSQL("@pAPPREGNM", xmnuName)
            '            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
            '            If Ar1.Count > 0 Then

            '                'oTreeNode.Target = obj("target")
            '                'oTreeNode.SelectAction = TreeNodeSelectAction.None
            '            End If
            '        End If
            '        DisabledMenuField(, , obj)
            '    Next
            'End If

            If Not IsNothing(View) Then
                For Each oCtl In View.Controls
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.ImageButton"
                            oImageBtn = CType(oCtl, ImageButton)
                            xmnuName = oImageBtn.CommandName.Trim
                            sSQL = "usp_tbl_AppObj_Find"
                            ReDim Params(0)
                            Params(0) = New ParamSQL("@pAPPREGNM", xmnuName)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oImageBtn.Enabled = False
                            End If
                        Case "System.Web.UI.WebControls.LinkButton"
                            oLinkButton = CType(oCtl, LinkButton)
                            xmnuName = oLinkButton.CommandName.Trim
                            sSQL = "usp_tbl_AppObj_Find"
                            ReDim Params(0)
                            Params(0) = New ParamSQL("@pAPPREGNM", xmnuName)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oLinkButton.Enabled = False
                            End If
                    End Select
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub EnabledMenuField(ByVal UName As String, Optional ByVal param As System.Web.UI.WebControls.TreeNode = Nothing, Optional ByVal View As System.Web.UI.WebControls.View = Nothing)
        Try
            Dim xmnuName As String
            Dim sSQL As String
            Dim Ar1 As ArrayList
            Dim Params() As ParamSQL
            Dim oImageBtn As ImageButton
            Dim oLinkButton As LinkButton

            If Not IsNothing(param) Then
                If param.ChildNodes.Count = 0 Then
                    Exit Sub
                End If
                Dim obj As TreeNode
                For Each obj In param.ChildNodes
                    If TypeOf obj Is TreeNode Then
                        xmnuName = obj.Value
                        If xmnuName Like "*line*" _
                        Or xmnuName Like "*Exit" _
                        Or xmnuName Like "*Help*" _
                        Or xmnuName Like "*LogIn" _
                        Or xmnuName Like "*File" Then
                            'do nothing
                        Else
                            'select authorized menu for user log in
                            xmnuName = obj.Value
                            sSQL = "usp_AppUserAccess"
                            ReDim Params(1)
                            Params(0) = New ParamSQL("@pUNAME", UName)
                            Params(1) = New ParamSQL("@pAPPREGNM", xmnuName)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                obj.SelectAction = TreeNodeSelectAction.Select
                            End If
                        End If
                    End If
                    EnabledMenuField(UName, obj)
                Next obj
            End If

            If Not IsNothing(View) Then
                For Each oCtl In View.Controls
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.ImageButton"
                            oImageBtn = CType(oCtl, ImageButton)
                            xmnuName = oImageBtn.CommandName.Trim
                            sSQL = "usp_AppUserAccess"
                            ReDim Params(1)
                            Params(0) = New ParamSQL("@pUNAME", UName)
                            Params(1) = New ParamSQL("@pAPPREGNM", xmnuName)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oImageBtn.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.LinkButton"
                            oLinkButton = CType(oCtl, LinkButton)
                            xmnuName = oLinkButton.CommandName.Trim
                            sSQL = "usp_AppUserAccess"
                            ReDim Params(1)
                            Params(0) = New ParamSQL("@pUNAME", UName)
                            Params(1) = New ParamSQL("@pAPPREGNM", xmnuName)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oLinkButton.Enabled = True
                            End If
                    End Select
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub ClearPanelAplicationField(ByVal oPanel As System.Web.UI.WebControls.Panel)
        Try
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList

            '****"Form1" is the Id of the Form found in the HTML tab &lt;FORM language=javascript id=Form1
            For Each oCtl In oPanel.Controls
                Select Case oCtl.GetType.ToString
                    Case "System.Web.UI.WebControls.TextBox"
                        oTextBoxCtl = CType(oCtl, TextBox)
                        oTextBoxCtl.Text = String.Empty
                    Case "System.Web.UI.WebControls.DropDownList"
                        oDropDownListCtl = CType(oCtl, DropDownList)
                        oDropDownListCtl.ClearSelection()
                        oDropDownListCtl.SelectedIndex = -1
                    Case "System.Web.UI.WebControls.CheckBox"
                        oCheckBoxCtl = CType(oCtl, CheckBox)
                        If oCheckBoxCtl.ID = "" Then
                            oCheckBoxCtl.Checked = False
                        End If
                    Case "System.Web.UI.WebControls.CheckBoxList"
                        oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                        oCheckBoxListCtl.Items.Clear()
                    Case "System.Web.UI.WebControls.RadioButton"
                        oRadioButtonCtl = CType(oCtl, RadioButton)
                        oRadioButtonCtl.Checked = False
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                        oRadioButtonListCtl.Items.Clear()
                End Select
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub EnablePanelAplicationField(ByVal Value As Boolean, ByVal oPanel As System.Web.UI.WebControls.Panel)
        Try
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList
            Dim oCommandBtn As Button
            Dim oImageButton As ImageButton
            '****"Form1" is the Id of the Form found in the HTML tab &lt;FORM language=javascript id=Form1
            For Each oCtl In oPanel.Controls
                Select Case oCtl.GetType.ToString
                    Case "System.Web.UI.WebControls.TextBox"
                        oTextBoxCtl = CType(oCtl, TextBox)
                        If InStr(1, oTextBoxCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) = 0 Then
                            oTextBoxCtl.Enabled = Value
                        End If
                    Case "System.Web.UI.WebControls.DropDownList"
                        oDropDownListCtl = CType(oCtl, DropDownList)
                        oDropDownListCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.CheckBox"
                        oCheckBoxCtl = CType(oCtl, CheckBox)
                        oCheckBoxCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.CheckBoxList"
                        oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                        oCheckBoxListCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.RadioButton"
                        oRadioButtonCtl = CType(oCtl, RadioButton)
                        oRadioButtonCtl.Enabled = Value
                    Case "System.Web.UI.WebControls.RadioButtonList"
                        oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                        For iRow = 0 To oRadioButtonListCtl.Items.Count - 1
                            oRadioButtonListCtl.Items(iRow).Enabled = Value
                        Next
                    Case "System.Web.UI.WebControls.Button"
                        oCommandBtn = CType(oCtl, Button)
                        If InStr(1, oCommandBtn.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                            oCommandBtn.Enabled = Value
                        End If
                    Case "System.Web.UI.WebControls.ImageButton"
                        oImageButton = CType(oCtl, ImageButton)
                        If InStr(1, oImageButton.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                            oImageButton.Enabled = Value
                        End If
                End Select
            Next
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub EnabledViewMenuButton(ByVal UName As String, ByVal oView As System.Web.UI.WebControls.View, Optional ByVal oPanel As System.Web.UI.WebControls.Panel = Nothing)
        Try
            Dim oCtl As Control
            Dim oButtonCtl As Button
            Dim oImgButtonCtl As ImageButton
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim sSQL As String
            Dim Ar1 As ArrayList
            Dim Params() As ParamSQL
            Dim IRow As Integer
            'Dim IRows As Integer

            If IsNothing(oPanel) Then
                For Each oCtl In oView.Controls
                    sSQL = "usp_AppUserAccess"
                    ReDim Params(1)
                    Params(0) = New ParamSQL("@pUNAME", UName)
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.Button"
                            oButtonCtl = CType(oCtl, Button)
                            Params(1) = New ParamSQL("@pAPPREGNM", oButtonCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oButtonCtl.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.ImageButton"
                            oImgButtonCtl = CType(oCtl, ImageButton)
                            Params(1) = New ParamSQL("@pAPPREGNM", oImgButtonCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oImgButtonCtl.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.RadioButton"
                            oRadioButtonCtl = CType(oCtl, RadioButton)
                            Params(1) = New ParamSQL("@pAPPREGNM", oRadioButtonCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oRadioButtonCtl.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.RadioButtonList"
                            oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                            For IRow = 0 To oRadioButtonListCtl.Items.Count - 1
                                Params(1) = New ParamSQL("@pAPPREGNM", oRadioButtonListCtl.Items(IRow).Value) 'oRadioButtonListCtl.ID
                                Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                                If Ar1.Count > 0 Then
                                    'oRadioButtonListCtl.Enabled = True
                                    oRadioButtonListCtl.Items(IRow).Enabled = True
                                End If
                            Next
                    End Select
                Next
            Else
                For Each oCtl In oPanel.Controls
                    sSQL = "usp_AppUserAccess"
                    ReDim Params(1)
                    Params(0) = New ParamSQL("@pUNAME", UName)
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.Button"
                            oButtonCtl = CType(oCtl, Button)
                            Params(1) = New ParamSQL("@pAPPREGNM", oButtonCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oButtonCtl.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.ImageButton"
                            oImgButtonCtl = CType(oCtl, ImageButton)
                            Params(1) = New ParamSQL("@pAPPREGNM", oImgButtonCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oImgButtonCtl.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.RadioButton"
                            oRadioButtonCtl = CType(oCtl, RadioButton)
                            Params(1) = New ParamSQL("@pAPPREGNM", oRadioButtonCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oRadioButtonCtl.Enabled = True
                            End If
                        Case "System.Web.UI.WebControls.RadioButtonList"
                            oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                            Params(1) = New ParamSQL("@pAPPREGNM", oRadioButtonListCtl.ID)
                            Ar1 = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
                            If Ar1.Count > 0 Then
                                oRadioButtonListCtl.Enabled = True
                            End If
                    End Select
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub ReadOnlyAplicationFieldPanel(ByVal oView As System.Web.UI.WebControls.Panel)
        Try
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList
            If oView IsNot Nothing Then
                For Each oCtl In oView.Controls
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.TextBox"
                            oTextBoxCtl = CType(oCtl, TextBox)
                            If InStr(1, oTextBoxCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oTextBoxCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.CheckBox"
                            oCheckBoxCtl = CType(oCtl, CheckBox)
                            If InStr(1, oCheckBoxCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oCheckBoxCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.CheckBoxList"
                            oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                            If InStr(1, oCheckBoxListCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oCheckBoxListCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.RadioButton"
                            oRadioButtonCtl = CType(oCtl, RadioButton)
                            If InStr(1, oRadioButtonCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oRadioButtonCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.RadioButtonList"
                            oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                            For iRow = 0 To oRadioButtonListCtl.Items.Count - 1
                                If InStr(1, oRadioButtonListCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                    oRadioButtonListCtl.Items(iRow).Attributes.Add("readonly", "readonly")
                                End If
                            Next
                        Case "System.Web.UI.WebControls.DropDownList"
                            oDropDownListCtl = CType(oCtl, DropDownList)
                            If InStr(1, oDropDownListCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oDropDownListCtl.Attributes.Add("readonly", "readonly")
                            End If
                    End Select
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub ReadOnlyAplicationField(ByVal oView As System.Web.UI.WebControls.View)
        Try
            Dim oCtl As Control
            Dim oTextBoxCtl As TextBox
            Dim oCheckBoxCtl As CheckBox
            Dim oCheckBoxListCtl As CheckBoxList
            Dim oRadioButtonCtl As RadioButton
            Dim oRadioButtonListCtl As RadioButtonList
            Dim oDropDownListCtl As DropDownList

            If oView IsNot Nothing Then
                For Each oCtl In oView.Controls
                    Select Case oCtl.GetType.ToString
                        Case "System.Web.UI.WebControls.TextBox"
                            oTextBoxCtl = CType(oCtl, TextBox)
                            If InStr(1, oTextBoxCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oTextBoxCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.CheckBox"
                            oCheckBoxCtl = CType(oCtl, CheckBox)
                            If InStr(1, oCheckBoxCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oCheckBoxCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.CheckBoxList"
                            oCheckBoxListCtl = CType(oCtl, CheckBoxList)
                            If InStr(1, oCheckBoxListCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oCheckBoxListCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.RadioButton"
                            oRadioButtonCtl = CType(oCtl, RadioButton)
                            If InStr(1, oRadioButtonCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oRadioButtonCtl.Attributes.Add("readonly", "readonly")
                            End If
                        Case "System.Web.UI.WebControls.RadioButtonList"
                            oRadioButtonListCtl = CType(oCtl, RadioButtonList)
                            For iRow = 0 To oRadioButtonListCtl.Items.Count - 1
                                If InStr(1, oRadioButtonListCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                    oRadioButtonListCtl.Items(iRow).Attributes.Add("readonly", "readonly")
                                End If
                            Next
                        Case "System.Web.UI.WebControls.DropDownList"
                            oDropDownListCtl = CType(oCtl, DropDownList)
                            If InStr(1, oDropDownListCtl.ID.ToString.Trim, "_R_", CompareMethod.Text) > 0 Then
                                oDropDownListCtl.Attributes.Add("readonly", "readonly")
                            End If
                    End Select
                Next
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Function NumericField(ByVal sValue As String, Optional ByVal bOrgFormat As Boolean = False) As String
        Try
            If sValue = "" Then
                sValue = "0"
            End If
            If Not bOrgFormat Then
                NumericField = String.Format("{0:n}", Convert.ToDecimal(sValue))
            Else
                NumericField = Convert.ToDouble(sValue)
            End If
        Catch
            Throw
        End Try
    End Function

    Public Shared Function DateField(ByVal sValue As String) As String
        DateField = Format(FormatDateTime(sValue.ToString, DateFormat.ShortDate), "{0:MM/dd/yyyy}") '{0:MM/dd/yyyy}
    End Function
End Class
Public Class DBTrans
    Private Const ERR_MSG As String = "DB Error"

#Region " Reading "
    Public Shared Function ReadSingleValue(ByVal SQLText As String, ByVal ConString As String, Optional ByVal IsStoredProcedure As Boolean = False, Optional ByVal Params() As ParamSQL = Nothing) As Object
        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim oData As Object = Nothing
        Dim JumlahParameter As Short
        Dim pParam As SqlParameter

        If IsNothing(Params) Then
            JumlahParameter = -1
        Else
            JumlahParameter = Params.GetUpperBound(0)
        End If

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cmSQL = New SqlCommand
            cmSQL.Connection = cnSQL
            cmSQL.CommandText = SQLText

            If IsStoredProcedure Then
                cmSQL.CommandType = CommandType.StoredProcedure
            End If

            cnSQL.Open()

            Dim i As Short
            For i = 0 To JumlahParameter
                pParam = New SqlParameter(Params(i).ParameterName, Params(i).ParameterValue)
                pParam.Direction = ParameterDirection.Input
                cmSQL.Parameters.Add(pParam)
            Next

            oData = cmSQL.ExecuteScalar
            cnSQL.Close()
        Catch
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        Finally
        End Try
        Return oData
    End Function
    Public Shared Function ReadSingleRecord(ByVal SQLText As String, ByVal ConString As String, Optional ByVal IsStoredProcedure As Boolean = False, Optional ByVal Params() As ParamSQL = Nothing) As ArrayList

        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim rdSQL As SqlDataReader
        Dim pParam As SqlParameter

        Dim aData As New ArrayList


        Dim JumlahParameter As Short

        If IsNothing(Params) Then
            JumlahParameter = -1
        Else
            JumlahParameter = Params.GetUpperBound(0)
        End If

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cmSQL = New SqlCommand
            cmSQL.Connection = cnSQL
            cmSQL.CommandText = SQLText

            If IsStoredProcedure Then
                cmSQL.CommandType = CommandType.StoredProcedure
            End If

            cnSQL.Open()
            Dim i As Short
            For i = 0 To JumlahParameter
                pParam = New SqlParameter(Params(i).ParameterName, Params(i).ParameterValue)
                pParam.Direction = ParameterDirection.Input
                cmSQL.Parameters.Add(pParam)
            Next

            rdSQL = cmSQL.ExecuteReader(CommandBehavior.SingleRow)
            Dim iCount As Short = rdSQL.FieldCount - 1

            If rdSQL.HasRows Then
                rdSQL.Read()
                For i = 0 To iCount
                    aData.Add(rdSQL(i))
                Next

            End If
            cnSQL.Close()

        Catch
            aData.Clear()
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        Finally
        End Try
        Return aData
    End Function
    Public Shared Function ReadRecordsToArrayList(ByVal SQLText As String, ByVal ConString As String, Optional ByVal IsStoredProcedure As Boolean = False, Optional ByVal Params() As ParamSQL = Nothing) As ArrayList
        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim rdSQL As SqlDataReader
        Dim pParam As SqlParameter

        Dim aData As New ArrayList
        Dim oData() As Object
        Dim JumlahParameter As Short

        If IsNothing(Params) Then
            JumlahParameter = -1
        Else
            JumlahParameter = Params.GetUpperBound(0)
        End If

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cmSQL = New SqlCommand
            cmSQL.Connection = cnSQL
            cmSQL.CommandText = SQLText

            If IsStoredProcedure Then
                cmSQL.CommandType = CommandType.StoredProcedure
            End If

            cnSQL.Open()

            Dim i As Short
            Dim iCount As Short

            For i = 0 To JumlahParameter
                pParam = New SqlParameter(Params(i).ParameterName, Params(i).ParameterValue)
                pParam.Direction = ParameterDirection.Input
                cmSQL.Parameters.Add(pParam)
            Next

            rdSQL = cmSQL.ExecuteReader()
            iCount = rdSQL.FieldCount - 1


            If rdSQL.HasRows Then
                While rdSQL.Read()
                    oData = New Object(iCount) {}
                    For i = 0 To iCount
                        oData(i) = rdSQL(i)
                    Next
                    aData.Add(oData)
                End While
            End If
            cnSQL.Close()
        Catch
            aData.Clear()
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        End Try
        Return aData
    End Function
    Public Shared Function ReadRecordMultiQueryToArrayList(ByVal SQLText() As String, ByVal ConString As String, Optional ByVal JumlahParam() As Short = Nothing, Optional ByVal IsStoredProcedure() As Boolean = Nothing, Optional ByVal Params() As ParamSQL = Nothing) As ArrayList

        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim rdSQL As SqlDataReader
        Dim pParam As SqlParameter

        Dim iTimes As Short = SQLText.GetUpperBound(0)
        Dim aData As New ArrayList
        Dim oData() As Object

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cmSQL = New SqlCommand
            cmSQL.Connection = cnSQL

            cnSQL.Open()
            Dim i As Short
            Dim j As Short = 0
            Dim k As Short
            Dim iCount As Short
            For i = 0 To iTimes
                cmSQL.CommandText = SQLText(i)
                If Not IsNothing(IsStoredProcedure) Then
                    If IsStoredProcedure(i) Then
                        cmSQL.CommandType = CommandType.StoredProcedure
                    Else
                        cmSQL.CommandType = CommandType.Text
                    End If
                End If

                If Not IsNothing(JumlahParam) Then

                    cmSQL.Parameters.Clear()

                    For j = j To JumlahParam(i) + j - 1
                        pParam = New SqlParameter(Params(j).ParameterName, Params(j).ParameterValue)
                        pParam.Direction = ParameterDirection.Input
                        cmSQL.Parameters.Add(pParam)
                    Next

                End If


                rdSQL = cmSQL.ExecuteReader(CommandBehavior.SingleRow)
                iCount = rdSQL.FieldCount - 1

                If rdSQL.HasRows Then
                    rdSQL.Read()

                    oData = New Object(iCount) {}

                    For k = 0 To iCount
                        oData(k) = rdSQL(k)
                    Next
                    aData.Add(oData)


                End If
                rdSQL.Close()
            Next

            cnSQL.Close()

        Catch
            aData.Clear()
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        End Try
        Return aData
    End Function
    Public Shared Function ReadRecordsMultiQueryToDataSet(ByVal SQLText() As String, ByVal ConString As String, Optional ByVal JumlahParam() As Short = Nothing, Optional ByVal IsStoredProcedure() As Boolean = Nothing, Optional ByVal Params() As ParamSQL = Nothing) As DataSet

        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim daSQL As SqlDataAdapter
        Dim pParam As SqlParameter

        Dim iTimes As Short = SQLText.GetUpperBound(0)
        Dim dsSQL As New DataSet

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString

            cmSQL = New SqlCommand
            daSQL = New SqlDataAdapter

            cmSQL.Connection = cnSQL

            cnSQL.Open()
            Dim i As Short
            Dim j As Short = 0
            For i = 0 To iTimes
                cmSQL.CommandText = SQLText(i)
                cmSQL.CommandTimeout = 600
                If Not IsNothing(IsStoredProcedure) Then
                    If IsStoredProcedure(i) Then
                        cmSQL.CommandType = CommandType.StoredProcedure
                    Else
                        cmSQL.CommandType = CommandType.Text
                    End If
                End If

                If Not IsNothing(JumlahParam) Then

                    cmSQL.Parameters.Clear()

                    For j = j To JumlahParam(i) + j - 1
                        pParam = New SqlParameter(Params(j).ParameterName, Params(j).ParameterValue)
                        pParam.Direction = ParameterDirection.Input
                        cmSQL.Parameters.Add(pParam)
                    Next

                End If

                daSQL.SelectCommand = cmSQL
                daSQL.Fill(dsSQL, i)

            Next

            cnSQL.Close()

        Catch
            dsSQL.Clear()
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        End Try
        Return dsSQL
    End Function
    Public Shared Function ReadRecordsToDataTable(ByVal SQLText As String, ByVal ConString As String, Optional ByVal IsStoredProcedure As Boolean = False, Optional ByVal Params() As ParamSQL = Nothing) As DataTable

        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim daSQL As SqlDataAdapter
        Dim pParam As SqlParameter

        Dim dtSQL As New DataTable


        Dim JumlahParameter As Short

        If IsNothing(Params) Then
            JumlahParameter = -1
        Else
            JumlahParameter = Params.GetUpperBound(0)
        End If

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cmSQL = New SqlCommand
            cmSQL.Connection = cnSQL
            cmSQL.CommandText = SQLText

            daSQL = New SqlDataAdapter
            If IsStoredProcedure Then
                cmSQL.CommandType = CommandType.StoredProcedure
            End If

            cnSQL.Open()

            Dim i As Short

            For i = 0 To JumlahParameter
                pParam = New SqlParameter(Params(i).ParameterName, Params(i).ParameterValue)
                pParam.Direction = ParameterDirection.Input
                cmSQL.Parameters.Add(pParam)
            Next
            daSQL.SelectCommand = cmSQL
            daSQL.Fill(dtSQL)

            cnSQL.Close()

        Catch
            dtSQL.Clear()
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        End Try
        Return dtSQL
    End Function
    Public Shared Function CopyArrayListToArray(ByVal DataToCopy As ArrayList) As Object(,)
        Try
            Dim oData(,) As Object = Nothing
            Dim iKolom As Short
            Dim iBaris As Integer
            iBaris = DataToCopy.Count - 1
            iKolom = CType(DataToCopy.Item(0), Array).GetUpperBound(0)

            oData = New Object(iKolom, iBaris) {}

            Dim i As Integer
            Dim j As Short

            For i = 0 To iBaris
                For j = 0 To iKolom
                    oData(j, i) = DataToCopy(i)(j)
                Next
            Next
            Return oData
        Catch
            Throw
        End Try
    End Function

    Public Shared Function CopyArrayListToStringForComboFlexX(ByVal DataToCopy As ArrayList, Optional ByVal HaveKey As Boolean = False, Optional ByVal AsList As Boolean = False) As String
        Try
            Dim sResult As String = ""
            If DataToCopy.Count > 0 Then
                If Not AsList Then sResult = "|"
            Else
                Return sResult
                Exit Function

            End If
            Dim i As Integer
            Dim j, st As Short, en As Short
            en = CType(DataToCopy(0), Array).GetUpperBound(0)

            For i = 0 To DataToCopy.Count - 1
                If HaveKey Then
                    sResult = sResult & "" & DataToCopy(i)(0) & "\t"
                    st = 1
                Else
                    st = 0
                End If
                For j = st To en
                    sResult = sResult & DataToCopy(i)(j)
                    If j < en Then
                        sResult = sResult & vbTab
                    Else
                        If i < DataToCopy.Count - 1 Then sResult = sResult & "|"
                    End If
                Next
            Next

            Return sResult
        Catch
            Throw
        End Try
    End Function

    Public Shared Function CopyArrayListToStringForComboFlex(ByVal DataToCopy As ArrayList, Optional ByVal HaveKey As Boolean = False, Optional ByVal AsList As Boolean = False) As String
        Try
            Dim sResult As String = ""
            If DataToCopy.Count > 0 Then
                If Not AsList Then sResult = "|"
            Else
                Return sResult
                Exit Function

            End If
            Dim i As Integer
            Dim j, st As Short, en As Short
            en = CType(DataToCopy(0), Array).GetUpperBound(0)

            For i = 0 To DataToCopy.Count - 1
                If HaveKey Then
                    sResult = sResult & "#" & DataToCopy(i)(0) & ";"
                    st = 1
                Else
                    st = 0
                End If
                For j = st To en
                    sResult = sResult & DataToCopy(i)(j)
                    If j < en Then
                        sResult = sResult & vbTab
                    Else
                        If i < DataToCopy.Count - 1 Then sResult = sResult & "|"
                    End If
                Next
            Next

            Return sResult

        Catch
            Throw
        End Try
    End Function
#End Region
#Region " Writing "
    Public Shared Function WriteSingleTransaction(ByVal SQLText As String, ByVal ConString As String, Optional ByVal IsStoredProcedure As Boolean = False, Optional ByVal Params() As ParamSQL = Nothing) As Integer
        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim iRows As Integer
        Dim JumlahParameter As Short
        Dim pParam As SqlParameter

        If IsNothing(Params) Then
            JumlahParameter = -1
        Else
            JumlahParameter = Params.GetUpperBound(0)
        End If

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cmSQL = New SqlCommand
            cmSQL.Connection = cnSQL
            cmSQL.CommandText = SQLText
            cmSQL.CommandTimeout = 600

            If IsStoredProcedure Then
                cmSQL.CommandType = CommandType.StoredProcedure
            End If

            cnSQL.Open()


            Dim i As Short
            For i = 0 To JumlahParameter
                pParam = New SqlParameter(Params(i).ParameterName, Params(i).ParameterValue)
                pParam.Direction = ParameterDirection.Input
                cmSQL.Parameters.Add(pParam)
            Next

            iRows = cmSQL.ExecuteNonQuery()
            cnSQL.Close()
        Catch
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            Throw
        End Try
        Return iRows
    End Function
    Public Shared Function WriteMultiTransaction(ByVal SQLText() As String, ByVal ConString As String, Optional ByVal JumlahParam() As Short = Nothing, Optional ByVal IsStoredProcedure() As Boolean = Nothing, Optional ByVal Params() As ParamSQL = Nothing) As Integer

        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim trSQL As SqlTransaction
        Dim pParam As SqlParameter

        Dim iRows As Integer
        Dim iTimes As Short = SQLText.GetUpperBound(0)

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString

            cnSQL.Open()
            trSQL = cnSQL.BeginTransaction
            Try
                cmSQL = New SqlCommand
                cmSQL.Connection = cnSQL
                cmSQL.Transaction = trSQL


                Dim i As Short
                Dim j As Short = 0

                For i = 0 To iTimes
                    cmSQL.CommandText = SQLText(i)
                    If Not IsNothing(IsStoredProcedure) Then
                        If IsStoredProcedure(i) Then
                            cmSQL.CommandType = CommandType.StoredProcedure
                        Else
                            cmSQL.CommandType = CommandType.Text
                        End If
                    End If
                    If Not IsNothing(JumlahParam) Then
                        cmSQL.Parameters.Clear()
                        For j = j To JumlahParam(i) + j - 1
                            pParam = New SqlParameter(Params(j).ParameterName, Params(j).ParameterValue)
                            pParam.Direction = ParameterDirection.Input
                            cmSQL.Parameters.Add(pParam)
                        Next
                    End If
                    iRows += cmSQL.ExecuteNonQuery()

                Next
                trSQL.Commit()

                cnSQL.Close()


            Catch
                trSQL.Rollback()
                If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
                iRows = 0
                Throw
            End Try
        Catch
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            iRows = 0
            Throw
        End Try
        Return iRows
    End Function

    Public Shared Function WriteSingleTransactionRepeatedly(ByVal SQLText As String, ByVal ConString As String, ByVal Params(,) As ParamSQL, Optional ByVal IsStoredProcedure As Boolean = False) As Integer


        Dim cnSQL As SqlConnection = Nothing
        Dim cmSQL As SqlCommand
        Dim trSQL As SqlTransaction
        Dim pParam As SqlParameter

        Dim iRows As Integer
        Dim iTimes As Short = Params.GetUpperBound(0)
        Dim JumlahParameter As Short = Params.GetUpperBound(1)

        Try
            cnSQL = New SqlConnection
            cnSQL.ConnectionString = ConString
            cnSQL.Open()
            trSQL = cnSQL.BeginTransaction
            Try
                cmSQL = New SqlCommand
                cmSQL.Connection = cnSQL
                cmSQL.Transaction = trSQL
                cmSQL.CommandText = SQLText
                If IsStoredProcedure Then
                    cmSQL.CommandType = CommandType.StoredProcedure
                End If

                Dim i As Short
                Dim j As Short

                For i = 0 To iTimes

                    cmSQL.Parameters.Clear()
                    For j = 0 To JumlahParameter
                        pParam = New SqlParameter(Params(i, j).ParameterName, Params(i, j).ParameterValue)
                        pParam.Direction = ParameterDirection.Input
                        cmSQL.Parameters.Add(pParam)
                    Next
                    iRows += cmSQL.ExecuteNonQuery()
                Next
                trSQL.Commit()

                cnSQL.Close()


            Catch
                trSQL.Rollback()
                If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
                iRows = 0
                Throw
            End Try

        Catch
            If cnSQL.State = ConnectionState.Open Then cnSQL.Close()
            iRows = 0
            Throw
        End Try
        Return iRows
    End Function

#End Region
End Class
Public Class ParamSQL
#Region " Private Attributes "
    Private Nama As String
    Private Nilai As Object

#End Region
#Region " Constructors "
    Public Sub New()
        MyBase.New()
        Nama = ""
        Nilai = Nothing
    End Sub
    Public Sub New(ByVal ParameterName As String, ByVal ParameterValue As Object)
        MyBase.New()
        Nama = ParameterName
        Nilai = ParameterValue
    End Sub
#End Region
#Region " Properties "
    Public Property ParameterName() As String
        Get
            Return Nama
        End Get
        Set(ByVal Value As String)
            Nama = Value
        End Set
    End Property
    Public Property ParameterValue() As Object
        Get
            Return Nilai
        End Get
        Set(ByVal Value As Object)
            Nilai = Value
        End Set
    End Property
#End Region

End Class
Public Class UserApp
#Region " Private Attributes "

    Private UID As String
    Private UPassword As String
    Private UName As String
    Private USection As String
    Private UActived As Char
    Private UStatus As ReturnFill
    Private UBackColor As Drawing.Color
    Private UForeColor As Drawing.Color
    Private UWarehouse As String
    Private ULookFeel As String

#End Region
    Public Enum ReturnFill As Short
        ValidUser = 1
        PasswordNotMatch = 0
        InvalidUser = -1
    End Enum

#Region " Properties "
    Public Property UserID() As String
        Get
            Return UID
        End Get
        Set(ByVal Value As String)
            UID = Value
        End Set
    End Property
    Public Property LookFeel() As String
        Get
            Return ULookFeel
        End Get
        Set(ByVal Value As String)
            ULookFeel = Value
        End Set
    End Property
    Public Property UserPassword() As String
        Get
            Return UPassword
        End Get
        Set(ByVal Value As String)
            UPassword = Value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return UName
        End Get
        Set(ByVal Value As String)
            UName = Value
        End Set
    End Property
    Public Property UserSection() As String
        Get
            Return USection
        End Get
        Set(ByVal Value As String)
            USection = Value
        End Set
    End Property

    Public Property Actived() As Char
        Get
            Return UActived
        End Get
        Set(ByVal Value As Char)
            UActived = Value
        End Set
    End Property
    Public Property Status() As ReturnFill
        Get
            Return UStatus
        End Get
        Set(ByVal Value As ReturnFill)
            UStatus = Value
        End Set
    End Property
    Public Property BackColor() As Drawing.Color
        Get
            Return UBackColor
        End Get
        Set(ByVal Value As Drawing.Color)
            UBackColor = Value

        End Set
    End Property
    Public Property ForeColor() As Drawing.Color
        Get
            Return UForeColor
        End Get
        Set(ByVal Value As Drawing.Color)
            UForeColor = Value
        End Set
    End Property
    Public Property WareHouse() As String
        Get
            Return UWarehouse
        End Get
        Set(ByVal Value As String)
            UWarehouse = Value
        End Set
    End Property
#End Region
#Region " Methods "
    Public Function FillAttributes(ByVal SQLText As String, ByVal ID As String, ByVal Pwd As String, ByVal ConString As String) As ReturnFill
        Try
            Dim sSQL As String
            Dim pParam(0) As ParamSQL
            pParam(0) = New ParamSQL("@ID", ID)
            sSQL = SQLText & " where UserID = @ID"
            Dim arrList As ArrayList
            arrList = DBTrans.ReadSingleRecord(sSQL, ConString, False, pParam)
            If arrList.Count > 0 Then
                If arrList(2) = Utility.Encryption(Pwd) Then
                    UID = arrList(0)
                    UName = arrList(1)
                    UPassword = arrList(2)
                    UActived = arrList(3)
                    USection = arrList(4)
                    ULookFeel = arrList(7)
                    'UWarehouse = arrList(7)
                    UStatus = ReturnFill.ValidUser
                Else
                    UStatus = ReturnFill.PasswordNotMatch
                End If
            Else
                UStatus = ReturnFill.InvalidUser
            End If
            Return UStatus
        Catch
            Throw
        End Try
    End Function
#End Region
End Class



Public Class DBConn
#Region " Private Attributes "

    Private DBServer As String
    Private DBName As String
    Private DBUser As String
    Private DBPassword As String
    Private DBString As String

#End Region

#Region " Properties "
    Public Property Server() As String
        Get
            Return DBServer
        End Get
        Set(ByVal Value As String)
            DBServer = Value
        End Set
    End Property
    Public Property DataBase() As String
        Get
            Return DBName
        End Get
        Set(ByVal Value As String)
            DBName = Value
        End Set
    End Property
    Public Property User() As String
        Get
            Return DBUser
        End Get
        Set(ByVal Value As String)
            DBUser = Value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return DBPassword
        End Get
        Set(ByVal Value As String)
            DBPassword = Value
        End Set
    End Property
    Public Property ConnectionString() As String
        Get
            Return DBString
        End Get
        Set(ByVal Value As String)
            DBString = Value
        End Set
    End Property

#End Region
#Region " Methods "
    Public Sub LoadConfiguration(Optional ByVal Opt As String = "")
        DBServer = ConfigurationManager.AppSettings("Server" & Opt)
        DBServer = Utility.Decryption(DBServer)
        DBName = ConfigurationManager.AppSettings("Database" & Opt)
        DBName = Utility.Decryption(DBName)
        DBUser = ConfigurationManager.AppSettings("UserID" & Opt)
        DBUser = Utility.Decryption(DBUser)
        DBPassword = ConfigurationManager.AppSettings("Password" & Opt)
        DBPassword = Utility.Decryption(DBPassword)
        CreateConnectionString()
    End Sub
    Public Sub CreateConnectionString()
        DBString = "server=" & DBServer & ";database=" & DBName & ";user id=" & DBUser & ";password=" & DBPassword
    End Sub
    Public Shared Function Connection(Optional ByVal Opt As String = "") As String
        Try
            Dim sDBServer As String
            Dim sDBName As String
            Dim sDBUser As String
            Dim sDBPassword As String

            sDBServer = ConfigurationManager.AppSettings("Server" & Opt)
            sDBServer = Utility.Decryption(sDBServer)
            sDBName = ConfigurationManager.AppSettings("Database" & Opt)
            sDBName = Utility.Decryption(sDBName)
            sDBUser = ConfigurationManager.AppSettings("UserID" & Opt)
            sDBUser = Utility.Decryption(sDBUser)
            sDBPassword = ConfigurationManager.AppSettings("Password" & Opt)
            sDBPassword = Utility.Decryption(sDBPassword)
            Connection = "server=" & sDBServer & ";database=" & sDBName & ";user id=" & sDBUser & ";password=" & sDBPassword
        Catch
            Throw
        End Try
    End Function
#End Region
End Class
Public Class ConfigFile

    Dim m_strConfigFile As String
    Dim oDoc As New XmlDocument

    Public Sub New(ByVal ConfigFile As String)
        m_strConfigFile = ConfigFile
        oDoc.Load(m_strConfigFile)
    End Sub

    ' Adds a configuration section
    Public Sub AddConfigSection(ByVal SectionName As String, ByVal HandlerClass As String)
        Try
            Dim rootNode As XmlNode = oDoc.GetElementsByTagName("configuration").Item(0)

            ' create the configSections node if it doesn't exist as config sections
            ' need an entry in this node
            Dim oNode As XmlNode
            oNode = oDoc.DocumentElement("configSections")
            If oNode Is Nothing Then
                oNode = oDoc.CreateElement("configSections")
                If rootNode.ChildNodes.Count > 0 Then
                    Dim oFirstChild As XmlNode = rootNode.FirstChild
                    rootNode.InsertBefore(oNode, oFirstChild)
                Else
                    rootNode.AppendChild(oNode)
                End If
            End If

            ' add the section to the configSectionsNode
            Dim oSubNode As XmlNode
            oSubNode = oDoc.CreateElement("section")
            With oSubNode.Attributes.Append(oDoc.CreateAttribute("name"))
                .Value = SectionName
            End With
            With oSubNode.Attributes.Append(oDoc.CreateAttribute("type"))
                .Value = HandlerClass
            End With
            oNode.AppendChild(oSubNode)

            ' now create the actual section, if it's not there
            oNode = oDoc.DocumentElement(SectionName)
            If oNode Is Nothing Then
                oNode = oDoc.CreateElement(SectionName)
                rootNode.AppendChild(oNode)
            End If

            oDoc.Save(m_strConfigFile)
        Catch
            Throw
        End Try
    End Sub

    ' Checks whether a configuration section exists
    Public Function ConfigSectionExists(ByVal SectionName As String) As Boolean
        If Not oDoc.DocumentElement(SectionName) Is Nothing Then
            Return True
        End If
    End Function

    Public Sub SetConfigAttribute(ByVal SectionName As String, ByVal AttributeName As String, ByVal Value As String, Optional ByVal Create As Boolean = True)
        Try
            ' get the section node
            Dim oNode As XmlNode = oDoc.DocumentElement(SectionName)
            Dim oAttr As XmlAttribute
            oAttr = oNode.Attributes.ItemOf(AttributeName)
            If oAttr Is Nothing Then
                oAttr = oDoc.CreateAttribute(AttributeName)
                oNode.Attributes.Append(oAttr)
            End If
            oAttr.Value = Value
            oDoc.Save(m_strConfigFile)
        Catch
            Throw
        End Try
    End Sub

    Public Sub SetAppSetting(ByVal SettingName As String, ByVal Value As String)
        Try
            ' get the node that corresponds to the setting 
            Dim oSettingNode As XmlNode = GetAppSettingNode(SettingName)

            ' encrypt the value if requested and then set it

            oSettingNode.Attributes.ItemOf("value").Value = Value

            ' save changes
            oDoc.Save(m_strConfigFile)
        Catch
            Throw
        End Try
    End Sub

    Private Function AddAppSetting(ByVal SettingName As String) As XmlNode
        Try
            ' get the appSettings node
            Dim oNode As XmlNode = oDoc.DocumentElement("appSettings")

            ' create the key attribute 
            Dim oKey As XmlAttribute = oDoc.CreateAttribute("key")
            oKey.Value = SettingName

            ' create the value attribute
            Dim oValue As XmlAttribute = oDoc.CreateAttribute("value")
            oValue.Value = ""

            ' create the node for the setting
            Dim oChild As XmlNode = oDoc.CreateElement("add")
            oChild.Attributes.Append(oKey)
            oChild.Attributes.Append(oValue)

            ' add the node to the appSettings section
            oNode.AppendChild(oChild)
            Return oChild
        Catch
            Throw
        End Try
    End Function

    Private Function GetAppSettingNode(ByVal SettingName As String, Optional ByVal CreateIfNotFound As Boolean = True) As XmlNode
        Try
            ' get the appSettings node
            Dim oNode As XmlNode = oDoc.DocumentElement("appSettings")
            Dim oChild As XmlNode = Nothing
            Dim blnFound As Boolean

            ' find the node corresponding to the setting
            For Each oChild In oNode.ChildNodes
                If oChild.Attributes.ItemOf("key").Value = SettingName Then
                    blnFound = True
                    Exit For
                End If
            Next

            ' if the node was not found we need to create it if it was requested
            If Not blnFound Then
                If CreateIfNotFound Then
                    oChild = AddAppSetting(SettingName)
                Else
                    oChild = Nothing
                End If
            End If

            ' return the node
            Return oChild
        Catch
            Throw
        End Try
    End Function

    Public Function GetAppSetting(ByVal SettingName As String) As String
        Try
            ' get the node that corresponds to the setting 
            Dim oSettingNode As XmlNode = GetAppSettingNode(SettingName, False)

            ' if the node for the setting does not exist the just return an empty string
            If oSettingNode Is Nothing Then
                Return ""
            Else
                ' get the value and decrypt it if requested 
                GetAppSetting = oSettingNode.Attributes.ItemOf("value").Value

            End If
        Catch
            Throw
        End Try
    End Function

    Public Function GetConfigAttribute(ByVal SectionName As String, ByVal AttributeName As String, Optional ByVal Create As Boolean = True) As String
        Try
            ' get the section node
            Dim oNode As XmlNode = oDoc.DocumentElement(SectionName)
            Dim oAttr As XmlAttribute
            oAttr = oNode.Attributes.ItemOf(AttributeName)
            If oAttr Is Nothing Then
                oAttr = oDoc.CreateAttribute(AttributeName)
                oNode.Attributes.Append(oAttr)
                oDoc.Save(m_strConfigFile)
            End If
            Return oAttr.Value
        Catch
            Throw
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

Public Class AppUtils
    Public Shared Function UploadData(ByVal FileUploadCtl As System.Web.UI.WebControls.FileUpload, ByVal DesctPath As String, ByVal DataFileName As String, ByVal DataContentType As String, Optional ByVal DataContentLen As Long = 2024000) As String
        Try
            If FileUploadCtl.PostedFile.ContentType = DataContentType Then
                If FileUploadCtl.PostedFile.ContentLength < DataContentLen Then
                    Dim filename As String
                    filename = DataFileName
                    AppUtils.CreateFolder(DesctPath)
                    FileUploadCtl.SaveAs(DesctPath & "/" & filename) 'Server.MapPath("~/")
                    UploadData = "Upload status: File uploaded!"
                Else
                    UploadData = "Upload status: The file has to be less than 5 M!"
                End If
            Else
                UploadData = "Upload status: Only TXT files are accepted!"
            End If

        Catch ex As Exception
            UploadData = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message
        End Try
    End Function

    Public Shared Sub CreateFolder(ByVal NewFolder As String)
        Try
            ' Check if directory exists
            If Not Directory.Exists(NewFolder) Then
                ' Create the directory.
                Directory.CreateDirectory(NewFolder)
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Function SaveArrayListToExcel(ByVal NamaFile As String, ByVal Ar As ArrayList, Optional ByVal Header() As String = Nothing) As Integer

        'Dim JumlahRecord As Integer
        'Dim JumlahKolom As Integer
        'Dim i, j As Integer
        'JumlahRecord = Ar.Count
        'If JumlahRecord > 0 Then
        '    JumlahKolom = Ar(0).GetUpperBound(0) + 1

        'End If

        'Dim xcApp As New Excel.ExcelApplication '.Application
        'xcApp.Workbooks.Add()
        'With xcApp.ActiveSheet
        '    If Not IsNothing(Header) Then
        '        For j = 0 To Header.GetUpperBound(0)
        '            .Cells(1, j + 1) = Header(j)
        '        Next
        '    End If
        '    For i = 1 To JumlahRecord
        '        For j = 1 To JumlahKolom
        '            .Cells(i + 1, j) = Ar(i - 1)(j - 1)
        '        Next
        '    Next
        'End With
        'System.IO.File.Delete(NamaFile)


        ''xcApp.WindowState = Excel.XlWindowState.xlMaximized
        ''xcApp.Visible = True

        ''xcApp.Workbooks(1).SaveAs(NamaFile)
        ''xcApp.ActiveSheet.PrintPreview(False)
        'xcApp.ActiveSheet.SaveAs(NamaFile)
        ''xcApp.Visible = True
        ''xcApp.Workbooks(1).Saved = True

        ''xcApp.ActiveSheet.Saved = True
        'xcApp.Quit()
        'Return JumlahRecord

    End Function

    Public Shared Sub ImportDataFromTextFile(ByVal TableName As String, ByVal PathDir As String, ByVal FileName As String)
        Try
            Using bulk As New SqlBulkCopy(DBConn.Connection)
                bulk.BatchSize = 1000
                bulk.NotifyAfter = 1000
                bulk.DestinationTableName = TableName
                AddHandler bulk.SqlRowsCopied, AddressOf OnSqlRowsCopied
                bulk.WriteToServer(ImportFromTxt(PathDir, FileName))
            End Using
        Catch
            Throw
        End Try
        'Dts.TaskResult = Dts.Results.Success
    End Sub
    Public Shared Sub ImportDataFromTextFile2(ByVal TableName As String, ByVal PathDir As String, ByVal FileName As String)
        Try
            Using bulk As New SqlBulkCopy(DBConn.Connection)
                bulk.BatchSize = 1000
                bulk.NotifyAfter = 1000
                bulk.DestinationTableName = TableName
                AddHandler bulk.SqlRowsCopied, AddressOf OnSqlRowsCopied
                bulk.WriteToServer(ImportFromTxt2(PathDir, FileName))
            End Using
        Catch
            Throw
        End Try
        'Dts.TaskResult = Dts.Results.Success
    End Sub
    Public Shared Sub OnSqlRowsCopied(ByVal sender As Object, _
        ByVal args As SqlRowsCopiedEventArgs)
        Console.WriteLine("Copied {0} so far...", args.RowsCopied)
    End Sub

    Public Shared Function ImportFromTxt(ByVal PathDir As String, ByVal FileName As String) As DataTable
        Try
            'Dim cn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & PathDir & ";Extended Properties=""Text;HDR=No;FMT=Delimited""")
            Dim cn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & PathDir & ";Extended Properties=""Text;HDR=No;FMT=Delimited""")
            Dim da As New OleDbDataAdapter()
            Dim ds As New DataSet
            Dim cd As New OleDbCommand("SELECT * FROM [" & FileName & "]", cn)
            cn.Open()
            da.SelectCommand = cd
            ds.Clear()
            da.Fill(ds, Mid(FileName, 1, InStr(FileName, ".") - 1))
            Return ds.Tables(0)
            cn.Close()
        Catch
            Throw
        End Try
    End Function
    Public Shared Function ImportFromTxt2(ByVal PathDir As String, ByVal FileName As String) As DataTable
        Try
            'Dim cn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & PathDir & ";Extended Properties=""Text;HDR=No;FMT=Delimited""")
            Dim cn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & PathDir & ";Extended Properties=""Text;HDR=No;FMT=Delimited""")
            Dim da As New OleDbDataAdapter()
            Dim ds As New DataSet
            Dim cd As New OleDbCommand("SELECT * FROM [" & FileName & "]", cn)
            cn.Open()
            da.SelectCommand = cd
            ds.Clear()
            da.Fill(ds, Mid(FileName, 1, InStr(FileName, ".") - 1))
            Return ds.Tables(0)
            cn.Close()
        Catch
            Throw
        End Try
    End Function
    Public Shared Sub ExportDataToExcel(ByVal ds As DataSet, ByVal DirPath As String, ByVal Filename As String)
        Dim objExcel As Excel.Application
        Dim objBooks As Excel.Workbooks, objBook As Excel.Workbook
        Dim objSheets As Excel.Sheets, objSheet As Excel.Worksheet
        Dim objRange As Excel.Range
        Try
            Dim response As HttpResponse = HttpContext.Current.Response

            If ds.Tables(0).Rows.Count > 0 Then
                Dim sTmpDir As String
                sTmpDir = "C:\Download\" & DirPath
                If Not Directory.Exists(sTmpDir) Then
                    ' Create the directory.
                    Directory.CreateDirectory(sTmpDir)
                End If

                Dim strFileName As String
                strFileName = sTmpDir & Filename



                'Creating a new object of the Excel application object
                objExcel = New Excel.Application
                'Hiding the Excel application
                objExcel.Visible = False
                'Hiding all the alert messages occurring during the process
                objExcel.DisplayAlerts = False

                'Adding a collection of Workbooks to the Excel object
                objBook = CType(objExcel.Workbooks.Add(), Excel.Workbook)

                'Saving the Workbook as a normal workbook format.
                objBook.SaveAs(strFileName, Excel.XlFileFormat.xlWorkbookNormal)

                'Getting the collection of workbooks in an object
                objBooks = objExcel.Workbooks

                'Get the reference to the first sheet
                'in the workbook collection in a variable
                objSheet = CType(objBooks(1).Sheets.Item(1), Excel.Worksheet)
                'Optionally name the worksheet
                objSheet.Name = "First Sheet"
                'You can even set the font attributes of a range of cells
                'in the sheet. Here we have set the fonts to bold.
                objSheet.Range("A1", "Z1").Font.Bold = True

                'Get the cells collection of the sheet in a variable, to write the data.
                objRange = objSheet.Cells

                'Calling the function to write the dataset data in the cells of the first sheet.
                WriteData(ds.Tables(0), objRange)

                'Setting the width of the specified range of cells
                'so as to absolutely fit the written data.
                objSheet.Range("A1", "Z1").EntireColumn.AutoFit()
                'Saving the worksheet.
                objSheet.SaveAs(strFileName)


            End If
        Catch
            Throw
        Finally
            'Close the Excel application
            objExcel.Quit()

            'Release all the COM objects so as to free the memory
            ReleaseComObject(objRange)
            ReleaseComObject(objSheet)
            'ReleaseComObject(objSheets)
            ReleaseComObject(objBook)
            ReleaseComObject(objBooks)
            ReleaseComObject(objExcel)

            'Set the all the objects for the Garbage collector to collect them.
            objExcel = Nothing
            objBooks = Nothing
            objBook = Nothing
            objSheets = Nothing
            objSheet = Nothing
            objRange = Nothing

            'Specifically call the garbage collector.
            System.GC.Collect()
        End Try
    End Sub

    Public Shared Sub ExportDataToExcelMultipleSheet(ByVal ds() As DataSet, ByVal DirPath As String, ByVal Filename As String, Optional ByVal ColTittle As Integer = 0)
        Dim objExcel As Excel.Application
        Dim objBooks As Excel.Workbooks, objBook As Excel.Workbook
        Dim objSheets As Excel.Sheets, objSheet As Excel.Worksheet
        Dim objRange As Excel.Range
        Try
            Dim response As HttpResponse = HttpContext.Current.Response

            Dim iTimes As Short = ds.GetUpperBound(0)
            Dim i As Short
            Dim sTmpDir As String = ""
            Dim strFileName As String = ""

            If iTimes >= 0 Then
                sTmpDir = "C:\Download\" & DirPath
                If Not Directory.Exists(sTmpDir) Then
                    ' Create the directory.
                    Directory.CreateDirectory(sTmpDir)
                End If

                strFileName = sTmpDir & Filename

                'Creating a new object of the Excel application object
                objExcel = New Excel.Application
                'Hiding the Excel application
                objExcel.Visible = False
                'Hiding all the alert messages occurring during the process
                objExcel.DisplayAlerts = False

                'Adding a collection of Workbooks to the Excel object
                objBook = CType(objExcel.Workbooks.Add(), Excel.Workbook)

                'Saving the Workbook as a normal workbook format.
                objBook.SaveAs(strFileName, Excel.XlFileFormat.xlWorkbookNormal)

                'Getting the collection of workbooks in an object
                objBooks = objExcel.Workbooks
            Else
                Exit Sub
            End If

            For i = 0 To iTimes
                If Not IsNothing(ds(i)) Then
                    If ds(i).Tables(0).Rows.Count > 0 Then
                        'Get the reference to the first sheet
                        'in the workbook collection in a variable
                        If i >= 3 Then 'sheet excel hanya ada 3 defaultnya
                            objSheet = CType(objBooks(1).Worksheets.Add(), Excel.Worksheet)
                        Else
                            objSheet = CType(objBooks(1).Sheets.Item(i + 1), Excel.Worksheet)
                        End If
                        'objSheet = CType(objBooks(1).Sheets.Item(i + 1), Excel.Worksheet)
                        'Optionally name the worksheet
                        objSheet.Name = ds(i).Tables(0).Rows(0)(ColTittle).ToString '"First Sheet"
                        'You can even set the font attributes of a range of cells
                        'in the sheet. Here we have set the fonts to bold.
                        objSheet.Range("A1", "Z1").Font.Bold = True

                        'Get the cells collection of the sheet in a variable, to write the data.
                        objRange = objSheet.Cells

                        'Calling the function to write the dataset data in the cells of the first sheet.
                        WriteData(ds(i).Tables(0), objRange)

                        'Setting the width of the specified range of cells
                        'so as to absolutely fit the written data.
                        objSheet.Range("A1", "Z1").EntireColumn.AutoFit()
                        'Saving the worksheet.
                        objSheet.SaveAs(strFileName)
                    End If
                End If
            Next

        Catch
            Throw
        Finally
            'Close the Excel application
            objExcel.Quit()

            'Release all the COM objects so as to free the memory
            ReleaseComObject(objRange)
            ReleaseComObject(objSheet)
            'ReleaseComObject(objSheets)
            ReleaseComObject(objBook)
            ReleaseComObject(objBooks)
            ReleaseComObject(objExcel)

            'Set the all the objects for the Garbage collector to collect them.
            objExcel = Nothing
            objBooks = Nothing
            objBook = Nothing
            objSheets = Nothing
            objSheet = Nothing
            objRange = Nothing

            'Specifically call the garbage collector.
            System.GC.Collect()
        End Try
    End Sub

    Public Shared Function WriteData(ByVal DT_DataTable As DataTable, _
        ByVal objCells As Excel.Range) As String
        Dim iRow As Integer, iCol As Integer

        'Traverse through the DataTable columns to write the
        'headers on the first row of the excel sheet.
        For iCol = 0 To DT_DataTable.Columns.Count - 1
            objCells(1, iCol + 1) = DT_DataTable.Columns(iCol).ToString
        Next

        'Traverse through the rows and columns
        'of the datatable to write the data in the sheet.
        For iRow = 0 To DT_DataTable.Rows.Count - 1
            For iCol = 0 To DT_DataTable.Columns.Count - 1
                If Not IsDBNull(DT_DataTable.Rows(iRow)(iCol)) Then
                    If DT_DataTable.Columns(iCol).DataType.Name = "String" Or DT_DataTable.Columns(iCol).DataType.Name = "Char" Then
                        objCells(iRow + 2, iCol + 1).NumberFormat = "@"
                        objCells(iRow + 2, iCol + 1).FormulaR1C1 = DT_DataTable.Rows(iRow)(iCol)
                    ElseIf DT_DataTable.Columns(iCol).DataType.Name = "TimeSpan" Or DT_DataTable.Columns(iCol).DataType.Name = "DateTime" Then
                        objCells(iRow + 2, iCol + 1).NumberFormat = "m/d/yyyy;@"
                        objCells(iRow + 2, iCol + 1).FormulaR1C1 = DT_DataTable.Rows(iRow)(iCol)
                    ElseIf DT_DataTable.Columns(iCol).DataType.Name = "Decimal" Or DT_DataTable.Columns(iCol).DataType.Name = "Double" Or _
                          DT_DataTable.Columns(iCol).DataType.Name = "Int32" Or DT_DataTable.Columns(iCol).DataType.Name = "Int64" Or _
                        DT_DataTable.Columns(iCol).DataType.Name = "Int16" Or DT_DataTable.Columns(iCol).DataType.Name = "Single" Then
                        objCells(iRow + 2, iCol + 1).NumberFormat = "0.00"
                        objCells(iRow + 2, iCol + 1).FormulaR1C1 = Convert.ToDouble(DT_DataTable.Rows(iRow)(iCol))
                    Else
                        objCells(iRow + 2, iCol + 1) = DT_DataTable.Rows(iRow)(iCol)
                    End If
                Else
                    objCells(iRow + 2, iCol + 1) = DT_DataTable.Rows(iRow)(iCol)
                End If

            Next
        Next
    End Function

    Public Shared Sub ExportDataToTextFile(ByVal ds As DataSet, ByVal DirPath As String, ByVal Filename As String, Optional ByVal WithHeader As Boolean = True)
        Dim delim As String
        Dim sw As StreamWriter
        Try
            ' DOWNLOAD DATA TO .TXT.... 
            '-------------------------------- 
            ' PULL THE DATA FROM SQL SERVER AND POPULATE THE DATA TO A DATASET..... 
            '-------------------------------- 
            'ReDim sSQL(0)
            'sSQL(0) = "SELECT * FROM  ds_PICTYPE"
            'ds = DBTrans.ReadRecordsMultiQueryToDataSet(sSQL, DBConn.Connection)
            If ds.Tables(0).Rows.Count > 0 Then
                '-------------------------------- 
                ' PULL THE DATA FROM DATASET AND INSERT INTO .TXT FILE..... 
                '-------------------------------- 
                ' Write out the header row 
                delim = ""

                Dim sTmpDir As String
                sTmpDir = "C:\Download\" & DirPath
                If Not Directory.Exists(sTmpDir) Then
                    ' Create the directory.
                    Directory.CreateDirectory(sTmpDir)
                End If

                sw = New StreamWriter(sTmpDir & "\" & Filename, False, UnicodeEncoding.Default)

                For Each dt As System.Data.DataTable In ds.Tables
                    ' Write out the header row 
                    If WithHeader Then
                        delim = ""
                        For Each col As DataColumn In dt.Columns
                            sw.Write(delim)
                            sw.Write(""""c)
                            sw.Write(col.ColumnName)
                            sw.Write(""""c)
                            delim = ","
                        Next
                        sw.WriteLine()
                    End If

                    ' write out each data row 
                    For Each row As DataRow In dt.Rows
                        delim = ""
                        For Each value As Object In row.ItemArray
                            sw.Write(delim)


                            'If TypeOf value Is String Then
                            sw.Write(""""c) ' thats four double quotes and a c 
                            'sw.Write(value)
                            If Not IsDBNull(value) Then
                                If InStr(value, vbCrLf) > 0 Then
                                    sw.Write(value.ToString.Replace(vbCrLf, " "))
                                Else
                                    sw.Write(value)
                                End If
                            Else
                                sw.Write(value)
                            End If
                            sw.Write(""""c) ' thats four double quotes and a c 
                            'Else
                            '    sw.Write(value)
                            'End If
                            delim = ","
                        Next
                        sw.WriteLine()
                    Next
                Next

            End If

        Catch
            Throw
        Finally
            If Not sw Is Nothing Then
                sw.Close()
            End If
        End Try
    End Sub
    Public Shared Sub ExportDataToTextFileNoHeader(ByVal ds As DataSet, ByVal DirPath As String, ByVal Filename As String, Optional ByVal WithHeader As Boolean = True)
        Dim delim As String
        Dim sw As StreamWriter
        Try
            ' DOWNLOAD DATA TO .TXT.... 
            '-------------------------------- 
            ' PULL THE DATA FROM SQL SERVER AND POPULATE THE DATA TO A DATASET..... 
            '-------------------------------- 
            'ReDim sSQL(0)
            'sSQL(0) = "SELECT * FROM  ds_PICTYPE"
            'ds = DBTrans.ReadRecordsMultiQueryToDataSet(sSQL, DBConn.Connection)
            If ds.Tables(0).Rows.Count > 0 Then
                '-------------------------------- 
                ' PULL THE DATA FROM DATASET AND INSERT INTO .TXT FILE..... 
                '-------------------------------- 
                ' Write out the header row 
                delim = ""

                Dim sTmpDir As String
                sTmpDir = "C:\Download\" & DirPath
                If Not Directory.Exists(sTmpDir) Then
                    ' Create the directory.
                    Directory.CreateDirectory(sTmpDir)
                End If

                sw = New StreamWriter(sTmpDir & "\" & Filename, False, UnicodeEncoding.Default)

                For Each dt As System.Data.DataTable In ds.Tables
                    ' Write out the header row 
                    'If WithHeader Then
                    '    delim = ""
                    '    For Each col As DataColumn In dt.Columns
                    '        sw.Write(delim)
                    '        sw.Write(""""c)
                    '        sw.Write(col.ColumnName)
                    '        sw.Write(""""c)
                    '        delim = ","
                    '    Next
                    '    sw.WriteLine()
                    'End If

                    ' write out each data row 
                    For Each row As DataRow In dt.Rows
                        delim = ""
                        For Each value As Object In row.ItemArray
                            sw.Write(delim)


                            'If TypeOf value Is String Then
                            sw.Write(""""c) ' thats four double quotes and a c 
                            'sw.Write(value)
                            If Not IsDBNull(value) Then
                                If InStr(value, vbCrLf) > 0 Then
                                    sw.Write(value.ToString.Replace(vbCrLf, " "))
                                Else
                                    sw.Write(value)
                                End If
                            Else
                                sw.Write(value)
                            End If
                            sw.Write(""""c) ' thats four double quotes and a c 
                            'Else
                            '    sw.Write(value)
                            'End If
                            delim = ","
                        Next
                        sw.WriteLine()
                    Next
                Next

            End If

        Catch
            Throw
        Finally
            If Not sw Is Nothing Then
                sw.Close()
            End If
        End Try
    End Sub
    Public Shared Sub DownloadFile(ByVal FilePath As String, ByVal FileName As String, Optional ByVal ContentType As String = "application/octet-stream")
        Try
            Dim path As String = "C:\Download\" & FilePath & "\" & FileName 'Server.MapPath("C:/_Test.txt") 'get file object as FileInfo  
            Dim tmpFile As System.IO.FileInfo = New System.IO.FileInfo(path) '-- if the file exists on the server  
            If tmpFile.Exists Then 'set appropriate headers  
                'HttpContext.Current.Response.BufferOutput = True
                'HttpContext.Current.Response.BinaryWrite( bytes)
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.Buffer = True
                HttpContext.Current.Response.ContentType = GetMimeTypeByFileName(FileName) 'ContentType
                'HttpContext.Current.Response.AddHeader("Pragma", "no-cache")
                HttpContext.Current.Response.AddHeader("Cache-Control", "public")
                HttpContext.Current.Response.AddHeader("max-age", "0")
                HttpContext.Current.Response.AddHeader("Content-Disposition", " attachment; filename=" & tmpFile.Name) '
                HttpContext.Current.Response.AddHeader("Content-Length", tmpFile.Length.ToString())
                HttpContext.Current.Response.WriteFile(tmpFile.FullName)
                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.End() 'if file does not exist 
                If File.Exists(tmpFile.FullName) Then
                    File.Delete(tmpFile.FullName)
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Shared Function GetMimeTypeByFileName(ByVal sFileName As String) As String
        Dim sMime As String = "application/octet-stream"

        Dim sExtension As String = System.IO.Path.GetExtension(sFileName)
        If Not String.IsNullOrEmpty(sExtension) Then
            sExtension = sExtension.Replace(".", "")
            sExtension = sExtension.ToLower()

            If sExtension = "xls" OrElse sExtension = "xlsx" Then
                sMime = "application/ms-excel"
            ElseIf sExtension = "doc" OrElse sExtension = "docx" Then
                sMime = "application/msword"
            ElseIf sExtension = "ppt" OrElse sExtension = "pptx" Then
                sMime = "application/ms-powerpoint"
            ElseIf sExtension = "rtf" Then
                sMime = "application/rtf"
            ElseIf sExtension = "zip" Then
                sMime = "application/zip"
            ElseIf sExtension = "mp3" Then
                sMime = "audio/mpeg"
            ElseIf sExtension = "bmp" Then
                sMime = "image/bmp"
            ElseIf sExtension = "gif" Then
                sMime = "image/gif"
            ElseIf sExtension = "jpg" OrElse sExtension = "jpeg" Then
                sMime = "image/jpeg"
            ElseIf sExtension = "png" Then
                sMime = "image/png"
            ElseIf sExtension = "tiff" OrElse sExtension = "tif" Then
                sMime = "image/tiff"
            ElseIf sExtension = "txt" Then
                sMime = "text/plain"
            End If
        End If

        Return sMime
    End Function

    Public Shared Sub ZipAllFiles(ByVal TempFileName As String, ByVal FilePathZip As String, ByVal FileNameZip As ArrayList)
        Try
            'Dim buffer As Byte() = New Byte(4095) {}
            ' the path on the server where the temp file will be created!
            'Dim tempFileName = "C:\Download\" & sPath & "\" & Guid.NewGuid().ToString() & ".zip"

            Dim filePath = [String].Empty
            Dim fileName = [String].Empty
            Dim iRow As Integer
            Dim iRows As Integer

            Dim MyZip As ZipFile
            MyZip = ZipFile.Create(TempFileName)
            With MyZip
                iRows = FileNameZip.Count - 1
                For iRow = 0 To iRows
                    filePath = FilePathZip & "\" & FileNameZip(iRow).ToString
                    fileName = FileNameZip(iRow).ToString
                    .BeginUpdate()
                    .Add(filePath, fileName)
                    .CommitUpdate()
                Next
            End With
            MyZip.Close()

            'Dim zipOutputStream = New ZipOutputStream(File.Create(TempFileName))
            'Dim filePath = [String].Empty
            'Dim fileName = [String].Empty
            'Dim readBytes = 0
            'Dim iRow As Integer
            'Dim iRows As Integer

            'iRows = FileNameZip.Count - 1
            'For iRow = 0 To iRows
            '    filePath = FilePathZip & "\" & FileNameZip(iRow).ToString
            '    fileName = FileNameZip(iRow).ToString

            '    Dim zipEntry = New ZipEntry(fileName)

            '    zipOutputStream.PutNextEntry(zipEntry)

            '    Using fs = File.OpenRead(filePath)
            '        Do
            '            readBytes = fs.Read(buffer, 0, buffer.Length)

            '            zipOutputStream.Write(buffer, 0, readBytes)
            '        Loop While readBytes > 0
            '    End Using
            'Next

            'zipOutputStream.Finish()
            'zipOutputStream.Close()

            ' delete the temp file 
            'If File.Exists(tempFileName) Then
            '    File.Delete(tempFileName)
            'End If
        Catch
            Throw
        End Try
    End Sub

    'Public Const BUFFER_SIZE As Long = 4096
    'Public Shared Sub AddFileToZip(ByVal zipFilename As String, ByVal fileToAdd As String)
    '    Using zip As Package = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate)
    '        Dim destFilename As String = ".\" & Path.GetFileName(fileToAdd)
    '        Dim uri As Uri = PackUriHelper.CreatePartUri(New Uri(destFilename, UriKind.Relative))
    '        If zip.PartExists(uri) Then
    '            zip.DeletePart(uri)
    '        End If
    '        Dim part As PackagePart = zip.CreatePart(uri, "", CompressionOption.Normal)
    '        Using fileStream As New FileStream(fileToAdd, FileMode.Open, FileAccess.Read)
    '            Using dest As Stream = part.GetStream()
    '                CopyStream(fileStream, dest)
    '            End Using
    '        End Using
    '    End Using
    'End Sub
    'Public Shared Sub CopyStream(ByVal inputStream As System.IO.FileStream, ByVal outputStream As System.IO.Stream)
    '    Dim bufferSize As Long = If(inputStream.Length < BUFFER_SIZE, inputStream.Length, BUFFER_SIZE)
    '    Dim buffer As Byte() = New Byte(bufferSize - 1) {}
    '    Dim bytesRead As Integer = 0
    '    Dim bytesWritten As Long = 0
    '    While (InlineAssignHelper(bytesRead, inputStream.Read(buffer, 0, buffer.Length))) <> 0
    '        outputStream.Write(buffer, 0, bytesRead)
    '        bytesWritten += bufferSize            
    '    End While
    'End Sub
    Public Shared Function ReplaceFile(ByVal filename As String) As String
        Try
            Dim Extension As String = Path.GetExtension(filename)
            Dim str As String = filename

            str = str.Replace(Extension, "")
            str = str.Replace(".", "")
            str = str.Replace(" ", "")
            str = str.Replace(",", "")
            str = str.Replace("'", "")
            str = str.Replace("~", "")
            str = str.Replace("(", "")
            str = str.Replace(")", "")
            str = str.Replace(">", "")
            str = str.Replace("<", "")
            str = str + Extension

            Return str
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub GetReportService(ByVal reportparam As String, ByVal path As String, ByVal filename As String, ByVal format As String)
        Dim sTargetURL As String
        Dim sProcess As String
        Dim strReportUser As String = "javasdev"
        Dim strReportUserPW As String = "p@ssw0rd0908"
        Dim strReportUserDomain As String = "JADEV"

        sTargetURL = "http://localhost/ReportServer?/ReportWeb/" + reportparam + "&rs:Command=Render&rs:ClearSession=true&rc:Toolbar=false&rs:Format=" + format
        Dim req As HttpWebRequest = DirectCast(WebRequest.Create(sTargetURL), HttpWebRequest)
        req.PreAuthenticate = True
        req.Credentials = New System.Net.NetworkCredential(strReportUser, strReportUserPW, strReportUserDomain)

        Dim HttpWResp As HttpWebResponse = DirectCast(req.GetResponse(), HttpWebResponse)
        Dim fStream As Stream = HttpWResp.GetResponseStream()

        Dim fileBytes As Byte() = ReadFully(fStream)
        'Save to local
        If format.ToLower = "excel" Then
            format = "xls"
        End If
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
        End If

        sProcess = path + filename
        If File.Exists(sProcess) Then
            File.Delete(sProcess)
        End If

        File.WriteAllBytes(sProcess, fileBytes) '-- if the file exists on the server 
        HttpWResp.Close()

    End Sub
    Public Shared Function ReadFully(input As Stream) As Byte()
        Using ms As New MemoryStream()
            input.CopyTo(ms)
            Return ms.ToArray()
        End Using
    End Function
    Public Shared Sub SendMailHtmlModule(ByVal Id As Integer, Optional ByVal FromAddr As String = "", Optional ByVal ToAddr As String = "", Optional ByVal CcAddr As String = "", Optional ByVal Subject As String = "", Optional ByVal Body As String = "", Optional ByVal FileAttachment As String = "")
        Dim msg As New System.Net.Mail.MailMessage
        Dim BCcAddr As String
        Dim SmtpServerName As String
        Dim ArTmp As ArrayList
        Dim sSQL As String
        Dim Params() As ParamSQL
        Dim ResultTrans As Integer
        Try
            SmtpServerName = ""
            BCcAddr = ""

            sSQL = "usp_Tbl_Email_Module"
            ReDim Params(0)
            Params(0) = New ParamSQL("@Id", Id)
            ArTmp = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
            If ArTmp.Count > 0 Then
                If FromAddr.Trim = "" Then
                    FromAddr = ArTmp(3).ToString.Trim
                End If
                If ToAddr.Trim = "" Then
                    ToAddr = ArTmp(4).ToString.Trim
                End If
                If CcAddr.Trim = "" Then
                    CcAddr = ArTmp(5).ToString.Trim
                End If
                BCcAddr = ArTmp(6).ToString.Trim
                If Subject.Trim = "" Then
                    Subject = ArTmp(7).ToString.Trim
                End If
                SmtpServerName = ArTmp(8).ToString.Trim
            End If

            With msg
                .To.Add(ToAddr.Replace(";", ","))
                If CcAddr.Trim <> "" Then
                    .CC.Add(CcAddr.Replace(";", ","))
                End If
                If BCcAddr.Trim <> "" Then
                    .Bcc.Add(BCcAddr.Replace(";", ","))
                End If
                .From = New System.Net.Mail.MailAddress(FromAddr)
                If FileAttachment.Trim <> "" Then
                    .Attachments.Add(New System.Net.Mail.Attachment(FileAttachment))
                End If
                .Subject = Subject
                .IsBodyHtml = True
                .Body = Body
                .Priority = Net.Mail.MailPriority.High
            End With

            Dim SmtpClient As New System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            With SmtpClient
                .UseDefaultCredentials = False
                .Credentials = New System.Net.NetworkCredential("hobiit978@gmail.com", "p@ssw0rd978")
                .DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
                .EnableSsl = True
                .Port = 587
                .Host = SmtpServerName
                .Send(msg)
            End With

        Catch ex As Exception
            sSQL = "usp_usp_log_error_email"
            ReDim Params(5)
            Params(0) = New ParamSQL("@pFROM", FromAddr)
            Params(1) = New ParamSQL("@pTO", ToAddr)
            Params(2) = New ParamSQL("@pCC", CcAddr)
            Params(3) = New ParamSQL("@pSUBJECT", Subject)
            Params(4) = New ParamSQL("@pERROR", ex.Message)
            Params(5) = New ParamSQL("@pOpt", "1")
            ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
            Throw
        End Try
    End Sub

    Public Shared Sub SendMailHtml(ByVal FromAddr As String, ByVal ToAddr As String, ByVal Subject As String, ByVal Body As String, ByVal SmtpServerName As String, Optional ByVal CcAddr As String = "", Optional ByVal BccAddr As String = "", Optional ByVal FileAttachment As String = "")
        Try
            Dim msg As New System.Net.Mail.MailMessage

            With msg
                .To.Add(ToAddr.Replace(";", ","))
                If CcAddr.Trim <> "" Then
                    .CC.Add(CcAddr.Replace(";", ","))
                End If
                If BccAddr.Trim <> "" Then
                    .Bcc.Add(BccAddr.Replace(";", ","))
                End If
                .From = New System.Net.Mail.MailAddress(FromAddr)
                If FileAttachment.Trim <> "" Then
                    .Attachments.Add(New System.Net.Mail.Attachment(FileAttachment))
                End If
                .Subject = Subject
                .IsBodyHtml = True
                .Body = Body
                .Priority = Net.Mail.MailPriority.High
            End With

            Dim SmtpClient As New System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            'With SmtpClient
            '    .UseDefaultCredentials = False
            '    .Credentials = New System.Net.NetworkCredential("hobiit978@gmail.com", "p@ssw0rd978")
            '    .DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
            '    .EnableSsl = True
            '    .Port = 587
            '    .Host = SmtpServerName
            '    .Send(msg)
            'End With

        Catch ex As Exception
            Dim sSQL As String
            Dim Params() As ParamSQL
            Dim ResultTrans As Integer
            sSQL = "usp_usp_log_error_email"
            ReDim Params(5)
            Params(0) = New ParamSQL("@pFROM", FromAddr)
            Params(1) = New ParamSQL("@pTO", ToAddr)
            Params(2) = New ParamSQL("@pCC", CcAddr)
            Params(3) = New ParamSQL("@pSUBJECT", Subject)
            Params(4) = New ParamSQL("@pERROR", ex.Message)
            Params(5) = New ParamSQL("@pOpt", "1")
            ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
            Throw
        End Try
    End Sub

    Public Shared Sub SendMailModule(ByVal Id As Integer, Optional ByVal ToAddr As String = "", Optional ByVal Subject As String = "", Optional ByVal Body As String = "", Optional ByVal FileAttachment As String = "")
        Dim msg As New System.Net.Mail.MailMessage
        Dim FromAddr As String
        Dim CcAddr As String
        Dim BCcAddr As String
        Dim SmtpServerName As String
        Dim ArTmp As ArrayList
        Dim sSQL As String
        Dim Params() As ParamSQL
        Dim ResultTrans As Integer
        Try
            sSQL = "usp_Tbl_Email_Module"
            ReDim Params(0)
            Params(0) = New ParamSQL("@Id", Id)
            ArTmp = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
            If ArTmp.Count > 0 Then
                FromAddr = ArTmp(3).ToString.Trim
                If ToAddr.Trim = "" Then
                    ToAddr = ArTmp(4).ToString.Trim
                End If
                CcAddr = ArTmp(5).ToString.Trim
                BCcAddr = ArTmp(6).ToString.Trim
                If Subject.Trim = "" Then
                    Subject = ArTmp(7).ToString.Trim
                End If
                SmtpServerName = ArTmp(8).ToString.Trim
            End If

            With msg
                .To.Add(ToAddr.Replace(";", ","))
                If CcAddr.Trim <> "" Then
                    .CC.Add(CcAddr.Replace(";", ","))
                End If
                If BCcAddr.Trim <> "" Then
                    .Bcc.Add(BCcAddr.Replace(";", ","))
                End If
                .From = New System.Net.Mail.MailAddress(FromAddr)
                '.Headers.Add("noreply@hanwhalife.co.id", True)
                If FileAttachment.Trim <> "" Then
                    .Attachments.Add(New System.Net.Mail.Attachment(FileAttachment))
                End If
                .Subject = Subject
                .IsBodyHtml = False
                .Body = Body
                .Priority = Net.Mail.MailPriority.High
            End With

            Dim SmtpClient As New System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            'With SmtpClient
            '    .UseDefaultCredentials = False
            '    .Credentials = New System.Net.NetworkCredential("noreplyhli@gmail.com", "p@ssw0rd2016")
            '    .DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
            '    .EnableSsl = True
            '    .Port = 587
            '    .Host = SmtpServerName
            '    .Send(msg)
            'End With
        Catch ex As Exception
            sSQL = "usp_usp_log_error_email"
            ReDim Params(5)
            Params(0) = New ParamSQL("@pFROM", FromAddr)
            Params(1) = New ParamSQL("@pTO", ToAddr)
            Params(2) = New ParamSQL("@pCC", CcAddr)
            Params(3) = New ParamSQL("@pSUBJECT", Subject)
            Params(4) = New ParamSQL("@pERROR", ex.Message)
            Params(5) = New ParamSQL("@pOpt", "1")
            ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
            Throw
        End Try
    End Sub

    Public Shared Sub SendMail(ByVal FromAddr As String, ByVal ToAddr As String, ByVal Subject As String, ByVal Body As String, ByVal SmtpServerName As String, Optional ByVal CcAddr As String = "", Optional ByVal BccAddr As String = "", Optional ByVal FileAttachment As String = "")
        Try
            Dim msg As New System.Net.Mail.MailMessage

            With msg
                .To.Add(ToAddr.Replace(";", ","))
                If CcAddr.Trim <> "" Then
                    .CC.Add(CcAddr.Replace(";", ","))
                End If
                If BccAddr.Trim <> "" Then
                    .Bcc.Add(BccAddr.Replace(";", ","))
                End If
                .From = New System.Net.Mail.MailAddress(FromAddr)
                If FileAttachment.Trim <> "" Then
                    .Attachments.Add(New System.Net.Mail.Attachment(FileAttachment))
                End If
                .Subject = Subject
                .IsBodyHtml = False
                .Body = Body
                .Priority = Net.Mail.MailPriority.High
            End With

            Dim SmtpClient As New System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            'With SmtpClient
            '    .UseDefaultCredentials = False
            '    .Credentials = New System.Net.NetworkCredential("hobiit978@gmail.com", "p@ssw0rd978")
            '    .DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
            '    .EnableSsl = True
            '    .Port = 587
            '    .Host = SmtpServerName
            '    .Send(msg)
            'End With
        Catch ex As Exception
            Dim sSQL As String
            Dim Params() As ParamSQL
            Dim ResultTrans As Integer
            sSQL = "usp_usp_log_error_email"
            ReDim Params(5)
            Params(0) = New ParamSQL("@pFROM", FromAddr)
            Params(1) = New ParamSQL("@pTO", ToAddr)
            Params(2) = New ParamSQL("@pCC", CcAddr)
            Params(3) = New ParamSQL("@pSUBJECT", Subject)
            Params(4) = New ParamSQL("@pERROR", ex.Message)
            Params(5) = New ParamSQL("@pOpt", "1")
            ResultTrans = DBTrans.WriteSingleTransaction(sSQL, DBConn.Connection, True, Params)
            Throw
        End Try
    End Sub

    Public Shared Sub ExportCrystalReportToPDF(ByVal ReportDoc As CrystalDecisions.CrystalReports.Engine.ReportDocument)
        Try
            Dim s As System.IO.MemoryStream = DirectCast(ReportDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat), MemoryStream)
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=" & "Report.pdf")
            HttpContext.Current.Response.BinaryWrite(s.ToArray())
            HttpContext.Current.Response.[End]()
        Catch
            Throw
        End Try
    End Sub

    Public Shared Function prepareReport(ByVal sReportName As String, Optional ByVal sSelectionFormula As String = "", Optional ByVal param As String = "") As ReportDocument
        Try
            Dim crSections As Sections
            Dim crReportDocument As ReportDocument, crSubreportDocument As ReportDocument
            Dim crSubreportObject As SubreportObject
            Dim crReportObjects As ReportObjects
            Dim crConnectionInfo As ConnectionInfo
            Dim crDatabase As Database
            Dim crTables As Tables
            Dim crTableLogOnInfo As TableLogOnInfo
            Dim strParamenters As String
            Dim strParValPair() As String
            Dim strVal() As String
            Dim index As Integer
            Dim paraValue As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim currValue As CrystalDecisions.Shared.ParameterValues
            Dim intCounter As Integer
            Dim cnn As New DBConn

            crReportDocument = New ReportDocument()
            crReportDocument.Load(System.Web.HttpContext.Current.Server.MapPath(sReportName), CrystalDecisions.[Shared].OpenReportMethod.OpenReportByTempCopy)

            intCounter = crReportDocument.DataDefinition.ParameterFields.Count
            If intCounter = 1 Then
                If InStr(crReportDocument.DataDefinition.ParameterFields(0).ParameterFieldName, ".", CompareMethod.Text) > 0 Then
                    intCounter = 0
                End If
            End If

            If intCounter > 0 And Trim(param) <> "" Then
                strParamenters = param
                strParValPair = strParamenters.Split(",")
                For index = 0 To UBound(strParValPair)
                    If InStr(strParValPair(index), "=") > 0 Then
                        strVal = strParValPair(index).Split("=")
                        If Left(strVal(1).Trim, 2) = "`-" Then
                            paraValue.Value = Security.DecryptString(Mid(strVal(1).Trim, 3))
                        ElseIf InStr(strVal(1).Trim, "~*~", 0) > 0 Then
                            paraValue.Value = strVal(1).Trim.Replace("~*~", "&")
                        Else
                            paraValue.Value = strVal(1)
                        End If
                        currValue = crReportDocument.DataDefinition.ParameterFields(strVal(0)).CurrentValues
                        currValue.Add(paraValue)
                        crReportDocument.DataDefinition.ParameterFields(strVal(0)).ApplyCurrentValues(currValue)
                    End If
                Next
            End If

            cnn.LoadConfiguration()

            crDatabase = crReportDocument.Database
            crTables = crDatabase.Tables
            crConnectionInfo = New ConnectionInfo()
            crConnectionInfo.ServerName = cnn.Server
            crConnectionInfo.DatabaseName = cnn.DataBase
            crConnectionInfo.UserID = cnn.User
            crConnectionInfo.Password = cnn.Password

            For Each aTable As CrystalDecisions.CrystalReports.Engine.Table In crTables
                crTableLogOnInfo = aTable.LogOnInfo
                crTableLogOnInfo.ConnectionInfo = crConnectionInfo
                aTable.ApplyLogOnInfo(crTableLogOnInfo)
            Next
            ' THIS STUFF HERE IS FOR REPORTS HAVING SUBREPORTS 
            ' set the sections object to the current report's section 

            crSections = crReportDocument.ReportDefinition.Sections
            ' loop through all the sections to find all the report objects 
            For Each crSection As Section In crSections
                crReportObjects = crSection.ReportObjects
                'loop through all the report objects in there to find all subreports 
                For Each crReportObject As ReportObject In crReportObjects
                    If crReportObject.Kind = ReportObjectKind.SubreportObject Then
                        crSubreportObject = DirectCast(crReportObject, SubreportObject)
                        'open the subreport object and logon as for the general report 
                        crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName)
                        crDatabase = crSubreportDocument.Database
                        crTables = crDatabase.Tables
                        For Each aTable As CrystalDecisions.CrystalReports.Engine.Table In crTables
                            crTableLogOnInfo = aTable.LogOnInfo
                            crTableLogOnInfo.ConnectionInfo = crConnectionInfo
                            aTable.ApplyLogOnInfo(crTableLogOnInfo)
                        Next
                    End If
                Next
            Next

            Return crReportDocument

        Catch
            Throw
        End Try

    End Function

    Public Shared Sub ExportCrystalReportToRTF(ByVal ReportDoc As CrystalDecisions.CrystalReports.Engine.ReportDocument)
        Try
            Dim s As System.IO.MemoryStream = DirectCast(ReportDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat), MemoryStream)
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ContentType = "application/rtf"
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=" & "Report.rtf")
            HttpContext.Current.Response.BinaryWrite(s.ToArray())
            HttpContext.Current.Response.[End]()
        Catch
            Throw
        End Try
    End Sub

    Public Shared Sub ExportCrystalReportToMsOffice(ByVal ReportDoc As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal sTempFileName As String, ByVal sOpt As String)
        Try
            Dim ContentType As String = ""
            Dim FileOption As New DiskFileDestinationOptions
            Dim DirPath As String

            FileOption.DiskFileName = sTempFileName

            DirPath = "C:\Download\Temp\"

            If Not Directory.Exists(DirPath) Then
                ' Create the directory.
                Directory.CreateDirectory(DirPath)
            End If

            Dim Options As New ExportOptions

            Options.ExportDestinationOptions = FileOption
            Options.ExportDestinationType = ExportDestinationType.NoDestination

            Select Case sOpt
                Case "1" 'Excel
                    Options.ExportFormatType = ExportFormatType.ExcelRecord
                    ReportDoc.ExportToDisk(ExportFormatType.ExcelRecord, DirPath & sTempFileName)
                    ContentType = "application/vnd.ms-excel"
                Case "2" 'Word
                    Options.ExportFormatType = ExportFormatType.WordForWindows
                    ReportDoc.ExportToDisk(ExportFormatType.WordForWindows, DirPath & sTempFileName)
                    ContentType = "application/msword"
            End Select

            'Write the file to the web page
            System.Web.HttpContext.Current.Response.ClearContent()
            System.Web.HttpContext.Current.Response.ClearHeaders()
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & sTempFileName)
            System.Web.HttpContext.Current.Response.ContentType = ContentType
            System.Web.HttpContext.Current.Response.WriteFile(DirPath & sTempFileName)

            'Clean up the response object
            System.Web.HttpContext.Current.Response.Flush()
            System.Web.HttpContext.Current.Response.Close()

            'IMPORTANT do the garbage collection
            ReportDoc.Dispose()
            FileOption = Nothing
            Options = Nothing

            System.IO.File.Delete(DirPath & sTempFileName)

        Catch
            Throw
        End Try
    End Sub

    Public Shared Function HitungPPN(ByVal value As Double) As Double
        Try
            Dim dblPajak As Double
            Dim Ar As ArrayList
            Dim sSQL As String
            Dim Params() As ParamSQL

            sSQL = "usp_TblTaxrate_Find"
            ReDim Params(0)
            Params(0) = New ParamSQL("@Opt", "1")
            Ar = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
            If Ar.Count > 0 Then
                'sSQL = "Select * From Tbl_Tax_rate Where Source = '2'  And TaxCode = 'PPN' Order By Seq"
                'avData = GetMatrix(cnn, sSQL)
                dblPajak = dblPajak + (value * (Ar(4) / 100))
                HitungPPN = dblPajak
            End If
        Catch
            Throw
        End Try
    End Function

    Public Shared Function HitungPPH23(ByVal value As Double) As Double
        Try
            Dim dblPajak As Double
            Dim Ar As ArrayList
            Dim sSQL As String
            Dim Params() As ParamSQL

            sSQL = "usp_TblTaxrate_Find"
            ReDim Params(0)
            Params(0) = New ParamSQL("@Opt", "2")
            Ar = DBTrans.ReadSingleRecord(sSQL, DBConn.Connection, True, Params)
            If Ar.Count > 0 Then
                'sSQL = "Select * From Tbl_Tax_rate Where Source = '2' And TaxCode = 'PPH23' Order By Seq"
                'avData = GetMatrix(cnn, sSQL)
                dblPajak = dblPajak + (value * (Ar(4) / 100))
                HitungPPH23 = dblPajak
            End If
        Catch
            Throw
        End Try
    End Function

    Public Shared Function HitungPajakProgresif(ByVal OldValue As Double, ByVal CurrentValue As Double) As Double
        Try
            Dim iRow As Integer
            Dim iRows As Integer
            Dim dblSisa As Double
            Dim dblPajak As Double
            Dim bAkhir As Boolean
            Dim bFirstCalc As Boolean
            Dim dblNilaiAkhir As Double
            Dim lAwal As Long
            Dim Ar As ArrayList
            Dim sSQL As String
            Dim Params() As ParamSQL

            If OldValue = 0 Then 'Jika tidak terdapat pembayaran PPH di bulan yang sama
                dblSisa = Abs(CurrentValue)
            Else 'Jika terdapat pembayaran PPH di bulan yang sama
                dblSisa = Abs(OldValue)
            End If
            bAkhir = False
            sSQL = "usp_TblTaxrate_Find"
            ReDim Params(0)
            Params(0) = New ParamSQL("@Opt", "3")
            Ar = DBTrans.ReadRecordsToArrayList(sSQL, DBConn.Connection, True, Params)
            If Ar.Count > 0 Then
                'sSQL = "Select * From Tbl_Tax_rate Where Source = '1' And TaxCode = 'PPH21' Order By Seq"
                'avData = GetMatrix(cnn, sSQL)
                'iRows = UBound(avData, 2)
                iRows = Ar.Count - 1
                For iRow = 0 To iRows
                    If iRow = iRows Then
                        dblPajak = dblPajak + (dblSisa * (Ar(iRow)(4) / 100))
                        bAkhir = True
                    Else
                        If dblSisa > Ar(iRow)(3) Then
                            dblPajak = dblPajak + (Ar(iRow)(3) * (Ar(iRow)(4) / 100))
                        ElseIf dblSisa <= Ar(iRow)(3) Then
                            dblPajak = dblPajak + (dblSisa * (Ar(iRow)(4) / 100))
                            Exit For
                        End If
                    End If
                    dblSisa = dblSisa - Ar(iRow)(3)
                Next


                If OldValue = 0 Then
                    HitungPajakProgresif = dblPajak
                Else
                    If bAkhir Then
                        dblPajak = dblPajak + (CurrentValue * (Ar(iRow)(4) / 100))
                    Else
                        dblNilaiAkhir = dblSisa
                        dblSisa = 0
                        lAwal = iRow
                        bFirstCalc = True
                        dblPajak = 0
                        dblSisa = Abs(CurrentValue)
                        For iRow = lAwal To iRows
                            If iRow = iRows Then
                                dblPajak = dblPajak + (dblSisa * (Ar(iRow)(4) / 100))
                            Else
                                If bFirstCalc Then
                                    If dblNilaiAkhir + dblSisa > Ar(iRow)(3) Then
                                        dblPajak = dblPajak + ((Ar(iRow)(3) - dblNilaiAkhir) * (Ar(iRow)(4) / 100))
                                    ElseIf dblNilaiAkhir + dblSisa <= Ar(iRow)(3) Then
                                        dblPajak = dblPajak + (dblSisa * (Ar(iRow)(4) / 100))
                                        Exit For
                                    End If
                                    bFirstCalc = False
                                    dblSisa = dblSisa - (Ar(iRow)(3) - dblNilaiAkhir)
                                Else
                                    If dblSisa > Ar(iRow)(3) Then
                                        dblPajak = dblPajak + (Ar(iRow)(3) * (Ar(iRow)(4) / 100))
                                    ElseIf dblSisa <= Ar(iRow)(3) Then
                                        dblPajak = dblPajak + (dblSisa * (Ar(iRow)(4) / 100))
                                        Exit For
                                    End If
                                    dblSisa = dblSisa - Ar(iRow)(3)
                                End If
                            End If
                        Next
                    End If
                    HitungPajakProgresif = dblPajak
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    Public Shared Function HitungPajakProgresifAdj(ByVal OldValue As Double, ByVal CurrentValue As Double) As Double
        Try
            Dim iRow As Integer
            Dim iRows As Integer
            Dim dblSisa As Double
            Dim dblPajak As Double
            Dim bAkhir As Boolean
            Dim bFirstCalc As Boolean
            Dim dblNilaiAkhir As Double
            Dim lAwal As Long
            Dim Ar As ArrayList
            Dim sSQL As String
            Dim Params() As ParamSQL

            If OldValue = 0 Then 'Jika tidak terdapat pembayaran PPH di bulan yang sama
                dblSisa = Abs(CurrentValue)
            Else 'Jika terdapat pembayaran PPH di bulan yang sama
                dblSisa = Abs(OldValue)
            End If
            bAkhir = False
            sSQL = "usp_TblTaxrate_Find"
            ReDim Params(0)
            Params(0) = New ParamSQL("@Opt", "4")
            Ar = DBTrans.ReadRecordsToArrayList(sSQL, DBConn.Connection, True, Params)
            If Ar.Count > 0 Then
                'sSQL = "Select * From Tbl_Tax_rate Where Source = '1' And TaxCode = 'PPH21' Order By Seq"
                'avData = GetMatrix(cnn, sSQL)
                'iRows = UBound(avData, 2)
                iRows = Ar.Count - 1
                For iRow = 0 To iRows
                    If iRow = iRows Then
                        dblPajak = dblPajak + (dblSisa * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                        bAkhir = True
                    Else
                        If dblSisa > Ar(iRow)(3) Then
                            dblPajak = dblPajak + (Ar(iRow)(3) * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                        ElseIf dblSisa <= Ar(iRow)(3) Then
                            dblPajak = dblPajak + (dblSisa * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                            Exit For
                        End If
                    End If
                    dblSisa = dblSisa - Ar(iRow)(3)
                Next


                If OldValue = 0 Then
                    HitungPajakProgresifAdj = dblPajak
                Else
                    If bAkhir Then
                        dblPajak = dblPajak + (CurrentValue * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                    Else
                        dblNilaiAkhir = dblSisa
                        dblSisa = 0
                        lAwal = iRow
                        bFirstCalc = True
                        dblPajak = 0
                        dblSisa = Abs(CurrentValue)
                        For iRow = lAwal To iRows
                            If iRow = iRows Then
                                dblPajak = dblPajak + (dblSisa * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                            Else
                                If bFirstCalc Then
                                    If dblNilaiAkhir + dblSisa > Ar(iRow)(3) Then
                                        dblPajak = dblPajak + ((Ar(iRow)(3) - dblNilaiAkhir) * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                                    ElseIf dblNilaiAkhir + dblSisa <= Ar(iRow)(3) Then
                                        dblPajak = dblPajak + (dblSisa * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                                        Exit For
                                    End If
                                    bFirstCalc = False
                                    dblSisa = dblSisa - (Ar(iRow)(3) - dblNilaiAkhir)
                                Else
                                    If dblSisa > Ar(iRow)(3) Then
                                        dblPajak = dblPajak + (Ar(iRow)(3) * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                                    ElseIf dblSisa <= Ar(iRow)(3) Then
                                        dblPajak = dblPajak + (dblSisa * ((Ar(iRow)(4) / 100) + ((Ar(iRow)(4) / 100) * (Ar(iRow)(6) / 100))))
                                        Exit For
                                    End If
                                    dblSisa = dblSisa - Ar(iRow)(3)
                                End If
                            End If
                        Next
                    End If
                    HitungPajakProgresifAdj = dblPajak
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    Public Shared Function GetJson(ByVal dt As DataTable) As String
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        serializer.MaxJsonLength = Int32.MaxValue

        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object) = Nothing
        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)()
            For Each dc As DataColumn In dt.Columns
                row.Add(dc.ColumnName.Trim(), dr(dc))
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function
End Class

Public Class ComboText
    Public Shared Sub SetComboText(ByVal ComboBoxToSet As Object, ByVal sText As Object)
        Try
            Dim iFound As Integer
            iFound = ComboBoxToSet.FindStringExact(sText)
            ComboBoxToSet.SelectedIndex = iFound
            If iFound = -1 Then ComboBoxToSet.Text = ""
        Catch
            Throw
        End Try
    End Sub
End Class



