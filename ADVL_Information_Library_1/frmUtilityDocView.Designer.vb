<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtilityDocView
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDocLabel = New System.Windows.Forms.TextBox()
        Me.TabPage12 = New System.Windows.Forms.TabPage()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.txtDocumentLabel = New System.Windows.Forms.TextBox()
        Me.txtNodeDescription = New System.Windows.Forms.TextBox()
        Me.txtLastEditDate = New System.Windows.Forms.TextBox()
        Me.txtCreationDate = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbIconAuthor = New System.Windows.Forms.PictureBox()
        Me.txtAuthorFile = New System.Windows.Forms.TextBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtAuthorSummary = New System.Windows.Forms.TextBox()
        Me.btnEditAuthor = New System.Windows.Forms.Button()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.dgvDocument = New System.Windows.Forms.DataGridView()
        Me.txtDocumentFileName = New System.Windows.Forms.TextBox()
        Me.rtbDocument = New System.Windows.Forms.RichTextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnCancelDocumentChanges = New System.Windows.Forms.Button()
        Me.btnSaveDocumentChanges = New System.Windows.Forms.Button()
        Me.TabControl4 = New System.Windows.Forms.TabControl()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FontToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.TabPage12.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.pbIconAuthor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage6.SuspendLayout()
        CType(Me.dgvDocument, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl4.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(564, 12)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(64, 22)
        Me.btnExit.TabIndex = 62
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 67
        Me.Label2.Text = "Document label:"
        '
        'txtDocLabel
        '
        Me.txtDocLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocLabel.Location = New System.Drawing.Point(102, 14)
        Me.txtDocLabel.Name = "txtDocLabel"
        Me.txtDocLabel.Size = New System.Drawing.Size(456, 20)
        Me.txtDocLabel.TabIndex = 68
        '
        'TabPage12
        '
        Me.TabPage12.Controls.Add(Me.Label18)
        Me.TabPage12.Controls.Add(Me.txtFileName)
        Me.TabPage12.Controls.Add(Me.txtDocumentLabel)
        Me.TabPage12.Controls.Add(Me.txtNodeDescription)
        Me.TabPage12.Controls.Add(Me.txtLastEditDate)
        Me.TabPage12.Controls.Add(Me.txtCreationDate)
        Me.TabPage12.Controls.Add(Me.Label3)
        Me.TabPage12.Controls.Add(Me.GroupBox2)
        Me.TabPage12.Controls.Add(Me.Label35)
        Me.TabPage12.Controls.Add(Me.Label37)
        Me.TabPage12.Controls.Add(Me.Label36)
        Me.TabPage12.Location = New System.Drawing.Point(4, 22)
        Me.TabPage12.Name = "TabPage12"
        Me.TabPage12.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage12.Size = New System.Drawing.Size(608, 456)
        Me.TabPage12.TabIndex = 0
        Me.TabPage12.Text = "Document Info"
        Me.TabPage12.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 35)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(55, 13)
        Me.Label18.TabIndex = 149
        Me.Label18.Text = "File name:"
        '
        'txtFileName
        '
        Me.txtFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFileName.Location = New System.Drawing.Point(96, 32)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(506, 20)
        Me.txtFileName.TabIndex = 148
        '
        'txtDocumentLabel
        '
        Me.txtDocumentLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocumentLabel.Location = New System.Drawing.Point(96, 6)
        Me.txtDocumentLabel.Name = "txtDocumentLabel"
        Me.txtDocumentLabel.Size = New System.Drawing.Size(506, 20)
        Me.txtDocumentLabel.TabIndex = 147
        '
        'txtNodeDescription
        '
        Me.txtNodeDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNodeDescription.Location = New System.Drawing.Point(88, 58)
        Me.txtNodeDescription.Multiline = True
        Me.txtNodeDescription.Name = "txtNodeDescription"
        Me.txtNodeDescription.Size = New System.Drawing.Size(514, 54)
        Me.txtNodeDescription.TabIndex = 116
        '
        'txtLastEditDate
        '
        Me.txtLastEditDate.Location = New System.Drawing.Point(401, 118)
        Me.txtLastEditDate.Name = "txtLastEditDate"
        Me.txtLastEditDate.Size = New System.Drawing.Size(218, 20)
        Me.txtLastEditDate.TabIndex = 108
        '
        'txtCreationDate
        '
        Me.txtCreationDate.Location = New System.Drawing.Point(88, 118)
        Me.txtCreationDate.Name = "txtCreationDate"
        Me.txtCreationDate.Size = New System.Drawing.Size(218, 20)
        Me.txtCreationDate.TabIndex = 106
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 146
        Me.Label3.Text = "Document label:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.pbIconAuthor)
        Me.GroupBox2.Controls.Add(Me.txtAuthorFile)
        Me.GroupBox2.Controls.Add(Me.Label39)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.txtAuthorSummary)
        Me.GroupBox2.Controls.Add(Me.btnEditAuthor)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 144)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(596, 75)
        Me.GroupBox2.TabIndex = 143
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Author:"
        '
        'pbIconAuthor
        '
        Me.pbIconAuthor.Location = New System.Drawing.Point(5, 17)
        Me.pbIconAuthor.Name = "pbIconAuthor"
        Me.pbIconAuthor.Size = New System.Drawing.Size(32, 34)
        Me.pbIconAuthor.TabIndex = 149
        Me.pbIconAuthor.TabStop = False
        '
        'txtAuthorFile
        '
        Me.txtAuthorFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorFile.Location = New System.Drawing.Point(102, 19)
        Me.txtAuthorFile.Name = "txtAuthorFile"
        Me.txtAuthorFile.Size = New System.Drawing.Size(420, 20)
        Me.txtAuthorFile.TabIndex = 110
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(70, 22)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(26, 13)
        Me.Label39.TabIndex = 109
        Me.Label39.Text = "File:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(43, 48)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 124
        Me.Label11.Text = "Summary:"
        '
        'txtAuthorSummary
        '
        Me.txtAuthorSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorSummary.Location = New System.Drawing.Point(102, 45)
        Me.txtAuthorSummary.Name = "txtAuthorSummary"
        Me.txtAuthorSummary.Size = New System.Drawing.Size(488, 20)
        Me.txtAuthorSummary.TabIndex = 125
        '
        'btnEditAuthor
        '
        Me.btnEditAuthor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditAuthor.Location = New System.Drawing.Point(528, 17)
        Me.btnEditAuthor.Name = "btnEditAuthor"
        Me.btnEditAuthor.Size = New System.Drawing.Size(62, 22)
        Me.btnEditAuthor.TabIndex = 113
        Me.btnEditAuthor.Text = "View/Edit"
        Me.btnEditAuthor.UseVisualStyleBackColor = True
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(6, 61)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(63, 13)
        Me.Label35.TabIndex = 115
        Me.Label35.Text = "Description:"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(321, 121)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(74, 13)
        Me.Label37.TabIndex = 107
        Me.Label37.Text = "Last edit date:"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(6, 121)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(73, 13)
        Me.Label36.TabIndex = 105
        Me.Label36.Text = "Creation date:"
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.dgvDocument)
        Me.TabPage6.Controls.Add(Me.txtDocumentFileName)
        Me.TabPage6.Controls.Add(Me.rtbDocument)
        Me.TabPage6.Controls.Add(Me.Label17)
        Me.TabPage6.Controls.Add(Me.btnCancelDocumentChanges)
        Me.TabPage6.Controls.Add(Me.btnSaveDocumentChanges)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(608, 456)
        Me.TabPage6.TabIndex = 6
        Me.TabPage6.Text = "Document"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'dgvDocument
        '
        Me.dgvDocument.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDocument.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDocument.Location = New System.Drawing.Point(3, 34)
        Me.dgvDocument.Name = "dgvDocument"
        Me.dgvDocument.Size = New System.Drawing.Size(589, 419)
        Me.dgvDocument.TabIndex = 94
        '
        'txtDocumentFileName
        '
        Me.txtDocumentFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocumentFileName.Location = New System.Drawing.Point(207, 8)
        Me.txtDocumentFileName.Name = "txtDocumentFileName"
        Me.txtDocumentFileName.Size = New System.Drawing.Size(385, 20)
        Me.txtDocumentFileName.TabIndex = 93
        '
        'rtbDocument
        '
        Me.rtbDocument.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbDocument.Location = New System.Drawing.Point(3, 34)
        Me.rtbDocument.Name = "rtbDocument"
        Me.rtbDocument.Size = New System.Drawing.Size(589, 419)
        Me.rtbDocument.TabIndex = 1
        Me.rtbDocument.Text = ""
        Me.rtbDocument.WordWrap = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(146, 11)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(55, 13)
        Me.Label17.TabIndex = 92
        Me.Label17.Text = "File name:"
        '
        'btnCancelDocumentChanges
        '
        Me.btnCancelDocumentChanges.Location = New System.Drawing.Point(76, 6)
        Me.btnCancelDocumentChanges.Name = "btnCancelDocumentChanges"
        Me.btnCancelDocumentChanges.Size = New System.Drawing.Size(64, 22)
        Me.btnCancelDocumentChanges.TabIndex = 64
        Me.btnCancelDocumentChanges.Text = "Cancel"
        Me.btnCancelDocumentChanges.UseVisualStyleBackColor = True
        '
        'btnSaveDocumentChanges
        '
        Me.btnSaveDocumentChanges.Location = New System.Drawing.Point(6, 6)
        Me.btnSaveDocumentChanges.Name = "btnSaveDocumentChanges"
        Me.btnSaveDocumentChanges.Size = New System.Drawing.Size(64, 22)
        Me.btnSaveDocumentChanges.TabIndex = 63
        Me.btnSaveDocumentChanges.Text = "Save"
        Me.btnSaveDocumentChanges.UseVisualStyleBackColor = True
        '
        'TabControl4
        '
        Me.TabControl4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl4.Controls.Add(Me.TabPage6)
        Me.TabControl4.Controls.Add(Me.TabPage12)
        Me.TabControl4.Location = New System.Drawing.Point(12, 40)
        Me.TabControl4.Name = "TabControl4"
        Me.TabControl4.SelectedIndex = 0
        Me.TabControl4.Size = New System.Drawing.Size(616, 482)
        Me.TabControl4.TabIndex = 103
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FontToolStripMenuItem, Me.PasteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(103, 48)
        '
        'FontToolStripMenuItem
        '
        Me.FontToolStripMenuItem.Name = "FontToolStripMenuItem"
        Me.FontToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.FontToolStripMenuItem.Text = "Font"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'frmUtilityDocView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 534)
        Me.Controls.Add(Me.TabControl4)
        Me.Controls.Add(Me.txtDocLabel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnExit)
        Me.Name = "frmUtilityDocView"
        Me.Text = "Utility Document View"
        Me.TabPage12.ResumeLayout(False)
        Me.TabPage12.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.pbIconAuthor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        CType(Me.dgvDocument, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl4.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnExit As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtDocLabel As TextBox
    Friend WithEvents TabPage12 As TabPage
    Friend WithEvents Label18 As Label
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents txtDocumentLabel As TextBox
    Friend WithEvents txtNodeDescription As TextBox
    Friend WithEvents txtLastEditDate As TextBox
    Friend WithEvents txtCreationDate As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents pbIconAuthor As PictureBox
    Friend WithEvents txtAuthorFile As TextBox
    Friend WithEvents Label39 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents txtAuthorSummary As TextBox
    Friend WithEvents btnEditAuthor As Button
    Friend WithEvents Label35 As Label
    Friend WithEvents Label37 As Label
    Friend WithEvents Label36 As Label
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents dgvDocument As DataGridView
    Friend WithEvents txtDocumentFileName As TextBox
    Friend WithEvents rtbDocument As RichTextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents btnCancelDocumentChanges As Button
    Friend WithEvents btnSaveDocumentChanges As Button
    Friend WithEvents TabControl4 As TabControl
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents FontToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FontDialog1 As FontDialog
End Class
