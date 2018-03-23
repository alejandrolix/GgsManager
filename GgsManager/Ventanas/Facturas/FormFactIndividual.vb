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

        Me.DatosVehiculo = Vehiculo.ObtenerVehiculoPorIdCliente(IdClienteSelec)

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

    Private Sub ReportViewer_PrintingBegin(sender As Object, e As ReportPrintEventArgs) Handles ReportViewer.PrintingBegin

        Dim factura As New Factura(Date.Now.Date, IdClienteSelec, True)
        Factura.InsertarFacturaToCliente(factura)

    End Sub


    ''' <summary>
    ''' Añade el número de la factura a su campo correspondiente.
    ''' </summary>
    Private Sub AddParametroNumFactura()

        Dim nuevoId As Integer = Factura.ObtenerNuevoIdFactura()
        Dim rpNumFactura As New ReportParameter("NumFactura", nuevoId.ToString())

        ReportViewer.LocalReport.SetParameters(rpNumFactura)

    End Sub

    Public Sub New(idCliente As Integer)

        InitializeComponent()
        Me.IdClienteSelec = idCliente

    End Sub

End Class