Imports Microsoft.Reporting.WinForms
Imports MySql.Data.MySqlClient

Public Class FormFactIndividual

    Private IdClienteSelec As Integer

    Private Sub FormFactIndividual_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)
        AddParametrosCliente()
        AddParametrosVehiculo()

        ReportViewer.RefreshReport()

    End Sub


    ''' <summary>
    ''' Añade los datos del cliente a sus campos correspondientes.
    ''' </summary>
    Private Sub AddParametrosCliente()

        Dim datosCliente As Cliente = Cliente.ObtenerClientePorId(IdClienteSelec)

        If datosCliente IsNot Nothing Then

            Dim listaRp As New ReportParameterCollection()
            listaRp.Add(New ReportParameter("NombreYApellidosCliente", datosCliente.Nombre))
            listaRp.Add(New ReportParameter("DNICliente", datosCliente.DNI))
            listaRp.Add(New ReportParameter("DireccionCliente", datosCliente.Direccion))
            listaRp.Add(New ReportParameter("ProvinciaCliente", datosCliente.Provincia))
            listaRp.Add(New ReportParameter("MovilCliente", datosCliente.Movil))

            ReportViewer.LocalReport.SetParameters(listaRp)

        End If

    End Sub


    ''' <summary>
    ''' Añade los datos del vehículo del cliente a sus campos correspondientes.
    ''' </summary>
    Private Sub AddParametrosVehiculo()

        Dim datosVehiculo As Vehiculo = Vehiculo.ObtenerVehiculoPorIdCliente(IdClienteSelec)

        If datosVehiculo IsNot Nothing Then

            Dim listaRp As New ReportParameterCollection()
            listaRp.Add(New ReportParameter("MarcaVehiculo", datosVehiculo.Marca))
            listaRp.Add(New ReportParameter("ModeloVehiculo", datosVehiculo.Modelo))
            listaRp.Add(New ReportParameter("MatriculaVehiculo", datosVehiculo.Matricula))

            ReportViewer.LocalReport.SetParameters(listaRp)

        End If

    End Sub

    Public Sub New(idCliente As Integer)

        InitializeComponent()
        Me.IdClienteSelec = idCliente

    End Sub

End Class