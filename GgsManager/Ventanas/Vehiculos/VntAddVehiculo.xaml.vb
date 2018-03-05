Imports System.IO
Imports System.Text
Imports Microsoft.Win32

Public Class VntAddVehiculo

    Private ArrayUrlsImgs As String()

    ''' <summary>
    ''' Indica cuantas veces se ha pulsado al botón "Examinar".
    ''' </summary>
    Private NumPulsacionesBtnExaminar As Integer
    Private PrecioBase As Decimal

    ''' <summary>
    ''' Almacena el Id del garaje seleccionado por el usuario.
    ''' </summary>
    Private IdGaraje As Integer
    Private Accion As Foo.Accion
    Private VehiculoSelec As Vehiculo

    ''' <summary>
    ''' Para actualizar el DataGrid de vehículos.
    ''' </summary>    
    Property VntVehiculos As VntVehiculos

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        If Accion = Foo.Accion.Insertar Then

            GarajesCmb.IsEnabled = False
            Keyboard.Focus(MatrVehiculoTxt)

            ClientesCmb.DataContext = Cliente.ObtenerNombreYApellidosClientes()           ' Cargamos los clientes en su ComboBox.
            ClientesCmb.SelectedIndex = 0

            PlazasCmb.DataContext = Plaza.ObtenerIdPlazasLibresPorIdGaraje(IdGaraje)          ' Cargamos los Ids de las plazas en su ComboBox.
            PlazasCmb.SelectedIndex = 0

            Me.NumPulsacionesBtnExaminar = 0
            PrecTotalVehiculoTxt.Visibility = Visibility.Collapsed

        ElseIf Accion = Foo.Accion.Modificar Then

            MatrVehiculoTxt.DataContext = VehiculoSelec
            MarcaVehiculoTxt.DataContext = VehiculoSelec
            ModVehiculoTxt.DataContext = VehiculoSelec

            PrecioLbl.Text = "Precio Total"
            PrecBaseVehiculoTxt.Visibility = Visibility.Collapsed

            PrecTotalVehiculoTxt.Language = Markup.XmlLanguage.GetLanguage(Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag)               ' Cambiamos el "." por una ",".
            PrecTotalVehiculoTxt.DataContext = VehiculoSelec

            Dim listaGarajes As List(Of Garaje) = Garaje.ObtenerNombresGarajes()
            GarajesCmb.DataContext = listaGarajes               ' Cargamos los garajes en su ComboBox.

            Dim posicionGaraje As Integer = ObtenerPosicionGaraje(listaGarajes)

            If posicionGaraje <> -1 Then

                GarajesCmb.SelectedIndex = posicionGaraje

            End If

            Dim listaClientes As List(Of Cliente) = Cliente.ObtenerNombreYApellidosClientes()
            ClientesCmb.DataContext = listaClientes                 ' Cargamos los clientes en su ComboBox.

            Dim posicionCliente As Integer = ObtenerPosicionCliente(listaClientes)

            If posicionCliente <> -1 Then

                ClientesCmb.SelectedIndex = posicionCliente

            End If

            Dim listaPlazasOcupadas As List(Of Plaza) = Plaza.ObtenerIdPlazasOcupadasPorIdGaraje(listaGarajes(posicionGaraje).Id)
            PlazasCmb.DataContext = listaPlazasOcupadas               ' Cargamos las plazas ocupadas en su ComboBox.

            Dim posicionPlaza As Integer = ObtenerPosicionPlaza(listaPlazasOcupadas)

            If posicionPlaza <> -1 Then

                PlazasCmb.SelectedIndex = posicionPlaza

            End If

        End If

    End Sub


    ''' <summary>
    ''' Obtiene la posición de la lista donde el nombre y apellidos del cliente coincida con el del vehículo seleccionado.
    ''' </summary>
    ''' <param name="listaClientes">La lista de los clientes.</param>
    ''' <returns>La posición de la lista.</returns>
    Private Function ObtenerPosicionCliente(ByRef listaClientes As List(Of Cliente)) As Integer

        For i As Integer = 0 To listaClientes.Count - 1 Step 1

            Dim cliente As Cliente = listaClientes(i)

            If cliente.Nombre.Equals(VehiculoSelec.Cliente.Nombre) And cliente.Apellidos.Equals(VehiculoSelec.Cliente.Apellidos) Then

                Return i

            End If

        Next

        Return -1

    End Function


    ''' <summary>
    ''' Obtiene la posición de la lista donde el Id del garaje coincida con el del vehículo seleccionado.
    ''' </summary>
    ''' <param name="listaGarajes">La lista de los garajes.</param>
    ''' <returns>La posición de la lista.</returns>
    Private Function ObtenerPosicionGaraje(ByRef listaGarajes As List(Of Garaje)) As Integer

        For i As Integer = 0 To listaGarajes.Count - 1 Step 1

            Dim garaje As Garaje = listaGarajes(i)

            If garaje.Id = VehiculoSelec.IdGaraje Then

                Return i

            End If

        Next

        Return -1

    End Function


    ''' <summary>
    ''' Obtiene la posición de la lista donde el Id de la plaza coincida con el del vehículo seleccionado.
    ''' </summary>
    ''' <param name="listaPlazasLibres">La lista de las plazas libres.</param>
    ''' <returns>La posición de la lista.</returns>
    Private Function ObtenerPosicionPlaza(ByRef listaPlazasLibres As List(Of Plaza)) As Integer

        For i As Integer = 0 To listaPlazasLibres.Count - 1 Step 1

            Dim plaza As Plaza = listaPlazasLibres(i)

            If plaza.Id = VehiculoSelec.IdPlaza Then

                Return i

            End If

        Next

        Return -1

    End Function

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

            Dim clienteSelec As Cliente = CType(ClientesCmb.SelectedItem, Cliente)              ' Obtenemos los datos del cliente seleccionado en su ComboBox.
            Dim plazaSelec As Plaza = CType(PlazasCmb.SelectedItem, Plaza)              ' Obtenemos los datos de la plaza seleccionada en su ComboBox.            

            Dim porcentajeIva As Decimal = (PrecioBase * Foo.LeerIVA) / 100
            Dim cliente As New Cliente(clienteSelec.Id)         ' Creamos el cliente.

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

                ' Calculamos el precio total para el pago mensual.
                Dim precioTotal As Decimal = PrecioBase + porcentajeIva
                Dim vehiculoo As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, cliente, IdGaraje, plazaSelec.Id, PrecioBase, precioTotal)

                If PrecBaseVehiculoTxt.Text.Contains(".") Then              ' Si el usuario introduce un ".", se tiene que cambiar por una ",".

                    MessageBox.Show("Tienes que cambiar el ""."" por "",""", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    If Vehiculo.InsertarVehiculo(vehiculoo) Then

                        If Plaza.CambiarSituacionPlazaToOcupada(plazaSelec.Id, IdGaraje) Then

                            MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                            LimpiarCampos()

                        End If

                    End If

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                PrecioBase = Decimal.Parse(PrecTotalVehiculoTxt.Text)
                PrecioBase -= porcentajeIva

                Dim vehiculoo As New Vehiculo(VehiculoSelec.Id, MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, cliente, IdGaraje, plazaSelec.Id, PrecioBase, Decimal.Parse(PrecTotalVehiculoTxt.Text))

                If PrecTotalVehiculoTxt.Text.Contains(".") Then

                    MessageBox.Show("Tienes que cambiar el ""."" por "",""", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    If Vehiculo.ModificarVehiculo(vehiculoo) Then

                        MessageBox.Show("Se ha modificado el vehículo.", "Vehículo Modificado", MessageBoxButton.OK, MessageBoxImage.Information)

                    End If

                End If

            End If

            VntVehiculos.VehiculosDg.DataContext = Vehiculo.ObtenerVehiculosPorIdGaraje(IdGaraje)               ' Actualizamos el DataGrid de vehículos.

        End If

    End Sub


    ''' <summary>
    ''' Limpia los datos introducidos por el usuario.
    ''' </summary>
    Private Sub LimpiarCampos()

        MatrVehiculoTxt.ClearValue(TextBox.TextProperty)
        MarcaVehiculoTxt.ClearValue(TextBox.TextProperty)
        ModVehiculoTxt.ClearValue(TextBox.TextProperty)

        ClientesCmb.ClearValue(ComboBox.TextProperty)
        PrecBaseVehiculoTxt.ClearValue(TextBox.TextProperty)
        PlazasCmb.ClearValue(ComboBox.TextProperty)

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

        Dim hayMatricula, hayMarca, hayModelo As Boolean

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

        If Accion = Foo.Accion.Insertar Then

            Dim hayPrecioBase As Boolean

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

        ElseIf Accion = Foo.Accion.Modificar Then

            Dim hayPrecioTotal As Boolean

            Try
                Decimal.Parse(PrecTotalVehiculoTxt.Text)
                hayPrecioTotal = True

            Catch ex As Exception

                MessageBox.Show("Tienes que introducir un precio total.", "Precio Total Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

                If Foo.HayTexto(PrecTotalVehiculoTxt.Text) Then

                    PrecTotalVehiculoTxt.ClearValue(TextBox.TextProperty)

                End If

            End Try

            Return hayMatricula And hayMarca And hayModelo And hayPrecioTotal

        End If

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
