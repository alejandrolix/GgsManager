Imports MySql.Data.MySqlClient
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

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
    ''' Comprueba si hay una imagen en un Image.
    ''' </summary>
    ''' <param name="img">Imagen a comprobar.</param>
    ''' <returns>True: Hay imagen. False: No hay imagen.</returns>
    Public Shared Function HayImagen(ByRef img As ImageSource) As Boolean

        Return img IsNot Nothing

    End Function


    ''' <summary>
    ''' Comprueba si la dirección contiene una "\". Si es así, devolverá "\\" para insertarlo en la tabla.
    ''' </summary>
    ''' <param name="direccion">Dirección introducida a comprobar.</param>
    ''' <returns>La nueva dirección o la misma.</returns>
    Public Shared Function ComprobarDireccion(ByRef direccion As String) As String

        If direccion.Contains("\") Then

            Dim nuevaDireccion As String = direccion.Replace("\", "\\")
        Else

            Return direccion

        End If

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
    ''' Guarda el nuevo I.V.A.
    ''' </summary>
    ''' <param name="porcentajeIva">El I.V.A. a guardar.</param>
    ''' <returns>True: Se ha guardado el I.V.A. False: No se ha guardado el I.V.A.</returns>
    Public Shared Function GuardarNuevoIVA(ByRef porcentajeIva As Integer) As Boolean

        Dim iva As New JObject()
        Dim haGuardadoDatos As Boolean

        Try
            iva.Add("Porcentaje", porcentajeIva)

            Dim sw As StreamWriter = File.CreateText(My.Settings.RutaArchivos & "Iva.json")
            Dim jWriter As New JsonTextWriter(sw)

            jWriter.Formatting = Formatting.Indented
            iva.WriteTo(jWriter)

            sw.Close()
            jWriter.Close()

            haGuardadoDatos = True

        Catch ex As Exception

            haGuardadoDatos = False

        End Try

        Return haGuardadoDatos

    End Function


    ''' <summary>
    ''' Lee el I.V.A.
    ''' </summary>
    ''' <returns>El porcentaje del I.V.A. almacenado.</returns>
    Public Shared Function LeerIVA() As Integer

        Dim sw As StreamReader = File.OpenText(My.Settings.RutaArchivos & "Iva.json")
        Dim jReader As New JsonTextReader(sw)

        Dim iva As JObject = CType(JToken.ReadFrom(jReader), JObject)
        Dim numero As Integer = Integer.Parse(iva.Item("Porcentaje").ToString())

        Return numero

    End Function


    ''' <summary>
    ''' Indica la acción que se va a realizar en un mismo formulario. Como añadir o modificar datos.
    ''' </summary>
    Enum Accion

        Insertar
        Modificar

    End Enum


    ''' <summary>
    ''' Indica qué ventana se va a abrir despúes de seleccionar un garaje de "VntSeleccGaraje".
    ''' </summary>
    Enum Ventana
        Vehiculos
        Plazas
    End Enum

End Class
