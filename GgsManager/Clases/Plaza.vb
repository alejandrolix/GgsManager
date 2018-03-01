Imports MySql.Data.MySqlClient

Public Class Plaza

    Property Id As Integer
    Property IdGaraje As Integer
    Property Situacion As Integer


    ''' <summary>
    ''' Obtiene las plazas a partir del Id del garaje seleccionado.
    ''' </summary>
    ''' <param name="idGaraje">Id del garaje seleccionado.</param>
    ''' <returns>Lista con las plazas.</returns>
    Public Shared Function ObtenerPlazasPorIdGaraje(ByRef idGaraje As Integer) As List(Of Plaza)

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
    ''' Cambia la situación de la plaza a "Ocupada".
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

    Public Sub New(id As Integer, idGaraje As Integer, situacion As Integer)

        Me.Id = id
        Me.IdGaraje = idGaraje
        Me.Situacion = situacion

    End Sub

    Public Sub New(id As Integer)

        Me.Id = id

    End Sub

End Class
