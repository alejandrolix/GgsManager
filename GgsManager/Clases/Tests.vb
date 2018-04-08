
Imports System.IO
Imports MySql.Data.MySqlClient
Imports NUnit.Framework

<TestFixture>
Public Class Tests

    <Test>
    Public Sub ObtenerClientes()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaHoraAlta, Observaciones
                                         FROM   kk;", conexion)
        Dim datos As MySqlDataReader = Nothing
        Dim i As Integer

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            i = 1

            'Dim listaClientes As New List(Of Cliente)()

            'While datos.Read()

            '    Dim id As Integer = datos.GetInt32("IdCliente")
            '    Dim nombre As String = datos.GetString("Nombre")
            '    Dim apellidos As String = datos.GetString("Apellidos")
            '    Dim dni As String = datos.GetString("DNI")
            '    Dim direccion As String = datos.GetString("Direccion")
            '    Dim poblacion As String = datos.GetString("Poblacion")
            '    Dim provincia As String = datos.GetString("Provincia")
            '    Dim movil As String = datos.GetString("Movil")
            '    Dim fechaHoraAlta As Date = datos.GetDateTime("FechaHoraAlta")
            '    Dim observaciones As String

            '    If datos.IsDBNull(9) Then

            '        observaciones = ""
            '    Else

            '        observaciones = datos.GetString("Observaciones")

            '    End If

            '    Dim arrayFoto As String() = Directory.GetFiles(My.Settings.RutaClientes, id & ".jpg")

            '    If arrayFoto.Length > 0 Then

            '        Dim ivm As New ImageViewModelCliente(arrayFoto(0))
            '        listaClientes.Add(New Cliente(id, nombre, apellidos, dni, direccion, poblacion, provincia, movil, fechaHoraAlta, observaciones, ivm))
            '    Else

            '        listaClientes.Add(New Cliente(id, nombre, apellidos, dni, direccion, poblacion, provincia, movil, fechaHoraAlta, observaciones))

            '    End If

            'End While

            conexion.Close()
            datos.Close()
        Else

            i = 2
            conexion.Close()

        End If

        Assert.AreEqual(i, 2)

    End Sub

End Class
