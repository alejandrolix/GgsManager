Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient

''' <summary>
''' Clase que contiene métodos que acceden a la base de datos.
''' </summary>
Public Class GestionarBd

    Public Shared UsuarioIniciado As String

    ''' <summary>
    ''' Devuelve una conexión a la base de datos.
    ''' </summary>
    ''' <returns>Conexión a la base de datos.</returns>
    Private Shared Function ConexionABd() As MySqlConnection

        Dim conexion As New MySqlConnection(My.Settings.ConexionABd)
        conexion.Open()

        Return conexion

    End Function


    ''' <summary>
    ''' Comprueba si existe el usuario introducido en la Bd.
    ''' </summary>
    ''' <param name="usuario">Nombre de usuario a comprobar en la Bd.</param>
    ''' <returns>True: El usuario existe. False: El usuario no existe.</returns>
    Public Shared Function ExisteUsuario(ByRef usuario As String) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim comando As New MySqlCommand(String.Format("SELECT COUNT(IdUsuario) 
                                                       FROM   UsuariosPrograma 
                                                       WHERE  Nombre = '{0}';", usuario), conexion)

        Dim resultado As Integer = CType(comando.ExecuteScalar(), Integer)
        conexion.Close()

        Return Not resultado = 0

    End Function


    ''' <summary>
    ''' Compueba si el hash de la contraseña introducida por el usuario es igual al de la base de datos.
    ''' </summary>
    ''' <param name="usuario">El nombre de usuario que ha introducido el usuario.</param>
    ''' <param name="passwordIntroducida">La contraseña que ha introducido el usuario.</param>
    ''' <returns>True: Los hashes son iguales. False: Los hashes no son iguales.</returns>
    Public Shared Function ComprobarHashPassword(ByRef usuario As String, ByRef passwordIntroducida As String) As Boolean

        Dim conexion As MySqlConnection = ConexionABd()
        Dim hashPasswordIntroducida As String = ObtenerSHA1HashFromPassword(passwordIntroducida)

        Dim comando As New MySqlCommand(String.Format("SELECT Password 
                                                       FROM   UsuariosPrograma 
                                                       WHERE  Nombre = '{0}';", usuario), conexion)

        Dim hashPasswordBd As String = CType(comando.ExecuteScalar(), String)
        conexion.Close()

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

End Class
