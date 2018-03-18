Public Class VntSeleccGaraje

    Private VntPrincipal As VntPrincipal
    Private Ventana As Foo.Ventana

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesCmb.DataContext = Garaje.ObtenerNombresGarajes()
        GarajesCmb.SelectedIndex = 0

    End Sub

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim gjSelec As Garaje = CType(GarajesCmb.SelectedItem, Garaje)

        If gjSelec IsNot Nothing Then

            Dim vntVehic As WPF.MDI.MdiChild
            Dim vntPlz As WPF.MDI.MdiChild
            Dim formInfClientes As FormInfClientes
            ' Dim formInfPlazas As FormInfPlazas

            If Ventana = Foo.Ventana.Vehiculos Then

                vntVehic = New WPF.MDI.MdiChild()
                vntVehic.Title = "Gestión de Vehículos"
                vntVehic.Content = New VntVehiculos()

                VntVehiculos.IdGaraje = gjSelec.Id
                vntVehic.Width = 800
                vntVehic.Height = 401

            ElseIf Ventana = Foo.Ventana.Plazas Then

                vntPlz = New WPF.MDI.MdiChild()
                vntPlz.Title = "Gestión de Plazas"
                vntPlz.Content = New VntPlazas()

                VntPlazas.IdGaraje = gjSelec.Id
                vntPlz.Width = 500
                vntPlz.Height = 401

            ElseIf Ventana = Foo.Ventana.InfClientes Then

                formInfClientes = New FormInfClientes(gjSelec.Id)
                formInfClientes.ShowDialog()

            ElseIf Ventana = Foo.Ventana.InfPlazas Then

                'formInfPlazas = New FormInfPlazas(gjSelec.Id)
                'formInfPlazas.ShowDialog()

            End If

            Me.Close()

            If vntVehic IsNot Nothing Then

                VntPrincipal.ContenedorMDI.Children.Add(vntVehic)               ' Mostramos "VntVehiculos".

            ElseIf vntPlz IsNot Nothing Then

                VntPrincipal.ContenedorMDI.Children.Add(vntPlz)             ' Mostramos "VntPlazas".

            End If

        End If

    End Sub

    Public Sub New(vntPrincipal As VntPrincipal, ventana As Foo.Ventana)

        InitializeComponent()

        Me.VntPrincipal = vntPrincipal
        Me.Ventana = ventana

    End Sub

    Public Sub New(ventana As Foo.Ventana)

        InitializeComponent()
        Me.Ventana = ventana

    End Sub

End Class
