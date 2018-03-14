Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class FormInfClientes

    Private IdGarajeSelec As Integer

    Private Sub FormInfClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)
        ' Terminar.

        Me.ReportViewer.RefreshReport()

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class