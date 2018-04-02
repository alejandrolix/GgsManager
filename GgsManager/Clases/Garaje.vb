Imports MySql.Data.MySqlClient

''' <summary>
''' Representa un garaje de la tabla "Garajes".
''' </summary>
Public Class Garaje

    Property Id As Integer
    Property Nombre As String
    Property Direccion As String
    Property NumPlazas As Integer
    Property NumPlazasLibres As Integer
    Property NumPlazasOcupadas As Integer
    Property Observaciones As String


    ''' <summary>
    ''' Obtiene el nombre del garaje.
    ''' </summary>
    ''' <returns>El nombre del garaje.</returns>
    Public Overrides Function ToString() As String

        Return Nombre

    End Function


    ''' <summary>
    ''' Obtiene todos los garajes.
    ''' </summary>
    ''' <returns>Array con todos los garajes.</returns>
    Public Shared Function ObtenerGarajes() As Garaje()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones
                                         FROM   Garajes
                                         ORDER BY Nombre;", conexion)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaGarajes As List(Of Garaje) = Nothing

            If datos.HasRows Then

                listaGarajes = New List(Of Garaje)()

                While datos.Read()

                    Dim id As Integer = datos.GetInt32("IdGaraje")
                    Dim nombre As String = datos.GetString("Nombre")
                    Dim direccion As String = datos.GetString("Direccion")
                    Dim numPlazas As Integer = datos.GetInt32("NumPlazas")
                    Dim numPlazasLibres As Integer = datos.GetInt32("NumPlazasLibres")
                    Dim numPlazasOcupadas As Integer = datos.GetInt32("NumPlazasOcupadas")
                    Dim observaciones As String

                    If datos.IsDBNull(6) Then               ' Si el contenido de la 2ª columna, (Observaciones), es NULL.

                        observaciones = ""
                    Else

                        observaciones = datos.GetString("Observaciones")

                    End If

                    listaGarajes.Add(New Garaje(id, nombre, direccion, numPlazas, numPlazasLibres, numPlazasOcupadas, observaciones))

                End While

                datos.Close()

            End If

            conexion.Close()

            Return listaGarajes.ToArray()

        End If

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene los nombres de todos los garajes.
    ''' </summary>
    ''' <returns>Array con los nombres de los garajes.</returns>
    Public Shared Function ObtenerNombresGarajes() As Garaje()

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdGaraje, Nombre
                                         FROM   Garajes
                                         ORDER BY Nombre;", conexion)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los nombres de los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaGarajes As New List(Of Garaje)

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdGaraje")
                Dim nombre As String = datos.GetString("Nombre")

                Dim garaje As New Garaje(id, nombre)
                listaGarajes.Add(garaje)

            End While

            conexion.Close()
            datos.Close()

            Return listaGarajes.ToArray()

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene el nombre de un garaje.
    ''' </summary>    
    ''' <param name="idGaraje">El Id de un garaje.</param>
    ''' <returns>El nombre del garaje correspondiente.</returns>
    Public Shared Function ObtenerNombreGarajePorId(ByRef idGaraje As Integer) As String

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT Nombre
                                         FROM   Garajes
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim nombreGaraje As String

        Try
            nombreGaraje = CType(comando.ExecuteScalar, String)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el nombre del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return nombreGaraje

    End Function


    ''' <summary>
    ''' Obtiene el último Id de la tabla "Garajes".
    ''' </summary>
    ''' <returns>El último Id de la tabla "Garajes".</returns>
    Public Shared Function ObtenerUltimoIdGarajes() As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT MAX(IdGaraje)
                                         FROM   Garajes;", conexion)

        Dim ultimoId As Integer

        Try
            ultimoId = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el último Id del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return ultimoId

    End Function

    ''' <summary>
    ''' Elimina un garaje.
    ''' </summary>    
    ''' <returns>True: El garaje se ha eliminado. False: El garaje no se ha eliminado.</returns>
    Public Function Eliminar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Garajes 
                                         WHERE IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Inserta un garaje.
    ''' </summary>    
    ''' <returns>True: El garaje se ha insertado. False: El garaje no se ha insertado.</returns>
    Public Function Insertar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As MySqlCommand

        If Foo.HayTexto(Observaciones) Then

            comando = New MySqlCommand("INSERT INTO Garajes (IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones) 
                                        VALUES (NULL, @Nombre, @Direccion, @NumPlazas, @NumPlazasLibres, 0, @Observaciones);", conexion)

            comando.Parameters.AddWithValue("@Observaciones", Observaciones)
        Else

            comando = New MySqlCommand("INSERT INTO Garajes (IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones) 
                                        VALUES (NULL, @Nombre, @Direccion, @NumPlazas, @NumPlazasLibres, 0, NULL);", conexion)
        End If

        comando.Parameters.AddWithValue("@Nombre", Nombre)
        comando.Parameters.AddWithValue("@Direccion", Direccion)
        comando.Parameters.AddWithValue("@NumPlazas", NumPlazas)
        comando.Parameters.AddWithValue("@NumPlazasLibres", NumPlazas)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un garaje.
    ''' </summary>    
    ''' <returns>True: Se ha modificado el garaje. False: No se ha modificado el garaje.</returns>
    Public Function Modificar() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As MySqlCommand

        If Foo.HayTexto(Observaciones) Then

            comando = New MySqlCommand("UPDATE Garajes 
                                        SET    Nombre = @Nombre, Direccion = @Direccion, Observaciones = @Observaciones
                                        WHERE  IdGaraje = @IdGaraje;", conexion)

            comando.Parameters.AddWithValue("@Observaciones", Observaciones)
        Else

            comando = New MySqlCommand("UPDATE Garajes 
                                        SET    Nombre = @Nombre, Direccion = @Direccion, Observaciones = NULL 
                                        WHERE  IdGaraje = @IdGaraje;", conexion)
        End If

        comando.Parameters.AddWithValue("@Nombre", Nombre)
        comando.Parameters.AddWithValue("@Direccion", Direccion)
        comando.Parameters.AddWithValue("@IdGaraje", Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function

    Public Sub New(id As Integer, nombre As String, direccion As String, numPlazas As Integer, numPlazasLibres As Integer, numPlazasOcupadas As Integer, observaciones As String)               ' Para mostrar los garajes en el DataGrid.

        Me.Id = id
        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.NumPlazasLibres = numPlazasLibres
        Me.NumPlazasOcupadas = numPlazasOcupadas
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(nombre As String, direccion As String, numPlazas As Integer, observaciones As String)            ' Para crear un nuevo garaje,

        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(id As Integer, nombre As String, direccion As String, numPlazas As Integer, observaciones As String)             ' Para modificar los datos de un garaje seleccionado.

        Me.Id = id
        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(id As Integer, nombre As String)             ' Para elegir un garaje.

        Me.Id = id
        Me.Nombre = nombre

    End Sub

End Class
