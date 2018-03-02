Imports System.IO
Imports System.Text
Imports Microsoft.Win32

Public Class VntAddVehiculo

    Private ArrayUrlImgs As String()

    ''' <summary>
    ''' Indica cuantas veces se ha pulsado al botón "Examinar".
    ''' </summary>
    Private NumPulsacBtnExaminar As Integer

    ''' <summary>
    ''' Indica cuantas veces se ha guardado una foto.
    ''' </summary>
    Private NumVecesGuardadoFoto As Integer
    Private PrecioBase As Decimal
    Private PrecioTotal As Decimal
    Private HayImagen As Boolean

    ''' <summary>
    ''' Almacena el garaje seleccionado por el usuario.
    ''' </summary>
    Private GarajeSelec As Garaje

    ''' <summary>
    ''' Almacena el cliente seleccionado por el usuario.
    ''' </summary>
    Private ClienteSelec As Cliente
    Private Accion As Foo.Accion
    Private VehiculoSelec As Vehiculo

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Insertar Then

            GarajesCmb.DataContext = Garaje.ObtenerGarajes()             ' Cargamos los garajes en su ComboBox.
            GarajesCmb.SelectedIndex = 0

            ClientesCmb.DataContext = Cliente.ObtenerClientes()           ' Cargamos los clientes en su ComboBox.
            ClientesCmb.SelectedIndex = 0

            Me.NumPulsacBtnExaminar = 0
            Me.NumVecesGuardadoFoto = 0
            Me.HayImagen = False                ' Indica si, por lo menos, hay una imagen para guardar.

        ElseIf Accion = Foo.Accion.Modificar Then

            MatrVehiculoTxt.DataContext = VehiculoSelec
            MarcaVehiculoTxt.DataContext = VehiculoSelec
            ModVehiculoTxt.DataContext = VehiculoSelec
            GarajesCmb.DataContext = VehiculoSelec
            ClientesCmb.DataContext = VehiculoSelec
            PlazasCmb.DataContext = VehiculoSelec
            PrecBaseVehiculoTxt.DataContext = VehiculoSelec
            PrecTotalVehiculoTxt.DataContext = VehiculoSelec

        End If

    End Sub

    Private Sub GarajesCmb_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        Me.GarajeSelec = CType(GarajesCmb.SelectedItem, Garaje)

        PlazasCmb.DataContext = Plaza.ObtenerIdPlazasPorIdGaraje(GarajeSelec.Id)
        PlazasCmb.SelectedIndex = 0

    End Sub

    Private Sub ClientesCmb_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        Me.ClienteSelec = CType(ClientesCmb.SelectedItem, Cliente)

    End Sub

    Private Sub AddFotoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim abrirArchivo As New OpenFileDialog()
        abrirArchivo.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png"

        If abrirArchivo.ShowDialog() Then

            NumPulsacBtnExaminar += 1
            Me.ArrayUrlImgs = New String(1) {}

            Dim img As BitmapImage = ReducirImagen(abrirArchivo.FileName)

            If NumPulsacBtnExaminar = 1 Then

                Vehic1Img.Source = img
                ArrayUrlImgs(0) = abrirArchivo.FileName
            Else

                Vehic2Img.Source = img
                ArrayUrlImgs(1) = abrirArchivo.FileName

            End If

        End If

    End Sub


    ''' <summary>
    ''' Reduce el tamaño de la imagen seleccionada.
    ''' </summary>
    ''' <param name="rutaImg">Ruta de la imagen seleccionada.</param>
    ''' <returns>Imagen reducida.</returns>
    Private Function ReducirImagen(ByRef rutaImg As String) As BitmapImage

        Dim img As New BitmapImage(New Uri(rutaImg))
        img.DecodePixelWidth = 114

        Return img

    End Function

    Private Sub GuardarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            If Accion = Foo.Accion.Insertar Then

                If Foo.HayImagen(Vehic1Img.Source) Then             ' Comprobamos si, por lo menos, hay una imagen.

                    HayImagen = True

                ElseIf Foo.HayImagen(Vehic2Img.Source) Then

                    HayImagen = True

                End If

                If HayImagen Then

                    Dim nuevoId As Integer = Vehiculo.ObtenerNuevoIdVehiculos()

                    If Foo.HayImagen(Vehic1Img.Source) Then

                        GuardarFoto(Vehic1Img.Source, ArrayUrlImgs(0), nuevoId)

                    End If

                    If Foo.HayImagen(Vehic2Img.Source) Then

                        GuardarFoto(Vehic2Img.Source, ArrayUrlImgs(1), nuevoId)

                    End If

                    Dim cadenaUrls As New StringBuilder()

                    For Each url As String In ArrayUrlImgs

                        cadenaUrls.Append(url).Append(" ")

                    Next

                    Dim plazaSelecc As Plaza = CType(PlazasCmb.SelectedItem, Plaza)
                    Dim vehic As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, ClienteSelec, GarajeSelec.Id, plazaSelecc.Id, PrecioBase, PrecioTotal, cadenaUrls.ToString())

                    If Vehiculo.InsertarVehiculo(vehic) Then

                        MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Insertado", MessageBoxButton.OK, MessageBoxImage.Information)
                        LimpiarCampos()

                    End If

                End If

                Dim plazaSelec As Plaza = CType(PlazasCmb.SelectedItem, Plaza)
                Dim vehicc As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, ClienteSelec, GarajeSelec.Id, plazaSelec.Id, PrecioBase, PrecioTotal, Nothing)

                If Vehiculo.InsertarVehiculo(vehicc) Then

                    MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Insertado", MessageBoxButton.OK, MessageBoxImage.Information)
                    LimpiarCampos()

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                If Vehiculo.ModificarVehiculo(VehiculoSelec) Then

                    MessageBox.Show("Se ha modificado el vehículo.", "Vehículo Modificado", MessageBoxButton.OK, MessageBoxImage.Information)

                End If

            End If

        End If

    End Sub


    ''' <summary>
    ''' Limpia los datos introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        MatrVehiculoTxt.Text = ""
        MarcaVehiculoTxt.Text = ""
        ModVehiculoTxt.Text = ""

        If Foo.HayImagen(Vehic1Img.Source) Then

            Vehic1Img.Source = Nothing

        ElseIf Foo.HayImagen(Vehic2Img.Source) Then

            Vehic2Img.Source = Nothing

        End If

    End Sub


    ''' <summary>
    ''' Crea un archivo JPG que contiene la imagen que ha seleccionado el usuario.
    ''' </summary>
    ''' <param name="img">Imagen a convertir en archivo.</param>
    ''' <param name="urlFotoSeleccionada">URL de la foto seleccionada.</param>
    ''' <param name="nuevoId">Id de la foto.</param>
    Private Sub GuardarFoto(ByRef img As ImageSource, ByRef urlFotoSeleccionada As String, ByRef nuevoId As Integer)

        Dim bitmap As New BitmapImage()
        NumVecesGuardadoFoto += 1

        bitmap.BeginInit()
        bitmap.DecodePixelWidth = 106
        bitmap.DecodePixelHeight = 92
        bitmap.CacheOption = BitmapCacheOption.OnLoad
        bitmap.UriSource = New Uri(urlFotoSeleccionada)
        bitmap.EndInit()

        Dim encoder As New JpegBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(bitmap))

        Dim cadena As New StringBuilder()
        cadena.Append(My.Settings.RutaVehiculos).Append(nuevoId).Append("_").Append(NumVecesGuardadoFoto).Append(".jpg")

        ArrayUrlImgs(NumVecesGuardadoFoto - 1) = cadena.ToString()

        Using stream As New FileStream(ArrayUrlImgs(NumVecesGuardadoFoto - 1), FileMode.Create)

            encoder.Save(stream)

        End Using

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos por el usuario son correctos.
    ''' </summary>
    ''' <returns>True: Los datos son correctos. False: Los datos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayMatricula, hayMarca, hayModelo, hayPrecioBase, hayPrecioTotal As Boolean

        ' Comprobación de errores.

        If Foo.HayTexto(MatrVehiculoTxt.Text) Then

            hayMatricula = True
        Else

            MessageBox.Show("Tienes que introducir una matrícula.", "Matrícula Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(MarcaVehiculoTxt.Text) Then

            hayMarca = True
        Else

            MessageBox.Show("Tienes que introducir una marca.", "Marca Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        If Foo.HayTexto(ModVehiculoTxt.Text) Then

            hayModelo = True
        Else

            MessageBox.Show("Tienes que introducir un modelo.", "Modelo Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Try
            PrecioBase = Decimal.Parse(PrecBaseVehiculoTxt.Text)
            hayPrecioBase = True

        Catch ex As Exception

            MessageBox.Show("Tienes que introducir un precio base.", "Precio Base Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If Foo.HayTexto(PrecBaseVehiculoTxt.Text) Then

                PrecBaseVehiculoTxt.Text = ""

            End If

        End Try

        Try
            PrecioTotal = Decimal.Parse(PrecTotalVehiculoTxt.Text)
            hayPrecioTotal = True

        Catch ex As Exception

            MessageBox.Show("Tienes que introducir un precio total.", "Precio Total Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If Foo.HayTexto(PrecTotalVehiculoTxt.Text) Then

                PrecTotalVehiculoTxt.Text = ""

            End If

        End Try

        Return hayMatricula And hayMarca And hayModelo And hayPrecioBase And hayPrecioTotal

    End Function

    Public Sub New(accion As Foo.Accion)

        InitializeComponent()
        Me.Accion = accion

    End Sub

    Public Sub New(accion As Foo.Accion, vehiculoSelec As Vehiculo)

        InitializeComponent()

        Me.Accion = accion
        Me.VehiculoSelec = vehiculoSelec

    End Sub

End Class
