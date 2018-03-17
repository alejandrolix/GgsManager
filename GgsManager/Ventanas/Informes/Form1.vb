Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim adaptador As New MySqlDataAdapter("SELECT Nombre, ROUND((NumPlazas * NumPlazasOcupadas) / 100) AS 'Porcentaje'
                                               FROM   Garajes;", conexion)

        Dim dtPorcPlazasOcupadas As New DtPorcPlazas()
        adaptador.Fill(dtPorcPlazasOcupadas, "Garajes")

        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DtPorcPlazasOcupadas", dtPorcPlazasOcupadas.Tables("Garajes")))

        Dim adaptador1 As New MySqlDataAdapter("SELECT Nombre, ROUND((NumPlazas * NumPlazasLibres) / 100) AS 'Porcentaje'
                                               FROM   Garajes;", conexion)

        Dim dtPorcPlazasLibres As New DtPorcPlazas()
        adaptador1.Fill(dtPorcPlazasLibres, "Garajes")
        conexion.Close()

        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DtPorcPlazasLibres", dtPorcPlazasLibres.Tables("Garajes")))

        ReportViewer1.DocumentMapCollapsed = True
        ReportViewer1.RefreshReport()

    End Sub

End Class