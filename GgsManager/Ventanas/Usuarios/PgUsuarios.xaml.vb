﻿Class PgUsuarios

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        UsuariosDg.DataContext = Usuario.ObtenerUsuarios()

        If UsuariosDg.DataContext Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los usuarios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

    End Sub

    Private Sub NuevoUsuarioBtn_Click(sender As Object, e As RoutedEventArgs)

        AbrirVntAddUsuario(Foo.Accion.Insertar)

    End Sub

    Private Sub EliminarUsuarioBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim usuarioSelec As Usuario = CType(UsuariosDg.SelectedItem, Usuario)

        If usuarioSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If Usuario.Eliminar(usuarioSelec) Then

                MessageBox.Show("Se ha eliminado el usuario.", "Usuario Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)
                UsuariosDg.DataContext = Usuario.ObtenerUsuarios()

                If UsuariosDg.DataContext Is Nothing Then

                    MessageBox.Show("Ha habido un problema al obtener los usuarios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If
            Else

                MessageBox.Show("Ha habido un problema al eliminar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub

    Private Sub ModificarUsuarioBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim usuarioSelec As Usuario = CType(UsuariosDg.SelectedItem, Usuario)

        If usuarioSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            AbrirVntAddUsuario(Foo.Accion.Modificar, usuarioSelec)

        End If

    End Sub


    ''' <summary>
    ''' Abre "VntAddUsuario" para añadir un usuario.
    ''' </summary>
    ''' <param name="accion">Acción a realizar.</param>
    Private Sub AbrirVntAddUsuario(ByRef accion As Foo.Accion)

        Dim vntAddUsuario As New VntAddUsuario(accion, Me)
        vntAddUsuario.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "VntAddUsuario" para modificar los datos del usuario seleccionado.
    ''' </summary>
    ''' <param name="accion">Acción a realizar.</param>
    ''' <param name="usuarioSelec">Datos del usuario seleccionado.</param>
    Private Sub AbrirVntAddUsuario(ByRef accion As Foo.Accion, ByRef usuarioSelec As Usuario)

        Dim vntAddUsuario As New VntAddUsuario(accion, usuarioSelec, Me)
        vntAddUsuario.Title = "Modificar Usuario"
        vntAddUsuario.ShowDialog()

    End Sub

End Class
