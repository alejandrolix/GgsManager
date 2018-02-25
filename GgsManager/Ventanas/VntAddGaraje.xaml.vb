Public Class AddGaraje

    Property VntGarajes As VntGarajes              ' Almacena una instancia de la ventana "Gestión de Garajes".
    Property GarajeSelec As Garaje                   ' Contiene los datos del garaje seleccionado para poder modificarlos.
    Property Accion As Foo.Accion
    Property NumPlazas As Integer

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Modificar Then           ' Ponemos los datos del garaje seleccionado en los TextBoxs.

            NombreGarajeTxt.DataContext = GarajeSelec
            DireccionGarajeTxt.DataContext = GarajeSelec
            NumPlazasGarajeTxt.DataContext = GarajeSelec
            ObservGarajeTxt.DataContext = GarajeSelec

        End If

    End Sub

    Private Sub InsGarajeBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            If Accion = Foo.Accion.Insertar Then            ' Vamos a insertar un garaje.

                If Foo.HayTexto(ObservGarajeTxt.Text) Then

                    ' Creamos el nuevo garaje a insertar con observaciones.
                    Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, NumPlazas, ObservGarajeTxt.Text)

                    If GestionBd.InsertarGarajeConObservaciones(garaje) Then

                        MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                        LimpiarCampos()

                    End If
                Else

                    ' Creamos el nuevo garaje a insertar sin observaciones.
                    Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, NumPlazas, Nothing)

                    If GestionBd.InsertarGarajeSinObservaciones(garaje) Then                 ' Insertamos un garaje sin observaciones.

                        MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                        LimpiarCampos()

                    End If

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                If Foo.HayTexto(ObservGarajeTxt.Text) Then

                    ' Creamos el nuevo garaje a modificar con observaciones.
                    Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, NumPlazas, ObservGarajeTxt.Text)

                    If GestionBd.ModificarGarajeConObservaciones(garaje) Then

                        MessageBox.Show("Se ha modificado el garaje.", "Garaje Modificado", MessageBoxButton.OK, MessageBoxImage.Information)
                        LimpiarCampos()

                    End If
                Else

                    ' Creamos el nuevo garaje a modificar sin observaciones.
                    Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, NumPlazas, Nothing)

                    If GestionBd.ModificarGarajesSinObservaciones(garaje) Then                 ' Insertamos un garaje sin observaciones.

                        MessageBox.Show("Se ha modificado el garaje.", "Garaje Modificado", MessageBoxButton.OK, MessageBoxImage.Information)
                        LimpiarCampos()

                    End If

                End If

            End If

        End If

        VntGarajes.GarajesDg.DataContext = GestionBd.ObtenerGarajes()

    End Sub


    ''' <summary>
    ''' Comprueba si los datos del garaje introducidos son correctos.
    ''' </summary>
    ''' <returns>True: Los datos introducidos son correctos. False: Los datos introducidos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayNombre As Boolean, hayDireccion As Boolean, hayNumPlazas As Boolean

        If Not Foo.HayTexto(NombreGarajeTxt.Text) Then

            MessageBox.Show("Tienes que introducir un nombre", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        Else

            hayNombre = True

        End If

        If Not Foo.HayTexto(DireccionGarajeTxt.Text) Then

            MessageBox.Show("Tienes que introducir una dirección", "Dirección Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        Else

            hayDireccion = True

        End If

        Try
            Me.NumPlazas = Integer.Parse(NumPlazasGarajeTxt.Text)
            hayNumPlazas = True

        Catch ex As Exception

            MessageBox.Show("Tienes que introducir un número de plazas.", "Nº de Plazas Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If NumPlazasGarajeTxt.Text.Length > 0 Then

                NumPlazasGarajeTxt.Text = ""

            End If

        End Try

        Return hayNombre And hayDireccion And hayNumPlazas

    End Function


    ''' <summary>
    ''' Limpia los datos del garaje introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreGarajeTxt.Text = ""
        DireccionGarajeTxt.Text = ""
        NumPlazasGarajeTxt.Text = ""

        If ObservGarajeTxt.Text.Length > 0 Then

            ObservGarajeTxt.Text = ""

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

End Class
