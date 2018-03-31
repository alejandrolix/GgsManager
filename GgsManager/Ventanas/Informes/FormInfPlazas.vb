Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class FormInfPlazas

    Private IdGarajeSelec As Integer
    Private Adaptador As MySqlDataAdapter

    Private Sub FormInfPlazas_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)
        PlTodasRb.Checked = True            ' Lanzará el evento "CheckedChanged".

    End Sub

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs) Handles ReportViewer.Load

        Dim nombreGaraje As String = Garaje.ObtenerNombreGarajePorId(IdGarajeSelec)
        ReportViewer.LocalReport.SetParameters(New ReportParameter("NombreGaraje", nombreGaraje))

    End Sub

    Private Sub PlLibreRb_CheckedChanged(sender As Object, e As EventArgs) Handles PlLibreRb.CheckedChanged

        If PlLibreRb.Checked Then

            Dim dtPlazas As DtPlazas = Plaza.ObtenerDatosPlazasLibresPorIdGaraje(IdGarajeSelec)

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub

    Private Sub PlOcupadaRb_CheckedChanged(sender As Object, e As EventArgs) Handles PlOcupadaRb.CheckedChanged

        If PlOcupadaRb.Checked Then

            Dim dtPlazas As DtPlazas = Plaza.ObtenerDatosPlazasOcupadasPorIdGaraje(IdGarajeSelec)

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub

    Private Sub PlTodasRb_CheckedChanged(sender As Object, e As EventArgs) Handles PlTodasRb.CheckedChanged

        If PlTodasRb.Checked Then

            Dim dtPlazas As DtPlazas = Plaza.ObtenerDatosTodasPlazasPorIdGaraje(IdGarajeSelec)

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class