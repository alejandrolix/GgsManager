Imports System.Collections.ObjectModel
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient

''' <summary>
''' Contiene métodos que maneja la base de datos.
''' </summary>
Public Class GestionBd

    Public Shared Property UsuarioPrograma As UsuarioPrograma        ' Almacena el usuario que ha iniciado sesión en el programa.

    ''' <summary>
    ''' Realiza una conexión a la base de datos.
    ''' </summary>
    ''' <returns>Conexión a la base de datos.</returns>
    Private Shared Function ConexionABd() As MySqlConnection

        Dim conexion As New MySqlConnection(My.Settings.ConexionABd)

        Try
            conexion.Open()
            Return conexion

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al conectarse con la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return Nothing

    End Function


    ''' <summary>
    ''' Comprueba si existe el usuario introducido.
    ''' </summary>
    ''' <param name="usuario">Nombre de usuario a comprobar.</param>
    ''' <returns>True: El usuario existe. False: El usuario no existe.</returns>
    Public Shared Function ExisteUsuario(ByRef usuario As String) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT COUNT(IdUsuario) 
                                                       FROM   UsuariosPrograma 
                                                       WHERE  Nombre = '{0}';", usuario), conexion)
        Dim resultado As Integer

        Try
            resultado = CType(comando.ExecuteScalar(), Integer)
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al comprobar si existe el usuario introducido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return Not resultado = 0

    End Function


    ''' <summary>
    ''' Obtiene los datos del usuario que ha iniciado sesión.
    ''' </summary>
    ''' <param name="nombreUsuario">Nombre del usuario introducido.</param>
    ''' <returns>Datos del usuario iniciado.</returns>
    Public Shared Function ObtenerUsuarioPrograma(ByRef nombreUsuario As String) As UsuarioPrograma

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT IdUsuario, EsGestor 
                                                       FROM   UsuariosPrograma 
                                                       WHERE  Nombre = '{0}';", nombreUsuario), conexion)
        Dim datos As MySqlDataReader
        Dim usuarioPrograma As UsuarioPrograma

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el usuario del programa.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos.HasRows Then

            If datos.Read() Then

                Dim idUsuario As Integer = datos.GetInt64("IdUsuario")
                Dim esGestor As Boolean = datos.GetBoolean("EsGestor")

                usuarioPrograma = New UsuarioPrograma(idUsuario, nombreUsuario, esGestor)

            End If

        End If

        Return usuarioPrograma

    End Function


    ''' <summary>
    ''' Compueba si el hash de la contraseña introducida por el usuario es igual al de la base de datos.
    ''' </summary>    
    ''' <param name="passwordIntroducida">La contraseña que ha introducido el usuario.</param>
    ''' <returns>True: Los hashes son iguales. False: Los hashes no son iguales.</returns>
    Public Shared Function ComprobarHashPassword(ByRef passwordIntroducida As String) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim hashPasswordIntroducida As String = ObtenerSHA1HashFromPassword(passwordIntroducida)

        Dim comando As New MySqlCommand(String.Format("SELECT Password 
                                                       FROM   UsuariosPrograma 
                                                       WHERE  Nombre = '{0}';", UsuarioPrograma.Nombre), conexion)
        Dim hashPasswordBd As String = ""

        Try
            hashPasswordBd = CType(comando.ExecuteScalar(), String)
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el hash de la contraseña del usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return hashPasswordIntroducida.Equals(hashPasswordBd)

    End Function


    ''' <summary>
    ''' Obtiene el hash a partir de la contraseña introducida por el usuario.
    ''' </summary>
    ''' <param name="password">La contraseña introducida por el usuario.</param>
    ''' <returns>El hash obtenido a partir de la contraseña.</returns>
    Private Shared Function ObtenerSHA1HashFromPassword(ByRef password As String) As String

        Dim sha1 As New SHA1CryptoServiceProvider()
        Dim arrayPassword() As Byte = Encoding.ASCII.GetBytes(password)

        arrayPassword = sha1.ComputeHash(arrayPassword)
        Dim cadena As New StringBuilder()

        For Each num As Byte In arrayPassword

            cadena.Append(num.ToString("x2"))

        Next

        Return cadena.ToString()

    End Function


    ''' <summary>
    ''' Obtiene todos los garajes.
    ''' </summary>
    ''' <returns>Lista con todos los garajes.</returns>
    Public Shared Function ObtenerGarajes() As List(Of Garaje)

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdGaraje, Nombre, Direccion, NumPlazas, Observaciones
                                         FROM   Garajes", conexion)

        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaGarajes As List(Of Garaje) = Nothing

            If datos.HasRows Then

                listaGarajes = New List(Of Garaje)()

                While datos.Read()

                    Dim id As Integer = datos.GetInt32("IdGaraje")
                    Dim nombre As String = datos.GetString("Nombre")
                    Dim direccion As String = datos.GetString("Direccion")
                    Dim numPlazas As Integer = datos.GetInt64("NumPlazas")
                    Dim observaciones As String

                    If datos.IsDBNull(4) Then               ' Si el contenido de la 2ª columna, (Observaciones), es NULL.

                        observaciones = ""
                    Else

                        observaciones = datos.GetString("Observaciones")

                    End If

                    listaGarajes.Add(New Garaje(id, nombre, direccion, numPlazas, observaciones))

                End While

                datos.Close()
                conexion.Close()

            End If

            Return listaGarajes

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Elimina un garaje a partir de su Id.
    ''' </summary>
    ''' <param name="idGaraje">El Id del garaje a eliminar.</param>
    ''' <returns>True: El garaje se ha eliminado. False: El garaje no se ha eliminado.</returns>
    Public Shared Function EliminarGarajePorId(ByRef idGaraje As Integer) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("DELETE FROM Garajes WHERE IdGaraje = {0}", idGaraje), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Inserta un garaje con observaciones.
    ''' </summary>
    ''' <param name="garaje">Datos del garaje a insertar.</param>
    ''' <returns>True: El garaje se ha insertado. False: El garaje no se ha insertado.</returns>
    Public Shared Function InsertarGarajeConObservaciones(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()

        Dim comando As New MySqlCommand(String.Format("INSERT INTO Garajes (IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones) VALUES (NULL, '{0}', '{1}', {2}, 0, 0, '{3}');", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.Observaciones), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila <> 0

    End Function


    ''' <summary>
    ''' Inserta un garaje sin observaciones.
    ''' </summary>
    ''' <param name="garaje">Datos del garaje a insertar.</param>
    ''' <returns>True: El garaje se ha insertado. False: El garaje no se ha insertado.</returns>
    Public Shared Function InsertarGarajeSinObservaciones(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()

        Dim comando As New MySqlCommand(String.Format("INSERT INTO Garajes (IdGaraje, Nombre, Direccion, NumPlazas, NumPlazasLibres, NumPlazasOcupadas, Observaciones) VALUES (NULL, '{0}', '{1}', {2}, 0, 0, NULL);", garaje.Nombre, garaje.Direccion, garaje.NumPlazas), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila <> 0

    End Function


    ''' <summary>
    ''' Modifica los datos de un garaje con observaciones.
    ''' </summary>
    ''' <param name="garaje">Datos del garaje a modificar.</param>
    ''' <returns>True: Se han modificado los datos del garaje. False: No se han modificado los datos del garaje.</returns>
    Public Shared Function ModificarGarajeConObservaciones(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("UPDATE Garajes SET Nombre = '{0}', Direccion = '{1}', NumPlazas = {2}, Observaciones = '{3}' 
                                                       WHERE  IdGaraje = {4};", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.Observaciones, garaje.Id), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un garaje sin observaciones.
    ''' </summary>
    ''' <param name="garaje">Datos del garaje a modificar.</param>
    ''' <returns>True: Se han modificado los datos del garaje. False: No se han modificado los datos del garaje.</returns>
    Public Shared Function ModificarGarajesSinObservaciones(ByRef garaje As Garaje) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("UPDATE Garajes SET Nombre = '{0}', Direccion = '{1}', NumPlazas = {2}, Observaciones = NULL 
                                                       WHERE  IdGaraje = {4};", garaje.Nombre, garaje.Direccion, garaje.NumPlazas, garaje.Id), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar el garaje.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Obtiene todos los clientes.
    ''' </summary>
    ''' <returns>ObservableCollection de todos los clientes.</returns>
    Public Shared Function ObtenerClientes() As ObservableCollection(Of Cliente)

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, Observaciones
                                         FROM   Clientes", conexion)

        Dim datos As MySqlDataReader = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los garajes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaClientes As New ObservableCollection(Of Cliente)()

            While datos.Read()

                Dim id As Integer = datos.GetInt64("IdCliente")
                Dim nombre As String = datos.GetString("Nombre")
                Dim apellidos As String = datos.GetString("Apellidos")
                Dim dni As String = datos.GetString("DNI")
                Dim direccion As String = datos.GetString("Direccion")
                Dim poblacion As String = datos.GetString("Poblacion")
                Dim provincia As String = datos.GetString("Provincia")
                Dim movil As String = datos.GetString("Movil")
                Dim fechaAlta As Date = datos.GetDateTime("FechaAlta")

                Dim observaciones As String

                If datos.IsDBNull(9) Then

                    observaciones = ""
                Else

                    observaciones = datos.GetString("Observaciones")

                End If

                listaClientes.Add(New Cliente(id, nombre, apellidos, dni, direccion, poblacion, provincia, movil, fechaAlta, observaciones))

            End While

            datos.Close()
            conexion.Close()

            Return listaClientes

        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Inserta un cliente con observaciones.
    ''' </summary>
    ''' <param name="cliente">Datos del cliente a insertar.</param>
    ''' <returns>True: El cliente se ha insertado. False: El cliente no se ha insertado.</returns>
    Public Shared Function InsertarClienteConObservaciones(ByRef cliente As Cliente) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, Observaciones, URLFoto)
                                         VALUES (NULL, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', NOW(), '{7}', NULL);", cliente.Nombre, cliente.Apellidos, cliente.DNI, cliente.Direccion, cliente.Poblacion, cliente.Provincia, cliente.Movil, cliente.Observaciones), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Inserta un cliente sin observaciones.
    ''' </summary>
    ''' <param name="cliente">Datos del cliente a insertar.</param>
    ''' <returns>True: El cliente se ha insertado. False: El cliente no se ha insertado.</returns>
    Public Shared Function InsertarClienteSinObservaciones(ByRef cliente As Cliente) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("INSERT INTO Clientes (IdCliente, Nombre, Apellidos, DNI, Direccion, Poblacion, Provincia, Movil, FechaAlta, Observaciones, URLFoto)
                                         VALUES (NULL, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', NOW(), NULL, NULL);", cliente.Nombre, cliente.Apellidos, cliente.DNI, cliente.Direccion, cliente.Poblacion, cliente.Provincia, cliente.Movil), conexion)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()
            conexion.Close()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al añadir el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        Return numFila >= 1

    End Function

End Class
