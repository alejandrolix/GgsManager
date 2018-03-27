Imports Microsoft.Reporting.WinForms

Public Class FormFactConjunto

    Private IdGarajeSelec As Integer

    Private Sub FormFactConjunto_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AddParametrosEmpresa()

        'Dim conexion As MySqlConnection = Foo.ConexionABd()
        'Dim comando As New MySqlCommand("SELECT CONCAT(Nombre, ' ', Apellidos) AS 'Nombre', DNI, Direccion, Provincia, Movil
        '                                 FROM   Clientes
        '                                 LIMIT  2;", conexion)

        'Dim datos As MySqlDataReader = comando.ExecuteReader()
        'Dim numFila As Integer = 0

        'While datos.Read()

        '    numFila += 1
        '    Dim nombre As String = datos.GetString("Nombre")

        '    If numFila = 1 Then

        '        Dim dni As String = datos.GetString("DNI")
        '        Dim direccion As String = datos.GetString("Direccion")
        '        Dim provincia As String = datos.GetString("Provincia")
        '        Dim movil As String = datos.GetString("Movil")

        '        Dim listaRp As New ReportParameterCollection()
        '        listaRp.Add(New ReportParameter("NombreYApellidosCliente", nombre))
        '        listaRp.Add(New ReportParameter("DNICliente", dni))
        '        listaRp.Add(New ReportParameter("DireccionCliente", direccion))
        '        listaRp.Add(New ReportParameter("ProvinciaCliente", provincia))
        '        listaRp.Add(New ReportParameter("MovilCliente", movil))

        '        ReportViewer.LocalReport.SetParameters(listaRp)

        '        Dim listaRp1 As New ReportParameterCollection()
        '        listaRp1.Add(New ReportParameter("MarcaVehiculo", "Hola"))
        '        listaRp1.Add(New ReportParameter("ModeloVehiculo", "Hola"))
        '        listaRp1.Add(New ReportParameter("MatriculaVehiculo", "Hola"))

        '        ReportViewer.LocalReport.SetParameters(listaRp1)

        '        Dim listaRp2 As New ReportParameterCollection()
        '        listaRp2.Add(New ReportParameter("BaseImponible", "13"))
        '        listaRp2.Add(New ReportParameter("PorcIVA", "21"))
        '        listaRp2.Add(New ReportParameter("PrecioIVA", "13.40"))
        '        listaRp2.Add(New ReportParameter("Total", "20"))

        '        ReportViewer.LocalReport.SetParameters(listaRp2)

        '    ElseIf numFila = 2 Then

        '        Dim rpNomApeCliente2 As New ReportParameter("NombreYApellidosCliente2", nombre)
        '        ReportViewer.LocalReport.SetParameters(rpNomApeCliente2)

        '    End If

        'End While

        'Dim rpNumFactura As New ReportParameter("NumFactura", "3")
        'ReportViewer.LocalReport.SetParameters(rpNumFactura)

        'ReportViewer.RefreshReport()

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

    Public Sub New(idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class