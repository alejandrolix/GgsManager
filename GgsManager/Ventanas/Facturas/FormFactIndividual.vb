Imports Microsoft.Reporting.WinForms

Public Class FormFactIndividual

    Private IdClienteSelec As Integer
    Private DatosVehiculo As Vehiculo

    Private Sub FormFactIndividual_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReportViewer.SetDisplayMode(DisplayMode.PrintLayout)

        AddParametrosCliente()
        AddParametrosVehiculo()
        AddParametrosImporte()
        AddParametroNumFactura()
        AddParametrosEmpresa()

        Dim factura As New Factura(Date.Now.Date, IdClienteSelec, True)

        If factura.InsertarParaCliente() Then

            ReportViewer.RefreshReport()
        Else

            MessageBox.Show("Ha habido un problema al añadir la factura al cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

    End Sub


    ''' <summary>
    ''' Añade los datos del cliente a sus campos correspondientes.
    ''' </summary>
    Private Sub AddParametrosCliente()

        Dim datosCliente As Cliente = Cliente.ObtenerClientePorId(IdClienteSelec)

        If datosCliente Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los datos del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

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

        Me.DatosVehiculo = Vehiculo.ObtenerVehiculoPorIdCliente(IdClienteSelec)

        If DatosVehiculo Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener el vehículo del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If DatosVehiculo IsNot Nothing Then

            Dim listaRp As New ReportParameterCollection()
            listaRp.Add(New ReportParameter("MarcaVehiculo", DatosVehiculo.Marca))
            listaRp.Add(New ReportParameter("ModeloVehiculo", DatosVehiculo.Modelo))
            listaRp.Add(New ReportParameter("MatriculaVehiculo", DatosVehiculo.Matricula))

            ReportViewer.LocalReport.SetParameters(listaRp)

        End If

    End Sub


    ''' <summary>
    ''' Añade los datos del importe del cliente a sus campos correspondientes.
    ''' </summary>
    Private Sub AddParametrosImporte()

        Dim porcentajeIva As Integer = Foo.LeerIVA()
        Dim precioIva As Double = DatosVehiculo.PrecioBase + (DatosVehiculo.PrecioBase * porcentajeIva / 100)
        Dim total As Double = DatosVehiculo.PrecioBase + precioIva

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("BaseImponible", DatosVehiculo.PrecioBase.ToString()))
        listaRp.Add(New ReportParameter("PorcIVA", porcentajeIva.ToString()))
        listaRp.Add(New ReportParameter("PrecioIVA", precioIva.ToString()))
        listaRp.Add(New ReportParameter("Total", total.ToString()))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos de la empresa a sus campos correspondientes.
    ''' </summary>
    Private Sub AddParametrosEmpresa()

        Dim arrayDatosEmpresa As String() = Foo.LeerDatosEmpresa()

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("CIFEmpresa", arrayDatosEmpresa(0)))
        listaRp.Add(New ReportParameter("DireccionEmpresa", arrayDatosEmpresa(1)))
        listaRp.Add(New ReportParameter("TelefonoEmpresa", arrayDatosEmpresa(2)))
        listaRp.Add(New ReportParameter("LocalidadEmpresa", arrayDatosEmpresa(3)))
        listaRp.Add(New ReportParameter("CodPostalEmpresa", arrayDatosEmpresa(4)))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade el número de la factura a su campo correspondiente.
    ''' </summary>
    Private Sub AddParametroNumFactura()

        Dim nuevoId As Integer = Factura.ObtenerNuevoIdFactura()

        If nuevoId <= 0 Then

            MessageBox.Show("Ha habido un problema al obtener el nuevo Id de la factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim rpNumFactura As New ReportParameter("NumFactura", nuevoId.ToString())
            ReportViewer.LocalReport.SetParameters(rpNumFactura)

        End If

    End Sub

    Public Sub New(idCliente As Integer)

        InitializeComponent()
        Me.IdClienteSelec = idCliente

    End Sub

End Class