Public Class VntLogin

    ''' <summary>
    ''' Indica si se puede cerrar la ventana después de iniciar sesión el usuario. Para el evento "Window_Closing".
    ''' </summary>
    Private CerrarVentana As Boolean

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Keyboard.Focus(UsuarioTxt)              ' Establecemos el foco en el TextBox del nombre de usuario.

    End Sub

    Private Sub IniciarSesionBtn_Click(sender As Object, e As RoutedEventArgs)

        ' Comprobación de errores.

        If Not Foo.HayTexto(UsuarioTxt.Text) Then

            MessageBox.Show("El campo usuario está vacío.", "Usuario Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Not Foo.HayTexto(PasswordBox.Password) Then

            MessageBox.Show("El campo contraseña está vacío.", "Contraseña Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(UsuarioTxt.Text) And Foo.HayTexto(PasswordBox.Password) Then            ' Si el usuario ha introducido su nombre y su contraseña.         

            If Usuario.ExisteUsuario(UsuarioTxt.Text) Then

                Usuario.UsuarioLogueado = Usuario.ObtenerUsuario(UsuarioTxt.Text)           ' Guardamos el usuario que ha iniciado sesión.

                Dim hashPassUsuarioBd As String = Usuario.ObtenerPasswordUsuario(UsuarioTxt.Text)

                If hashPassUsuarioBd.Equals("") Then

                    MessageBox.Show("Ha habido un problema al obtener la contraseña del usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    Dim hashPassUsuario As String = Usuario.ObtenerSHA1HashFromPassword(PasswordBox.Password)          ' Obtenemos el hash de la contraseña introducida.

                    If Usuario.ComprobarPasswords(hashPassUsuario, hashPassUsuarioBd) Then

                        Me.CerrarVentana = True
                        Me.Close()
                    Else

                        MessageBox.Show("La contraseña introducida no es correcta.", "Contraseña Incorrecta", MessageBoxButton.OK, MessageBoxImage.Error)
                        PasswordBox.Clear()

                    End If

                End If
            Else

                MessageBox.Show("El usuario introducido no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                UsuarioTxt.ClearValue(TextBox.TextProperty)

            End If

        End If

    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)

        If CerrarVentana Then

            e.Cancel = False
        Else

            e.Cancel = True

        End If

    End Sub

    Private Sub SalirBtn_Click(sender As Object, e As RoutedEventArgs)

        Environment.Exit(0)

    End Sub

End Class
