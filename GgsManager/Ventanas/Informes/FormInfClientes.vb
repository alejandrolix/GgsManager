Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class FormInfClientes

    Private IdGarajeSelec As Integer

    Private Sub FormInfClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)

        Dim conexion As New MySqlConnection(My.Settings.ConexionABd)
        conexion.Open()

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

    Public Sub New(idGarajeSelec As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGarajeSelec

    End Sub

End Class