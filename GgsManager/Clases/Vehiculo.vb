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
        Dim comando As New MySqlCommand(String.Format("SELECT Veh.IdVehiculo, Veh.Matricula, Veh.Marca, Veh.Modelo, Cli.Nombre, Cli.Apellidos, Veh.IdGaraje, Veh.IdPlaza, Veh.PrecioBase, Veh.PrecioTotal
                                                       FROM   Vehiculos Veh
                                                              JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente
                                                       WHERE  Veh.IdGaraje = {0};", idGaraje), conexion)
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

                listaVehiculos.Add(New Vehiculo(id, matricula, marca, modelo, cliente, idGj, idPl, precioBase, precioTotal))

            End While

            datos.Close()
            conexion.Close()

            Return listaVehiculos

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
        Dim porcentajeIva As Decimal = (vehiculo.PrecioBase * Foo.LeerIVA()) / 100
        Dim precioTotal As Decimal = vehiculo.PrecioBase + porcentajeIva

        Dim comando As New MySqlCommand(String.Format("INSERT INTO Vehiculos (IdVehiculo, Matricula, Marca, Modelo, IdCliente, IdGaraje, IdPlaza, PrecioBase, PrecioTotal) 
                                                       VALUES (NULL, '{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, {7});", vehiculo.Matricula, vehiculo.Marca, vehiculo.Modelo,
                                                       vehiculo.Cliente.Id, vehiculo.IdGaraje, vehiculo.IdPlaza, vehiculo.PrecioBase.ToString("F2", CultureInfo.InvariantCulture), precioTotal.ToString("F2", CultureInfo.InvariantCulture)), conexion)
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
    ''' Obtiene el nuevo Id de la tabla "Vehiculos", (ultimoId + 1) para guardar su imagen.
    ''' </summary>
    ''' <returns>El nuevo Id de la imagen.</returns>
    Public Shared Function ObtenerNuevoIdVehiculos() As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT MAX(IdVehiculo) 
                                         FROM   Vehiculos;", conexion)
        Dim ultimoId As Integer

        Try
            ultimoId = CType(comando.ExecuteScalar(), Integer)
            ultimoId += 1

            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el nuevo Id de la tabla Vehiculos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return ultimoId

    End Function


    ''' <summary>
    ''' Modifica los datos de un vehículo seleccionado.
    ''' </summary>
    ''' <param name="vehiculo">Datos del vehículo seleccionado.</param>
    ''' <returns>True: Se ha modificado los datos del vehículo. False: No se ha modificado los datos del vehículo.</returns>
    Public Shared Function ModificarVehiculo(ByRef vehiculo As Vehiculo) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("UPDATE Vehiculos
                                                       SET    Matricula = '{0}', Marca = '{1}', Modelo = '{2}', IdGaraje = {3}, IdPlaza = {4}, PrecioBase = {5}, PrecioTotal = {6}
                                                       WHERE  IdVehiculo = {7};", vehiculo.Matricula, vehiculo.Marca, vehiculo.Modelo, vehiculo.IdGaraje, vehiculo.IdPlaza, vehiculo.PrecioBase, vehiculo.PrecioTotal, vehiculo.Id), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar los datos del vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

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
        Dim comando As New MySqlCommand(String.Format("DELETE FROM Vehiculos
                                                       WHERE  IdVehiculo = {0};", idVehiculo), conexion)
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
        Dim comando As New MySqlCommand(String.Format("DELETE FROM Vehiculos
                                                       WHERE  IdCliente = {0};", idCliente), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar los vehículos del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function

    Public Sub New(id As Integer, matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal, precioTotal As Decimal)

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

    Public Sub New(matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal)

        Me.Id = Id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = precioBase

    End Sub

End Class
