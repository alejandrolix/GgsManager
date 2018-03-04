Public Class VntVehiculos

    ''' <summary>
    ''' Almacena el Id del garaje seleccionado.
    ''' </summary>    
    Public Shared Property IdGaraje As Integer

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)

        VehiculosDg.Language = Markup.XmlLanguage.GetLanguage(Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag)            ' Establece el idioma a español, (para el euro).        
        VehiculosDg.DataContext = Vehiculo.ObtenerVehiculosPorIdGaraje(IdGaraje)

    End Sub

    Private Sub NuevoVehiculoBtn_Click(sender As Object, e As RoutedEventArgs)

        AbrirVntAddVehiculo(Foo.Accion.Insertar)

    End Sub

    Private Sub EliminarVehiculoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim vehiculoSelec As Vehiculo = CType(VehiculosDg.SelectedItem, Vehiculo)

        If vehiculoSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If Vehiculo.EliminarVehiculoPorId(vehiculoSelec.Id) Then

                If Plaza.CambiarSituacionPlazaToLibre(vehiculoSelec.IdGaraje, vehiculoSelec.IdPlaza) Then

                    VehiculosDg.DataContext = Garaje.ObtenerGarajes()
                    MessageBox.Show("Se ha eliminado el vehículo.", "Vehículo Eliminado", MessageBoxButton.OK, MessageBoxImage.Information)

                End If

            End If

        End If

    End Sub

    Private Sub ModificarVehiculoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim vehiculoSelec As Vehiculo = CType(VehiculosDg.SelectedItem, Vehiculo)

        If vehiculoSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            AbrirVntAddVehiculo(Foo.Accion.Modificar, vehiculoSelec)

        End If

    End Sub


    ''' <summary>
    ''' Abre "VntAddVehiculo" para añadir un vehículo.
    ''' </summary>
    ''' <param name="accion">Acción a realizar.</param>
    Private Sub AbrirVntAddVehiculo(ByRef accion As Foo.Accion)

        Dim vntAddVehiculo As New VntAddVehiculo(accion)
        vntAddVehiculo.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "VntAddVehiculo" para modificar un vehículo.
    ''' </summary>
    ''' <param name="accion">Acción a realizar.</param>
    ''' <param name="vehiculoSelec">Datos del vehículo seleccionado.</param>
    Private Sub AbrirVntAddVehiculo(ByRef accion As Foo.Accion, ByRef vehiculoSelec As Vehiculo)

        Dim vntAddVehiculo As New VntAddVehiculo(accion, vehiculoSelec)
        vntAddVehiculo.ShowDialog()

    End Sub

End Class
