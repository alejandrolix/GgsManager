<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormInfPlazas
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ReportDataSource2 As Microsoft.Reporting.WinForms.ReportDataSource = New Microsoft.Reporting.WinForms.ReportDataSource()
        Me.PlazasBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DtPlazas = New GgsManager.DtPlazas()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ReportViewer = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PlTodasRb = New System.Windows.Forms.RadioButton()
        Me.PlOcupadaRb = New System.Windows.Forms.RadioButton()
        Me.PlLibreRb = New System.Windows.Forms.RadioButton()
        CType(Me.PlazasBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DtPlazas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PlazasBindingSource
        '
        Me.PlazasBindingSource.DataMember = "Plazas"
        Me.PlazasBindingSource.DataSource = Me.DtPlazas
        '
        'DtPlazas
        '
        Me.DtPlazas.DataSetName = "DtPlazas"
        Me.DtPlazas.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ReportViewer)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Size = New System.Drawing.Size(800, 450)
        Me.SplitContainer1.SplitterDistance = 711
        Me.SplitContainer1.TabIndex = 0
        '
        'ReportViewer
        '
        Me.ReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        ReportDataSource2.Name = "DtPlazas"
        ReportDataSource2.Value = Me.PlazasBindingSource
        Me.ReportViewer.LocalReport.DataSources.Add(ReportDataSource2)
        Me.ReportViewer.LocalReport.ReportEmbeddedResource = "GgsManager.InfPlazas.rdlc"
        Me.ReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewer.Name = "ReportViewer"
        Me.ReportViewer.ServerReport.BearerToken = Nothing
        Me.ReportViewer.Size = New System.Drawing.Size(711, 450)
        Me.ReportViewer.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.PlTodasRb)
        Me.GroupBox1.Controls.Add(Me.PlOcupadaRb)
        Me.GroupBox1.Controls.Add(Me.PlLibreRb)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(22, 251)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(109, 135)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Situación Plaza"
        '
        'PlTodasRb
        '
        Me.PlTodasRb.AutoSize = True
        Me.PlTodasRb.Location = New System.Drawing.Point(9, 108)
        Me.PlTodasRb.Name = "PlTodasRb"
        Me.PlTodasRb.Size = New System.Drawing.Size(66, 21)
        Me.PlTodasRb.TabIndex = 2
        Me.PlTodasRb.Text = "Todas"
        Me.PlTodasRb.UseVisualStyleBackColor = True
        '
        'PlOcupadaRb
        '
        Me.PlOcupadaRb.AutoSize = True
        Me.PlOcupadaRb.Location = New System.Drawing.Point(9, 81)
        Me.PlOcupadaRb.Name = "PlOcupadaRb"
        Me.PlOcupadaRb.Size = New System.Drawing.Size(84, 21)
        Me.PlOcupadaRb.TabIndex = 1
        Me.PlOcupadaRb.Text = "Ocupada"
        Me.PlOcupadaRb.UseVisualStyleBackColor = True
        '
        'PlLibreRb
        '
        Me.PlLibreRb.AutoSize = True
        Me.PlLibreRb.Location = New System.Drawing.Point(9, 54)
        Me.PlLibreRb.Name = "PlLibreRb"
        Me.PlLibreRb.Size = New System.Drawing.Size(58, 21)
        Me.PlLibreRb.TabIndex = 0
        Me.PlLibreRb.Text = "Libre"
        Me.PlLibreRb.UseVisualStyleBackColor = True
        '
        'FormInfPlazas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "FormInfPlazas"
        Me.Text = "Informe de Plazas"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PlazasBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DtPlazas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As Forms.SplitContainer
    Friend WithEvents ReportViewer As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents GroupBox1 As Forms.GroupBox
    Friend WithEvents PlTodasRb As Forms.RadioButton
    Friend WithEvents PlOcupadaRb As Forms.RadioButton
    Friend WithEvents PlLibreRb As Forms.RadioButton
    Friend WithEvents PlazasBindingSource As Forms.BindingSource
    Friend WithEvents DtPlazas As DtPlazas
End Class
