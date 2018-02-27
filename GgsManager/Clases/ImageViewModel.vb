Imports System.ComponentModel
Imports System.IO

''' <summary>
''' Convierte la URL de la imagen a un objeto BitmapImage.
''' </summary>
Public Class ImageViewModel
    Implements INotifyPropertyChanged

    Private _bitmap As BitmapImage
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public ReadOnly Property Bitmap() As BitmapImage
        Get
            Return _bitmap
        End Get
    End Property

    Protected Sub OnPropertyChanged(ByVal name As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

    Public Sub New(urlFoto As String)

        Me._bitmap = New BitmapImage()
        _bitmap.BeginInit()
        _bitmap.StreamSource = New FileStream(urlFoto, FileMode.Open)
        _bitmap.EndInit()

    End Sub

End Class
