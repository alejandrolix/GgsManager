Imports System.IO
Imports System.Text
Imports Microsoft.Win32

Public Class VntAddVehiculo

    Private ArrayUrlsImgs As String()

    ''' <summary>
    ''' Indica cuantas veces se ha pulsado al botón "Examinar".
    ''' </summary>
    Private NumPulsacionesBtnExaminar As Integer

    ''' <summary>
    ''' Indica cuantas veces se ha guardado una foto.
    ''' </summary>
    Private NumVecesGuardadoFoto As Integer
    Private PrecioBase As Decimal

    ''' <summary>
    ''' Almacena el Id del garaje seleccionado por el usuario.
    ''' </summary>
    Private IdGaraje As Integer
    Private Accion As Foo.Accion
    Private VehiculoSelec As Vehiculo

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Insertar Then

            GarajesCmb.IsEnabled = False
            Keyboard.Focus(MatrVehiculoTxt)

            ClientesCmb.DataContext = Cliente.ObtenerNombreYApellidosClientes()           ' Cargamos los clientes en su ComboBox.
            ClientesCmb.SelectedIndex = 0

            PlazasCmb.DataContext = Plaza.ObtenerIdPlazasPorIdGaraje(IdGaraje)          ' Cargamos los Ids de las plazas en su ComboBox.
            PlazasCmb.SelectedIndex = 0

            Me.NumPulsacionesBtnExaminar = 0
            Me.NumVecesGuardadoFoto = 0

        ElseIf Accion = Foo.Accion.Modificar Then

            Me.Title = "Modificar Vehículo"

            MatrVehiculoTxt.DataContext = VehiculoSelec
            MarcaVehiculoTxt.DataContext = VehiculoSelec
            ModVehiculoTxt.DataContext = VehiculoSelec
            GarajesCmb.DataContext = VehiculoSelec
            PlazasCmb.DataContext = VehiculoSelec
            PrecBaseVehiculoTxt.DataContext = VehiculoSelec

        End If

    End Sub

    Private Sub AddFotoBtn_Click(sender As Object, e As RoutedEventArgs)

        Dim abrirArchivo As New OpenFileDialog()
        abrirArchivo.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png"

        If abrirArchivo.ShowDialog() Then

            NumPulsacionesBtnExaminar += 1
            Me.ArrayUrlsImgs = New String(1) {}

            Dim img As BitmapImage = ReducirImagen(abrirArchivo.FileName)

            If NumPulsacionesBtnExaminar = 1 Then

                Vehic1Img.Source = img
                ArrayUrlsImgs(0) = abrirArchivo.FileName
            Else

                Vehic2Img.Source = img
                ArrayUrlsImgs(1) = abrirArchivo.FileName

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

                Dim nuevoId As Integer

                If Foo.HayImagen(Vehic1Img.Source) Or Foo.HayImagen(Vehic2Img.Source) Then

                    nuevoId = Vehiculo.ObtenerNuevoIdVehiculos()

                End If

                If Foo.HayImagen(Vehic1Img.Source) Then

                    GuardarFoto(Vehic1Img.Source, ArrayUrlsImgs(0), nuevoId, 1)

                End If

                If Foo.HayImagen(Vehic2Img.Source) Then

                    GuardarFoto(Vehic2Img.Source, ArrayUrlsImgs(1), nuevoId, 2)

                End If

                If ArrayUrlsImgs IsNot Nothing Then

                    ArrayUrlsImgs = Nothing             ' Quitamos las URLs de las imágenes.

                End If

                Dim clienteSelec As Cliente = CType(ClientesCmb.SelectedItem, Cliente)
                Dim plazaSelec As Plaza = CType(PlazasCmb.SelectedItem, Plaza)

                Dim cliente As New Cliente(clienteSelec.Id)
                Dim vehiculoo As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, cliente, IdGaraje, plazaSelec.Id, PrecioBase)

                If PrecBaseVehiculoTxt.Text.Contains(".") Then              ' Si el usuario introduce un ".", se quejará.

                    MessageBox.Show("Tienes que cambiar el ""."" por "",""")
                Else

                    If Vehiculo.InsertarVehiculo(vehiculoo) Then

                        If Plaza.CambiarSituacionPlazaToOcupada(plazaSelec.Id, IdGaraje) Then

                            MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                            LimpiarCampos()

                        End If

                    End If

                End If

            End If


            'If Accion = Foo.Accion.Insertar Then

            '        Dim cadenaUrls As New StringBuilder()

            '        For Each url As String In ArrayUrlsImgs

            '            cadenaUrls.Append(url).Append(" ")

            '        Next

            '        Dim plazaSelecc As Plaza = CType(PlazasCmb.SelectedItem, Plaza)
            '        Dim vehic As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, ClienteSelec, IdGaraje, plazaSelecc.Id, PrecioBase, cadenaUrls.ToString())

            '        If Vehiculo.InsertarVehiculo(vehic) Then

            '            MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Insertado", MessageBoxButton.OK, MessageBoxImage.Information)
            '            LimpiarCampos()

            '        End If

            '    End If

            '    Dim plazaSelec As Plaza = CType(PlazasCmb.SelectedItem, Plaza)
            '    Dim vehicc As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, ClienteSelec, IdGaraje, plazaSelec.Id, PrecioBase, Nothing)

            '    If Vehiculo.InsertarVehiculo(vehicc) Then

            '        MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Insertado", MessageBoxButton.OK, MessageBoxImage.Information)
            '        LimpiarCampos()

            '    End If

            'ElseIf Accion = Foo.Accion.Modificar Then

            '    If Vehiculo.ModificarVehiculo(VehiculoSelec) Then

            '        MessageBox.Show("Se ha modificado el vehículo.", "Vehículo Modificado", MessageBoxButton.OK, MessageBoxImage.Information)

            '    End If

            'End If

        End If

    End Sub


    ''' <summary>
    ''' Limpia los datos introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        MatrVehiculoTxt.ClearValue(TextBox.TextProperty)
        MarcaVehiculoTxt.ClearValue(TextBox.TextProperty)
        ModVehiculoTxt.ClearValue(TextBox.TextProperty)

        If Foo.HayImagen(Vehic1Img.Source) Then

            Vehic1Img.ClearValue(Image.SourceProperty)

        End If

        If Foo.HayImagen(Vehic2Img.Source) Then

            Vehic2Img.ClearValue(Image.SourceProperty)

        End If

    End Sub


    ''' <summary>
    ''' Crea un archivo JPG que contiene la imagen que ha seleccionado el usuario.
    ''' </summary>
    ''' <param name="img">Imagen a convertir en archivo.</param>
    ''' <param name="urlFotoSeleccionada">URL de la foto seleccionada.</param>
    ''' <param name="nuevoId">Id de la foto.</param>
    ''' <param name="subId">SubId de la foto.</param>
    Private Sub GuardarFoto(ByRef img As ImageSource, ByRef urlFotoSeleccionada As String, ByRef nuevoId As Integer, ByRef subId As Integer)

        Dim bitmap As New BitmapImage()

        bitmap.BeginInit()
        bitmap.DecodePixelWidth = 106
        bitmap.DecodePixelHeight = 92
        bitmap.CacheOption = BitmapCacheOption.OnLoad
        bitmap.UriSource = New Uri(urlFotoSeleccionada)
        bitmap.EndInit()

        Dim encoder As New JpegBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(bitmap))

        Dim cadena As New StringBuilder()
        cadena.Append(My.Settings.RutaVehiculos).Append(nuevoId).Append("_").Append(subId).Append(".jpg")           ' Creamos la nueva ruta para guardar la imagen del vehículo.

        Using stream As New FileStream(cadena.ToString(), FileMode.Create)

            encoder.Save(stream)

        End Using

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos por el usuario son correctos.
    ''' </summary>
    ''' <returns>True: Los datos son correctos. False: Los datos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayMatricula, hayMarca, hayModelo, hayPrecioBase As Boolean

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
            Me.PrecioBase = Decimal.Parse(PrecBaseVehiculoTxt.Text)
            hayPrecioBase = True

        Catch ex As Exception

            MessageBox.Show("Tienes que introducir un precio base.", "Precio Base Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

            If Foo.HayTexto(PrecBaseVehiculoTxt.Text) Then

                PrecBaseVehiculoTxt.ClearValue(TextBox.TextProperty)

            End If

        End Try

        Return hayMatricula And hayMarca And hayModelo And hayPrecioBase

    End Function

    Public Sub New(accion As Foo.Accion, idGaraje As Integer)

        InitializeComponent()

        Me.Accion = accion
        Me.IdGaraje = idGaraje

    End Sub

    Public Sub New(accion As Foo.Accion, vehiculoSelec As Vehiculo)

        InitializeComponent()

        Me.Accion = accion
        Me.VehiculoSelec = vehiculoSelec

    End Sub

End Class
