Class PgClientes

    Property Vista As CollectionViewSource

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        Me.Vista = New CollectionViewSource()
        Vista.Source = Cliente.ObtenerClientes()

        If Vista.Source Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            ClientesDg.DataContext = Vista
            AddHandler Vista.Filter, AddressOf Vista_Filter

        End If

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

    Private Sub EliminarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim clienteSelec As Cliente = CType(ClientesDg.SelectedItem, Cliente)

        If clienteSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If Vehiculo.EliminarVehiculoPorIdCliente(clienteSelec.Id) Then              ' Eliminamos el vehículo del cliente.

                If Factura.EliminarFacturasPorIdCliente(clienteSelec.Id) Then           ' Eliminamos las facturas del cliente.

                    If clienteSelec.Eliminar() Then             ' Eliminamos el cliente.

                        clienteSelec.EliminarImg()               ' Eliminamos la imagen del cliente.
                        MessageBox.Show("Se ha eliminado el cliente.", "Cliente Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)

                        Vista.Source = Cliente.ObtenerClientes()

                        If Vista.Source Is Nothing Then

                            MessageBox.Show("Ha habido un problema al obtener los clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                        Else

                            ClientesDg.DataContext = Vista

                        End If
                    Else

                        MessageBox.Show("Ha habido un problema al eliminar el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                    End If
                Else

                    MessageBox.Show("Ha habido un problema al eliminar las facturas del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If
            Else

                MessageBox.Show("Ha habido un problema al eliminar el vehículo del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub

    Private Sub ModificarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

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

        Dim vntAddCliente As New VntAddCliente(Foo.Accion.Insertar, Me)
        vntAddCliente.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre la ventana "AddCliente" para modificar los datos de un cliente seleccionado.
    ''' </summary>
    ''' <param name="accion">Enum para modificar el cliente.</param>
    ''' <param name="clienteSelec">Datos del cliente seleccionado para poder editarlos.</param>
    Private Sub AbrirVentanaAddCliente(ByRef accion As Integer, ByRef clienteSelec As Cliente)

        Dim vntAddCliente As New VntAddCliente(Foo.Accion.Modificar, clienteSelec, Me)
        vntAddCliente.Title = "Modificar Cliente"
        vntAddCliente.ShowDialog()

    End Sub

    Private Sub BuscarApellidosTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)

        Vista.View.Refresh()

    End Sub

End Class
