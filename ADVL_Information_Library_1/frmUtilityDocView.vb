Public Class frmUtilityDocView
    'This form is used to view and edit utility documents.

#Region " Variable Declarations - All the variables used in this form and this application." '=================================================================================================

    'Private DocItem As New Dictionary(Of String, DocItemInfo)     'Dictionary if information about each document item (Update, Version, Note, ...)


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

    Private _docFileName As String = "" 'The name of the Utility Document file.
    Property DocFilename As String
        Get
            Return _docFileName
        End Get
        Set(value As String)
            _docFileName = value
            'txtStructureFileName.Text = _docStructure 'Updates and Versions tab \ Structure sub tab \ Document structure file groupbox - File name.

            'Dim XDocLib As XDocument
            'Main.Project.ReadXmlData(_docFileName, XDocLib)
            'OpenDocStructure(XDocLib)

            txtDocumentFileName.Text = _docFileName
            OpenDocument
        End Set
    End Property

    Private _docLabel As String = "" 'The Document label.
    Property DocLabel As String
        Get
            Return _docLabel
        End Get
        Set(value As String)
            _docLabel = value
            'txtDocStrucLabel.Text = _label
            txtDocLabel.Text = _docLabel
        End Set
    End Property

    Private _description As String = "" 'A description of the document.
    Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
            'txtDocStrucDescription.Text = _description 'Updates and Versions tab \ Structure sub tab \ Description taxt box
        End Set
    End Property

    Private _creationDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the document. (Default value is Now.)
    Property CreationDate As DateTime
        Get
            Return _creationDate
        End Get
        Set(value As DateTime)
            _creationDate = value
        End Set
    End Property

    Private _lastEditDate As DateTime = Format(Now, "d-MMM-yyyy H:mm:ss") 'The last edit date of the document. (Default value is Now.)
    Property LastEditDate As DateTime
        Get
            Return _lastEditDate
        End Get
        Set(value As DateTime)
            _lastEditDate = value
        End Set
    End Property


    Private _authorFile As String = "" 'The name of a .Auth file containing information about the author of the document.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
        End Set
    End Property

    Private _authorSummary As String = "" 'A summary of the document author. The full details are stored in the Author File.
    Property AuthorSummary As String
        Get
            Return _authorSummary
        End Get
        Set(value As String)
            _authorSummary = value
        End Set
    End Property


#End Region 'Properties -----------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Process XML files - Read and write XML files." '=====================================================================================================================================

    Private Sub SaveFormSettings()
        'This SaveFormSettings method saves the settings in Main.UtilViewSettings.List

        'If FormNo + 1 > Main.DocViewList.Count Then
        If FormNo + 1 > Main.UtilViewList.Count Then
            Main.Message.AddWarning("Form number: " & FormNo & " does not exist in the Utility Document View Settings List!" & vbCrLf)
        Else
            'Save the form settings:

            ''Main.DocViewSettings.List(FormNo).Left = Me.Left
            'Main.UtilViewSettings.List(FormNo).Left = Me.Left
            'Main.UtilViewSettings.List(FormNo).Top = Me.Top
            'Main.UtilViewSettings.List(FormNo).Width = Me.Width
            'Main.UtilViewSettings.List(FormNo).Height = Me.Height
            'Main.UtilViewSettings.List(FormNo).FileName = ""

        End If
    End Sub

    Private Sub RestoreFormSettings()
        'This RestoreFormSettings method restores the settings from Main.SharePricesSettings.List

        'If FormNo + 1 > Main.DocViewSettings.List.Count Then
        'If FormNo + 1 > Main.UtilViewSettings.List.Count Then
        '    'Main.Message.AddWarning("Form number: " & FormNo & " does not exist in the Share Prices Settings List!" & vbCrLf)
        '    'Add form entry to the Share Prices Settings list.
        '    Dim NewSettings As New DVSettings
        '    'Main.DocViewSettings.InsertSettings(FormNo, NewSettings)
        '    Main.UtilViewSettings.InsertSettings(FormNo, NewSettings)
        'Else
        '    'Restore the form settings:
        '    'Me.Left = Main.DocViewSettings.List(FormNo).Left
        '    Me.Left = Main.UtilViewSettings.List(FormNo).Left
        '    Me.Top = Main.UtilViewSettings.List(FormNo).Top
        '    Me.Width = Main.UtilViewSettings.List(FormNo).Width
        '    Me.Height = Main.UtilViewSettings.List(FormNo).Height

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

    Private Sub frmUtilityDocView_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Initialization code executed after the form is loaded.

        'Note that the application that opens this form will also set the DocFileName  and DocLabel properties after the form is opened.

        RestoreFormSettings()   'Restore the form settings
        rtbDocument.ContextMenuStrip = ContextMenuStrip1

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit the Form
        Main.ClosedFormNo = FormNo 'The Main form property ClosedFormNo is set to this form number. This is used in the DocumentViewFormClosed method to select the correct form to set to nothing.

        'Save the DocumentSettings
        'If Main.DocDisplayInfo.ContainsKey(DocStructure) Then
        'If Main.UtilDisplayInfo.ContainsKey(DocFilename) Then
        If Main.DocDisplayInfo.ContainsKey(DocFilename) Then
            Main.DocDisplayInfo(DocFilename).Left = Me.Left
            Main.DocDisplayInfo(DocFilename).Top = Me.Top
            Main.DocDisplayInfo(DocFilename).Width = Me.Width
            Main.DocDisplayInfo(DocFilename).Height = Me.Height
        Else
            'Main.DocDisplayInfo.Add(DocStructure, New DocumentInfo)
            'Main.UtilDisplayInfo.Add(DocFilename, New DocumentInfo)
            Main.DocDisplayInfo.Add(DocFilename, New DocumentInfo)
            Main.DocDisplayInfo(DocFilename).Left = Me.Left
            Main.DocDisplayInfo(DocFilename).Top = Me.Top
            Main.DocDisplayInfo(DocFilename).Width = Me.Width
            Main.DocDisplayInfo(DocFilename).Height = Me.Height
        End If

        'SaveStructureFile()

        Me.Close() 'Close the form

    End Sub

    Private Sub frmUtilityDocView_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If WindowState = FormWindowState.Normal Then
            SaveFormSettings()
        Else
            'Dont save settings if form is minimised.
        End If

    End Sub


    Private Sub frmUtilityDocView_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Main.UtilityDocViewFormClosed()
    End Sub

#End Region 'Form Display Methods -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Open and Close Forms - Code used to open and close other forms." '===================================================================================================================
#End Region 'Open and Close Forms -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Methods - The main actions performed by this form." '===========================================================================================================================

    Private Sub OpenDocument()
        'Open the document stored in DocFileName.

        If DocFilename.EndsWith(".Note") Then
            rtbDocument.Show()
            dgvDocument.Hide()
            rtbDocument.Clear()
            'rtbDocument.LoadFile(DocFilename)
            If Main.Project.DataFileExists(DocFilename) Then
                Dim rtbData As New IO.MemoryStream()
                Main.Project.ReadData(DocFilename, rtbData)
                rtbData.Position = 0
                rtbDocument.LoadFile(rtbData, RichTextBoxStreamType.RichText)
                'rtbDocument.BackColor = Color.FromArgb(250, 250, 250)
                rtbDocument.BackColor = Color.LemonChiffon

                'Set the form position and size:
                If Main.DocDisplayInfo.ContainsKey(DocFilename) Then
                    Me.Left = Main.DocDisplayInfo(DocFilename).Left
                    Me.Top = Main.DocDisplayInfo(DocFilename).Top
                    Me.Width = Main.DocDisplayInfo(DocFilename).Width
                    Me.Height = Main.DocDisplayInfo(DocFilename).Height

                End If

            Else
                Main.Message.AddWarning("File not found." & vbCrLf)
            End If
        End If

    End Sub

    Private Sub btnSaveDocumentChanges_Click(sender As Object, e As EventArgs) Handles btnSaveDocumentChanges.Click
        SaveDocument()
    End Sub

    Private Sub SaveDocument()
        'Save the Utility Document.

        If DocFilename.EndsWith(".Note") Then
            Dim rtbData As New IO.MemoryStream
            rtbDocument.SaveFile(rtbData, RichTextBoxStreamType.RichText)
            rtbData.Position = 0
            Main.Project.SaveData(DocFilename, rtbData)
        End If

    End Sub

    Private Sub FontToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FontToolStripMenuItem.Click
        FontDialog1.ShowColor = True
        If FontDialog1.ShowDialog <> DialogResult.Cancel Then
            rtbDocument.SelectionFont = FontDialog1.Font
            rtbDocument.SelectionColor = FontDialog1.Color
        End If
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click

        rtbDocument.Paste()

    End Sub







#End Region 'Form Methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Events - Events that can be triggered by this form." '==========================================================================================================================
#End Region 'Form Events ----------------------------------------------------------------------------------------------------------------------------------------------------------------------



End Class