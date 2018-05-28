Class VntPrincipal

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        AbrirVntLogin()

    End Sub


    ''' <summary>
    ''' Abre "VntLogin" para iniciar sesión.
    ''' </summary>
    Private Sub AbrirVntLogin()

        Dim vntLogin As New VntLogin()
        vntLogin.ShowDialog()

        If Usuario.UsuarioLogueado IsNot Nothing Then           ' Si se ha iniciado sesión, activamos el menú.

            MenuPrincipal.IsEnabled = True

            If Usuario.UsuarioLogueado.EsGestor = False Then           ' Si el usuario logueado no es un gestor, desactivamos las opciones de "Usuarios", "Configuración" y "Estadísticas".

                Usuarios.IsEnabled = False
                Configuracion.IsEnabled = False
                Estadisticas.IsEnabled = False
            Else

                Usuarios.IsEnabled = True
                Configuracion.IsEnabled = True
                Estadisticas.IsEnabled = True

            End If

        End If

    End Sub

    Private Sub Garajes_Click(sender As Object, e As RoutedEventArgs)

        Frame.Content = New PgGarajes()

    End Sub

    Private Sub Clientes_Click(sender As Object, e As RoutedEventArgs)

        Frame.Content = New PgClientes()

    End Sub

    Private Sub Vehiculos_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Me, Foo.Ventana.Vehiculos)

    End Sub

    Private Sub Usuarios_Click(sender As Object, e As RoutedEventArgs)

        Frame.Content = New PgUsuarios()

    End Sub

    Private Sub Plazas_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Me, Foo.Ventana.Plazas)

    End Sub

    Private Sub InfClientes_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Foo.Ventana.InformeClientes)

    End Sub

    Private Sub InfPlazas_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Foo.Ventana.InformePlazas)

    End Sub

    Private Sub FactIndividual_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Foo.Ventana.FacturaIndividual)

    End Sub

    Private Sub FactPorGaraje_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Foo.Ventana.FacturaGaraje)

    End Sub

    Private Sub CerrarSesion_Click(sender As Object, e As RoutedEventArgs)

        Frame.Content = Nothing

        MenuPrincipal.IsEnabled = False
        AbrirVntLogin()

    End Sub


    ''' <summary>
    ''' Abre "VntSeleccGaraje" para seleccionar un garaje.
    ''' </summary>
    ''' <param name="vntPrincipal">La ventana actual.</param>
    ''' <param name="ventana">Ventana a mostrar.</param>
    Private Sub AbrirVntSeleccGaraje(ByRef vntPrincipal As VntPrincipal, ByRef ventana As Foo.Ventana)

        Dim vntSeleccGaraje As New VntSeleccGaraje(Me, ventana)
        vntSeleccGaraje.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "VntSeleccGaraje" para seleccionar un garaje.
    ''' </summary>    
    ''' <param name="ventana">Ventana a mostrar.</param>
    Private Sub AbrirVntSeleccGaraje(ByRef ventana As Foo.Ventana)

        Dim vntSeleccGaraje As New VntSeleccGaraje(ventana)
        vntSeleccGaraje.ShowDialog()

    End Sub

    Private Sub CambiarIVA_Click(sender As Object, e As RoutedEventArgs)

        Dim vntCambiarIva As New VntCambiarIVA()
        vntCambiarIva.ShowDialog()

    End Sub

    Private Sub EstadTodosGarajes_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        Dim vntEstadGaraje As New VntEstadGaraje(True)
        vntEstadGaraje.ShowDialog()

    End Sub

    Private Sub EstadGaraje_Click(sender As Object, e As RoutedEventArgs)

        If Frame.Content IsNot Nothing Then

            Frame.Content = Nothing

        End If

        AbrirVntSeleccGaraje(Foo.Ventana.InformeEstadGaraje)

    End Sub

    Private Sub ImportarBd_Click(sender As Object, e As RoutedEventArgs)

        If Foo.ImportarBd() Then

            MessageBox.Show("Importación Realizada con Éxito.", "Importación Realizada", MessageBoxButton.OK, MessageBoxImage.Information)
        Else

            MessageBox.Show("Ha habido un error al importar la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

    End Sub

    Private Sub ExportarBd_Click(sender As Object, e As RoutedEventArgs)

        If Foo.ExportarBd() Then

            MessageBox.Show("Exportación Realizada con Éxito.", "Exportación Realizada", MessageBoxButton.OK, MessageBoxImage.Information)
        Else

            MessageBox.Show("Ha habido un error al exportar la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

    End Sub

End Class
