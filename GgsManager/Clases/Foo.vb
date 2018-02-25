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
    ''' Indica la acción que se va a realizar en un mismo formulario. Ya sea para añadir o modificar datos.
    ''' </summary>
    Enum Accion
        Insertar
        Modificar
    End Enum

End Class
