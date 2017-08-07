<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLicense
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.pbIconAuthor = New System.Windows.Forms.PictureBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtAuthorSummary = New System.Windows.Forms.TextBox()
        Me.btnApplyAuthor = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtAuthorContact = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtAuthorDescription = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAuthorName = New System.Windows.Forms.TextBox()
        Me.btnOpenAuthor = New System.Windows.Forms.Button()
        Me.btnSaveAuthor = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtAuthorFileName = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.pbIconCopyright = New System.Windows.Forms.PictureBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtCopyrightSummary = New System.Windows.Forms.TextBox()
        Me.btnApplyCopyright = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCopyrightNotice = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtCopyrightPublicationYear = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCopyrightOwnerName = New System.Windows.Forms.TextBox()
        Me.btnOpenCopyright = New System.Windows.Forms.Button()
        Me.btnSaveCopyright = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCopyrightFileName = New System.Windows.Forms.TextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.pbIconLicense = New System.Windows.Forms.PictureBox()
        Me.rtbLicenseText = New System.Windows.Forms.RichTextBox()
        Me.rtbLicenseNotice = New System.Windows.Forms.RichTextBox()
        Me.rtbLicenseDescription = New System.Windows.Forms.RichTextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtLicenseName = New System.Windows.Forms.TextBox()
        Me.txtLicenseType = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtLicensePublicationYear = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbLicense = New System.Windows.Forms.ComboBox()
        Me.btnApplyLicense = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtLicenseOwnerName = New System.Windows.Forms.TextBox()
        Me.btnOpenLicense = New System.Windows.Forms.Button()
        Me.btnSaveLicense = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtLicenseFileName = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.pbIconAuthor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.pbIconCopyright, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.pbIconLicense, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(599, 12)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(64, 22)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 40)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(651, 532)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.pbIconAuthor)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.txtAuthorSummary)
        Me.TabPage1.Controls.Add(Me.btnApplyAuthor)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.txtAuthorContact)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.txtAuthorDescription)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.txtAuthorName)
        Me.TabPage1.Controls.Add(Me.btnOpenAuthor)
        Me.TabPage1.Controls.Add(Me.btnSaveAuthor)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.txtAuthorFileName)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(643, 506)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Author"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'pbIconAuthor
        '
        Me.pbIconAuthor.Location = New System.Drawing.Point(9, 28)
        Me.pbIconAuthor.Name = "pbIconAuthor"
        Me.pbIconAuthor.Size = New System.Drawing.Size(32, 34)
        Me.pbIconAuthor.TabIndex = 148
        Me.pbIconAuthor.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 95)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 119
        Me.Label11.Text = "Summary:"
        '
        'txtAuthorSummary
        '
        Me.txtAuthorSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorSummary.Location = New System.Drawing.Point(98, 92)
        Me.txtAuthorSummary.Name = "txtAuthorSummary"
        Me.txtAuthorSummary.Size = New System.Drawing.Size(539, 20)
        Me.txtAuthorSummary.TabIndex = 118
        '
        'btnApplyAuthor
        '
        Me.btnApplyAuthor.Location = New System.Drawing.Point(214, 35)
        Me.btnApplyAuthor.Name = "btnApplyAuthor"
        Me.btnApplyAuthor.Size = New System.Drawing.Size(143, 22)
        Me.btnApplyAuthor.TabIndex = 117
        Me.btnApplyAuthor.Text = "Apply Document Author"
        Me.btnApplyAuthor.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 211)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 116
        Me.Label3.Text = "Contact:"
        '
        'txtAuthorContact
        '
        Me.txtAuthorContact.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorContact.Location = New System.Drawing.Point(101, 208)
        Me.txtAuthorContact.Name = "txtAuthorContact"
        Me.txtAuthorContact.Size = New System.Drawing.Size(536, 20)
        Me.txtAuthorContact.TabIndex = 115
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 121)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 114
        Me.Label2.Text = "Description:"
        '
        'txtAuthorDescription
        '
        Me.txtAuthorDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorDescription.Location = New System.Drawing.Point(101, 118)
        Me.txtAuthorDescription.Multiline = True
        Me.txtAuthorDescription.Name = "txtAuthorDescription"
        Me.txtAuthorDescription.Size = New System.Drawing.Size(536, 84)
        Me.txtAuthorDescription.TabIndex = 113
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 69)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 112
        Me.Label1.Text = "Name:"
        '
        'txtAuthorName
        '
        Me.txtAuthorName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorName.Location = New System.Drawing.Point(98, 66)
        Me.txtAuthorName.Name = "txtAuthorName"
        Me.txtAuthorName.Size = New System.Drawing.Size(539, 20)
        Me.txtAuthorName.TabIndex = 111
        '
        'btnOpenAuthor
        '
        Me.btnOpenAuthor.Location = New System.Drawing.Point(98, 35)
        Me.btnOpenAuthor.Name = "btnOpenAuthor"
        Me.btnOpenAuthor.Size = New System.Drawing.Size(52, 22)
        Me.btnOpenAuthor.TabIndex = 110
        Me.btnOpenAuthor.Text = "Open"
        Me.btnOpenAuthor.UseVisualStyleBackColor = True
        '
        'btnSaveAuthor
        '
        Me.btnSaveAuthor.Location = New System.Drawing.Point(156, 35)
        Me.btnSaveAuthor.Name = "btnSaveAuthor"
        Me.btnSaveAuthor.Size = New System.Drawing.Size(52, 22)
        Me.btnSaveAuthor.TabIndex = 109
        Me.btnSaveAuthor.Text = "Save"
        Me.btnSaveAuthor.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 13)
        Me.Label7.TabIndex = 108
        Me.Label7.Text = "Author file name:"
        '
        'txtAuthorFileName
        '
        Me.txtAuthorFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAuthorFileName.Location = New System.Drawing.Point(98, 9)
        Me.txtAuthorFileName.Name = "txtAuthorFileName"
        Me.txtAuthorFileName.Size = New System.Drawing.Size(539, 20)
        Me.txtAuthorFileName.TabIndex = 107
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.pbIconCopyright)
        Me.TabPage2.Controls.Add(Me.Label12)
        Me.TabPage2.Controls.Add(Me.txtCopyrightSummary)
        Me.TabPage2.Controls.Add(Me.btnApplyCopyright)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.txtCopyrightNotice)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.txtCopyrightPublicationYear)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.txtCopyrightOwnerName)
        Me.TabPage2.Controls.Add(Me.btnOpenCopyright)
        Me.TabPage2.Controls.Add(Me.btnSaveCopyright)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.txtCopyrightFileName)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(643, 506)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Copyright"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'pbIconCopyright
        '
        Me.pbIconCopyright.Location = New System.Drawing.Point(9, 28)
        Me.pbIconCopyright.Name = "pbIconCopyright"
        Me.pbIconCopyright.Size = New System.Drawing.Size(32, 34)
        Me.pbIconCopyright.TabIndex = 144
        Me.pbIconCopyright.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 121)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(53, 13)
        Me.Label12.TabIndex = 123
        Me.Label12.Text = "Summary:"
        '
        'txtCopyrightSummary
        '
        Me.txtCopyrightSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyrightSummary.Location = New System.Drawing.Point(98, 118)
        Me.txtCopyrightSummary.Name = "txtCopyrightSummary"
        Me.txtCopyrightSummary.Size = New System.Drawing.Size(539, 20)
        Me.txtCopyrightSummary.TabIndex = 122
        '
        'btnApplyCopyright
        '
        Me.btnApplyCopyright.Location = New System.Drawing.Point(227, 35)
        Me.btnApplyCopyright.Name = "btnApplyCopyright"
        Me.btnApplyCopyright.Size = New System.Drawing.Size(143, 22)
        Me.btnApplyCopyright.TabIndex = 121
        Me.btnApplyCopyright.Text = "Apply Document Copyright"
        Me.btnApplyCopyright.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 147)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(41, 13)
        Me.Label8.TabIndex = 120
        Me.Label8.Text = "Notice:"
        '
        'txtCopyrightNotice
        '
        Me.txtCopyrightNotice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyrightNotice.Location = New System.Drawing.Point(98, 144)
        Me.txtCopyrightNotice.Multiline = True
        Me.txtCopyrightNotice.Name = "txtCopyrightNotice"
        Me.txtCopyrightNotice.Size = New System.Drawing.Size(539, 175)
        Me.txtCopyrightNotice.TabIndex = 119
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 95)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(85, 13)
        Me.Label6.TabIndex = 118
        Me.Label6.Text = "Publication year:"
        '
        'txtCopyrightPublicationYear
        '
        Me.txtCopyrightPublicationYear.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyrightPublicationYear.Location = New System.Drawing.Point(98, 92)
        Me.txtCopyrightPublicationYear.Name = "txtCopyrightPublicationYear"
        Me.txtCopyrightPublicationYear.Size = New System.Drawing.Size(539, 20)
        Me.txtCopyrightPublicationYear.TabIndex = 117
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 13)
        Me.Label5.TabIndex = 116
        Me.Label5.Text = "Owner name:"
        '
        'txtCopyrightOwnerName
        '
        Me.txtCopyrightOwnerName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyrightOwnerName.Location = New System.Drawing.Point(98, 66)
        Me.txtCopyrightOwnerName.Name = "txtCopyrightOwnerName"
        Me.txtCopyrightOwnerName.Size = New System.Drawing.Size(539, 20)
        Me.txtCopyrightOwnerName.TabIndex = 115
        '
        'btnOpenCopyright
        '
        Me.btnOpenCopyright.Location = New System.Drawing.Point(111, 35)
        Me.btnOpenCopyright.Name = "btnOpenCopyright"
        Me.btnOpenCopyright.Size = New System.Drawing.Size(52, 22)
        Me.btnOpenCopyright.TabIndex = 114
        Me.btnOpenCopyright.Text = "Open"
        Me.btnOpenCopyright.UseVisualStyleBackColor = True
        '
        'btnSaveCopyright
        '
        Me.btnSaveCopyright.Location = New System.Drawing.Point(169, 35)
        Me.btnSaveCopyright.Name = "btnSaveCopyright"
        Me.btnSaveCopyright.Size = New System.Drawing.Size(52, 22)
        Me.btnSaveCopyright.TabIndex = 113
        Me.btnSaveCopyright.Text = "Save"
        Me.btnSaveCopyright.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 13)
        Me.Label4.TabIndex = 112
        Me.Label4.Text = "Copyright file name:"
        '
        'txtCopyrightFileName
        '
        Me.txtCopyrightFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyrightFileName.Location = New System.Drawing.Point(111, 9)
        Me.txtCopyrightFileName.Name = "txtCopyrightFileName"
        Me.txtCopyrightFileName.Size = New System.Drawing.Size(526, 20)
        Me.txtCopyrightFileName.TabIndex = 111
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.pbIconLicense)
        Me.TabPage3.Controls.Add(Me.rtbLicenseText)
        Me.TabPage3.Controls.Add(Me.rtbLicenseNotice)
        Me.TabPage3.Controls.Add(Me.rtbLicenseDescription)
        Me.TabPage3.Controls.Add(Me.Label20)
        Me.TabPage3.Controls.Add(Me.txtLicenseName)
        Me.TabPage3.Controls.Add(Me.txtLicenseType)
        Me.TabPage3.Controls.Add(Me.Label19)
        Me.TabPage3.Controls.Add(Me.Label18)
        Me.TabPage3.Controls.Add(Me.Label17)
        Me.TabPage3.Controls.Add(Me.Label16)
        Me.TabPage3.Controls.Add(Me.Label15)
        Me.TabPage3.Controls.Add(Me.txtLicensePublicationYear)
        Me.TabPage3.Controls.Add(Me.Label14)
        Me.TabPage3.Controls.Add(Me.Label13)
        Me.TabPage3.Controls.Add(Me.cmbLicense)
        Me.TabPage3.Controls.Add(Me.btnApplyLicense)
        Me.TabPage3.Controls.Add(Me.Label10)
        Me.TabPage3.Controls.Add(Me.txtLicenseOwnerName)
        Me.TabPage3.Controls.Add(Me.btnOpenLicense)
        Me.TabPage3.Controls.Add(Me.btnSaveLicense)
        Me.TabPage3.Controls.Add(Me.Label9)
        Me.TabPage3.Controls.Add(Me.txtLicenseFileName)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(643, 506)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "License"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'pbIconLicense
        '
        Me.pbIconLicense.Location = New System.Drawing.Point(9, 28)
        Me.pbIconLicense.Name = "pbIconLicense"
        Me.pbIconLicense.Size = New System.Drawing.Size(32, 34)
        Me.pbIconLicense.TabIndex = 143
        Me.pbIconLicense.TabStop = False
        '
        'rtbLicenseText
        '
        Me.rtbLicenseText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbLicenseText.Location = New System.Drawing.Point(97, 329)
        Me.rtbLicenseText.Name = "rtbLicenseText"
        Me.rtbLicenseText.Size = New System.Drawing.Size(540, 170)
        Me.rtbLicenseText.TabIndex = 142
        Me.rtbLicenseText.Text = ""
        '
        'rtbLicenseNotice
        '
        Me.rtbLicenseNotice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbLicenseNotice.Location = New System.Drawing.Point(97, 249)
        Me.rtbLicenseNotice.Name = "rtbLicenseNotice"
        Me.rtbLicenseNotice.Size = New System.Drawing.Size(540, 74)
        Me.rtbLicenseNotice.TabIndex = 141
        Me.rtbLicenseNotice.Text = ""
        '
        'rtbLicenseDescription
        '
        Me.rtbLicenseDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbLicenseDescription.Location = New System.Drawing.Point(97, 171)
        Me.rtbLicenseDescription.Name = "rtbLicenseDescription"
        Me.rtbLicenseDescription.Size = New System.Drawing.Size(540, 72)
        Me.rtbLicenseDescription.TabIndex = 140
        Me.rtbLicenseDescription.Text = ""
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(6, 148)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(76, 13)
        Me.Label20.TabIndex = 139
        Me.Label20.Text = "License name:"
        '
        'txtLicenseName
        '
        Me.txtLicenseName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseName.Location = New System.Drawing.Point(97, 145)
        Me.txtLicenseName.Name = "txtLicenseName"
        Me.txtLicenseName.Size = New System.Drawing.Size(540, 20)
        Me.txtLicenseName.TabIndex = 138
        '
        'txtLicenseType
        '
        Me.txtLicenseType.Location = New System.Drawing.Point(290, 119)
        Me.txtLicenseType.Name = "txtLicenseType"
        Me.txtLicenseType.Size = New System.Drawing.Size(209, 20)
        Me.txtLicenseType.TabIndex = 137
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(214, 122)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(70, 13)
        Me.Label19.TabIndex = 136
        Me.Label19.Text = "License type:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 187)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(61, 13)
        Me.Label18.TabIndex = 135
        Me.Label18.Text = "description:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 174)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(44, 13)
        Me.Label17.TabIndex = 133
        Me.Label17.Text = "License"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 332)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(67, 13)
        Me.Label16.TabIndex = 129
        Me.Label16.Text = "License text:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 252)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(79, 13)
        Me.Label15.TabIndex = 127
        Me.Label15.Text = "License notice:"
        '
        'txtLicensePublicationYear
        '
        Me.txtLicensePublicationYear.Location = New System.Drawing.Point(97, 119)
        Me.txtLicensePublicationYear.Name = "txtLicensePublicationYear"
        Me.txtLicensePublicationYear.Size = New System.Drawing.Size(90, 20)
        Me.txtLicensePublicationYear.TabIndex = 126
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 122)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(85, 13)
        Me.Label14.TabIndex = 125
        Me.Label14.Text = "Publication year:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 96)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 13)
        Me.Label13.TabIndex = 124
        Me.Label13.Text = "Owner name:"
        '
        'cmbLicense
        '
        Me.cmbLicense.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLicense.FormattingEnabled = True
        Me.cmbLicense.Location = New System.Drawing.Point(82, 66)
        Me.cmbLicense.Name = "cmbLicense"
        Me.cmbLicense.Size = New System.Drawing.Size(555, 21)
        Me.cmbLicense.TabIndex = 123
        '
        'btnApplyLicense
        '
        Me.btnApplyLicense.Location = New System.Drawing.Point(227, 35)
        Me.btnApplyLicense.Name = "btnApplyLicense"
        Me.btnApplyLicense.Size = New System.Drawing.Size(143, 22)
        Me.btnApplyLicense.TabIndex = 122
        Me.btnApplyLicense.Text = "Apply Document License"
        Me.btnApplyLicense.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 69)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 120
        Me.Label10.Text = "Abbreviation:"
        '
        'txtLicenseOwnerName
        '
        Me.txtLicenseOwnerName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseOwnerName.Location = New System.Drawing.Point(82, 93)
        Me.txtLicenseOwnerName.Name = "txtLicenseOwnerName"
        Me.txtLicenseOwnerName.Size = New System.Drawing.Size(555, 20)
        Me.txtLicenseOwnerName.TabIndex = 119
        '
        'btnOpenLicense
        '
        Me.btnOpenLicense.Location = New System.Drawing.Point(111, 35)
        Me.btnOpenLicense.Name = "btnOpenLicense"
        Me.btnOpenLicense.Size = New System.Drawing.Size(52, 22)
        Me.btnOpenLicense.TabIndex = 118
        Me.btnOpenLicense.Text = "Open"
        Me.btnOpenLicense.UseVisualStyleBackColor = True
        '
        'btnSaveLicense
        '
        Me.btnSaveLicense.Location = New System.Drawing.Point(169, 35)
        Me.btnSaveLicense.Name = "btnSaveLicense"
        Me.btnSaveLicense.Size = New System.Drawing.Size(52, 22)
        Me.btnSaveLicense.TabIndex = 117
        Me.btnSaveLicense.Text = "Save"
        Me.btnSaveLicense.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(92, 13)
        Me.Label9.TabIndex = 116
        Me.Label9.Text = "License file name:"
        '
        'txtLicenseFileName
        '
        Me.txtLicenseFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseFileName.Location = New System.Drawing.Point(111, 9)
        Me.txtLicenseFileName.Name = "txtLicenseFileName"
        Me.txtLicenseFileName.Size = New System.Drawing.Size(526, 20)
        Me.txtLicenseFileName.TabIndex = 115
        '
        'frmLicense
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 584)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnExit)
        Me.Name = "frmLicense"
        Me.Text = "Author, Copyright and License Information"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.pbIconAuthor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.pbIconCopyright, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.pbIconLicense, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnExit As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents Label7 As Label
    Friend WithEvents txtAuthorFileName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtAuthorContact As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtAuthorDescription As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAuthorName As TextBox
    Friend WithEvents btnOpenAuthor As Button
    Friend WithEvents btnSaveAuthor As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents txtCopyrightNotice As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtCopyrightPublicationYear As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtCopyrightOwnerName As TextBox
    Friend WithEvents btnOpenCopyright As Button
    Friend WithEvents btnSaveCopyright As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtCopyrightFileName As TextBox
    Friend WithEvents btnOpenLicense As Button
    Friend WithEvents btnSaveLicense As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents txtLicenseFileName As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtLicenseOwnerName As TextBox
    Friend WithEvents btnApplyAuthor As Button
    Friend WithEvents btnApplyCopyright As Button
    Friend WithEvents btnApplyLicense As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents txtAuthorSummary As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtCopyrightSummary As TextBox
    Friend WithEvents cmbLicense As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents txtLicensePublicationYear As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents txtLicenseType As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents txtLicenseName As TextBox
    Friend WithEvents rtbLicenseDescription As RichTextBox
    Friend WithEvents rtbLicenseNotice As RichTextBox
    Friend WithEvents rtbLicenseText As RichTextBox
    Friend WithEvents pbIconAuthor As PictureBox
    Friend WithEvents pbIconCopyright As PictureBox
    Friend WithEvents pbIconLicense As PictureBox
End Class
