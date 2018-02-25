''' <summary>
''' Representa un usuario del programa de la tabla "UsuariosPrograma" de la base de datos.
''' </summary>
Public Class UsuarioPrograma

    Property Id As Integer
    Property Nombre As String
    Property EsGestor As Boolean

    Public Sub New(id As Integer, nombre As String, esGestor As Boolean)
        Me.Id = id
        Me.Nombre = nombre
        Me.EsGestor = esGestor
    End Sub

End Class
