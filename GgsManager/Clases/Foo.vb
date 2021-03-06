﻿Imports MySql.Data.MySqlClient
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
    ''' Guarda el nuevo porcentaje del I.V.A.
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

            jWriter.Close()

            haGuardadoDatos = True

        Catch ex As Exception

            haGuardadoDatos = False

        End Try

        Return haGuardadoDatos

    End Function


    ''' <summary>
    ''' Lee el porcentaje del I.V.A.
    ''' </summary>
    ''' <returns>El porcentaje del I.V.A. almacenado.</returns>
    Public Shared Function LeerIVA() As Integer

        Dim sw As StreamReader = File.OpenText(My.Settings.RutaArchivos & "Iva.json")
        Dim jReader As New JsonTextReader(sw)

        Dim iva As JObject = CType(JToken.ReadFrom(jReader), JObject)
        Dim numero As Integer = Integer.Parse(iva.Item("Porcentaje").ToString())

        jReader.Close()

        Return numero

    End Function


    ''' <summary>
    ''' Importa el fichero sql a la base de datos.
    ''' </summary>
    ''' <returns>True: Los datos del fichero se ha importado a la base de datos. False: Los datos del fichero no se ha importado a la base de datos.</returns>
    Public Shared Function ImportarBd() As Boolean

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand()
        Dim importar As New MySqlBackup(comando)
        Dim haImportadoBd As Boolean

        comando.Connection = conexion

        Try
            importar.ImportFromFile(My.Settings.RutaBd & "Bd.sql")
            haImportadoBd = True

        Catch ex As Exception

            haImportadoBd = False

        End Try

        conexion.Close()

        Return haImportadoBd

    End Function


    ''' <summary>
    ''' Exporta el fichero sql de la base de datos.
    ''' </summary>
    ''' <returns>True: La base de datos se ha exportado al fichero. False: La base de datos no se ha exportado al fichero.</returns>
    Public Shared Function ExportarBd() As Boolean

        If Not Directory.Exists(My.Settings.RutaBd) Then

            Directory.CreateDirectory(My.Settings.RutaBd)

        End If

        Dim conexion As MySqlConnection = Foo.ConexionABd()
        Dim comando As New MySqlCommand()
        Dim exportar As New MySqlBackup(comando)
        Dim haExportadoBd As Boolean

        comando.Connection = conexion

        Try
            exportar.ExportToFile(My.Settings.RutaBd & "Bd.sql")
            haExportadoBd = True

        Catch ex As Exception

            haExportadoBd = False

        End Try

        conexion.Close()

        Return haExportadoBd

    End Function


    ''' <summary>
    ''' Guarda los principales datos de la empresa.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GuardarDatosEmpresa() As Boolean

        Dim datos As New JObject()
        Dim haGuardadoDatos As Boolean

        Try
            datos.Add("CIF", "B38201987")
            datos.Add("Direccion", "Calle Marqués de Fontalba, 124")
            datos.Add("Telefono", "324 54 97 90")
            datos.Add("Localidad", "Alicante")
            datos.Add("CodigoPostal", "03012")

            Dim sw As StreamWriter = File.CreateText(My.Settings.RutaArchivos & "DatosEmpresa.json")
            Dim jWriter As New JsonTextWriter(sw)

            jWriter.Formatting = Formatting.Indented
            datos.WriteTo(jWriter)

            jWriter.Close()

            haGuardadoDatos = True

        Catch ex As Exception

            haGuardadoDatos = False

        End Try

        Return haGuardadoDatos

    End Function


    ''' <summary>
    ''' Lee los principales datos de la empresa.
    ''' </summary>
    ''' <returns>Lista con los datos de la empresa.</returns>
    Public Shared Function LeerDatosEmpresa() As String()

        Dim sw As StreamReader = File.OpenText(My.Settings.RutaArchivos & "DatosEmpresa.json")
        Dim jReader As New JsonTextReader(sw)

        Dim datos As JObject = CType(JToken.ReadFrom(jReader), JObject)
        Dim listaDatos As New List(Of String)()

        listaDatos.Add(datos.Item("CIF").ToString())
        listaDatos.Add(datos.Item("Direccion").ToString())
        listaDatos.Add(datos.Item("Telefono").ToString())
        listaDatos.Add(datos.Item("Localidad").ToString())
        listaDatos.Add(datos.Item("CodigoPostal").ToString())

        jReader.Close()

        Return listaDatos.ToArray()

    End Function


    ''' <summary>
    ''' Indica la acción que se va a realizar en un mismo formulario. Como añadir o modificar datos.
    ''' </summary>
    Enum Accion

        ''' <summary>
        ''' Se van a Insertar Datos en un Formulario.
        ''' </summary>
        Insertar

        ''' <summary>
        ''' Se van a Modificar Datos en un Formulario.
        ''' </summary>
        Modificar

    End Enum


    ''' <summary>
    ''' Indica qué ventana se va a abrir despúes de seleccionar un garaje en "VntSeleccGaraje".
    ''' </summary>
    Enum Ventana

        ''' <summary>
        ''' Abre la Ventana de Vehículos.
        ''' </summary>
        Vehiculos

        ''' <summary>
        ''' Abre la Ventana de Plazas.
        ''' </summary>
        Plazas

        ''' <summary>
        ''' Abre la Ventana del Informe de Clientes.
        ''' </summary>
        InformeClientes

        ''' <summary>
        ''' Abre la Ventana del Informe de Plazas.
        ''' </summary>
        InformePlazas

        ''' <summary>
        ''' Abre la Ventana del Informe de Estadísticas de Uno o Todos los Garajes.
        ''' </summary>
        InformeEstadGaraje

        ''' <summary>
        ''' Abre la Ventana de la Factura Individual, (a un cliente).
        ''' </summary>
        FacturaIndividual

        ''' <summary>
        ''' Abre la Ventana de la Factura de los Clientes de un Garaje.
        ''' </summary>
        FacturaGaraje
    End Enum

End Class
