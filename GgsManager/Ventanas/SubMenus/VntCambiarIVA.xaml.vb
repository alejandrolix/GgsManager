Public Class VntCambiarIVA

    Private PorcentajeIVA As Integer

    Private Sub GuardarIVABtn_Click(sender As Object, e As RoutedEventArgs)

        If ComprobarNumIntroducido() Then

            If Foo.GuardarNuevoIVA(PorcentajeIVA) Then

                MessageBox.Show("Se ha guardado el I.V.A.", "I.V.A. Guardado", MessageBoxButton.OK, MessageBoxImage.Information)
            Else

                MessageBox.Show("Ha habido un problema al guardar el I.V.A.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        End If

    End Sub

    Private Function ComprobarNumIntroducido() As Boolean

        Dim hayNumero As Boolean

        If Foo.HayTexto(NuevoIVATxt.Text) Then

            Try
                Me.PorcentajeIVA = Integer.Parse(NuevoIVATxt.Text)
                hayNumero = True

            Catch ex As Exception

                MessageBox.Show("Tienes que introducir un número.", "I.V.A. Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

                If Foo.HayTexto(NuevoIVATxt.Text) Then

                    NuevoIVATxt.Text = ""

                End If

            End Try

        ElseIf Not Foo.HayTexto(NuevoIVATxt.Text) Then              ' El usuario no ha introducido nada.

            hayNumero = False
            MessageBox.Show("Tienes que introducir un número.", "I.V.A. Vacío", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Return hayNumero

    End Function

End Class
