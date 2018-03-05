Imports MySql.Data.MySqlClient

''' <summary>
''' Representa una plaza de la tabla "Plazas" de la base de datos.
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
    ''' <returns>Lista con las plazas libres.</returns>
    Public Shared Function ObtenerIdPlazasLibresPorIdGaraje(ByRef idGaraje As Integer) As List(Of Plaza)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT IdPlaza
                                                       FROM   Plazas
                                                       WHERE  IdGaraje = {0} AND IdSituacion = 1;", idGaraje), conexion)
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

            Return listaPlazas

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene todos los Id de las plazas ocupadas a partir del Id del garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje.</param>
    ''' <returns>Lista con las plazas ocupadas.</returns>
    Public Shared Function ObtenerIdPlazasOcupadasPorIdGaraje(ByRef idGaraje As Integer) As List(Of Plaza)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT IdPlaza
                                                       FROM   Plazas
                                                       WHERE  IdGaraje = {0} AND IdSituacion = 2;", idGaraje), conexion)
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

            Return listaPlazas

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene todas las plazas a partir del Id del garaje.
    ''' </summary>
    ''' <param name="idGaraje">Id del garaje.</param>
    ''' <returns>Lista con los datos de las plazas.</returns>
    Public Shared Function ObtenerPlazasPorIdGaraje(ByRef idGaraje As Integer) As List(Of Plaza)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT Veh.Matricula, Veh.Marca, Veh.Modelo, Plz.IdPlaza, Sit.Tipo
                                                       FROM   Vehiculos Veh 
	                                                          JOIN Plazas Plz ON Plz.IdPlaza = Veh.IdPlaza 
                                                              JOIN Situaciones Sit ON Sit.IdSituacion = Plz.IdSituacion 
                                                       WHERE Veh.IdGaraje = {0};", idGaraje), conexion)
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

            Return listaPlazas

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Añade el número de plazas introducidas al garaje. Empieza desde el número 1 hasta el número introducido.
    ''' </summary>
    ''' <param name="numPlazas">El número total de plazas a insertar.</param>
    ''' <param name="idGaraje">El Id del garaje.</param>
    ''' <returns>True: Se han insertado todas las plazas. False: No se han insertado todas las plazas.</returns>
    Public Shared Function AddPlazasToGaraje(ByRef numPlazas As Integer, ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("", conexion)
        Dim numPlzInsertadas As Integer = 0

        For i As Integer = 1 To numPlazas Step 1                ' Añadimos uno a uno cada plaza.

            comando.CommandText = String.Format("INSERT INTO Plazas (IdPlaza, IdGaraje, IdSituacion) VALUES ({0}, {1}, 1);", i, idGaraje)
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
    Public Shared Function CambiarSituacionPlazaToLibre(ByRef idPlaza As Integer, ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("UPDATE Plazas
                                                       SET    IdSituacion = 1
                                                       WHERE  IdPlaza = {0} AND IdGaraje = {1}", idPlaza, idGaraje), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al cambiar la situación de la plaza a Libre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Cambia la situación de una plaza a Ocupada.
    ''' </summary>
    ''' <param name="idPlaza">El Id de la plaza dónde está el vehículo.</param>
    ''' <param name="idGaraje">El Id del garaje donde está el vehículo.</param>
    ''' <returns>True: Se ha cambiado la situación de la plaza. False: No se ha cambiado la situación de la plaza.</returns>
    Public Shared Function CambiarSituacionPlazaToOcupada(ByRef idPlaza As Integer, ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("UPDATE Plazas
                                                       SET    IdSituacion = 2
                                                       WHERE  IdPlaza = {0} AND IdGaraje = {1}", idPlaza, idGaraje), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al cambiar la situación de la plaza a Ocupado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

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
