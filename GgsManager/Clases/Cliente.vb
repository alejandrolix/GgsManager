Imports System.Collections.ObjectModel
Imports MySql.Data.MySqlClient

''' <summary>
''' Representa un cliente de la tabla "Clientes" de la base de datos.
''' </summary>
Public Class Cliente

    Property Id As Integer
    Property Nombre As String
    Property Apellidos As String
    Property DNI As String
    Property Direccion As String
    Property Poblacion As String
    Property Provincia As String
    Property Movil As String
    Property FechaAlta As Date
    Property Observaciones As String
    Property UrlFoto As String


    ''' <summary>
    ''' Obtiene todos los clientes.
    ''' </summary>
    ''' <returns>ObservableCollection de todos los clientes.</returns>
    Public Shared Function ObtenerClientes() As ObservableCollection(Of Cliente)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, URLFoto, Observaciones
                                         FROM   Clientes", conexion)

        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaClientes As New ObservableCollection(Of Cliente)()

            While datos.Read()

                Dim id As Integer = datos.GetInt64("IdCliente")
                Dim nombre As String = datos.GetString("Nombre")
                Dim apellidos As String = datos.GetString("Apellidos")
                Dim dni As String = datos.GetString("DNI")
                Dim direccion As String = datos.GetString("Direccion")
                Dim poblacion As String = datos.GetString("Poblacion")
                Dim provincia As String = datos.GetString("Provincia")
                Dim movil As String = datos.GetString("Movil")
                Dim fechaAlta As Date = datos.GetDateTime("FechaAlta")
                Dim urlFoto As String
                Dim observaciones As String

                If datos.IsDBNull(9) Then

                    urlFoto = ""
                Else

                    urlFoto = datos.GetString("URLFoto")

                End If

                If datos.IsDBNull(10) Then

                    observaciones = ""
                Else

                    observaciones = datos.GetString("Observaciones")

                End If

                listaClientes.Add(New Cliente(id, nombre, apellidos, dni, direccion, poblacion, provincia, movil, fechaAlta, observaciones, urlFoto))

            End While

            datos.Close()
            conexion.Close()

            Return listaClientes

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene el nuevo Id de la tabla "Clientes", (ultimoId + 1) para guardar su imagen.
    ''' </summary>
    ''' <returns>El nuevo Id de la imagen.</returns>
    Public Shared Function ObtenerNuevoIdClientes() As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT MAX(IdCliente) 
                                         FROM   Clientes;", conexion)
        Dim ultimoId As Integer

        Try
            ultimoId = CType(comando.ExecuteScalar(), Integer)
            ultimoId += 1

            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el nuevo Id de la tabla Clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return ultimoId

    End Function


    ''' <summary>
    ''' Inserta un cliente.
    ''' </summary>
    ''' <param name="cliente">Datos del cliente a insertar.</param>
    ''' <returns>True: El cliente se ha insertado. False: El cliente no se ha insertado.</returns>
    Public Shared Function InsertarCliente(ByRef cliente As Cliente) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As MySqlCommand
        Dim numFila As Integer

        If Foo.HayTexto(cliente.Observaciones) And Not Foo.HayTexto(cliente.UrlFoto) Then           ' Insertamos al cliente las observaciones, aparte de sus principales datos.

            comando = New MySqlCommand(String.Format("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, Observaciones, URLFoto)
                                                      VALUES (NULL, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', NOW(), '{7}', NULL);", cliente.Nombre, cliente.Apellidos, cliente.DNI, cliente.Direccion, cliente.Poblacion, cliente.Provincia, cliente.Movil, cliente.Observaciones), conexion)

        ElseIf Foo.HayTexto(cliente.UrlFoto) And Not Foo.HayTexto(cliente.Observaciones) Then       ' Insertamos al cliente la URL de su foto, aparte de sus principales datos.

            comando = New MySqlCommand(String.Format("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, Observaciones, URLFoto)
                                                      VALUES (NULL, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', NOW(), NULL, '{7}');", cliente.Nombre, cliente.Apellidos, cliente.DNI, cliente.Direccion, cliente.Poblacion, cliente.Provincia, cliente.Movil, cliente.UrlFoto), conexion)

        ElseIf Not Foo.HayTexto(cliente.Observaciones) And Not Foo.HayTexto(cliente.UrlFoto) Then       ' No le insertamos al cliente ni las observaciones ni la URL de la foto. Se le insertan sus principales datos.

            comando = New MySqlCommand(String.Format("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, Observaciones, URLFoto)
                                                      VALUES (NULL, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', NOW(), NULL, NULL);", cliente.Nombre, cliente.Apellidos, cliente.DNI, cliente.Direccion, cliente.Poblacion, cliente.Provincia, cliente.Movil), conexion)

        End If

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina un cliente a partir de su Id.
    ''' </summary>
    ''' <param name="idCliente">El Id del cliente a eliminar.</param>
    ''' <returns>True: El cliente se ha eliminado. False: El cliente no se ha eliminado.</returns>
    Public Shared Function EliminarClientePorId(ByRef idCliente As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("DELETE FROM Clientes
                                                       WHERE  IdCliente = {0}", idCliente), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function

    Public Sub New(nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, observaciones As String, urlFoto As String)

        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.Observaciones = observaciones
        Me.UrlFoto = urlFoto

    End Sub

    Public Sub New(id As Integer, nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, fechaAlta As Date, observaciones As String, urlFoto As String)

        Me.Id = id
        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.FechaAlta = fechaAlta
        Me.Observaciones = observaciones
        Me.UrlFoto = urlFoto

    End Sub

    Public Sub New(nombre As String, apellidos As String)

        Me.Nombre = nombre
        Me.Apellidos = apellidos

    End Sub

End Class
