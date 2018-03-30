Imports Microsoft.Reporting.WinForms

Public Class FormFactConjunto

    Private IdGarajeSelec As Integer

    Private Sub FormFactConjunto_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AddParametrosEmpresa()
        AddParametroIVA()
        Dim arrayClientes As Cliente() = Cliente.ObtenerClientesPorIdGaraje(IdGarajeSelec)

        For i As Integer = 0 To arrayClientes.Length - 1

            If i = 0 Then

                Dim rpNumFactura As New ReportParameter("NumFactura1", "1")
                ReportViewer.LocalReport.SetParameters(rpNumFactura)

                AddParametrosCliente(1, arrayClientes(i))
                AddParametrosVehiculo(1, arrayClientes(i).Vehiculo)
                AddParametrosImporte(1, arrayClientes(i).Vehiculo.PrecioBase)
            Else

                Dim rpNumFactura As New ReportParameter("NumFactura2", "2")
                ReportViewer.LocalReport.SetParameters(rpNumFactura)

                AddParametrosCliente(2, arrayClientes(i))
                AddParametrosVehiculo(2, arrayClientes(i).Vehiculo)
                AddParametrosImporte(2, arrayClientes(i).Vehiculo.PrecioBase)

            End If

        Next

        ReportViewer.RefreshReport()

    End Sub


    ''' <summary>
    ''' Añade los datos del cliente a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numCliente">Id para identificar la factura.</param>
    ''' <param name="cliente">Los datos de un cliente.</param>
    Private Sub AddParametrosCliente(ByRef numCliente As Integer, ByRef cliente As Cliente)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("NombreApeCliente" & numCliente, cliente.Nombre))
        listaRp.Add(New ReportParameter("DNICliente" & numCliente, cliente.DNI))
        listaRp.Add(New ReportParameter("DireccionCliente" & numCliente, cliente.Direccion))
        listaRp.Add(New ReportParameter("ProvinciaCliente" & numCliente, cliente.Provincia))
        listaRp.Add(New ReportParameter("MovilCliente" & numCliente, cliente.Movil))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos del vehículo a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numCliente">Id para identificar la factura.</param>
    ''' <param name="vehiculo">Los datos de un vehículo.</param>
    Private Sub AddParametrosVehiculo(ByRef numCliente As Integer, ByRef vehiculo As Vehiculo)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("MarcaVehiculo" & numCliente, vehiculo.Marca))
        listaRp.Add(New ReportParameter("ModeloVehiculo" & numCliente, vehiculo.Modelo))
        listaRp.Add(New ReportParameter("MatriculaVehiculo" & numCliente, vehiculo.Matricula))

        ReportViewer.LocalReport.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos del importe a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numCliente">Id para identificar la factura.</param>
    ''' <param name="precioBase">El precio base.</param>
    Private Sub AddParametrosImporte(ByRef numCliente As Integer, ByRef precioBase As Decimal)

        Dim listaRp As New ReportParameterCollection()
        Dim precioConIva As Decimal = Vehiculo.CalcularPrecioTotal(precioBase)

        listaRp.Add(New ReportParameter("BaseImponible" & numCliente, precioBase.ToString()))
        listaRp.Add(New ReportParameter("PrecioIVA" & numCliente, precioConIva.ToString()))

        Dim total As Decimal = precioBase + precioConIva
        listaRp.Add(New ReportParameter("Total" & numCliente, total.ToString()))

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

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class