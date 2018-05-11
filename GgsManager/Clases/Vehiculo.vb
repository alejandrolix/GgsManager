Imports MySql.Data.MySqlClient
Imports NPoco

''' <summary>
''' Representa un vehículo de la tabla "Vehiculos".
''' </summary>
Public Class Vehiculo

    ''' <summary>
    ''' El Id del Vehículo del Cliente.
    ''' </summary>   
    <Column("IdVehiculo")>
    Property Id As Integer

    ''' <summary>
    ''' La Matrícula del Vehículo del Cliente.
    ''' </summary>    
    Property Matricula As String

    ''' <summary>
    ''' La Marca del Vehículo del Cliente.
    ''' </summary>    
    Property Marca As String

    ''' <summary>
    ''' El Modelo del Vehículo del Cliente.
    ''' </summary>    
    Property Modelo As String

    ''' <summary>
    ''' El Cliente.
    ''' </summary>    
    Property Cliente As Cliente

    ''' <summary>
    ''' El Id de la Plaza en la que está Vehículo del Cliente.
    ''' </summary>    
    Property IdPlaza As Integer

    ''' <summary>
    ''' El Id del Garaje en el que está el Vehículo del Cliente.
    ''' </summary>    
    Property IdGaraje As Integer

    ''' <summary>
    ''' El Precio Base, (sin I.V.A.) que abona Mensualmente el Cliente.
    ''' </summary>    
    Property PrecioBase As Decimal


    ''' <summary>
    ''' Obtiene todos los vehículos a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje seleccionado.</param>
    ''' <returns>Array con los vehículos del garaje seleccionado.</returns>
    Public Shared Function ObtenerVehiculosPorIdGaraje(ByRef idGaraje As Integer) As Vehiculo()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayVehiculos As Vehiculo() = conexion.Query(Of Vehiculo)(Sql.Builder.Append("SELECT Veh.IdVehiculo, Veh.Matricula, Veh.Marca, Veh.Modelo, Cli.Nombre, Cli.Apellidos, Veh.IdGaraje, Veh.IdPlaza, Veh.PrecioBase
                                                                                           FROM   Vehiculos Veh
                                                                                                  JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente
                                                                                           WHERE  Veh.IdGaraje = @0
                                                                                           ORDER BY Cli.Apellidos;", idGaraje)).ToArray()
        conexion.CloseSharedConnection()

        Return arrayVehiculos

    End Function


    ''' <summary>
    ''' Obtiene los datos del vehículo a partir del Id de un cliente.
    ''' </summary>
    ''' <param name="idCliente">El Id del cliente.</param>
    ''' <returns>Los datos del vehículo.</returns>
    Public Shared Function ObtenerVehiculoPorIdCliente(ByRef idCliente As Integer) As Vehiculo

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim vehiculo As Vehiculo = conexion.Query(Of Vehiculo)(Sql.Builder.Append("SELECT Marca, Modelo, Matricula, PrecioBase
                                                                                   FROM   Vehiculos
                                                                                   WHERE  IdCliente = @0;", idCliente)).First()
        conexion.CloseSharedConnection()

        Return vehiculo

    End Function


    ''' <summary>
    ''' Inserta un vehículo.
    ''' </summary>    
    ''' <returns>True: El vehículo se ha insertado. False: El vehículo no se ha insertado.</returns>
    Public Function Insertar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO Vehiculos (IdVehiculo, Matricula, Marca, Modelo, IdCliente, IdGaraje, IdPlaza, PrecioBase, PrecioTotal) 
                                         VALUES (NULL, @Matricula, @Marca, @Modelo, @IdCliente, @IdGaraje, @IdPlaza, @PrecioBase, @PrecioTotal);", conexion)

        Dim precioTotal As Decimal = CalcularPrecioTotal(PrecioBase)

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
                                         SET    Matricula = @Matricula, Marca = @Marca, Modelo = @Modelo, PrecioBase = @PrecioBase, PrecioTotal = @PrecioTotal
                                         WHERE  IdVehiculo = @IdVehiculo;", conexion)

        Dim precioTotal As Decimal = CalcularPrecioTotal()

        comando.Parameters.AddWithValue("@Matricula", Matricula)
        comando.Parameters.AddWithValue("@Marca", Marca)
        comando.Parameters.AddWithValue("@Modelo", Modelo)
        comando.Parameters.AddWithValue("@PrecioBase", PrecioBase)
        comando.Parameters.AddWithValue("@PrecioTotal", precioTotal)
        comando.Parameters.AddWithValue("@IdVehiculo", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina un vehículo.
    ''' </summary>    
    ''' <returns>True: Se ha eliminado el vehículo. False: No se ha eliminado el vehículo.</returns>
    Public Shared Function Eliminar(ByRef vehiculo As Vehiculo) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim eliminacion As Integer

        Try
            eliminacion = conexion.Delete("Vehiculos", "IdVehiculo", vehiculo)

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return eliminacion >= 1

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

    Public Sub New(id As Integer, matricula As String, marca As String, modelo As String, cliente As Cliente, precioBase As Decimal)               ' Para modificar los datos de un vehículo.

        Me.Id = id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
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

    Public Sub New()

    End Sub

End Class
