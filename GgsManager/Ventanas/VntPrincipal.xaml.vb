Class MainWindow

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        ' AbrirVntLogin()

    End Sub


    ''' <summary>
    ''' Abre la ventana "VntLogin" para iniciar sesión.
    ''' </summary>
    Private Sub AbrirVntLogin()

        Dim vntLogin As New VntLogin()
        vntLogin.ShowDialog()

        If GestionBd.UsuarioPrograma IsNot Nothing Then           ' Si se ha iniciado sesión, activamos el menú.

            MenuPrincipal.IsEnabled = True

            If GestionBd.UsuarioPrograma.EsGestor = False Then           ' Si el usuario logueado no es un gestor, desactivamos las opciones de "Usuarios" y "Configuración".

                Usuarios.IsEnabled = False
                Configuracion.IsEnabled = False

            End If

        End If

    End Sub

    Private Sub Garajes_Click(sender As Object, e As RoutedEventArgs)

        Dim vntGarajes As New WPF.MDI.MdiChild()
        vntGarajes.Title = "Gestión de Garajes"
        vntGarajes.Content = New VntGarajes()

        vntGarajes.Width = 726
        vntGarajes.Height = 404

        ContenedorMDI.Children.Add(vntGarajes)

    End Sub

    Private Sub Clientes_Click(sender As Object, e As RoutedEventArgs)

        Dim vntClientes As New WPF.MDI.MdiChild()
        vntClientes.Title = "Gestión de Clientes"
        vntClientes.Content = New VntClientes()

        vntClientes.Width = 1460
        vntClientes.Height = 483

        ContenedorMDI.Children.Add(vntClientes)

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

        MenuPrincipal.IsEnabled = False
        AbrirVntLogin()

    End Sub

End Class
