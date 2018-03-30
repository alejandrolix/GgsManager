Imports Microsoft.Reporting.WinForms

Public Class FormFactConjunto

    Private IdGarajeSelec As Integer

    Private Sub FormFactConjunto_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AddParametrosEmpresa()
        AddParametroIVA()

        Dim arrayClientes As Cliente() = Cliente.ObtenerClientesPorIdGaraje(IdGarajeSelec)
        Dim numFacturaActual As Integer = 0
        Dim cantClientesTotal As Integer = arrayClientes.Length - 1
        Dim numClienteActual As Integer = 0

        For i As Integer = 1 To cantClientesTotal

            numFacturaActual += 1
            numClienteActual += 1

            Dim rpNumFactura As New ReportParameter("NumFactura" & numFacturaActual, numFacturaActual.ToString())
            ReportViewer.LocalReport.SetParameters(rpNumFactura)

            If numClienteActual <> cantClientesTotal Then           ' Si el número del cliente actual es distinto al número total de clientes.

                If numFacturaActual = 1 Then

                    AddParametrosCliente(numFacturaActual, arrayClientes(i - 1))
                    AddParametrosVehiculo(numFacturaActual, arrayClientes(i - 1).Vehiculo)
                    AddParametrosImporte(numFacturaActual, arrayClientes(i - 1).Vehiculo.PrecioBase)
                Else

                    AddParametrosCliente(numFacturaActual, arrayClientes(i - 1))
                    AddParametrosVehiculo(numFacturaActual, arrayClientes(i - 1).Vehiculo)
                    AddParametrosImporte(numFacturaActual, arrayClientes(i - 1).Vehiculo.PrecioBase)

                    ' Imprimir Factura.

                    numFacturaActual = 0

                End If
            Else

                ' Se añade el último cliente a la primera factura, la otra se queda vacía.
                AddParametrosCliente(numFacturaActual, arrayClientes(i - 1))
                AddParametrosVehiculo(numFacturaActual, arrayClientes(i - 1).Vehiculo)
                AddParametrosImporte(numFacturaActual, arrayClientes(i - 1).Vehiculo.PrecioBase)

                AddParametrosVacios(numFacturaActual + 1)

            End If

        Next

        ReportViewer.RefreshReport()

    End Sub


    ''' <summary>
    ''' Añade los datos del cliente a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numFacturaActual">Id para identificar la factura.</param>
    ''' <param name="cliente">Los datos de un cliente.</param>
    Private Sub AddParametrosCliente(ByRef numFacturaActual As Integer, ByRef cliente As Cliente)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("NombreApeCliente" & numFacturaActual, cliente.Nombre))
        listaRp.Add(New ReportParameter("DNICliente" & numFacturaActual, cliente.DNI))
        listaRp.Add(New ReportParameter("DireccionCliente" & numFacturaActual, cliente.Direccion))
        listaRp.Add(New ReportParameter("ProvinciaCliente" & numFacturaActual, cliente.Provincia))
        listaRp.Add(New ReportParameter("MovilCliente" & numFacturaActual, cliente.Movil))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos del vehículo a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numFacturaActual">Id para identificar la factura.</param>
    ''' <param name="vehiculo">Los datos de un vehículo.</param>
    Private Sub AddParametrosVehiculo(ByRef numFacturaActual As Integer, ByRef vehiculo As Vehiculo)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("MarcaVehiculo" & numFacturaActual, vehiculo.Marca))
        listaRp.Add(New ReportParameter("ModeloVehiculo" & numFacturaActual, vehiculo.Modelo))
        listaRp.Add(New ReportParameter("MatriculaVehiculo" & numFacturaActual, vehiculo.Matricula))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos del importe a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numFacturaActual">Id para identificar la factura.</param>
    ''' <param name="precioBase">El precio base.</param>
    Private Sub AddParametrosImporte(ByRef numFacturaActual As Integer, ByRef precioBase As Decimal)

        Dim listaRp As New ReportParameterCollection()
        Dim precioConIva As Decimal = Vehiculo.CalcularPrecioTotal(precioBase)

        listaRp.Add(New ReportParameter("BaseImponible" & numFacturaActual, precioBase.ToString()))
        listaRp.Add(New ReportParameter("PrecioIVA" & numFacturaActual, precioConIva.ToString()))

        Dim total As Decimal = precioBase + precioConIva
        listaRp.Add(New ReportParameter("Total" & numFacturaActual, total.ToString()))

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
    ''' Añade los datos del importe a sus campos correspondientes.
    ''' </summary>
    Private Sub AddParametroIVA()

        Dim porcIva As Integer = Foo.LeerIVA()
        Dim rpPorcIva As New ReportParameter("PorcIVA", porcIva.ToString())

        ReportViewer.LocalReport.SetParameters(rpPorcIva)

    End Sub


    ''' <summary>
    ''' No añade ningún dato a los parámetros.
    ''' </summary>
    ''' <param name="numFacturaActual">El número de la factura actual que se está procesando.</param>
    Private Sub AddParametrosVacios(ByRef numFacturaActual As Integer)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("NumFactura" & numFacturaActual, ""))

        listaRp.Add(New ReportParameter("NombreApeCliente" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("DNICliente" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("DireccionCliente" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("ProvinciaCliente" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("MovilCliente" & numFacturaActual, ""))

        listaRp.Add(New ReportParameter("MarcaVehiculo" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("ModeloVehiculo" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("MatriculaVehiculo" & numFacturaActual, ""))

        listaRp.Add(New ReportParameter("BaseImponible" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("PrecioIVA" & numFacturaActual, ""))
        listaRp.Add(New ReportParameter("Total" & numFacturaActual, ""))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class