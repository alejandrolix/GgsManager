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

            MessageBox.Show("Tienes que introducir un nombre.", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ApellidosClienteTxt.Text) Then

            hayApellidos = True
        Else

            MessageBox.Show("Tienes que introducir unos apellidos.", "Apellidos Vacíos", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(DNIClienteTxt.Text) Then

            hayDNI = True
        Else

            MessageBox.Show("Tienes que introducir un D.N.I.", "DNI Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(DireccionClienteTxt.Text) Then

            hayDireccion = True
        Else

            MessageBox.Show("Tienes que introducir una dirección.", "Dirección Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(PoblacionClienteTxt.Text) Then

            hayPoblacion = True
        Else

            MessageBox.Show("Tienes que introducir una población.", "Población Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ProvinciaClienteTxt.Text) Then

            hayProvincia = True
        Else

            MessageBox.Show("Tienes que introducir una província.", "Província Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(MovilClienteTxt.Text) Then

            hayMovil = True
        Else

            MessageBox.Show("Tienes que introducir un móvil.", "Móvil Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If hayNombre And hayApellidos And hayDNI And hayDireccion And hayPoblacion And hayProvincia And hayMovil Then

            If ObservClienteTxt.Text.Length >= 1 Then               ' Insertamos un cliente con observaciones.

                Dim cliente As New Cliente(NombreClienteTxt.Text, ApellidosClienteTxt.Text, DNIClienteTxt.Text, DireccionClienteTxt.Text, PoblacionClienteTxt.Text, ProvinciaClienteTxt.Text, MovilClienteTxt.Text, ObservClienteTxt.Text)

                If GestionBd.InsertarClienteConObservaciones(cliente) Then

                    MessageBox.Show("Cliente Añadido.", "Cliente Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If
            Else

                Dim cliente As New Cliente(NombreClienteTxt.Text, ApellidosClienteTxt.Text, DNIClienteTxt.Text, DireccionClienteTxt.Text, PoblacionClienteTxt.Text, ProvinciaClienteTxt.Text, MovilClienteTxt.Text, Nothing)

                If GestionBd.InsertarClienteSinObservaciones(cliente) Then

                    MessageBox.Show("Cliente Añadido.", "Cliente Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If

            End If

        End If

    End Sub


    ''' <summary>
    ''' Limpia los datos introducidos después de haber añadido el cliente en la base de datos.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreClienteTxt.Text = ""
        ApellidosClienteTxt.Text = ""
        DNIClienteTxt.Text = ""
        DireccionClienteTxt.Text = ""
        PoblacionClienteTxt.Text = ""
        ProvinciaClienteTxt.Text = ""
        MovilClienteTxt.Text = ""

        If ObservClienteTxt.Text.Length >= 1 Then

            ObservClienteTxt.Text = ""

        End If

    End Sub

    Private Sub AddFotoBtn_Click(sender As Object, e As RoutedEventArgs)



    End Sub
End Class
