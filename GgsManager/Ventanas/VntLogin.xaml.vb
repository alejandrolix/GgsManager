Public Class VntLogin

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Keyboard.Focus(UsuarioTxt)              ' Establecemos el foco en el TextBox del nombre de usuario.

    End Sub

    Private Sub IniciarSesion_Click(sender As Object, e As RoutedEventArgs)

        If Not Foo.HayTexto(UsuarioTxt.Text) Then

            MessageBox.Show("El campo usuario está vacío", "Usuario Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Not Foo.HayTexto(PasswordBox.Password) Then

            MessageBox.Show("El campo contraseña está vacío", "Contraseña Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(UsuarioTxt.Text) And Foo.HayTexto(PasswordBox.Password) Then            ' Si el usuario ha introducido su nombre y su contraseña.         

            If GestionBd.ExisteUsuario(UsuarioTxt.Text) Then

                GestionBd.UsuarioIniciado = UsuarioTxt.Text           ' Guardamos el usuario introducido, que servirá para obtener su contraseña.

                If GestionBd.ComprobarHashPassword(GestionBd.UsuarioIniciado, PasswordBox.Password) Then

                    Me.Close()
                Else

                    MessageBox.Show("La contraseña introducida no es correcta", "Contraseña Incorrecta", MessageBoxButton.OK, MessageBoxImage.Error)

                    If PasswordBox.Password.Length >= 1 Then

                        PasswordBox.Password = ""

                    End If

                End If
            Else

                MessageBox.Show("El usuario introducido no existe.", "Usuario Incorrecto", MessageBoxButton.OK, MessageBoxImage.Error)

                If UsuarioTxt.Text.Length >= 1 Then

                    UsuarioTxt.Text = ""

                End If

            End If

        End If

    End Sub

End Class
