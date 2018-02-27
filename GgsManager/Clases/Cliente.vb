''' <summary>
''' Representa un cliente de la tabla "Clientes" de la base de datos.
''' </summary>
Public Class Cliente

    Property Id As Integer
    Property Nombre As String
    Property Apellidos As String
    Property DNI As String
    Property Direccion As String
    Property Poblacion As String
    Property Provincia As String
    Property Movil As String
    Property FechaAlta As Date
    Property Observaciones As String
    Property UrlFoto As String

    Public Sub New(nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, observaciones As String, urlFoto As String)

        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.Observaciones = observaciones
        Me.UrlFoto = urlFoto

    End Sub

    Public Sub New(id As Integer, nombre As String, apellidos As String, dni As String, direccion As String, poblacion As String, provincia As String, movil As String, fechaAlta As Date, observaciones As String, urlFoto As String)

        Me.Id = id
        Me.Nombre = nombre
        Me.Apellidos = apellidos
        Me.DNI = dni
        Me.Direccion = direccion
        Me.Poblacion = poblacion
        Me.Provincia = provincia
        Me.Movil = movil
        Me.FechaAlta = fechaAlta
        Me.Observaciones = observaciones
        Me.UrlFoto = urlFoto

    End Sub

    Public Sub New(nombre As String, apellidos As String)

        Me.Nombre = nombre
        Me.Apellidos = apellidos

    End Sub

End Class
