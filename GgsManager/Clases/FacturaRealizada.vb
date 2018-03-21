Imports MySql.Data.MySqlClient

''' <summary>
''' Representa una factura de la tabla "FacturasRealizadas".
''' </summary>
Public Class FacturaRealizada

    Property IdFactura As Integer
    Property Fecha As Date
    Property IdCliente As Integer
    Property IdGaraje As Integer


    ''' <summary>
    ''' Inserta una factura para un cliente.
    ''' </summary>
    ''' <param name="factura">Los datos de la factura.</param>    
    Public Shared Sub InsertarFacturaToCliente(ByRef factura As FacturaRealizada)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO FacturasRealizadas (IdFactura, Fecha, IdCliente, IdGaraje) VALUES (NULL, @Fecha, @IdCliente, NULL);", conexion)

        comando.Parameters.AddWithValue("@Fecha", factura.Fecha)
        comando.Parameters.AddWithValue("@IdCliente", factura.IdCliente)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir la factura del cliente seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

    End Sub


    ''' <summary>
    ''' Inserta una factura para un garaje.
    ''' </summary>
    ''' <param name="factura">Los datos de la factura.</param>    
    Public Shared Sub InsertarFacturaToGaraje(ByRef factura As FacturaRealizada)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO FacturasRealizadas (IdFactura, Fecha, IdCliente, IdGaraje) VALUES (NULL, @Fecha, NULL, @IdGaraje);", conexion)

        comando.Parameters.AddWithValue("@Fecha", factura.Fecha)
        comando.Parameters.AddWithValue("@IdGaraje", factura.IdGaraje)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir la factura del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

    End Sub


    ''' <summary>
    ''' Elimina las facturas de un cliente a partir de su Id.
    ''' </summary>
    ''' <param name="idCliente">El Id del cliente.</param>
    ''' <returns>True: Se han eliminado las facturas del cliente. False: No se han eliminado las facturas del cliente.</returns>
    Public Shared Function EliminarFacturasPorIdCliente(ByRef idCliente As Integer) As Boolean

        Dim numFacturasCliente As Integer = ObtenerNumFacturasPorIdCliente(idCliente)

        If numFacturasCliente >= 1 Then

            Dim conexion As MySqlConnection = Foo.ConexionABd()
            Dim comando As New MySqlCommand("DELETE FROM FacturasRealizadas
                                             WHERE  IdCliente = @IdCliente;", conexion)

            comando.Parameters.AddWithValue("@IdCliente", idCliente)

            Dim numFila As Integer
            Try
                numFila = comando.ExecuteNonQuery()

            Catch ex As Exception

                MessageBox.Show("Ha habido un problema al eliminar las facturas del cliente seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End Try

            conexion.Close()

            Return numFila >= 1

        End If

        Return False

    End Function


    Public Shared Function EliminarFacturasPorIdGaraje(ByRef idGaraje As Integer) As Boolean

        Dim numFacturasGaraje As Integer = ObtenerNumFacturasPorIdGaraje(idGaraje)

        If numFacturasGaraje >= 1 Then

            Dim conexion As MySqlConnection = Foo.ConexionABd()
            Dim comando As New MySqlCommand("DELETE FROM FacturasRealizadas
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


    ''' <summary>
    ''' Obtiene el número de facturas a partir del Id de un cliente.
    ''' </summary>
    ''' <param name="idCliente">El Id de un cliente.</param>
    ''' <returns>El número de facturas del cliente.</returns>
    Private Shared Function ObtenerNumFacturasPorIdCliente(ByRef idCliente As Integer) As Integer

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT COUNT(IdFactura)
                                         FROM   FacturasRealizadas
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim numFacturas As Integer

        Try
            numFacturas = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el número de facturas del cliente seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

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
                                         FROM   FacturasRealizadas
                                         WHERE  IdGaraje = @IdGaraje;", conexion)

        comando.Parameters.AddWithValue("@IdGaraje", idGaraje)
        Dim numFacturas As Integer

        Try
            numFacturas = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el número de facturas del garaje seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

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
                                         FROM   FacturasRealizadas;", conexion)
        Dim nuevoId As Integer

        Try
            nuevoId = CType(comando.ExecuteScalar(), Integer)
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el nuevo Id de la factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return nuevoId

    End Function

    Public Sub New(fecha As Date, id As Integer, esParaCliente As Boolean)

        Me.Fecha = fecha

        If esParaCliente Then

            Me.IdCliente = id
        Else
            Me.IdGaraje = id

        End If

    End Sub

End Class
