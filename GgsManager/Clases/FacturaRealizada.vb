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
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir la factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

    End Sub


    ''' <summary>
    ''' Elimina las facturas de un cliente a partir de su Id.
    ''' </summary>
    ''' <param name="idCliente">El Id del cliente.</param>
    ''' <returns>True: Se han eliminado las facturas del cliente. False: No se han eliminado las facturas del cliente.</returns>
    Public Shared Function EliminarFacturasPorIdCliente(ByRef idCliente As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT COUNT(IdFactura)
                                         FROM   FacturasRealizadas
                                         WHERE  IdCliente = @IdCliente;", conexion)

        comando.Parameters.AddWithValue("@IdCliente", idCliente)
        Dim numFacturas As Integer

        Try
            numFacturas = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el número de facturas del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If numFacturas >= 1 Then

            comando.CommandText = "DELETE FROM FacturasRealizadas
                                   WHERE  IdCliente = @IdCliente;"

            Dim numFila As Integer
            Try
                numFila = comando.ExecuteNonQuery()
                conexion.Close()

            Catch ex As Exception

                MessageBox.Show("Ha habido un problema al eliminar las facturas del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End Try

            Return numFila >= 1

        End If

        Return False

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

    Public Sub New(fecha As Date, idCliente As Integer)             ' Para registrar una factura para un cliente.

        Me.Fecha = fecha
        Me.IdCliente = idCliente

    End Sub

End Class
