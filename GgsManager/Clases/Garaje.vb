''' <summary>
''' Representa un garaje de la tabla "Garajes" de la base de datos.
''' </summary>
Public Class Garaje

    Property Id As Integer
    Property Nombre As String
    Property Direccion As String
    Property NumPlazas As Integer
    Property NumPlazasLibres As Integer
    Property NumPlazasOcupadas As Integer
    Property Observaciones As String

    Public Sub New(id As Integer, nombre As String, direccion As String, numPlazas As Integer, numPlazasLibres As Integer, numPlazasOcupadas As Integer, observaciones As String)

        Me.Id = id
        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.NumPlazasLibres = numPlazasLibres
        Me.NumPlazasOcupadas = numPlazasOcupadas
        Me.Observaciones = observaciones

    End Sub

    Public Sub New(nombre As String, direccion As String, numPlazas As Integer, observaciones As String)

        Me.Nombre = nombre
        Me.Direccion = direccion
        Me.NumPlazas = numPlazas
        Me.Observaciones = observaciones

    End Sub

End Class
