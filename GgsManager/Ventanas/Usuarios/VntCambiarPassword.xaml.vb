Public Class VntCambiarPassword

    Private IdUsuario As Integer

    Private Sub AceptarBtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarDatosIntroducidos() Then

            Dim hashPassword As String = Usuario.ObtenerSHA1HashFromPassword(NuevaPasswordPsb.Password)

            NuevaPasswordPsb.Clear()
            RepetirPasswordPsb.Clear()

            If Usuario.ModificarPasswordPorId(IdUsuario, hashPassword) Then

                MessageBox.Show("Se ha modificado la contraseña del usuario seleccionado.", "Contraseña Modificada", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub


    ''' <summary>
    ''' Comprueba que los datos introducidos por el usuario son correctos.
    ''' </summary>
    ''' <returns>True: Los datos introducidos son correctos. False: Los datos introducidos no son correctos.</returns>
    Private Function ComprobarDatosIntroducidos() As Boolean

        Dim hayNuevaPassword, hayRepePassword As Boolean

        ' Comprobación de errores.

        If Foo.HayTexto(NuevaPasswordPsb.Password) Then

            hayNuevaPassword = True

        ElseIf Not Foo.HayTexto(NuevaPasswordPsb.Password) Then

            MessageBox.Show("Tienes que introducir la nueva contraseña.", "Nueva Contraseña Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        ElseIf NuevaPasswordPsb.Password.Length <= 4 Then

            MessageBox.Show("La contraseña tiene que tener más de 4 caracteres.", "Contraseña Corta", MessageBoxButton.OK, MessageBoxImage.Error)
            NuevaPasswordPsb.Clear()

        End If

        If Foo.HayTexto(RepetirPasswordPsb.Password) Then

            If RepetirPasswordPsb.Password.Equals(NuevaPasswordPsb.Password) Then

                hayRepePassword = True
            Else

                MessageBox.Show("Las contraseñas no son iguales.", "Contraseñas no Coinciden", MessageBoxButton.OK, MessageBoxImage.Error)
                RepetirPasswordPsb.Clear()

            End If
        ElseIf Not Foo.HayTexto(RepetirPasswordPsb.Password) Then

            MessageBox.Show("La repetición de la nueva contraseña está vacía.", "Repetición Contraseña Vacía", MessageBoxButton.OK, MessageBoxImage.Error)
        Else

            MessageBox.Show("Tienes que introducir otra vez la nueva contraseña.", "Repetición Contraseña Vacía", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Return hayNuevaPassword And hayRepePassword

    End Function

    Public Sub New(ByRef idUsuario As Integer)

        InitializeComponent()
        Me.IdUsuario = idUsuario

    End Sub

End Class
