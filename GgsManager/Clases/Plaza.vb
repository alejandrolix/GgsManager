Imports MySql.Data.MySqlClient

''' <summary>
''' Representa una plaza de la tabla "Plazas".
''' </summary>
Public Class Plaza

    Property Id As Integer
    Property Marca As String
    Property Matricula As String
    Property Modelo As String
    Property Situacion As String


    ''' <summary>
    ''' Obtiene el Id de la plaza.
    ''' </summary>
    ''' <returns>El Id de la plaza.</returns>
    Public Overrides Function ToString() As String

        Dim id As String = CType(Me.Id, String)

        Return id

    End Function


    ''' <summary>
    ''' Obtiene todos los Id de las plazas libres a partir del Id del garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje.</param>
    ''' <returns>Array con las plazas libres.</returns>
    Public Shared Function ObtenerIdPlazasLibresPorIdGaraje(ByRef idGaraje As Integer) As Plaza()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdPlaza
                                         FROM   Plazas
                                         WHERE  IdGaraje = @IdGaraje AND IdSituacion = (
                                                                                        SELECT IdSituacion
                                                                                        FROM   SituacionesPlaza
                                                                                        WHERE  Tipo = 'Libre')
                                         ORDER BY IdPlaza;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener las plazas libres del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaPlazas As New List(Of Plaza)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdPlaza")

                listaPlazas.Add(New Plaza(id))

            End While

            conexion.Close()
            datos.Close()

            Return listaPlazas.ToArray()

        End If

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene todos los Id de las plazas ocupadas a partir del Id del garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje.</param>
    ''' <returns>Array con las plazas ocupadas.</returns>
    Public Shared Function ObtenerIdPlazasOcupadasPorIdGaraje(ByRef idGaraje As Integer) As Plaza()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdPlaza
                                         FROM   Plazas
                                         WHERE  IdGaraje = @IdGaraje AND IdSituacion = (
                                                                                        SELECT IdSituacion
                                                                                        FROM   SituacionesPlaza
                                                                                        WHERE  Tipo = 'Ocupada')
                                         ORDER BY IdPlaza;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener las plazas ocupadas del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaPlazas As New List(Of Plaza)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdPlaza")

                listaPlazas.Add(New Plaza(id))

            End While

            conexion.Close()
            datos.Close()

            Return listaPlazas.ToArray()

        End If

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene todas las plazas a partir del Id del garaje.
    ''' </summary>
    ''' <param name="idGaraje">Id del garaje.</param>
    ''' <returns>Array con los datos de las plazas.</returns>
    Public Shared Function ObtenerPlazasPorIdGaraje(ByRef idGaraje As Integer) As Plaza()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT Veh.Matricula, Veh.Marca, Veh.Modelo, Plz.IdPlaza, Sit.Tipo
                                         FROM   Vehiculos Veh 
	                                            JOIN Plazas Plz ON Plz.IdPlaza = Veh.IdPlaza 
                                                JOIN SituacionesPlaza Sit ON Sit.IdSituacion = Plz.IdSituacion 
                                         WHERE  Veh.IdGaraje = @IdGaraje
                                         ORDER BY Plz.IdPlaza;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un error al obtener las plazas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaPlazas As New List(Of Plaza)()

            While datos.Read()

                Dim matricula As String = datos.GetString("Matricula")
                Dim marca As String = datos.GetString("Marca")
                Dim modelo As String = datos.GetString("Modelo")
                Dim idPlaza As Integer = datos.GetInt32("IdPlaza")
                Dim situacion As String = datos.GetString("Tipo")

                Dim plaza As New Plaza(matricula, marca, modelo, idPlaza, situacion)
                listaPlazas.Add(plaza)

            End While

            datos.Close()
            conexion.Close()

            Return listaPlazas.ToArray()

        End If

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene los datos de las plazas libres a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>    
    ''' <returns>DataSet con los datos de las plazas libres.</returns>
    Public Shared Function ObtenerDatosPlazasLibresPorIdGaraje(ByRef idGaraje As Integer) As DtPlazas

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim dtPlazas As New DtPlazas()

        Try
            Dim adaptador As New MySqlDataAdapter("SELECT Plz.IdPlaza, CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Cliente', Veh.Matricula, Veh.Marca, Veh.Modelo, ROUND(Veh.PrecioBase * (@PorcIva / 100) + Veh.PrecioBase, 2) AS 'PrecioTotal'
                                                   FROM   Plazas Plz
                                                          JOIN Vehiculos Veh ON Veh.IdPlaza = Plz.IdPlaza
                                                          JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente       
                                                   WHERE  Plz.IdGaraje = @IdGaraje AND Plz.IdSituacion = (
												                                                           SELECT IdSituacion
                                                                                                           FROM   SituacionesPlaza
                                                                                                           WHERE  Tipo = 'Libre');", conexion)
            adaptador.SelectCommand.Parameters.AddWithValue("@PorcIva", Foo.LeerIVA())
            adaptador.SelectCommand.Parameters.AddWithValue("@IdGaraje", idGaraje)

            adaptador.Fill(dtPlazas, "Plazas")

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los datos de las plazas libres.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return dtPlazas

    End Function


    ''' <summary>
    ''' Obtiene los datos de las plazas ocupadas a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>    
    ''' <returns>DataSet con los datos de las plazas ocupadas.</returns>
    Public Shared Function ObtenerDatosPlazasOcupadasPorIdGaraje(ByRef idGaraje As Integer) As DtPlazas

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim dtPlazas As New DtPlazas()

        Try
            Dim adaptador As New MySqlDataAdapter("SELECT Plz.IdPlaza, CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Cliente', Veh.Matricula, Veh.Marca, Veh.Modelo, ROUND(Veh.PrecioBase * (@PorcIva / 100) + Veh.PrecioBase, 2) AS 'PrecioTotal'
                                                   FROM   Plazas Plz
                                                          JOIN Vehiculos Veh ON Veh.IdPlaza = Plz.IdPlaza
                                                          JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente       
                                                   WHERE  Plz.IdGaraje = @IdGaraje AND Plz.IdSituacion = (
												                                                           SELECT IdSituacion
                                                                                                           FROM   SituacionesPlaza
                                                                                                           WHERE  Tipo = 'Ocupada');", conexion)
            adaptador.SelectCommand.Parameters.AddWithValue("@PorcIva", Foo.LeerIVA())
            adaptador.SelectCommand.Parameters.AddWithValue("@IdGaraje", idGaraje)

            adaptador.Fill(dtPlazas, "Plazas")

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los datos de las plazas ocupadas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return dtPlazas

    End Function


    ''' <summary>
    ''' Obtiene los datos de todas las plazas a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>    
    ''' <returns>DataSet con los datos de todas las plazas.</returns>
    Public Shared Function ObtenerDatosTodasPlazasPorIdGaraje(ByRef idGaraje As Integer) As DtPlazas

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim dtPlazas As New DtPlazas()

        Try
            Dim adaptador As New MySqlDataAdapter("SELECT Plz.IdPlaza, CONCAT(Cli.Nombre, ' ', Cli.Apellidos) AS 'Cliente', Veh.Matricula, Veh.Marca, Veh.Modelo, ROUND(Veh.PrecioBase * (@PorcIva / 100) + Veh.PrecioBase, 2) AS 'PrecioTotal'
                                                   FROM   Plazas Plz
                                                          JOIN Vehiculos Veh ON Veh.IdPlaza = Plz.IdPlaza
                                                          JOIN Clientes Cli ON Cli.IdCliente = Veh.IdCliente       
                                                   WHERE  Plz.IdGaraje = @IdGaraje AND Plz.IdSituacion IN (
												                                                            SELECT IdSituacion
                                                                                                            FROM   SituacionesPlaza);", conexion)
            adaptador.SelectCommand.Parameters.AddWithValue("@PorcIva", Foo.LeerIVA())
            adaptador.SelectCommand.Parameters.AddWithValue("@IdGaraje", idGaraje)

            adaptador.Fill(dtPlazas, "Plazas")

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los datos de todas las plazas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return dtPlazas

    End Function


    ''' <summary>
    ''' Añade el número de plazas introducidas al garaje. Empieza desde el número 1 hasta el número introducido.
    ''' </summary>
    ''' <param name="numPlazas">El número total de plazas a insertar.</param>
    ''' <param name="idGaraje">El Id del garaje.</param>
    ''' <returns>True: Se han insertado todas las plazas. False: No se han insertado todas las plazas.</returns>
    Public Shared Function AddPlazasAGaraje(ByRef numPlazas As Integer, ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("", conexion)
        Dim numPlzInsertadas As Integer = 0

        For i As Integer = 1 To numPlazas Step 1                ' Añadimos uno a uno cada plaza.

            comando.CommandText = "INSERT INTO Plazas (IdPlaza, IdGaraje, IdSituacion) VALUES (@IdPlaza, @IdGaraje, 1);"

            comando.Parameters.AddWithValue("@IdPlaza", i)
            comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
            comando.ExecuteNonQuery()

            numPlzInsertadas += 1

        Next

        conexion.Close()
        Return numPlzInsertadas = numPlazas

    End Function


    ''' <summary>
    ''' Cambia la situación de una plaza a Libre.
    ''' </summary>    
    ''' <param name="idPlaza">El Id de la plaza dónde está el vehículo.</param>
    ''' <param name="idGaraje">El Id del garaje donde está el vehículo.</param>
    ''' <returns>True: Se ha cambiado la situación de la plaza. False: No se ha cambiado la situación de la plaza.</returns>
    Public Shared Function CambiarSituacionPlazaALibre(ByRef idPlaza As Integer, ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Plazas
                                         SET    IdSituacion = 1
                                         WHERE  IdPlaza = @IdPlaza AND IdGaraje = @IdGaraje", conexion)

        comando.Parameters.AddWithValue("@IdPlaza", idPlaza)
        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al cambiar la situación de la plaza a Libre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Cambia la situación de una plaza a Ocupada.
    ''' </summary>
    ''' <param name="idPlaza">El Id de la plaza dónde está el vehículo.</param>
    ''' <param name="idGaraje">El Id del garaje donde está el vehículo.</param>
    ''' <returns>True: Se ha cambiado la situación de la plaza. False: No se ha cambiado la situación de la plaza.</returns>
    Public Shared Function CambiarSituacionPlazaAOcupada(ByRef idPlaza As Integer, ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Plazas
                                         SET    IdSituacion = 2
                                         WHERE  IdPlaza = @IdPlaza AND IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdPlaza", idPlaza)
        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al cambiar la situación de la plaza a Ocupado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function

    Public Sub New(matricula As String, marca As String, modelo As String, idPlaza As Integer, situacion As String)              ' Para mostrar una plaza en el DataGrid.

        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Id = idPlaza
        Me.Situacion = situacion

    End Sub

    Public Sub New(id As Integer)               ' Para mostrar los Ids de las plazas en el ComboBox de "VntAddVehiculo".

        Me.Id = id

    End Sub

End Class
