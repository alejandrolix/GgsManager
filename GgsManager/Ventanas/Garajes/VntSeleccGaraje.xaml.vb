Public Class VntSeleccGaraje

    Property VntPrincipal As VntPrincipal

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesCmb.DataContext = Garaje.ObtenerNombresGarajes()
        GarajesCmb.SelectedIndex = 0

    End Sub

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim gjSelec As Garaje = CType(GarajesCmb.SelectedItem, Garaje)

        If gjSelec IsNot Nothing Then

            Dim vntVehic As New WPF.MDI.MdiChild()
            vntVehic.Title = "Gestión de Vehículos"
            vntVehic.Content = New VntVehiculos()

            VntVehiculos.IdGaraje = gjSelec.Id
            vntVehic.Width = 800
            vntVehic.Height = 401

            VntPrincipal.ContenedorMDI.Children.Add(vntVehic)
            Me.Close()

        End If

    End Sub

End Class
