Imports MySql.Data.MySqlClient

''' <summary>
''' Representa un vehículo de la tabla "Vehiculos" de la base de datos.
''' </summary>
Public Class Vehiculo

    Property Id As String
    Property Matricula As String
    Property Marca As String
    Property Modelo As String
    Property Cliente As Cliente
    Property IdPlaza As Integer
    Property IdGaraje As Integer
    Property PrecioBase As Decimal
    Property PrecioTotal As Decimal
    Property ArrayUrlFotos As String()
    Property UrlFotos As String

    'Public Shared Function ObtenerVehiculos() As List(Of Vehiculo)

    '    Dim conexion As MySqlConnection = Foo.ConexionABd()
    '    Dim comando As New MySqlCommand("SELECT Veh.IdVehiculo, Veh.Matricula, Veh.Marca, Veh.Modelo, Cli.Nombre, Cli.Apellidos, Veh.Total
    '                                     FROM   Vehiculos Veh
    '                                         JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente;", conexion)
    '    Dim datos As MySqlDataReader

    '    Try
    '        datos = comando.ExecuteReader()

    '    Catch ex As Exception

    '        MessageBox.Show("Ha habido un problema al obtener los vehículos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

    '    End Try

    '    If datos IsNot Nothing Then

    '        Dim listaVehiculos As New List(Of Vehiculo)()

    '        While datos.Read()

    '            Dim id As Integer = datos.GetInt32("IdVehiculo")
    '            Dim matricula As String = datos.GetString("Matricula")
    '            Dim marca As String = datos.GetString("Marca")
    '            Dim modelo As String = datos.GetString("Modelo")
    '            Dim cliente As Cliente = New Cliente(datos.GetString("Nombre"), datos.GetString("Apellidos"))
    '            Dim total As Decimal = datos.GetDecimal("Total")

    '            listaVehiculos.Add(New Vehiculo(id, matricula, marca, modelo, cliente, total))

    '        End While

    '        datos.Close()
    '        conexion.Close()

    '        Return listaVehiculos

    '    End If

    '    Return Nothing

    'End Function


    ''' <summary>
    ''' Obtiene todos los vehículos a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje seleccionado.</param>
    ''' <returns>Lista con los vehículos del garaje seleccionado.</returns>
    Public Shared Function ObtenerVehiculosFromIdGaraje(ByRef idGaraje As Integer) As List(Of Vehiculo)

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
        Dim comando As MySqlCommand

        If vehiculo.ArrayUrlFotos Is Nothing Then

            comando = New MySqlCommand(String.Format("INSERT INTO Vehiculos (IdVehiculo, Matricula, Marca, Modelo, IdCliente, IdGaraje, IdPlaza, Base, Total, URLFotos)
                                                      VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, NULL);", vehiculo.Id, vehiculo.Matricula, vehiculo.Marca, vehiculo.Modelo, vehiculo.Cliente.Id, vehiculo.IdGaraje, vehiculo.IdPlaza, vehiculo.PrecioBase, vehiculo.PrecioTotal), conexion)
        Else

            comando = New MySqlCommand(String.Format("INSERT INTO Vehiculos (IdVehiculo, Matricula, Marca, Modelo, IdCliente, IdGaraje, IdPlaza, Base, Total, URLFotos)
                                                      VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, '{9}');", vehiculo.Id, vehiculo.Matricula, vehiculo.Marca, vehiculo.Modelo, vehiculo.Cliente.Id, vehiculo.IdGaraje, vehiculo.IdPlaza, vehiculo.PrecioBase, vehiculo.PrecioTotal, vehiculo.UrlFotos), conexion)
        End If

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
                                                       SET    Matricula = '{0}', Marca = '{1}', Modelo = '{2}', IdGaraje = {3}, IdPlaza = {4}, PrecioBase = {5}, PrecioTotal = {6}", vehiculo.Matricula, vehiculo.Marca, vehiculo.Modelo, vehiculo.IdGaraje, vehiculo.IdPlaza, vehiculo.PrecioBase, vehiculo.PrecioTotal), conexion)
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
                                                       WHERE  IdVehiculo = {0}", idVehiculo), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el vehículo seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function

    Public Sub New(id As String, matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, precioBase As Decimal, precioTotal As Decimal)

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

    Public Sub New(matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, base As Decimal, total As Decimal, urlFotos As String)

        Me.Id = Id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = base
        Me.PrecioTotal = total
        Me.UrlFotos = urlFotos

        'If Foo.HayTexto(urlFotos) Then

        '    Me.ArrayUrlFotos = urlFotos.Split(" ")

        'End If

    End Sub

End Class
