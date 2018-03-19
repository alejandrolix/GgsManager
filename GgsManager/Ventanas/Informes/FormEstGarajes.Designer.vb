<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormEstGarajes
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ReportDataSource1 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        Me.ReportViewer = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.DtPorcGaraje = New GgsManager.DtPorcGaraje()
        Me.EstadisticasBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.DtPorcGaraje, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EstadisticasBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ReportViewer
        '
        Me.ReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        ReportDataSource1.Name = "DtPorcGaraje"
        ReportDataSource1.Value = Me.EstadisticasBindingSource
        Me.ReportViewer.LocalReport.DataSources.Add(ReportDataSource1)
        Me.ReportViewer.LocalReport.ReportEmbeddedResource = "GgsManager.InfEstGarajes.rdlc"
        Me.ReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewer.Name = "ReportViewer"
        Me.ReportViewer.Size = New System.Drawing.Size(757, 394)
        Me.ReportViewer.TabIndex = 0
        '
        'DtPorcGaraje
        '
        Me.DtPorcGaraje.DataSetName = "DtPorcGaraje"
        Me.DtPorcGaraje.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'EstadisticasBindingSource
        '
        Me.EstadisticasBindingSource.DataMember = "Estadisticas"
        Me.EstadisticasBindingSource.DataSource = Me.DtPorcGaraje
        '
        'FormEstGarajes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(757, 394)
        Me.Controls.Add(Me.ReportViewer)
        Me.Name = "FormEstGarajes"
        Me.Text = "Estadísticas de Garajes"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DtPorcGaraje, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EstadisticasBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ReportViewer As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents EstadisticasBindingSource As Forms.BindingSource
    Friend WithEvents DtPorcGaraje As DtPorcGaraje
End Class
