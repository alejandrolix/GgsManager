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
    ''' <returns>Array con los vehículos del garaje seleccionado.</returns>
    Public Shared Function ObtenerVehiculosPorIdGaraje(ByRef idGaraje As Integer) As Vehiculo()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT Veh.IdVehiculo, Veh.Matricula, Veh.Marca, Veh.Modelo, Cli.Nombre, Cli.Apellidos, Veh.IdGaraje, Veh.IdPlaza, Veh.PrecioBase
                                         FROM   Vehiculos Veh
                                                JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente
                                         WHERE  Veh.IdGaraje = @IdGaraje
                                         ORDER BY Cli.Apellidos;", conexion)

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

                Dim vehiculo As New Vehiculo(id, matricula, marca, modelo, cliente, idGj, idPl, precioBase)
                listaVehiculos.Add(vehiculo)

            End While

            datos.Close()
            conexion.Close()

            Return listaVehiculos.ToArray()

        End If

        conexion.Close()

        Return Nothing

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

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Inserta un vehículo.
    ''' </summary>    
    ''' <returns>True: El vehículo se ha insertado. False: El vehículo no se ha insertado.</returns>
    Public Function Insertar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO Vehiculos (IdVehiculo, Matricula, Marca, Modelo, IdCliente, IdGaraje, IdPlaza, PrecioBase, PrecioTotal) 
                                         VALUES (NULL, @Matricula, @Marca, @Modelo, @IdCliente, @IdGaraje, @IdPlaza, @PrecioBase, @PrecioTotal);", conexion)

        Dim precioTotal As Decimal = CalcularPrecioTotal()

        comando.Parameters.AddWithValue("@Matricula", Matricula)
        comando.Parameters.AddWithValue("@Marca", Marca)
        comando.Parameters.AddWithValue("@Modelo", Modelo)
        comando.Parameters.AddWithValue("@IdCliente", Cliente.Id)
        comando.Parameters.AddWithValue("@IdGaraje", IdGaraje)
        comando.Parameters.AddWithValue("@IdPlaza", IdPlaza)
        comando.Parameters.AddWithValue("@PrecioBase", PrecioBase)
        comando.Parameters.AddWithValue("@PrecioTotal", precioTotal)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al insertar el vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un vehículo.
    ''' </summary>    
    ''' <returns>True: Se ha modificado los datos del vehículo. False: No se ha modificado los datos del vehículo.</returns>
    Public Function Modificar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Vehiculos
                                         SET    Matricula = @Matricula, Marca = @Marca, Modelo = @Modelo, IdGaraje = @IdGaraje, IdPlaza = @IdPlaza, PrecioBase = @PrecioBase, PrecioTotal = @PrecioTotal
                                         WHERE  IdVehiculo = @IdVehiculo;", conexion)

        Dim precioTotal As Decimal = CalcularPrecioTotal()

        comando.Parameters.AddWithValue("@Matricula", Matricula)
        comando.Parameters.AddWithValue("@Marca", Marca)
        comando.Parameters.AddWithValue("@Modelo", Modelo)
        comando.Parameters.AddWithValue("@IdGaraje", IdGaraje)
        comando.Parameters.AddWithValue("@IdPlaza", IdPlaza)
        comando.Parameters.AddWithValue("@PrecioBase", PrecioBase)
        comando.Parameters.AddWithValue("@PrecioTotal", precioTotal)
        comando.Parameters.AddWithValue("@IdVehiculo", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar los datos del vehículo seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina un vehículo.
    ''' </summary>    
    ''' <returns>True: Se ha eliminado el vehículo. False: No se ha eliminado el vehículo.</returns>
    Public Function Eliminar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Vehiculos
                                         WHERE  IdVehiculo = @IdVehiculo;", conexion)

        comando.Parameters.AddWithValue("@IdVehiculo", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el vehículo seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina los vehículos a partir del Id de un cliente.
    ''' </summary>
    ''' <param name="idCliente">Id del cliente.</param>
    ''' <returns>True: Se han eliminado los vehículos del cliente. False: No se han eliminado los vehículos del cliente.</returns>
    Public Shared Function EliminarVehiculoPorIdCliente(ByRef idCliente As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Vehiculos
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar los vehículos del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Calcula el precio más el I.V.A. a partir del precio base.
    ''' </summary>
    ''' <returns>El precio más el I.V.A.</returns>
    Private Function CalcularPrecioTotal() As Decimal

        Dim porcentajeIva As Integer = Foo.LeerIVA()
        Dim porcPrecioTotal As Decimal = (PrecioBase * porcentajeIva) / 100
        Dim precioTotal As Decimal = PrecioBase + porcPrecioTotal

        Return precioTotal

    End Function


    ''' <summary>
    ''' Calcula el precio más el I.V.A. a partir del precio base.
    ''' </summary>
    ''' <param name="precioBase">El precio base.</param>
    ''' <returns>El precio más el I.V.A.</returns>
    Public Shared Function CalcularPrecioTotal(ByRef precioBase As Decimal) As Decimal

        Dim porcentajeIva As Integer = Foo.LeerIVA()
        Dim porcPrecioTotal As Decimal = (precioBase * porcentajeIva) / 100
        Dim precioConIva As Decimal = precioBase + porcPrecioTotal

        Return precioConIva

    End Function

    Public Sub New(id As Integer, matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal)               ' Para mostrar los vehículos en el DataGrid.

        Me.Id = id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = precioBase

    End Sub

    Public Sub New(matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal)                  ' Para crear un vehículo.

        Me.Id = Id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = precioBase

    End Sub

    Public Sub New(marca As String, modelo As String, matricula As String, precioBase As Decimal)                  ' Para mostrar sus datos en la factura.

        Me.Marca = marca
        Me.Modelo = modelo
        Me.Matricula = matricula
        Me.PrecioBase = precioBase

    End Sub

End Class
