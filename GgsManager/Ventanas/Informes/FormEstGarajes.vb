Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms

Public Class FormEstGarajes

    Private SonTodosGarajes As Boolean
    Private IdGarajeSelec As Integer

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs) Handles ReportViewer.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)

        Dim conexion As New MySqlConnection(My.Settings.ConexionABd)
        conexion.Open()

        Dim adaptador As MySqlDataAdapter

        If SonTodosGarajes Then

            adaptador = New MySqlDataAdapter("SELECT Nombre AS 'NombreGaraje', NumPlazas AS 'NumeroPlazas', TRUNCATE((NumPlazasLibres * NumPlazas) / 100, 0) AS 'PorcentajePlazasLibres', TRUNCATE((NumPlazasOcupadas * NumPlazas) / 100, 0) AS 'PorcentajePlazasOcupadas'
                                              FROM   Garajes;", conexion)
        Else

            adaptador = New MySqlDataAdapter(String.Format("SELECT Nombre AS 'NombreGaraje', NumPlazas AS 'NumeroPlazas', TRUNCATE((NumPlazasLibres * NumPlazas) / 100, 0) AS 'PorcentajePlazasLibres', TRUNCATE((NumPlazasOcupadas * NumPlazas) / 100, 0) AS 'PorcentajePlazasOcupadas'
                                                            FROM   Garajes
                                                            WHERE  IdGaraje = {0}", IdGarajeSelec), conexion)
        End If

        EstablecerTituloInforme()

        Dim dtPorcGaraje As New DtClientes()

        adaptador.Fill(dtPorcGaraje, "Estadisticas")
        conexion.Close()

        ReportViewer.ProcessingMode = ProcessingMode.Local
        ReportViewer.LocalReport.DataSources.Clear()
        ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPorcGaraje", dtPorcGaraje.Tables("Estadisticas")))

        ReportViewer.DocumentMapCollapsed = True
        ReportViewer.RefreshReport()

    End Sub


    ''' <summary>
    ''' Establece el título del informe.
    ''' </summary>
    Private Sub EstablecerTituloInforme()

        If SonTodosGarajes Then

            Dim rpTituloInf As New ReportParameter("TituloInforme", "Estadísticas de Todos los Garajes")
            ReportViewer.LocalReport.SetParameters(rpTituloInf)
        Else

            Dim nombreGaraje As String = Garaje.ObtenerNombreGarajePorId(IdGarajeSelec)
            Dim rpTituloInf As New ReportParameter("TituloInforme", "Estadísticas del garaje de " & nombreGaraje)

            ReportViewer.LocalReport.SetParameters(rpTituloInf)

        End If

    End Sub

    Public Sub New(sonTodosGarajes As Boolean)

        InitializeComponent()
        Me.SonTodosGarajes = sonTodosGarajes

    End Sub

    Public Sub New(sonTodosGarajes As Boolean, idGaraje As Integer)

        InitializeComponent()

        Me.SonTodosGarajes = sonTodosGarajes
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class