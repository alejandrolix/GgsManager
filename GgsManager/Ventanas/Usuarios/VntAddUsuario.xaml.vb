Public Class VntAddUsuario

    Private Accion As Foo.Accion

    ''' <summary>
    ''' Contiene los datos del usuario seleccionado.
    ''' </summary>
    Private UsuarioSelec As Usuario

    ''' <summary>
    ''' Para actualizar el DataGrid de usuarios.
    ''' </summary>    
    Private PgUsuarios As PgUsuarios

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Insertar Then

            CambiarPasswordBtn.IsEnabled = False
            Keyboard.Focus(NombreUsuTxt)

        ElseIf Accion = Foo.Accion.Modificar Then

            PasswordUsuPb.IsEnabled = False

            NombreUsuTxt.DataContext = UsuarioSelec
            EsGestorUsuChk.DataContext = UsuarioSelec

        End If

    End Sub

    Private Sub GuardarUsuBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            Dim usuario As Usuario

            If Accion = Foo.Accion.Insertar Then

                Dim hashPassword As String = Usuario.ObtenerSHA1HashFromPassword(PasswordUsuPb.Password)
                usuario = New Usuario(NombreUsuTxt.Text, EsGestorUsuChk.IsChecked.Value)

                If usuario.Insertar(hashPassword) Then

                    MessageBox.Show("Se ha añadido el usuario.", "Usuario Añadido", MessageBoxButton.OK, MessageBoxImage.Error)
                    LimpiarCampos()
                Else

                    MessageBox.Show("Ha habido un problema al insertar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                usuario = New Usuario(UsuarioSelec.Id, NombreUsuTxt.Text, EsGestorUsuChk.IsChecked.Value)

                If usuario.Modificar() Then

                    MessageBox.Show("Se ha modificado los datos del usuario seleccionado.", "Usuario Modificado", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    MessageBox.Show("Ha habido un problema al modificar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            End If

            PgUsuarios.UsuariosDg.DataContext = Usuario.ObtenerUsuarios()                  ' Actualizamos el DataGrid de Usuarios.

            If PgUsuarios.UsuariosDg.DataContext Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener los usuarios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub


    ''' <summary>
    ''' Limpia los datos introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreUsuTxt.ClearValue(TextBox.TextProperty)
        PasswordUsuPb.Clear()
        EsGestorUsuChk.ClearValue(CheckBox.IsCheckedProperty)

    End Sub


    ''' <summary>
    ''' Comprueba si los datos introducidos por el usuario son correctos.
    ''' </summary>
    ''' <returns>True: Los datos son correctos. False: Los datos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayNombre, hayPassword As Boolean

        ' Comprobación de errores.

        If Foo.HayTexto(NombreUsuTxt.Text) Then

            hayNombre = True
        Else

            MessageBox.Show("Tienes que introducir un nombre.", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(PasswordUsuPb.Password) And PasswordUsuPb.Password.Length > 4 Then

            hayPassword = True

        ElseIf Not Foo.HayTexto(PasswordUsuPb.Password) Then

            MessageBox.Show("Tienes que introducir un nombre.", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If Foo.HayTexto(PasswordUsuPb.Password) Then

                PasswordUsuPb.Clear()

            End If

        ElseIf PasswordUsuPb.Password.Length <= 4 Then

            MessageBox.Show("La contraseña tiene que tener más de 4 letras.", "Tamaño de la contraseña corta", MessageBoxButton.OK, MessageBoxImage.Error)
            PasswordUsuPb.Clear()

        End If

        Return hayNombre And hayPassword

    End Function

    Private Sub CambiarPasswordBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim vntCambPassword As New VntCambiarPassword(UsuarioSelec.Id)
        vntCambPassword.Title = "Cambiar Contraseña"
        vntCambPassword.ShowDialog()

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef pgUsuarios As PgUsuarios)

        InitializeComponent()
        Me.Accion = accion
        Me.PgUsuarios = pgUsuarios

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef usuarioSelec As Usuario, ByRef pgUsuarios As PgUsuarios)

        InitializeComponent()

        Me.Accion = accion
        Me.UsuarioSelec = usuarioSelec
        Me.PgUsuarios = pgUsuarios

    End Sub

End Class
