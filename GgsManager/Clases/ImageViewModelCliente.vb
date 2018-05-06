Imports System.ComponentModel

''' <summary>
''' Convierte la URL de la imagen a un objeto BitmapImage.
''' </summary>
Public Class ImageViewModelCliente
    Implements INotifyPropertyChanged

    ''' <summary>
    ''' Contiene la Imagen del Cliente.
    ''' </summary>
    Private _bitmap As BitmapImage
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property Bitmap() As BitmapImage
        Get
            Return _bitmap
        End Get

        Set(value As BitmapImage)
            Me._bitmap = value
        End Set
    End Property

    Protected Sub OnPropertyChanged(ByVal nombre As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
    End Sub

    Public Sub New(urlFoto As String)

        Me._bitmap = New BitmapImage()
        _bitmap.BeginInit()
        _bitmap.UriSource = New Uri(urlFoto, UriKind.RelativeOrAbsolute)
        _bitmap.CacheOption = BitmapCacheOption.OnLoad
        _bitmap.EndInit()

    End Sub

End Class
