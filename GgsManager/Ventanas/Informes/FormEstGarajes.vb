Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms

Public Class FormEstGarajes

    Private SonTodosGarajes As Boolean
    Private IdGarajeSelec As Integer

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs) Handles ReportViewer.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)
        Dim dtPorcGaraje As DtPorcGaraje

        If SonTodosGarajes Then

            dtPorcGaraje = Garaje.RellenarDatosEstadTodosGarajes()          ' Obtenemos las estadísticas de todos los garajes.

            If dtPorcGaraje Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener las estadísticas de los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                EstablecerTituloInforme()
                EstablecerDataSourceInforme(dtPorcGaraje)

                ReportViewer.RefreshReport()

            End If
        Else

            dtPorcGaraje = Garaje.RellenarDatosEstadGarajePorId(IdGarajeSelec)            ' Obtenemos las estadísticas del garaje seleccionado.

            If dtPorcGaraje Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener las estadísticas del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                EstablecerTituloInforme()
                EstablecerDataSourceInforme(dtPorcGaraje)

                ReportViewer.RefreshReport()

            End If

        End If

    End Sub


    ''' <summary>
    ''' Añade el DataSet con los datos al DataSource.
    ''' </summary>
    ''' <param name="dtPorcGaraje">DataSet con los datos de los garajes.</param>
    Private Sub EstablecerDataSourceInforme(ByRef dtPorcGaraje As DtPorcGaraje)

        ReportViewer.ProcessingMode = ProcessingMode.Local
        ReportViewer.LocalReport.DataSources.Clear()
        ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPorcGaraje", dtPorcGaraje.Tables("Estadisticas")))

        ReportViewer.DocumentMapCollapsed = True

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

            If nombreGaraje Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener el nombre del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                Dim rpTituloInf As New ReportParameter("TituloInforme", "Estadísticas del garaje de " & nombreGaraje)
                ReportViewer.LocalReport.SetParameters(rpTituloInf)

            End If

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