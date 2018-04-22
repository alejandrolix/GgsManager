Imports Microsoft.Win32
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class VntAddCliente

    ''' <summary>
    ''' Para actualizar el DataGrid de clientes.
    ''' </summary>    
    Private PgClientes As PgClientes

    ''' <summary>
    ''' Contiene la ruta de la imagen seleccionada por el usuario.
    ''' </summary>
    Private UrlFotoSeleccionada As String
    Private Accion As Foo.Accion

    ''' <summary>
    ''' Contiene el cliente seleccionado.
    ''' </summary>
    Private ClienteSelec As Cliente

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Insertar Then

            Keyboard.Focus(NombreClienteTxt)

        ElseIf Accion = Foo.Accion.Modificar Then

            NombreClienteTxt.DataContext = ClienteSelec
            ApellidosClienteTxt.DataContext = ClienteSelec
            DNIClienteTxt.DataContext = ClienteSelec
            DireccionClienteTxt.DataContext = ClienteSelec
            PoblacionClienteTxt.DataContext = ClienteSelec
            ProvinciaClienteTxt.DataContext = ClienteSelec
            MovilClienteTxt.DataContext = ClienteSelec
            ObservClienteTxt.DataContext = ClienteSelec

            If ClienteSelec.Ivm IsNot Nothing Then

                ClienteImg.DataContext = ClienteSelec

            End If

        End If

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos del cliente son correctos.
    ''' </summary>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayNombre, hayApellidos, hayDNI, hayDireccion, hayPoblacion, hayProvincia, hayMovil As Boolean

        ' Comprobación de errores.

        If Foo.HayTexto(NombreClienteTxt.Text) Then

            hayNombre = True
        Else

            MessageBox.Show("Tienes que introducir un nombre.", "Nombre Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ApellidosClienteTxt.Text) Then

            hayApellidos = True
        Else

            MessageBox.Show("Tienes que introducir unos apellidos.", "Apellidos Vacíos", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Not Foo.HayTexto(DNIClienteTxt.Text) Then

            MessageBox.Show("Tienes que introducir un D.N.I.", "DNI Vacío", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            hayDNI = True

        End If

        If Foo.HayTexto(DireccionClienteTxt.Text) Then

            hayDireccion = True
        Else

            MessageBox.Show("Tienes que introducir una dirección.", "Dirección Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(PoblacionClienteTxt.Text) Then

            hayPoblacion = True
        Else

            MessageBox.Show("Tienes que introducir una población.", "Población Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ProvinciaClienteTxt.Text) Then

            hayProvincia = True
        Else

            MessageBox.Show("Tienes que introducir una província.", "Província Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Dim exprMovil As New Regex("^[0-9]{9}$")

        If Not Foo.HayTexto(MovilClienteTxt.Text) Then

            MessageBox.Show("Tienes que introducir un móvil.", "Móvil Vacío", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            hayMovil = True

        End If

        Return hayNombre And hayApellidos And hayDNI And hayDireccion And hayPoblacion And hayProvincia And hayMovil

    End Function


    ''' <summary>
    ''' Limpia los datos del cliente introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreClienteTxt.ClearValue(TextBox.TextProperty)
        ApellidosClienteTxt.ClearValue(TextBox.TextProperty)
        DNIClienteTxt.ClearValue(TextBox.TextProperty)
        DireccionClienteTxt.ClearValue(TextBox.TextProperty)
        PoblacionClienteTxt.ClearValue(TextBox.TextProperty)
        ProvinciaClienteTxt.ClearValue(TextBox.TextProperty)
        MovilClienteTxt.ClearValue(TextBox.TextProperty)

        If Foo.HayTexto(ObservClienteTxt.Text) Then             ' Si hay observaciones, las borramos.

            ObservClienteTxt.ClearValue(TextBox.TextProperty)

        End If

        If Cliente.HayImagen(ClienteImg.Source) Then                ' Si hay una imagen, la borramos.

            ClienteImg.ClearValue(Image.SourceProperty)

        End If

    End Sub

    Private Sub AddFotoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim abrirArchivo As New OpenFileDialog()
        abrirArchivo.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png"

        If abrirArchivo.ShowDialog() Then

            Me.UrlFotoSeleccionada = abrirArchivo.FileName

            Dim img As BitmapImage = ReducirImagen(UrlFotoSeleccionada)
            ClienteImg.Source = img             ' Establecemos la nueva imagen al cliente.

        End If

    End Sub


    ''' <summary>
    ''' Reduce el tamaño de la imagen seleccionada.
    ''' </summary>
    ''' <param name="rutaImg">Ruta de la imagen seleccionada.</param>
    ''' <returns>Imagen reducida.</returns>
    Private Function ReducirImagen(ByRef rutaImg As String) As BitmapImage

        Dim img As New BitmapImage(New Uri(rutaImg))
        img.DecodePixelWidth = 106

        Return img

    End Function


    ''' <summary>
    ''' Crea un archivo JPG que contiene la imagen que ha seleccionado el usuario.
    ''' </summary>
    ''' <param name="img">El origen de la foto seleccionada.</param>
    Private Sub GuardarFoto(ByRef img As ImageSource)

        Dim nuevoId As Integer = Cliente.ObtenerNuevoIdClientes()

        If nuevoId <= 0 Then

            MessageBox.Show("Ha habido un problema al obtener el nuevo Id del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim bitmap As New BitmapImage()

            bitmap.BeginInit()
            bitmap.DecodePixelWidth = 106
            bitmap.DecodePixelHeight = 92
            bitmap.CacheOption = BitmapCacheOption.OnLoad
            bitmap.UriSource = New Uri(UrlFotoSeleccionada, UriKind.RelativeOrAbsolute)
            bitmap.EndInit()

            Dim encoder As New JpegBitmapEncoder()
            encoder.Frames.Add(BitmapFrame.Create(bitmap))

            Dim cadena As New StringBuilder()
            cadena.Append(My.Settings.RutaClientes).Append(nuevoId).Append(".jpg")

            UrlFotoSeleccionada = cadena.ToString()         ' Establecemos la nueva ruta de la imagen a guardar.

            Using stream As New FileStream(UrlFotoSeleccionada, FileMode.Create)

                encoder.Save(stream)

            End Using

        End If

    End Sub

    Private Sub GuardarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            If Accion = Foo.Accion.Insertar Then

                Dim cliente As New Cliente(NombreClienteTxt.Text, ApellidosClienteTxt.Text, DNIClienteTxt.Text, DireccionClienteTxt.Text, PoblacionClienteTxt.Text, ProvinciaClienteTxt.Text,
                                           MovilClienteTxt.Text, ObservClienteTxt.Text)

                If Cliente.HayImagen(ClienteImg.Source) Then             ' Si el usuario ha seleccionado una imagen, la guardamos.

                    GuardarFoto(ClienteImg.Source)

                End If

                If cliente.Insertar() Then          ' Guardamos el cliente.

                    MessageBox.Show("Se ha añadido el cliente.", "Cliente Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()
                Else

                    MessageBox.Show("Ha habido un problema al añadir el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                Dim cliente As New Cliente(ClienteSelec.Id, NombreClienteTxt.Text, ApellidosClienteTxt.Text, DNIClienteTxt.Text, DireccionClienteTxt.Text, PoblacionClienteTxt.Text, ProvinciaClienteTxt.Text,
                                           MovilClienteTxt.Text, ObservClienteTxt.Text)

                If cliente.Modificar() Then

                    MessageBox.Show("Se ha modificado el cliente.", "Cliente Modificado", MessageBoxButton.OK, MessageBoxImage.Information)
                Else

                    MessageBox.Show("Ha habido un problema al modificar los datos del cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            End If

            PgClientes.Vista.Source = Cliente.ObtenerClientes()         ' Actualizamos el DataGrid de Clientes.

            If PgClientes.Vista.Source Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener los clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            Else

                PgClientes.ClientesDg.DataContext = PgClientes.Vista.Source

            End If

        End If

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef clienteSelec As Cliente, ByRef pgClientes As PgClientes)

        InitializeComponent()

        Me.Accion = accion
        Me.ClienteSelec = clienteSelec
        Me.PgClientes = pgClientes

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef pgClientes As PgClientes)

        InitializeComponent()

        Me.Accion = accion
        Me.PgClientes = pgClientes

    End Sub

    Private Sub DNIClienteTxt_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)

        Dim exprDniEspanol As New Regex("^[0-9]{8}[A-Z]$")
        Dim exprDniExtranjero As New Regex("^X|Y[0-9]{7}[A-Z]$")

        If Not exprDniEspanol.IsMatch(DNIClienteTxt.Text) And Not exprDniExtranjero.IsMatch(DNIClienteTxt.Text) Then

            MessageBox.Show("Tienes que introducir un formato de D.N.I. válido.", "Formato D.N.I. Incorrecto", MessageBoxButton.OK, MessageBoxImage.Error)
            Keyboard.Focus(DNIClienteTxt)

        End If

    End Sub

    Private Sub MovilClienteTxt_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)

        Dim exprMovil As New Regex("^[0-9]{9}$")

        If Not exprMovil.IsMatch(MovilClienteTxt.Text) Then

            MessageBox.Show("Tienes que introducir un número que tenga 9 dígitos.", "Formato de Móvil Incorrecto", MessageBoxButton.OK, MessageBoxImage.Error)
            Keyboard.Focus(MovilClienteTxt)

        End If

    End Sub

End Class
