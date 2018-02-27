Public Class Plaza

    Property Id As Integer
    Property IdGaraje As Integer
    Property Situacion As Integer

    Public Sub New(id As Integer, idGaraje As Integer, situacion As Integer)

        Me.Id = id
        Me.IdGaraje = idGaraje
        Me.Situacion = situacion

    End Sub

    Public Sub New(id As Integer)

        Me.Id = id

    End Sub

End Class
