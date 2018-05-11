Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient
Imports NPoco

''' <summary>
''' Representa un usuario de la tabla "Usuarios".
''' </summary>
Public Class Usuario

    ''' <summary>
    ''' El Id del Usuario.
    ''' </summary>    
    <Column("IdUsuario")>
    Property Id As Integer

    ''' <summary>
    ''' El Nombre del Usuario.
    ''' </summary>    
    Property Nombre As String

    ''' <summary>
    ''' Indica si el Usuario es Gestor.
    ''' </summary>    
    Property EsGestor As Boolean

    ''' <summary>
    ''' Almacena el usuario que ha iniciado sesión en el programa.
    ''' </summary>    
    Public Shared Property UsuarioLogueado As Usuario


    ''' <summary>
    ''' Comprueba si existe el usuario introducido.
    ''' </summary>
    ''' <param name="nombreUsuario">Nombre de usuario a comprobar.</param>
    ''' <returns>True: El usuario existe. False: El usuario no existe.</returns>
    Public Shared Function ExisteUsuario(ByRef nombreUsuario As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT ExisteUsuario(@NombreUsuario);", conexion)

        comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario)
        Dim existe As Boolean

        Try
            existe = CType(comando.ExecuteScalar(), Boolean)

        Catch ex As Exception

        End Try

        conexion.Close()

        Return existe

    End Function


    ''' <summary>
    ''' Obtiene los datos del usuario que ha iniciado sesión.
    ''' </summary>
    ''' <param name="nombreUsuario">Nombre del usuario introducido.</param>
    ''' <returns>Datos del usuario iniciado.</returns>
    Public Shared Function ObtenerUsuario(ByRef nombreUsuario As String) As Usuario

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT IdUsuario, EsGestor 
                                         FROM   Usuarios 
                                         WHERE  Nombre = @Nombre;", conexion)

        comando.Parameters.AddWithValue("@Nombre", nombreUsuario)
        Dim datos As MySqlDataReader = Nothing
        Dim usuario As Usuario = Nothing

        Try
            datos = comando.ExecuteReader()

        Catch ex As Exception

        End Try

        If datos IsNot Nothing Then

            If datos.Read() Then

                Dim idUsuario As Integer = datos.GetInt32("IdUsuario")
                Dim esGestor As Boolean = datos.GetBoolean("EsGestor")

                usuario = New Usuario(idUsuario, nombreUsuario, esGestor)

            End If

            datos.Close()

        End If

        conexion.Close()
        Return usuario

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
    ''' Obtiene todos los usuarios.
    ''' </summary>
    ''' <returns>Array con los usuarios.</returns>
    Public Shared Function ObtenerUsuarios() As Usuario()

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")

        Dim arrayUsuarios As Usuario() = conexion.Query(Of Usuario)("SELECT IdUsuario, Nombre, EsGestor
                                                                     FROM   Usuarios;").ToArray()
        conexion.CloseSharedConnection()

        Return arrayUsuarios

    End Function


    ''' <summary>
    ''' Obtiene el hash de la contraseña del usuario en la base de datos.
    ''' </summary>    
    ''' <param name="nombreUsuario">El nombre del usuario introducido.</param>    
    ''' <returns>La contraseña del usuario.</returns>
    Public Shared Function ObtenerPasswordUsuario(ByRef nombreUsuario As String) As String

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT ObtenerPasswordUsuario(@NombreUsuario);", conexion)

        comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario)
        Dim hashPasswordUsuario As String = ""

        Try
            hashPasswordUsuario = CType(comando.ExecuteScalar(), String)

        Catch ex As Exception

        End Try

        conexion.Close()

        Return hashPasswordUsuario

    End Function


    ''' <summary>
    ''' Comprueba si la contraseña introducida por el usuario es igual al de la base de datos.
    ''' </summary>            
    ''' <param name="passwordIntroducida">La contraseña introducida por el usuario.</param>
    ''' <param name="passwordBd">La contraseña del usuario de la base de datos.</param>    
    ''' <returns>True: Las contraseñas son iguales. False: Las contraseñas no son iguales.</returns>
    Public Shared Function ComprobarPasswords(ByRef passwordIntroducida As String, ByRef passwordBd As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("SELECT ComprobarPasswords(@PasswordIntroducida, @PasswordBd);", conexion)

        comando.Parameters.AddWithValue("@PasswordIntroducida", passwordIntroducida)
        comando.Parameters.AddWithValue("@PasswordBd", passwordBd)
        Dim sonIguales As Boolean

        Try
            sonIguales = CType(comando.ExecuteScalar(), Boolean)

        Catch ex As Exception

        End Try

        conexion.Close()

        Return sonIguales

    End Function


    ''' <summary>
    ''' Inserta un usuario.
    ''' </summary>    
    ''' <param name="hashPassword">El hash de la contraseña.</param>    
    ''' <returns>True: Se ha insertado el usuario. False: No se ha insertado el usuario.</returns>
    Public Function Insertar(ByRef hashPassword As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()

        If ExisteUsuario(Nombre) Then

            MessageBox.Show("El usuario introducido ya existe, introduzca otro usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            Dim comando As New MySqlCommand("INSERT INTO Usuarios (IdUsuario, Nombre, Password, EsGestor) VALUES (NULL, @Nombre, @Password, @EsGestorB);", conexion)

            comando.Parameters.AddWithValue("@Nombre", Nombre)
            comando.Parameters.AddWithValue("@Password", hashPassword)
            comando.Parameters.AddWithValue("@EsGestorB", EsGestor)
            Dim numFila As Integer

            Try
                numFila = comando.ExecuteNonQuery()

            Catch ex As Exception

            End Try

            conexion.Close()

            Return numFila >= 1

        End If

        Return False

    End Function


    ''' <summary>
    ''' Modifica los datos de un usuario.
    ''' </summary>        
    ''' <param name="usuario">El usuario a modificar.</param>
    ''' <returns>True: Se ha modificado el usuario. False: No se ha modificado el usuario.</returns>
    Public Shared Function Modificar(ByRef usuario As Usuario) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim actualizacion As Integer

        Try
            actualizacion = conexion.Update("Usuarios", "IdUsuario", usuario)

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return actualizacion >= 1

    End Function


    ''' <summary>
    ''' Elimina un usuario.
    ''' </summary>    
    ''' <param name="usuario">El usuario a eliminar.</param>
    ''' <returns>True: Se ha eliminado el usuario. False: No se ha eliminado el usuario.</returns>
    Public Shared Function Eliminar(ByRef usuario As Usuario) As Boolean

        Dim conexion As New Database(My.Settings.ConexionABd, "MySql.Data.MySqlClient")
        Dim eliminacion As Integer

        Try
            eliminacion = conexion.Delete("Usuarios", "IdUsuario", usuario)

        Catch ex As Exception

        End Try

        conexion.CloseSharedConnection()

        Return eliminacion >= 1

    End Function


    ''' <summary>
    ''' Modifica el hash de la contraseña del usuario a partir de su Id.
    ''' </summary>
    ''' <param name="idUsuario">Id del usuario.</param>
    ''' <param name="hashPassword">Hash de la contraseña.</param>
    ''' <returns>True: Se ha modificado la contraseña del usuario. False: No se ha modificado la contraseña del usuario.</returns>
    Public Shared Function ModificarPasswordPorId(ByRef idUsuario As Integer, ByRef hashPassword As String) As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand("UPDATE Usuarios
                                         SET    Password = @Password
                                         WHERE  IdUsuario = @IdUsuario;", conexion)

        comando.Parameters.AddWithValue("@Password", hashPassword)
        comando.Parameters.AddWithValue("@IdUsuario", idUsuario)
        Dim numFila As Integer

        Try
            numFila = comando.ExecuteNonQuery()

        Catch ex As Exception

        End Try

        conexion.Close()

        Return numFila >= 1

    End Function

    Public Sub New(id As Integer, nombre As String, esGestor As Boolean)                    ' Para mostrar los usuarios en el DataGrid.

        Me.Id = id
        Me.Nombre = nombre
        Me.EsGestor = esGestor

    End Sub

    Public Sub New(nombre As String, esGestor As Boolean)               ' Para crear un usuario.

        Me.Nombre = nombre
        Me.EsGestor = esGestor

    End Sub

    Public Sub New()

    End Sub

End Class
