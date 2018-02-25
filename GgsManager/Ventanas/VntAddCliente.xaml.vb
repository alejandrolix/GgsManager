Public Class VntAddCliente

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Keyboard.Focus(NombreClienteTxt)

    End Sub

    Private Sub GuardarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

        ComprobarDatosIntroducidos()

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos del cliente son correctos.
    ''' </summary>
    Private Sub ComprobarDatosIntroducidos()

        Dim hayNombre, hayApellidos, hayDNI, hayDireccion, hayPoblacion, hayProvincia, hayMovil As Boolean

        ' Comprobación de errores.

        If Foo.HayTexto(NombreClienteTxt.Text) Then

            hayNombre = True
        Else

            MessageBox.Show("Tienes que introducir un nombre", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ApellidosClienteTxt.Text) Then

            hayApellidos = True
        Else

            MessageBox.Show("Tienes que introducir unos apellidos", "Apellidos Vacíos", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(DNIClienteTxt.Text) Then

            hayDNI = True
        Else

            MessageBox.Show("Tienes que introducir un D.N.I.", "DNI Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(DireccionClienteTxt.Text) Then

            hayDireccion = True
        Else

            MessageBox.Show("Tienes que introducir una dirección", "Dirección Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(PoblacionClienteTxt.Text) Then

            hayPoblacion = True
        Else

            MessageBox.Show("Tienes que introducir una población", "Población Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ProvinciaClienteTxt.Text) Then

            hayProvincia = True
        Else

            MessageBox.Show("Tienes que introducir una província", "Província Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(MovilClienteTxt.Text) Then

            hayMovil = True
        Else

            MessageBox.Show("Tienes que introducir un móvil", "Móvil Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If ObservClienteTxt.Text.Length >= 1 Then



        End If

    End Sub

End Class
