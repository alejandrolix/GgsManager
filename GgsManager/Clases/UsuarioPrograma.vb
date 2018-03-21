Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient

''' <summary>
''' Representa un usuario del programa de la tabla "UsuariosPrograma".
''' </summary>
Public Class UsuarioPrograma

    Property Id As Integer
    Property Nombre As String
    Property EsGestorB As Boolean

    ''' <summary>
    ''' Indica si el usuario es gestor mediante las palabras "Sí" y "No", para mostrarlo en el DataGrid.
    ''' </summary>    
    Property EsGestor As String

    ''' <summary>
    ''' Almacena el usuario que ha iniciado sesión en el programa.
    ''' </summary>    
    Public Shared Property UsuarioLogueado As UsuarioPrograma


    ''' <summary>
    ''' Comprueba si existe el usuario introducido.
    ''' </summary>
    ''' <param name="nombreUsuario">Nombre de usuario a comprobar.</param>
    ''' <returns>True: El usuario existe. False: El usuario no existe.</returns>
    Public Shared Function ExisteUsuario(ByRef nombreUsuario As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT COUNT(IdUsuario) 
                                         FROM   UsuariosPrograma 
                                         WHERE  Nombre = @Nombre;", conexion)

        comando.Parameters.AddWithValue("@Nombre", nombreUsuario)
        Dim resultado As Integer

        Try
            resultado = CType(comando.ExecuteScalar(), Integer)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al comprobar si existe el usuario introducido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return Not resultado = 0

    End Function


    ''' <summary>
    ''' Obtiene los datos del usuario que ha iniciado sesión.
    ''' </summary>
    ''' <param name="nombreUsuario">Nombre del usuario introducido.</param>
    ''' <returns>Datos del usuario iniciado.</returns>
    Public Shared Function ObtenerUsuarioPrograma(ByRef nombreUsuario As String) As UsuarioPrograma

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdUsuario, EsGestor 
                                         FROM   UsuariosPrograma 
                                         WHERE  Nombre = @Nombre;", conexion)

        comando.Parameters.AddWithValue("@Nombre", nombreUsuario)
        Dim datos As MySqlDataReader
        Dim usuarioPrograma As UsuarioPrograma

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el usuario del programa.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            If datos.Read() Then

                Dim idUsuario As Integer = datos.GetInt32("IdUsuario")
                Dim esGestor As Boolean = datos.GetBoolean("EsGestor")

                usuarioPrograma = New UsuarioPrograma(idUsuario, nombreUsuario, esGestor)

            End If

            datos.Close()

        End If

        conexion.Close()

        Return usuarioPrograma

    End Function


    ''' <summary>
    ''' Obtiene el hash a partir de una contraseña introducida por el usuario.
    ''' </summary>
    ''' <param name="password">La contraseña introducida por el usuario.</param>
    ''' <returns>El hash obtenido a partir de la contraseña.</returns>
    Public Shared Function ObtenerSHA1HashFromPassword(ByRef password As String) As String

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
    ''' Obtiene todos los usuarios del programa.
    ''' </summary>
    ''' <returns>Lista con los usuarios del programa.</returns>
    Public Shared Function ObtenerUsuariosPrograma() As List(Of UsuarioPrograma)

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdUsuario, Nombre, EsGestor
                                         FROM   UsuariosPrograma", conexion)
        Dim datos As MySqlDataReader

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener los usuarios del programa.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        If datos IsNot Nothing Then

            Dim listaUsuarios As New List(Of UsuarioPrograma)()

            While datos.Read()

                Dim id As Integer = datos.GetInt32("IdUsuario")
                Dim nombre As String = datos.GetString("Nombre")
                Dim esGestor As Boolean = datos.GetBoolean("EsGestor")

                Dim usuarioPrograma As New UsuarioPrograma(id, nombre, esGestor)
                listaUsuarios.Add(usuarioPrograma)

            End While

            datos.Close()
            conexion.Close()

            Return listaUsuarios

        End If

        conexion.Close()

        Return Nothing

    End Function


    ''' <summary>
    ''' Compueba si el hash de la contraseña introducida por el usuario es igual al de la base de datos.
    ''' </summary>    
    ''' <param name="passwordIntroducida">La contraseña que ha introducido el usuario.</param>
    ''' <returns>True: Los hashes son iguales. False: Los hashes no son iguales.</returns>
    Public Shared Function ComprobarHashPassword(ByRef passwordIntroducida As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim hashPasswordIntroducida As String = ObtenerSHA1HashFromPassword(passwordIntroducida)

        Dim comando As New MySqlCommand("SELECT Password 
                                         FROM   UsuariosPrograma 
                                         WHERE  Nombre = @Nombre;", conexion)

        comando.Parameters.AddWithValue("@Nombre", UsuarioLogueado.Nombre)
        Dim hashPasswordBd As String = ""

        Try
            hashPasswordBd = CType(comando.ExecuteScalar(), String)

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al obtener el hash de la contraseña del usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return hashPasswordIntroducida.Equals(hashPasswordBd)

    End Function


    ''' <summary>
    ''' Inserta un usuario.
    ''' </summary>
    ''' <param name="usuarioPrograma">Datos del usuario.</param>
    ''' <param name="hashPassword">El hash de la contraseña.</param>    
    ''' <returns>True: Se ha insertado el usuario. False: No se ha insertado el usuario.</returns>
    Public Shared Function InsertarUsuario(ByRef usuarioPrograma As UsuarioPrograma, ByRef hashPassword As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("INSERT INTO UsuariosPrograma (IdUsuario, Nombre, Password, EsGestor) VALUES (NULL, @Nombre, @Password, @EsGestorB);", conexion)

        comando.Parameters.AddWithValue("@Nombre", usuarioPrograma.Nombre)
        comando.Parameters.AddWithValue("@Password", hashPassword)
        comando.Parameters.AddWithValue("@EsGestorB", usuarioPrograma.EsGestorB)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al insertar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica los datos de un usuario.
    ''' </summary>
    ''' <param name="nombre">Nombre del usuario.</param>
    ''' <param name="esGestor">Es gestor el usuario.</param>
    ''' <param name="idUsuario">Id del usuario.</param>
    ''' <returns>True: Se ha modificado el usuario. False: No se ha modificado el usuario.</returns>
    Public Shared Function ModificarUsuarioPorId(ByRef nombre As String, ByRef esGestor As Boolean, ByRef idUsuario As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE UsuariosPrograma
                                         SET    Nombre = @Nombre, EsGestor = @EsGestor
                                         WHERE  IdUsuario = @IdUsuario;", conexion)

        comando.Parameters.AddWithValue("@Nombre", nombre)
        comando.Parameters.AddWithValue("@EsGestor", esGestor)
        comando.Parameters.AddWithValue("@IdUsuario", idUsuario)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Modifica el hash de la contraseña del usuario a partir de su Id.
    ''' </summary>
    ''' <param name="idUsuario">Id del usuario.</param>
    ''' <param name="hashPassword">Hash de la contraseña.</param>
    ''' <returns>True: Se ha modificado la contraseña del usuario. False: No se ha modificado la contraseña del usuario.</returns>
    Public Shared Function ModificarPasswordPorId(ByRef idUsuario As Integer, ByRef hashPassword As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE UsuariosPrograma
                                         SET    Password = @Password
                                         WHERE  IdUsuario = @IdUsuario;", conexion)

        comando.Parameters.AddWithValue("@Password", hashPassword)
        comando.Parameters.AddWithValue("@IdUsuario", idUsuario)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al modificar la contraseña del usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function


    ''' <summary>
    ''' Elimina un usuario a partir de su Id.
    ''' </summary>
    ''' <param name="idUsuario">El Id del usuario a eliminar.</param>
    ''' <returns>True: Se ha eliminado el usuario. False: No se ha eliminado el usuario.</returns>
    Public Shared Function EliminarUsuarioPorId(ByRef idUsuario As Integer) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("DELETE FROM UsuariosPrograma
                                         WHERE  IdUsuario = @IdUsuario;", conexion)

        comando.Parameters.AddWithValue("@IdUsuario", idUsuario)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

            MessageBox.Show("Ha habido un problema al eliminar el usuario seleccionado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function

    Public Sub New(id As Integer, nombre As String, esGestor As Boolean)                    ' Para mostrar los usuarios en el DataGrid.

        Me.Id = id
        Me.Nombre = nombre
        Me.EsGestorB = esGestor

        If esGestor Then

            Me.EsGestor = "Sí"
        Else

            Me.EsGestor = "No"

        End If

    End Sub

    Public Sub New(nombre As String, esGestor As Boolean)               ' Para crear un usuario.

        Me.Nombre = nombre
        Me.EsGestorB = esGestor

    End Sub

End Class
