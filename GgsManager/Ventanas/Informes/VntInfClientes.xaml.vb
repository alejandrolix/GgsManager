Imports Microsoft.Reporting.WinForms

Public Class VntInfClientes

    Private IdGarajeSelec As Integer

    Private Sub ReportViewer_Load(sender As Object, e As EventArgs)

        ReportViewer.LocalReport.ReportPath = "..\..\Informes\InfClientes.rdlc"
        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)

        Dim dtClientes As DtClientes = Cliente.RellenarDatosClientesPorIdGaraje(IdGarajeSelec)

        If dtClientes Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los datos de los clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            EstablecerNombreGaraje()

            ReportViewer.ProcessingMode = ProcessingMode.Local
            ReportViewer.LocalReport.DataSources.Clear()
            ReportViewer.LocalReport.DataSources.Add(New ReportDataSource("DtClientes", dtClientes.Tables("Clientes")))

            ReportViewer.DocumentMapCollapsed = True
            ReportViewer.RefreshReport()

        End If

    End Sub


    ''' <summary>
    ''' Establece el nombre del garaje al parámetro correspondiente.
    ''' </summary>
    Private Sub EstablecerNombreGaraje()

        Dim nombreGaraje As String = Garaje.ObtenerNombreGarajePorId(IdGarajeSelec)

        If nombreGaraje Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener el nombre del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim rpNombreGaraje As New ReportParameter("NombreGaraje", nombreGaraje)
            ReportViewer.LocalReport.SetParameters(rpNombreGaraje)

        End If

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class
