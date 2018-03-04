﻿Public Class VntUsuarios

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        UsuariosDg.DataContext = UsuarioPrograma.ObtenerUsuariosPrograma()

    End Sub

    Private Sub NuevoUsuarioBtn_Click(sender As Object, e As RoutedEventArgs)

        AbrirVntAddUsuario(Foo.Accion.Insertar)

    End Sub

    Private Sub EliminarUsuarioBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim usuarioSelec As UsuarioPrograma = CType(UsuariosDg.SelectedItem, UsuarioPrograma)

        If usuarioSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If UsuarioPrograma.EliminarUsuarioPorId(usuarioSelec.Id) Then                   ' Eliminamos el usuario de la base de datos.

                UsuariosDg.DataContext = UsuarioPrograma.ObtenerUsuariosPrograma()
                MessageBox.Show("Se ha eliminado el usuario.", "Usuario Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)

            End If

        End If

    End Sub

    Private Sub ModificarUsuarioBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim usuarioSelec As UsuarioPrograma = CType(UsuariosDg.SelectedItem, UsuarioPrograma)

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

        Dim vntAddUsuario As New VntAddUsuario(accion)
        vntAddUsuario.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "VntAddUsuario" para modificar los datos del usuario seleccionado.
    ''' </summary>
    ''' <param name="accion">Acción a realizar.</param>
    ''' <param name="usuarioSelec">Datos del usuario seleccionado.</param>
    Private Sub AbrirVntAddUsuario(ByRef accion As Foo.Accion, ByRef usuarioSelec As UsuarioPrograma)

        Dim vntAddUsuario As New VntAddUsuario(accion, usuarioSelec)
        vntAddUsuario.Title = "Modificar Usuario"

        vntAddUsuario.ShowDialog()

    End Sub

End Class