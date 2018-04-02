Class PgPlazas

    ''' <summary>
    ''' Almacena el Id del garaje seleccionado de "VntSeleccGaraje".
    ''' </summary>
    Private IdGarajeSelec As Integer

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        PlazasDg.DataContext = Plaza.ObtenerPlazasPorIdGaraje(IdGarajeSelec)

    End Sub

    Public Sub New(ByRef idGaraje As Integer)

        InitializeComponent()
        Me.IdGarajeSelec = idGaraje

    End Sub

End Class
