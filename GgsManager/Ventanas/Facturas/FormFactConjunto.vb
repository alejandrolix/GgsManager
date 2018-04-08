Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.IO
Imports System.Text
Imports Microsoft.Reporting.WinForms

Public Class FormFactConjunto

    Private IdGarajeSelec As Integer
    Private ListaStreams As List(Of Stream)
    Private NumPagActual As Integer


    ''' <summary>
    ''' Empieza a imprimir las facturas correspondientes.
    ''' </summary>
    Public Sub EmpezarImpresion()

        Dim arrayClientes As Cliente() = Cliente.ObtenerClientesPorIdGaraje(IdGarajeSelec)

        If arrayClientes Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los clientes del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim numFacturaActual As Integer = 0
            Dim cantClientesTotal As Integer = arrayClientes.Length - 1

            Dim factura As New LocalReport()
            factura.ReportPath = "..\..\Facturas\FactConjunto.rdlc"

            AddParametrosEmpresa(factura)
            AddParametroIVA(factura)

            For i As Integer = 0 To cantClientesTotal

                numFacturaActual += 1

                Dim rpNumFactura As New ReportParameter("NumFactura" & numFacturaActual, numFacturaActual.ToString())
                factura.SetParameters(rpNumFactura)

                If i <> cantClientesTotal Then           ' Si la posición del array es distinta al número total de clientes.

                    If numFacturaActual = 1 Then

                        AddParametrosCliente(numFacturaActual, arrayClientes(i), factura)
                        AddParametrosVehiculo(numFacturaActual, arrayClientes(i).Vehiculo, factura)
                        AddParametrosImporte(numFacturaActual, arrayClientes(i).Vehiculo.PrecioBase, factura)
                    Else

                        AddParametrosCliente(numFacturaActual, arrayClientes(i), factura)
                        AddParametrosVehiculo(numFacturaActual, arrayClientes(i).Vehiculo, factura)
                        AddParametrosImporte(numFacturaActual, arrayClientes(i).Vehiculo.PrecioBase, factura)

                        ImprimirFactura(factura)
                        numFacturaActual = 0

                    End If
                Else

                    If numFacturaActual = 1 Then

                        ' Se añade el último cliente a la primera factura, la otra se queda vacía.
                        AddParametrosCliente(numFacturaActual, arrayClientes(i), factura)
                        AddParametrosVehiculo(numFacturaActual, arrayClientes(i).Vehiculo, factura)
                        AddParametrosImporte(numFacturaActual, arrayClientes(i).Vehiculo.PrecioBase, factura)

                        AddParametrosVacios(numFacturaActual + 1, factura)
                    Else

                        AddParametrosCliente(numFacturaActual, arrayClientes(i), factura)
                        AddParametrosVehiculo(numFacturaActual, arrayClientes(i).Vehiculo, factura)
                        AddParametrosImporte(numFacturaActual, arrayClientes(i).Vehiculo.PrecioBase, factura)

                    End If

                    ImprimirFactura(factura)

                End If

            Next

        End If

    End Sub


    ''' <summary>
    ''' Añade los datos del cliente a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numFacturaActual">Id para identificar la factura.</param>
    ''' <param name="cliente">Los datos de un cliente.</param>
    ''' <param name="factura">La factura que se está procesando.</param>
    Private Sub AddParametrosCliente(ByRef numFacturaActual As Integer, ByRef cliente As Cliente, ByRef factura As LocalReport)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("NombreApeCliente" & numFacturaActual, cliente.Nombre))
        listaRp.Add(New ReportParameter("DNICliente" & numFacturaActual, cliente.DNI))
        listaRp.Add(New ReportParameter("DireccionCliente" & numFacturaActual, cliente.Direccion))
        listaRp.Add(New ReportParameter("ProvinciaCliente" & numFacturaActual, cliente.Provincia))
        listaRp.Add(New ReportParameter("MovilCliente" & numFacturaActual, cliente.Movil))

        factura.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos del vehículo a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numFacturaActual">Id para identificar la factura.</param>
    ''' <param name="vehiculo">Los datos de un vehículo.</param>
    ''' <param name="factura">La factura que se está procesando.</param>
    Private Sub AddParametrosVehiculo(ByRef numFacturaActual As Integer, ByRef vehiculo As Vehiculo, ByRef factura As LocalReport)

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("MarcaVehiculo" & numFacturaActual, vehiculo.Marca))
        listaRp.Add(New ReportParameter("ModeloVehiculo" & numFacturaActual, vehiculo.Modelo))
        listaRp.Add(New ReportParameter("MatriculaVehiculo" & numFacturaActual, vehiculo.Matricula))

        factura.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos del importe a sus campos correspondientes.
    ''' </summary>
    ''' <param name="numFacturaActual">Id para identificar la factura.</param>
    ''' <param name="precioBase">El precio base.</param>
    ''' <param name="factura">La factura que se está procesando.</param>
    Private Sub AddParametrosImporte(ByRef numFacturaActual As Integer, ByRef precioBase As Decimal, ByRef factura As LocalReport)

        Dim listaRp As New ReportParameterCollection()
        Dim precioConIva As Decimal = Vehiculo.CalcularPrecioTotal(precioBase)

        listaRp.Add(New ReportParameter("BaseImponible" & numFacturaActual, precioBase.ToString()))
        listaRp.Add(New ReportParameter("PrecioIVA" & numFacturaActual, precioConIva.ToString()))

        Dim total As Decimal = precioBase + precioConIva
        listaRp.Add(New ReportParameter("Total" & numFacturaActual, total.ToString()))

        factura.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade los datos de la empresa a sus campos correspondientes.
    ''' </summary>
    ''' <param name="factura">La factura que se está procesando.</param>
    Private Sub AddParametrosEmpresa(ByRef factura As LocalReport)

        Dim arrayDatosEmpresa As String() = Foo.LeerDatosEmpresa()

        Dim listaRp As New ReportParameterCollection()
        listaRp.Add(New ReportParameter("CIFEmpresa", arrayDatosEmpresa(0)))
        listaRp.Add(New ReportParameter("DireccionEmpresa", arrayDatosEmpresa(1)))
        listaRp.Add(New ReportParameter("TelefonoEmpresa", arrayDatosEmpresa(2)))
        listaRp.Add(New ReportParameter("LocalidadEmpresa", arrayDatosEmpresa(3)))
        listaRp.Add(New ReportParameter("CodPostalEmpresa", arrayDatosEmpresa(4)))

        factura.SetParameters(listaRp)

    End Sub


    ''' <summary>
    ''' Añade el porcentaje del IVA a su campo correspondiente.
    ''' </summary>
    ''' <param name="factura">La factura que se está procesando.</param>
    Private Sub AddParametroIVA(ByRef factura As LocalReport)

        Dim porcIva As Integer = Foo.LeerIVA()
        Dim rpPorcIva As New ReportParameter("PorcIVA", porcIva.ToString())

        factura.SetParameters(rpPorcIva)

    End Sub


    ''' <summary>
    ''' No añade ningún dato a los parámetros.
    ''' </summary>
    ''' <param name="numFacturaActual">El número de la factura actual que se está procesando.</param>
    ''' <param name="factura">La factura que se está procesando.</param>
    Private Sub AddParametrosVacios(ByRef numFacturaActual As Integer, ByRef factura As LocalReport)

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

        factura.SetParameters(listaRp)

    End Sub

    Private Function CrearStream(ByVal name As String, ByVal fileNameExtension As String, ByVal encoding As Encoding, ByVal mimeType As String, ByVal willSeek As Boolean) As Stream

        Dim stream As Stream = New MemoryStream()
        ListaStreams.Add(stream)

        Return stream

    End Function


    ''' <summary>
    ''' Imprime la factura en formato DIN A4.
    ''' </summary>
    Private Sub ImprimirFactura(ByRef factura As LocalReport)

        Dim infoHoja As String = "<DeviceInfo>" &
                                        "<OutputFormat>EMF</OutputFormat>" &
                                        "<PageWidth>21cm</PageWidth>" &
                                        "<PageHeight>29.7cm</PageHeight>" &
                                        "<MarginTop>2cm</MarginTop>" &
                                        "<MarginLeft>0.5cm</MarginLeft>" &
                                        "<MarginRight>0.5cm</MarginRight>" &
                                        "<MarginBottom>2cm</MarginBottom>" &
                                 "</DeviceInfo>"
        Dim warnings As Warning()
        Me.ListaStreams = New List(Of Stream)()

        Try
            factura.Render("Image", infoHoja, AddressOf CrearStream, warnings)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al imprimir la factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        For Each stream As Stream In ListaStreams

            stream.Position = 0

        Next

        If PrintDialog.ShowDialog() = Forms.DialogResult.OK Then

            Dim printDoc As New PrintDocument()
            printDoc.PrinterSettings.PrinterName = PrintDialog.PrinterSettings.PrinterName
            PrintDialog.Document = printDoc

            AddHandler printDoc.PrintPage, AddressOf ImprimirPagina

            Me.NumPagActual = 0
            printDoc.Print()

        End If

    End Sub

    Private Sub ImprimirPagina(sender As Object, e As PrintPageEventArgs)

        Dim imgPagina As New Metafile(ListaStreams(NumPagActual))
        Dim rectangulo As New System.Drawing.Rectangle(e.PageBounds.Left - CInt(e.PageSettings.HardMarginX), e.PageBounds.Top - CInt(e.PageSettings.HardMarginY), e.PageBounds.Width, e.PageBounds.Height)

        e.Graphics.FillRectangle(System.Drawing.Brushes.White, rectangulo)
        e.Graphics.DrawImage(imgPagina, rectangulo)

        e.HasMorePages = False

    End Sub

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class