Public Class AddGaraje

    Property VntGarajes As VntGarajes              ' Almacena una instancia de la ventana "Gestión de Garajes".
    Property Garaje As Garaje

    Private Sub InsGarajeBtn_Click(sender As Object, e As RoutedEventArgs)

        ComprobarDatosIntroducidos()
        VntGarajes.GarajesDg.DataContext = GestionarBd.ObtenerGarajes()

    End Sub


    ''' <summary>
    ''' Comprueba que los datos del garaje que ha introducido el usuario son correctos.
    ''' </summary>
    Private Sub ComprobarDatosIntroducidos()

        Dim hayNombre As Boolean, hayDireccion As Boolean, hayNumPlazas As Boolean
        Dim numPlazas As Integer

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
            numPlazas = Integer.Parse(NumPlazasGarajeTxt.Text)
            hayNumPlazas = True

        Catch ex As Exception

            MessageBox.Show("Tienes que introducir un número de plazas.", "Nº de Plazas Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If NumPlazasGarajeTxt.Text.Length > 0 Then

                NumPlazasGarajeTxt.Text = ""

            End If

        End Try

        If hayNombre And hayDireccion And hayNumPlazas Then             ' Si el usuario ha introducido bien los datos.

            If ObservGarajeTxt.Text.Length > 0 Then          ' Si en las observaciones del garaje hay texto, insertamos un garaje con observaciones.

                ' Creamos el nuevo garaje a insertar con observaciones.
                Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, numPlazas, ObservGarajeTxt.Text)

                If GestionarBd.InsertarGarajeConObservaciones(garaje) Then

                    MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If
            Else

                ' Creamos el nuevo garaje a insertar sin observaciones.
                Dim garaje As New Garaje(NombreGarajeTxt.Text, DireccionGarajeTxt.Text, numPlazas, Nothing)

                If GestionarBd.InsertarGarajeSinObservaciones(garaje) Then                 ' Insertamos un garaje sin observaciones.

                    MessageBox.Show("Se ha añadido el garaje.", "Garaje Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If

            End If

        End If

    End Sub


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


    ''' <summary>
    ''' Comprueba el tipo de acción para saber si utilizamos ésta ventana para añadir un garaje o para modificar sus datos.
    ''' </summary>
    ''' <param name="tipoAccion">Número que indica si vamos a insertar un garaje o modificar sus datos.</param>
    Private Sub ComprobarTipoAccion(ByRef tipoAccion As Integer)

        If tipoAccion = 1 Then              ' Ponemos los datos del garaje seleccionado para modificarlos.

            NombreGarajeTxt.Text = Garaje.Nombre
            DireccionGarajeTxt.Text = Garaje.Direccion
            NumPlazasGarajeTxt.Text = Garaje.NumPlazas
            ObservGarajeTxt.Text = Garaje.Observaciones

        End If

    End Sub


    ''' <summary>
    ''' Crea una ventana para modificar los datos de un garaje seleccionado.
    ''' </summary>
    ''' <param name="tipoAccion">Número que indica si vamos a insertar un garaje o modificar sus datos.</param>
    ''' <param name="garaje">Garaje seleccionado para poder modificar sus datos.</param>
    Public Sub New(ByRef tipoAccion As Integer, ByRef garaje As Garaje)

        InitializeComponent()

        Me.Garaje = garaje
        ComprobarTipoAccion(tipoAccion)

    End Sub


    ''' <summary>
    ''' Crea una ventana para añadir un garaje.
    ''' </summary>
    Public Sub New()

        InitializeComponent()

    End Sub

End Class
