Public Class VntCambiarPassword

    Property IdUsuario As Integer

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            If NuevaPasswordPsb.Password.Equals(RepetirPasswordPsb.Password) Then

                Dim hashPassword As String = UsuarioPrograma.ObtenerSHA1HashFromPassword(NuevaPasswordPsb.Password)

                If UsuarioPrograma.ModificarPasswordPorId(IdUsuario, hashPassword) Then

                    MessageBox.Show("Se ha modificado la contraseña del usuario seleccionado.", "Contraseña Modificada", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            Else

                MessageBox.Show("La contraseña introducida ha de ser igual que la primera.", "Contraseñas no Coinciden", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos por el usuario son correctos.
    ''' </summary>
    ''' <returns>True: Los datos introducidos son correctos. False: Los datos introducidos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayNuevaPassword, hayRepePassword As Boolean

        ' Comprobación de errores.

        If Foo.HayTexto(NuevaPasswordPsb.Password) Then

            hayNuevaPassword = True

        ElseIf NuevaPasswordPsb.Password.Length <= 4 Then

            MessageBox.Show("La contraseña tiene que tener más de 4 caracteres.", "Contraseña Corta", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            MessageBox.Show("Tienes que introducir la nueva contraseña.", "Nueva Contraseña Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(RepetirPasswordPsb.Password) Then

            hayRepePassword = True
        Else

            MessageBox.Show("Tienes que introducir la repetición de la nueva contraseña.", "Repetición Contraseña", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Return hayNuevaPassword And hayRepePassword

    End Function

End Class
