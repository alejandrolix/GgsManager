Public Class VntGarajes
    Property GarajeSelec As Garaje

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesDg.DataContext = GestionarBd.ObtenerGarajes()

    End Sub

    Private Sub NuevoGaraje_Click(sender As Object, e As RoutedEventArgs)

        AbrirVentanaAddGaraje()

    End Sub

    Private Sub EliminarGaraje_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSelec As Garaje = CType(GarajesDg.SelectedItem, Garaje)

        If garajeSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If GestionarBd.EliminarGarajePorId(garajeSelec.Id) Then                   ' Intentamos eliminar el garaje de la base de datos.

                GarajesDg.DataContext = GestionarBd.ObtenerGarajes()

            End If

        End If

    End Sub

    Private Sub ModificarGaraje_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSelec As Garaje = CType(GarajesDg.SelectedItem, Garaje)

        If garajeSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            AbrirVentanaAddGaraje(1, garajeSelec)

        End If

    End Sub


    ''' <summary>
    ''' Abre la ventana "AddGaraje" para añadir un garaje.
    ''' </summary>
    Private Sub AbrirVentanaAddGaraje()

        Dim vntAddGaraje As New AddGaraje()
        vntAddGaraje.VntGarajes = Me
        vntAddGaraje.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre la ventana "AddGaraje" para modificar los datos de un garaje seleccionado.
    ''' </summary>
    ''' <param name="tipoAccion">Enum para modificar el garaje.</param>
    ''' <param name="garajeSelec">Datos del garaje seleccionado para poder editarlos.</param>
    Private Sub AbrirVentanaAddGaraje(ByRef tipoAccion As Integer, ByRef garajeSelec As Garaje)

        Dim vntAddGaraje As New AddGaraje(tipoAccion, garajeSelec)
        vntAddGaraje.VntGarajes = Me
        vntAddGaraje.ShowDialog()

    End Sub

End Class
