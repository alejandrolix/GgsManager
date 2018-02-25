Public Class VntClientes

    Private Vista As CollectionViewSource

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        Me.Vista = New CollectionViewSource()
        Vista.Source = GestionarBd.ObtenerClientes()

        ClientesDg.DataContext = Vista
        AddHandler Vista.Filter, AddressOf Vista_Filter

    End Sub

    Private Sub Vista_Filter(sender As Object, e As FilterEventArgs)

        Dim cliente As Cliente = CType(e.Item, Cliente)

        If BuscarNombreTextBox.Text = "" Then

            e.Accepted = True

        ElseIf cliente.Nombre.Contains(BuscarNombreTextBox.Text) Then

            e.Accepted = True
        Else

            e.Accepted = False

        End If

    End Sub

    Private Sub NuevoCliente_Click(sender As Object, e As RoutedEventArgs)

        Dim vntAddCliente As New VntAddCliente()
        vntAddCliente.ShowDialog()

    End Sub

    Private Sub EliminarCliente_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub ModificarCliente_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub BuscarNombreTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)

        Vista.View.Refresh()

    End Sub
End Class
