Imports Microsoft.Win32
Imports System.IO

Public Class VntAddCliente

    Property VntClientes As VntClientes
    Private UrlFoto As String
    Private Accion As Foo.Accion
    Private ClienteSelec As Cliente                 ' Almacena el cliente seleccionado.

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

            If Foo.HayTexto(ClienteSelec.UrlFoto) Then

                Dim ivm As New ImageViewModel(ClienteSelec.UrlFoto)
                ClienteImg.DataContext = ivm

                'Dim bitmap As New BitmapImage()
                'bitmap.BeginInit()
                'bitmap.StreamSource = New FileStream(ClienteSelec.UrlFoto, FileMode.Open)
                'bitmap.EndInit()

                'ClienteImg.Source = bitmap

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

        If Foo.HayTexto(DNIClienteTxt.Text) Then

            hayDNI = True
        Else

            MessageBox.Show("Tienes que introducir un D.N.I.", "DNI Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

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

        If Foo.HayTexto(MovilClienteTxt.Text) Then

            hayMovil = True
        Else

            MessageBox.Show("Tienes que introducir un móvil.", "Móvil Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Return hayNombre And hayApellidos And hayDNI And hayDireccion And hayPoblacion And hayProvincia And hayMovil

    End Function


    ''' <summary>
    ''' Limpia los datos introducidos después de haber añadido el cliente en la base de datos.
    ''' </summary>
    Private Sub LimpiarCampos()

        NombreClienteTxt.Text = ""
        ApellidosClienteTxt.Text = ""
        DNIClienteTxt.Text = ""
        DireccionClienteTxt.Text = ""
        PoblacionClienteTxt.Text = ""
        ProvinciaClienteTxt.Text = ""
        MovilClienteTxt.Text = ""

        If ObservClienteTxt.Text.Length >= 1 Then

            ObservClienteTxt.Text = ""

        End If

    End Sub

    Private Sub AddFotoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim abrirArchivo As New OpenFileDialog()
        abrirArchivo.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png"

        If abrirArchivo.ShowDialog() Then

            Me.UrlFoto = abrirArchivo.FileName

            Dim img As BitmapImage = ReducirImagen(abrirArchivo.FileName)
            ClienteImg.Source = img

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

        Dim ultimoId As Integer = GestionBd.ObtenerUltimoIdClientes()
        Dim bitmap As New BitmapImage()

        bitmap.BeginInit()
        bitmap.DecodePixelWidth = 106
        bitmap.DecodePixelHeight = 92
        bitmap.CacheOption = BitmapCacheOption.OnLoad
        bitmap.UriSource = New Uri(UrlFoto)
        bitmap.EndInit()

        Dim encoder As New JpegBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(bitmap))

        Using stream As New FileStream(My.Settings.RutaImgs & "5.jpg", FileMode.Create)

            encoder.Save(stream)

        End Using

        ' Me.UrlFoto = "..\..\" & My.Settings.RutaImgs & ultimoId & ".jpg"            ' Asignamos la nueva ruta de la foto.

    End Sub

    Private Sub GuardarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            ' Creamos el cliente.
            Dim cliente As New Cliente(NombreClienteTxt.Text, ApellidosClienteTxt.Text, DNIClienteTxt.Text, DireccionClienteTxt.Text, PoblacionClienteTxt.Text, ProvinciaClienteTxt.Text, MovilClienteTxt.Text, ObservClienteTxt.Text, UrlFoto)

            If Accion = Foo.Accion.Insertar Then

                If ClienteImg.Source IsNot Nothing Then             ' Si el usuario ha seleccionado una imagen, la guardamos.

                    GuardarFoto(ClienteImg.Source)

                End If

                If GestionBd.InsertarCliente(cliente) Then

                    MessageBox.Show("Cliente Añadido.", "", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If

            End If

            VntClientes.ClientesDg.DataContext = GestionBd.ObtenerClientes()
            UrlFoto = ""

        End If

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef clienteSelec As Cliente)

        InitializeComponent()

        Me.Accion = accion
        Me.ClienteSelec = clienteSelec

    End Sub

    Public Sub New(ByRef accion As Foo.Accion)

        InitializeComponent()
        Me.Accion = accion

    End Sub

End Class
