Public Class VntGarajes

    ''' <summary>
    ''' Contiene los datos del garaje seleccionado.
    ''' </summary>    
    Property GarajeSelec As Garaje

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesDg.DataContext = Garaje.ObtenerGarajes()

    End Sub

    Private Sub NuevoGaraje_Click(sender As Object, e As RoutedEventArgs)

        AbrirVentanaAddGaraje()

    End Sub

    Private Sub EliminarGaraje_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSelec As Garaje = CType(GarajesDg.SelectedItem, Garaje)

        If garajeSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If FacturaRealizada.EliminarFacturasPorIdGaraje(garajeSelec.Id) Then

                If Garaje.EliminarGarajePorId(garajeSelec.Id) Then

                    GarajesDg.DataContext = Garaje.ObtenerGarajes()
                    MessageBox.Show("Se ha eliminado el garaje.", "Garaje Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)

                End If

            End If

        End If

    End Sub

    Private Sub ModificarGaraje_Click(sender As Object, e As RoutedEventArgs)

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

        Dim vntAddGaraje As New AddGaraje(Foo.Accion.Insertar)
        vntAddGaraje.VntGarajes = Me
        vntAddGaraje.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "AddGaraje" para modificar los datos de un garaje seleccionado.
    ''' </summary>
    ''' <param name="accion">Enum para modificar el garaje.</param>
    ''' <param name="garajeSelec">Datos del garaje seleccionado para poder editarlos.</param>
    Private Sub AbrirVentanaAddGaraje(ByRef accion As Integer, ByRef garajeSelec As Garaje)

        Dim vntAddGaraje As New AddGaraje(Foo.Accion.Modificar, garajeSelec)
        vntAddGaraje.Title = "Modificar Garaje"
        vntAddGaraje.VntGarajes = Me
        vntAddGaraje.ShowDialog()

    End Sub

End Class
