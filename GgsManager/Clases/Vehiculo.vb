''' <summary>
''' Representa un vehículo de la tabla "Vehiculos" de la base de datos.
''' </summary>
Public Class Vehiculo

    Property Id As String
    Property Matricula As String
    Property Marca As String
    Property Modelo As String
    Property Cliente As Cliente
    Property IdPlaza As Integer
    Property IdGaraje As Integer
    Property PrecioBase As Decimal
    Property PrecioTotal As Decimal
    Property ArrayUrlFotos As String()
    Property UrlFotos As String

    Public Sub New(id As String, matricula As String, marca As String, modelo As String, cliente As Cliente, total As Decimal)

        Me.Id = id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.PrecioTotal = total

    End Sub


    Public Sub New(matricula As String, marca As String, modelo As String, cliente As Cliente, idGaraje As Integer, idPlaza As Integer, base As Decimal, total As Decimal, urlFotos As String)

        Me.Id = Id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.IdGaraje = idGaraje
        Me.IdPlaza = idPlaza
        Me.PrecioBase = base
        Me.PrecioTotal = total
        Me.UrlFotos = urlFotos

        'If Foo.HayTexto(urlFotos) Then

        '    Me.ArrayUrlFotos = urlFotos.Split(" ")

        'End If

    End Sub

End Class
