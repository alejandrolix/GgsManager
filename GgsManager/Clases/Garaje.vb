Imports MySql.Data.MySqlClient
Imports NPoco

''' <summary>
''' Representa un garaje de la tabla "Garajes".
''' </summary>
Public Class Garaje

    ''' <summary>
    ''' El Id del Garaje.
    ''' </summary>    
    <Column("IdGaraje")>
    Property Id As Integer

    ''' <summary>
    ''' El Nombre del Garaje.
    ''' </summary>    
    Property Nombre As String

    ''' <summary>
    ''' La Dirección del Garaje.
    ''' </summary>    
    Property Direccion As String

    ''' <summary>
    ''' El Número de Plazas que tiene el Garaje.
    ''' </summary>    
    Property NumPlazas As Integer

    ''' <summary>
    ''' El Número de Plazas Libres que tiene el Garaje.
    ''' </summary>    
    Property NumPlazasLibres As Integer

    ''' <summary>
    ''' El Número de Plazas Ocupadas que tiene el Garaje.
    ''' </summary>    
    Property NumPlazasOcupadas As Integer

    ''' <summary>
    ''' Las Observaciones del Garaje.
    ''' </summary>    
    Property Observaciones As String


    ''' <summary>
    ''' Obtiene todos los garajes.
    ''' </summary>
    ''' <returns>Array con todos los garajes.</returns>
    Public Shared Function ObtenerGarajes() As Garaje()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayGarajes As Garaje() = conexion.Query(Of Garaje)("SELECT IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones
                                                                  FROM   Garajes
                                                                  ORDER BY Nombre;").ToArray()
        conexion.CloseSharedConnection()

        Return arrayGarajes

    End Function


    ''' <summary>
    ''' Obtiene los nombres de todos los garajes.
    ''' </summary>
    ''' <returns>Array con los nombres de los garajes.</returns>
    Public Shared Function ObtenerNombresGarajes() As Garaje()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayNombresGjs As Garaje() = conexion.Query(Of Garaje)("SELECT IdGaraje, Nombre
                                                                     FROM   Garajes
                                                                     ORDER BY Nombre;").ToArray()
        conexion.CloseSharedConnection()

        Return arrayNombresGjs

    End Function


    ''' <summary>
    ''' Obtiene el nombre de un garaje.
    ''' </summary>    
    ''' <param name="idGaraje">El Id de un garaje.</param>
    ''' <returns>El nombre del garaje correspondiente.</returns>
    Public Shared Function ObtenerNombreGarajePorId(ByRef idGaraje As Integer) As String

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim nombreGaraje As String = conexion.ExecuteScalar(Of String)(Sql.Builder.Append("SELECT Nombre
                                                                                           FROM   Garajes
                                                                                           WHERE  IdGaraje = @0;", idGaraje))

        conexion.CloseSharedConnection()

        Return nombreGaraje

    End Function


    ''' <summary>
    ''' Obtiene el último Id de la tabla "Garajes".
    ''' </summary>
    ''' <returns>El último Id de la tabla "Garajes".</returns>
    Public Shared Function ObtenerUltimoIdGaraje() As Integer

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim ultimoId As Integer = conexion.ExecuteScalar(Of Integer)("SELECT MAX(IdGaraje)
                                                                      FROM   Garajes;")

        conexion.CloseSharedConnection()

        Return ultimoId

    End Function


    ''' <summary>
    ''' Obtiene las estadísticas de todos los garajes.
    ''' </summary>
    ''' <returns>DataSet con las estadísticas de los garajes.</returns>
    Public Shared Function RellenarDatosEstadTodosGarajes() As DtPorcGaraje

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim dtPorcGaraje As New DtPorcGaraje()

        Try
            Dim adaptador As New MySqlDataAdapter("SELECT Nombre AS 'NombreGaraje', NumPlazas AS 'NumeroPlazas', TRUNCATE((NumPlazasLibres * NumPlazas) / 100, 0) AS 'PorcentajePlazasLibres', 
                                                          TRUNCATE((NumPlazasOcupadas * NumPlazas) / 100, 0) AS 'PorcentajePlazasOcupadas'
                                                   FROM   Garajes;", conexion)

            adaptador.Fill(dtPorcGaraje, "Estadisticas")

        Catch ex As Exception

        End Try

        conexion.Close()

        Return dtPorcGaraje

    End Function


    ''' <summary>
    ''' Obtiene las estadísticas de un garaje a partir de su Id.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    ''' <returns>DataSet con las estadísticas.</returns>
    Public Shared Function RellenarDatosEstadGarajePorId(ByRef idGaraje As Integer) As DtPorcGaraje

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim dtPorcGaraje As New DtPorcGaraje()

        Try
            Dim adaptador As New MySqlDataAdapter("SELECT Nombre AS 'NombreGaraje', NumPlazas AS 'NumeroPlazas', TRUNCATE((NumPlazasLibres * NumPlazas) / 100, 0) AS 'PorcentajePlazasLibres', 
                                                          TRUNCATE((NumPlazasOcupadas * NumPlazas) / 100, 0) AS 'PorcentajePlazasOcupadas'
                                                   FROM   Garajes
                                                   WHERE  IdGaraje = @IdGaraje;", conexion)

            adaptador.SelectCommand.Parameters.AddWithValue("@IdGaraje", idGaraje)
            adaptador.Fill(dtPorcGaraje, "Estadisticas")

        Catch ex As Exception

        End Try

        conexion.Close()

        Return dtPorcGaraje

    End Function


    ''' <summary>
    ''' Incrementa el número de plazas ocupadas de un garaje cuando se añade un vehículo.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    Public Shared Sub SumarNumPlazasOcupadasPorId(ByRef idGaraje As Integer)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Garajes
                                         SET    NumPlazasOcupadas = NumPlazasOcupadas + 1
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)

        Try
            comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

    End Sub


    ''' <summary>
    ''' Incrementa el número de plazas libres de un garaje cuando se elimina un vehículo.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    Public Shared Sub SumarNumPlazasLibresPorId(ByRef idGaraje As Integer)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Garajes
                                         SET    NumPlazasLibres = NumPlazasLibres + 1
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)

        Try
            comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

    End Sub


    ''' <summary>
    ''' Resta el número de plazas libres de un garaje cuando se añade un vehículo.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    Public Shared Sub RestarNumPlazasLibresPorId(ByRef idGaraje As Integer)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Garajes
                                         SET    NumPlazasLibres = NumPlazasLibres - 1
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)

        Try
            comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

    End Sub


    ''' <summary>
    ''' Resta el número de plazas ocupadas de un garaje cuando se elimina un vehículo.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    Public Shared Sub RestarNumPlazasOcupadasPorId(ByRef idGaraje As Integer)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Garajes
                                         SET    NumPlazasOcupadas = NumPlazasOcupadas - 1
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)

        Try
            comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

    End Sub


    ''' <summary>
    ''' Elimina un garaje.
    ''' </summary>    
    ''' <param name="garaje">El garaje a eliminar.</param>
    ''' <returns>True: El garaje se ha eliminado. False: El garaje no se ha eliminado.</returns>
    Public Shared Function Eliminar(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM Garajes
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("IdGaraje", garaje.Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Inserta un garaje.
    ''' </summary>    
    ''' <param name="garaje">El garaje a insertar.</param>
    ''' <returns>True: El garaje se ha insertado. False: El garaje no se ha insertado.</returns>
    Public Shared Function Insertar(ByRef garaje As Garaje) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim numInsercion As Integer

        Try
            Dim insercion As Object = conexion.Insert("Garajes", "IdGaraje", True, garaje)
            numInsercion = Integer.Parse(insercion.ToString())

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return numInsercion >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un garaje.
    ''' </summary>    
    ''' <param name="garaje">El garaje a modificar.</param>
    ''' <returns>True: Se ha modificado el garaje. False: No se ha modificado el garaje.</returns>
    Public Shared Function Modificar(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Garajes
                                         SET    Nombre = @Nombre, Direccion = @Direccion, Observaciones = @Observaciones
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@Nombre", garaje.Nombre)
        comando.Parameters.AddWithValue("@Direccion", garaje.Direccion)
        comando.Parameters.AddWithValue("@Observaciones", garaje.Observaciones)
        comando.Parameters.AddWithValue("@IdGaraje", garaje.Id)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

        'Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        'Dim actualizacion As Integer

        'Try
        '    actualizacion = conexion.Update("Garajes", "IdGaraje", garaje)

        'Catch ex As Exception

        'End Try

        'conexion.CloseSharedConnection()

        'Return actualizacion >= 1

    End Function

    Public Sub New(nombre As String, direccion As String, numPlazas As Integer, observaciones As String)            ' Para crear un nuevo garaje,

        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.NumPlazasLibres = numPlazas
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(id As Integer, nombre As String, direccion As String, numPlazas As Integer, observaciones As String)             ' Para modificar los datos de un garaje seleccionado.

        Me.Id = id
        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.Observaciones = observaciones

    End Sub

    Public Sub New()

    End Sub

End Class
