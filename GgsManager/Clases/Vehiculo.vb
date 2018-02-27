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
    Property Base As Decimal
    Property Total As Decimal
    Property ArrayUrlFotos As String()

    Public Sub New(id As String, matricula As String, marca As String, modelo As String, cliente As Cliente, total As Decimal, urlFotos As String)

        Me.Id = id
        Me.Matricula = matricula
        Me.Marca = marca
        Me.Modelo = modelo
        Me.Cliente = cliente
        Me.Total = total
        ' Me.ArrayUrlFotos = urlFotos.Split(" ")

    End Sub

End Class
