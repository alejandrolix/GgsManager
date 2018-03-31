Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class FormInfClientes

    Private IdGarajeSelec As Integer

    Private Sub FormInfClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)

        Dim conexion As New MySqlConnection(My.Settings.ConexionABd)
        conexion.Open()

        Dim adaptador As New MySqlDataAdapter("SELECT Cli.IdCliente, Cli.Nombre, Cli.DNI, Cli.Movil, Cli.Observaciones
                                               FROM   Clientes Cli
	                                                  JOIN Vehiculos Veh ON Veh.IdCliente = Cli.IdCliente
                                               WHERE  Veh.IdGaraje = @IdGaraje;", conexion)

        adaptador.SelectCommand.Parameters.AddWithValue("@IdGaraje", IdGarajeSelec)
        Dim dtClientes As New DtClientes()

        adaptador.Fill(dtClientes, "Clientes")
        conexion.Close()

        EstablecerNombreGaraje()

        ReportViewer.ProcessingMode = ProcessingMode.Local
        ReportViewer.LocalReport.DataSources.Clear()
        ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtClientes", dtClientes.Tables("Clientes")))

        ReportViewer.DocumentMapCollapsed = True
        ReportViewer.RefreshReport()

    End Sub


    ''' <summary>
    ''' Establece el nombre del garaje al parámetro correspondiente.
    ''' </summary>
    Private Sub EstablecerNombreGaraje()

        Dim nombreGaraje As String = Garaje.ObtenerNombreGarajePorId(IdGarajeSelec)
        ReportViewer.LocalReport.SetParameters(New ReportParameter("NombreGaraje", nombreGaraje))

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class