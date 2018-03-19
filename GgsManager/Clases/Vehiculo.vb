Imports System.Globalization
Imports MySql.Data.MySqlClient

''' <summary>
''' Representa un vehículo de la tabla "Vehiculos".
''' </summary>
Public Class Vehiculo

    Property Id As Integer
    Property Matricula As String
    Property Marca As String
    Property Modelo As String
    Property Cliente As Cliente
    Property IdPlaza As Integer
    Property IdGaraje As Integer
    Property PrecioBase As Decimal
    Property PrecioTotal As Decimal
    Property ArrayUrlFotos As String()


    ''' <summary>
    ''' Obtiene todos los vehículos a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje seleccionado.</param>
    ''' <returns>Lista con los vehículos del garaje seleccionado.</returns>
    Public Shared Function ObtenerVehiculosPorIdGaraje(ByRef idGaraje As Integer) As List(Of Vehiculo)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT Veh.IdVehiculo, Veh.Matricula, Veh.Marca, Veh.Modelo, Cli.Nombre, Cli.Apellidos, Veh.IdGaraje, Veh.IdPlaza, Veh.PrecioBase, Veh.PrecioTotal
                                         FROM   Vehiculos Veh
                                                JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente
                                         WHERE  Veh.IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los vehículos del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaVehiculos As New List(Of Vehiculo)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdVehiculo")
                Dim matricula As String = datos.GetString("Matricula")
                Dim marca As String = datos.GetString("Marca")
                Dim modelo As String = datos.GetString("Modelo")
                Dim cliente As Cliente = New Cliente(datos.GetString("Nombre"), datos.GetString("Apellidos"))
                Dim idGj As Integer = datos.GetInt32("IdGaraje")
                Dim idPl As Integer = datos.GetInt32("IdPlaza")
                Dim precioBase As Decimal = datos.GetDecimal("PrecioBase")
                Dim precioTotal As Decimal = datos.GetDecimal("PrecioTotal")

                Dim vehiculo As New Vehiculo(id, matricula, marca, modelo, cliente, idGj, idPl, precioBase, precioTotal)
                listaVehiculos.Add(vehiculo)

            End While

            datos.Close()
            conexion.Close()

            Return listaVehiculos

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene el nuevo Id de la tabla "Vehiculos", (ultimoId + 1) para guardar su imagen.
    ''' </summary>
    ''' <returns>El nuevo Id de la imagen.</returns>
    Public Shared Function ObtenerNuevoIdVehiculos() As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT MAX(IdVehiculo) + 1
                                         FROM   Vehiculos;", conexion)
        Dim ultimoId As Integer

        Try
            ultimoId = CType(comando.ExecuteScalar(), Integer)
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el nuevo Id de la tabla Vehiculos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return ultimoId

    End Function


    ''' <summary>
    ''' Obtiene los datos del vehículo a partir del Id de un cliente.
    ''' </summary>
    ''' <param name="idCliente">El Id del cliente.</param>
    ''' <returns>Los datos del vehículo.</returns>
    Public Shared Function ObtenerVehiculoPorIdCliente(ByRef idCliente As Integer) As Vehiculo

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT Marca, Modelo, Matricula, PrecioBase
                                         FROM   Vehiculos
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el vehículo del cliente seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim vehiculo As Vehiculo

            While datos.Read()

                Dim marca As String = datos.GetString("Marca")
                Dim modelo As String = datos.GetString("Modelo")
                Dim matricula As String = datos.GetString("Matricula")
                Dim precioBase As Decimal = datos.GetDecimal("PrecioBase")

                vehiculo = New Vehiculo(marca, modelo, matricula, precioBase)

            End While

            datos.Close()
            conexion.Close()

            Return vehiculo

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Inserta un vehículo.
    ''' </summary>
    ''' <param name="vehiculo">Datos del vehículo a insertar.</param>
    ''' <returns>True: El vehículo se ha insertado. False: El vehículo no se ha insertado.</returns>
    Public Shared Function InsertarVehiculo(ByRef vehiculo As Vehiculo) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO Vehiculos (IdVehiculo, Matricula, Marca, Modelo, IdCliente, IdGaraje, IdPlaza, PrecioBase, PrecioTotal) 
                                         VALUES (NULL, @Matricula, @Marca, @Modelo, @IdCliente, @IdGaraje, @IdPlaza, @PrecioBase, @PrecioTotal);", conexion)

        comando.Parameters.AddWithValue("@Matricula", vehiculo.Matricula)
        comando.Parameters.AddWithValue("@Marca", vehiculo.Marca)
        comando.Parameters.AddWithValue("@Modelo", vehiculo.Modelo)
        comando.Parameters.AddWithValue("@IdCliente", vehiculo.Cliente.Id)
        comando.Parameters.AddWithValue("@IdGaraje", vehiculo.IdGaraje)
        comando.Parameters.AddWithValue("@IdPlaza", vehiculo.IdPlaza)
        comando.Parameters.AddWithValue("@PrecioBase", vehiculo.PrecioBase)
        comando.Parameters.AddWithValue("@PrecioTotal", vehiculo.PrecioTotal)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al insertar el vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un vehículo seleccionado.
    ''' </summary>
    ''' <param name="vehiculo">Datos del vehículo seleccionado.</param>
    ''' <returns>True: Se ha modificado los datos del vehículo. False: No se ha modificado los datos del vehículo.</returns>
    Public Shared Function ModificarVehiculo(ByRef vehiculo As Vehiculo) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Vehiculos
                                         SET    Matricula = @Matricula, Marca = @Marca, Modelo = @Modelo, IdGaraje = @IdGaraje, IdPlaza = @IdPlaza, PrecioBase = @PrecioBase, PrecioTotal = @PrecioTotal
                                         WHERE  IdVehiculo = @IdVehiculo;", conexion)

        comando.Parameters.AddWithValue("@Matricula", vehiculo.Matricula)
        comando.Parameters.AddWithValue("@Marca", vehiculo.Marca)
        comando.Parameters.AddWithValue("@Modelo", vehiculo.Modelo)
        comando.Parameters.AddWithValue("@IdGaraje", vehiculo.IdGaraje)
        comando.Parameters.AddWithValue("@IdPlaza", vehiculo.IdPlaza)
        comando.Parameters.AddWithValue("@PrecioBase", vehiculo.PrecioBase)
        comando.Parameters.AddWithValue("@PrecioTotal", vehiculo.PrecioTotal)
        comando.Parameters.AddWithValue("@IdVehiculo", vehiculo.Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar los datos del vehículo seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina un vehículo a partir de su Id.
    ''' </summary>
    ''' <param name="idVehiculo">Id del vehículo a eliminar.</param>
    ''' <returns>True: Se ha eliminado el vehículo. False: No se ha eliminado el vehículo.</returns>
    Public Shared Function EliminarVehiculoPorId(ByRef idVehiculo As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Vehiculos
                                         WHERE  IdVehiculo = @IdVehiculo;", conexion)

        comando.Parameters.AddWithValue("@IdVehiculo", idVehiculo)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el vehículo seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina los vehículos a partir del Id de un cliente.
    ''' </summary>
    ''' <param name="idCliente">Id del cliente.</param>
    ''' <returns>True: Se han eliminado los vehículos del cliente. False: No se han eliminado los vehículos del cliente.</returns>
    Public Shared Function EliminarVehiculosPorIdCliente(ByRef idCliente As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Vehiculos
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar los vehículos del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function

    Public Sub New(id As Integer, matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal, precioTotal As Decimal)               ' Para modificar los datos de un vehículo seleccionado y mostrarlos en el DataGrid.

        Me.Id = id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = precioBase
        Me.PrecioTotal = precioTotal

    End Sub

    Public Sub New(id As Integer, matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioTotal As Decimal)               ' Para mostrar los vehículos en el DataGrid.

        Me.Id = id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioTotal = precioTotal

    End Sub

    Public Sub New(matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal, precioTotal As Decimal)                  ' Para crear un vehículo.

        Me.Id = Id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = precioBase
        Me.PrecioTotal = precioTotal

    End Sub

    Public Sub New(marca As String, modelo As String, matricula As String, precioBase As Decimal)                  ' Para mostrar sus datos en la factura.

        Me.Marca = marca
        Me.Modelo = modelo
        Me.Matricula = matricula
        Me.PrecioBase = precioBase

    End Sub

End Class
