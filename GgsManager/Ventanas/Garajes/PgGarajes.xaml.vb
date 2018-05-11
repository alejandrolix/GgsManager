Class PgGarajes

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesDg.DataContext = Garaje.ObtenerGarajes()

        If GarajesDg.DataContext Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

    End Sub

    Private Sub NuevoGaraje_Click(sender As Object, e As RoutedEventArgs)

        AbrirVentanaAddGaraje()

    End Sub

    Private Sub EliminarGarajeBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSelec As Garaje = CType(GarajesDg.SelectedItem, Garaje)

        If garajeSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If Garaje.Eliminar(garajeSelec) Then

                MessageBox.Show("Se ha eliminado el garaje.", "Garaje Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)
                GarajesDg.DataContext = Garaje.ObtenerGarajes()

                If GarajesDg.DataContext Is Nothing Then

                    MessageBox.Show("Ha habido un problema al obtener los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If
            Else

                MessageBox.Show("Ha habido un problema al eliminar el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub

    Private Sub ModificarGarajeBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSelec As Garaje = CType(GarajesDg.SelectedItem, Garaje)

        If garajeSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            AbrirVentanaAddGaraje(Foo.Accion.Modificar, garajeSelec)

        End If

    End Sub


    ''' <summary>
    ''' Abre "AddGaraje" para añadir un garaje.
    ''' </summary>
    Private Sub AbrirVentanaAddGaraje()

        Dim vntAddGaraje As New AddGaraje(Foo.Accion.Insertar, Me)
        vntAddGaraje.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "AddGaraje" para modificar los datos de un garaje seleccionado.
    ''' </summary>
    ''' <param name="accion">Enum para modificar el garaje.</param>
    ''' <param name="garaje">Datos del garaje seleccionado para poder modificarlos.</param>
    Private Sub AbrirVentanaAddGaraje(ByRef accion As Integer, ByRef garaje As Garaje)

        Dim vntAddGaraje As New AddGaraje(Foo.Accion.Modificar, garaje, Me)
        vntAddGaraje.Title = "Modificar Garaje"
        vntAddGaraje.ShowDialog()

    End Sub

End Class
