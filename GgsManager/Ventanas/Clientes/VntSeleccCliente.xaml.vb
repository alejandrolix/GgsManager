Public Class VntSeleccCliente

    Private IdGarajeSelec As Integer

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        ClientesCmb.DataContext = Cliente.ObtenerNombreYApellidosClientesPorIdGaraje(IdGarajeSelec)

        If ClientesCmb.DataContext Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener los nombres y apellidos de los clientes del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            ClientesCmb.SelectedIndex = 0

        End If

    End Sub

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim clienteSelec As Cliente = CType(ClientesCmb.SelectedItem, Cliente)

        If clienteSelec IsNot Nothing Then

            Dim factura As New Factura(Date.Now.Date, clienteSelec.Id, True)

            If factura.InsertarParaCliente() Then

                Dim formFactIndividual As New FormFactIndividual(clienteSelec.Id)
                formFactIndividual.ShowDialog()
            Else

                MessageBox.Show("Ha habido un problema al añadir la factura al cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub

    Public Sub New(idGarajeSelec As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGarajeSelec

    End Sub

End Class
