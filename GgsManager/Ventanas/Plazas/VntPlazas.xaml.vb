Public Class VntPlazas

    Shared Property IdGaraje As Integer             ' Almacena el Id del garaje seleccionado de "VntSeleccGaraje".

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        PlazasDg.DataContext = Plaza.ObtenerPlazasPorIdGaraje(IdGaraje)

    End Sub

End Class
