Public Class VntVehiculos

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        VehiculosDg.Language = Markup.XmlLanguage.GetLanguage(Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag)            ' Establece el idioma a español, (para el euro).
        VehiculosDg.DataContext = GestionBd.ObtenerVehiculos()

    End Sub

    Private Sub NuevoVehiculo_Click(sender As Object, e As RoutedEventArgs)

        Dim vntAddVehiculo As New VntAddVehiculo()
        vntAddVehiculo.ShowDialog()

    End Sub

    Private Sub EliminarVehiculo_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub ModificarCliente_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
