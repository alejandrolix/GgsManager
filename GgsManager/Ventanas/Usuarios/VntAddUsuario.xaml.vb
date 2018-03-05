Public Class VntAddUsuario

    Private Accion As Foo.Accion

    ''' <summary>
    ''' Contiene los datos del usuario seleccionado.
    ''' </summary>
    Private UsuarioSelec As UsuarioPrograma

    ''' <summary>
    ''' Para actualizar el DataGrid de usuarios.
    ''' </summary>    
    Property VntUsuarios As VntUsuarios

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

            If Accion = Foo.Accion.Insertar Then

                Dim hashPassword As String = UsuarioPrograma.ObtenerSHA1HashFromPassword(PasswordUsuPb.Password)
                Dim usuarioProgramaa As New UsuarioPrograma(NombreUsuTxt.Text, EsGestorUsuChk.IsChecked.Value)

                If UsuarioPrograma.InsertarUsuario(usuarioProgramaa, hashPassword) Then

                    MessageBox.Show("Se ha añadido el usuario.", "Usuario Añadido", MessageBoxButton.OK, MessageBoxImage.Error)
                    LimpiarCampos()

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                If UsuarioPrograma.ModificarUsuarioPorId(NombreUsuTxt.Text, EsGestorUsuChk.IsChecked.Value, UsuarioSelec.Id) Then

                    MessageBox.Show("Se ha modificado los datos del usuario seleccionado.", "Usuario Modificado", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            End If

            VntUsuarios.UsuariosDg.DataContext = UsuarioPrograma.ObtenerUsuariosPrograma()                  ' Actualizamos el DataGrid de Usuarios.

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

    Public Sub New(accion As Foo.Accion)

        InitializeComponent()
        Me.Accion = accion

    End Sub

    Public Sub New(accion As Foo.Accion, usuarioSelec As UsuarioPrograma)

        InitializeComponent()

        Me.Accion = accion
        Me.UsuarioSelec = usuarioSelec

    End Sub

    Private Sub CambiarPasswordBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim vntCambPassword As New VntCambiarPassword()
        vntCambPassword.Title = "Cambiar Contraseña"
        vntCambPassword.IdUsuario = UsuarioSelec.Id

        vntCambPassword.ShowDialog()

    End Sub

End Class
