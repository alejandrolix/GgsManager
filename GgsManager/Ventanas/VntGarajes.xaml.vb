Public Class VntGarajes
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesDg.DataContext = GestionarBd.ObtenerGarajes()

    End Sub

    Private Sub NuevoGaraje_Click(sender As Object, e As RoutedEventArgs)

        Dim vntAddGaraje As New AddGaraje()
        vntAddGaraje.VntGarajes = Me
        vntAddGaraje.ShowDialog()

    End Sub

    Private Sub EliminarGaraje_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSelec As Garaje = CType(GarajesDg.SelectedItem, Garaje)

        If garajeSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If GestionarBd.EliminarGarajePorId(garajeSelec.Id) Then

                GarajesDg.DataContext = GestionarBd.ObtenerGarajes()

            End If

        End If

    End Sub
End Class
