
''' <summary>
''' Clase que contiene métodos que se van a usar en varios sitios.
''' </summary>
Public Class Foo

    ''' <summary>
    ''' Comprueba si hay texto.
    ''' </summary>
    ''' <param name="texto">El texto a comprobar.</param>
    ''' <returns>True: No hay texto. False: Hay texto.</returns>
    Public Shared Function HayTexto(ByRef texto As String) As Boolean

        Return Not texto.Equals("")

    End Function

End Class
