Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text

''' <summary>
''' Representa un cliente de la tabla "Clientes".
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
    Property FechaHoraAlta As Date
    Property Observaciones As String
    Property Vehiculo As Vehiculo
    Property Ivm As ImageViewModelCliente


    ''' <summary>
    ''' Obtiene el nombre y apellidos del cliente.
    ''' </summary>
    ''' <returns>El nombre y apellidos del cliente.</returns>
    Public Overrides Function ToString() As String

        Dim cadena As New StringBuilder()
        cadena.Append(Nombre).Append(" ").Append(Apellidos)

        Return cadena.ToString()

    End Function


    ''' <summary>
    ''' Obtiene todos los clientes.
    ''' </summary>
    ''' <returns>Array con los clientes.</returns>
    Public Shared Function ObtenerClientes() As Cliente()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaHoraAlta, Observaciones
                                         FROM   Clientes
                                         ORDER BY Apellidos;", conexion)
        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            Dim listaClientes As New List(Of Cliente)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdCliente")
                Dim nombre As String = datos.GetString("Nombre")
                Dim apellidos As String = datos.GetString("Apellidos")
                Dim dni As String = datos.GetString("DNI")
                Dim direccion As String = datos.GetString("Direccion")
                Dim poblacion As String = datos.GetString("Poblacion")
                Dim provincia As String = datos.GetString("Provincia")
                Dim movil As String = datos.GetString("Movil")
                Dim fechaHoraAlta As Date = datos.GetDateTime("FechaHoraAlta")
                Dim observaciones As String

                If datos.IsDBNull(9) Then

                    observaciones = ""
                Else

                    observaciones = datos.GetString("Observaciones")

                End If

                Dim arrayFoto As String() = Directory.GetFiles(My.Settings.RutaClientes, id & ".jpg")

                If arrayFoto.Length > 0 Then

                    Dim ivm As New ImageViewModelCliente(arrayFoto(0))
                    listaClientes.Add(New Cliente(id, nombre, apellidos, dni, direccion, poblacion, provincia, movil, fechaHoraAlta, observaciones, ivm))
                Else

                    listaClientes.Add(New Cliente(id, nombre, apellidos, dni, direccion, poblacion, provincia, movil, fechaHoraAlta, observaciones))

                End If

            End While

            conexion.Close()
            datos.Close()

            Return listaClientes.ToArray()
        Else

            conexion.Close()
            Return Nothing

        End If

    End Function


    ''' <summary>
    ''' Obtiene el Id, nombre y apellidos de todos los clientes.
    ''' </summary>
    ''' <returns>Array con el Id, nombre y apellidos de los clientes.</returns>
    Public Shared Function ObtenerNombreYApellidosClientes() As Cliente()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdCliente, Nombre, Apellidos
                                         FROM   Clientes
                                         ORDER BY Apellidos;", conexion)

        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            Dim listaClientes As New List(Of Cliente)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdCliente")
                Dim nombre As String = datos.GetString("Nombre")
                Dim apellidos As String = datos.GetString("Apellidos")

                Dim cliente As New Cliente(id, nombre, apellidos)
                listaClientes.Add(cliente)

            End While

            conexion.Close()
            datos.Close()

            Return listaClientes.ToArray()
        Else

            conexion.Close()
            Return Nothing

        End If

    End Function


    ''' <summary>
    ''' Obtiene el Id, nombre y apellidos de los clientes a partir del Id de un garaje.
    ''' </summary>
    ''' <returns>Array con el Id, nombre y apellidos de los clientes.</returns>
    Public Shared Function ObtenerNombreYApellidosClientesPorIdGaraje(ByRef idGaraje As Integer) As Cliente()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT Cli.IdCliente, Cli.Nombre, Cli.Apellidos
                                         FROM   Clientes Cli
	                                            JOIN Vehiculos Veh ON Veh.IdCliente = Cli.IdCliente
                                         WHERE  Veh.IdGaraje = @IdGaraje
                                         ORDER BY Cli.Apellidos;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            Dim listaClientes As New List(Of Cliente)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdCliente")
                Dim nombre As String = datos.GetString("Nombre")
                Dim apellidos As String = datos.GetString("Apellidos")

                Dim cliente As New Cliente(id, nombre, apellidos)
                listaClientes.Add(cliente)

            End While

            conexion.Close()
            datos.Close()

            Return listaClientes.ToArray()
        Else

            conexion.Close()
            Return Nothing

        End If

    End Function

    ''' <summary>
    ''' Obtiene los datos de los clientes a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    ''' <returns>Array con los datos de los clientes.</returns>
    Public Shared Function ObtenerClientesPorIdGaraje(ByRef idGaraje As Integer) As Cliente()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Nombre', Cli.DNI, Cli.Direccion, Cli.Provincia, Cli.Movil, Veh.Marca, Veh.Modelo, Veh.Matricula, Veh.PrecioBase
                                         FROM   Clientes Cli
	                                            JOIN Vehiculos Veh ON Cli.IdCliente = Veh.IdCliente
                                         WHERE  Veh.IdGaraje = @IdGaraje
                                         ORDER BY Nombre;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            Dim listaClientes As New List(Of Cliente)()

            While datos.Read()

                Dim nombre As String = datos.GetString("Nombre")
                Dim dni As String = datos.GetString("DNI")
                Dim direccion As String = datos.GetString("Direccion")
                Dim provincia As String = datos.GetString("Provincia")
                Dim movil As String = datos.GetString("Movil")
                Dim marca As String = datos.GetString("Marca")
                Dim modelo As String = datos.GetString("Modelo")
                Dim matricula As String = datos.GetString("Matricula")
                Dim precioBase As Decimal = datos.GetDecimal("PrecioBase")

                Dim cliente As New Cliente(nombre, dni, direccion, provincia, movil)
                Dim vehiculo As New Vehiculo(marca, modelo, matricula, precioBase)

                cliente.Vehiculo = vehiculo
                listaClientes.Add(cliente)

            End While

            datos.Close()
            conexion.Close()

            Return listaClientes.ToArray()
        Else

            conexion.Close()
            Return Nothing

        End If

    End Function


    ''' <summary>
    ''' Obtiene los datos de los clientes a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    ''' <returns>DataSet con los datos de los clientes.</returns>
    Public Shared Function RellenarDatosClientesPorIdGaraje(ByRef idGaraje As Integer) As DtClientes

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim dtClientes As New DtClientes()

        Try
            Dim adaptador As New MySqlDataAdapter("SELECT Cli.IdCliente, Cli.Nombre, Cli.DNI, Cli.Movil, Cli.Observaciones
                                                   FROM   Clientes Cli
	                                                      JOIN Vehiculos Veh ON Veh.IdCliente = Cli.IdCliente
                                                   WHERE  Veh.IdGaraje = @IdGaraje;", conexion)

            adaptador.SelectCommand.Parameters.AddWithValue("@IdGaraje", idGaraje)
            adaptador.Fill(dtClientes, "Clientes")

        Catch ex As Exception

        End Try

        conexion.Close()

        Return dtClientes

    End Function


    ''' <summary>
    ''' Obtiene el nuevo Id de la tabla "Clientes", (ultimoId + 1) para guardar su imagen.
    ''' </summary>
    ''' <returns>El nuevo Id de la imagen.</returns>
    Public Shared Function ObtenerNuevoIdClientes() As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT MAX(IdCliente) + 1
                                         FROM   Clientes;", conexion)
        Dim nuevoId As Integer

        Try
            nuevoId = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

        End Try

        conexion.Close()

        Return nuevoId

    End Function


    ''' <summary>
    ''' Obtiene un cliente a partir de su Id.
    ''' </summary>
    ''' <param name="idCliente">El Id del cliente.</param>
    ''' <returns>Los datos del cliente.</returns>
    Public Shared Function ObtenerClientePorId(ByRef idCliente As Integer) As Cliente

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT CONCAT(Nombre, ' ', Apellidos) AS 'Nombre', DNI, Direccion, Provincia, Movil
                                         FROM   Clientes
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            Dim cliente As Cliente = Nothing

            While datos.Read()

                Dim nombre As String = datos.GetString("Nombre")
                Dim dni As String = datos.GetString("DNI")
                Dim direccion As String = datos.GetString("Direccion")
                Dim provincia As String = datos.GetString("Provincia")
                Dim movil As String = datos.GetString("Movil")

                cliente = New Cliente(nombre, dni, direccion, provincia, movil)

            End While

            conexion.Close()
            datos.Close()

            Return cliente

        End If

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Inserta un cliente.
    ''' </summary>    
    ''' <returns>True: El cliente se ha insertado. False: El cliente no se ha insertado.</returns>
    Public Function Insertar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As MySqlCommand
        Dim numFila As Integer

        If Foo.HayTexto(Observaciones) Then              ' Insertamos al cliente las observaciones, aparte de sus principales datos.

            comando = New MySqlCommand("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaHoraAlta, Observaciones)
                                        VALUES (NULL, @Nombre, @Apellidos, @DNI, @Direccion, @Poblacion, @Provincia, @Movil, NOW(), @Observaciones);", conexion)
        Else
            comando = New MySqlCommand("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaHoraAlta, Observaciones)
                                        VALUES (NULL, @Nombre, @Apellidos, @DNI, @Direccion, @Poblacion, @Provincia, @Movil, NOW(), NULL);", conexion)
        End If

        comando.Parameters.AddWithValue("@Nombre", Nombre)
        comando.Parameters.AddWithValue("@Apellidos", Apellidos)
        comando.Parameters.AddWithValue("@DNI", DNI)
        comando.Parameters.AddWithValue("@Direccion", Direccion)
        comando.Parameters.AddWithValue("@Poblacion", Poblacion)
        comando.Parameters.AddWithValue("@Provincia", Provincia)
        comando.Parameters.AddWithValue("@Movil", Movil)

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina un cliente.
    ''' </summary>    
    ''' <returns>True: El cliente se ha eliminado. False: El cliente no se ha eliminado.</returns>
    Public Function Eliminar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Clientes
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina la imagen de un cliente.
    ''' </summary>    
    Public Sub EliminarImg()

        Dim cadena As New StringBuilder()
        cadena.Append(My.Settings.RutaClientes).Append(Id).Append(".jpg")

        If File.Exists(cadena.ToString()) Then

            File.Delete(cadena.ToString())

        End If

    End Sub


    ''' <summary>
    ''' Modifica los datos de un cliente.
    ''' </summary>    
    ''' <returns>True: Se han modificado los datos del cliente. False: No se han modificado los datos del cliente.</returns>
    Public Function Modificar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Clientes
                                         SET    Nombre = @Nombre, Apellidos = @Apellidos, DNI = @DNI, Direccion = @Direccion, Poblacion = @Poblacion, Provincia = @Provincia, Movil = @Movil, Observaciones = @Observaciones
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@Nombre", Nombre)
        comando.Parameters.AddWithValue("@Apellidos", Apellidos)
        comando.Parameters.AddWithValue("@DNI", DNI)
        comando.Parameters.AddWithValue("@Direccion", Direccion)
        comando.Parameters.AddWithValue("@Poblacion", Poblacion)
        comando.Parameters.AddWithValue("@Provincia", Provincia)
        comando.Parameters.AddWithValue("@Movil", Movil)
        comando.Parameters.AddWithValue("@Observaciones", Observaciones)
        comando.Parameters.AddWithValue("@IdCliente", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Comprueba si hay una imagen en un Image.
    ''' </summary>
    ''' <param name="img">Imagen a comprobar.</param>
    ''' <returns>True: Hay imagen. False: No hay imagen.</returns>
    Public Shared Function HayImagen(ByRef img As ImageSource) As Boolean

        Return img IsNot Nothing

    End Function

    Public Sub New(nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, observaciones As String)               ' Para crear un cliente.

        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(id As Integer, nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, observaciones As String)               ' Para modificar los datos de un cliente seleccionado.

        Me.Id = id
        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(id As Integer, nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, fechaHoraAlta As Date, observaciones As String, ivm As ImageViewModelCliente)              ' Para mostrar el cliente con su imagen en el DataGrid.

        Me.Id = id
        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.FechaHoraAlta = fechaHoraAlta
        Me.Observaciones = observaciones
        Me.Ivm = ivm

    End Sub

    Public Sub New(id As Integer, nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, fechaHoraAlta As Date, observaciones As String)              ' Para mostrar el cliente sin su imagen en el DataGrid.

        Me.Id = id
        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.FechaHoraAlta = fechaHoraAlta
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(nombre As String, apellidos As String)               ' Para mostrar el nombre y apellidos de un cliente en el DataGrid de Vehículos.

        Me.Nombre = nombre
        Me.Apellidos = apellidos

    End Sub

    Public Sub New(id As Integer, nombre As String, apellidos As String)               ' Para mostrar el nombre y apellidos de un cliente en el ComboBox de "VntAddVehiculo".

        Me.Id = id
        Me.Nombre = nombre
        Me.Apellidos = apellidos

    End Sub

    Public Sub New(id As Integer)               ' Para crear un vehículo.

        Me.Id = id

    End Sub

    Public Sub New(nombre As String, dni As String, direccion As String, provincia As String, movil As String)              ' Para mostrar la factura con sus datos.

        Me.Nombre = nombre
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Provincia = provincia
        Me.Movil = movil

    End Sub

End Class
