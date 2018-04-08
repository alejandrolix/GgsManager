Class PgPlazas

    ''' <summary>
    ''' Almacena el Id del garaje seleccionado de "VntSeleccGaraje".
    ''' </summary>
    Private IdGarajeSelec As Integer

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        PlazasDg.DataContext = Plaza.ObtenerPlazasPorIdGaraje(IdGarajeSelec)

        If PlazasDg.DataContext Is Nothing Then

            MessageBox.Show("Ha habido un error al obtener las plazas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

    End Sub

    Public Sub New(ByRef idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class
