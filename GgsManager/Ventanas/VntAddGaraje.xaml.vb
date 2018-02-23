Public Class AddGaraje

    Property VntGarajes As VntGarajes              ' Almacena una instancia de la ventana de "Gestión de Garajes".

    Private Sub InsGarajeBtn_Click(sender As Object, e As RoutedEventArgs)

        ComprobarDatosIntroducidos()

    End Sub


    ''' <summary>
    ''' Comprueba que los datos del garaje que ha introducido el usuario son correctos.
    ''' </summary>
    Private Sub ComprobarDatosIntroducidos()

        Dim hayNombre As Boolean, hayDireccion As Boolean, hayNumPlazas As Boolean
        Dim numPlazas As Integer

        If Not Foo.HayTexto(NombreGarajeTextBox.Text) Then

            MessageBox.Show("Tienes que introducir un nombre", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        Else

            hayNombre = True

        End If

        If Not Foo.HayTexto(DireccionGarajeTextBox.Text) Then

            MessageBox.Show("Tienes que introducir una dirección", "Dirección Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        Else

            hayDireccion = True

        End If

        Try
            numPlazas = Integer.Parse(NumPlazasGarajeTextBox.Text)
            hayNumPlazas = True

        Catch ex As Exception

            MessageBox.Show("Tienes que introducir un número de plazas.", "Nº de Plazas Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If NumPlazasGarajeTextBox.Text.Length > 0 Then

                NumPlazasGarajeTextBox.Text = ""

            End If

        End Try

        If hayNombre And hayDireccion And hayNumPlazas Then             ' Si el usuario ha introducido bien los datos.

            If ObservGarajeTextBox.Text.Length > 0 Then          ' Si en las observaciones del garaje hay texto, insertamos un garaje con observaciones.

                If GestionarBd.AddGarajeConObservaciones(NombreGarajeTextBox.Text, DireccionGarajeTextBox.Text, numPlazas, ObservGarajeTextBox.Text) Then

                    MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If
            Else

                If GestionarBd.AddGarajeSinObservaciones(NombreGarajeTextBox.Text, DireccionGarajeTextBox.Text, numPlazas) Then                 ' Insertamos un garaje sin observaciones.

                    MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If

            End If

        End If

    End Sub


    ''' <summary>
    ''' Limpia los datos del garaje introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreGarajeTextBox.Text = ""
        DireccionGarajeTextBox.Text = ""
        NumPlazasGarajeTextBox.Text = ""

        If ObservGarajeTextBox.Text.Length > 0 Then

            ObservGarajeTextBox.Text = ""

        End If

    End Sub

End Class
