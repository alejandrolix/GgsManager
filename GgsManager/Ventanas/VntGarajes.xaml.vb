Public Class VntGarajes

    Private ListaGarajes As List(Of Garaje)

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesDgv.DataContext = GestionarBd.ObtenerGarajes()

    End Sub

    Private Sub NuevoGaraje_Click(sender As Object, e As RoutedEventArgs)



    End Sub

    Private Sub EliminarGaraje_Click(sender As Object, e As RoutedEventArgs)

        Dim garajeSeleccionado As Garaje = CType(GarajesDgv.SelectedItem, Garaje)

        If garajeSeleccionado Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un garaje", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            GestionarBd.EliminarGarajePorId(garajeSeleccionado.Id)              ' Eliminamos el garaje de la base de datos.
            GarajesDgv.DataContext = GestionarBd.ObtenerGarajes()

        End If

    End Sub
End Class
