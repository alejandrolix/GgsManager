﻿Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class FormInfClientes

    Private IdGarajeSelec As Integer

    Private Sub FormInfClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs) Handles ReportViewer.Load

        Dim conexion As MySqlConnection = Foo.ConexionABd()

        Dim adaptador As New MySqlDataAdapter(String.Format("SELECT Cli.IdCliente, Cli.Nombre, Cli.DNI, Cli.Movil, Cli.Observaciones
                                                             FROM   Clientes Cli
	                                                                JOIN Vehiculos Veh ON Veh.IdCliente = Cli.IdCliente
                                                             WHERE  Veh.IdGaraje = {0};", IdGarajeSelec), conexion)
        Dim dtClientes As New DtClientes()

        adaptador.Fill(dtClientes, "Clientes")
        conexion.Close()

        ReportViewer.ProcessingMode = ProcessingMode.Local
        ReportViewer.LocalReport.DataSources.Clear()
        ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtClientes", dtClientes.Tables("Clientes")))

        ReportViewer.DocumentMapCollapsed = True
        ReportViewer.RefreshReport()

    End Sub
End Class