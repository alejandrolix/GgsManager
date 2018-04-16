Imports Microsoft.Reporting.WinForms

Public Class VntInfPlazas

    Private IdGarajeSelec As Integer

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        PlzTodasRb.IsChecked = True            ' Lanzamos el evento "Checked".

    End Sub

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs)

        ReportViewer.LocalReport.ReportPath = "..\..\Informes\InfPlazas.rdlc"
        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)
        ReportViewer.ProcessingMode = ProcessingMode.Local

        Dim nombreGaraje As String = Garaje.ObtenerNombreGarajePorId(IdGarajeSelec)

        If nombreGaraje Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener el nombre del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim rpNombreGaraje As New ReportParameter("NombreGaraje", nombreGaraje)
            ReportViewer.LocalReport.SetParameters(rpNombreGaraje)

        End If

    End Sub

    Private Sub PlzLibre_Checked(sender As Object, e As RoutedEventArgs)

        If PlzLibreRb.IsChecked Then

            Dim dtPlazas As DtPlazas = Plaza.RellenarDatosPlazasLibresPorIdGaraje(IdGarajeSelec)

            If dtPlazas Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener los datos de las plazas libres.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                ReportViewer.LocalReport.DataSources.Clear()
                ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

                ReportViewer.DocumentMapCollapsed = True
                ReportViewer.RefreshReport()

            End If

        End If

    End Sub

    Private Sub PlzOcupada_Checked(sender As Object, e As RoutedEventArgs)

        If PlzOcupadaRb.IsChecked Then

            Dim dtPlazas As DtPlazas = Plaza.RellenarDatosPlazasOcupadasPorIdGaraje(IdGarajeSelec)

            If dtPlazas Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener los datos de las plazas ocupadas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                ReportViewer.LocalReport.DataSources.Clear()
                ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

                ReportViewer.DocumentMapCollapsed = True
                ReportViewer.RefreshReport()

            End If

        End If

    End Sub

    Private Sub PlzTodas_Checked(sender As Object, e As RoutedEventArgs)

        If PlzTodasRb.IsChecked Then

            Dim dtPlazas As DtPlazas = Plaza.RellenarDatosTodasPlazasPorIdGaraje(IdGarajeSelec)

            If dtPlazas Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener los datos de todas las plazas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                ReportViewer.LocalReport.DataSources.Clear()
                ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtPlazas", dtPlazas.Tables("Plazas")))

                ReportViewer.DocumentMapCollapsed = True
                ReportViewer.RefreshReport()

            End If

        End If

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class
