Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text
Imports NPoco

''' <summary>
''' Representa un cliente de la tabla "Clientes".
''' </summary>
Public Class Cliente

    ''' <summary>
    ''' El Id del Cliente.
    ''' </summary>  
    <Column("IdCliente")>
    Property Id As Integer

    ''' <summary>
    ''' El Nombre del Cliente.
    ''' </summary>    
    Property Nombre As String

    ''' <summary>
    ''' Los Apellidos del Cliente.
    ''' </summary>    
    Property Apellidos As String

    ''' <summary>
    ''' El DNI del Cliente.
    ''' </summary>    
    Property DNI As String

    ''' <summary>
    ''' La Dirección del Domicilio del Cliente.
    ''' </summary>    
    Property Direccion As String

    ''' <summary>
    ''' La Población del Domicilio del Cliente.
    ''' </summary>    
    Property Poblacion As String

    ''' <summary>
    ''' La Província del Domicilio del Cliente.
    ''' </summary>    
    Property Provincia As String

    ''' <summary>
    ''' El Móvil de Contacto del Cliente.
    ''' </summary>    
    Property Movil As String

    ''' <summary>
    ''' La Fecha y Hora de Alta del Cliente en el Programa.
    ''' </summary>        
    Property FechaHoraAlta As Date

    ''' <summary>
    ''' Las Observaciones del Cliente.
    ''' </summary>    
    Property Observaciones As String

    ''' <summary>
    ''' El Vehículo del Cliente.
    ''' </summary>    
    <ResultColumn>
    Property Vehiculo As Vehiculo

    ''' <summary>
    ''' La Imagen del Cliente, (opcional)
    ''' </summary>    
    <ResultColumn>
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

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayClientes As Cliente() = conexion.Query(Of Cliente)("SELECT IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaHoraAlta, Observaciones
                                                                     FROM   Clientes
                                                                     ORDER BY Apellidos;").ToArray()
        For Each cliente As Cliente In arrayClientes

            Dim arrayFoto As String() = Directory.GetFiles(My.Settings.RutaClientes, cliente.Id & ".jpg")                   ' Comprobamos si el cliente tiene su foto.

            If arrayFoto.Length > 0 Then

                cliente.Ivm = New ImageViewModelCliente(arrayFoto(0))

            End If

        Next

        conexion.CloseSharedConnection()

        Return arrayClientes

    End Function


    ''' <summary>
    ''' Obtiene el Id, nombre y apellidos de todos los clientes.
    ''' </summary>
    ''' <returns>Array con el Id, nombre y apellidos de los clientes.</returns>
    Public Shared Function ObtenerNombreYApellidosClientes() As Cliente()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayClientes As Cliente() = conexion.Query(Of Cliente)("SELECT IdCliente, Nombre, Apellidos
                                                                     FROM   Clientes
                                                                     ORDER BY Apellidos;").ToArray()
        conexion.CloseSharedConnection()

        Return arrayClientes

    End Function


    ''' <summary>
    ''' Obtiene el Id, nombre y apellidos de los clientes que no tienen vehículo.
    ''' </summary>
    ''' <returns>Array con el Id, nombre y apellidos de los clientes sin vehículo.</returns>
    Public Shared Function ObtenerNombreYApellidosClientesSinVehiculo() As Cliente()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayClientes As Cliente() = conexion.Query(Of Cliente)("SELECT IdCliente, Nombre, Apellidos
                                                                     FROM   Clientes	   
                                                                     WHERE  IdCliente NOT IN (
						                                                                       SELECT IdCliente
                                                                                               FROM   Vehiculos)
                                                                     ORDER BY Apellidos;").ToArray()
        conexion.CloseSharedConnection()

        Return arrayClientes

    End Function


    ''' <summary>
    ''' Obtiene el Id, nombre y apellidos de los clientes a partir del Id de un garaje.
    ''' </summary>
    ''' <returns>Array con el Id, nombre y apellidos de los clientes.</returns>
    Public Shared Function ObtenerNombreYApellidosClientesPorIdGaraje(ByRef idGaraje As Integer) As Cliente()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayClientes As Cliente() = conexion.Query(Of Cliente)(Sql.Builder.Append("SELECT Cli.IdCliente, Cli.Nombre, Cli.Apellidos
                                                                                        FROM   Clientes Cli
                                                                                               JOIN Vehiculos Veh ON Veh.IdCliente = Cli.IdCliente
                                                                                        WHERE  Veh.IdGaraje = @0
                                                                                        ORDER BY Cli.Apellidos;", idGaraje)).ToArray()
        conexion.CloseSharedConnection()

        Return arrayClientes

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
        Dim listaClientes As New List(Of Cliente)()

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos.HasRows Then

            While datos.Read()

                Dim vehiculo As New Vehiculo(datos.GetString("Marca"), datos.GetString("Modelo"), datos.GetString("Matricula"), datos.GetDecimal("PrecioBase"))
                Dim cliente As New Cliente(datos.GetString("Nombre"), datos.GetString("DNI"), datos.GetString("Direccion"), datos.GetString("Provincia"), datos.GetString("Movil"), vehiculo)

                listaClientes.Add(cliente)

            End While

            datos.Close()

        End If

        conexion.Close()

        Return listaClientes.ToArray()

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
            Dim adaptador As New MySqlDataAdapter("SELECT Cli.IdCliente, Cli.Nombre, Cli.Apellidos, Cli.DNI, Cli.Movil, Cli.Observaciones
                                                   FROM   Clientes Cli
	                                                      JOIN Vehiculos Veh ON Veh.IdCliente = Cli.IdCliente
                                                   WHERE  Veh.IdGaraje = @IdGaraje
                                                   ORDER BY Cli.Apellidos;", conexion)

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

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim cliente As Cliente = conexion.Query(Of Cliente)(Sql.Builder.Append("SELECT CONCAT(Nombre, ' ', Apellidos) AS 'Nombre', DNI, Direccion, Provincia, Movil
                                                                                FROM   Clientes
                                                                                WHERE  IdCliente = @0;", idCliente)).First()
        conexion.CloseSharedConnection()

        Return cliente

    End Function


    ''' <summary>
    ''' Inserta un cliente.
    ''' </summary>    
    ''' <param name="cliente">El cliente a insertar.</param>
    ''' <returns>True: El cliente se ha insertado. False: El cliente no se ha insertado.</returns>
    Public Shared Function Insertar(ByRef cliente As Cliente) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim numInsercion As Integer

        Try
            Dim insercion As Object = conexion.Insert("Clientes", "IdCliente", True, cliente)
            numInsercion = Integer.Parse(insercion.ToString())

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return numInsercion >= 1

    End Function


    ''' <summary>
    ''' Elimina un cliente.
    ''' </summary>    
    ''' <param name="cliente">El cliente a eliminar.</param>
    ''' <returns>True: El cliente se ha eliminado. False: El cliente no se ha eliminado.</returns>
    Public Shared Function Eliminar(ByRef cliente As Cliente) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim eliminacion As Integer

        Try
            eliminacion = conexion.Delete("Clientes", "IdCliente", cliente)

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return eliminacion >= 1

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
    ''' <param name="cliente">El cliente a modificar.</param>
    ''' <returns>True: Se han modificado los datos del cliente. False: No se han modificado los datos del cliente.</returns>
    Public Shared Function Modificar(ByRef cliente As Cliente) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim actualizacion As Integer

        Try
            actualizacion = conexion.Update("Clientes", "IdCliente", cliente)

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return actualizacion >= 1

    End Function


    ''' <summary>
    ''' Comprueba si hay una imagen en un Image.
    ''' </summary>
    ''' <param name="img">Imagen a comprobar.</param>
    ''' <returns>True: Hay imagen. False: No hay imagen.</returns>
    Public Shared Function HayImagen(ByRef img As ImageSource) As Boolean

        Return img IsNot Nothing

    End Function

    Public Sub New(nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, fechaHoraAlta As DateTime, observaciones As String)               ' Para crear un cliente.

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

    Public Sub New(nombreCompleto As String, dni As String, direccion As String, provincia As String, movil As String, vehiculo As Vehiculo)

        Me.Nombre = nombreCompleto
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.Vehiculo = vehiculo

    End Sub

    Public Sub New()

    End Sub

End Class
