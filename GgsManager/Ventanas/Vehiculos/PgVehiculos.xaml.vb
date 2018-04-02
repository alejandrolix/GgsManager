Class PgVehiculos

    ''' <summary>
    ''' Almacena el Id del garaje seleccionado.
    ''' </summary>    
    Private IdGarajeSelec As Integer

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        VehiculosDg.Language = Markup.XmlLanguage.GetLanguage(Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag)            ' Establece el idioma a español, (para el euro).        
        VehiculosDg.DataContext = Vehiculo.ObtenerVehiculosPorIdGaraje(IdGarajeSelec)

    End Sub

    Private Sub NuevoVehiculoBtn_Click(sender As Object, e As RoutedEventArgs)

        AbrirVntAddVehiculo(Foo.Accion.Insertar, IdGarajeSelec)

    End Sub

    Private Sub EliminarVehiculoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim vehiculoSelec As Vehiculo = CType(VehiculosDg.SelectedItem, Vehiculo)

        If vehiculoSelec Is Nothing Then

            MessageBox.Show("Tienes que seleccionar un vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            If vehiculoSelec.Eliminar() Then

                If Plaza.CambiarSituacionPlazaALibre(vehiculoSelec.IdPlaza, vehiculoSelec.IdGaraje) Then

                    VehiculosDg.DataContext = Vehiculo.ObtenerVehiculosPorIdGaraje(IdGarajeSelec)
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
    Private Sub AbrirVntAddVehiculo(ByRef accion As Foo.Accion, ByRef idGaraje As Integer)

        Dim vntAddVehiculo As New VntAddVehiculo(accion, idGaraje, Me)
        vntAddVehiculo.ShowDialog()

    End Sub


    ''' <summary>
    ''' Abre "VntAddVehiculo" para modificar un vehículo.
    ''' </summary>
    ''' <param name="accion">Acción a realizar.</param>
    ''' <param name="vehiculoSelec">Datos del vehículo seleccionado.</param>
    Private Sub AbrirVntAddVehiculo(ByRef accion As Foo.Accion, ByRef vehiculoSelec As Vehiculo)

        Dim vntAddVehiculo As New VntAddVehiculo(accion, vehiculoSelec, Me)
        vntAddVehiculo.Title = "Modificar Vehículo"
        vntAddVehiculo.ShowDialog()

    End Sub

    Public Sub New(ByRef idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class
