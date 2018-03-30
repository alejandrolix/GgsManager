Public Class VntSeleccCliente

    Private IdGarajeSelec As Integer

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        ClientesCmb.DataContext = Cliente.ObtenerNombreYApellidosClientesPorIdGaraje(IdGarajeSelec)
        ClientesCmb.SelectedIndex = 0

    End Sub

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim clienteSelec As Cliente = CType(ClientesCmb.SelectedItem, Cliente)

        If clienteSelec IsNot Nothing Then

            Dim factura As New Factura(Date.Now.Date, clienteSelec.Id, True)

            If factura.InsertarParaCliente() Then

                Dim formFactIndividual As New FormFactIndividual(clienteSelec.Id)
                formFactIndividual.ShowDialog()

            End If

        End If

    End Sub

    Public Sub New(idGarajeSelec As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGarajeSelec

    End Sub

End Class
