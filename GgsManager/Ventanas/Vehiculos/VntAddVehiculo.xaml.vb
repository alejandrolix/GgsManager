Imports System.Text.RegularExpressions

Public Class VntAddVehiculo

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
    Private PgVehiculos As PgVehiculos

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        PrecBaseVehiculoTxt.Language = Markup.XmlLanguage.GetLanguage(Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag)            ' Establece la moneda del precio base al euro.   

        Dim nombreGaraje As String = Garaje.ObtenerNombreGarajePorId(IdGaraje)

        If nombreGaraje Is Nothing Then

            MessageBox.Show("Ha habido un problema al obtener el nombre del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            NombreGarajeLbl.DataContext = nombreGaraje

            If Accion = Foo.Accion.Insertar Then

                Keyboard.Focus(MatrVehiculoTxt)

                ClientesCmb.DataContext = Cliente.ObtenerNombreYApellidosClientesSinVehiculo()           ' Cargamos los clientes en su ComboBox.

                If ClientesCmb.DataContext Is Nothing Then

                    MessageBox.Show("Ha habido un problema al obtener los nombres y apellidos de los clientes sin vehículos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    ClientesCmb.SelectedIndex = 0

                End If

                PlazasCmb.DataContext = Plaza.ObtenerIdPlazasLibresPorIdGaraje(IdGaraje)          ' Cargamos los Ids de las plazas libres en su ComboBox.

                If PlazasCmb.DataContext Is Nothing Then

                    MessageBox.Show("Ha habido un problema al obtener las plazas libres del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    PlazasCmb.SelectedIndex = 0

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                MatrVehiculoTxt.DataContext = VehiculoSelec
                MarcaVehiculoTxt.DataContext = VehiculoSelec
                ModVehiculoTxt.DataContext = VehiculoSelec

                PrecBaseVehiculoTxt.DataContext = VehiculoSelec

                Dim arrayPlazasOcupadas As Plaza() = Plaza.ObtenerIdPlazasOcupadasPorIdGaraje(IdGaraje)

                If arrayPlazasOcupadas Is Nothing Then

                    MessageBox.Show("Ha habido un problema al obtener las plazas ocupadas del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    PlazasCmb.DataContext = arrayPlazasOcupadas               ' Cargamos las plazas ocupadas en su ComboBox.

                    Dim posicionPlaza As Integer = ObtenerPosicionPlaza(arrayPlazasOcupadas)

                    If posicionPlaza <> -1 Then

                        PlazasCmb.SelectedIndex = posicionPlaza
                        PlazasCmb.IsEnabled = False

                    End If

                End If

                Dim arrayClientes As Cliente() = Cliente.ObtenerNombreYApellidosClientes()

                If arrayClientes Is Nothing Then             ' Comprobamos si hay datos.

                    MessageBox.Show("Ha habido un problema al obtener los nombres y apellidos de los clientes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else

                    ClientesCmb.DataContext = arrayClientes                 ' Cargamos los clientes en su ComboBox.                    

                    Dim posicionCliente As Integer = ObtenerPosicionCliente(arrayClientes)

                    If posicionCliente <> -1 Then

                        ClientesCmb.SelectedIndex = posicionCliente
                        ClientesCmb.IsEnabled = False

                    End If

                End If

            End If

        End If

    End Sub


    ''' <summary>
    ''' Obtiene la posición de la lista donde el nombre y apellidos del cliente coincida con el del vehículo seleccionado.
    ''' </summary>
    ''' <param name="arrayClientes">El array de los clientes.</param>
    ''' <returns>La posición del array.</returns>
    Private Function ObtenerPosicionCliente(ByRef arrayClientes As Cliente()) As Integer

        For i As Integer = 0 To arrayClientes.Count - 1 Step 1

            Dim cliente As Cliente = arrayClientes(i)

            If cliente.Nombre.Equals(VehiculoSelec.Cliente.Nombre) And cliente.Apellidos.Equals(VehiculoSelec.Cliente.Apellidos) Then

                Return i

            End If

        Next

        Return -1

    End Function


    ''' <summary>
    ''' Obtiene la posición de la lista donde el Id de la plaza coincida con el del vehículo seleccionado.
    ''' </summary>
    ''' <param name="arrayPlazasLibres">El array de las plazas libres.</param>
    ''' <returns>La posición del array.</returns>
    Private Function ObtenerPosicionPlaza(ByRef arrayPlazasLibres As Plaza()) As Integer

        For i As Integer = 0 To arrayPlazasLibres.Count - 1 Step 1

            Dim plaza As Plaza = arrayPlazasLibres(i)

            If plaza.Id = VehiculoSelec.IdPlaza Then

                Return i

            End If

        Next

        Return -1

    End Function

    Private Sub GuardarClienteBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            Dim clienteSelec As Cliente = CType(ClientesCmb.SelectedItem, Cliente)              ' Obtenemos los datos del cliente seleccionado en su ComboBox.
            Dim plazaSelec As Plaza = CType(PlazasCmb.SelectedItem, Plaza)              ' Obtenemos los datos de la plaza seleccionada en su ComboBox.            

            Dim cliente As New Cliente(clienteSelec.Id)         ' Creamos el cliente.

            If Accion = Foo.Accion.Insertar Then

                Dim vehiculo As New Vehiculo(MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, cliente, IdGaraje, plazaSelec.Id, PrecioBase)

                If vehiculo.Insertar() Then

                    If Plaza.CambiarSituacionPlazaAOcupada(plazaSelec.Id, IdGaraje) Then

                        Garaje.SumarNumPlazasOcupadasPorId(IdGaraje)
                        Garaje.RestarNumPlazasLibresPorId(IdGaraje)

                        MessageBox.Show("Se ha añadido el vehículo.", "Vehículo Añadido", MessageBoxButton.OK, MessageBoxImage.Information)
                        PlazasCmb.DataContext = Plaza.ObtenerIdPlazasLibresPorIdGaraje(IdGaraje)          ' Cargamos los Ids de las plazas libres en su ComboBox.

                        PgVehiculos.VehiculosDg.DataContext = Vehiculo.ObtenerVehiculosPorIdGaraje(IdGaraje)               ' Actualizamos el DataGrid de vehículos.
                        LimpiarCampos()
                    Else

                        MessageBox.Show("Ha habido un problema al cambiar la situación de la plaza a Ocupado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                    End If
                Else

                    MessageBox.Show("Ha habido un problema al insertar el vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            ElseIf Accion = Foo.Accion.Modificar Then

                Dim vehiculo As New Vehiculo(VehiculoSelec.Id, MatrVehiculoTxt.Text, MarcaVehiculoTxt.Text, ModVehiculoTxt.Text, cliente, PrecioBase)

                If vehiculo.Modificar() Then

                    MessageBox.Show("Se ha modificado el vehículo.", "Vehículo Modificado", MessageBoxButton.OK, MessageBoxImage.Information)

                    PgVehiculos.VehiculosDg.DataContext = Vehiculo.ObtenerVehiculosPorIdGaraje(VehiculoSelec.IdGaraje)
                Else

                    MessageBox.Show("Ha habido un problema al modificar los datos del vehículo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

                End If

            End If

            If PgVehiculos.VehiculosDg.DataContext Is Nothing Then

                MessageBox.Show("Ha habido un problema al obtener los vehículos del garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

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

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos por el usuario son correctos.
    ''' </summary>
    ''' <returns>True: Los datos son correctos. False: Los datos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayMatricula, hayMarca, hayModelo, hayPrecioBase, hayCliente As Boolean
        Dim exprMatricula As New Regex("^([A-Z]{1,2})?\s?-?\d{4}\s?-?([A-Z]{2,3})$")

        If Not Foo.HayTexto(MatrVehiculoTxt.Text) Then

            MessageBox.Show("Tienes que introducir una matrícula.", "Matrícula Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        ElseIf exprMatricula.IsMatch(MatrVehiculoTxt.Text) Then

            hayMatricula = True
        Else

            MessageBox.Show("El formato de la matrícula no es correcto.", "Formato Matrícula Incorrecto", MessageBoxButton.OK, MessageBoxImage.Error)

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

        If Foo.HayTexto(ClientesCmb.Text) Then

            hayCliente = True
        Else

            MessageBox.Show("Tienes que seleccionar un cliente.", "Cliente Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

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

        Return hayMatricula And hayMarca And hayModelo And hayPrecioBase And hayCliente

    End Function

    Public Sub New(ByRef accion As Foo.Accion, ByRef idGaraje As Integer, ByRef pgVehiculos As PgVehiculos)

        InitializeComponent()

        Me.Accion = accion
        Me.IdGaraje = idGaraje
        Me.PgVehiculos = pgVehiculos

    End Sub

    Public Sub New(ByRef accion As Foo.Accion, ByRef vehiculoSelec As Vehiculo, ByRef pgVehiculos As PgVehiculos, ByRef idGaraje As Integer)

        InitializeComponent()

        Me.Accion = accion
        Me.VehiculoSelec = vehiculoSelec
        Me.PgVehiculos = pgVehiculos
        Me.IdGaraje = idGaraje

    End Sub

End Class
