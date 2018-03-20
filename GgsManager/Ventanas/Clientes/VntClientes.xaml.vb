Public Class VntClientes

    Private Vista As CollectionViewSource

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        Me.Vista = New CollectionViewSource()
        Vista.Source = Cliente.ObtenerClientes()

        ClientesDg.DataContext = Vista

        AddHandler Vista.Filter, AddressOf Vista_Filter

    End Sub

    Private Sub Vista_Filter(sender As Object, e As FilterEventArgs)

        Dim cliente As Cliente = CType(e.Item, Cliente)

        If BuscarApellidosTextBox.Text = "" Then

            e.Accepted = True

        ElseIf cliente.Apellidos.Contains(BuscarApellidosTextBox.Text) Then

            e.Accepted = True
        Else

            e.Accepted = False

        End If

    End Sub

    Private Sub NuevoCliente_Click(sender As Object, e As RoutedEventArgs)

        AbrirVentanaAddCliente()

    End Sub

    Private Sub EliminarCliente_Click(sender As Object, e As RoutedEventArgs)

        Dim clienteSelec As Cliente = CType(ClientesDg.SelectedItem, Cliente)

        If clienteSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If Vehiculo.EliminarVehiculosPorIdCliente(clienteSelec.Id) Then

                If FacturaRealizada.EliminarFacturasPorIdCliente(clienteSelec.Id) Then

                    If Cliente.EliminarClientePorId(clienteSelec.Id) Then

                        Cliente.EliminarImgClientePorId(clienteSelec.Id)               ' Eliminamos la imagen del cliente.
                        Vista.Source = Cliente.ObtenerClientes()

                        MessageBox.Show("Se ha eliminado el cliente.", "Cliente Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)

                    End If

                End If

            End If

        End If

    End Sub

    Private Sub ModificarCliente_Click(sender As Object, e As RoutedEventArgs)

        Dim clienteSelec As Cliente = CType(ClientesDg.SelectedItem, Cliente)

        If clienteSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            AbrirVentanaAddCliente(Foo.Accion.Modificar, clienteSelec)

        End If

    End Sub


    ''' <summary>
    ''' Abre "VntAddCliente" para añadir un cliente.
    ''' </summary>
    Private Sub AbrirVentanaAddCliente()

        Dim vntAddCliente As New VntAddCliente(Foo.Accion.Insertar)
        vntAddCliente.VntClientes = Me
        vntAddCliente.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre la ventana "AddCliente" para modificar los datos de un cliente seleccionado.
    ''' </summary>
    ''' <param name="accion">Enum para modificar el cliente.</param>
    ''' <param name="clienteSelec">Datos del cliente seleccionado para poder editarlos.</param>
    Private Sub AbrirVentanaAddCliente(ByRef accion As Integer, ByRef clienteSelec As Cliente)

        Dim vntAddCliente As New VntAddCliente(Foo.Accion.Modificar, clienteSelec)
        vntAddCliente.Title = "Modificar Cliente"
        vntAddCliente.VntClientes = Me
        vntAddCliente.ShowDialog()

    End Sub

    Private Sub BuscarApellidosTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)

        Vista.View.Refresh()

    End Sub
End Class
