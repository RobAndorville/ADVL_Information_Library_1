<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNewLibrary
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
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtNewLibraryFileName = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtNewLibraryDescription = New System.Windows.Forms.TextBox()
        Me.txtNewLibraryLabel = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnNewLibrary = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.pbIconDefaultAuthor = New System.Windows.Forms.PictureBox()
        Me.txtDefaultAuthorFile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.txtDefaultAuthorSummary = New System.Windows.Forms.TextBox()
        Me.btnEditDefaultAuthor = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.pbIconDefaultCopyright = New System.Windows.Forms.PictureBox()
        Me.txtDefaultCopyrightFile = New System.Windows.Forms.TextBox()
        Me.btnEditDefaultCopyright = New System.Windows.Forms.Button()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.txtDefaultCopyrightSummary = New System.Windows.Forms.TextBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.pbIconDefaultLicense = New System.Windows.Forms.PictureBox()
        Me.txtDefaultLicenseFile = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.btnEditDefaultLicense = New System.Windows.Forms.Button()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.rtbDefaultLicenseNotice = New System.Windows.Forms.RichTextBox()
        Me.GroupBox4.SuspendLayout()
        CType(Me.pbIconDefaultAuthor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        CType(Me.pbIconDefaultCopyright, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox6.SuspendLayout()
        CType(Me.pbIconDefaultLicense, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(714, 12)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(64, 22)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(12, 72)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(55, 13)
        Me.Label26.TabIndex = 97
        Me.Label26.Text = "File name:"
        '
        'txtNewLibraryFileName
        '
        Me.txtNewLibraryFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewLibraryFileName.Location = New System.Drawing.Point(88, 69)
        Me.txtNewLibraryFileName.Name = "txtNewLibraryFileName"
        Me.txtNewLibraryFileName.Size = New System.Drawing.Size(690, 20)
        Me.txtNewLibraryFileName.TabIndex = 96
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(12, 98)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(63, 13)
        Me.Label16.TabIndex = 94
        Me.Label16.Text = "Description:"
        '
        'txtNewLibraryDescription
        '
        Me.txtNewLibraryDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewLibraryDescription.Location = New System.Drawing.Point(88, 95)
        Me.txtNewLibraryDescription.Multiline = True
        Me.txtNewLibraryDescription.Name = "txtNewLibraryDescription"
        Me.txtNewLibraryDescription.Size = New System.Drawing.Size(690, 96)
        Me.txtNewLibraryDescription.TabIndex = 95
        '
        'txtNewLibraryLabel
        '
        Me.txtNewLibraryLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewLibraryLabel.Location = New System.Drawing.Point(88, 40)
        Me.txtNewLibraryLabel.Name = "txtNewLibraryLabel"
        Me.txtNewLibraryLabel.Size = New System.Drawing.Size(690, 20)
        Me.txtNewLibraryLabel.TabIndex = 93
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(12, 43)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(70, 13)
        Me.Label17.TabIndex = 92
        Me.Label17.Text = "Library name:"
        '
        'btnNewLibrary
        '
        Me.btnNewLibrary.Location = New System.Drawing.Point(12, 12)
        Me.btnNewLibrary.Name = "btnNewLibrary"
        Me.btnNewLibrary.Size = New System.Drawing.Size(112, 22)
        Me.btnNewLibrary.TabIndex = 98
        Me.btnNewLibrary.Text = "Create New Library"
        Me.btnNewLibrary.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.pbIconDefaultAuthor)
        Me.GroupBox4.Controls.Add(Me.txtDefaultAuthorFile)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.Label34)
        Me.GroupBox4.Controls.Add(Me.txtDefaultAuthorSummary)
        Me.GroupBox4.Controls.Add(Me.btnEditDefaultAuthor)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 197)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(766, 75)
        Me.GroupBox4.TabIndex = 146
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Default Author:"
        '
        'pbIconDefaultAuthor
        '
        Me.pbIconDefaultAuthor.Location = New System.Drawing.Point(7, 19)
        Me.pbIconDefaultAuthor.Name = "pbIconDefaultAuthor"
        Me.pbIconDefaultAuthor.Size = New System.Drawing.Size(32, 34)
        Me.pbIconDefaultAuthor.TabIndex = 149
        Me.pbIconDefaultAuthor.TabStop = False
        '
        'txtDefaultAuthorFile
        '
        Me.txtDefaultAuthorFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultAuthorFile.Location = New System.Drawing.Point(102, 19)
        Me.txtDefaultAuthorFile.Name = "txtDefaultAuthorFile"
        Me.txtDefaultAuthorFile.Size = New System.Drawing.Size(590, 20)
        Me.txtDefaultAuthorFile.TabIndex = 110
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(70, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 109
        Me.Label1.Text = "File:"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(43, 48)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(53, 13)
        Me.Label34.TabIndex = 124
        Me.Label34.Text = "Summary:"
        '
        'txtDefaultAuthorSummary
        '
        Me.txtDefaultAuthorSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultAuthorSummary.Location = New System.Drawing.Point(102, 45)
        Me.txtDefaultAuthorSummary.Name = "txtDefaultAuthorSummary"
        Me.txtDefaultAuthorSummary.Size = New System.Drawing.Size(658, 20)
        Me.txtDefaultAuthorSummary.TabIndex = 125
        '
        'btnEditDefaultAuthor
        '
        Me.btnEditDefaultAuthor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditDefaultAuthor.Location = New System.Drawing.Point(698, 17)
        Me.btnEditDefaultAuthor.Name = "btnEditDefaultAuthor"
        Me.btnEditDefaultAuthor.Size = New System.Drawing.Size(62, 22)
        Me.btnEditDefaultAuthor.TabIndex = 113
        Me.btnEditDefaultAuthor.Text = "View/Edit"
        Me.btnEditDefaultAuthor.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.pbIconDefaultCopyright)
        Me.GroupBox5.Controls.Add(Me.txtDefaultCopyrightFile)
        Me.GroupBox5.Controls.Add(Me.btnEditDefaultCopyright)
        Me.GroupBox5.Controls.Add(Me.Label38)
        Me.GroupBox5.Controls.Add(Me.Label41)
        Me.GroupBox5.Controls.Add(Me.txtDefaultCopyrightSummary)
        Me.GroupBox5.Location = New System.Drawing.Point(12, 278)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(766, 74)
        Me.GroupBox5.TabIndex = 147
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Default Copyright:"
        '
        'pbIconDefaultCopyright
        '
        Me.pbIconDefaultCopyright.Location = New System.Drawing.Point(6, 19)
        Me.pbIconDefaultCopyright.Name = "pbIconDefaultCopyright"
        Me.pbIconDefaultCopyright.Size = New System.Drawing.Size(32, 34)
        Me.pbIconDefaultCopyright.TabIndex = 145
        Me.pbIconDefaultCopyright.TabStop = False
        '
        'txtDefaultCopyrightFile
        '
        Me.txtDefaultCopyrightFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultCopyrightFile.Location = New System.Drawing.Point(102, 19)
        Me.txtDefaultCopyrightFile.Name = "txtDefaultCopyrightFile"
        Me.txtDefaultCopyrightFile.Size = New System.Drawing.Size(590, 20)
        Me.txtDefaultCopyrightFile.TabIndex = 117
        '
        'btnEditDefaultCopyright
        '
        Me.btnEditDefaultCopyright.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditDefaultCopyright.Location = New System.Drawing.Point(698, 17)
        Me.btnEditDefaultCopyright.Name = "btnEditDefaultCopyright"
        Me.btnEditDefaultCopyright.Size = New System.Drawing.Size(62, 22)
        Me.btnEditDefaultCopyright.TabIndex = 119
        Me.btnEditDefaultCopyright.Text = "View/Edit"
        Me.btnEditDefaultCopyright.UseVisualStyleBackColor = True
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(70, 22)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(26, 13)
        Me.Label38.TabIndex = 118
        Me.Label38.Text = "File:"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(43, 48)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(53, 13)
        Me.Label41.TabIndex = 123
        Me.Label41.Text = "Summary:"
        '
        'txtDefaultCopyrightSummary
        '
        Me.txtDefaultCopyrightSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultCopyrightSummary.Location = New System.Drawing.Point(102, 45)
        Me.txtDefaultCopyrightSummary.Name = "txtDefaultCopyrightSummary"
        Me.txtDefaultCopyrightSummary.Size = New System.Drawing.Size(658, 20)
        Me.txtDefaultCopyrightSummary.TabIndex = 122
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.pbIconDefaultLicense)
        Me.GroupBox6.Controls.Add(Me.txtDefaultLicenseFile)
        Me.GroupBox6.Controls.Add(Me.Label42)
        Me.GroupBox6.Controls.Add(Me.btnEditDefaultLicense)
        Me.GroupBox6.Controls.Add(Me.Label43)
        Me.GroupBox6.Controls.Add(Me.rtbDefaultLicenseNotice)
        Me.GroupBox6.Location = New System.Drawing.Point(12, 358)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(766, 169)
        Me.GroupBox6.TabIndex = 148
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Default License:"
        '
        'pbIconDefaultLicense
        '
        Me.pbIconDefaultLicense.Location = New System.Drawing.Point(6, 19)
        Me.pbIconDefaultLicense.Name = "pbIconDefaultLicense"
        Me.pbIconDefaultLicense.Size = New System.Drawing.Size(32, 34)
        Me.pbIconDefaultLicense.TabIndex = 144
        Me.pbIconDefaultLicense.TabStop = False
        '
        'txtDefaultLicenseFile
        '
        Me.txtDefaultLicenseFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefaultLicenseFile.Location = New System.Drawing.Point(102, 19)
        Me.txtDefaultLicenseFile.Name = "txtDefaultLicenseFile"
        Me.txtDefaultLicenseFile.Size = New System.Drawing.Size(590, 20)
        Me.txtDefaultLicenseFile.TabIndex = 112
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(70, 22)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(26, 13)
        Me.Label42.TabIndex = 111
        Me.Label42.Text = "File:"
        '
        'btnEditDefaultLicense
        '
        Me.btnEditDefaultLicense.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditDefaultLicense.Location = New System.Drawing.Point(698, 17)
        Me.btnEditDefaultLicense.Name = "btnEditDefaultLicense"
        Me.btnEditDefaultLicense.Size = New System.Drawing.Size(62, 22)
        Me.btnEditDefaultLicense.TabIndex = 114
        Me.btnEditDefaultLicense.Text = "View/Edit"
        Me.btnEditDefaultLicense.UseVisualStyleBackColor = True
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(6, 56)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(53, 13)
        Me.Label43.TabIndex = 120
        Me.Label43.Text = "Summary:"
        '
        'rtbDefaultLicenseNotice
        '
        Me.rtbDefaultLicenseNotice.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbDefaultLicenseNotice.Location = New System.Drawing.Point(65, 47)
        Me.rtbDefaultLicenseNotice.Name = "rtbDefaultLicenseNotice"
        Me.rtbDefaultLicenseNotice.Size = New System.Drawing.Size(695, 116)
        Me.rtbDefaultLicenseNotice.TabIndex = 142
        Me.rtbDefaultLicenseNotice.Text = ""
        '
        'frmNewLibrary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(790, 539)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.btnNewLibrary)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.txtNewLibraryFileName)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtNewLibraryDescription)
        Me.Controls.Add(Me.txtNewLibraryLabel)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.btnExit)
        Me.Name = "frmNewLibrary"
        Me.Text = "New Library"
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.pbIconDefaultAuthor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.pbIconDefaultCopyright, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.pbIconDefaultLicense, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnExit As Button
    Friend WithEvents Label26 As Label
    Friend WithEvents txtNewLibraryFileName As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtNewLibraryDescription As TextBox
    Friend WithEvents txtNewLibraryLabel As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents btnNewLibrary As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents pbIconDefaultAuthor As PictureBox
    Friend WithEvents txtDefaultAuthorFile As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label34 As Label
    Friend WithEvents txtDefaultAuthorSummary As TextBox
    Friend WithEvents btnEditDefaultAuthor As Button
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents pbIconDefaultCopyright As PictureBox
    Friend WithEvents txtDefaultCopyrightFile As TextBox
    Friend WithEvents btnEditDefaultCopyright As Button
    Friend WithEvents Label38 As Label
    Friend WithEvents Label41 As Label
    Friend WithEvents txtDefaultCopyrightSummary As TextBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents pbIconDefaultLicense As PictureBox
    Friend WithEvents txtDefaultLicenseFile As TextBox
    Friend WithEvents Label42 As Label
    Friend WithEvents btnEditDefaultLicense As Button
    Friend WithEvents Label43 As Label
    Friend WithEvents rtbDefaultLicenseNotice As RichTextBox
End Class
