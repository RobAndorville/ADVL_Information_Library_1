Public Class frmNewLibrary
    'This form is used to create a new Information Library.

#Region " Variable Declarations - All the variables used in this form and this application." '=================================================================================================

    Public WithEvents LicenseInfo As frmLicense

#End Region 'Variable Declarations ------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region " Properties - All the properties used in this form and this application" '============================================================================================================

#End Region 'Properties -----------------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region " Process XML files - Read and write XML files." '=====================================================================================================================================

    Private Sub SaveFormSettings()
        'Save the form settings in an XML document.
        Dim settingsData = <?xml version="1.0" encoding="utf-8"?>
                           <!---->
                           <FormSettings>
                               <Left><%= Me.Left %></Left>
                               <Top><%= Me.Top %></Top>
                               <Width><%= Me.Width %></Width>
                               <Height><%= Me.Height %></Height>
                               <!---->
                           </FormSettings>

        'Add code to include other settings to save after the comment line <!---->

        Dim SettingsFileName As String = "FormSettings_" & Main.ApplicationInfo.Name & "_" & Me.Text & ".xml"
        Main.Project.SaveXmlSettings(SettingsFileName, settingsData)
    End Sub

    Private Sub RestoreFormSettings()
        'Read the form settings from an XML document.

        Dim SettingsFileName As String = "FormSettings_" & Main.ApplicationInfo.Name & "_" & Me.Text & ".xml"

        If Main.Project.SettingsFileExists(SettingsFileName) Then
            Dim Settings As System.Xml.Linq.XDocument
            Main.Project.ReadXmlSettings(SettingsFileName, Settings)

            If IsNothing(Settings) Then 'There is no Settings XML data.
                Exit Sub
            End If

            'Restore form position and size:
            If Settings.<FormSettings>.<Left>.Value <> Nothing Then Me.Left = Settings.<FormSettings>.<Left>.Value
            If Settings.<FormSettings>.<Top>.Value <> Nothing Then Me.Top = Settings.<FormSettings>.<Top>.Value
            If Settings.<FormSettings>.<Height>.Value <> Nothing Then Me.Height = Settings.<FormSettings>.<Height>.Value
            If Settings.<FormSettings>.<Width>.Value <> Nothing Then Me.Width = Settings.<FormSettings>.<Width>.Value

            'Add code to read other saved setting here:

        End If
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

    'Private Sub frmTemplate_Load(sender As Object, e As EventArgs) Handles Me.Load
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        RestoreFormSettings()   'Restore the form settings

        pbIconDefaultAuthor.Image = Main.ImageList1.Images(75)
        pbIconDefaultCopyright.Image = Main.ImageList1.Images(81)
        pbIconDefaultLicense.Image = Main.ImageList1.Images(69)

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit the Form
        Me.Close() 'Close the form
    End Sub

    'Private Sub frmTemplate_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If WindowState = FormWindowState.Normal Then
            SaveFormSettings()
        Else
            'Dont save settings if form is minimised.
        End If
    End Sub

#End Region 'Form Display Methods -------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region " Open and Close Forms - Code used to open and close other forms." '===================================================================================================================

    Private Sub btnEditDefaultAuthor_Click(sender As Object, e As EventArgs) Handles btnEditDefaultAuthor.Click
        'Open the LicenseInfo form at the Author tab.

        If IsNothing(LicenseInfo) Then
            LicenseInfo = New frmLicense
            LicenseInfo.Show()
            LicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LicenseInfo.AuthorFile = txtDefaultAuthorFile.Text 'Setting the Author file last leaves the Author tab open
        Else
            LicenseInfo.Show()
            LicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LicenseInfo.AuthorFile = txtDefaultAuthorFile.Text 'Setting the Author file last leaves the Author tab open
        End If
    End Sub

    Private Sub btnEditDefaultCopyright_Click(sender As Object, e As EventArgs) Handles btnEditDefaultCopyright.Click
        'Open the LicenseInfo form at the Copyright tab.

        If IsNothing(LicenseInfo) Then
            LicenseInfo = New frmLicense
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
        Else
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
        End If
    End Sub

    Private Sub btnEditDefaultLicense_Click(sender As Object, e As EventArgs) Handles btnEditDefaultLicense.Click
        'Open the LicenseInfo form at the License tab.

        If IsNothing(LicenseInfo) Then
            LicenseInfo = New frmLicense
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LicenseInfo.LicenseFile = txtDefaultLicenseFile.Text 'Setting the License file last leaves the License tab open
            'LicenseInfo.OpenLicenseFile()
        Else
            LicenseInfo.Show()
            LicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LicenseInfo.LicenseFile = txtDefaultLicenseFile.Text 'Setting the License file last leaves the License tab open
            'LicenseInfo.OpenLicenseFile()
        End If
    End Sub

    Private Sub LicenseInfo_FormClosed(sender As Object, e As FormClosedEventArgs) Handles LicenseInfo.FormClosed
        LicenseInfo = Nothing
    End Sub

#End Region 'Open and Close Forms -------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region " Form Methods - The main actions performed by this form." '===========================================================================================================================

    Private Sub btnNewLibrary_Click(sender As Object, e As EventArgs) Handles btnNewLibrary.Click
        'Create a new library.
        'txtNewLibraryName.Text contains the new library name.
        'txtNewLibraryDescription.Text contains the new library description.
        'txtNewLibraryFileName.Text contains the new library file name.

        'Check if the Library List file exists. LibraryList.xml

        'Check new library file name:
        txtNewLibraryFileName.Text = Trim(txtNewLibraryFileName.Text) 'Remove leading and trailing spaces.
        If txtNewLibraryFileName.Text.EndsWith(".Libr") Then
            txtNewLibraryFileName.Text = txtNewLibraryFileName.Text.Replace(" ", "_")
        Else
            txtNewLibraryFileName.Text = txtNewLibraryFileName.Text.Replace(" ", "_") & ".Libr"
        End If

        'Check if the new library file name already exists:
        If Main.Project.DataFileExists(txtNewLibraryFileName.Text) = True Then
            Main.Message.AddWarning("The library file already exists: " & txtNewLibraryFileName.Text & vbCrLf)
            Exit Sub
        End If

        'Check the library name:
        txtNewLibraryLabel.Text = Trim(txtNewLibraryLabel.Text)
        If txtNewLibraryLabel.Text = "" Then
            Main.Message.AddWarning("The new library name is blank." & vbCrLf)
            Exit Sub
        End If

        'Main.SaveLibraryFile() 'Save the library that is currently open.
        Main.SaveLibrary(Main.LibraryFileName)
        Main.trvLibrary.Nodes.Clear() 'Clear the nodes in the Library TreeView
        Main.DocDisplayInfo.Clear() 'Clear the document display info dictionary.
        Main.DefaultDocInfo.Clear() 'Clear the Default document info dictionary.
        Main.dgvDeletedItems.Rows.Clear()
        'Main.dgvDeletedItems.Columns.Clear()

        'Create the new library:
        Main.trvLibrary.Nodes.Clear() 'Clear the nodes in the Library TreeView
        Dim Node1 As TreeNode = New TreeNode(txtNewLibraryLabel.Text, 0, 1)
        Node1.Name = txtNewLibraryFileName.Text
        Main.trvLibrary.Nodes.Add(Node1)

        'Main.txtLibraryName.Text = txtNewLibraryLabel.Text
        'Main.txtLibraryDescription.Text = txtNewLibraryDescription.Text

        Main.LibraryName = txtNewLibraryLabel.Text
        Main.LibraryDescription = txtNewLibraryDescription.Text
        Main.LibraryFileName = txtNewLibraryFileName.Text
        Main.LibraryCreationDate = Now
        Main.LibraryLastEditDate = Now
        Main.DefaultAuthorFile = txtDefaultAuthorFile.Text
        Main.DefaultAuthorSummary = txtDefaultAuthorSummary.Text
        Main.DefaultCopyrightFile = txtDefaultCopyrightFile.Text
        Main.DefaultCopyrightSummary = txtDefaultCopyrightSummary.Text
        Main.DefaultLicenseFile = txtDefaultLicenseFile.Text
        Main.DefaultLicenseSummary = rtbDefaultLicenseNotice.Text

        'Main.SaveLibrary(txtNewLibraryFileName.Text)
        Main.SaveLibrary(Main.LibraryFileName)

    End Sub



    'Private Sub btnNewLibrary_Click_1(sender As Object, e As EventArgs) Handles btnNewLibrary.Click
    '    'Create a new library.

    '    'Close all DocumentView forms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    '    Main.SaveLibraryFile() 'Save the library that is currently open.
    '    Main.trvLibrary.Nodes.Clear() 'Clear the nodes in the Library TreeView
    '    Main.DocDisplayInfo.Clear() 'Clear the document display info dictionary.
    '    Main.DefaultDocInfo.Clear() 'Clear the Default document info dictionary.


    '    'Dim mainNode As New TreeNode
    '    'mainNode.Name = "mainNode"
    '    'mainNode.Text = "Main"
    '    'TreeView1.Nodes.Add(mainNode)

    '    'Dim Node1 As TreeNode = New TreeNode("My Library", 0, 1) 'The TreeView will later be saved in an XML file. An XElement cannot have a space in the element name.
    '    'Node1.Name = "My_Library.Libr"
    '    'trvLibrary.Nodes.Add(Node1)

    '    Dim Node1 As TreeNode = New TreeNode(txtNewLibraryLabel.Text, 0, 1)
    '    Node1.Name = txtNewLibraryFileName.Text
    '    Main.trvLibrary.Nodes.Add(Node1)

    'End Sub


    Private Sub LicenseInfo_ApplyAuthor(AuthorFile As String, AuthorSummary As String) Handles LicenseInfo.ApplyAuthor
        txtDefaultAuthorFile.Text = AuthorFile
        txtDefaultAuthorSummary.Text = AuthorSummary
    End Sub

    Private Sub LicenseInfo_ApplyCopyright(CopyrightFile As String, CopyrightSummary As String) Handles LicenseInfo.ApplyCopyright
        txtDefaultCopyrightFile.Text = CopyrightFile
        txtDefaultCopyrightSummary.Text = CopyrightSummary
    End Sub

    Private Sub LicenseInfo_ApplyLicense(LicenseFile As String, LicenseSummary As String) Handles LicenseInfo.ApplyLicense
        txtDefaultLicenseFile.Text = LicenseFile
        rtbDefaultLicenseNotice.Text = LicenseSummary

    End Sub


#End Region 'Form Methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region " Form Events - Events that can be triggered by this form." '==========================================================================================================================

#End Region 'Form Events ----------------------------------------------------------------------------------------------------------------------------------------------------------------------





End Class