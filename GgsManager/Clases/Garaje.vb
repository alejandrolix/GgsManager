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
    ''' <returns>Lista con todos los garajes.</returns>
    Public Shared Function ObtenerGarajes() As List(Of Garaje)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones
                                         FROM   Garajes;", conexion)
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
                conexion.Close()

            End If

            Return listaGarajes

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Obtiene los nombres de todos los garajes.
    ''' </summary>
    ''' <returns>Lista con los nombres de los garajes.</returns>
    Public Shared Function ObtenerNombresGarajes() As List(Of Garaje)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdGaraje, Nombre
                                         FROM   Garajes", conexion)
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

            datos.Close()
            conexion.Close()

            Return listaGarajes

        End If

        Return Nothing

    End Function

    Public Shared Function ObtenerNombreGarajePorId(ByRef idGaraje As Integer) As String

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT Nombre
                                                       FROM   Garajes
                                                       WHERE  IdGaraje = {0};", idGaraje), conexion)
        Dim nombreGaraje As String

        Try
            nombreGaraje = CType(comando.ExecuteScalar, String)
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el nombre del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

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
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el último Id del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return ultimoId

    End Function

    ''' <summary>
    ''' Elimina un garaje a partir de su Id.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje a eliminar.</param>
    ''' <returns>True: El garaje se ha eliminado. False: El garaje no se ha eliminado.</returns>
    Public Shared Function EliminarGarajePorId(ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("DELETE FROM Garajes 
                                                       WHERE IdGaraje = {0};", idGaraje), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Inserta un garaje.
    ''' </summary>
    ''' <param name="garaje">Datos del garaje a insertar.</param>
    ''' <returns>True: El garaje se ha insertado. False: El garaje no se ha insertado.</returns>
    Public Shared Function InsertarGaraje(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As MySqlCommand

        If Foo.HayTexto(garaje.Observaciones) Then

            comando = New MySqlCommand(String.Format("INSERT INTO Garajes (IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones) VALUES (NULL, '{0}', '{1}', {2}, {3}, 0, '{4}');", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.NumPlazas, garaje.Observaciones), conexion)
        Else

            comando = New MySqlCommand(String.Format("INSERT INTO Garajes (IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones) VALUES (NULL, '{0}', '{1}', {2}, {3}, 0, NULL);", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.NumPlazas), conexion)

        End If

        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un garaje seleccionado.
    ''' </summary>
    ''' <param name="garaje">Datos del garaje que se van a modificar.</param>
    ''' <returns>True: Se ha modificado el garaje. False: No se ha modificado el garaje.</returns>
    Public Shared Function ModificarGaraje(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As MySqlCommand

        If Foo.HayTexto(garaje.Observaciones) Then

            comando = New MySqlCommand(String.Format("UPDATE Garajes 
                                                      SET Nombre = '{0}', Direccion = '{1}', NumPlazas = {2}, Observaciones = '{3}' 
                                                      WHERE  IdGaraje = {4};", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.Observaciones, garaje.Id), conexion)
        Else

            comando = New MySqlCommand(String.Format("UPDATE Garajes 
                                                      SET Nombre = '{0}', Direccion = '{1}', NumPlazas = {2}, Observaciones = NULL 
                                                      WHERE  IdGaraje = {3};", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.Id), conexion)
        End If

        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

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
