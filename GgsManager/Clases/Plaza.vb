Imports MySql.Data.MySqlClient

Public Class Plaza

    Property Id As Integer
    Property Matricula As String
    Property Modelo As String
    Property TipoSituacion As String


    ''' <summary>
    ''' Obtiene los Id de las plazas a partir del Id del garaje.
    ''' </summary>
    ''' <param name="idGaraje">Id del garaje.</param>
    ''' <returns>Lista con las plazas.</returns>
    Public Shared Function ObtenerIdPlazasPorIdGaraje(ByRef idGaraje As Integer) As List(Of Plaza)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT IdPlaza
                                                       FROM   Plazas
                                                       WHERE  IdGaraje = {0};", idGaraje), conexion)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener las plazas del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

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
        Dim comando As New MySqlCommand(String.Format("SELECT Veh.Matricula, Veh.Modelo, Plz.IdPlaza, Sit.Tipo AS 'Tipo Situación'
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
                Dim modelo As String = datos.GetString("Modelo")
                Dim idPlaza As Integer = datos.GetInt32("IdPlaza")
                Dim tipoSituacion As String = datos.GetString("Tipo Situación")

                Dim plaza As New Plaza(matricula, modelo, idPlaza, tipoSituacion)
                listaPlazas.Add(plaza)

            End While

            datos.Close()
            conexion.Close()

            Return listaPlazas

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Cambia la situación de la plaza a "Libre".
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje donde está el vehículo.</param>
    ''' <param name="idPlaza">El Id de la plaza dónde está el vehículo.</param>
    ''' <returns>True: Se ha cambiado la situación de la plaza. False: No se ha cambiado la situación de la plaza.</returns>
    Public Shared Function CambiarSituacionPlazaToLibre(ByRef idGaraje As Integer, ByRef idPlaza As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand(String.Format("UPDATE Plazas
                                                       SET    IdSituacion = 1
                                                       WHERE  IdGaraje = {0} AND IdPlaza = {1}", idGaraje, idPlaza), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al cambiar la situación de la plaza a Ocupado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function

    Public Sub New(matricula As String, modelo As String, idPlaza As String, tipoSituacion As String)

        Me.Matricula = matricula
        Me.Modelo = modelo
        Me.Id = idPlaza
        Me.TipoSituacion = tipoSituacion

    End Sub

    Public Sub New(id As Integer)

        Me.Id = id

    End Sub

End Class
