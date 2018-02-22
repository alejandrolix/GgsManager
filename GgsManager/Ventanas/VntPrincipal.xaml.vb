Class MainWindow

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        Dim vntLogin As New VntLogin()
        vntLogin.ShowDialog()

        If GestionarBd.UsuarioIniciado IsNot Nothing Then           ' Si se ha iniciado sesión, activamos el menú.

            MenuPrincipal.IsEnabled = True

            If GestionarBd.UsuarioIniciado <> "root" Then           ' Si el usuario logueado no es "root", desactivamos las opciones de "Usuarios" y "Configuración".

                Usuarios.IsEnabled = False
                Configuracion.IsEnabled = False

            End If

        End If

    End Sub

    Private Sub Garajes_Click(sender As Object, e As RoutedEventArgs)

        'Dim vntLogin As New WPF.MDI.MdiChild()
        'vntLogin.Title = "Inicio de Sesión"
        'vntLogin.Content = New VntLogin()

        'ContenedorMDI.Children.Add(vntLogin)

    End Sub

    Private Sub Clientes_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Vehiculos_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Usuarios_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Plazas_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Listados_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Facturas_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Configuracion_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub CerrarSesion_Click(sender As Object, e As RoutedEventArgs)

    End Sub

End Class
