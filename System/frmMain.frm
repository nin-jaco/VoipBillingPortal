VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "Form1"
   ClientHeight    =   5055
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   8910
   LinkTopic       =   "Form1"
   ScaleHeight     =   5055
   ScaleWidth      =   8910
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdTransferData 
      Caption         =   "Transfer CDR Data To SQL Server"
      Height          =   375
      Left            =   3120
      TabIndex        =   1
      Top             =   1680
      Width           =   2655
   End
   Begin VB.CommandButton cmdImport 
      Caption         =   "Import"
      Height          =   375
      Left            =   3000
      TabIndex        =   0
      Top             =   240
      Width           =   2535
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const giNATCostPerMin As Double = 0.5
Private Const giJHBCostPerMin As Double = 0.4

Private Sub cmdImport_Click()
  Dim strSql As String
  Dim rsKey As New ADODB.Recordset
  Dim cnMySQL As ADODB.Connection
  Dim dConCost As Double
  Dim sCityCodes As String
  On Error GoTo ErrHandler
  Set cnMySQL = New ADODB.Connection
  cnMySQL.ConnectionTimeout = 30
  cnMySQL.Open "DRIVER={MySQL ODBC 5.1 Driver};SERVER=196.31.142.211;DATABASE=asteriskcdrdb;UID=reports;PWD=Qcs0lut1onsscr!ba99!;PORT=3306;"
  
  strSql = "Select rawBillingID, Destination, Prefix, CostPerMin, CityCode From rawCost Order By rawBillingID"
  rsKey.Open strSql, cnMySQL
  If Not rsKey.EOF Then
    'Delete Cost Table
    cnMySQL.Execute "Delete From Cost"
    'Insert Records
    While Not rsKey.EOF
      sCityCodes = Trim$(Replace$(rsKey!CityCode, Chr(13), ""))
      If Len(sCityCodes) = 0 Then
        dConCost = rsKey!CostPerMin
      Else
        While InStr(1, sCityCodes, "|") > 0
          strSql = "Insert Into Cost(Destination, Prefix, CityCode, ConnectionCost, CostPerMin, CostPerSec)"
          strSql = strSql & " Values('" & Replace$(rsKey!Destination, "'", "''") & "', '" & Trim$(rsKey!Prefix) & "', '" & Trim$(Left$(sCityCodes, InStr(1, sCityCodes, "|") - 1)) & "', " & dConCost & ", " & CDbl(rsKey!CostPerMin) & ", " & rsKey!CostPerMin / 60 & ")"
          cnMySQL.Execute strSql
          sCityCodes = Trim$(Right$(sCityCodes, Len(sCityCodes) - InStr(1, sCityCodes, "|")))
        Wend
        If InStr(1, sCityCodes, "|") = 0 Then
          strSql = "Insert Into Cost(Destination, Prefix, CityCode, ConnectionCost, CostPerMin, CostPerSec)"
          strSql = strSql & " Values('" & Replace$(rsKey!Destination, "'", "''") & "', '" & Trim$(rsKey!Prefix) & "', '" & Trim$(sCityCodes) & "', " & dConCost & ", " & CDbl(rsKey!CostPerMin) & ", " & rsKey!CostPerMin / 60 & ")"
          cnMySQL.Execute strSql
        End If
      End If
      rsKey.MoveNext
    Wend
    rsKey.Close
    'Delete CostLocal Table
    cnMySQL.Execute "Delete From CostLocal"
    'Copy and change local Cost
    strSql = "Select Destination, Prefix, CityCode, ConnectionCost, CostPerMin, CostPerSec From Cost Where Prefix = '27'"
    rsKey.Open strSql, cnMySQL
    While Not rsKey.EOF
      If InStr(1, rsKey!Destination, "Mobile") > 0 Or InStr(1, rsKey!Destination, "Johannes") > 0 Then
        strSql = "Insert Into CostLocal(Destination, Prefix, CityCode, ConnectionCost, CostPerMin, CostPerSec)"
        strSql = strSql & " Values('" & Replace$(rsKey!Destination, "'", "''") & "', '0', '" & Trim$(rsKey!CityCode) & "', 0, " & rsKey!CostPerMin & ", " & rsKey!CostPerSec & ")"
        cnMySQL.Execute strSql
      End If
      rsKey.MoveNext
    Wend
    rsKey.Close
    'Insert Telkom Hardcoded Values
    cnMySQL.Execute "Update CostLocal Set Destination = 'Telkom-Johannesburg', CostPerMin = " & giJHBCostPerMin & ", CostPerSec = " & giJHBCostPerMin / 60 & " where Destination = 'SOUTH AFRICA-Johannesburg'"
    strSql = "Insert Into CostLocal(Destination, Prefix, CityCode, ConnectionCost, CostPerMin, CostPerSec)"
    strSql = strSql & " Values('Telkom-National', '0', '*', 0, " & giNATCostPerMin & ", " & giNATCostPerMin / 60 & ")"
    cnMySQL.Execute strSql
    'Remove Local Cost
    cnMySQL.Execute "Delete From Cost Where Prefix = '27'"
  End If
  Set rsKey = Nothing
  cnMySQL.Close
  Set cnMySQL = Nothing
  Exit Sub
ErrHandler:

End Sub

Private Sub cmdTransferData_Click()
  Dim strSql As String
  Dim rsKey As New ADODB.Recordset
  Dim cnMySQL As ADODB.Connection
  Dim cnSQLServer As ADODB.Connection
  'On Error GoTo ErrHandler
  
  Set cnMySQL = New ADODB.Connection
  cnMySQL.ConnectionTimeout = 30
  cnMySQL.Open "DRIVER={MySQL ODBC 5.1 Driver};SERVER=196.31.142.211;DATABASE=asteriskcdrdb;UID=reports;PWD=Qcs0lut1onsscr!ba99!;PORT=3306;"

  Set cnSQLServer = New ADODB.Connection
  cnSQLServer.Provider = "sqloledb"
  cnSQLServer.Open "server=(Local);database=QCAstBilling;", "sa", "scr!ba99"
  
  strSql = "Select calldate, clid, src, dst, duration, billsec, accountcode, dcontext, channel, dstchannel From cdr WHERE disposition = 'answered' And dst <> 's' And length(src) < 5 And length(dst) > 5;"
  Set rsKey = New ADODB.Recordset
  rsKey.Open strSql, cnMySQL
  While Not rsKey.EOF
    strSql = "Insert Into Billing(calldate, clid, src, dst, duration, billsec, accountcode, dcontext, channel, dstchannel)"
    strSql = strSql & " Values('" & rsKey!calldate & "', '" & rsKey!clid & "', '" & rsKey!src & "', '" & rsKey!dst & "',"
    strSql = strSql & " " & rsKey!duration & ", " & rsKey!billsec & ", '" & rsKey!accountcode & "', '" & rsKey!dcontext & "',"
    strSql = strSql & " '" & rsKey!channel & "', '" & rsKey!dstchannel & "')"
    cnSQLServer.Execute strSql
    rsKey.MoveNext
  Wend
  rsKey.Close
  Set rsKey = Nothing
  cnMySQL.Close
  Set cnMySQL = Nothing
  cnSQLServer.Close
  Set cnSQLServer = Nothing
End Sub
