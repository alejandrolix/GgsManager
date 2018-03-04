Public Class AddGaraje

    ''' <summary>
    ''' Para actualizar el DataGrid de garajes.
    ''' </summary>    
    Property VntGarajes As VntGarajes

    ''' <summary>
    ''' Contiene los datos del garaje seleccionado.
    ''' </summary>
    Private GarajeSelec As Garaje
    Private Accion As Foo.Accion
    Private NumPlazas As Integer

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Insertar Then

            Keyboard.Focus(NombreGarajeTxt)

        ElseIf Accion = Foo.Accion.Modificar Then

            NombreGarajeTxt.DataContext = GarajeSelec
            DireccionGarajeTxt.DataContext = GarajeSelec
            NumPlazasGarajeTxt.DataContext = GarajeSelec
            ObservGarajeTxt.DataContext = GarajeSelec

        End If

    End Sub


    ''' <summary>
    ''' Comprueba si los datos del garaje introducidos son correctos.
    ''' </summary>
    ''' <returns>True: Los datos introducidos son correctos. False: Los datos introducidos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayNombre As Boolean, hayDireccion As Boolean, hayNumPlazas As Boolean

        If Not Foo.HayTexto(NombreGarajeTxt.Text) Then

            MessageBox.Show("Tienes que introducir un nombre.", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            hayNombre = True

        End If

        If Not Foo.HayTexto(DireccionGarajeTxt.Text) Then

            MessageBox.Show("Tienes que introducir una dirección.", "Dirección Vacía", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            hayDireccion = True

        End If

        If Not Foo.HayTexto(NumPlazasGarajeTxt.Text) Then

            MessageBox.Show("Tienes que introducir un número de plazas.", "Número de Plazas Vacío", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Try
                Me.NumPlazas = Integer.Parse(NumPlazasGarajeTxt.Text)

                If NumPlazas <= 0 Then

                    MessageBox.Show("Tienes que introducir un número de plazas mayor que 0.", "Número de Plazas Incorrecto", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    hayNumPlazas = True

                End If

            Catch ex As Exception

                MessageBox.Show("Tienes que introducir un número de plazas.", "Número de Plazas Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

                If Foo.HayTexto(NumPlazasGarajeTxt.Text) Then

                    NumPlazasGarajeTxt.ClearValue(TextBox.TextProperty)

                End If

            End Try

        End If

        Return hayNombre And hayDireccion And hayNumPlazas

    End Function


    ''' <summary>
    ''' Limpia los datos del garaje introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreGarajeTxt.ClearValue(TextBox.TextProperty)
        DireccionGarajeTxt.ClearValue(TextBox.TextProperty)
        NumPlazasGarajeTxt.ClearValue(TextBox.TextProperty)

        If Foo.HayTexto(ObservGarajeTxt.Text) Then

            ObservGarajeTxt.ClearValue(TextBox.TextProperty)

        End If

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef garaje As Garaje)

        InitializeComponent()

        Me.Accion = accion
        Me.GarajeSelec = garaje

    End Sub

    Public Sub New(ByRef accion As Foo.Accion)

        InitializeComponent()
        Me.Accion = accion

    End Sub

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            If Accion = Foo.Accion.Insertar Then

                Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, NumPlazas, ObservGarajeTxt.Text)
                garaje.Direccion = Foo.ComprobarDireccion(garaje.Direccion)

                If Garaje.InsertarGaraje(garaje) Then

                    Dim ultimoId As Integer = Garaje.ObtenerUltimoIdGarajes()               ' Obtenemos el último Id del garaje.

                    If Plaza.AddPlazasToGaraje(garaje.NumPlazas, ultimoId) Then

                        MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                        LimpiarCampos()

                    End If

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                Dim garaje As New Garaje(GarajeSelec.Id, NombreGarajeTxt.Text, DireccionGarajeTxt.Text, Integer.Parse(NumPlazasGarajeTxt.Text), ObservGarajeTxt.Text)
                garaje.Direccion = Foo.ComprobarDireccion(garaje.Direccion)

                If Garaje.ModificarGaraje(garaje) Then

                    MessageBox.Show("Se ha modificado el garaje.", "Garaje Modificado", MessageBoxButton.OK, MessageBoxImage.Information)

                End If

            End If

        End If

        VntGarajes.GarajesDg.DataContext = Garaje.ObtenerGarajes()              ' Actualizamos el DataGrid de Garajes.

    End Sub

End Class
