Public Class frmDocumentView
    'This form is used to view and edit documents.

#Region " Variable Declarations - All the variables used in this form and this application." '=================================================================================================

    'Dim UpdateNeeded As Boolean 'NOTE: the document is now updated automatically whnever the focus has moved from the Document rich text box.

    Dim WithEvents Zip As ADVL_Utilities_Library_1.ZipComp

    'NOTE: The dictUpdateInfo and dictVersionInfo dictionaries are now replaced by the Update() Version() Author() Copyright() and License() dictionaries.
    'Private dictUpdateInfo As New Dictionary(Of String, UpdateInfo) 'Dictionary of information about each document update.
    'Private dictVersionInfo As New Dictionary(Of String, VersionInfo) 'Dictionary of information about each document version.

    'Private Update As New Dictionary(Of String, UpdateInfo)       'Dictionary of information about each document update.
    'Private Version As New Dictionary(Of String, VersionInfo)     'Dictionary of information about each document version.
    Private DocItem As New Dictionary(Of String, DocItemInfo)     'Dictionary if information about each document item (Update, Version, Note, ...)
    'NOTE: Author information is now included in the Update and Version dictionaries.
    'Private Author As New Dictionary(Of String, AuthorInfo)       'Dictionary if information about each document author.
    Private Copyright As New Dictionary(Of String, CopyrightInfo) 'Dictionary of information about each document copyright
    Private License As New Dictionary(Of String, LicenseInfo)     'Dictionary of information about each document license.

    Private StoredDocStructure As String = "" ' The name of the XML file specifying the structure of the document, as stored in the document. If the document has been renamed, this will need to be updated.

    Private FirstUpdateLoaded As Boolean = False 'Flag used to indicate if the first update node has been loaded (using the initial document icon). Update nodes loaded after this use the update icon.

    Public WithEvents LicenseInfo As frmLicense

#End Region 'Variable Declarations ------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Properties - All the properties used in this form and this application" '============================================================================================================

    'The FormNo property stores the number of the instance of this form.
    'This form can have multipe instances, which are stored in the CodeViewList ArrayList in the Main form.
    'When this form is closed, the FormNo is used to update the ClosedFormNo property of the Main form.
    'ClosedFormNo is then used by a method to set the corresponding form element in CodeViewList to Nothing.

    Private _formNo As Integer
    Public Property FormNo As Integer
        Get
            Return _formNo
        End Get
        Set(ByVal value As Integer)
            _formNo = value
        End Set
    End Property

    Private _docStructure As String = "" 'The name of the XML file specifying the structure of the document. This defines the tree of document updates and alternate versions.
    Property DocStructure As String
        Get
            Return _docStructure
        End Get
        Set(value As String)
            _docStructure = value
            txtStructureFileName.Text = _docStructure 'Updates and Versions tab \ Structure sub tab \ Document structure file groupbox - File name.
            'txtDocStrucName.Text = _docStructure 'Updates and Versions tab \ Structure sub tab \ Document structure name textbox.

            Dim XDocLib As New XDocument
            Main.Project.ReadXmlData(_docStructure, XDocLib)
            OpenDocStructure(XDocLib)
        End Set
    End Property

    Private _label As String = "" 'The Document Structure label.
    Property Label As String
        Get
            Return _label
        End Get
        Set(value As String)
            _label = value
            txtDocStrucLabel.Text = _label
        End Set
    End Property

    Private _description As String = "" 'A description of the document structure.
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
            txtDocStrucDescription.Text = _description 'Updates and Versions tab \ Structure sub tab \ Description taxt box
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the document structure. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the document structure. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property

    'Private _authorFile As String = "" 'The name of a .Auth file containing information about the author of the document.
    Private _defaultAuthorFile As String = "" 'The name of a .Auth file containing information about the author of the document.
    'NOTE:The document structure may contain one or more updates. Each update may contain one or more versions of the document.
    '      Each Update and Version can have different authors.
    'Property AuthorFile As String
    Property DefaultAuthorFile As String
        Get
            Return _defaultAuthorFile
        End Get
        Set(value As String)
            _defaultAuthorFile = value
        End Set
    End Property

    'Private _authorSummary As String = "" 'A summary of the document structure author. The full details are stored in the Author File.
    Private _defaultAuthorSummary As String = "" 'A summary of the document structure author. The full details are stored in the Author File.
    'Property AuthorSummary As String
    Property DefaultAuthorSummary As String
        Get
            Return _defaultAuthorSummary
        End Get
        Set(value As String)
            _defaultAuthorSummary = value
        End Set
    End Property

    'Private _copyrightFile As String = "" 'The name of a .Copy file containing the copyright notice.
    Private _defaultCopyrightFile As String = "" 'The name of a .Copy file containing the copyright notice.
    'Property CopyrightFile As String
    Property DefaultCopyrightFile As String
        Get
            Return _defaultCopyrightFile
        End Get
        Set(value As String)
            _defaultCopyrightFile = value
        End Set
    End Property

    'Private _copyrightSummary As String = "" 'A summary of the copyright. The full copyright details are stored in the Copyright file.
    Private _defaultCopyrightSummary As String = "" 'A summary of the copyright. The full copyright details are stored in the Copyright file.
    'Property CopyrightSummary As String
    Property DefaultCopyrightSummary As String
        Get
            Return _defaultCopyrightSummary
        End Get
        Set(value As String)
            _defaultCopyrightSummary = value
        End Set
    End Property

    'Private _licenseFile As String = "" 'The name of a .Lic file containing the software, data or media license.
    Private _defaultLicenseFile As String = "" 'The name of a .Lic file containing the software, data or media license.
    'Property LicenseFile As String
    Property DefaultLicenseFile As String
        Get
            Return _defaultLicenseFile
        End Get
        Set(value As String)
            _defaultLicenseFile = value
        End Set
    End Property

    'Private _licenseSummary As String = "" 'A summary of the license. The full license details are stored in the License file.
    Private _defaultLicenseSummary As String = "" 'A summary of the license. The full license details are stored in the License file.
    'Property LicenseSummary As String
    Property DefaultLicenseSummary As String
        Get
            Return _defaultLicenseSummary
        End Get
        Set(value As String)
            _defaultLicenseSummary = value
        End Set
    End Property



    'Private _selectedUpdate As String = "" 'The selected update node in the document structure.
    'Property SelectedUpdate As String
    '    Get
    '        Return _selectedUpdate
    '    End Get
    '    Set(value As String)
    '        _selectedUpdate = value
    '    End Set
    'End Property

    Private _defaultUpdate As String 'The default update node. This set of Document versions will be used when the document structure is accessed.
    Property DefaultUpdate As String
        Get
            Return _defaultUpdate
        End Get
        Set(value As String)
            _defaultUpdate = value
        End Set
    End Property

    'Private _selectedVersion As String = "" 'The selected document version.
    'Property SelectedVersion As String
    '    Get
    '        Return _selectedVersion
    '    End Get
    '    Set(value As String)
    '        _selectedVersion = value
    '    End Set
    'End Property

    Private _defaultVersion As String 'The default document version. This document will be used when the document structure is accessed.
    Property DefaultVersion As String
        Get
            Return _defaultVersion
        End Get
        Set(value As String)
            _defaultVersion = value
        End Set
    End Property

    Private _selectedNode As String = "" 'The selected node in the document structure.
    Property SelectedNode As String
        Get
            Return _selectedNode
        End Get
        Set(value As String)
            _selectedNode = value
            txtSelectedNode.Text = _selectedNode 'Updates and Versions tab \ Structure sub tab \ Selected Node
        End Set
    End Property

    Private _selectedDocumentFileName As String = "" 'The File Name of the selected document.
    Property SelectedDocumentFileName As String
        Get
            Return _selectedDocumentFileName
        End Get
        Set(value As String)
            _selectedDocumentFileName = value
            'txtDocFileName.Text = _selectedDocumentFileName
            txtDocumentFileName.Text = _selectedDocumentFileName
            txtFileName.Text = _selectedDocumentFileName
            OpenSelectedDocument()
        End Set
    End Property

#End Region 'Properties -----------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Process XML files - Read and write XML files." '=====================================================================================================================================


    Private Sub SaveFormSettings()
        'This SaveFormSettings method saves the settings in Main.SharePricesSettingsList

        'If FormNo + 1 > Main.CodeViewList.Count Then
        If FormNo + 1 > Main.DocViewList.Count Then
            Main.Message.AddWarning("Form number: " & FormNo & " does not exist in the Share Prices Settings List!" & vbCrLf)
        Else
            'Save the form settings:
            'Main.CodeViewSettings.List(FormNo).Left = Me.Left
            'Main.CodeViewSettings.List(FormNo).Top = Me.Top
            'Main.CodeViewSettings.List(FormNo).Width = Me.Width
            'Main.CodeViewSettings.List(FormNo).Height = Me.Height
            'Main.CodeViewSettings.List(FormNo).FileName = ""

            'Main.DocViewSettings.List(FormNo).Left = Me.Left
            'Main.DocViewSettings.List(FormNo).Top = Me.Top
            'Main.DocViewSettings.List(FormNo).Width = Me.Width
            'Main.DocViewSettings.List(FormNo).Height = Me.Height
            'Main.DocViewSettings.List(FormNo).FileName = ""

        End If
    End Sub

    Private Sub RestoreFormSettings()
        'This RestoreFormSettings method restores the settings from Main.SharePricesSettings.List

        ''If FormNo + 1 > Main.CodeViewSettings.List.Count Then
        'If FormNo + 1 > Main.DocViewSettings.List.Count Then
        '    'Main.Message.AddWarning("Form number: " & FormNo & " does not exist in the Share Prices Settings List!" & vbCrLf)
        '    'Add form entry to the Share Prices Settings list.
        '    'Dim NewSettings As New DataViewSettings
        '    Dim NewSettings As New DVSettings
        '    Main.DocViewSettings.InsertSettings(FormNo, NewSettings)
        'Else
        '    'Restore the form settings:
        '    Me.Left = Main.DocViewSettings.List(FormNo).Left
        '    Me.Top = Main.DocViewSettings.List(FormNo).Top
        '    Me.Width = Main.DocViewSettings.List(FormNo).Width
        '    Me.Height = Main.DocViewSettings.List(FormNo).Height

        '    'txtDocName.Text = Main.CodeViewSettings.List(FormNo).Name
        '    'txtDescription.Text = Main.CodeViewSettings.List(FormNo).Description
        '    'txtDocType.Text = Main.CodeViewSettings.List(FormNo).Type
        '    'txtLanguage.Text = Main.CodeViewSettings.List(FormNo).Language
        '    'txtAuthor_Old.Text = Main.CodeViewSettings.List(FormNo).Author
        '    'txtLicense_Old.Text = Main.CodeViewSettings.List(FormNo).License
        '    'txtUpdateName.Text = Main.CodeViewSettings.List(FormNo).UpdateName
        '    'txtUpdateNo.Text = Main.CodeViewSettings.List(FormNo).UpdateNo
        '    'txtUpdateDesc.Text = Main.CodeViewSettings.List(FormNo).UpdateDesc
        '    'txtVersionName.Text = Main.CodeViewSettings.List(FormNo).VersionName
        '    'txtVersionNo.Text = Main.CodeViewSettings.List(FormNo).VersionNo
        '    'txtVersionDesc.Text = Main.CodeViewSettings.List(FormNo).VersionDesc
        '    'txtFileName.Text = Main.CodeViewSettings.List(FormNo).FileName
        '    'txtDateCreated.Text = Main.CodeViewSettings.List(FormNo).CreationDate
        '    'txtLastModified.Text = Main.CodeViewSettings.List(FormNo).LastEditDate

        'End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message) 'Save the form settings before the form is minimised:
        If m.Msg = &H112 Then 'SysCommand
            If m.WParam.ToInt32 = &HF020 Then 'Form is being minimised
                SaveFormSettings()
            End If
        End If
        MyBase.WndProc(m)
    End Sub

#End Region 'Process XML Files ----------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Display Methods - Code used to display this form." '============================================================================================================================

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Initialization code executed after the form is loaded.

        'Note that the application that opens this form will also set the DocStrucutre property and run the SelectCurrentNode method after the form is opened.

        'Initially only the rtbDocument is shown and dgvDocument is hidden. If a To Do List, Modification List, Checklist etc is shown, then the rtbDocument is hidden and the dgvDocument is shown.
        dgvDocument.Hide()

        'Set up deleted items list:
        dgvDeletedItems.ColumnCount = 5
        dgvDeletedItems.Columns(0).HeaderText = "Node Text"
        dgvDeletedItems.Columns(1).HeaderText = "Node Key"
        dgvDeletedItems.Columns(2).HeaderText = "Parent Node"
        dgvDeletedItems.Columns(3).HeaderText = "Node Index"
        dgvDeletedItems.Columns(4).HeaderText = "Date Deleted"
        dgvDeletedItems.AllowUserToAddRows = False
        dgvDeletedItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect


        RestoreFormSettings()   'Restore the form settings

        'Set up trvLibrary tree view:
        'trvDocument.ImageList = Main.ImageList1
        'trvDocument.ImageList = ImageList1
        trvDocument.ImageList = Main.ImageList1

        'Add list of new item types:
        cmbNewDocumentItemType.Items.Add("Update")
        cmbNewDocumentItemType.Items.Add("Version")
        cmbNewDocumentItemType.Items.Add("  Book Section")
        cmbNewDocumentItemType.Items.Add("  Application Code")
        cmbNewDocumentItemType.Items.Add("  Application Form")
        cmbNewDocumentItemType.Items.Add("  Picture")
        cmbNewDocumentItemType.Items.Add("Note")
        cmbNewDocumentItemType.Items.Add("To Do List")
        cmbNewDocumentItemType.Items.Add("Process")
        cmbNewDocumentItemType.Items.Add("Progress")
        cmbNewDocumentItemType.Items.Add("Date-Time")
        cmbNewDocumentItemType.Items.Add("License")
        cmbNewDocumentItemType.Items.Add("  License Notice")
        cmbNewDocumentItemType.Items.Add("  License Text")
        cmbNewDocumentItemType.Items.Add("Author")
        cmbNewDocumentItemType.Items.Add("Quality Control")
        cmbNewDocumentItemType.Items.Add("Checklist")
        cmbNewDocumentItemType.Items.Add("Modification List")



        'pbIconOpenUpdate.Image = ImageList1.Images(2) 'Note: images 0 and 1 are Initial Document and Open Initial Document icons.
        'pbIconClosedUpdate.Image = ImageList1.Images(2)
        pbIconUpdate.Image = Main.ImageList1.Images(40)
        'pbIconOpenVersion.Image = ImageList1.Images(4)
        'pbIconClosedVersion.Image = ImageList1.Images(4)
        pbIconVersion.Image = Main.ImageList1.Images(42)
        'pbIconOpenNote.Image = ImageList1.Images(6)
        'pbIconClosedNote.Image = ImageList1.Images(6)
        pbIconNote.Image = Main.ImageList1.Images(26)
        'pbIconOpenProcess.Image = ImageList1.Images(8)
        'pbIconClosedProcess.Image = ImageList1.Images(8)
        pbIconProcess.Image = Main.ImageList1.Images(32)
        'pbIconClosedToDo.Image = ImageList1.Images(20)
        pbIconToDo.Image = Main.ImageList1.Images(44)

        'pbIconClosedDateTime.Image = ImageList1.Images(10)
        pbIconDateTime.Image = Main.ImageList1.Images(34)
        'pbIconClosedBookSection.Image = ImageList1.Images(12)
        pbIconBookSection.Image = Main.ImageList1.Images(6)
        'pbIconClosedCode.Image = ImageList1.Images(14)
        pbIconCode.Image = Main.ImageList1.Images(10)
        'pbIconClosedForm.Image = ImageList1.Images(16)
        pbIconForm.Image = Main.ImageList1.Images(12)
        'pbIconClosedPicture.Image = ImageList1.Images(18)
        pbIconPicture.Image = Main.ImageList1.Images(16)
        pbIconModList.Image = Main.ImageList1.Images(83)
        'pbIconLicense.Image = Main.ImageList1.Images(69)
        'pbIconLicNotice.Image = Main.ImageList1.Images(71)
        'pbIconLicText.Image = Main.ImageList1.Images(73)
        pbIconModList.Image = Main.ImageList1.Images(83)
        pbIconQC.Image = Main.ImageList1.Images(77)
        pbIconChecklist.Image = Main.ImageList1.Images(79)
        pbIconRtfDoc.Image = Main.ImageList1.Images(36)


        'pbIconProgress.Image = Progress.Images(10)
        pbIconProgress.Image = Main.ImageList1.Images(57)
        'pbIconProgress0.Image = Progress.Images(20)
        pbIconProgress0.Image = Main.ImageList1.Images(47)
        'pbIconProgress10.Image = Progress.Images(18)
        pbIconProgress10.Image = Main.ImageList1.Images(49)
        'pbIconProgress20.Image = Progress.Images(16)
        pbIconProgress20.Image = Main.ImageList1.Images(51)
        'pbIconProgress30.Image = Progress.Images(14)
        pbIconProgress30.Image = Main.ImageList1.Images(53)
        'pbIconProgress40.Image = Progress.Images(12)
        pbIconProgress40.Image = Main.ImageList1.Images(55)
        'pbIconProgress50.Image = Progress.Images(10)
        pbIconProgress50.Image = Main.ImageList1.Images(57)
        'pbIconProgress60.Image = Progress.Images(8)
        pbIconProgress60.Image = Main.ImageList1.Images(59)
        'pbIconProgress70.Image = Progress.Images(6)
        pbIconProgress70.Image = Main.ImageList1.Images(61)
        'pbIconProgress80.Image = Progress.Images(4)
        pbIconProgress80.Image = Main.ImageList1.Images(63)
        'pbIconProgress90.Image = Progress.Images(2)
        pbIconProgress90.Image = Main.ImageList1.Images(65)
        'pbIconProgress100.Image = Progress.Images(0)

        pbIconProgress100.Image = Main.ImageList1.Images(67)

        pbIconAuthor.Image = Main.ImageList1.Images(75)
        pbIconCopyright.Image = Main.ImageList1.Images(81)
        pbIconLicense.Image = Main.ImageList1.Images(69)

        'UpdateNeeded = False 'This will be set to True if any changes are made to the document structure.

        'NumericUpDown1.Increment

        'NOTE: DocStructure has a blank value until a document structure file is opened.
        'Read the DocumentSettings
        'If Main.dictDocDisplayInfo.ContainsKey(DocStructure) Then
        '    Me.Left = Main.dictDocDisplayInfo(DocStructure).Left
        '    Me.Top = Main.dictDocDisplayInfo(DocStructure).Top
        '    Me.Width = Main.dictDocDisplayInfo(DocStructure).Width
        '    Me.Height = Main.dictDocDisplayInfo(DocStructure).Height
        'End If

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit the Form
        Main.ClosedFormNo = FormNo 'The Main form property ClosedFormNo is set to this form number. This is used in the DocumentViewFormClosed method to select the correct form to set to nothing.

        'Save the DocumentSettings
        If Main.DocDisplayInfo.ContainsKey(DocStructure) Then
            Main.DocDisplayInfo(DocStructure).Left = Me.Left
            Main.DocDisplayInfo(DocStructure).Top = Me.Top
            Main.DocDisplayInfo(DocStructure).Width = Me.Width
            Main.DocDisplayInfo(DocStructure).Height = Me.Height
        Else
            Main.DocDisplayInfo.Add(DocStructure, New DocumentInfo)
            Main.DocDisplayInfo(DocStructure).Left = Me.Left
            Main.DocDisplayInfo(DocStructure).Top = Me.Top
            Main.DocDisplayInfo(DocStructure).Width = Me.Width
            Main.DocDisplayInfo(DocStructure).Height = Me.Height
        End If

        SaveStructureFile()

        Me.Close() 'Close the form
    End Sub

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If WindowState = FormWindowState.Normal Then
            SaveFormSettings()
        Else
            'Dont save settings if form is minimised.
        End If
    End Sub

    Private Sub Form_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Main.DocumentViewFormClosed()
    End Sub




#End Region 'Form Display Methods -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Open and Close Forms - Code used to open and close other forms." '===================================================================================================================


    Private Sub LicenseInfo_FormClosed(sender As Object, e As FormClosedEventArgs) Handles LicenseInfo.FormClosed
        LicenseInfo = Nothing
    End Sub

    Private Sub btnEditAuthor_Click(sender As Object, e As EventArgs) Handles btnEditAuthor.Click
        'Open the LicenseInfo form at the Author tab.

        If IsNothing(LicenseInfo) Then
            LicenseInfo = New frmLicense
            LicenseInfo.Show()
            LicenseInfo.CopyrightFile = txtCopyrightFile.Text
            LicenseInfo.LicenseFile = txtLicenseFile.Text
            LicenseInfo.AuthorFile = txtAuthorFile.Text 'Setting the Author file last leaves the Author tab open
            'LicenseInfo.OpenAuthorFile()
        Else
            LicenseInfo.Show()
            LicenseInfo.CopyrightFile = txtCopyrightFile.Text
            LicenseInfo.LicenseFile = txtLicenseFile.Text
            LicenseInfo.AuthorFile = txtAuthorFile.Text 'Setting the Author file last leaves the Author tab open
            'LicenseInfo.OpenAuthorFile()
        End If

    End Sub

    Private Sub btnEditCopyright_Click(sender As Object, e As EventArgs) Handles btnEditCopyright.Click
        'Open the LicenseInfo form at the Copyright tab.

        If IsNothing(LicenseInfo) Then
            LicenseInfo = New frmLicense
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtAuthorFile.Text
            LicenseInfo.LicenseFile = txtLicenseFile.Text
            LicenseInfo.CopyrightFile = txtCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
            'LicenseInfo.OpenCopyrightFile()
        Else
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtAuthorFile.Text
            LicenseInfo.LicenseFile = txtLicenseFile.Text
            LicenseInfo.CopyrightFile = txtCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
            'LicenseInfo.OpenCopyrightFile()
        End If

    End Sub

    Private Sub btnEditLicense_Click(sender As Object, e As EventArgs) Handles btnEditLicense.Click
        'Open the LicenseInfo form at the License tab.

        If IsNothing(LicenseInfo) Then
            LicenseInfo = New frmLicense
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtAuthorFile.Text
            LicenseInfo.CopyrightFile = txtCopyrightFile.Text
            LicenseInfo.LicenseFile = txtLicenseFile.Text 'Setting the License file last leaves the License tab open
            'LicenseInfo.OpenLicenseFile()
        Else
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtAuthorFile.Text
            LicenseInfo.CopyrightFile = txtCopyrightFile.Text
            LicenseInfo.LicenseFile = txtLicenseFile.Text 'Setting the License file last leaves the License tab open
            'LicenseInfo.OpenLicenseFile()
        End If

    End Sub



#End Region 'Open and Close Forms -------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region " Form Methods - The main actions performed by this form." '===========================================================================================================================

    Private Sub SetAsDefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetAsDefaultToolStripMenuItem.Click
        SetAsDefault()
    End Sub

    Private Sub btnOpenStructure_Click(sender As Object, e As EventArgs) Handles btnOpenStructure.Click
        'Open the Document Structure with the specified file name.
        'This version of the code uses the old System.Xml.XmlDocument vesion of an XDoc

        Select Case Main.Project.DataLocn.Type
            Case ADVL_Utilities_Library_1.FileLocation.Types.Directory
                'Select a Library file from the project directory:
                OpenFileDialog1.InitialDirectory = Main.Project.DataLocn.Path
                'OpenFileDialog1.Filter = "Section file | *.Sect"
                OpenFileDialog1.Filter = "Structure files | *.Sect; *.Code"
                OpenFileDialog1.FileName = ""
                If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                    Dim DataFileName As String = System.IO.Path.GetFileName(OpenFileDialog1.FileName)
                    txtStructureFileName.Text = DataFileName
                    'SelectedDocumentFileName = DataFileName 'ERROR: DocStructure holds the name of the XML file containing the document structure!
                    DocStructure = DataFileName
                    'OpenLibrary(DataFileName)
                    Dim XDocLib As XDocument
                    Main.Project.ReadXmlData(DataFileName, XDocLib)
                    'OpenLibrary(XDocLib)
                    OpenDocStructure(XDocLib)
                End If
            Case ADVL_Utilities_Library_1.FileLocation.Types.Archive
                'Select a Library file from the project archive:
                'Show the zip archive file selection form:
                Zip = New ADVL_Utilities_Library_1.ZipComp
                Zip.ArchivePath = Main.Project.DataLocn.Path
                Zip.SelectFile() 'Show the SelectFile form.
                Zip.SelectFileForm.ApplicationName = Main.Project.ApplicationName
                Zip.SelectFileForm.SettingsLocn = Main.Project.SettingsLocn
                Zip.SelectFileForm.Show()
                Zip.SelectFileForm.RestoreFormSettings()
                'Zip.SelectFileForm.FileExtension = ".Libr"
                Zip.SelectFileForm.FileExtension = ".Sect"
                Zip.SelectFileForm.GetFileList()
                If Zip.SelectedFile <> "" Then
                    'A file has been selected
                    'CopyDataSettingsFile = Zip.SelectedFile
                    'txtCopyDataSettings.Text = Zip.SelectedFile
                    'LoadCopyColumnsSettingsFile()
                    'LoadCopyDataSettingsFile()
                    Dim DataFileName As String = Zip.SelectedFile
                    txtStructureFileName.Text = DataFileName
                    SelectedDocumentFileName = DataFileName 'ERROR: DocStructure holds the name of the XML file containing the document structure!
                    DocStructure = DataFileName
                    Dim XDocLib As XDocument
                    Main.Project.ReadXmlData(DataFileName, XDocLib)
                    'OpenLibrary(XDocLib)
                    OpenDocStructure(XDocLib)
                End If
        End Select
    End Sub

    Private Sub OpenDocStructure(ByVal myXDoc As XDocument)
        'Open the Document Structure stored in the XDocument

        trvDocument.Nodes.Clear() 'Clear the nodes in the Document Structure TreeView
        'dictUpdateInfo.Clear() 'Clear the Update information dictionary
        'dictVersionInfo.Clear() 'Clear the version information dictionary
        'Update.Clear()
        'Version.Clear()
        DocItem.Clear() 'This now includes information about the following node types: Update, Version, Note, ...
        'Author.Clear() 'Author information is now included in the Update and Version dictionaries.
        Copyright.Clear()
        License.Clear()


        If myXDoc Is Nothing Then
            Main.Message.AddWarning("Cannot open Document Structure - XDocument is blank." & vbCrLf)
            Exit Sub
        End If

        Dim I As Integer

        'Need to convert the XDocument to an XmlDocument:
        Dim XDoc As New System.Xml.XmlDocument

        XDoc.LoadXml(myXDoc.ToString)

        Dim node As System.Xml.XmlNode
        node = XDoc.DocumentElement
        Main.Message.Add("Reading Document structure file. Node = " & node.Name & vbCrLf)

        FirstUpdateLoaded = False 'This indicates that the first update node has not been loaded.
        StoredDocStructure = node.Name 'This is the name of the document structure XML file as stored in the file. If the file has been renamed, this will be updated later.

        'Add the root node to the Document Structure TreeView:
        'dictItemInfo.Add(DocStructure, New ItemInfo) 'Add a blank entry to the Item Info dictionary. The Description, CreationDate and LastEditDate fields will be updated later.
        'NOTE: The DocStructure properties are stored in the DocumentView form properties. (There is only one DocStructure - a dictionary containing multiple entries is not needed.)
        'dictVersionInfo.Add(DocStructure, New VersionInfo) 'Add a blank entry to the Item Info dictionary. The Description, CreationDate and LastEditDate fields will be updated later.
        Dim new_Node As TreeNode
        If DocStructure.EndsWith(".Sect") Then 'Book section node.
            new_Node = trvDocument.Nodes.Add(DocStructure, "", 6, 7) 'Key, Text, ImageIndex, SelectedImageIndex (Node text will be added later when it is read from the Text child node.)
            'new_Node.ContextMenuStrip = ContextMenuStrip1
        ElseIf DocStructure.EndsWith(".Code") Then 'Application Code node.
            new_Node = trvDocument.Nodes.Add(DocStructure, "", 10, 11) 'Key, Text, ImageIndex, SelectedImageIndex (Node text will be added later when it is read from the Text child node.)
        Else
            Main.Message.AddWarning("Document structure type not recognised: " & DocStructure & vbCrLf)
        End If

        'ProcessStrucChildNode(node, trvDocument.Nodes, "")
        ProcessStrucChildNode(node, new_Node.Nodes, "")

        'Update the Document Structure Name, Selected Node and Description:
        'txtDocumentStrucName.Text = DocStructure
        'NOTE: these are now updated when the property is updated:
        'txtDocStrucName.Text = DocStructure 'The Document Structure file name.
        'txtSelectedNode.Text = SelectedNode
        'NOTE: this is now stored in the Description property of the Document View form:
        'If dictVersionInfo.ContainsKey(DocStructure) Then
        '    txtDocStrucDescription.Text = dictVersionInfo(DocStructure).Description
        'End If

        'Display the name of the document.
        Dim myRootNode() As TreeNode = trvDocument.Nodes.Find(DocStructure, False)
        If myRootNode.Count = 1 Then
            'txtDocStrucLabel.Text = myRootNode(0).Text
            Label = myRootNode(0).Text
        End If


        'Restore the saved form settings:
        If Main.DocDisplayInfo.ContainsKey(DocStructure) Then
            Me.Left = Main.DocDisplayInfo(DocStructure).Left
            Me.Top = Main.DocDisplayInfo(DocStructure).Top
            Me.Width = Main.DocDisplayInfo(DocStructure).Width
            Me.Height = Main.DocDisplayInfo(DocStructure).Height
        End If

    End Sub

    Private Sub ProcessStrucChildNode(ByVal xml_Node As System.Xml.XmlNode, ByVal tnc As TreeNodeCollection, ByVal Spaces As String)
        'Process child nodes and add spcified nodes to the TreeView.
        '(Used to read the .Libr library file.)

        Dim ParentNodeName As String = xml_Node.Name
        If ParentNodeName = StoredDocStructure Then 'This is the root node.
            If StoredDocStructure <> DocStructure Then 'The XML file used to store the document structure has been renamed.
                ParentNodeName = DocStructure 'Change the name of the node to the new name.
            End If
        End If

        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            'Main.Message.Add("Processing ChildNode: " & ChildNode.Name & vbCrLf)
            If ChildNode.Name.EndsWith(".Sect") Then 'Book section node.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.
                'Version.Add(ChildNode.Name, New VersionInfo)
                DocItem.Add(ChildNode.Name, New DocItemInfo)
                'Author.Add(ChildNode.Name, New AuthorInfo) 'Author information is now included in the Update and Version dictionaries.
                Copyright.Add(ChildNode.Name, New CopyrightInfo)
                License.Add(ChildNode.Name, New LicenseInfo)
                Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 6, 7) 'Image 12 is a message icon, Image 13 is an open message icon.
                new_Node.EnsureVisible()
                ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)

            ElseIf ChildNode.Name.EndsWith(".Update") Then 'Update node. (This will contain one or more versions of the document.)
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.
                'dictUpdateInfo.Add(ChildNode.Name, New UpdateInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.
                'Update.Add(ChildNode.Name, New UpdateInfo)
                DocItem.Add(ChildNode.Name, New DocItemInfo)
                'Author.Add(ChildNode.Name, New AuthorInfo) 'Author information is now included in the Update and Version dictionaries.
                Copyright.Add(ChildNode.Name, New CopyrightInfo)
                License.Add(ChildNode.Name, New LicenseInfo)
                If FirstUpdateLoaded = False Then 'This is the first update node - use the initial document icon.
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 38, 39) 'Image 0 is the initial document icon, Image 1 is the open initial document icon.
                    new_Node.EnsureVisible()
                    FirstUpdateLoaded = True
                    ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)
                Else 'This is not the first update node - use the update icon.
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 40, 41) 'Image 2 is the update icon, Image 3 is the open update icon.
                    new_Node.EnsureVisible()
                    ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)
                End If

            ElseIf ChildNode.Name.EndsWith(".rtf") Then 'This is the name of a rich text format file containing the document.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.
                'Version.Add(ChildNode.Name, New VersionInfo)
                DocItem.Add(ChildNode.Name, New DocItemInfo)
                'Author.Add(ChildNode.Name, New AuthorInfo) 'Author information is now included in the Update and Version dictionaries.
                Copyright.Add(ChildNode.Name, New CopyrightInfo)
                License.Add(ChildNode.Name, New LicenseInfo)
                Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 36, 37)
                new_Node.ContextMenuStrip = ContextMenuStrip1
                new_Node.EnsureVisible()
                ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)

            ElseIf ChildNode.Name.EndsWith(".Coll") Then 'Collection node.
                'NOTE: Collection node should not be in a Document Structure!?
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.


            ElseIf ChildNode.Name.EndsWith(".Book") Then 'Book node. (A document structure should not contain a Book node!)
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.

            ElseIf ChildNode.Name.EndsWith(".Appl") Then 'Application node. (A document structure should not contain a Application node!)
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.

            ElseIf ChildNode.Name.EndsWith(".Code") Then 'Code node. This is the root node for a Code document structure.
                'NOTE: Properties in the Document View form are used to store information about the document structure.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.
                Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 10, 11) 'Image 14 is a code icon, Image 15 is an open code icon.
                new_Node.EnsureVisible()
                ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)

            ElseIf ChildNode.Name.EndsWith(".Form") Then 'Form node. This is the root node for a Form document structure.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.

            ElseIf ChildNode.Name.EndsWith(".Pics") Then 'Picture Album node. (A document Structure should Not contain a Picture Album node!)
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.

            ElseIf ChildNode.Name.EndsWith(".Pict") Then 'Picture node. This is the root node for a Picture document structure.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.

            ElseIf ChildNode.Name.EndsWith(".Note") Then 'Note node. This is used for notes on the document update or version.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.
                DocItem.Add(ChildNode.Name, New DocItemInfo)
                'Copyright.Add(ChildNode.Name, New CopyrightInfo)
                'License.Add(ChildNode.Name, New LicenseInfo)
                Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 26, 27)
                new_Node.EnsureVisible()
                ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)

            ElseIf ChildNode.Name.EndsWith(".ModList") Then 'Note node. This contains a modification list for the document update or version.
                DocItem.Add(ChildNode.Name, New DocItemInfo)
                'Copyright.Add(ChildNode.Name, New CopyrightInfo)
                'License.Add(ChildNode.Name, New LicenseInfo)
                Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 83, 84)
                new_Node.EnsureVisible()
                ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)

            ElseIf ChildNode.Name.EndsWith(".ToDoList") Then 'Note node. This contains a To Do list for the document update or version.
                DocItem.Add(ChildNode.Name, New DocItemInfo)
                'Copyright.Add(ChildNode.Name, New CopyrightInfo)
                'License.Add(ChildNode.Name, New LicenseInfo)
                Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, "", 44, 45)
                new_Node.EnsureVisible()
                ProcessStrucChildNode(ChildNode, new_Node.Nodes, Spaces)


            ElseIf ChildNode.Name.EndsWith(".Proc") Then 'Process node. This node stores a process that can be applied to one or more documents in the structure.
                'dictVersionInfo.Add(ChildNode.Name, New VersionInfo) 'Add a new blank entry to the ItemInfo dictionary cwith the node name as the key.

            ElseIf ChildNode.Name = "Text" Then
                'This is the node text.
                Dim myNodeText As String = ChildNode.InnerText
                'The node can be updated with the node text:
                'First find the corresponding node:
                Dim myNode() As TreeNode = trvDocument.Nodes.Find(ParentNodeName, True)
                If myNode.Length = 1 Then
                    myNode(0).Text = myNodeText 'Update the node text
                ElseIf myNode.Length = 0 Then
                    Main.Message.AddWarning("Tree node not found: " & ParentNodeName & vbCrLf)
                ElseIf myNode.Length > 1 Then
                    Main.Message.AddWarning("More than one tree nodes found: " & ParentNodeName & vbCrLf)
                End If

            ElseIf ChildNode.Name = "Description" Then
                Dim myNodeDesc As String = ChildNode.InnerText
                'This is the node description
                If ParentNodeName = DocStructure Then 'This is the root node
                    Description = myNodeDesc
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).Description = myNodeDesc
                    '    'Update(ParentNodeName).Description = myNodeDesc
                    '    DocItem(ParentNodeName).Description = myNodeDesc
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).Description = myNodeDesc
                    'Version(ParentNodeName).Description = myNodeDesc
                    DocItem(ParentNodeName).Description = myNodeDesc
                End If

            ElseIf ChildNode.Name = "CreationDate" Then
                Dim myNodeCreationDate As String = ChildNode.InnerText
                'This is the node creation date
                If ParentNodeName = DocStructure Then 'This is the root node
                    CreationDate = myNodeCreationDate
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    ' dictUpdateInfo(ParentNodeName).CreationDate = myNodeCreationDate
                    '    'Update(ParentNodeName).CreationDate = myNodeCreationDate
                    '    DocItem(ParentNodeName).CreationDate = myNodeCreationDate
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).CreationDate = myNodeCreationDate
                    'Version(ParentNodeName).CreationDate = myNodeCreationDate
                    DocItem(ParentNodeName).CreationDate = myNodeCreationDate
                End If


            ElseIf ChildNode.Name = "LastEditDate" Then
                Dim myNodeLastEditDate As String = ChildNode.InnerText
                'This is the node last edit date
                If ParentNodeName = DocStructure Then 'This is the root node
                    LastEditDate = myNodeLastEditDate
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).LastEditDate = myNodeLastEditDate
                    '    'Update(ParentNodeName).LastEditDate = myNodeLastEditDate
                    '    DocItem(ParentNodeName).LastEditDate = myNodeLastEditDate
                Else
                    'Update the version dictionary entry:
                    ' dictVersionInfo(ParentNodeName).LastEditDate = myNodeLastEditDate
                    'Version(ParentNodeName).LastEditDate = myNodeLastEditDate
                    DocItem(ParentNodeName).LastEditDate = myNodeLastEditDate
                End If

            ElseIf ChildNode.Name = "AuthorFile" Then
                Dim myNodeAuthorFile As String = ChildNode.InnerText
                'This is the node Author File containing the author details.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultAuthorFile = myNodeAuthorFile
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).AuthorFile = myNodeAuthorFile
                    '    'Author(ParentNodeName).File = myNodeAuthorFile
                    '    'Update(ParentNodeName).AuthorFile = myNodeAuthorFile
                    '    DocItem(ParentNodeName).AuthorFile = myNodeAuthorFile
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).AuthorFile = myNodeAuthorFile
                    'Author(ParentNodeName).File = myNodeAuthorFile
                    'Version(ParentNodeName).AuthorFile = myNodeAuthorFile
                    DocItem(ParentNodeName).AuthorFile = myNodeAuthorFile
                End If

            ElseIf ChildNode.Name = "DefaultAuthorFile" Then
                Dim myNodeAuthorFile As String = ChildNode.InnerText
                'This is the node Author File containing the author details.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultAuthorFile = myNodeAuthorFile
                Else
                    DocItem(ParentNodeName).AuthorFile = myNodeAuthorFile
                End If

            ElseIf ChildNode.Name = "AuthorSummary" Then
                Dim myNodeAuthorSummary As String = ChildNode.InnerText
                'This is the node Author Summary.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultAuthorSummary = myNodeAuthorSummary
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                    '    'Author(ParentNodeName).Summary = myNodeAuthorSummary
                    '    'Update(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                    '    DocItem(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                    'Author(ParentNodeName).Summary = myNodeAuthorSummary
                    'Version(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                    DocItem(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                End If

            ElseIf ChildNode.Name = "DefaultAuthorSummary" Then
                Dim myNodeAuthorSummary As String = ChildNode.InnerText
                'This is the node Author Summary.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultAuthorSummary = myNodeAuthorSummary
                Else
                    'Update the version dictionary entry:
                    DocItem(ParentNodeName).AuthorSummary = myNodeAuthorSummary
                End If
                'NOTE: Author information is stores in the DocItem dictionary, which can include Note, Checklist etc items.
                '  Copyright and License information is not stores in the DocItem dictionary because some items (Note, Checklist etc) do not have copyright or license info. THESE MAY BE ADDED LATER.

            ElseIf ChildNode.Name = "CopyrightFile" Then
                Dim myNodeCopyrightFile As String = ChildNode.InnerText
                'This is the node Copyright File containing the copyright details.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultCopyrightFile = myNodeCopyrightFile
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).CopyrightFile = myNodeCopyrightFile
                    '    Copyright(ParentNodeName).File = myNodeCopyrightFile
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).CopyrightFile = myNodeCopyrightFile
                    Copyright(ParentNodeName).File = myNodeCopyrightFile
                End If

            ElseIf ChildNode.Name = "DefaultCopyrightFile" Then
                Dim myNodeCopyrightFile As String = ChildNode.InnerText
                'This is the node Copyright File containing the copyright details.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultCopyrightFile = myNodeCopyrightFile
                Else
                    'Update the version dictionary entry:
                    Copyright(ParentNodeName).File = myNodeCopyrightFile
                End If

            ElseIf ChildNode.Name = "CopyrightSummary" Then
                Dim myNodeCopyrightSummary As String = ChildNode.InnerText
                'This is the node Copyright Summary.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultCopyrightSummary = myNodeCopyrightSummary
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).CopyrightSummary = myNodeCopyrightSummary
                    '    Copyright(ParentNodeName).Summary = myNodeCopyrightSummary
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).CopyrightSummary = myNodeCopyrightSummary
                    Copyright(ParentNodeName).Summary = myNodeCopyrightSummary
                End If

            ElseIf ChildNode.Name = "DefaultCopyrightSummary" Then
                Dim myNodeCopyrightSummary As String = ChildNode.InnerText
                'This is the node Copyright Summary.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultCopyrightSummary = myNodeCopyrightSummary
                Else
                    'Update the version dictionary entry:
                    Copyright(ParentNodeName).Summary = myNodeCopyrightSummary
                End If


            ElseIf ChildNode.Name = "LicenseFile" Then
                Dim myNodeLicenseFile As String = ChildNode.InnerText
                'This is the node License File containing the license details.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultLicenseFile = myNodeLicenseFile
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).LicenseFile = myNodeLicenseFile
                    '    License(ParentNodeName).File = myNodeLicenseFile
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).LicenseFile = myNodeLicenseFile
                    License(ParentNodeName).File = myNodeLicenseFile
                End If

            ElseIf ChildNode.Name = "DefaultLicenseFile" Then
                Dim myNodeLicenseFile As String = ChildNode.InnerText
                'This is the node License File containing the license details.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultLicenseFile = myNodeLicenseFile
                Else
                    'Update the version dictionary entry:
                    License(ParentNodeName).File = myNodeLicenseFile
                End If

            ElseIf ChildNode.Name = "LicenseSummary" Then
                Dim myNodeLicenseSummary As String = ChildNode.InnerText
                'This is the node License Summary.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultLicenseSummary = myNodeLicenseSummary
                    'ElseIf ParentNodeName.EndsWith(".Update") Then
                    '    'Update the update dictionary entry:
                    '    'dictUpdateInfo(ParentNodeName).LicenseSummary = myNodeLicenseSummary
                    '    License(ParentNodeName).Summary = myNodeLicenseSummary
                Else
                    'Update the version dictionary entry:
                    'dictVersionInfo(ParentNodeName).LicenseSummary = myNodeLicenseSummary
                    License(ParentNodeName).Summary = myNodeLicenseSummary
                End If

            ElseIf ChildNode.Name = "DefaultLicenseSummary" Then
                Dim myNodeLicenseSummary As String = ChildNode.InnerText
                'This is the node License Summary.
                If ParentNodeName = DocStructure Then 'This is the root node
                    DefaultLicenseSummary = myNodeLicenseSummary
                Else
                    'Update the version dictionary entry:
                    License(ParentNodeName).Summary = myNodeLicenseSummary
                End If

            ElseIf ChildNode.Name = "SelectedNode" Then
                'This is the selected node
                SelectedNode = ChildNode.InnerText 'Update the property

            ElseIf ChildNode.Name = "#comment" Then
                'This is a comment line.
                'Ignore comments.

            ElseIf ChildNode.Name = "DeletedNodes" Then
                ProcessDeletedNodesList(ChildNode)
            Else
                Main.Message.AddWarning("Unknown node name: " & ChildNode.Name & vbCrLf)
            End If
        Next

    End Sub

    Private Sub ProcessDeletedNodesList(ByVal xml_Node As System.Xml.XmlNode)
        'Process each of the Deleted Nodes records.

        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            If ChildNode.Name = "Node" Then
                Dim nodeText As System.Xml.XmlNode
                nodeText = ChildNode.SelectSingleNode("Text")
                Dim nodeKey As System.Xml.XmlNode
                nodeKey = ChildNode.SelectSingleNode("Key")
                Dim NodeKeyString As String = nodeKey.InnerText 'Used later when restoring DocItem, Author and License settings.
                Dim nodeIndex As System.Xml.XmlNode
                nodeIndex = ChildNode.SelectSingleNode("Index")
                Dim nodeParent As System.Xml.XmlNode
                nodeParent = ChildNode.SelectSingleNode("Parent")
                Dim delDate As System.Xml.XmlNode
                delDate = ChildNode.SelectSingleNode("DeletedDate")
                'dgvDeletedItems.Rows.Add(nodeText.InnerText, nodeKey.InnerText, nodeParent.InnerText, nodeIndex.InnerText)
                If delDate Is Nothing Then
                    dgvDeletedItems.Rows.Add(nodeText.InnerText, nodeKey.InnerText, nodeParent.InnerText, nodeIndex.InnerText, "")
                Else
                    dgvDeletedItems.Rows.Add(nodeText.InnerText, nodeKey.InnerText, nodeParent.InnerText, nodeIndex.InnerText, delDate.InnerText)
                End If

                'Restore any DocItem settings:
                Dim ItemDescription As System.Xml.XmlNode
                ItemDescription = ChildNode.SelectSingleNode("ItemDescription")
                If ItemDescription Is Nothing Then
                    'No Description available.
                Else
                    If DocItem.ContainsKey(NodeKeyString) Then
                        DocItem(NodeKeyString).Description = ItemDescription.InnerText
                    Else
                        DocItem.Add(NodeKeyString, New DocItemInfo)
                        DocItem(NodeKeyString).Description = ItemDescription.InnerText
                    End If
                End If
                Dim ItemCreationDate As System.Xml.XmlNode
                ItemCreationDate = ChildNode.SelectSingleNode("ItemCreationDate")
                If ItemCreationDate Is Nothing Then
                    'No Creation Date available.
                Else
                    If DocItem.ContainsKey(NodeKeyString) Then
                        DocItem(NodeKeyString).CreationDate = ItemCreationDate.InnerText
                    Else
                        DocItem.Add(NodeKeyString, New DocItemInfo)
                        DocItem(NodeKeyString).CreationDate = ItemCreationDate.InnerText
                    End If
                End If
                Dim ItemLastEditDate As System.Xml.XmlNode
                ItemLastEditDate = ChildNode.SelectSingleNode("ItemLastEditDate")
                If ItemLastEditDate Is Nothing Then
                    'No Last Edit Date available.
                Else
                    If DocItem.ContainsKey(NodeKeyString) Then
                        DocItem(NodeKeyString).LastEditDate = ItemLastEditDate.InnerText
                    Else
                        DocItem.Add(NodeKeyString, New DocItemInfo)
                        DocItem(NodeKeyString).LastEditDate = ItemLastEditDate.InnerText
                    End If
                End If
                Dim ItemAuthorFile As System.Xml.XmlNode
                ItemAuthorFile = ChildNode.SelectSingleNode("ItemAuthorFile")
                If ItemAuthorFile Is Nothing Then
                    'No Author File available.
                Else
                    If DocItem.ContainsKey(NodeKeyString) Then
                        DocItem(NodeKeyString).AuthorFile = ItemAuthorFile.InnerText
                    Else
                        DocItem.Add(NodeKeyString, New DocItemInfo)
                        DocItem(NodeKeyString).AuthorFile = ItemAuthorFile.InnerText
                    End If
                End If
                Dim ItemAuthorSummary As System.Xml.XmlNode
                ItemAuthorSummary = ChildNode.SelectSingleNode("ItemAuthorSummary")
                If ItemAuthorSummary Is Nothing Then
                    'No Authro Summary available.
                Else
                    If DocItem.ContainsKey(NodeKeyString) Then
                        DocItem(NodeKeyString).AuthorSummary = ItemAuthorSummary.InnerText
                    Else
                        DocItem.Add(NodeKeyString, New DocItemInfo)
                        DocItem(NodeKeyString).AuthorSummary = ItemAuthorSummary.InnerText
                    End If
                End If

                'Restore any Copyright settings:
                Dim CopyrightFile As System.Xml.XmlNode
                CopyrightFile = ChildNode.SelectSingleNode("CopyrightFile")
                If CopyrightFile Is Nothing Then
                    'No Copyright File available.
                Else
                    If Copyright.ContainsKey(NodeKeyString) Then
                        Copyright(NodeKeyString).File = CopyrightFile.InnerText
                    Else
                        Copyright.Add(NodeKeyString, New CopyrightInfo)
                        Copyright(NodeKeyString).File = CopyrightFile.InnerText
                    End If
                End If
                Dim CopyrightSummary As System.Xml.XmlNode
                CopyrightSummary = ChildNode.SelectSingleNode("CopyrightSummary")
                If CopyrightSummary Is Nothing Then
                    'No Copyright Summary available.
                Else
                    If Copyright.ContainsKey(NodeKeyString) Then
                        Copyright(NodeKeyString).Summary = CopyrightSummary.InnerText
                    Else
                        Copyright.Add(NodeKeyString, New CopyrightInfo)
                        Copyright(NodeKeyString).Summary = CopyrightSummary.InnerText
                    End If
                End If

                'Restore any License settings:
                Dim LicenseFile As System.Xml.XmlNode
                LicenseFile = ChildNode.SelectSingleNode("LicenseFile")
                If LicenseFile Is Nothing Then
                    'No License File available.
                Else
                    If License.ContainsKey(NodeKeyString) Then
                        License(NodeKeyString).File = LicenseFile.InnerText
                    Else
                        License.Add(NodeKeyString, New LicenseInfo)
                        License(NodeKeyString).File = LicenseFile.InnerText
                    End If
                End If
                Dim LicenseSummary As System.Xml.XmlNode
                LicenseSummary = ChildNode.SelectSingleNode("LicenseSummary")
                If LicenseSummary Is Nothing Then
                    'No License Summary available.
                Else
                    If License.ContainsKey(NodeKeyString) Then
                        License(NodeKeyString).Summary = LicenseSummary.InnerText
                    Else
                        License.Add(NodeKeyString, New LicenseInfo)
                        License(NodeKeyString).Summary = LicenseSummary.InnerText
                    End If
                End If

            Else
                'Unknown Node name.
                Main.Message.AddWarning("Unknown DeletedNodes node name: " & ChildNode.Name & vbCrLf)
            End If
        Next

        dgvDeletedItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvDeletedItems.AutoResizeColumns()

    End Sub

    Private Sub SelectNode(ByVal NodeKey As String)
        'Find the node with the specified Node Key.

        If NodeKey = "" Then
            Main.Message.AddWarning("Cannot find the node in the Document View. Blank node key specified." & vbCrLf)
            Exit Sub
        End If

        Dim myNode() As TreeNode = trvDocument.Nodes.Find(NodeKey, True)

        If myNode.Length > 0 Then
            trvDocument.SelectedNode = myNode(0)
            trvDocument.Focus()
        Else
            Main.Message.AddWarning("Node key not found: " & NodeKey & vbCrLf)
        End If
    End Sub

    Public Sub SelectCurrentNode()
        'Select the node specified in SelectedNode property.

        If SelectedNode = "" Then
            Main.Message.AddWarning("Cannot find the node in the Document View. SelectedNode is blank." & vbCrLf)
        Else
            SelectNode(SelectedNode)
        End If
    End Sub

    Private Sub trvDocument_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles trvDocument.AfterSelect
        'A node has been selected on the TreeView.
        'Update the display.

        txtNodePath.Text = e.Node.FullPath
        txtNodeText.Text = e.Node.Text
        txtNodeKey.Text = e.Node.Name
        If e.Node.Parent Is Nothing Then 'Root node
            Main.Message.Add("Root node." & e.Node.Name & vbCrLf)
            txtParentNodeName.Text = ""
            txtNodeDescription.Text = Description
            txtCreationDate.Text = CreationDate
            txtLastEditDate.Text = LastEditDate
            txtAuthorFile.Text = DefaultAuthorFile
            txtAuthorSummary.Text = DefaultAuthorSummary
            txtCopyrightFile.Text = DefaultCopyrightFile
            txtCopyrightSummary.Text = DefaultCopyrightSummary
            txtLicenseFile.Text = DefaultLicenseFile
            rtbLicenseNotice.Text = DefaultLicenseSummary
        Else
            txtParentNodeName.Text = e.Node.Parent.Name
            If DocItem.ContainsKey(e.Node.Name) Then
                txtNodeDescription.Text = DocItem(e.Node.Name).Description
                txtCreationDate.Text = DocItem(e.Node.Name).CreationDate
                txtLastEditDate.Text = DocItem(e.Node.Name).LastEditDate
                txtAuthorFile.Text = DocItem(e.Node.Name).AuthorFile
                txtAuthorSummary.Text = DocItem(e.Node.Name).AuthorSummary
            Else
                txtNodeDescription.Text = ""
                txtCreationDate.Text = Format(Now, "d-MMM-yyyy H:mm:ss")
                txtLastEditDate.Text = Format(Now, "d-MMM-yyyy H:mm:ss")
                txtAuthorFile.Text = ""
                txtAuthorSummary.Text = ""
            End If

            If Copyright.ContainsKey(e.Node.Name) Then
                txtCopyrightFile.Text = Copyright(e.Node.Name).File
                txtCopyrightSummary.Text = Copyright(e.Node.Name).Summary
            Else
                txtCopyrightFile.Text = ""
                txtCopyrightSummary.Text = ""
            End If

            If License.ContainsKey(e.Node.Name) Then
                txtLicenseFile.Text = License(e.Node.Name).File
                rtbLicenseNotice.Text = License(e.Node.Name).Summary
            Else
                txtLicenseFile.Text = ""
                rtbLicenseNotice.Text = ""
            End If
        End If

        txtNodeNumber.Text = e.Node.Index

        'If e.Node.Name.EndsWith(".rtf") Then
        '    If SelectedDocumentFileName = "" Then 'There is no node selected. 'OK to select this node.
        '        SelectedDocumentFileName = e.Node.Name
        '    ElseIf SelectedDocumentFileName = e.Node.Name Then 'This node is already selected.
        '        'Document file already selected.
        '        'ElseIf UpdateNeeded = True Then 'Send a message to save the current node. The selected node cannot be changed until the current node is saved.
        '        '    Main.Message.AddWarning("Save the current document!" & vbCrLf)
        '    Else 'Change the node selection.
        '        SelectedDocumentFileName = e.Node.Name
        '    End If
        'ElseIf e.Node.Name.EndsWith(".Note") Then
        '    If Main.Project.DataFileExists(e.Node.Name) Then
        '        Dim rtbData As New IO.MemoryStream
        '        rtbData.Position = 0
        '        Main.Project.ReadData(e.Node.Name, rtbData)
        '        rtbDocument.LoadFile(rtbData, RichTextBoxStreamType.RichText)
        '        'http://condor.depaul.edu/sjost/it236/documents/colorNames.htm
        '        rtbDocument.BackColor = Color.LemonChiffon
        '    Else
        '        Main.Message.Add("Document file does not exist: " & e.Node.Name & vbCrLf)
        '        Main.Message.Add("The document file will be created when the new text is saved." & vbCrLf)
        '        rtbDocument.Clear()
        '        'http://condor.depaul.edu/sjost/it236/documents/colorNames.htm
        '        rtbDocument.BackColor = Color.LemonChiffon
        '    End If
        'End If

        If e.Node.Name.EndsWith(".rtf") Then 'This is a document that can be displayed in the Document rich text box.
            SelectedDocumentFileName = e.Node.Name 'NOTE: Setting the SelectedDocumentFileName triggers the document display code: OpenSelectedDocument.
            txtDocLabel.Text = e.Node.Text
        ElseIf e.Node.Name.EndsWith(".Note") Then 'This is a document that can be displayed in the Document rich text box.
            SelectedDocumentFileName = e.Node.Name
            txtDocLabel.Text = e.Node.Text
        ElseIf e.Node.Name.EndsWith(".ModList") Then 'This is a document that can be displayed in the Document data grid view.
            SelectedDocumentFileName = e.Node.Name
            txtDocLabel.Text = e.Node.Text
        ElseIf e.Node.Name.EndsWith(".ToDoList") Then 'This is a document that can be displayed in the Document data grid view.
            SelectedDocumentFileName = e.Node.Name
            txtDocLabel.Text = e.Node.Text
        End If

        'Update New Item options:
        If e.Node.Parent Is Nothing Then 'At root node.
            ShowRootNodeOptions()
        ElseIf e.Node.Name.EndsWith(".Update") Then
            ShowUpdateNodeOptions()
        ElseIf e.Node.Name.EndsWith(".rtf") Then
            ShowVersionNodeOptions()
        ElseIf e.Node.Name.EndsWith(".Note") Then
            ShowNoteNodeOptions()
        ElseIf e.Node.Name.EndsWith(".ModList") Then
            ShowNoteNodeOptions()
        ElseIf e.Node.Name.EndsWith(".ToDoList") Then
            ShowNoteNodeOptions()

        Else
            Main.Message.AddWarning("Unknown node type: " & e.Node.Name & vbCrLf)
        End If
    End Sub


    Private Sub ShowRootNodeOptions()
        'Show the valid root node options:
        'Valid options: Update, Note, License, Author, Date-Time
        'Invalid options: To Do List, Process, License Notice, License Text, Quality Control, Checklist

        'Allow valid item radio buttons:
        rbUpdate.Enabled = True
        pbIconUpdate.Enabled = True
        rbNote.Enabled = True
        pbIconNote.Enabled = True
        'rbLicense.Enabled = True
        'pbIconLicense.Enabled = True
        rbVersion.Enabled = False
        pbIconVersion.Enabled = False
        rbToDo.Enabled = False
        pbIconToDo.Enabled = False
        rbProcess.Enabled = False
        pbIconProcess.Enabled = False
        'rbLicenseNotice.Enabled = False
        'pbIconLicNotice.Enabled = False
        rbModList.Enabled = True
        pbIconModList.Enabled = True
        rbDateTime.Enabled = True
        pbIconDateTime.Enabled = True
        'rbLicenseText.Enabled = False
        'pbIconLicText.Enabled = False
        rbQC.Enabled = False
        pbIconQC.Enabled = False
        rbChecklist.Enabled = False
        pbIconChecklist.Enabled = False
        rbProgress.Enabled = False
        'pbIconProgress.Enabled = False
        DisableProgressIcons()
        rbBookSection.Enabled = False
        pbIconBookSection.Enabled = False
        rbPicture.Enabled = False
        pbIconPicture.Enabled = False
        rbCode.Enabled = False
        pbIconCode.Enabled = False
        rbForm.Enabled = False
        pbIconForm.Enabled = False
        'pbIconForm.Image = Main.ImageList1.Images(12)
        pbIconModList.Enabled = True
        rbModList.Enabled = True
        pbIconRtfDoc.Enabled = False
        rbRtfDoc.Enabled = False



        cmbNewDocumentItemType.Items.Clear()
        cmbNewDocumentItemType.Items.Add("Update")
        'cmbNewLibraryItemType.Items.Add("Version")
        'cmbNewLibraryItemType.Items.Add("  Book Section")
        'cmbNewLibraryItemType.Items.Add("  Application Code")
        'cmbNewLibraryItemType.Items.Add("  Application Form")
        'cmbNewLibraryItemType.Items.Add("  Picture")
        cmbNewDocumentItemType.Items.Add("Note")
        'cmbNewLibraryItemType.Items.Add("To Do List")
        'cmbNewLibraryItemType.Items.Add("Process")
        'cmbNewLibraryItemType.Items.Add("Progress")
        cmbNewDocumentItemType.Items.Add("Date-Time")
        cmbNewDocumentItemType.Items.Add("License")
        'cmbNewLibraryItemType.Items.Add("  License Notice")
        'cmbNewLibraryItemType.Items.Add("  License Text")
        cmbNewDocumentItemType.Items.Add("Author")
        'cmbNewLibraryItemType.Items.Add("Quality Control")
        'cmbNewLibraryItemType.Items.Add("Checklist")
        cmbNewDocumentItemType.Items.Add("Modification List")
        'cmbNewLibraryItemType.Items.Add("Rich Text Format Document")


    End Sub

    Private Sub ShowUpdateNodeOptions()
        'Show the valid Update node options:

        'Allow valid item radio buttons:
        rbUpdate.Enabled = False
        pbIconUpdate.Enabled = False
        rbNote.Enabled = True
        pbIconNote.Enabled = True
        'rbLicense.Enabled = True
        'pbIconLicense.Enabled = True
        rbVersion.Enabled = True
        pbIconVersion.Enabled = True
        rbToDo.Enabled = False
        pbIconToDo.Enabled = False
        rbProcess.Enabled = False
        pbIconProcess.Enabled = False
        'rbLicenseNotice.Enabled = False
        'pbIconLicNotice.Enabled = False
        rbModList.Enabled = True
        pbIconModList.Enabled = True
        rbDateTime.Enabled = True
        pbIconDateTime.Enabled = True
        'rbLicenseText.Enabled = False
        'pbIconLicText.Enabled = False
        rbQC.Enabled = False
        pbIconQC.Enabled = False
        rbChecklist.Enabled = False
        pbIconChecklist.Enabled = False
        rbProgress.Enabled = False
        'pbIconProgress.Enabled = False
        DisableProgressIcons()
        rbBookSection.Enabled = True
        pbIconBookSection.Enabled = True
        rbPicture.Enabled = True
        pbIconPicture.Enabled = True
        rbCode.Enabled = True
        pbIconCode.Enabled = True
        rbForm.Enabled = True
        pbIconForm.Enabled = True
        pbIconModList.Enabled = True
        rbModList.Enabled = True
        pbIconRtfDoc.Enabled = True
        rbRtfDoc.Enabled = True

        cmbNewDocumentItemType.Items.Clear()
        'cmbNewLibraryItemType.Items.Add("Update")
        cmbNewDocumentItemType.Items.Add("Version")
        cmbNewDocumentItemType.Items.Add("  Book Section")
        cmbNewDocumentItemType.Items.Add("  Application Code")
        cmbNewDocumentItemType.Items.Add("  Application Form")
        cmbNewDocumentItemType.Items.Add("  Picture")
        cmbNewDocumentItemType.Items.Add("Note")
        'cmbNewLibraryItemType.Items.Add("To Do List")
        'cmbNewLibraryItemType.Items.Add("Process")
        'cmbNewLibraryItemType.Items.Add("Progress")
        cmbNewDocumentItemType.Items.Add("Date-Time")
        cmbNewDocumentItemType.Items.Add("License")
        'cmbNewLibraryItemType.Items.Add("  License Notice")
        'cmbNewLibraryItemType.Items.Add("  License Text")
        cmbNewDocumentItemType.Items.Add("Author")
        'cmbNewLibraryItemType.Items.Add("Quality Control")
        'cmbNewLibraryItemType.Items.Add("Checklist")
        cmbNewDocumentItemType.Items.Add("Modification List")
        cmbNewDocumentItemType.Items.Add("Rich Text Format Document")

    End Sub

    Private Sub ShowVersionNodeOptions()
        'Show the valid Version node options:

        'Allow valid item radio buttons:
        rbUpdate.Enabled = False
        pbIconUpdate.Enabled = False
        rbNote.Enabled = True
        pbIconNote.Enabled = True
        'rbLicense.Enabled = True
        'pbIconLicense.Enabled = True
        rbVersion.Enabled = False
        pbIconVersion.Enabled = False
        rbToDo.Enabled = True
        pbIconToDo.Enabled = True
        rbProcess.Enabled = False
        pbIconProcess.Enabled = False
        'rbLicenseNotice.Enabled = False
        'pbIconLicNotice.Enabled = False
        rbModList.Enabled = True
        pbIconModList.Enabled = True
        rbDateTime.Enabled = True
        pbIconDateTime.Enabled = True
        'rbLicenseText.Enabled = False
        'pbIconLicText.Enabled = False
        rbQC.Enabled = True
        pbIconQC.Enabled = True
        rbChecklist.Enabled = True
        pbIconChecklist.Enabled = False
        rbProgress.Enabled = True
        'pbIconProgress.Enabled = True
        EnableProgressIcons()
        rbBookSection.Enabled = False
        pbIconBookSection.Enabled = False
        rbPicture.Enabled = False
        pbIconPicture.Enabled = False
        rbCode.Enabled = False
        pbIconCode.Enabled = False
        rbForm.Enabled = False
        pbIconForm.Enabled = False
        pbIconModList.Enabled = True
        rbModList.Enabled = True
        pbIconRtfDoc.Enabled = False
        rbRtfDoc.Enabled = False

        cmbNewDocumentItemType.Items.Clear()
        'cmbNewLibraryItemType.Items.Add("Update")
        'cmbNewLibraryItemType.Items.Add("Version")
        'cmbNewLibraryItemType.Items.Add("  Book Section")
        'cmbNewLibraryItemType.Items.Add("  Application Code")
        'cmbNewLibraryItemType.Items.Add("  Application Form")
        'cmbNewLibraryItemType.Items.Add("  Picture")
        cmbNewDocumentItemType.Items.Add("Note")
        cmbNewDocumentItemType.Items.Add("To Do List")
        'cmbNewLibraryItemType.Items.Add("Process")
        cmbNewDocumentItemType.Items.Add("Progress")
        cmbNewDocumentItemType.Items.Add("Date-Time")
        cmbNewDocumentItemType.Items.Add("License")
        'cmbNewLibraryItemType.Items.Add("  License Notice")
        'cmbNewLibraryItemType.Items.Add("  License Text")
        cmbNewDocumentItemType.Items.Add("Author")
        cmbNewDocumentItemType.Items.Add("Quality Control")
        cmbNewDocumentItemType.Items.Add("Checklist")
        cmbNewDocumentItemType.Items.Add("Modification List")
        'cmbNewLibraryItemType.Items.Add("Rich Text Format Document")
    End Sub

    Private Sub ShowNoteNodeOptions()
        'Show the valid Note node options:

        'Allow valid item radio buttons:
        rbUpdate.Enabled = False
        pbIconUpdate.Enabled = False
        rbNote.Enabled = False
        pbIconNote.Enabled = False
        rbVersion.Enabled = False
        pbIconVersion.Enabled = False
        rbToDo.Enabled = False
        pbIconToDo.Enabled = False
        rbProcess.Enabled = False
        pbIconProcess.Enabled = False
        rbModList.Enabled = False
        pbIconModList.Enabled = False
        rbDateTime.Enabled = False
        pbIconDateTime.Enabled = False
        rbQC.Enabled = False
        pbIconQC.Enabled = False
        rbChecklist.Enabled = False
        pbIconChecklist.Enabled = False
        rbProgress.Enabled = False
        'pbIconProgress.Enabled = True
        EnableProgressIcons()
        rbBookSection.Enabled = False
        pbIconBookSection.Enabled = False
        rbPicture.Enabled = False
        pbIconPicture.Enabled = False
        rbCode.Enabled = False
        pbIconCode.Enabled = False
        rbForm.Enabled = False
        pbIconForm.Enabled = False
        pbIconModList.Enabled = True
        rbModList.Enabled = False
        pbIconRtfDoc.Enabled = False
        rbRtfDoc.Enabled = False

        cmbNewDocumentItemType.Items.Clear()
        'cmbNewLibraryItemType.Items.Add("Update")
        'cmbNewLibraryItemType.Items.Add("Version")
        'cmbNewLibraryItemType.Items.Add("  Book Section")
        'cmbNewLibraryItemType.Items.Add("  Application Code")
        'cmbNewLibraryItemType.Items.Add("  Application Form")
        'cmbNewLibraryItemType.Items.Add("  Picture")
        'cmbNewDocumentItemType.Items.Add("Note")
        'cmbNewDocumentItemType.Items.Add("To Do List")
        'cmbNewLibraryItemType.Items.Add("Process")
        'cmbNewDocumentItemType.Items.Add("Progress")
        'cmbNewDocumentItemType.Items.Add("Date-Time")
        'cmbNewDocumentItemType.Items.Add("License")
        'cmbNewLibraryItemType.Items.Add("  License Notice")
        'cmbNewLibraryItemType.Items.Add("  License Text")
        'cmbNewDocumentItemType.Items.Add("Author")
        'cmbNewDocumentItemType.Items.Add("Quality Control")
        'cmbNewDocumentItemType.Items.Add("Checklist")
        'cmbNewDocumentItemType.Items.Add("Modification List")
        'cmbNewLibraryItemType.Items.Add("Rich Text Format Document")
    End Sub

    Private Sub ShowLicenseNodeOptions()
        'Show the valid License node options:

        'NOTE: This method is no longer needed - Licence details are stored in the Document Structure file.

        'Allow valid item radio buttons:
        rbUpdate.Enabled = False
        pbIconUpdate.Enabled = False
        rbNote.Enabled = True
        pbIconNote.Enabled = True
        'rbLicense.Enabled = False
        'pbIconLicense.Enabled = False
        rbVersion.Enabled = False
        pbIconVersion.Enabled = False
        rbToDo.Enabled = False
        pbIconToDo.Enabled = False
        rbProcess.Enabled = False
        pbIconProcess.Enabled = False
        'rbLicenseNotice.Enabled = True
        'pbIconLicNotice.Enabled = True
        rbModList.Enabled = True
        pbIconModList.Enabled = True
        rbDateTime.Enabled = False
        pbIconDateTime.Enabled = False
        'rbLicenseText.Enabled = True
        'pbIconLicText.Enabled = True
        rbQC.Enabled = False
        pbIconQC.Enabled = False
        rbChecklist.Enabled = False
        pbIconChecklist.Enabled = False
        rbProgress.Enabled = False
        'pbIconProgress.Enabled = False
        DisableProgressIcons()
        rbBookSection.Enabled = False
        pbIconBookSection.Enabled = False
        rbPicture.Enabled = False
        pbIconPicture.Enabled = False
        rbCode.Enabled = False
        pbIconCode.Enabled = False
        rbForm.Enabled = False
        pbIconForm.Enabled = False
        pbIconModList.Enabled = True
        rbModList.Enabled = True

        cmbNewDocumentItemType.Items.Clear()
        'cmbNewLibraryItemType.Items.Add("Update")
        'cmbNewLibraryItemType.Items.Add("Version")
        'cmbNewLibraryItemType.Items.Add("  Book Section")
        'cmbNewLibraryItemType.Items.Add("  Application Code")
        'cmbNewLibraryItemType.Items.Add("  Application Form")
        'cmbNewLibraryItemType.Items.Add("  Picture")
        cmbNewDocumentItemType.Items.Add("Note")
        'cmbNewLibraryItemType.Items.Add("To Do List")
        'cmbNewLibraryItemType.Items.Add("Process")
        'cmbNewLibraryItemType.Items.Add("Progress")
        'cmbNewLibraryItemType.Items.Add("Date-Time")
        'cmbNewLibraryItemType.Items.Add("License")
        cmbNewDocumentItemType.Items.Add("  License Notice")
        cmbNewDocumentItemType.Items.Add("  License Text")
        cmbNewDocumentItemType.Items.Add("Author")
        'cmbNewLibraryItemType.Items.Add("Quality Control")
        'cmbNewLibraryItemType.Items.Add("Checklist")
        cmbNewDocumentItemType.Items.Add("Modification List")
    End Sub

    Private Sub DisableProgressIcons()
        'Disable all progress icons.
        pbIconProgress.Enabled = False
        NumericUpDown1.Enabled = False
        pbIconProgress0.Enabled = False
        pbIconProgress10.Enabled = False
        pbIconProgress20.Enabled = False
        pbIconProgress30.Enabled = False
        pbIconProgress40.Enabled = False
        pbIconProgress50.Enabled = False
        pbIconProgress60.Enabled = False
        pbIconProgress70.Enabled = False
        pbIconProgress80.Enabled = False
        pbIconProgress90.Enabled = False
        pbIconProgress100.Enabled = False
    End Sub

    Private Sub EnableProgressIcons()
        'Enable all progress icons.
        pbIconProgress.Enabled = True
        NumericUpDown1.Enabled = True
        pbIconProgress0.Enabled = True
        pbIconProgress10.Enabled = True
        pbIconProgress20.Enabled = True
        pbIconProgress30.Enabled = True
        pbIconProgress40.Enabled = True
        pbIconProgress50.Enabled = True
        pbIconProgress60.Enabled = True
        pbIconProgress70.Enabled = True
        pbIconProgress80.Enabled = True
        pbIconProgress90.Enabled = True
        pbIconProgress100.Enabled = True
    End Sub

    Private Sub OpenSelectedDocument()
        'Open the Selected Document.
        'The document name can be found in SelectedDocumentFileName property.

        If Main.Project.DataFileExists(SelectedDocumentFileName) Then 'File exists - show the data.
            If SelectedDocumentFileName.EndsWith("rtf") Then
                dgvDocument.Hide()
                rtbDocument.Show()
                Dim rtbData As New IO.MemoryStream()
                Main.Project.ReadData(SelectedDocumentFileName, rtbData)
                rtbData.Position = 0
                rtbDocument.LoadFile(rtbData, RichTextBoxStreamType.RichText)
                'http://condor.depaul.edu/sjost/it236/documents/colorNames.htm
                'rtbDocument.BackColor = Color.White
                'rtbDocument.BackColor = Color.WhiteSmoke
                'rtbDocument.BackColor = Color.Ivory
                'rtbDocument.BackColor = Color.GhostWhite
                rtbDocument.BackColor = Color.FromArgb(250, 250, 250)
            ElseIf SelectedDocumentFileName.EndsWith(".Note") Then
                dgvDocument.Hide()
                rtbDocument.Show()
                Dim rtbData As New IO.MemoryStream()
                Main.Project.ReadData(SelectedDocumentFileName, rtbData)
                rtbData.Position = 0
                rtbDocument.LoadFile(rtbData, RichTextBoxStreamType.RichText)
                'http://condor.depaul.edu/sjost/it236/documents/colorNames.htm
                rtbDocument.BackColor = Color.LemonChiffon
            ElseIf SelectedDocumentFileName.EndsWith(".ModList") Then
                dgvDocument.Show()
                rtbDocument.Hide()
                OpenModificationList()

            ElseIf SelectedDocumentFileName.EndsWith(".ToDoList") Then
                dgvDocument.Show()
                rtbDocument.Hide()
                OpenToDoList

            End If

        Else 'File does not exists - just show a blank page.
            Main.Message.AddWarning("Document file does not exist: " & SelectedDocumentFileName & vbCrLf)
            Main.Message.Add("The document file will be created when the new text is saved." & vbCrLf)
            'rtbCode.Clear()
            rtbDocument.Clear()

            If SelectedDocumentFileName.EndsWith("rtf") Then
                dgvDocument.Hide()
                rtbDocument.Show()
                'rtbDocument.BackColor = Color.White
                'rtbDocument.BackColor = Color.WhiteSmoke
                'rtbDocument.BackColor = Color.Ivory
                'rtbDocument.BackColor = Color.GhostWhite
                rtbDocument.BackColor = Color.FromArgb(250, 250, 250)
            ElseIf SelectedDocumentFileName.EndsWith(".Note") Then
                dgvDocument.Hide()
                rtbDocument.Show()
                rtbDocument.BackColor = Color.LemonChiffon
            ElseIf SelectedDocumentFileName.EndsWith(".ModList") Then
                dgvDocument.Show()
                rtbDocument.Hide()
                SetUpModificationList()
            ElseIf SelectedDocumentFileName.EndsWith(".ToDoList") Then
                dgvDocument.Show()
                rtbDocument.Hide()
                SetUpToDoList()
            End If
        End If

    End Sub

    Private Sub OpenModificationList()
        'Open the modification list.
        'This is stored in an xml file with the .ModList file extension.
        SetUpModificationList()

        Dim XDoc As System.Xml.Linq.XDocument
        Main.Project.ReadXmlData(SelectedDocumentFileName, XDoc)

        Dim Mods = From item In XDoc.<ModificationList>.<Modification>

        For Each item In Mods
            dgvDocument.Rows.Add(New String() {item.<Number>.Value, item.<Date>.Value, item.<Author>.Value, item.<Summary>.Value, item.<Description>.Value})
        Next


    End Sub

    Private Sub SetUpModificationList()
        'Set up the dgvDocument to display the Modification list.
        dgvDocument.Rows.Clear()
        dgvDocument.Columns.Clear()
        dgvDocument.ColumnCount = 5
        dgvDocument.Columns(0).HeaderText = "No."
        dgvDocument.Columns(1).HeaderText = "Date"
        dgvDocument.Columns(2).HeaderText = "Author"
        dgvDocument.Columns(3).HeaderText = "Summary"
        dgvDocument.Columns(4).HeaderText = "Description"
        'dgvDocument.Columns(0).Width = 50
        dgvDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        dgvDocument.AutoResizeColumns()

    End Sub

    Private Sub OpenToDoList()
        'Open the To Do list.
        'This is stored in an xml file with the .ToDoList file extension.

        SetUpToDoList()

        Dim XDoc As System.Xml.Linq.XDocument
        Main.Project.ReadXmlData(SelectedDocumentFileName, XDoc)

        Dim Mods = From item In XDoc.<ToDoList>.<ToDo>

        For Each item In Mods
            'dgvDocument.Rows.Add(New String() {item.<Number>.Value, item.<DateAdded>.Value, item.<CompletionDateRequested>.Value, item.<IsDone>.Value, item.<DateDone>.Value, item.<Author>.Value, item.<Summary>.Value, item.<Description>.Value})
            dgvDocument.Rows.Add(New String() {item.<Number>.Value, item.<DateAdded>.Value, item.<Weighting>.Value, item.<DueDate>.Value, item.<IsDone>.Value, item.<DateDone>.Value, item.<Author>.Value, item.<Summary>.Value, item.<Description>.Value})
        Next

    End Sub


    Private Sub SetUpToDoList()
        'Set up the dgvDocument to display the To Do list.
        dgvDocument.Rows.Clear()
        dgvDocument.Columns.Clear()
        'dgvDocument.ColumnCount = 8



        'dgvDocument.Columns(3) = New DataGridViewComboBoxColumn
        'dgvDocument.Columns(3).CellType = ComboBoxCol3
        'dgvDocument.Columns(3) = ComboBoxCol3

        'dgvDocument.Columns(0).HeaderText = "No."
        'dgvDocument.Columns(1).HeaderText = "Date Added"
        'dgvDocument.Columns(2).HeaderText = "Date Requested"
        'dgvDocument.Columns(3).HeaderText = "Done?"
        'dgvDocument.Columns(4).HeaderText = "Date Done"
        'dgvDocument.Columns(5).HeaderText = "Author"
        'dgvDocument.Columns(6).HeaderText = "Summary"
        'dgvDocument.Columns(7).HeaderText = "Description"

        Dim TextBoxCol0 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol0)
        dgvDocument.Columns(0).HeaderText = "No."

        Dim TextBoxCol1 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol1)
        dgvDocument.Columns(1).HeaderText = "Date Added"

        Dim TextBoxCol2 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol2)
        dgvDocument.Columns(2).HeaderText = "Weighting"

        Dim TextBoxCol3 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol3)
        'dgvDocument.Columns(2).HeaderText = "Completion Date Requested"
        dgvDocument.Columns(3).HeaderText = "Due Date"

        Dim ComboBoxCol4 As New DataGridViewComboBoxColumn
        dgvDocument.Columns.Add(ComboBoxCol4)
        dgvDocument.Columns(4).HeaderText = "Done?"
        ComboBoxCol4.Items.Add("No")
        ComboBoxCol4.Items.Add("Yes")

        Dim TextBoxCol5 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol5)
        dgvDocument.Columns(5).HeaderText = "Date Done"

        Dim TextBoxCol6 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol6)
        dgvDocument.Columns(6).HeaderText = "Author"

        Dim TextBoxCol7 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol7)
        dgvDocument.Columns(7).HeaderText = "Summary"

        Dim TextBoxCol8 As New DataGridViewTextBoxColumn
        dgvDocument.Columns.Add(TextBoxCol8)
        dgvDocument.Columns(8).HeaderText = "Description"


        'dgvDocument.Columns(0).Width = 50
        dgvDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        dgvDocument.AutoResizeColumns()


    End Sub

    Private Sub SaveSelectedDocument()
        'Save the selected document.

        If SelectedDocumentFileName.EndsWith("rtf") Then
            Dim rtbData As New IO.MemoryStream
            rtbDocument.SaveFile(rtbData, RichTextBoxStreamType.RichText)
            rtbData.Position = 0
            Main.Project.SaveData(SelectedDocumentFileName, rtbData)
        ElseIf SelectedDocumentFileName.EndsWith("ModList") Then
            'Save the list of modifications in the dgvDocument.
            'First remove the last blank row in the data grid view:
            dgvDocument.CommitEdit(DataGridViewDataErrorContexts.Commit)
            dgvDocument.AllowUserToAddRows = False
            Dim XDoc = <?xml version="1.0" encoding="utf-8"?>
                       <!---->
                       <!--List of modifications-->
                       <ModificationList>
                           <%= From item In dgvDocument.Rows
                               Select
                               <Modification>
                                   <Number><%= item.Cells(0).Value %></Number>
                                   <Date><%= item.Cells(1).Value %></Date>
                                   <Author><%= item.Cells(2).Value %></Author>
                                   <Summary><%= item.Cells(3).Value %></Summary>
                                   <Description><%= item.Cells(4).Value %></Description>
                               </Modification> %>
                       </ModificationList>
            Main.Project.SaveXmlData(SelectedDocumentFileName, XDoc)
            dgvDocument.AllowUserToAddRows = True 'Restore state.

        ElseIf SelectedDocumentFileName.EndsWith("ToDoList") Then
            'Save the list of modifications in the dgvDocument.
            'First remove the last blank row in the data grid view:
            dgvDocument.CommitEdit(DataGridViewDataErrorContexts.Commit)
            dgvDocument.AllowUserToAddRows = False
            Dim XDoc = <?xml version="1.0" encoding="utf-8"?>
                       <!---->
                       <!--To Do List-->
                       <ToDoList>
                           <%= From item In dgvDocument.Rows
                               Select
                               <ToDo>
                                   <Number><%= item.Cells(0).Value %></Number>
                                   <DateAdded><%= item.Cells(1).Value %></DateAdded>
                                   <Weighting><%= item.Cells(2).Value %></Weighting>
                                   <DueDate><%= item.Cells(3).Value %></DueDate>
                                   <IsDone><%= item.Cells(4).Value %></IsDone>
                                   <DateDone><%= item.Cells(5).Value %></DateDone>
                                   <Author><%= item.Cells(6).Value %></Author>
                                   <Summary><%= item.Cells(7).Value %></Summary>
                                   <Description><%= item.Cells(8).Value %></Description>
                               </ToDo> %>
                       </ToDoList>
            Main.Project.SaveXmlData(SelectedDocumentFileName, XDoc)
            dgvDocument.AllowUserToAddRows = True 'Restore state.

            ' <CompletionDateRequested><%= item.Cells(2).Value %></CompletionDateRequested>
        Else
            Main.Message.AddWarning("Unknown document type: " & SelectedDocumentFileName & vbCrLf)
        End If

    End Sub

    Private Sub btnSaveChangesToFile_Click(sender As Object, e As EventArgs)
        SaveSelectedDocument()
    End Sub


    Private Sub btnSaveStructure_Click(sender As Object, e As EventArgs) Handles btnSaveStructure.Click
        'Save the TreeView as an XML file.
        SaveStructureFile()
    End Sub

    Private Sub SaveStructureFile()

        'Remove any leading or lagging spaces from the file name:
        txtStructureFileName.Text = Trim(txtStructureFileName.Text)

        'Check if a file name has been specified:
        If txtStructureFileName.Text = "" Then
            Main.Message.AddWarning("Structure settings not saved. The structure file name is blank." & vbCrLf)
            Beep()
            Exit Sub
        End If

        'Replace any spaces in the file name with underscore characters:
        If txtStructureFileName.Text.Contains(" ") Then
            txtStructureFileName.Text = txtStructureFileName.Text.Replace(" ", "_")
        End If

        SaveStructure(txtStructureFileName.Text)

    End Sub

    Private Sub SaveStructure(ByVal FileName As String)
        'Save the TreeView structure in an XML file with the specified file name.

        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)

        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Document Structure Definition"))

        Dim myStructure As New XElement(FileName) 'The root node is assigned the FileName.
        If trvDocument.Nodes(0).Name <> FileName Then
            Main.Message.AddWarning("The file name is different from the root node name." & vbCrLf)
            Main.Message.AddWarning("The file name is " & FileName & vbCrLf)
            Main.Message.AddWarning("The root node name is " & trvDocument.Nodes(0).Name & vbCrLf)
            Main.Message.AddWarning("The root node name will be changed to the file name." & vbCrLf)
        Else
            Main.Message.Add("The file name is the same as the root node name: " & FileName & vbCrLf)
        End If

        Dim myText As New XElement("Text", trvDocument.Nodes(0).Text)
        myStructure.Add(myText)
        Dim myDescription As New XElement("Description", Description)
        myStructure.Add(myDescription)
        Dim myCreationDate As New XElement("CreationDate", CreationDate)
        myStructure.Add(myCreationDate)
        Dim myLastEditDate As New XElement("LastEditDate", LastEditDate)
        myStructure.Add(myLastEditDate)
        'Dim myAuthorFile As New XElement("AuthorFile", DefaultAuthorFile)
        Dim myAuthorFile As New XElement("DefaultAuthorFile", DefaultAuthorFile)
        myStructure.Add(myAuthorFile)
        'Dim myAuthorSummary As New XElement("AuthorSummary", DefaultAuthorSummary)
        Dim myAuthorSummary As New XElement("DefaultAuthorSummary", DefaultAuthorSummary)
        myStructure.Add(myAuthorSummary)
        'Dim myCopyrightFile As New XElement("CopyrightFile", DefaultCopyrightFile)
        Dim myCopyrightFile As New XElement("DefaultCopyrightFile", DefaultCopyrightFile)
        myStructure.Add(myCopyrightFile)
        'Dim myCopyrightSummary As New XElement("CopyrightSummary", DefaultCopyrightSummary)
        Dim myCopyrightSummary As New XElement("DefaultCopyrightSummary", DefaultCopyrightSummary)
        myStructure.Add(myCopyrightSummary)
        'Dim myLicenseFile As New XElement("LicenseFile", DefaultLicenseFile)
        Dim myLicenseFile As New XElement("DefaultLicenseFile", DefaultLicenseFile)
        myStructure.Add(myLicenseFile)
        'Dim myLicenseSummary As New XElement("LicenseSummary", DefaultLicenseSummary)
        Dim myLicenseSummary As New XElement("DefaultLicenseSummary", DefaultLicenseSummary)
        myStructure.Add(myLicenseSummary)
        Dim mySelectedNode As New XElement("SelectedNode", SelectedNode)
        myStructure.Add(mySelectedNode)

        'SaveStrucNode(myCode, "", trvDocument.Nodes)
        SaveStrucNode(myStructure, "", trvDocument.Nodes(0).Nodes)

        'Save the list of deleted nodes:
        myStructure.Add(New XComment(""))
        Dim DeletedNodes As New XElement("DeletedNodes")
        For Each item In dgvDeletedItems.Rows
            Dim Node As New XElement("Node")
            Dim nodeText As New XElement("Text", item.Cells(0).Value)
            Node.Add(nodeText)
            Dim NodeKeyString As String = item.Cells(1).Value 'This is used later when saving node settings.
            Dim nodeKey As New XElement("Key", item.Cells(1).Value)
            Node.Add(nodeKey)
            Dim nodeIndex As New XElement("Index", item.Cells(3).Value)
            Node.Add(nodeIndex)
            Dim nodeParent As New XElement("Parent", item.Cells(2).Value)
            Node.Add(nodeParent)
            Dim delDate As New XElement("DeletedDate", item.Cells(4).Value)
            Node.Add(delDate)
            'Save any DocItem settings:
            If DocItem.ContainsKey(NodeKeyString) Then
                Dim ItemDescription As New XElement("ItemDescription", DocItem(NodeKeyString).Description)
                Node.Add(ItemDescription)
                Dim ItemCreationDate As New XElement("ItemCreationDate", DocItem(NodeKeyString).CreationDate)
                Node.Add(ItemCreationDate)
                Dim ItemLastEditDate As New XElement("ItemLastEditDate", DocItem(NodeKeyString).LastEditDate)
                Node.Add(ItemLastEditDate)
                Dim ItemAuthorFile As New XElement("ItemAuthorFile", DocItem(NodeKeyString).AuthorFile)
                Node.Add(ItemAuthorFile)
                Dim ItemAuthorSummary As New XElement("ItemAuthorSummary", DocItem(NodeKeyString).AuthorSummary)
                Node.Add(ItemAuthorSummary)
            End If
            'Save any Copyright settings:
            If Copyright.ContainsKey(NodeKeyString) Then
                Dim CopyrightFile As New XElement("CopyrightFile", Copyright(NodeKeyString).File)
                Node.Add(CopyrightFile)
                Dim CopyrightSummary As New XElement("CopyrightSummary", Copyright(NodeKeyString).Summary)
                Node.Add(CopyrightSummary)
            End If
            'Save any License settings:
            If License.ContainsKey(NodeKeyString) Then
                Dim LicenseFile As New XElement("LicenseFile", License(NodeKeyString).File)
                Node.Add(LicenseFile)
                Dim LicenseSummary As New XElement("LicenseSummary", License(NodeKeyString).Summary)
                Node.Add(LicenseSummary)
            End If
            DeletedNodes.Add(Node)
        Next
        myStructure.Add(DeletedNodes)

        XDoc.Add(myStructure)

        Main.Project.SaveXmlData(FileName, XDoc)

    End Sub


    Private Sub SaveStrucNode(ByRef myElement As XElement, Parent As String, ByRef tnc As TreeNodeCollection)
        'Save the nodes in the TreeNodeCollection in the XElement.
        'This method calls itself recursively to save all the nodes in trvDocument

        Dim I As Integer

        If tnc.Count = 0 Then 'Leaf
        Else
            For I = 0 To tnc.Count - 1
                Dim myNode As New XElement(tnc(I).Name)
                Dim myNodeText As New XElement("Text", tnc(I).Text)
                myNode.Add(myNodeText)

                'If tnc(I).Name = DocStructure Then
                '    Dim myStrucDescr As New XElement("Description", txtDocStrucDescription.Text)
                '    myNode.Add(myStrucDescr)
                '    Dim mySelectedNode As New XElement("SelectedNode", SelectedNode)
                '    myNode.Add(mySelectedNode)
                'End If

                'If tnc(I).Name.EndsWith(".Update") Then
                '    'Save Update node parameters:
                '    If dictUpdateInfo.ContainsKey(tnc(I).Name) Then
                '        'Save the parameters:
                '        Dim myUpdateDescription As New XElement("Description", dictUpdateInfo(tnc(I).Name).Description)
                '        myNode.Add(myUpdateDescription)
                '        Dim myUpdateCreationDate As New XElement("CreationDate", dictUpdateInfo(tnc(I).Name).CreationDate)
                '        myNode.Add(myUpdateCreationDate)
                '        Dim myUpdateLastEditDate As New XElement("LastEditDate", dictUpdateInfo(tnc(I).Name).LastEditDate)
                '        myNode.Add(myUpdateLastEditDate)
                '        Dim myUpdateAuthorFile As New XElement("AuthorFile", dictUpdateInfo(tnc(I).Name).AuthorFile)
                '        myNode.Add(myUpdateAuthorFile)
                '        Dim myUpdateAuthorSummary As New XElement("AuthorSummary", dictUpdateInfo(tnc(I).Name).AuthorSummary)
                '        myNode.Add(myUpdateAuthorSummary)
                '        Dim myUpdateCopyrightFile As New XElement("CopyrightFile", dictUpdateInfo(tnc(I).Name).CopyrightFile)
                '        myNode.Add(myUpdateCopyrightFile)
                '        Dim myUpdateCopyrightSummary As New XElement("CopyrightSummary", dictUpdateInfo(tnc(I).Name).CopyrightSummary)
                '        myNode.Add(myUpdateCopyrightSummary)
                '        Dim myUpdateLicenseFile As New XElement("LicenseFile", dictUpdateInfo(tnc(I).Name).LicenseFile)
                '        myNode.Add(myUpdateLicenseFile)
                '        Dim myUpdateLicenseSummary As New XElement("LicenseSummary", dictUpdateInfo(tnc(I).Name).LicenseSummary)
                '        myNode.Add(myUpdateLicenseSummary)
                '    End If
                'Else
                '    'Save Version node:
                '    'Save Update node parameters:
                '    If dictVersionInfo.ContainsKey(tnc(I).Name) Then
                '        'Save the parameters:
                '        Dim myVersionDescription As New XElement("Description", dictVersionInfo(tnc(I).Name).Description)
                '        myNode.Add(myVersionDescription)
                '        Dim myVersionCreationDate As New XElement("CreationDate", dictVersionInfo(tnc(I).Name).CreationDate)
                '        myNode.Add(myVersionCreationDate)
                '        Dim myVersionLastEditDate As New XElement("LastEditDate", dictVersionInfo(tnc(I).Name).LastEditDate)
                '        myNode.Add(myVersionLastEditDate)
                '        Dim myVersionAuthorFile As New XElement("AuthorFile", dictVersionInfo(tnc(I).Name).AuthorFile)
                '        myNode.Add(myVersionAuthorFile)
                '        Dim myVersionAuthorSummary As New XElement("AuthorSummary", dictVersionInfo(tnc(I).Name).AuthorSummary)
                '        myNode.Add(myVersionAuthorSummary)
                '        Dim myVersionCopyrightFile As New XElement("CopyrightFile", dictVersionInfo(tnc(I).Name).CopyrightFile)
                '        myNode.Add(myVersionCopyrightFile)
                '        Dim myVersionCopyrightSummary As New XElement("CopyrightSummary", dictVersionInfo(tnc(I).Name).CopyrightSummary)
                '        myNode.Add(myVersionCopyrightSummary)
                '        Dim myVersionLicenseFile As New XElement("LicenseFile", dictVersionInfo(tnc(I).Name).LicenseFile)
                '        myNode.Add(myVersionLicenseFile)
                '        Dim myVersionLicenseSummary As New XElement("LicenseSummary", dictVersionInfo(tnc(I).Name).LicenseSummary)
                '        myNode.Add(myVersionLicenseSummary)
                '    End If
                'End If

                If DocItem.ContainsKey(tnc(I).Name) Then
                    Dim myDescription As New XElement("Description", DocItem(tnc(I).Name).Description)
                    myNode.Add(myDescription)
                    Dim myCreationDate As New XElement("CreationDate", DocItem(tnc(I).Name).CreationDate)
                    myNode.Add(myCreationDate)
                    Dim myLastEditDate As New XElement("LastEditDate", DocItem(tnc(I).Name).LastEditDate)
                    myNode.Add(myLastEditDate)
                    If tnc(I).Name.EndsWith(".Update") Then
                        Dim myAuthorFile As New XElement("DefaultAuthorFile", DocItem(tnc(I).Name).AuthorFile)
                        myNode.Add(myAuthorFile)
                        Dim myAuthorSummary As New XElement("DefaultAuthorSummary", DocItem(tnc(I).Name).AuthorSummary)
                        myNode.Add(myAuthorSummary)
                    Else
                        Dim myAuthorFile As New XElement("AuthorFile", DocItem(tnc(I).Name).AuthorFile)
                        myNode.Add(myAuthorFile)
                        Dim myAuthorSummary As New XElement("AuthorSummary", DocItem(tnc(I).Name).AuthorSummary)
                        myNode.Add(myAuthorSummary)
                    End If

                End If

                If Copyright.ContainsKey(tnc(I).Name) Then
                    If tnc(I).Name.EndsWith(".Update") Then
                        Dim myCopyrightFile As New XElement("DefaultCopyrightFile", Copyright(tnc(I).Name).File)
                        myNode.Add(myCopyrightFile)
                        Dim myCopyrightSummary As New XElement("DefaultCopyrightSummary", Copyright(tnc(I).Name).Summary)
                        myNode.Add(myCopyrightSummary)
                    Else
                        Dim myCopyrightFile As New XElement("CopyrightFile", Copyright(tnc(I).Name).File)
                        myNode.Add(myCopyrightFile)
                        Dim myCopyrightSummary As New XElement("CopyrightSummary", Copyright(tnc(I).Name).Summary)
                        myNode.Add(myCopyrightSummary)
                    End If
                End If

                If License.ContainsKey(tnc(I).Name) Then
                    If tnc(I).Name.EndsWith(".Update") Then
                        Dim myLicenseFile As New XElement("DefaultLicenseFile", License(tnc(I).Name).File)
                        myNode.Add(myLicenseFile)
                        Dim myLicenseSummary As New XElement("DefaultLicenseSummary", License(tnc(I).Name).Summary)
                        myNode.Add(myLicenseSummary)
                    Else
                        Dim myLicenseFile As New XElement("LicenseFile", License(tnc(I).Name).File)
                        myNode.Add(myLicenseFile)
                        Dim myLicenseSummary As New XElement("LicenseSummary", License(tnc(I).Name).Summary)
                        myNode.Add(myLicenseSummary)
                    End If
                End If

                SaveStrucNode(myNode, tnc(I).Name, tnc(I).Nodes)
                myElement.Add(myNode)
            Next
        End If

    End Sub


#Region "Updates and Versions" '===============================================================================================================================================================

    Private Sub btnMoveItemUp_Click(sender As Object, e As EventArgs)
        'Move the selected item up on the Information Tree.

        If trvDocument.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            Dim Node As TreeNode
            Node = trvDocument.SelectedNode
            Dim index As Integer = Node.Index
            If index = 0 Then
                'Already at the first node.
                Node.TreeView.Focus()
            Else
                Dim Parent As TreeNode = Node.Parent
                Parent.Nodes.RemoveAt(index)
                Parent.Nodes.Insert(index - 1, Node)
                trvDocument.SelectedNode = Node
                Node.TreeView.Focus()
            End If
        End If

    End Sub

    Private Sub btnMoveItemDown_Click(sender As Object, e As EventArgs)
        'Move the selected item down on the Information Tree.

        If trvDocument.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            Dim Node As TreeNode
            Node = trvDocument.SelectedNode
            Dim index As Integer = Node.Index
            Dim Parent As TreeNode = Node.Parent
            If index < Parent.Nodes.Count - 1 Then
                Parent.Nodes.RemoveAt(index)
                Parent.Nodes.Insert(index + 1, Node)
                trvDocument.SelectedNode = Node
                Node.TreeView.Focus()
            Else
                'Already at the last node.
                Node.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnSelectPrev_Click(sender As Object, e As EventArgs)
        'Select the previous item in the treeview:

        If trvDocument.SelectedNode Is Nothing Then

        Else
            Dim Node As TreeNode
            Node = trvDocument.SelectedNode
            If Node.PrevNode Is Nothing Then
                Node.TreeView.Focus()
            Else
                trvDocument.SelectedNode = Node.PrevNode
                Node.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnSelectNext_Click(sender As Object, e As EventArgs)
        'Select the next item in the treeview:

        'Dim Nodes As TreeNodeCollection = trvLibrary.Nodes
        If trvDocument.SelectedNode Is Nothing Then

        Else
            Dim Node As TreeNode
            Node = trvDocument.SelectedNode
            If Node.NextNode Is Nothing Then
                Node.TreeView.Focus()
            Else
                trvDocument.SelectedNode = Node.NextNode
                Node.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnDeleteItem_Click(sender As Object, e As EventArgs)
        'Deleted the selected node.

        If trvDocument.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            Dim Node As TreeNode
            Node = trvDocument.SelectedNode
            'Dim index As Integer = Node.Index
            If Node.Nodes.Count > 0 Then
                Main.Message.AddWarning("The selected node has child nodes. Delete the child nodes before deleting this node." & vbCrLf)
            Else
                'dgvDeletedItems.Rows.Add(Node.Text, Node.Name, Node.Parent.Name, Node.Index)
                dgvDeletedItems.Rows.Add(Node.Text, Node.Name, Node.Parent.Name, Node.Index, Format(Now, "d-MMM-yyyy H:mm:ss"))
                Dim Parent As TreeNode = Node.Parent
                Parent.Nodes.RemoveAt(Node.Index)
                SaveStructureFile()
            End If
        End If
    End Sub

    Private Sub btnCutItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnCancelCut_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnPasteItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnApplyNodeTextEdit_Click(sender As Object, e As EventArgs)
        'Change the selected node text.

        If trvDocument.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            If Trim(txtNewNodeText.Text) = "" Then

            Else
                Dim Node As TreeNode
                Node = trvDocument.SelectedNode
                Node.Text = Trim(txtNewNodeText.Text)
                txtCurrentNodeText.Text = Trim(txtNewNodeText.Text)
                'SaveLibrary(txtLibraryFileName.Text)
                SaveStructure(txtStructureFileName.Text)
            End If

        End If
    End Sub

    Private Sub btnApplyNodeKeyEdit_Click(sender As Object, e As EventArgs)
        'Change the selected node key.

        If trvDocument.SelectedNode Is Nothing Then
            'No node has been selected.
            Main.Message.AddWarning("No node has been selected." & vbCrLf)
            Beep()
        Else
            If Trim(txtNewNodeKey.Text) = "" Then
                Main.Message.AddWarning("No new node key has been specified." & vbCrLf)
                Beep()
            Else
                'Check if there is already a file with the same name as the new node key in the project.
                If Main.Project.DataFileExists(Trim(txtNewNodeKey.Text)) Then
                    Main.Message.AddWarning("The new node key is already used." & vbCrLf)
                    Beep()
                Else
                    Main.Project.RenameDataFile(txtCurrentNodeKey.Text, Trim(txtNewNodeKey.Text))
                    Dim Node As TreeNode
                    Node = trvDocument.SelectedNode
                    Node.Name = Trim(txtNewNodeKey.Text)
                    txtCurrentNodeKey.Text = Trim(txtNewNodeKey.Text)
                    'SaveLibrary(txtLibraryFileName.Text)
                    SaveStructure(txtStructureFileName.Text)
                End If

            End If
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs)
        Select Case NumericUpDown1.Value
            Case 0
                'pbIconProgress.Image = Progress.Images(20)
                pbIconProgress.Image = Main.ImageList1.Images(47)
            Case 10
                'pbIconProgress.Image = Progress.Images(18)
                pbIconProgress.Image = Main.ImageList1.Images(49)
            Case 20
                'pbIconProgress.Image = Progress.Images(16)
                pbIconProgress.Image = Main.ImageList1.Images(51)
            Case 30
                'pbIconProgress.Image = Progress.Images(14)
                pbIconProgress.Image = Main.ImageList1.Images(53)
            Case 40
                'pbIconProgress.Image = Progress.Images(12)
                pbIconProgress.Image = Main.ImageList1.Images(55)
            Case 50
                'pbIconProgress.Image = Progress.Images(10)
                pbIconProgress.Image = Main.ImageList1.Images(57)
            Case 60
                'pbIconProgress.Image = Progress.Images(8)
                pbIconProgress.Image = Main.ImageList1.Images(59)
            Case 70
                'pbIconProgress.Image = Progress.Images(6)
                pbIconProgress.Image = Main.ImageList1.Images(61)
            Case 80
                'pbIconProgress.Image = Progress.Images(4)
                pbIconProgress.Image = Main.ImageList1.Images(63)
            Case 90
                'pbIconProgress.Image = Progress.Images(2)
                pbIconProgress.Image = Main.ImageList1.Images(65)
            Case 100
                'pbIconProgress.Image = Progress.Images(0)
                pbIconProgress.Image = Main.ImageList1.Images(67)
        End Select
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress0_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 0
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress10_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 10
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress20_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 20
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress30_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 30
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress40_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 40
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress50_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 50
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress60_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 60
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress70_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 70
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress80_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 80
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress90_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 90
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProgress100_Click(sender As Object, e As EventArgs)
        NumericUpDown1.Value = 100
        rbProgress.Checked = True
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs)
        'Restore the item selected in the Deleted Items list.
        'Note that the dgvDeletedItems selection mode has been set to FullRowSelect.
        '   This was set in the Form Load method:   Private Sub Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        '   This is the line of code used to set the selection mode:  dgvDeletedItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        If dgvDeletedItems.SelectedRows.Count > 0 Then
            If dgvDeletedItems.SelectedRows.Count > 1 Then
                'More than one row has been selected.
                Main.Message.AddWarning("Select only one deleted item." & vbCrLf)
            Else
                'Restore the deleted item.
                Dim RowNo As Integer = dgvDeletedItems.SelectedRows(0).Index
                Dim ParentNode As String = dgvDeletedItems.Rows(RowNo).Cells(2).Value
                Dim myNode() As TreeNode = trvDocument.Nodes.Find(ParentNode, True)
                Dim NodeRestored As Boolean = False

                If myNode.Length > 0 Then
                    If myNode.Length = 1 Then
                        Dim Index As Integer = dgvDeletedItems.Rows(RowNo).Cells(3).Value
                        Dim NodeText As String = dgvDeletedItems.Rows(RowNo).Cells(0).Value
                        Dim NodeKey As String = dgvDeletedItems.Rows(RowNo).Cells(1).Value
                        If NodeKey.EndsWith(".Libr") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 0, 1) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Coll") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 2, 3) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Book") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 4, 5) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Sect") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 6, 7) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Appl") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 8, 9) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Code") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 10, 11) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Form") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 12, 13) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Pics") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 14, 15) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Pict") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 16, 17) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Note") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 26, 27) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".Proc") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 32, 33) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        ElseIf NodeKey.EndsWith(".rtf") Then
                            myNode(0).Nodes.Insert(Index, NodeKey, NodeText, 36, 37) 'Index, Key, Text, ImageKey, SelectedImageKey
                            NodeRestored = True
                        Else
                            NodeRestored = False
                        End If
                        If NodeRestored = True Then
                            dgvDeletedItems.Rows.RemoveAt(RowNo)
                        End If

                    End If

                End If

            End If
        Else
            'No row has been selected.
            Main.Message.AddWarning("No deleted item has been selected." & vbCrLf)
        End If

    End Sub

    'Private Sub rbUpdate_CheckedChanged(sender As Object, e As EventArgs)
    '    If rbUpdate.Checked = True Then
    '        cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Update")
    '    End If
    'End Sub

    'Private Sub rbUpdate_CheckedChanged_1(sender As Object, e As EventArgs) Handles rbUpdate.CheckedChanged

    'End Sub

    Private Sub rbUpdate_CheckedChanged(sender As Object, e As EventArgs) Handles rbUpdate.CheckedChanged
        If rbUpdate.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Update")
        End If
    End Sub

    Private Sub rbToDo_CheckedChanged(sender As Object, e As EventArgs) Handles rbToDo.CheckedChanged
        If rbToDo.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("To Do List")
        End If
    End Sub

    Private Sub rbNote_CheckedChanged(sender As Object, e As EventArgs) Handles rbNote.CheckedChanged
        If rbNote.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Note")
        End If
    End Sub

    Private Sub rbProcess_CheckedChanged(sender As Object, e As EventArgs) Handles rbProcess.CheckedChanged
        If rbProcess.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Process")
        End If
    End Sub

    Private Sub rbProgress_CheckedChanged(sender As Object, e As EventArgs) Handles rbProgress.CheckedChanged
        If rbProgress.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Progress")
        End If
    End Sub

    Private Sub rbDateTime_CheckedChanged(sender As Object, e As EventArgs) Handles rbDateTime.CheckedChanged
        If rbDateTime.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Date-Time")
        End If
    End Sub

    Private Sub rbVersion_CheckedChanged(sender As Object, e As EventArgs) Handles rbVersion.CheckedChanged
        If rbVersion.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Version")
            rbBookSection.Enabled = True
            rbCode.Enabled = True
            rbForm.Enabled = True
            rbPicture.Enabled = True

            'Select the default version type:
            'Dim Extension As String = System.IO.Path.GetExtension(FileName) 'The Extension shows the type of document:
            'Dim Extension As String = System.IO.Path.GetExtension(SelectedDocumentFileName) 'The Extension shows the type of document:
            Dim Extension As String = System.IO.Path.GetExtension(DocStructure) 'The Extension shows the type of document:
            '.Book - A book
            '.Sect - A book section (usually uses a .rtf file to store the text.)
            '.Appl - An application
            '.Code - Source code
            '.Form - Application form
            '.Pics - A collection of pictures
            '.Pict - A picture
            '.Note - A note
            '.Proc - A process
            '.Libr - A library
            '.Coll - A collection
            '.Time - A Date-Time

            Select Case Extension
                Case ".Sect"
                    rbBookSection.Checked = True
                Case ".Code"
                    rbCode.Checked = True
                Case ".Form"
                    rbForm.Checked = True
                Case ".Pict"
                    rbPicture.Checked = True
            End Select


        Else
            rbBookSection.Enabled = False
            rbCode.Enabled = False
            rbForm.Enabled = False
            rbPicture.Enabled = False
        End If
    End Sub

    Private Sub rbBookSection_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookSection.CheckedChanged
        If rbBookSection.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("  Book Section")
        End If
    End Sub

    Private Sub rbCode_CheckedChanged(sender As Object, e As EventArgs) Handles rbCode.CheckedChanged
        If rbCode.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("  Application Code")
        End If
    End Sub

    Private Sub rbForm_CheckedChanged(sender As Object, e As EventArgs) Handles rbForm.CheckedChanged
        If rbForm.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("  Application Form")
        End If
    End Sub

    Private Sub rbPicture_CheckedChanged(sender As Object, e As EventArgs) Handles rbPicture.CheckedChanged
        If rbPicture.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("  Picture")
        End If
    End Sub

    Private Sub rbRtfDoc_CheckedChanged(sender As Object, e As EventArgs) Handles rbRtfDoc.CheckedChanged
        If rbRtfDoc.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Rich Text Format Document")
        End If
    End Sub

    Private Sub rbModList_CheckedChanged(sender As Object, e As EventArgs) Handles rbModList.CheckedChanged
        If rbModList.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Modification List")
        End If
    End Sub



    Private Sub rbQC_CheckedChanged(sender As Object, e As EventArgs) Handles rbQC.CheckedChanged
        If rbQC.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Quality Control")
        End If
    End Sub

    Private Sub rbChecklist_CheckedChanged(sender As Object, e As EventArgs) Handles rbChecklist.CheckedChanged
        If rbChecklist.Checked = True Then
            cmbNewDocumentItemType.SelectedIndex = cmbNewDocumentItemType.FindStringExact("Checklist")
        End If
    End Sub

    Private Sub btnAddDocumentItem_Click(sender As Object, e As EventArgs) Handles btnAddDocumentItem.Click
        'Add a new library item to the tree view.

        ''For Testing only:
        'Main.Message.AddWarning("Testing - Add library item -----------------------------------" & vbCrLf)
        'Main.Message.Add("cmbNewDocumentItemType.Text = " & cmbNewDocumentItemType.Text & vbCrLf)
        'If cmbNewDocumentItemType.SelectedItem Is Nothing Then
        '    Main.Message.AddWarning("cmbNewDocumentItemType.SelectedItem Is Nothing!" & vbCrLf)
        'Else
        '    Main.Message.Add("cmbNewDocumentItemType.SelectedItem.ToString = " & cmbNewDocumentItemType.SelectedItem.ToString & vbCrLf & vbCrLf)

        'End If

        'Exit Sub 'For testing only

        'NOTE: Use cmbNewDocumentItemType.Text rather than cmbNewDocumentItemType.SelectedItem 
        '      When leaving and re-entering the Add Item tab, the SelectedItem property can be set to Nothing. 
        '      The Text property remains at the last selection.

        If txtNewDocumentItemFileName.Text.Contains(".") Then
            Main.Message.AddWarning("Please enter a fie name without a file extension." & vbCrLf)
            Beep()
            Exit Sub
        End If

        'If cmbNewDocumentItemType.SelectedItem Is Nothing Then
        If cmbNewDocumentItemType.Text = "" Then
            Main.Message.AddWarning("Please select a Document Item type." & vbCrLf)
            Beep()
            Exit Sub
        End If

        If trvDocument.SelectedNode Is Nothing Then
            Select Case cmbNewDocumentItemType.SelectedItem.ToString

                Case "Collection"
                    trvDocument.Nodes.Add(txtNewDocumentItemName.Text.Replace(" ", "_") & ".Coll", txtNewDocumentItemName.Text, 2, 3)

            End Select
        Else

            'Select Case cmbNewDocumentItemType.SelectedItem.ToString
            Select Case cmbNewDocumentItemType.Text
                    'NOTE: Libraries are not used in the Document Structure. !!!!!!!!!!!!!!
                    'This code segment will be removed later
                Case "Library"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Libr"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 0, 1)
                    End If
                        'CreateLibrFile(NewFileName, txtNewLibraryItemDescription.Text)
                    'NOTE: Collections are not used in the Document Structure. !!!!!!!!!!!!!!
                    'This code segment will be removed later
                Case "Collection"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Coll"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 2, 3)
                    End If
                        'CreateCollFile(NewFileName, txtNewLibraryItemDescription.Text)
                    'NOTE: Books are not used in the Document Structure. !!!!!!!!!!!!!!
                    'Books are stored in the main Information Library.
                    'This code segment will be removed later
                Case "Book"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Book"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 4, 5)
                    End If
                        'CreateBookFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                    'NOTE: Rich Text Format Document is now used in the Document Structure to store Book Sections. !!!!!!!!!!!!!!
                    'This code segment will be removed later
                Case "  Section"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Sect"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 6, 7)
                    End If
                        'CreateBookSectionFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                    'NOTE: Applications are not used in the Document Structure. !!!!!!!!!!!!!!
                    'Applications are stored in the main Information Library.
                    'This code segment will be removed later
                Case "Application"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Appl"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 8, 9)
                    End If
                        'CreateApplicationFile(NewFileName, txtNewLibraryItemDescription.Text)
                    'Case "  Source Code"
                    'NOTE: Rich Text Format Document is now used in the Document Structure to store code. !!!!!!!!!!!!!!
                    'This code segment will be removed later
                Case "  Code"
                    'Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Srce"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Code"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 10, 11)
                    End If

                        'CreateApplicationCodeFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                   'NOTE: Rich Text Format Document is now used in the Document Structure to store forms. !!!!!!!!!!!!!!
                    'This code segment will be removed later
                Case "  Form"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Form"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 12, 13)
                    End If
                        'CreateApplicationFormFile(NewFileName, txtNewLibraryItemDescription.Text)
                    'NOTE: Picture Albums are not used in the Document Structure. !!!!!!!!!!!!!!
                     'Picture Albums are stored in the main Information Library.
                     'This code segment will be removed later
                Case "Picture Album"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Pics"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 14, 15)
                    End If
                        'CreatePicAlbumFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                Case "  Picture"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Pict"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 14, 15)
                    End If
                        'CreatePictureFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                Case "Note"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Note"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 26, 27)
                        CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
                    End If
                        'CreateNoteFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                Case "Process"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Proc"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 14, 15)
                    End If
                        'CreateProcessFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                Case "Web"

                Case "  Page"

                Case "Rich Text Format Document"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".rtf"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 36, 37)
                        CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
                        CopyCopyright(trvDocument.SelectedNode.Name, NewFileName)
                        CopyLicense(trvDocument.SelectedNode.Name, NewFileName)
                    End If

                Case "Update"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Update"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 40, 41)
                        CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
                        CopyCopyright(trvDocument.SelectedNode.Name, NewFileName)
                        CopyLicense(trvDocument.SelectedNode.Name, NewFileName)
                    End If

                Case "Modification List"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".ModList"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 83, 84)
                        CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
                        'CopyCopyright(trvDocument.SelectedNode.Name, NewFileName)
                        'CopyLicense(trvDocument.SelectedNode.Name, NewFileName)
                    End If

                Case "To Do List"
                    Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".ToDoList"
                    If Main.Project.DataFileExists(NewFileName) Then
                        Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 44, 45)
                        CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
                        'CopyCopyright(trvDocument.SelectedNode.Name, NewFileName)
                        'CopyLicense(trvDocument.SelectedNode.Name, NewFileName)
                    End If


                Case ""
                    Main.Message.AddWarning("No library item type selected." & vbCrLf)

                Case Else
                    'Main.Message.AddWarning("Unknown library item type: " & cmbNewDocumentItemType.SelectedItem.ToString & vbCrLf)
                    Main.Message.AddWarning("Unknown library item type: " & cmbNewDocumentItemType.Text & vbCrLf)


            End Select
        End If

        'SaveLibrary(txtLibraryFileName.Text)
        SaveStructure(txtStructureFileName.Text)

    End Sub

    '--------------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    'Private Sub btnAddDocumentItem_Click(sender As Object, e As EventArgs)
    '    'Add a new library item to the tree view.

    '    ''For Testing only:
    '    'Main.Message.AddWarning("Testing - Add library item -----------------------------------" & vbCrLf)
    '    'Main.Message.Add("cmbNewDocumentItemType.Text = " & cmbNewDocumentItemType.Text & vbCrLf)
    '    'If cmbNewDocumentItemType.SelectedItem Is Nothing Then
    '    '    Main.Message.AddWarning("cmbNewDocumentItemType.SelectedItem Is Nothing!" & vbCrLf)
    '    'Else
    '    '    Main.Message.Add("cmbNewDocumentItemType.SelectedItem.ToString = " & cmbNewDocumentItemType.SelectedItem.ToString & vbCrLf & vbCrLf)

    '    'End If

    '    'Exit Sub 'For testing only

    '    'NOTE: Use cmbNewDocumentItemType.Text rather than cmbNewDocumentItemType.SelectedItem 
    '    '      When leaving and re-entering the Add Item tab, the SelectedItem property can be set to Nothing. 
    '    '      The Text property remains at the last selection.

    '    If txtNewDocumentItemFileName.Text.Contains(".") Then
    '        Main.Message.AddWarning("Please enter a fie name without a file extension." & vbCrLf)
    '        Beep()
    '        Exit Sub
    '    End If

    '    'If cmbNewDocumentItemType.SelectedItem Is Nothing Then
    '    If cmbNewDocumentItemType.Text = "" Then
    '        Main.Message.AddWarning("Please select a Document Item type." & vbCrLf)
    '        Beep()
    '        Exit Sub
    '    End If

    '    If trvDocument.SelectedNode Is Nothing Then
    '        Select Case cmbNewDocumentItemType.SelectedItem.ToString

    '            Case "Collection"
    '                trvDocument.Nodes.Add(txtNewDocumentItemName.Text.Replace(" ", "_") & ".Coll", txtNewDocumentItemName.Text, 2, 3)

    '        End Select
    '    Else

    '        'Select Case cmbNewDocumentItemType.SelectedItem.ToString
    '        Select Case cmbNewDocumentItemType.Text
    '            'NOTE: Libraries are not used in the Document Structure. !!!!!!!!!!!!!!
    '            'This code segment will be removed later
    '            Case "Library"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Libr"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 0, 1)
    '                End If
    '                'CreateLibrFile(NewFileName, txtNewLibraryItemDescription.Text)
    '            'NOTE: Collections are not used in the Document Structure. !!!!!!!!!!!!!!
    '            'This code segment will be removed later
    '            Case "Collection"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Coll"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 2, 3)
    '                End If
    '                'CreateCollFile(NewFileName, txtNewLibraryItemDescription.Text)
    '            'NOTE: Books are not used in the Document Structure. !!!!!!!!!!!!!!
    '            'Books are stored in the main Information Library.
    '            'This code segment will be removed later
    '            Case "Book"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Book"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 4, 5)
    '                End If
    '                'CreateBookFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '            'NOTE: Rich Text Format Document is now used in the Document Structure to store Book Sections. !!!!!!!!!!!!!!
    '            'This code segment will be removed later
    '            Case "  Section"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Sect"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 6, 7)
    '                End If
    '                'CreateBookSectionFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '            'NOTE: Applications are not used in the Document Structure. !!!!!!!!!!!!!!
    '            'Applications are stored in the main Information Library.
    '            'This code segment will be removed later
    '            Case "Application"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Appl"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 8, 9)
    '                End If
    '                'CreateApplicationFile(NewFileName, txtNewLibraryItemDescription.Text)
    '            'Case "  Source Code"
    '            'NOTE: Rich Text Format Document is now used in the Document Structure to store code. !!!!!!!!!!!!!!
    '            'This code segment will be removed later
    '            Case "  Code"
    '                'Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Srce"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Code"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 10, 11)
    '                End If

    '                'CreateApplicationCodeFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '           'NOTE: Rich Text Format Document is now used in the Document Structure to store forms. !!!!!!!!!!!!!!
    '            'This code segment will be removed later
    '            Case "  Form"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Form"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 12, 13)
    '                End If
    '                'CreateApplicationFormFile(NewFileName, txtNewLibraryItemDescription.Text)
    '            'NOTE: Picture Albums are not used in the Document Structure. !!!!!!!!!!!!!!
    '             'Picture Albums are stored in the main Information Library.
    '             'This code segment will be removed later
    '            Case "Picture Album"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Pics"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 14, 15)
    '                End If
    '                'CreatePicAlbumFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '            Case "  Picture"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Pict"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 14, 15)
    '                End If
    '                'CreatePictureFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '            Case "Note"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Note"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 26, 27)
    '                    CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
    '                End If
    '                'CreateNoteFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '            Case "Process"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Proc"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 14, 15)
    '                End If
    '                'CreateProcessFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
    '            Case "Web"

    '            Case "  Page"

    '            Case "Rich Text Format Document"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".rtf"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 36, 37)
    '                    CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
    '                    CopyCopyright(trvDocument.SelectedNode.Name, NewFileName)
    '                    CopyLicense(trvDocument.SelectedNode.Name, NewFileName)
    '                End If

    '            Case "Update"
    '                Dim NewFileName As String = txtNewDocumentItemFileName.Text.Replace(" ", "_") & ".Update"
    '                If Main.Project.DataFileExists(NewFileName) Then
    '                    Main.Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
    '                Else
    '                    trvDocument.SelectedNode.Nodes.Add(NewFileName, txtNewDocumentItemName.Text, 40, 41)
    '                    CopyAuthor(trvDocument.SelectedNode.Name, NewFileName)
    '                    CopyCopyright(trvDocument.SelectedNode.Name, NewFileName)
    '                    CopyLicense(trvDocument.SelectedNode.Name, NewFileName)
    '                End If

    '            Case ""
    '                Main.Message.AddWarning("No library item type selected." & vbCrLf)

    '            Case Else
    '                'Main.Message.AddWarning("Unknown library item type: " & cmbNewDocumentItemType.SelectedItem.ToString & vbCrLf)
    '                Main.Message.AddWarning("Unknown library item type: " & cmbNewDocumentItemType.Text & vbCrLf)


    '        End Select
    '    End If

    '    'SaveLibrary(txtLibraryFileName.Text)
    '    SaveStructure(txtStructureFileName.Text)

    'End Sub

    Private Sub CopyAuthor(ByVal FromItem As String, ByVal ToItem As String)
        'Copy the Author settings from the FromItem node name to the ToItem node name.

        If FromItem = "" Then
            Main.Message.AddWarning("Can not copy the Author settings. The source node is not defined." & vbCrLf)
            Exit Sub
        End If
        If ToItem = "" Then
            Main.Message.AddWarning("Can not copy the Author settings. The destination node is not defined." & vbCrLf)
            Exit Sub
        End If

        If FromItem = DocStructure Then 'FromItem is the root node.
            If DocItem.ContainsKey(ToItem) Then
                DocItem(ToItem).AuthorFile = DefaultAuthorFile
                DocItem(ToItem).AuthorSummary = DefaultAuthorSummary
            Else
                DocItem.Add(ToItem, New DocItemInfo)
                DocItem(ToItem).Description = txtNewDocumentItemDescription.Text
                DocItem(ToItem).CreationDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                DocItem(ToItem).LastEditDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                DocItem(ToItem).AuthorFile = DefaultAuthorFile
                DocItem(ToItem).AuthorSummary = DefaultAuthorSummary
            End If
        Else
            If DocItem.ContainsKey(FromItem) Then
                If DocItem.ContainsKey(ToItem) Then
                    DocItem(ToItem).AuthorFile = DocItem(FromItem).AuthorFile
                    DocItem(ToItem).AuthorSummary = DocItem(FromItem).AuthorSummary
                Else
                    DocItem.Add(ToItem, New DocItemInfo)
                    DocItem(ToItem).Description = txtNewDocumentItemDescription.Text
                    DocItem(ToItem).CreationDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                    DocItem(ToItem).LastEditDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                    DocItem(ToItem).AuthorFile = DocItem(FromItem).AuthorFile
                    DocItem(ToItem).AuthorSummary = DocItem(FromItem).AuthorSummary
                End If
            Else
                Main.Message.AddWarning("Can not copy the Author settings. The source node Author is not defined." & vbCrLf)
                Main.Message.AddWarning("The source node name:" & FromItem & vbCrLf)
            End If
        End If

    End Sub

    Private Sub CopyCopyright(ByVal FromItem As String, ByVal ToItem As String)
        'Copy the Copyright settings from the FromItem node name to the ToItem node name.

        If FromItem = "" Then
            Main.Message.AddWarning("Cannot copy the Copyright settings. The source node is not defined." & vbCrLf)
            Exit Sub
        End If
        If ToItem = "" Then
            Main.Message.AddWarning("Cannot copy the Copyright settings. The destination node is not defined." & vbCrLf)
            Exit Sub
        End If

        If FromItem = DocStructure Then 'FromItem is the root node.
            If Copyright.ContainsKey(ToItem) Then
                Copyright(ToItem).File = DefaultCopyrightFile
                Copyright(ToItem).Summary = DefaultCopyrightSummary
            Else
                Copyright.Add(ToItem, New CopyrightInfo)
                Copyright(ToItem).File = DefaultCopyrightFile
                Copyright(ToItem).Summary = DefaultCopyrightSummary
            End If
        Else
            If Copyright.ContainsKey(FromItem) Then
                If Copyright.ContainsKey(ToItem) Then
                    Copyright(ToItem).File = Copyright(FromItem).File
                    Copyright(ToItem).Summary = Copyright(FromItem).Summary
                Else
                    Copyright.Add(ToItem, New CopyrightInfo)
                    Copyright(ToItem).File = Copyright(FromItem).File
                    Copyright(ToItem).Summary = Copyright(FromItem).Summary
                End If
            Else
                Main.Message.AddWarning("Can not copy the Copyright settings. The source node Copyright is not defined." & vbCrLf)
                Main.Message.AddWarning("The source node name:" & FromItem & vbCrLf)
            End If
        End If

    End Sub

    Private Sub CopyLicense(ByVal FromItem As String, ByVal ToItem As String)
        'Copy the License settings from the FromItem node name to the ToItem node name.

        If FromItem = "" Then
            Main.Message.AddWarning("Cannot copy the License settings. The source node is not defined." & vbCrLf)
            Exit Sub
        End If
        If ToItem = "" Then
            Main.Message.AddWarning("Cannot copy the License settings. The destination node is not defined." & vbCrLf)
            Exit Sub
        End If

        If FromItem = DocStructure Then  'FromItem is the root node.
            If License.ContainsKey(ToItem) Then
                License(ToItem).File = DefaultLicenseFile
                License(ToItem).Summary = DefaultLicenseSummary
            Else
                License.Add(ToItem, New LicenseInfo)
                License(ToItem).File = DefaultLicenseFile
                License(ToItem).Summary = DefaultLicenseSummary
            End If
        Else
            If License.ContainsKey(FromItem) Then
                If License.ContainsKey(ToItem) Then
                    License(ToItem).File = License(FromItem).File
                    License(ToItem).Summary = License(FromItem).Summary
                Else
                    License.Add(ToItem, New LicenseInfo)
                    License(ToItem).File = License(FromItem).File
                    License(ToItem).Summary = License(FromItem).Summary
                End If
            Else
                Main.Message.AddWarning("Can not copy the License settings. The source node License is not defined." & vbCrLf)
                Main.Message.AddWarning("The source node name:" & FromItem & vbCrLf)
            End If
        End If

    End Sub

    Private Sub btnUpdateDescr_Click(sender As Object, e As EventArgs)
        'Update the node description

        If DocItem.ContainsKey(txtNodeKey.Text) Then
            DocItem(txtNodeKey.Text).Description = txtNodeDescription.Text
        End If

    End Sub

    Private Sub pbIconUpdate_Click(sender As Object, e As EventArgs) Handles pbIconUpdate.Click
        rbUpdate.Checked = True
    End Sub

    Private Sub pbIconToDo_Click(sender As Object, e As EventArgs) Handles pbIconToDo.Click
        'rbToDo.Checked = True
    End Sub

    Private Sub pbIconProgress_Click(sender As Object, e As EventArgs) Handles pbIconProgress.Click
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconProcess_Click(sender As Object, e As EventArgs) Handles pbIconProcess.Click
        rbProgress.Checked = True
    End Sub

    Private Sub pbIconDateTime_Click(sender As Object, e As EventArgs) Handles pbIconDateTime.Click
        'rbDateTime.Checked = True
        rbDateTime.Checked = True
    End Sub

    Private Sub pbIconNote_Click(sender As Object, e As EventArgs) Handles pbIconNote.Click
        rbNote.Checked = True
    End Sub

    Private Sub pbIconVersion_Click(sender As Object, e As EventArgs) Handles pbIconVersion.Click
        rbVersion.Checked = True
    End Sub

    Private Sub pbIconAuthor_Click(sender As Object, e As EventArgs) Handles pbIconAuthor.Click
        rbModList.Checked = True
    End Sub

    Private Sub pbIconQC_Click(sender As Object, e As EventArgs) Handles pbIconQC.Click
        rbQC.Checked = True
    End Sub

    Private Sub pbIconChecklist_Click(sender As Object, e As EventArgs) Handles pbIconChecklist.Click
        rbChecklist.Checked = True
    End Sub

    Private Sub pbIconRtfDoc_Click(sender As Object, e As EventArgs) Handles pbIconRtfDoc.Click
        rbRtfDoc.Checked = True
    End Sub

    Private Sub LicenseInfo_ApplyAuthor(AuthorFile As String, AuthorSummary As String) Handles LicenseInfo.ApplyAuthor
        txtAuthorFile.Text = AuthorFile
        txtAuthorSummary.Text = AuthorSummary

        If trvDocument.SelectedNode.Parent Is Nothing Then 'Root node.
            'Me.AuthorFile = AuthorFile
            DefaultAuthorFile = AuthorFile
            'Me.AuthorSummary = AuthorSummary
            DefaultAuthorSummary = AuthorSummary
        Else
            If DocItem.ContainsKey(trvDocument.SelectedNode.Name) Then
                DocItem(trvDocument.SelectedNode.Name).AuthorFile = AuthorFile
                DocItem(trvDocument.SelectedNode.Name).AuthorSummary = AuthorSummary
            Else
                DocItem.Add(trvDocument.SelectedNode.Name, New DocItemInfo)
                DocItem(trvDocument.SelectedNode.Name).AuthorFile = AuthorFile
                DocItem(trvDocument.SelectedNode.Name).AuthorSummary = AuthorSummary
            End If
        End If
    End Sub

    Private Sub LicenseInfo_ApplyCopyright(CopyrightFile As String, CopyrightSummary As String) Handles LicenseInfo.ApplyCopyright
        txtCopyrightFile.Text = CopyrightFile
        txtCopyrightSummary.Text = CopyrightSummary

        If trvDocument.SelectedNode.Parent Is Nothing Then 'Root node.
            'Me.CopyrightFile = CopyrightFile
            DefaultCopyrightFile = CopyrightFile
            'Me.CopyrightSummary = CopyrightSummary
            DefaultCopyrightSummary = CopyrightSummary
        Else
            If Copyright.ContainsKey(trvDocument.SelectedNode.Name) Then
                Copyright(trvDocument.SelectedNode.Name).File = CopyrightFile
                Copyright(trvDocument.SelectedNode.Name).Summary = CopyrightSummary
            Else
                Copyright.Add(trvDocument.SelectedNode.Name, New CopyrightInfo)
                Copyright(trvDocument.SelectedNode.Name).File = CopyrightFile
                Copyright(trvDocument.SelectedNode.Name).Summary = CopyrightSummary
            End If
        End If
    End Sub

    Private Sub LicenseInfo_ApplyLicense(LicenseFile As String, LicenseSummary As String) Handles LicenseInfo.ApplyLicense
        txtLicenseFile.Text = LicenseFile
        rtbLicenseNotice.Text = LicenseSummary

        If trvDocument.SelectedNode.Parent Is Nothing Then 'Root node.
            'Me.LicenseFile = LicenseFile
            DefaultLicenseFile = LicenseFile
            'Me.LicenseSummary = LicenseSummary
            DefaultLicenseSummary = LicenseSummary
        Else
            If License.ContainsKey(trvDocument.SelectedNode.Name) Then
                License(trvDocument.SelectedNode.Name).File = LicenseFile
                License(trvDocument.SelectedNode.Name).Summary = LicenseSummary
            Else
                License.Add(trvDocument.SelectedNode.Name, New LicenseInfo)
                License(trvDocument.SelectedNode.Name).File = LicenseFile
                License(trvDocument.SelectedNode.Name).Summary = LicenseSummary
            End If
        End If
    End Sub

    Private Sub txtNodeDescription_LostFocus(sender As Object, e As EventArgs) Handles txtNodeDescription.LostFocus
        'The Node description has changed:
        'Apply the change:
        If trvDocument.SelectedNode.Parent Is Nothing Then 'Root node.
            Description = txtNodeDescription.Text
        Else
            If DocItem.ContainsKey(trvDocument.SelectedNode.Name) Then
                DocItem(trvDocument.SelectedNode.Name).Description = txtNodeDescription.Text
            Else
                DocItem.Add(trvDocument.SelectedNode.Name, New DocItemInfo)
                DocItem(trvDocument.SelectedNode.Name).Description = txtNodeDescription.Text
            End If
            'ElseIf trvDocument.SelectedNode.Name.EndsWith(".Update") Then 'Update node.
            '    If dictUpdateInfo.ContainsKey(trvDocument.SelectedNode.Name) Then
            '        dictUpdateInfo(trvDocument.SelectedNode.Name).Description = txtNodeDescription.Text
            '    Else
            '        dictUpdateInfo.Add(trvDocument.SelectedNode.Name, New UpdateInfo)
            '        dictUpdateInfo(trvDocument.SelectedNode.Name).Description = txtNodeDescription.Text
            '    End If
            'Else 'Version node??? 'Check for other non-version node types: .CList, .QC etc !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            '    If dictVersionInfo.ContainsKey(trvDocument.SelectedNode.Name) Then
            '        dictVersionInfo(trvDocument.SelectedNode.Name).Description = txtNodeDescription.Text
            '    Else
            '        dictVersionInfo.Add(trvDocument.SelectedNode.Name, New VersionInfo)
            '        dictVersionInfo(trvDocument.SelectedNode.Name).Description = txtNodeDescription.Text
            '    End If

        End If

    End Sub

    Private Sub btnSaveDocumentChanges_Click(sender As Object, e As EventArgs) Handles btnSaveDocumentChanges.Click
        'Save the changes made to the document.
        SaveSelectedDocument()
    End Sub



    Private Sub btnCancelDocumentChanges_Click(sender As Object, e As EventArgs) Handles btnCancelDocumentChanges.Click
        'Cancel the changes made to the document.
        OpenSelectedDocument()

    End Sub


    Private Sub rtbDocument_LostFocus(sender As Object, e As EventArgs) Handles rtbDocument.LostFocus
        'Focus has left the Document rich text box.

        'SaveSelectedDocument() 'NOTE: This is now done when the tab has lost focus. This leaves the option of cancelling the changes by pressing the Cancel button. 
        '(If SaveSelectedDocument is called here, the document is saved before the Cancel button can be pressed.)

    End Sub


    Private Sub TabPage6_LostFocus(sender As Object, e As EventArgs) Handles TabPage6.LostFocus
        'Focus has left the Document rich text box tab.
        'SaveSelectedDocument()
        'Main.Message.Add("Selected document saved." & vbCrLf)
        'NOTE: This doesnt work!!!
    End Sub

    Private Sub TabPage6_Click(sender As Object, e As EventArgs) Handles TabPage6.Click

    End Sub

    Private Sub TabPage6_Leave(sender As Object, e As EventArgs) Handles TabPage6.Leave
        'Leaving the Document rich text box tab.
        SaveSelectedDocument()
        Main.Message.Add("Selected document saved." & vbCrLf)
    End Sub

    Private Sub btnSetDefault_Click(sender As Object, e As EventArgs) Handles btnSetDefault.Click
        'Set the selected Updfate/Version as he default
        SetAsDefault()
    End Sub

    Private Sub SetAsDefault()
        'Set the selected Update/Version of the document as the default.

        Dim DocInfo As New DefaultDocInfo

        'DocInfo.Label = txtDocLabel.Text 'NOTE: This is the version label. The Document structure label is more appropriate. See code below.
        DocInfo.Label = Label
        DocInfo.FileName = txtFileName.Text
        DocInfo.Description = txtNodeDescription.Text
        DocInfo.CreationDate = txtCreationDate.Text
        DocInfo.LastEditDate = txtLastEditDate.Text
        DocInfo.AuthorFile = txtAuthorFile.Text
        DocInfo.AuthorSummary = txtAuthorSummary.Text
        DocInfo.CopyrightFile = txtCopyrightFile.Text
        DocInfo.CopyrightSummary = txtCopyrightSummary.Text
        DocInfo.LicenseFile = txtLicenseFile.Text
        DocInfo.LicenseSummary = rtbLicenseNotice.Text

        Main.SetDefaultDocInfo(DocStructure, DocInfo)
    End Sub



#End Region 'Updates and Versions -------------------------------------------------------------------------------------------------------------------------------------------------------------

#End Region 'Form Methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------

End Class

Public Class DocItemInfo
    'Document Item information.
    'A document item is one of the following nodes: Update, Version, Note, ...

    Private _description As String = "" 'A description of the item. (Default value is "".)
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the item. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the item. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property

    Private _authorFile As String = "" 'The name of a .Author file containing information about the author.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
        End Set
    End Property

    Private _authorSummary As String = "" 'A summary of the document update author. The full details are stored in the Author File.
    Property AuthorSummary As String
        Get
            Return _authorSummary
        End Get
        Set(value As String)
            _authorSummary = value
        End Set
    End Property

End Class

Public Class UpdateInfo_Old2
    'Update information.

    Private _description As String = "" 'A description of the item. (Default value is "".)
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the item. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the item. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property

    Private _authorFile As String = "" 'The name of a .Author file containing information about the author.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
        End Set
    End Property

    Private _authorSummary As String = "" 'A summary of the document update author. The full details are stored in the Author File.
    Property AuthorSummary As String
        Get
            Return _authorSummary
        End Get
        Set(value As String)
            _authorSummary = value
        End Set
    End Property

End Class

Public Class VersionInfo_Old2
    'Version information.

    Private _description As String = "" 'A description of the item. (Default value is "".)
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the item. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the item. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property

    Private _authorFile As String = "" 'The name of a .Author file containing information about the author.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
        End Set
    End Property

    Private _authorSummary As String = "" 'A summary of the document version author. The full details are stored in the Author File.
    Property AuthorSummary As String
        Get
            Return _authorSummary
        End Get
        Set(value As String)
            _authorSummary = value
        End Set
    End Property

End Class

'NOTE: Author information is now included in the Update and Version dictionaries.
Public Class AuthorInfo_Old
    'Author information.

    Private _file As String = "" 'The name of a .Author file containing information about the author.
    Property File As String
        Get
            Return _file
        End Get
        Set(value As String)
            _file = value
        End Set
    End Property

    Private _summary As String = "" 'A summary of the document version author. The full details are stored in the Author File.
    Property Summary As String
        Get
            Return _summary
        End Get
        Set(value As String)
            _summary = value
        End Set
    End Property

End Class

Public Class CopyrightInfo
    'Copyright information.

    Private _file As String = "" 'The name of a .Copyright file containing information about the copyright.
    Property File As String
        Get
            Return _file
        End Get
        Set(value As String)
            _file = value
        End Set
    End Property

    Private _summary As String = "" 'A summary of the document version copyright. The full details are stored in the Copyright File.
    Property Summary As String
        Get
            Return _summary
        End Get
        Set(value As String)
            _summary = value
        End Set
    End Property

End Class

Public Class LicenseInfo
    'License information.

    Private _file As String = "" 'The name of a .License file containing information about the license.
    Property File As String
        Get
            Return _file
        End Get
        Set(value As String)
            _file = value
        End Set
    End Property

    Private _summary As String = "" 'A summary of the document version license. The full details are stored in the License File.
    Property Summary As String
        Get
            Return _summary
        End Get
        Set(value As String)
            _summary = value
        End Set
    End Property

End Class

Public Class UpdateInfo_Old
    'Document update information.
    'The Text of the update is stored in the TreeView node.
    'The Name of the Update is stored in the TreeView node.

    Private _description As String = "" 'A description of the item. (Default value is "".)
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the item. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the item. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property

    Private _authorFile As String = "" 'The name of a .Auth file containing information about the author.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
        End Set
    End Property

    Private _authorSummary As String = "" 'A summary of the document version author. The full details are stored in the Author File.
    Property AuthorSummary As String
        Get
            Return _authorSummary
        End Get
        Set(value As String)
            _authorSummary = value
        End Set
    End Property

    Private _copyrightFile As String = "" 'The name of a .Copy file containing the copyright notice.
    Property CopyrightFile As String
        Get
            Return _copyrightFile
        End Get
        Set(value As String)
            _copyrightFile = value
        End Set
    End Property

    Private _copyrightSummary As String = "" 'A summary of the copyright. The full copyright details are stored in the Copyright file.
    Property CopyrightSummary As String
        Get
            Return _copyrightSummary
        End Get
        Set(value As String)
            _copyrightSummary = value
        End Set
    End Property

    Private _licenseFile As String = "" 'The name of a .Lic file containing the software, data or media license.
    Property LicenseFile As String
        Get
            Return _licenseFile
        End Get
        Set(value As String)
            _licenseFile = value
        End Set
    End Property

    Private _licenseSummary As String = "" 'A summary of the license. The full license details are stored in the License file.
    Property LicenseSummary As String
        Get
            Return _licenseSummary
        End Get
        Set(value As String)
            _licenseSummary = value
        End Set
    End Property

End Class

'The ItemInfo class is used to store information fields in the ItemInfo Dictionary.
'Public Class DocInfo
'Public Class ItemInfo
'Document TreeView Item Information.
'An Item can be the Root node, and Update node or a Document version.
'Information in a Root node is used as the default for all child nodes unless these nodes have their own information.
'Information in an Update node is used as the deafult for all child document versions unless these documents have their own information.

'The Text of the Item is stored in the TreeView node.
'The Name of the Item is stored in the TreeView node.
Public Class VersionInfo_Old
    'Document version information.
    'The Text if the version is stored in the TreeView node.
    'The Name of the version is stored in the TreeView node.

    Private _description As String = "" 'A description of the item. (Default value is "".)
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the item. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the item. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property

    Private _authorFile As String = "" 'The name of a .Auth file containing information about the author.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
        End Set
    End Property

    Private _authorSummary As String = "" 'A summary of the document version author. The full details are stored in the Author File.
    Property AuthorSummary As String
        Get
            Return _authorSummary
        End Get
        Set(value As String)
            _authorSummary = value
        End Set
    End Property

    Private _copyrightFile As String = "" 'The name of a .Copy file containing the copyright notice.
    Property CopyrightFile As String
        Get
            Return _copyrightFile
        End Get
        Set(value As String)
            _copyrightFile = value
        End Set
    End Property

    Private _copyrightSummary As String = "" 'A summary of the copyright. The full copyright details are stored in the Copyright file.
    Property CopyrightSummary As String
        Get
            Return _copyrightSummary
        End Get
        Set(value As String)
            _copyrightSummary = value
        End Set
    End Property

    Private _licenseFile As String = "" 'The name of a .Lic file containing the software, data or media license.
    Property LicenseFile As String
        Get
            Return _licenseFile
        End Get
        Set(value As String)
            _licenseFile = value
        End Set
    End Property

    Private _licenseSummary As String = "" 'A summary of the license. The full license details are stored in the License file.
    Property LicenseSummary As String
        Get
            Return _licenseSummary
        End Get
        Set(value As String)
            _licenseSummary = value
        End Set
    End Property

End Class