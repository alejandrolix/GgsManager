Imports System.Collections.ObjectModel
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient

''' <summary>
''' Contiene métodos que se van a usar en varios sitios.
''' </summary>
Public Class Foo

    ''' <summary>
    ''' Comprueba si hay texto.
    ''' </summary>
    ''' <param name="texto">El texto a comprobar.</param>
    ''' <returns>True: Hay texto. False: No hay texto.</returns>
    Public Shared Function HayTexto(ByRef texto As String) As Boolean

        Return texto.Length >= 1

    End Function


    ''' <summary>
    ''' Comprueba si hay una imagen en el Image.
    ''' </summary>
    ''' <param name="img">Imagen a comprobar.</param>
    ''' <returns>True: Hay imagen. False: No hay imagen.</returns>
    Public Shared Function HayImagen(ByRef img As ImageSource) As Boolean

        Return img IsNot Nothing

    End Function


    ''' <summary>
    ''' Realiza una conexión a la base de datos.
    ''' </summary>
    ''' <returns>Conexión a la base de datos.</returns>
    Public Shared Function ConexionABd() As MySqlConnection

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
    ''' Indica la acción que se va a realizar en un mismo formulario. Como añadir o modificar datos.
    ''' </summary>
    Enum Accion

        Insertar
        Modificar

    End Enum

End Class
