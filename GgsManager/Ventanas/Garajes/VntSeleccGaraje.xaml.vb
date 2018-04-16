Public Class VntSeleccGaraje

    Private VntPrincipal As VntPrincipal
    Private Ventana As Foo.Ventana

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        GarajesCmb.DataContext = Garaje.ObtenerNombresGarajes()

        If GarajesCmb.DataContext Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los nombres de los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            GarajesCmb.SelectedIndex = 0

        End If

    End Sub

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim gjSelec As Garaje = CType(GarajesCmb.SelectedItem, Garaje)

        If gjSelec IsNot Nothing Then

            Select Case Ventana
                Case Foo.Ventana.Vehiculos
                    VntPrincipal.Frame.Content = New PgVehiculos(gjSelec.Id)

                Case Foo.Ventana.Plazas
                    VntPrincipal.Frame.Content = New PgPlazas(gjSelec.Id)

                Case Foo.Ventana.InformeClientes
                    Dim formInfClientes As New FormInfClientes(gjSelec.Id)
                    formInfClientes.ShowDialog()

                Case Foo.Ventana.InformePlazas
                    Dim formInfPlazas As New FormInfPlazas(gjSelec.Id)
                    formInfPlazas.ShowDialog()

                Case Foo.Ventana.InformeEstadGaraje
                    'Dim formEstGarajes As New FormEstGarajes(False, gjSelec.Id)
                    'formEstGarajes.ShowDialog()

                Case Foo.Ventana.FacturaIndividual
                    Dim vntSeleccCliente As New VntSeleccCliente(gjSelec.Id)
                    vntSeleccCliente.ShowDialog()

                Case Foo.Ventana.FacturaGaraje
                    Dim factura As New Factura(Date.Now.Date, gjSelec.Id, False)

                    If factura.InsertarParaGaraje() Then

                        Dim formFactConjunto As New FormFactConjunto(gjSelec.Id)
                        formFactConjunto.EmpezarImpresion()
                    Else

                        MessageBox.Show("Ha habido un problema al añadir la factura al garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                    End If

            End Select

            If Ventana <> Foo.Ventana.InformeEstadGaraje Then

                Me.Close()

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
