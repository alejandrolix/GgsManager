﻿Imports MySql.Data.MySqlClient

''' <summary>
''' Representa una factura de la tabla "Facturas".
''' </summary>
Public Class Factura

    Property Id As Integer
    Property Fecha As Date
    Property IdCliente As Integer
    Property IdGaraje As Integer


    ''' <summary>
    ''' Obtiene el número de facturas a partir del Id de un cliente.
    ''' </summary>
    ''' <param name="idCliente">El Id de un cliente.</param>
    ''' <returns>El número de facturas del cliente.</returns>
    Private Shared Function ObtenerNumFacturasPorIdCliente(ByRef idCliente As Integer) As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT COUNT(IdFactura)
                                         FROM   Facturas
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim numFacturas As Integer

        Try
            numFacturas = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFacturas

    End Function


    ''' <summary>
    ''' Obtiene el número de facturas a partir del Id de un garaje.
    ''' </summary>
    ''' <param name="idGaraje">El Id de un garaje.</param>
    ''' <returns>El número de facturas del garaje.</returns>
    Private Shared Function ObtenerNumFacturasPorIdGaraje(ByRef idGaraje As Integer) As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT COUNT(IdFactura)
                                         FROM   Facturas
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim numFacturas As Integer

        Try
            numFacturas = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFacturas

    End Function


    ''' <summary>
    ''' Obtiene un nuevo Id de la factura para asignárselo a una.
    ''' </summary>
    ''' <returns>El nuevo Id de la factura.</returns>
    Public Shared Function ObtenerNuevoIdFactura() As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT MAX(IdFactura) + 1
                                         FROM   Facturas;", conexion)
        Dim nuevoId As Integer

        Try
            nuevoId = CType(comando.ExecuteScalar(), Integer)
            conexion.Close()

        Catch ex As Exception

        End Try

        Return nuevoId

    End Function


    ''' <summary>
    ''' Inserta una factura para un cliente.
    ''' </summary>  
    ''' <returns>True: Se ha insertado la factura para un cliente. False: No se ha insertado la factura para un cliente.</returns>
    Public Function InsertarParaCliente() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO Facturas (IdFactura, Fecha, IdCliente, IdGaraje) VALUES (NULL, @Fecha, @IdCliente, NULL);", conexion)

        comando.Parameters.AddWithValue("@Fecha", Fecha)
        comando.Parameters.AddWithValue("@IdCliente", IdCliente)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Inserta una factura para un garaje.
    ''' </summary>    
    ''' <returns>True: Se ha insertado la factura para un garaje. False: No se ha insertado la factura para un garaje.</returns>
    Public Function InsertarParaGaraje() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO Facturas (IdFactura, Fecha, IdCliente, IdGaraje) VALUES (NULL, @Fecha, NULL, @IdGaraje);", conexion)

        comando.Parameters.AddWithValue("@Fecha", Fecha)
        comando.Parameters.AddWithValue("@IdGaraje", IdGaraje)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina las facturas de un garaje a partir de su Id.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje.</param>
    ''' <returns>True: Se ha eliminado la factura del garaje. False: No se ha eliminado la factura del garaje.</returns>
    Public Shared Function EliminarFacturasPorIdGaraje(ByRef idGaraje As Integer) As Boolean

        Dim numFacturasGaraje As Integer = ObtenerNumFacturasPorIdGaraje(idGaraje)

        If numFacturasGaraje <= 0 Then

            MessageBox.Show("Ha habido un problema al obtener el número de facturas del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim conexion As MySqlConnection = Foo.ConexionABd()
            Dim comando As New MySqlCommand("DELETE FROM Facturas
                                             WHERE  IdGaraje = @IdGaraje;", conexion)

            comando.Parameters.AddWithValue("@IdGaraje", idGaraje)

            Dim numFila As Integer
            Try
                numFila = comando.ExecuteNonQuery()

            Catch ex As Exception

                MessageBox.Show("Ha habido un problema al eliminar las facturas del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End Try

            conexion.Close()

            Return numFila >= 1

        End If

        Return False

    End Function

    Public Sub New(fecha As Date, id As Integer, esParaCliente As Boolean)              ' Para insertar una factura.

        Me.Fecha = fecha

        If esParaCliente Then

            Me.IdCliente = id
        Else

            Me.IdGaraje = id

        End If

    End Sub

End Class
