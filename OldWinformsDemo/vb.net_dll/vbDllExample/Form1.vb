Option Explicit On
Imports System
Imports System.IO.Ports
Imports System.Threading.Thread
Imports System.Runtime.InteropServices

Public Class Form1

    Declare Function OpenComPort Lib "s30c.dll" (ByVal ComPort As Integer, ByVal Timeout As Int16) As Int16
    Declare Function CloseComPort Lib "s30c.dll" () As Int16
    Declare Function F48 Lib "s30c.dll" (ByVal DevAddr As Byte, ByRef bteClass As Byte, ByRef btegroup As Byte, ByRef bteyear As Byte, ByRef bteweek As Byte, ByRef btebuffer As Byte, ByRef btestate As Byte) As Int16
    Declare Function F69 Lib "s30c.dll" (ByVal DevAddr As Byte, ByRef sn As Long) As Int16
    Declare Function F73 Lib "s30c.dll" (ByVal DevAddr As Byte, ByVal Kanal As Byte, ByRef PresVal As Single, ByRef State As Byte) As Integer


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        'OpenComPort 
        'Initialise and release
        'Open Serial Port

        'OpenComPort(3, 500)

        Dim reply As Int16 = -1

        reply = OpenComPort(Integer.Parse(TextBox2.Text), 500)

        If reply = -1 Then

            MsgBox("Unable to establish Connection")

        End If

        'Initialise(Sensor)
        Dim dClass As Byte = 0
        Dim dGroup As Byte = 0
        Dim dYear As Byte = 0
        Dim dWeek As Byte = 0
        Dim dBuf As Byte = 0
        Dim dStat As Byte = 0

        'F48(250, dClass, dGroup, dWeek, dYear, dBuf, dStat)
        'Sleep(100)

        Dim Feedback As Int16 = 0

        Feedback = F48(250, dClass, dGroup, dYear, dWeek, dBuf, dStat)

        TextBox1.Clear()
        TextBox1.Text += "Return" + vbTab + Feedback.ToString() + vbCrLf
        TextBox1.Text += "Class" + vbTab + dClass.ToString() + vbCrLf
        TextBox1.Text += "Group" + vbTab + dGroup.ToString() + vbCrLf
        TextBox1.Text += "Year" + vbTab + dYear.ToString() + vbCrLf
        TextBox1.Text += "Week" + vbTab + dWeek.ToString() + vbCrLf
        TextBox1.Text += "Buf" + vbTab + dBuf.ToString() + vbCrLf
        TextBox1.Text += "Status" + vbTab + dStat.ToString() + vbCrLf + vbCrLf

        Dim sn As Integer = 0
        F69(250, sn)
        TextBox1.Text += "SN" + vbTab + sn.ToString() + vbCrLf + vbCrLf

        Dim value As Single = 0
        F73(250, 1, value, dStat)
        TextBox1.Text += "Value P1" + vbTab + value.ToString() + vbCrLf + vbCrLf


        CloseComPort()
    End Sub
End Class
