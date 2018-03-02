Class VntPrincipal

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        ' AbrirVntLogin()

    End Sub


    ''' <summary>
    ''' Abre la ventana "VntLogin" para iniciar sesión.
    ''' </summary>
    Private Sub AbrirVntLogin()

        Dim vntLogin As New VntLogin()
        vntLogin.ShowDialog()

        If UsuarioPrograma.UsuarioLogueado IsNot Nothing Then           ' Si se ha iniciado sesión, activamos el menú.

            MenuPrincipal.IsEnabled = True

            If UsuarioPrograma.UsuarioLogueado.EsGestorB = False Then           ' Si el usuario logueado no es un gestor, desactivamos las opciones de "Usuarios" y "Configuración".

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

        AbrirVntSeleccGaraje(1)

    End Sub

    Private Sub Usuarios_Click(sender As Object, e As RoutedEventArgs)

        Dim vntUsuarios As New WPF.MDI.MdiChild()
        vntUsuarios.Title = "Gestión de Usuarios"
        vntUsuarios.Content = New VntUsuarios()

        vntUsuarios.Width = 1460
        vntUsuarios.Height = 483

        ContenedorMDI.Children.Add(vntUsuarios)

    End Sub

    Private Sub Plazas_Click(sender As Object, e As RoutedEventArgs)

        AbrirVntSeleccGaraje(2)

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


    ''' <summary>
    ''' Abre "VntSeleccGaraje" para seleccionar un garaje.
    ''' </summary>
    ''' <param name="numVentana">Número de ventana a mostrar.</param>
    Private Sub AbrirVntSeleccGaraje(ByRef numVentana As Integer)

        Dim vntSeleccGaraje As New VntSeleccGaraje(Me, numVentana)
        vntSeleccGaraje.ShowDialog()

    End Sub

    Private Sub CambiarIVA_Click(sender As Object, e As RoutedEventArgs)

        Dim vntCambiarIva As New WPF.MDI.MdiChild()
        vntCambiarIva.Title = "Cambiar I.V.A."
        vntCambiarIva.Content = New VntCambiarIVA()

        vntCambiarIva.Width = 558
        vntCambiarIva.Height = 339

        ContenedorMDI.Children.Add(vntCambiarIva)

    End Sub

End Class
