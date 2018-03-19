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

            Dim conexion As MySqlConnection = Foo.ConexionABd()
            Dim dtPlazas As New DtPlazas()

            Me.Adaptador = New MySqlDataAdapter(String.Format("SELECT Plz.IdPlaza, CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Cliente', Veh.Matricula, Veh.Marca, Veh.Modelo, Veh.PrecioTotal
                                                               FROM   Plazas Plz
                                                                      JOIN Vehiculos Veh ON Veh.IdPlaza = Plz.IdPlaza
                                                                      JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente       
                                                               WHERE  Plz.IdGaraje = {0} AND Plz.IdSituacion = (
												                                                                SELECT IdSituacion
                                                                                                                FROM   Situaciones
                                                                                                                WHERE  Tipo = 'Libre');", IdGarajeSelec), conexion)
            Adaptador.Fill(dtPlazas, "Plazas")
            conexion.Close()

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub

    Private Sub PlOcupadaRb_CheckedChanged(sender As Object, e As EventArgs) Handles PlOcupadaRb.CheckedChanged

        If PlOcupadaRb.Checked Then

            Dim conexion As MySqlConnection = Foo.ConexionABd()
            Dim dtPlazas As New DtPlazas()

            Me.Adaptador = New MySqlDataAdapter(String.Format("SELECT Plz.IdPlaza, CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Cliente', Veh.Matricula, Veh.Marca, Veh.Modelo, Veh.PrecioTotal
                                                               FROM   Plazas Plz
                                                                      JOIN Vehiculos Veh ON Veh.IdPlaza = Plz.IdPlaza
                                                                      JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente       
                                                               WHERE  Plz.IdGaraje = {0} AND Plz.IdSituacion = (
												                                                                SELECT IdSituacion
                                                                                                                FROM   Situaciones
                                                                                                                WHERE  Tipo = 'Ocupada');", IdGarajeSelec), conexion)
            Adaptador.Fill(dtPlazas, "Plazas")
            conexion.Close()

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub

    Private Sub PlTodasRb_CheckedChanged(sender As Object, e As EventArgs) Handles PlTodasRb.CheckedChanged

        If PlTodasRb.Checked Then

            Dim conexion As MySqlConnection = Foo.ConexionABd()
            Dim dtPlazas As New DtPlazas()

            Me.Adaptador = New MySqlDataAdapter(String.Format("SELECT Plz.IdPlaza, CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Cliente', Veh.Matricula, Veh.Marca, Veh.Modelo, Veh.PrecioTotal
                                                               FROM   Plazas Plz
                                                                      JOIN Vehiculos Veh ON Veh.IdPlaza = Plz.IdPlaza
                                                                      JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente       
                                                               WHERE  Plz.IdGaraje = {0} AND Plz.IdSituacion IN (
												                                                                 SELECT IdSituacion
                                                                                                                 FROM   Situaciones);", IdGarajeSelec), conexion)
            Adaptador.Fill(dtPlazas, "Plazas")
            conexion.Close()

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub

    Public Sub New(idGarajeSelec As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGarajeSelec

    End Sub

End Class