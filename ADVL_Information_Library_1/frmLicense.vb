Public Class frmLicense
    'Author, copyright and license information.

#Region " Variable Declarations - All the variables used in this form and this application." '=================================================================================================

    'Dim AuthorXDoc As System.Xml.XmlDocument    'The Author information read from the author .Author xml file.
    Dim AuthorXDoc As XDocument    'The Author information read from the author .Author xml file.
    Dim CopyrightXDoc As XDocument 'The Copyright information read from the copyright .Copyright xml file.
    Dim LicenseXDoc As XDocument   'The License information read from the license .Lic xmk file.

    Dim License As New ADVL_Utilities_Library_1.License


#End Region 'Variable Declarations ------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Properties - All the properties used in this form and this application" '============================================================================================================

    Private _authorFile As String = "" 'The Author file containing information about the document author. The file contains XML data and has the .Author extension.
    Property AuthorFile As String
        Get
            Return _authorFile
        End Get
        Set(value As String)
            _authorFile = value
            txtAuthorFileName.Text = _authorFile
            OpenAuthorFile()
        End Set
    End Property

    Private _copyrightFile As String = "" 'The Copyright file containing information about the document copyright. The file contains XML data and has the .Copyright extension.
    Property CopyrightFile As String
        Get
            Return _copyrightFile
        End Get
        Set(value As String)
            _copyrightFile = value
            txtCopyrightFileName.Text = _copyrightFile
            OpenCopyrightFile()
        End Set
    End Property

    Private _licenseFile As String = "" 'The License file containing information about the document license. The file contains XML data and has the .Lic extension.
    Property LicenseFile As String
        Get
            Return _licenseFile
        End Get
        Set(value As String)
            _licenseFile = value
            txtLicenseFileName.Text = _licenseFile
            OpenLicenseFile()
        End Set
    End Property

    Private _viewOnly As Boolean = False 'If set to True, this form only shows the Author, Copyright and License information. The data cannot be edited.
    Property ViewOnly As Boolean
        Get
            Return _viewOnly
        End Get
        Set(value As Boolean)
            _viewOnly = value
            If _viewOnly = True Then
                SetViewOnly()
            Else
                SetViewEdit()
            End If
        End Set
    End Property

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

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        RestoreFormSettings()   'Restore the form settings

        License.LoadDefaultLicenseDictionary() 'Construct the default license dictionary.
        cmbLicense.Items.Clear()
        For Each Item In License.dictLicenses
            cmbLicense.Items.Add(Item.Value.Abbreviation)
        Next

        pbIconAuthor.Image = Main.ImageList1.Images(75)
        pbIconCopyright.Image = Main.ImageList1.Images(81)
        pbIconLicense.Image = Main.ImageList1.Images(69)

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit the Form
        Me.Close() 'Close the form
    End Sub

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If WindowState = FormWindowState.Normal Then
            SaveFormSettings()
        Else
            'Dont save settings if form is minimised.
        End If
    End Sub

#End Region 'Form Display Methods -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Open and Close Forms - Code used to open and close other forms." '===================================================================================================================
#End Region 'Open and Close Forms -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Methods - The main actions performed by this form." '===========================================================================================================================

    Private Sub SetViewOnly()
        'Set the for to view the Author, Copyright and License information only.

        'Author Tab:
        txtAuthorFileName.ReadOnly = True
        txtAuthorName.ReadOnly = True
        txtAuthorSummary.ReadOnly = True
        txtAuthorDescription.ReadOnly = True
        txtAuthorContact.ReadOnly = True
        btnOpenAuthor.Enabled = False
        btnSaveAuthor.Enabled = False
        btnApplyAuthor.Enabled = False

        'Copyright Tab:
        txtCopyrightFileName.ReadOnly = True
        txtCopyrightOwnerName.ReadOnly = True
        txtCopyrightPublicationYear.ReadOnly = True
        txtCopyrightSummary.ReadOnly = True
        txtCopyrightNotice.ReadOnly = True
        btnOpenCopyright.Enabled = False
        btnSaveCopyright.Enabled = False
        btnApplyCopyright.Enabled = False

        'License Tab:
        txtLicenseFileName.ReadOnly = True
        cmbLicense.Enabled = False
        txtLicenseOwnerName.ReadOnly = True
        txtLicensePublicationYear.ReadOnly = True
        txtLicenseType.ReadOnly = True
        txtLicenseName.ReadOnly = True
        rtbLicenseDescription.ReadOnly = True
        rtbLicenseNotice.ReadOnly = True
        rtbLicenseText.ReadOnly = True
        btnOpenLicense.Enabled = False
        btnSaveLicense.Enabled = False
        btnApplyLicense.Enabled = False

    End Sub

    Private Sub SetViewEdit()
        'Set the form to View and Edit the Author, Copyright and License information.

        'Author Tab:
        txtAuthorFileName.ReadOnly = False
        txtAuthorName.ReadOnly = False
        txtAuthorSummary.ReadOnly = False
        txtAuthorDescription.ReadOnly = False
        txtAuthorContact.ReadOnly = False
        btnOpenAuthor.Enabled = True
        btnSaveAuthor.Enabled = True
        btnApplyAuthor.Enabled = True

        'Copyright Tab:
        txtCopyrightFileName.ReadOnly = False
        txtCopyrightOwnerName.ReadOnly = False
        txtCopyrightPublicationYear.ReadOnly = False
        txtCopyrightSummary.ReadOnly = False
        txtCopyrightNotice.ReadOnly = False
        btnOpenCopyright.Enabled = True
        btnSaveCopyright.Enabled = True
        btnApplyCopyright.Enabled = True

        'License Tab:
        txtLicenseFileName.ReadOnly = False
        cmbLicense.Enabled = True
        txtLicenseOwnerName.ReadOnly = False
        txtLicensePublicationYear.ReadOnly = False
        txtLicenseType.ReadOnly = False
        txtLicenseName.ReadOnly = False
        rtbLicenseDescription.ReadOnly = False
        rtbLicenseNotice.ReadOnly = False
        rtbLicenseText.ReadOnly = False
        btnOpenLicense.Enabled = True
        btnSaveLicense.Enabled = True
        btnApplyLicense.Enabled = True

    End Sub

    Private Sub OpenAuthorFile()
        'Open the author file specified in the property AuthorFile

        If AuthorFile = "" Then
            AuthorXDoc = Nothing
        Else
            Main.Project.ReadXmlData(AuthorFile, AuthorXDoc)
        End If
        UpdateAuthorTab()
        TabControl1.SelectedIndex = 0
    End Sub

    Private Sub UpdateAuthorTab()
        'Update the Author tab display.

        If IsNothing(AuthorXDoc) Then
            txtAuthorFileName.Text = ""
            txtAuthorName.Text = ""
            txtAuthorSummary.Text = ""
            txtAuthorDescription.Text = ""
            txtAuthorContact.Text = ""
        Else
            txtAuthorFileName.Text = AuthorFile
            txtAuthorName.Text = AuthorXDoc.<Author>.<Name>.Value
            txtAuthorSummary.Text = AuthorXDoc.<Author>.<Summary>.Value
            txtAuthorDescription.Text = AuthorXDoc.<Author>.<Description>.Value
            txtAuthorContact.Text = AuthorXDoc.<Author>.<Contact>.Value
        End If
    End Sub

    Private Sub OpenCopyrightFile()
        'Open the copyright file specified in the property CopyrightFile

        If CopyrightFile = "" Then
            CopyrightXDoc = Nothing
        Else
            Main.Project.ReadXmlData(CopyrightFile, CopyrightXDoc)
        End If
        UpdateCopyrightTab()
        TabControl1.SelectedIndex = 1
    End Sub

    Private Sub UpdateCopyrightTab()
        'Update the Copyright tab display.

        If IsNothing(CopyrightXDoc) Then
            txtCopyrightFileName.Text = ""
            txtCopyrightOwnerName.Text = ""
            txtCopyrightPublicationYear.Text = ""
            txtCopyrightSummary.Text = ""
            txtCopyrightNotice.Text = ""
        Else
            txtCopyrightFileName.Text = CopyrightFile
            txtCopyrightOwnerName.Text = CopyrightXDoc.<Copyright>.<OwnerName>.Value
            txtCopyrightPublicationYear.Text = CopyrightXDoc.<Copyright>.<PublicationYear>.Value
            txtCopyrightSummary.Text = CopyrightXDoc.<Copyright>.<Summary>.Value
            txtCopyrightNotice.Text = CopyrightXDoc.<Copyright>.<Notice>.Value
        End If
    End Sub

    Private Sub OpenLicenseFile()
        'Open the license file specified in the property LicenseFile

        If LicenseFile = "" Then
            LicenseXDoc = Nothing
        Else
            Main.Project.ReadXmlData(LicenseFile, LicenseXDoc)

        End If
        UpdateLicenseTab()
        TabControl1.SelectedIndex = 2
    End Sub

    Private Sub UpdateLicenseTab()
        'Update the License tab display.
        If IsNothing(LicenseXDoc) Then
            cmbLicense.SelectedIndex = cmbLicense.FindStringExact("None")
            txtLicenseOwnerName.Text = ""
            txtLicensePublicationYear.Text = ""
            txtLicenseType.Text = ""
            txtLicenseName.Text = ""
            rtbLicenseDescription.Text = ""
            rtbLicenseNotice.Text = ""
            rtbLicenseText.Text = ""
        Else
            cmbLicense.SelectedIndex = cmbLicense.FindStringExact(LicenseXDoc.<License>.<Abbreviation>.Value)
            txtLicenseOwnerName.Text = LicenseXDoc.<License>.<OwnerName>.Value
            txtLicensePublicationYear.Text = LicenseXDoc.<License>.<PublicationYear>.Value
            txtLicenseType.Text = LicenseXDoc.<License>.<Type>.Value
            txtLicenseName.Text = LicenseXDoc.<License>.<Name>.Value
            rtbLicenseDescription.Text = LicenseXDoc.<License>.<Description>.Value
            rtbLicenseNotice.Text = LicenseXDoc.<License>.<Notice>.Value
            rtbLicenseText.Text = LicenseXDoc.<License>.<Text>.Value
        End If

    End Sub

    Private Sub btnOpenAuthor_Click(sender As Object, e As EventArgs) Handles btnOpenAuthor.Click
        'Open Author file

        Dim SelectedFile As String = Main.Project.SelectDataFile("Author", "Author")

        If SelectedFile = "" Then

        Else
            AuthorFile = SelectedFile
            'txtAuthorFileName.Text = SelectedFile
            'Dim xmlAuth As System.Xml.Linq.XDocument
            'Main.Project.ReadXmlData(SelectedFile, xmlAuth)
            Main.Project.ReadXmlData(SelectedFile, AuthorXDoc)
            If AuthorXDoc Is Nothing Then
                Exit Sub
            End If
            txtAuthorName.Text = AuthorXDoc.<Author>.<Name>.Value
            txtAuthorSummary.Text = AuthorXDoc.<Author>.<Summary>.Value
            txtAuthorDescription.Text = AuthorXDoc.<Author>.<Description>.Value
            txtAuthorContact.Text = AuthorXDoc.<Author>.<Contact>.Value
        End If

    End Sub

    Private Sub btnSaveAuthor_Click(sender As Object, e As EventArgs) Handles btnSaveAuthor.Click
        'Save the Author file.
        SaveAuthor()
    End Sub

    Private Sub SaveAuthor()
        'Save the Author file.

        'Check the specified file name:
        Dim FileName As String = ""
        If Trim(txtAuthorFileName.Text) = "" Then
            Main.Message.AddWarning("Author file name is not specified." & vbCrLf)
            Beep()
            Exit Sub
        End If

        If Trim(txtAuthorFileName.Text).Contains(".") Then
            If Trim(txtAuthorFileName.Text).EndsWith(".Author") Then
                FileName = Trim(txtAuthorFileName.Text)
            Else
                Main.Message.AddWarning("Specified Author file name does not have the .Author file extension." & vbCrLf)
                Beep()
                Exit Sub
            End If
        Else
            FileName = Trim(txtAuthorFileName.Text) & ".Author"
            txtAuthorFileName.Text = FileName
        End If

        'Save the Author information:
        Dim authorInfo = <?xml version="1.0" encoding="utf-8"?>
                         <!---->
                         <!--Author information-->
                         <Author>
                             <Name><%= Trim(txtAuthorName.Text) %></Name>
                             <Summary><%= Trim(txtAuthorSummary.Text) %></Summary>
                             <Description><%= Trim(txtAuthorDescription.Text) %></Description>
                             <Contact><%= Trim(txtAuthorContact.Text) %></Contact>
                         </Author>

        Main.Project.SaveXmlData(FileName, authorInfo)
    End Sub

    Private Sub btnApplyAuthor_Click(sender As Object, e As EventArgs) Handles btnApplyAuthor.Click
        SaveAuthor()
        RaiseEvent ApplyAuthor(txtAuthorFileName.Text, Trim(txtAuthorSummary.Text))
    End Sub

    Private Sub btnSaveCopyright_Click(sender As Object, e As EventArgs) Handles btnSaveCopyright.Click
        'Save the Copyright file.
        SaveCopyright()
    End Sub

    Private Sub SaveCopyright()
        'Save the Copyright file.

        'Check the specified file name:
        Dim FileName As String = ""
        If Trim(txtCopyrightFileName.Text) = "" Then
            Main.Message.AddWarning("Copyright file name is not specified." & vbCrLf)
            Beep()
            Exit Sub
        End If

        If Trim(txtCopyrightFileName.Text).Contains(".") Then
            If Trim(txtCopyrightFileName.Text).EndsWith(".Copyright") Then
                FileName = Trim(txtCopyrightFileName.Text)
            Else
                Main.Message.AddWarning("Specified Copyright file name does not have the .Copyright file extension." & vbCrLf)
                Beep()
                Exit Sub
            End If

        Else
            FileName = Trim(txtCopyrightFileName.Text) & ".Copyright"
            txtCopyrightFileName.Text = FileName
        End If

        'Save the Author information:
        Dim copyrightInfo = <?xml version="1.0" encoding="utf-8"?>
                            <!---->
                            <!--Copyright information-->
                            <Copyright>
                                <OwnerName><%= Trim(txtCopyrightOwnerName.Text) %></OwnerName>
                                <PublicationYear><%= Trim(txtCopyrightPublicationYear.Text) %></PublicationYear>
                                <Summary><%= Trim(txtCopyrightSummary.Text) %></Summary>
                                <Notice><%= Trim(txtCopyrightNotice.Text) %></Notice>
                            </Copyright>

        Main.Project.SaveXmlData(FileName, copyrightInfo)
    End Sub

    Private Sub btnOpenCopyright_Click(sender As Object, e As EventArgs) Handles btnOpenCopyright.Click
        'Open Copyright file

        Dim SelectedFile As String = Main.Project.SelectDataFile("Copyright", "Copyright")

        If SelectedFile = "" Then

        Else
            'AuthorFile = SelectedFile
            CopyrightFile = SelectedFile

            'Main.Project.ReadXmlData(SelectedFile, AuthorXDoc)
            Main.Project.ReadXmlData(SelectedFile, CopyrightXDoc)
            If CopyrightXDoc Is Nothing Then
                Exit Sub
            End If
            txtCopyrightOwnerName.Text = CopyrightXDoc.<Copyright>.<OwnerName>.Value
            txtCopyrightPublicationYear.Text = CopyrightXDoc.<Copyright>.<PublicationYear>.Value
            txtCopyrightSummary.Text = CopyrightXDoc.<Copyright>.<Summary>.Value
            txtCopyrightNotice.Text = CopyrightXDoc.<Copyright>.<Notice>.Value
        End If
    End Sub

    Private Sub bthApplyCopyright_Click(sender As Object, e As EventArgs) Handles btnApplyCopyright.Click
        SaveCopyright()
        RaiseEvent ApplyCopyright(txtCopyrightFileName.Text, Trim(txtCopyrightSummary.Text))
    End Sub

    Private Sub btnSaveLicense_Click(sender As Object, e As EventArgs) Handles btnSaveLicense.Click
        'Save the License file.
        SaveLicense()
    End Sub

    Private Sub SaveLicense()
        'Save the License file.

        'Check the specified file name:
        Dim FileName As String = ""
        If Trim(txtLicenseFileName.Text) = "" Then
            Main.Message.AddWarning("License file name is not specified." & vbCrLf)
            Beep()
            Exit Sub
        End If

        If Trim(txtLicenseFileName.Text).Contains(".") Then
            If Trim(txtLicenseFileName.Text).EndsWith(".License") Then
                FileName = Trim(txtLicenseFileName.Text)
            Else
                Main.Message.AddWarning("Specified License file name does not have the .License file extension." & vbCrLf)
                Beep()
                Exit Sub
            End If

        Else
            FileName = Trim(txtLicenseFileName.Text) & ".License"
            txtLicenseFileName.Text = FileName
        End If

        'Save the Author information:
        Dim licenseInfo = <?xml version="1.0" encoding="utf-8"?>
                          <!---->
                          <!--License information-->
                          <License>
                              <Abbreviation><%= cmbLicense.SelectedItem.ToString %></Abbreviation>
                              <OwnerName><%= txtLicenseOwnerName.Text %></OwnerName>
                              <PublicationYear><%= txtLicensePublicationYear.Text %></PublicationYear>
                              <Type><%= txtLicenseType.Text %></Type>
                              <Name><%= txtLicenseName.Text %></Name>
                              <Description><%= rtbLicenseDescription.Text %></Description>
                              <Notice><%= rtbLicenseNotice.Text %></Notice>
                              <Text><%= rtbLicenseText.Text %></Text>
                          </License>

        Main.Project.SaveXmlData(FileName, licenseInfo)
    End Sub

    Private Sub btnOpenLicense_Click(sender As Object, e As EventArgs) Handles btnOpenLicense.Click
        'Open License file

        Dim SelectedFile As String = Main.Project.SelectDataFile("License", "License")

        If SelectedFile = "" Then

        Else
            LicenseFile = SelectedFile

            Main.Project.ReadXmlData(SelectedFile, LicenseXDoc)
            If LicenseXDoc Is Nothing Then
                Exit Sub
            End If
            txtLicenseOwnerName.Text = ""
            txtLicensePublicationYear.Text = ""
            cmbLicense.SelectedIndex = cmbLicense.FindStringExact(LicenseXDoc.<License>.<Abbreviation>.Value)
            txtLicenseOwnerName.Text = LicenseXDoc.<License>.<OwnerName>.Value
            txtLicensePublicationYear.Text = LicenseXDoc.<License>.<PublicationYear>.Value
            txtLicenseType.Text = LicenseXDoc.<License>.<Type>.Value
            txtLicenseName.Text = LicenseXDoc.<License>.<Name>.Value
            rtbLicenseDescription.Text = LicenseXDoc.<License>.<Description>.Value
            rtbLicenseNotice.Text = LicenseXDoc.<License>.<Notice>.Value
            rtbLicenseText.Text = LicenseXDoc.<License>.<Text>.Value
        End If
    End Sub

    Private Sub btnApplyLicense_Click(sender As Object, e As EventArgs) Handles btnApplyLicense.Click
        SaveLicense()
        RaiseEvent ApplyLicense(txtLicenseFileName.Text, Trim(rtbLicenseNotice.Text))
    End Sub


    Private Sub DefaultLicenseNotice()
        'Generate the default license notice.
        Select Case cmbLicense.SelectedItem.ToString
            Case "None"

            Case "Unknown"

            Case "Apache License 2.0"

            Case "GNU GPLv3"

            Case "MIT License"

            Case "The Unlicense"

            Case "CC0 1.0 Universal"

            Case "CC-BY-4.0"

            Case "CC-BY-SA-4.0"

        End Select
    End Sub



    Private Sub DefaultLicenseText()
        'Generate the default license text.
        Select Case cmbLicense.SelectedItem.ToString
            Case "None"

            Case "Unknown"

            Case "Apache License 2.0"

            Case "GNU GPLv3"

            Case "MIT License"

            Case "The Unlicense"

            Case "CC0 1.0 Universal"

            Case "CC-BY-4.0"

            Case "CC-BY-SA-4.0"

        End Select

    End Sub

    Private Sub cmbLicense_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLicense.SelectedIndexChanged
        License.CopyrightOwnerName = txtLicenseOwnerName.Text
        License.PublicationYear = txtLicensePublicationYear.Text
        Select Case cmbLicense.SelectedItem.ToString
            Case "None"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.None).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.None).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.None).Description
                rtbLicenseNotice.Text = "No license specified"
                rtbLicenseText.Text = "No license specified"
            Case "Unknown"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.Unknown).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.Unknown).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.Unknown).Description
                rtbLicenseNotice.Text = "License is unknown"
                rtbLicenseText.Text = "License is unknown"
            Case "Apache License 2.0"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.Apache_License_2_0).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.Apache_License_2_0).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.Apache_License_2_0).Description
                rtbLicenseNotice.Text = License.ApacheLicenseNotice
                rtbLicenseText.Text = License.ApacheLicenseText
            Case "GNU GPLv3"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.GNU_GPL_V3_0).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.GNU_GPL_V3_0).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.GNU_GPL_V3_0).Description
                rtbLicenseNotice.Text = ""
                rtbLicenseText.Text = ""
            Case "MIT License"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.MIT_License).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.MIT_License).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.MIT_License).Description
                rtbLicenseNotice.Text = License.MITLicenseNotice
                rtbLicenseText.Text = License.MITLicenseText
            Case "The Unlicense"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.The_Unlicense).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.The_Unlicense).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.The_Unlicense).Description
                rtbLicenseNotice.Text = License.UnLicenseNotice
                rtbLicenseText.Text = License.UnLicenseText
            Case "CC0 1.0 Universal"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC0_1_0).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC0_1_0).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC0_1_0).Description
                rtbLicenseNotice.Text = ""
                rtbLicenseText.Text = ""
            Case "CC-BY-4.0"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC_BY_4_0).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC_BY_4_0).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC_BY_4_0).Description
                rtbLicenseNotice.Text = ""
                rtbLicenseText.Text = ""
            Case "CC-BY-SA-4.0"
                Select Case License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC_BY_SA_4_0).Type
                    Case ADVL_Utilities_Library_1.License.Types.Any
                        txtLicenseType.Text = "Any"
                    Case ADVL_Utilities_Library_1.License.Types.Data
                        txtLicenseType.Text = "Data"
                    Case ADVL_Utilities_Library_1.License.Types.Software
                        txtLicenseType.Text = "Software"
                End Select
                txtLicenseName.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC_BY_SA_4_0).Name
                rtbLicenseDescription.Text = License.dictLicenses(ADVL_Utilities_Library_1.License.Codes.CC_BY_SA_4_0).Description
                rtbLicenseNotice.Text = ""
                rtbLicenseText.Text = ""
        End Select
    End Sub

#End Region 'Form Methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Events - Events that can be triggered by this form." '==========================================================================================================================

    Event ApplyAuthor(ByVal AuthorFile As String, AuthorSummary As String)

    Event ApplyCopyright(ByVal CopyrightFile As String, CopyrightSummary As String)

    Event ApplyLicense(ByVal LicenseFile As String, LicenseSummary As String)











#End Region 'Form Events ----------------------------------------------------------------------------------------------------------------------------------------------------------------------



End Class