'==============================================================================================================================================================================================
'
'Copyright 2017 Signalworks Pty Ltd, ABN 26 066 681 598

'Licensed under the Apache License, Version 2.0 (the "License");
'you may not use this file except in compliance with the License.
'You may obtain a copy of the License at
'
'http://www.apache.org/licenses/LICENSE-2.0
'
'Unless required by applicable law or agreed to in writing, software
'distributed under the License is distributed on an "AS IS" BASIS,
''WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'See the License for the specific language governing permissions and
'limitations under the License.
'
'----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


Public Class Main
    'The ADVL_Code_Examples application stores a collection of source code examples.

#Region " Coding Notes - Notes on the code used in this class." '==============================================================================================================================

    'ADD THE SYSTEM UTILITIES REFERENCE: ==========================================================================================
    'The following references are required by this software: 
    'Project \ Add Reference... \ ADVL_Utilities_Library_1.dll
    'The Utilities Library is used for Project Management, Archive file management, running XSequence files and running XMessage files.
    'If there are problems with a reference, try deleting it from the references list and adding it again.

    'ADD THE SERVICE REFERENCE: ===================================================================================================
    'A service reference to the Message Service must be added to the source code before this service can be used.
    'This is used to connect to the Application Network.

    'Adding the service reference to a project that includes the WcfMsgServiceLib project: -----------------------------------------
    'Project \ Add Service Reference
    'Press the Discover button.
    'Expand the items in the Services window and select IMsgService.
    'Press OK.
    '------------------------------------------------------------------------------------------------------------------------------
    '------------------------------------------------------------------------------------------------------------------------------
    'Adding the service reference to other projects that dont include the WcfMsgServiceLib project: -------------------------------
    'Run the ADVL_Application_Network_1 application to start the Application Network message service.
    'In Microsoft Visual Studio select: Project \ Add Service Reference
    'Enter the address: http://localhost:8733/ADVLService
    'Press the Go button.
    'MsgService is found.
    'Press OK to add ServiceReference1 to the project.
    '------------------------------------------------------------------------------------------------------------------------------
    '
    'ADD THE MsgServiceCallback CODE: =============================================================================================
    'This is used to connect to the Application Network.
    'In Microsoft Visual Studio select: Project \ Add Class
    'MsgServiceCallback.vb
    'Add the following code to the class:
    'Imports System.ServiceModel
    'Public Class MsgServiceCallback
    '    Implements ServiceReference1.IMsgServiceCallback
    '    Public Sub OnSendMessage(message As String) Implements ServiceReference1.IMsgServiceCallback.OnSendMessage
    '        'A message has been received.
    '        'Set the InstrReceived property value to the message (usually in XMessage format). This will also apply the instructions in the XMessage.
    '        Main.InstrReceived = message
    '    End Sub
    'End Class
    '------------------------------------------------------------------------------------------------------------------------------

#End Region 'Coding Notes ---------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Variable Declarations - All the variables and class objects used in this form and this application." '===============================================================================

    Public WithEvents ApplicationInfo As New ADVL_Utilities_Library_1.ApplicationInfo 'This object is used to store application information.
    Public WithEvents Project As New ADVL_Utilities_Library_1.Project 'This object is used to store Project information.
    Public WithEvents Message As New ADVL_Utilities_Library_1.Message 'This object is used to display messages in the Messages window.
    Public WithEvents ApplicationUsage As New ADVL_Utilities_Library_1.Usage 'This object stores application usage information.
    Public WithEvents LibraryLicenseInfo As frmLicense
    Public WithEvents DocumentLicenseInfo As frmLicense

    'Declare Forms used by the application:
    'Public WithEvents CodeView As frmDocumentView
    Public WithEvents DocView As frmDocumentView
    Public WithEvents NewLibrary As frmNewLibrary
    Public WithEvents UtilView As frmUtilityDocView

    'Public CodeViewList As New ArrayList 'Used to display multiple CodeView forms
    Public DocViewList As New ArrayList 'Used to display multiple DocView forms
    Public UtilViewList As New ArrayList 'Used to display multiple UtilView forms

    '  Public DocViewSettings As New DocViewSettingsList 'Stores a list of settings used to display documents on the DocView forms.
    '  Public UtilViewSettings As New DocViewSettingsList 'Stores a list of settings used to display information on the UtilView sorms.
    '  Public DocViewSettingsChanged As Boolean 'If True then the Code View Settings list file needs to be updated.

    'Public FormInfo As New FormSettings 'This is used to store the form settings for the DocView and UtilView forms.


    'Declare objects used to connect to the Application Network:
    Public client As ServiceReference1.MsgServiceClient
    Public WithEvents XMsg As New ADVL_Utilities_Library_1.XMessage
    Dim XDoc As New System.Xml.XmlDocument
    Public Status As New System.Collections.Specialized.StringCollection
    Dim ClientAppName As String = "" 'The name of the client requesting service
    Dim ClientAppLocn As String = "" 'The location in the Client application requesting service
    Dim MessageXDoc As System.Xml.Linq.XDocument
    Dim xmessage As XElement 'This will contain the message. It will be added to MessageXDoc.
    Dim xlocns As New List(Of XElement) 'A list of locations. Each location forms part of the reply message. The information in the reply message will be sent to the specified location in the client application.
    Dim MessageText As String = "" 'The text of a message sent through the Application Network.

    Dim WithEvents Zip As ADVL_Utilities_Library_1.ZipComp

    'Public dictDocDisplayInfo As New Dictionary(Of String, DocumentInfo) 'Dictionary of information about each Document display form.
    Public DocDisplayInfo As New Dictionary(Of String, DocumentInfo) 'Dictionary of information about each Document display form. (Left, Top, Width, Height)
    'This dictionary info is saved in the Library information file.
    'Public UtilDisplayInfo As New Dictionary(Of String, DocumentInfo) 'Dictionary of information about each Utility Document display form. 'NOTE: DocDisplayInfo is now used for this.

    'This dictionary is used to display information about the default Update/Version of the document selected in the library tree.
    'Private DocItem As New Dictionary(Of String, DocItemInfo)     'Dictionary if information about each document item (Update, Version, Note, ...)
    ' Private Copyright As New Dictionary(Of String, CopyrightInfo) 'Dictionary of information about each document copyright
    'Private License As New Dictionary(Of String, LicenseInfo)     'Dictionary of information about each document license.
    Public DefaultDocInfo As New Dictionary(Of String, DefaultDocInfo) 'Dictionary of information about each default document selected for each document node in the library. These documents are a document version contained within a document structure.
    Public CollectionInfo As New Dictionary(Of String, CollectionInfo) 'Dictionary of information about each collection in the library.
    'Private DocItem As New Dictionary(Of String, DocItemInfo)     'Dictionary if information about each document item (Note, ...). These items do not contains Updates and Versions within a document structure.
    Private UtilDocInfo As New Dictionary(Of String, DocItemInfo)     'Dictionary if information about each utility document item (Note, Checklist, To Do list ...). These items do not contain Updates and Versions within a document structure.


#End Region 'Variable Declarations ------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Properties - All the properties used in this form and this application" '============================================================================================================

    Private _connectionHashcode As Integer 'The Application Network connection hashcode. This is used to identify a connection in the Application Netowrk when reconnecting.
    Property ConnectionHashcode As Integer
        Get
            Return _connectionHashcode
        End Get
        Set(value As Integer)
            _connectionHashcode = value
        End Set
    End Property

    Private _connectedToAppNet As Boolean = False  'True if the application is connected to the Application Network.
    Property ConnectedToAppnet As Boolean
        Get
            Return _connectedToAppNet
        End Get
        Set(value As Boolean)
            _connectedToAppNet = value
        End Set
    End Property

    Private _instrReceived As String = "" 'Contains Instructions received from the Application Network message service.
    Property InstrReceived As String
        Get
            Return _instrReceived
        End Get
        Set(value As String)
            If value = Nothing Then
                Message.Add("Empty message received!")
            Else
                _instrReceived = value

                'Add the message to the XMessages window:
                Message.Color = Color.Blue
                Message.FontStyle = FontStyle.Bold
                Message.XAdd("Message received: " & vbCrLf)
                Message.SetNormalStyle()
                Message.XAdd(_instrReceived & vbCrLf & vbCrLf)

                If _instrReceived.StartsWith("<XMsg>") Then 'This is an XMessage set of instructions.
                    Try
                        'Inititalise the reply message:
                        Dim Decl As New XDeclaration("1.0", "utf-8", "yes")
                        MessageXDoc = New XDocument(Decl, Nothing) 'Reply message - this will be sent to the Client App.
                        xmessage = New XElement("XMsg")
                        xlocns.Add(New XElement("Main")) 'Initially set the location in the Client App to Main.

                        'Run the received message:
                        Dim XmlHeader As String = "<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>"
                        XDoc.LoadXml(XmlHeader & vbCrLf & _instrReceived)
                        XMsg.Run(XDoc, Status)
                    Catch ex As Exception
                        Message.Add("Error running XMsg: " & ex.Message & vbCrLf)
                    End Try

                    'XMessage has been run.
                    'Reply to this message:
                    'Add the message reply to the XMessages window:
                    'Complete the MessageXDoc:
                    xmessage.Add(xlocns(xlocns.Count - 1)) 'Add the last location reply instructions to the message.
                    MessageXDoc.Add(xmessage)
                    MessageText = MessageXDoc.ToString

                    If ClientAppName = "" Then
                        'No client to send a message to!
                    Else
                        Message.Color = Color.Red
                        Message.FontStyle = FontStyle.Bold
                        Message.XAdd("Message sent to " & ClientAppName & ":" & vbCrLf)
                        Message.SetNormalStyle()
                        Message.XAdd(MessageText & vbCrLf & vbCrLf)
                        'SendMessage sends the contents of MessageText to MessageDest.
                        SendMessage() 'This subroutine triggers the timer to send the message after a short delay.
                    End If
                Else

                End If
            End If

        End Set
    End Property

    Private _closedFormNo As Integer 'Temporarily holds the number of the form that is being closed. 
    Property ClosedFormNo As Integer
        Get
            Return _closedFormNo
        End Get
        Set(value As Integer)
            _closedFormNo = value
        End Set
    End Property

    Private _libraryName As String = "" 'The name of the Information Library.
    Property LibraryName As String
        Get
            Return _libraryName
        End Get
        Set(value As String)
            _libraryName = value
            txtLibraryName.Text = _libraryName
            txtLibraryName2.Text = _libraryName
        End Set
    End Property

    Private _libraryFileName As String = "" 'The file name of the Information Library.
    Property LibraryFileName As String
        Get
            Return _libraryFileName
        End Get
        Set(value As String)
            _libraryFileName = value
            txtLibraryFileName.Text = _libraryFileName
        End Set
    End Property

    Private _libraryVersion As String = "1.0" 'The version of the Information Library.
    Property LibraryVersion As String
        Get
            Return _libraryVersion
        End Get
        Set(value As String)
            _libraryVersion = value
        End Set
    End Property

    Private _libraryDescription As String = "" 'A description of the Information Library.
    Property LibraryDescription As String
        Get
            Return _libraryDescription
        End Get
        Set(value As String)
            _libraryDescription = value
            txtLibraryDescription.Text = _libraryDescription
        End Set
    End Property

    Private _libraryCreationDate As DateTime = Now ' Format(Now, "d-MMM-yyyy H:mm:ss") 'The creation date of the Library. 
    Property LibraryCreationDate As DateTime
        Get
            Return _libraryCreationDate
        End Get
        Set(value As DateTime)
            _libraryCreationDate = value
            txtLibraryCreationDate.Text = Format(_libraryCreationDate, "d-MMM-yyyy H:mm:ss")
        End Set
    End Property

    Private _libraryLastEditDate As DateTime = Now 'The last edit date of the Library.
    Property LibraryLastEditDate As DateTime
        Get
            Return _libraryLastEditDate
        End Get
        Set(value As DateTime)
            _libraryLastEditDate = value
            txtLibraryLastEditDate.Text = Format(_libraryLastEditDate, "d-MMM-yyyy H:mm:ss")
        End Set
    End Property

    Private _libraryParent As String = "" 'The library from which this version has been modified.
    Property LibraryParent As String
        Get
            Return _libraryParent
        End Get
        Set(value As String)
            _libraryParent = value
        End Set
    End Property

    Private _defaultAuthorFile As String = "" 'The default author file for Collections or Documents added to the Library. The file contains author details.
    Property DefaultAuthorFile As String
        Get
            Return _defaultAuthorFile
        End Get
        Set(value As String)
            _defaultAuthorFile = value
            txtDefaultAuthorFile.Text = _defaultAuthorFile
        End Set
    End Property

    Private _defaultAuthorSummary As String = "" 'The default author summary.
    Property DefaultAuthorSummary As String
        Get
            Return _defaultAuthorSummary
        End Get
        Set(value As String)
            _defaultAuthorSummary = value
            txtDefaultAuthorSummary.Text = _defaultAuthorSummary
        End Set
    End Property

    Private _defaultCopyrightFile As String = "" 'The default copyright file. This file contains copyright details.
    Property DefaultCopyrightFile As String
        Get
            Return _defaultCopyrightFile
        End Get
        Set(value As String)
            _defaultCopyrightFile = value
            txtDefaultCopyrightFile.Text = _defaultCopyrightFile
        End Set
    End Property

    Private _defaultCopyrightSummary As String = "" 'The default copyright summary.
    Property DefaultCopyrightSummary As String
        Get
            Return _defaultCopyrightSummary
        End Get
        Set(value As String)
            _defaultCopyrightSummary = value
            txtDefaultCopyrightSummary.Text = _defaultCopyrightSummary
        End Set
    End Property

    Private _defaultLicenseFile As String = "" 'The default license file. This file contains license details.
    Property DefaultLicenseFile As String
        Get
            Return _defaultLicenseFile
        End Get
        Set(value As String)
            _defaultLicenseFile = value
            txtDefaultLicenseFile.Text = _defaultLicenseFile
        End Set
    End Property

    Private _defaultLicenseSummary As String = "" 'The default license summary.
    Property DefaultLicenseSummary As String
        Get
            Return _defaultLicenseSummary
        End Get
        Set(value As String)
            _defaultLicenseSummary = value
            rtbDefaultLicenseNotice.Text = _defaultLicenseSummary
        End Set
    End Property

    Private _selectedNodeText As String = "" 'The text of the selected node in the trvLibrary treeview.
    Property SelectedNodeText As String
        Get
            Return _selectedNodeText
        End Get
        Set(value As String)
            _selectedNodeText = value
            txtNodeText.Text = _selectedNodeText  'Node Information tab.
            txtCurrentNodeText.Text = _selectedNodeText 'Edit Item tab.
        End Set
    End Property

    Private _selectedNodeKey As String = "" 'The key of the selected node in the trvLibrary treeview.
    Property SelectedNodeKey As String
        Get
            Return _selectedNodeKey
        End Get
        Set(value As String)
            _selectedNodeKey = value
            txtNodeKey.Text = _selectedNodeKey 'Node Information tab.
            txtCurrentNodeKey.Text = _selectedNodeKey 'Edit Item tab.
        End Set
    End Property

    Private _hoverNodeText As String = "" 'The text of the node under the mouse pointer.
    Property HoverNodeText As String
        Get
            Return _hoverNodeText
        End Get
        Set(value As String)
            _hoverNodeText = value
        End Set
    End Property

    Private _hoverNodeKey As String = "" 'The key of the node under the mouse pointer.
    Property HoverNodeKey As String
        Get
            Return _hoverNodeKey
        End Get
        Set(value As String)
            _hoverNodeKey = value
        End Set
    End Property


#End Region 'Properties -----------------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Process XML Files - Read and write XML files." '=====================================================================================================================================

    Private Sub SaveFormSettings()
        'Save the form settings in an XML document.
        Dim settingsData = <?xml version="1.0" encoding="utf-8"?>
                           <!---->
                           <!--Form settings for Main form.-->
                           <FormSettings>
                               <Left><%= Me.Left %></Left>
                               <Top><%= Me.Top %></Top>
                               <Width><%= Me.Width %></Width>
                               <Height><%= Me.Height %></Height>
                               <SelectedTabIndex><%= TabControl1.SelectedIndex %></SelectedTabIndex>
                               <LibraryFileName><%= LibraryFileName %></LibraryFileName>
                               <SelectedLibraryTabIndex><%= TabControl4.SelectedIndex %></SelectedLibraryTabIndex>
                               <!---->
                           </FormSettings>

        'Add code to include other settings to save after the comment line <!---->

        ' <LibraryFileName><%= txtLibraryFileName.Text %></LibraryFileName>

        Dim SettingsFileName As String = "FormSettings_" & ApplicationInfo.Name & "_" & Me.Text & ".xml"
        Project.SaveXmlSettings(SettingsFileName, settingsData)
    End Sub

    Private Sub RestoreFormSettings()
        'Read the form settings from an XML document.

        Dim SettingsFileName As String = "FormSettings_" & ApplicationInfo.Name & "_" & Me.Text & ".xml"

        If Project.SettingsFileExists(SettingsFileName) Then
            Dim Settings As System.Xml.Linq.XDocument
            Project.ReadXmlSettings(SettingsFileName, Settings)

            If IsNothing(Settings) Then 'There is no Settings XML data.
                Exit Sub
            End If

            'Restore form position and size:
            If Settings.<FormSettings>.<Left>.Value <> Nothing Then Me.Left = Settings.<FormSettings>.<Left>.Value
            If Settings.<FormSettings>.<Top>.Value <> Nothing Then Me.Top = Settings.<FormSettings>.<Top>.Value
            If Settings.<FormSettings>.<Height>.Value <> Nothing Then Me.Height = Settings.<FormSettings>.<Height>.Value
            If Settings.<FormSettings>.<Width>.Value <> Nothing Then Me.Width = Settings.<FormSettings>.<Width>.Value

            'Add code to read other saved setting here:
            If Settings.<FormSettings>.<SelectedTabIndex>.Value <> Nothing Then TabControl1.SelectedIndex = Settings.<FormSettings>.<SelectedTabIndex>.Value
            If Settings.<FormSettings>.<LibraryFileName>.Value <> Nothing Then
                'txtLibraryFileName.Text = Settings.<FormSettings>.<LibraryFileName>.Value
                LibraryFileName = Settings.<FormSettings>.<LibraryFileName>.Value
                'Dim XDocLib As XDocument
                'Project.ReadXmlData(Settings.<FormSettings>.<LibraryFileName>.Value, XDocLib)
                'OpenLibraryXDoc(XDocLib)
                OpenLibrary(LibraryFileName)
            End If
            If Settings.<FormSettings>.<SelectedLibraryTabIndex>.Value <> Nothing Then TabControl4.SelectedIndex = Settings.<FormSettings>.<SelectedLibraryTabIndex>.Value

        End If
    End Sub

    Private Sub ReadApplicationInfo()
        'Read the Application Information.

        If ApplicationInfo.FileExists Then
            ApplicationInfo.ReadFile()
        Else
            'There is no Application_Info.xml file.
            DefaultAppProperties() 'Create a new Application Info file with default application properties:
        End If
    End Sub

    Private Sub DefaultAppProperties()
        'These properties will be saved in the Application_Info.xml file in the application directory.
        'If this file is deleted, it will be re-created using these default application properties.

        'Change this to show your application Name, Description and Creation Date.
        ApplicationInfo.Name = "ADVL_Code_Examples_1"

        'ApplicationInfo.ApplicationDir is set when the application is started.
        ApplicationInfo.ExecutablePath = Application.ExecutablePath

        ApplicationInfo.Description = "The Code Examples application stores source code examples."
        ApplicationInfo.CreationDate = "19-Feb-2017 12:00:00"

        'Author -----------------------------------------------------------------------------------------------------------
        'Change this to show your Name, Description and Contact information.
        ApplicationInfo.Author.Name = "Signalworks Pty Ltd"
        ApplicationInfo.Author.Description = "Signalworks Pty Ltd" & vbCrLf &
            "Australian Proprietary Company" & vbCrLf &
            "ABN 26 066 681 598" & vbCrLf &
            "Registration Date 05/10/1994"

        ApplicationInfo.Author.Contact = "http://www.andorville.com.au/"

        'File Associations: -----------------------------------------------------------------------------------------------
        'Add any file associations here.
        'The file extension and a description of files that can be opened by this application are specified.
        'The example below specifies a coordinate system parameter file type with the file extension .ADVLCoord.
        'Dim Assn1 As New ADVL_System_Utilities.FileAssociation
        'Assn1.Extension = "ADVLCoord"
        'Assn1.Description = "Andorville™ software coordinate system parameter file"
        'ApplicationInfo.FileAssociations.Add(Assn1)

        'Version ----------------------------------------------------------------------------------------------------------
        ApplicationInfo.Version.Major = My.Application.Info.Version.Major
        ApplicationInfo.Version.Minor = My.Application.Info.Version.Minor
        ApplicationInfo.Version.Build = My.Application.Info.Version.Build
        ApplicationInfo.Version.Revision = My.Application.Info.Version.Revision

        'Copyright --------------------------------------------------------------------------------------------------------
        'Add your copyright information here.
        ApplicationInfo.Copyright.OwnerName = "Signalworks Pty Ltd, ABN 26 066 681 598"
        ApplicationInfo.Copyright.PublicationYear = "2017"

        'Trademarks -------------------------------------------------------------------------------------------------------
        'Add your trademark information here.
        Dim Trademark1 As New ADVL_Utilities_Library_1.Trademark
        Trademark1.OwnerName = "Signalworks Pty Ltd, ABN 26 066 681 598"
        Trademark1.Text = "Andorville"
        Trademark1.Registered = False
        Trademark1.GenericTerm = "software"
        ApplicationInfo.Trademarks.Add(Trademark1)
        Dim Trademark2 As New ADVL_Utilities_Library_1.Trademark
        Trademark2.OwnerName = "Signalworks Pty Ltd, ABN 26 066 681 598"
        Trademark2.Text = "AL-H7"
        Trademark2.Registered = False
        Trademark2.GenericTerm = "software"
        ApplicationInfo.Trademarks.Add(Trademark2)

        'License -------------------------------------------------------------------------------------------------------
        'Add your license information here.
        ApplicationInfo.License.CopyrightOwnerName = "Signalworks Pty Ltd, ABN 26 066 681 598"
        ApplicationInfo.License.PublicationYear = "2017"

        'License Links:
        'http://choosealicense.com/
        'http://www.apache.org/licenses/
        'http://opensource.org/

        'Apache License 2.0 ---------------------------------------------
        ApplicationInfo.License.Code = ADVL_Utilities_Library_1.License.Codes.Apache_License_2_0
        ApplicationInfo.License.Notice = ApplicationInfo.License.ApacheLicenseNotice 'Get the pre-defined Aapche license notice.
        ApplicationInfo.License.Text = ApplicationInfo.License.ApacheLicenseText     'Get the pre-defined Apache license text.

        'Code to use other pre-defined license types is shown below:

        'GNU General Public License, version 3 --------------------------
        'ApplicationInfo.License.Type = ADVL_Utilities_Library_1.License.Types.GNU_GPL_V3_0
        'ApplicationInfo.License.Notice = 'Add the License Notice to ADVL_Utilities_Library_1 License class.
        'ApplicationInfo.License.Text = 'Add the License Text to ADVL_Utilities_Library_1 License class.

        'The MIT License ------------------------------------------------
        'ApplicationInfo.License.Type = ADVL_Utilities_Library_1.License.Types.MIT_License
        'ApplicationInfo.License.Notice = ApplicationInfo.License.MITLicenseNotice
        'ApplicationInfo.License.Text = ApplicationInfo.License.MITLicenseText

        'No License Specified -------------------------------------------
        'ApplicationInfo.License.Type = ADVL_Utilities_Library_1.License.Types.None
        'ApplicationInfo.License.Notice = ""
        'ApplicationInfo.License.Text = ""

        'The Unlicense --------------------------------------------------
        'ApplicationInfo.License.Type = ADVL_Utilities_Library_1.License.Types.The_Unlicense
        'ApplicationInfo.License.Notice = ApplicationInfo.License.UnLicenseNotice
        'ApplicationInfo.License.Text = ApplicationInfo.License.UnLicenseText

        'Unknown License ------------------------------------------------
        'ApplicationInfo.License.Type = ADVL_Utilities_Library_1.License.Types.Unknown
        'ApplicationInfo.License.Notice = ""
        'ApplicationInfo.License.Text = ""

        'Source Code: --------------------------------------------------------------------------------------------------
        'Add your source code information here if required.
        'THIS SECTION WILL BE UPDATED TO ALLOW A GITHUB LINK.
        ApplicationInfo.SourceCode.Language = "Visual Basic 2015"
        ApplicationInfo.SourceCode.FileName = ""
        ApplicationInfo.SourceCode.FileSize = 0
        ApplicationInfo.SourceCode.FileHash = ""
        ApplicationInfo.SourceCode.WebLink = ""
        ApplicationInfo.SourceCode.Contact = ""
        ApplicationInfo.SourceCode.Comments = ""

        'ModificationSummary: -----------------------------------------------------------------------------------------
        'Add any source code modification here is required.
        ApplicationInfo.ModificationSummary.BaseCodeName = ""
        ApplicationInfo.ModificationSummary.BaseCodeDescription = ""
        ApplicationInfo.ModificationSummary.BaseCodeVersion.Major = 0
        ApplicationInfo.ModificationSummary.BaseCodeVersion.Minor = 0
        ApplicationInfo.ModificationSummary.BaseCodeVersion.Build = 0
        ApplicationInfo.ModificationSummary.BaseCodeVersion.Revision = 0
        ApplicationInfo.ModificationSummary.Description = "This is the first released version of the application. No earlier base code used."

        'Library List: ------------------------------------------------------------------------------------------------
        'Add the ADVL_Utilties_Library_1 library:
        Dim NewLib As New ADVL_Utilities_Library_1.LibrarySummary
        NewLib.Name = "ADVL_System_Utilities"
        NewLib.Description = "System Utility classes used in Andorville™ software development system applications"
        NewLib.CreationDate = "7-Jan-2016 12:00:00"
        NewLib.LicenseNotice = "Copyright 2016 Signalworks Pty Ltd, ABN 26 066 681 598" & vbCrLf &
                               vbCrLf &
                               "Licensed under the Apache License, Version 2.0 (the ""License"");" & vbCrLf &
                               "you may not use this file except in compliance with the License." & vbCrLf &
                               "You may obtain a copy of the License at" & vbCrLf &
                               vbCrLf &
                               "http://www.apache.org/licenses/LICENSE-2.0" & vbCrLf &
                               vbCrLf &
                               "Unless required by applicable law or agreed to in writing, software" & vbCrLf &
                               "distributed under the License is distributed on an ""AS IS"" BASIS," & vbCrLf &
                               "WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied." & vbCrLf &
                               "See the License for the specific language governing permissions and" & vbCrLf &
                               "limitations under the License." & vbCrLf

        NewLib.CopyrightNotice = "Copyright 2016 Signalworks Pty Ltd, ABN 26 066 681 598"

        NewLib.Version.Major = 1
        NewLib.Version.Minor = 0
        NewLib.Version.Build = 1
        NewLib.Version.Revision = 0

        NewLib.Author.Name = "Signalworks Pty Ltd"
        NewLib.Author.Description = "Signalworks Pty Ltd" & vbCrLf &
            "Australian Proprietary Company" & vbCrLf &
            "ABN 26 066 681 598" & vbCrLf &
            "Registration Date 05/10/1994"

        NewLib.Author.Contact = "http://www.andorville.com.au/"

        Dim NewClass1 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass1.Name = "ZipComp"
        NewClass1.Description = "The ZipComp class is used to compress files into and extract files from a zip file."
        NewLib.Classes.Add(NewClass1)
        Dim NewClass2 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass2.Name = "XSequence"
        NewClass2.Description = "The XSequence class is used to run an XML property sequence (XSequence) file. XSequence files are used to record and replay processing sequences in Andorville™ software applications."
        NewLib.Classes.Add(NewClass2)
        Dim NewClass3 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass3.Name = "XMessage"
        NewClass3.Description = "The XMessage class is used to read an XML Message (XMessage). An XMessage is a simplified XSequence used to exchange information between Andorville™ software applications."
        NewLib.Classes.Add(NewClass3)
        Dim NewClass4 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass4.Name = "Location"
        NewClass4.Description = "The Location class consists of properties and methods to store data in a location, which is either a directory or archive file."
        NewLib.Classes.Add(NewClass4)
        Dim NewClass5 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass5.Name = "Project"
        NewClass5.Description = "An Andorville™ software application can store data within one or more projects. Each project stores a set of related data files. The Project class contains properties and methods used to manage a project."
        NewLib.Classes.Add(NewClass5)
        Dim NewClass6 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass6.Name = "ProjectSummary"
        NewClass6.Description = "ProjectSummary stores a summary of a project."
        NewLib.Classes.Add(NewClass6)
        Dim NewClass7 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass7.Name = "DataFileInfo"
        NewClass7.Description = "The DataFileInfo class stores information about a data file."
        NewLib.Classes.Add(NewClass7)
        Dim NewClass8 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass8.Name = "Message"
        NewClass8.Description = "The Message class contains text properties and methods used to display messages in an Andorville™ software application."
        NewLib.Classes.Add(NewClass8)
        Dim NewClass9 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass9.Name = "ApplicationSummary"
        NewClass9.Description = "The ApplicationSummary class stores a summary of an Andorville™ software application."
        NewLib.Classes.Add(NewClass9)
        Dim NewClass10 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass10.Name = "LibrarySummary"
        NewClass10.Description = "The LibrarySummary class stores a summary of a software library used by an application."
        NewLib.Classes.Add(NewClass10)
        Dim NewClass11 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass11.Name = "ClassSummary"
        NewClass11.Description = "The ClassSummary class stores a summary of a class contained in a software library."
        NewLib.Classes.Add(NewClass11)
        Dim NewClass12 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass12.Name = "ModificationSummary"
        NewClass12.Description = "The ModificationSummary class stores a summary of any modifications made to an application or library."
        NewLib.Classes.Add(NewClass12)
        Dim NewClass13 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass13.Name = "ApplicationInfo"
        NewClass13.Description = "The ApplicationInfo class stores information about an Andorville™ software application."
        NewLib.Classes.Add(NewClass13)
        Dim NewClass14 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass14.Name = "Version"
        NewClass14.Description = "The Version class stores application, library or project version information."
        NewLib.Classes.Add(NewClass14)
        Dim NewClass15 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass15.Name = "Author"
        NewClass15.Description = "The Author class stores information about an Author."
        NewLib.Classes.Add(NewClass15)
        Dim NewClass16 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass16.Name = "FileAssociation"
        NewClass16.Description = "The FileAssociation class stores the file association extension and description. An application can open files on its file association list."
        NewLib.Classes.Add(NewClass16)
        Dim NewClass17 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass17.Name = "Copyright"
        NewClass17.Description = "The Copyright class stores copyright information."
        NewLib.Classes.Add(NewClass17)
        Dim NewClass18 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass18.Name = "License"
        NewClass18.Description = "The License class stores license information."
        NewLib.Classes.Add(NewClass18)
        Dim NewClass19 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass19.Name = "SourceCode"
        NewClass19.Description = "The SourceCode class stores information about the source code for the application."
        NewLib.Classes.Add(NewClass19)
        Dim NewClass20 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass20.Name = "Usage"
        NewClass20.Description = "The Usage class stores information about application or project usage."
        NewLib.Classes.Add(NewClass20)
        Dim NewClass21 As New ADVL_Utilities_Library_1.ClassSummary
        NewClass21.Name = "Trademark"
        NewClass21.Description = "The Trademark class stored information about a trademark used by the author of an application or data."
        NewLib.Classes.Add(NewClass21)

        ApplicationInfo.Libraries.Add(NewLib)

        'Add other library information here: --------------------------------------------------------------------------

    End Sub

    'Save the form settings if the form is being minimised:
    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H112 Then 'SysCommand
            If m.WParam.ToInt32 = &HF020 Then 'Form is being minimised
                SaveFormSettings()
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub SaveProjectSettings()
        'Save the project settings in an XML file.
        'Add any Project Settings to be saved into the settingsData XDocument.
        Dim settingsData = <?xml version="1.0" encoding="utf-8"?>
                           <!---->
                           <!--Project settings for ADVL_Coordinates_1 application.-->
                           <ProjectSettings>
                           </ProjectSettings>

        'Dim SettingsFileName As String = "ProjectSettings_" & ApplicationInfo.Name & "_" & Me.Text & ".xml"
        Dim SettingsFileName As String = "ProjectSettings_" & ApplicationInfo.Name & "_" & ".xml"
        Project.SaveXmlSettings(SettingsFileName, settingsData)

    End Sub

    Private Sub RestoreProjectSettings()
        'Restore the project settings from an XML document.

        'Dim SettingsFileName As String = "ProjectSettings_" & ApplicationInfo.Name & "_" & Me.Text & ".xml"
        Dim SettingsFileName As String = "ProjectSettings_" & ApplicationInfo.Name & "_" & ".xml"

        If Project.SettingsFileExists(SettingsFileName) Then
            Dim Settings As System.Xml.Linq.XDocument
            Project.ReadXmlSettings(SettingsFileName, Settings)

            If IsNothing(Settings) Then 'There is no Settings XML data.
                Exit Sub
            End If

            'Restore a Project Setting example:
            If Settings.<ProjectSettings>.<Setting1>.Value = Nothing Then
                'Project setting not saved.
                'Setting1 = ""
            Else
                'Setting1 = Settings.<ProjectSettings>.<Setting1>.Value
            End If

            'Continue restoring saved settings.



        End If

    End Sub

#End Region 'Process XML Files ----------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Display Methods - Code used to display this form." '============================================================================================================================

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Write the startup messages in a stringbuilder object.
        'Messages cannot be written using Message.Add until this is set up later in the startup sequence.
        Dim sb As New System.Text.StringBuilder
        sb.Append("------------------- Starting Application: ADVL Code Examples ---------------------------------------------------------------- " & vbCrLf)

        'Set the Application Directory path: ------------------------------------------------
        Project.ApplicationDir = My.Application.Info.DirectoryPath.ToString

        'Read the Application Information file: ---------------------------------------------
        ApplicationInfo.ApplicationDir = My.Application.Info.DirectoryPath.ToString 'Set the Application Directory property

        If ApplicationInfo.ApplicationLocked Then
            MessageBox.Show("The application is locked. If the application is not already in use, remove the 'Application_Info.lock file from the application directory: " & ApplicationInfo.ApplicationDir, "Notice", MessageBoxButtons.OK)
            Dim dr As System.Windows.Forms.DialogResult
            dr = MessageBox.Show("Press 'Yes' to unlock the application", "Notice", MessageBoxButtons.YesNo)
            If dr = System.Windows.Forms.DialogResult.Yes Then
                ApplicationInfo.UnlockApplication()
            Else
                Application.Exit()
                'System.Windows.Forms.Application.Exit()
            End If
        End If

        ReadApplicationInfo()
        ApplicationInfo.LockApplication()

        'Read the Application Usage information: --------------------------------------------
        ApplicationUsage.StartTime = Now
        ApplicationUsage.SaveLocn.Type = ADVL_Utilities_Library_1.FileLocation.Types.Directory
        ApplicationUsage.SaveLocn.Path = Project.ApplicationDir
        ApplicationUsage.RestoreUsageInfo()
        sb.Append("Application usage: Total duration = " & Format(ApplicationUsage.TotalDuration.TotalHours, "#0.##") & " hours" & vbCrLf)

        'Restore Project information: -------------------------------------------------------
        Project.ApplicationName = ApplicationInfo.Name
        Project.ReadLastProjectInfo()
        Project.ReadProjectInfoFile()
        Project.Usage.StartTime = Now

        'Project.ReadProjectInfoFile()

        ApplicationInfo.SettingsLocn = Project.SettingsLocn

        'Set up the Message object:
        Message.ApplicationName = ApplicationInfo.Name
        Message.SettingsLocn = Project.SettingsLocn


        'Set up the Tree View:

        'Set up the ImageList1
        'ImageList1.Images.Add()

        'Dim mainNode As New TreeNode
        'mainNode.Name = "mainNode"
        'mainNode.Text = "Main"
        'TreeView1.Nodes.Add(mainNode)

        'Add list of new library item types:
        cmbNewLibraryItemType.Items.Add("Collection")
        cmbNewLibraryItemType.Items.Add("Book")
        cmbNewLibraryItemType.Items.Add("  Section")
        cmbNewLibraryItemType.Items.Add("Application")
        'cmbNewLibraryItemType.Items.Add("  Source Code")
        cmbNewLibraryItemType.Items.Add("  Code")
        cmbNewLibraryItemType.Items.Add("  Form")
        cmbNewLibraryItemType.Items.Add("Picture Album")
        cmbNewLibraryItemType.Items.Add("  Picture")
        cmbNewLibraryItemType.Items.Add("Note")
        cmbNewLibraryItemType.Items.Add("Process")
        'cmbNewLibraryItemType.Items.Add("Library")
        'cmbNewLibraryItemType.Items.Add("PowerPoint")
        'cmbNewLibraryItemType.Items.Add("Excel")
        'cmbNewLibraryItemType.Items.Add("ADVL Application")
        'cmbNewLibraryItemType.Items.Add("Web Site")
        'cmbNewLibraryItemType.Items.Add("  Page")

        'Set up trvLibrary tree view:
        trvLibrary.ImageList = ImageList1
        'Dim Node1 As TreeNode = New TreeNode("My Library", 0, 1) 'The TreeView will later be saved in an XML file. An XElement cannot have a space in the element name.
        'Node1.Name = "My_Library.Libr"
        'trvLibrary.Nodes.Add(Node1)

        dgvDeletedItems.ColumnCount = 5
        dgvDeletedItems.Columns(0).HeaderText = "Node Text"
        dgvDeletedItems.Columns(1).HeaderText = "Node Key"
        dgvDeletedItems.Columns(2).HeaderText = "Parent Node"
        dgvDeletedItems.Columns(3).HeaderText = "Node Index"
        dgvDeletedItems.Columns(4).HeaderText = "Date Deleted"
        dgvDeletedItems.AllowUserToAddRows = False
        dgvDeletedItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect


        RestoreFormSettings() 'Restore the form settings
        RestoreProjectSettings() 'Restore the Project settings

        'Show the project information: ------------------------------------------------------
        txtProjectName.Text = Project.Name
        txtProjectDescription.Text = Project.Description
        Select Case Project.Type
            Case ADVL_Utilities_Library_1.Project.Types.Directory
                txtProjectType.Text = "Directory"
            Case ADVL_Utilities_Library_1.Project.Types.Archive
                txtProjectType.Text = "Archive"
            Case ADVL_Utilities_Library_1.Project.Types.Hybrid
                txtProjectType.Text = "Hybrid"
            Case ADVL_Utilities_Library_1.Project.Types.None
                txtProjectType.Text = "None"
        End Select
        txtCreationDate.Text = Format(Project.Usage.FirstUsed, "d-MMM-yyyy H:mm:ss")
        txtLastUsed.Text = Format(Project.Usage.LastUsed, "d-MMM-yyyy H:mm:ss")
        Select Case Project.SettingsLocn.Type
            Case ADVL_Utilities_Library_1.FileLocation.Types.Directory
                txtSettingsLocationType.Text = "Directory"
            Case ADVL_Utilities_Library_1.FileLocation.Types.Archive
                txtSettingsLocationType.Text = "Archive"
        End Select
        txtSettingsLocationPath.Text = Project.SettingsLocn.Path
        Select Case Project.DataLocn.Type
            Case ADVL_Utilities_Library_1.FileLocation.Types.Directory
                txtDataLocationType.Text = "Directory"
            Case ADVL_Utilities_Library_1.FileLocation.Types.Archive
                txtDataLocationType.Text = "Archive"
        End Select
        txtDataLocationPath.Text = Project.DataLocn.Path

        pbIconCollection.Image = ImageList1.Images(2)
        pbIconBook.Image = ImageList1.Images(4)
        pbIconBookSection.Image = ImageList1.Images(6)
        pbIconApplication.Image = ImageList1.Images(8)
        pbIconCode.Image = ImageList1.Images(10)
        pbIconForm.Image = ImageList1.Images(12)
        pbIconAlbum.Image = ImageList1.Images(14)
        pbIconPicture.Image = ImageList1.Images(16)
        pbIconPowerPoint.Image = ImageList1.Images(18)
        pbIconExcel.Image = ImageList1.Images(20)
        pbIconAdvl.Image = ImageList1.Images(22)
        pbIconLib.Image = ImageList1.Images(0)
        pbIconNote.Image = ImageList1.Images(26)
        pbIconWebsite.Image = ImageList1.Images(28)
        pbIconWebpage.Image = ImageList1.Images(30)
        pbIconProcess.Image = ImageList1.Images(32)
        pbIconDateTime.Image = ImageList1.Images(35)

        pbIconAuthor.Image = ImageList1.Images(75)
        pbIconCopyright.Image = ImageList1.Images(81)
        pbIconLicense.Image = ImageList1.Images(69)

        pbIconDefaultAuthor.Image = ImageList1.Images(75)
        pbIconDefaultCopyright.Image = ImageList1.Images(81)
        pbIconDefaultLicense.Image = ImageList1.Images(69)

        'NOTE: Will try to add this on the dorm design view.
        'ContextMenuStrip1.Items.Add("View document updates and versions")

        UpdateLibraryList()


        sb.Append("------------------- Started OK ------------------------------------------------------------------------------------------------------------------------ " & vbCrLf & vbCrLf)
        Me.Show() 'Show this form before showing the Message form
        Message.Add(sb.ToString)

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit the Application

        DisconnectFromAppNet() 'Disconnect from the Application Network.

        'SaveFormSettings() 'Save the settings of this form. 'THESE ARE SAVED WHEN THE FORM_CLOSING EVENT TRIGGERS.
        SaveProjectSettings() 'Save project settings.

        ApplicationInfo.WriteFile() 'Update the Application Information file.
        ApplicationInfo.UnlockApplication()

        Project.SaveLastProjectInfo() 'Save information about the last project used.

        'Project.SaveProjectInfoFile() 'Update the Project Information file. This is not required unless there is a change made to the project.

        Project.Usage.SaveUsageInfo() 'Save Project usage information.

        ApplicationUsage.SaveUsageInfo() 'Save Application usage information.

        'SaveLibraryFile()
        SaveLibrary(LibraryFileName)

        Application.Exit()

    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Save the form settings if the form state is normal. (A minimised form will have the incorrect size and location.)
        If WindowState = FormWindowState.Normal Then
            SaveFormSettings()
        End If
    End Sub


#End Region 'Form Display Methods -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Open and Close Forms - Code used to open and close other forms." '===================================================================================================================

    'Private Sub btnOpenTemplateForm_Click(sender As Object, e As EventArgs) Handles btnOpenTemplateForm.Click
    '    'Open the Template form:
    '    If IsNothing(TemplateForm) Then
    '        TemplateForm = New frmTemplate
    '        TemplateForm.Show()
    '    Else
    '        TemplateForm.Show()
    '    End If
    'End Sub

    'Private Sub TemplateForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles TemplateForm.FormClosed
    '    TemplateForm = Nothing
    'End Sub

    Private Sub btnMessages_Click(sender As Object, e As EventArgs) Handles btnMessages.Click
        'Show the Messages form.
        Message.ApplicationName = ApplicationInfo.Name
        Message.SettingsLocn = Project.SettingsLocn
        Message.Show()
        Message.MessageForm.BringToFront()
    End Sub

    Private Sub OpenCodeViewFormNo(ByVal Index As Integer)
        'Open the CodeView form with specified Index number.

        'If CodeViewList.Count < Index + 1 Then
        If DocViewList.Count < Index + 1 Then
            'Insert null entries into CodeViewList then add a new form at the specified index position:
            Dim I As Integer
            For I = DocViewList.Count To Index
                DocViewList.Add(Nothing)
            Next
            'CodeView = New frmDocumentView
            DocView = New frmDocumentView
            DocViewList(Index) = DocView
            DocViewList(Index).FormNo = Index
            DocViewList(Index).Show
            'ElseIf SharePricesList(Index) = Nothing Then
        ElseIf IsNothing(DocViewList(Index)) Then
            'Add the new form at specified index position:
            DocView = New frmDocumentView
            DocViewList(Index) = DocView
            DocViewList(Index).FormNo = Index
            DocViewList(Index).Show()
        Else
            'The form at the specified index position is already displayed.
            DocViewList(Index).BringToFront()
        End If
    End Sub

    Private Sub OpenNewCodeViewForm()
        'Code to show multiple instances if the form:
        DocView = New frmDocumentView
        'If CodeViewList.Count = 0 Then
        If DocViewList.Count = 0 Then
            DocViewList.Add(DocView)
            DocViewList(0).FormNo = 0
            DocViewList(0).Show()
        Else
            Dim I As Integer
            Dim FormAdded As Boolean = False
            For I = 0 To DocViewList.Count - 1 'Check if there are closed forms in CodeViewListList. They can be re-used.
                If IsNothing(DocViewList(I)) Then
                    DocViewList(I) = DocView
                    DocViewList(I).FormNo = I
                    DocViewList(I).Show()
                    FormAdded = True
                    Exit For
                End If
            Next
            If FormAdded = False Then 'Add a new form to CodeViewListList.
                Dim FormNo As Integer
                DocViewList.Add(DocView)
                FormNo = DocViewList.Count - 1
                DocViewList(FormNo).FormNo = FormNo
                DocViewList(FormNo).Show()
            End If
        End If
    End Sub

    Public Sub DocumentViewFormClosed() 'This code is used if multiple Document View forms are to be shown.
        'This subroutine is called when the DocumentView form has been closed.
        'The subroutine is usually called from the FormClosed event of the DocumentView form.
        'The DocumentView form may have multiple instances.
        'The ClosedFormNumber property should contains the number of the instance of the DocumentView form.
        'This property should be updated by the DocumentView form when it is being closed.
        'The ClosedFormNumber property value is used to determine which element in DocumentViewList should be set to Nothing.

        'If CodeViewList.Count < ClosedFormNo + 1 Then
        If DocViewList.Count < ClosedFormNo + 1 Then
            'ClosedFormNo is too large to exist in CodeViewList
            Exit Sub
        End If

        If IsNothing(DocViewList(ClosedFormNo)) Then
            'The form is already set to nothing
        Else
            DocViewList(ClosedFormNo) = Nothing
        End If

    End Sub

    Public Sub UtilityDocViewFormClosed() 'This code is used if multiple Utility Document View forms are to be shown.
        'This subroutine is called when the UtilityDocView form has been closed.
        'The subroutine is usually called from the FormClosed event of the UtilityDocView form.
        'The DocumentView form may have multiple instances.
        'The ClosedFormNumber property should contains the number of the instance of the DocumentView form.
        'This property should be updated by the DocumentView form when it is being closed.
        'The ClosedFormNumber property value is used to determine which element in DocumentViewList should be set to Nothing.

        'If CodeViewList.Count < ClosedFormNo + 1 Then
        'If DocViewList.Count < ClosedFormNo + 1 Then
        If UtilViewList.Count < ClosedFormNo + 1 Then
            'ClosedFormNo is too large to exist in CodeViewList
            Exit Sub
        End If

        'If IsNothing(DocViewList(ClosedFormNo)) Then
        If IsNothing(UtilViewList(ClosedFormNo)) Then
            'The form is already set to nothing
        Else
            'DocViewList(ClosedFormNo) = Nothing
            UtilViewList(ClosedFormNo) = Nothing
        End If
    End Sub

    Private Sub btnNewLibrary_Click(sender As Object, e As EventArgs) Handles btnNewLibrary.Click
        'Open the New Library form:
        If IsNothing(NewLibrary) Then
            NewLibrary = New frmNewLibrary
            NewLibrary.Show()
        Else
            NewLibrary.Show()
        End If
    End Sub

    Private Sub NewLibrary_FormClosed(sender As Object, e As FormClosedEventArgs) Handles NewLibrary.FormClosed
        NewLibrary = Nothing
    End Sub


    Private Sub btnViewAuthor_Click(sender As Object, e As EventArgs) Handles btnViewAuthor.Click
        'Open the DocumentLicenseInfo form at the Author tab.
        If txtItemType.Text = "Collection" Then
            'Open DocumentLicenseInfo form for viewing and editing.
            If IsNothing(DocumentLicenseInfo) Then
                DocumentLicenseInfo = New frmLicense
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = False
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text 'Setting the Author file last leaves the Author tab open
            Else
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = False
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text 'Setting the Author file last leaves the Author tab open
            End If
        Else
            'Open DocumentLicenseInfo form for viewing only.
            If IsNothing(DocumentLicenseInfo) Then
                DocumentLicenseInfo = New frmLicense
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = True
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text 'Setting the Author file last leaves the Author tab open
            Else
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = True
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text 'Setting the Author file last leaves the Author tab open
            End If
        End If


    End Sub


    Private Sub btnViewCopyright_Click(sender As Object, e As EventArgs) Handles btnViewCopyright.Click
        'Open the LicenseInfo form at the Copyright tab.

        If txtItemType.Text = "Collection" Then
            If IsNothing(DocumentLicenseInfo) Then
                DocumentLicenseInfo = New frmLicense
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = False
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
            Else
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = False
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
            End If
        Else
            If IsNothing(DocumentLicenseInfo) Then
                DocumentLicenseInfo = New frmLicense
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = True
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
            Else
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = True
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
            End If
        End If


    End Sub

    Private Sub btnViewLicense_Click(sender As Object, e As EventArgs) Handles btnViewLicense.Click
        'Open the LicenseInfo form at the License tab.

        If txtItemType.Text = "Collection" Then
            If IsNothing(DocumentLicenseInfo) Then
                DocumentLicenseInfo = New frmLicense
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = False
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text 'Setting the License file last leaves the License tab open
            Else
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = False
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text 'Setting the License file last leaves the License tab open
            End If
        Else
            If IsNothing(DocumentLicenseInfo) Then
                DocumentLicenseInfo = New frmLicense
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = True
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text 'Setting the License file last leaves the License tab open
            Else
                DocumentLicenseInfo.Show()
                DocumentLicenseInfo.ViewOnly = True
                DocumentLicenseInfo.AuthorFile = txtAuthorFile.Text
                DocumentLicenseInfo.CopyrightFile = txtCopyrightFile.Text
                DocumentLicenseInfo.LicenseFile = txtLicenseFile.Text 'Setting the License file last leaves the License tab open
            End If
        End If


    End Sub

    Private Sub DocumentLicenseInfo_FormClosed(sender As Object, e As FormClosedEventArgs) Handles DocumentLicenseInfo.FormClosed
        DocumentLicenseInfo = Nothing
    End Sub

    Private Sub btnEditDefaultAuthor_Click(sender As Object, e As EventArgs) Handles btnEditDefaultAuthor.Click
        'Open the LibraryLicenseInfo form at the Author tab.

        If IsNothing(LibraryLicenseInfo) Then
            LibraryLicenseInfo = New frmLicense
            LibraryLicenseInfo.Show()
            LibraryLicenseInfo.ViewOnly = False
            LibraryLicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LibraryLicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LibraryLicenseInfo.AuthorFile = txtDefaultAuthorFile.Text 'Setting the Author file last leaves the Author tab open
        Else
            LibraryLicenseInfo.Show()
            LibraryLicenseInfo.ViewOnly = False
            LibraryLicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LibraryLicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LibraryLicenseInfo.AuthorFile = txtDefaultAuthorFile.Text 'Setting the Author file last leaves the Author tab open
        End If
    End Sub

    Private Sub btnEditDefaultCopyright_Click(sender As Object, e As EventArgs) Handles btnEditDefaultCopyright.Click
        'Open the LicenseInfo form at the Copyright tab.

        If IsNothing(LibraryLicenseInfo) Then
            LibraryLicenseInfo = New frmLicense
            LibraryLicenseInfo.Show()
            LibraryLicenseInfo.ViewOnly = False
            LibraryLicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LibraryLicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LibraryLicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
        Else
            LibraryLicenseInfo.Show()
            LibraryLicenseInfo.ViewOnly = False
            LibraryLicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LibraryLicenseInfo.LicenseFile = txtDefaultLicenseFile.Text
            LibraryLicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text 'Setting the Copyright file last leaves the Copyright tab open
        End If
    End Sub

    Private Sub btnEditDefaultLicense_Click(sender As Object, e As EventArgs) Handles btnEditDefaultLicense.Click
        'Open the LibraryLicenseInfo form at the License tab.

        If IsNothing(LibraryLicenseInfo) Then
            LibraryLicenseInfo = New frmLicense
            LibraryLicenseInfo.Show()
            LibraryLicenseInfo.ViewOnly = False
            LibraryLicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LibraryLicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LibraryLicenseInfo.LicenseFile = txtDefaultLicenseFile.Text 'Setting the License file last leaves the License tab open
        Else
            LibraryLicenseInfo.Show()
            LibraryLicenseInfo.ViewOnly = False
            LibraryLicenseInfo.AuthorFile = txtDefaultAuthorFile.Text
            LibraryLicenseInfo.CopyrightFile = txtDefaultCopyrightFile.Text
            LibraryLicenseInfo.LicenseFile = txtDefaultLicenseFile.Text 'Setting the License file last leaves the License tab open
        End If
    End Sub

    Private Sub LibraryLicenseInfo_FormClosed(sender As Object, e As FormClosedEventArgs) Handles LibraryLicenseInfo.FormClosed
        LibraryLicenseInfo = Nothing
    End Sub

#End Region 'Open and Close Forms -------------------------------------------------------------------------------------------------------------------------------------------------------------

#Region " Form Methods - The main actions performed by this form." '---------------------------------------------------------------------------------------------------------------------------


    Private Sub LibraryLicenseInfo_ApplyAuthor(AuthorFile As String, AuthorSummary As String) Handles LibraryLicenseInfo.ApplyAuthor
        'txtDefaultAuthorFile.Text = AuthorFile
        'txtDefaultAuthorSummary.Text = AuthorSummary
        DefaultAuthorFile = AuthorFile
        DefaultAuthorSummary = AuthorSummary
    End Sub

    Private Sub LibraryLicenseInfo_ApplyCopyright(CopyrightFile As String, CopyrightSummary As String) Handles LibraryLicenseInfo.ApplyCopyright
        DefaultCopyrightFile = CopyrightFile
        DefaultCopyrightSummary = CopyrightSummary
    End Sub

    Private Sub LibraryLicenseInfo_ApplyLicense(LicenseFile As String, LicenseSummary As String) Handles LibraryLicenseInfo.ApplyLicense
        DefaultLicenseFile = LicenseFile
        DefaultLicenseSummary = LicenseSummary
    End Sub

    Private Sub btnProject_Click(sender As Object, e As EventArgs) Handles btnProject.Click
        Project.SelectProject()
    End Sub

    Private Sub btnAppInfo_Click(sender As Object, e As EventArgs) Handles btnAppInfo.Click
        ApplicationInfo.ShowInfo()
    End Sub

    Private Sub btnAndorville_Click(sender As Object, e As EventArgs) Handles btnAndorville.Click
        ApplicationInfo.ShowInfo()
    End Sub

#Region " Project Events Code"

    Private Sub Project_Message(Msg As String) Handles Project.Message
        'Display the Project message:
        Message.Add(Msg & vbCrLf)
    End Sub

    Private Sub Project_ErrorMessage(Msg As String) Handles Project.ErrorMessage
        'Display the Project error message:
        Message.AddWarning(Msg & vbCrLf)
    End Sub

    Private Sub Project_Closing() Handles Project.Closing
        'The current project is closing.

        SaveFormSettings() 'Save the form settings - they are saved in the Project before is closes.
        SaveProjectSettings() 'Update this subroutine if project settings need to be saved.

        'Save the current project usage information:
        Project.Usage.SaveUsageInfo()
    End Sub

    Private Sub Project_Selected() Handles Project.Selected
        'A new project has been selected.

        RestoreFormSettings()
        Project.ReadProjectInfoFile()
        Project.Usage.StartTime = Now

        ApplicationInfo.SettingsLocn = Project.SettingsLocn
        Message.SettingsLocn = Project.SettingsLocn

        'Restore the new project settings:
        RestoreProjectSettings() 'Update this subroutine if project settings need to be restored.

        'Show the project information:
        txtProjectName.Text = Project.Name
        txtProjectDescription.Text = Project.Description
        Select Case Project.Type
            Case ADVL_Utilities_Library_1.Project.Types.Directory
                txtProjectType.Text = "Directory"
            Case ADVL_Utilities_Library_1.Project.Types.Archive
                txtProjectType.Text = "Archive"
            Case ADVL_Utilities_Library_1.Project.Types.Hybrid
                txtProjectType.Text = "Hybrid"
            Case ADVL_Utilities_Library_1.Project.Types.None
                txtProjectType.Text = "None"
        End Select

        txtCreationDate.Text = Format(Project.CreationDate, "d-MMM-yyyy H:mm:ss")
        txtLastUsed.Text = Format(Project.Usage.LastUsed, "d-MMM-yyyy H:mm:ss")
        Select Case Project.SettingsLocn.Type
            Case ADVL_Utilities_Library_1.FileLocation.Types.Directory
                txtSettingsLocationType.Text = "Directory"
            Case ADVL_Utilities_Library_1.FileLocation.Types.Archive
                txtSettingsLocationType.Text = "Archive"
        End Select
        txtSettingsLocationPath.Text = Project.SettingsLocn.Path
        Select Case Project.DataLocn.Type
            Case ADVL_Utilities_Library_1.FileLocation.Types.Directory
                txtDataLocationType.Text = "Directory"
            Case ADVL_Utilities_Library_1.FileLocation.Types.Archive
                txtDataLocationType.Text = "Archive"
        End Select
        txtDataLocationPath.Text = Project.DataLocn.Path

    End Sub

#End Region 'Project Events Code

#Region " Online/Offline Code"

    Private Sub btnOnline_Click(sender As Object, e As EventArgs) Handles btnOnline.Click
        'Connect to or disconnect from the Application Network.
        If ConnectedToAppnet = False Then
            ConnectToAppNet()
        Else
            DisconnectFromAppNet()
        End If
    End Sub

    Private Sub ConnectToAppNet()
        'Connect to the Application Network. (Message Exchange)

        Dim Result As Boolean

        If IsNothing(client) Then
            client = New ServiceReference1.MsgServiceClient(New System.ServiceModel.InstanceContext(New MsgServiceCallback))
        End If

        If client.State = ServiceModel.CommunicationState.Faulted Then
            Message.SetWarningStyle()
            Message.Add("client state is faulted. Connection not made!" & vbCrLf)
        Else
            Try
                client.Endpoint.Binding.SendTimeout = New System.TimeSpan(0, 0, 8) 'Temporarily set the send timeaout to 8 seconds

                Result = client.Connect(ApplicationInfo.Name, ServiceReference1.clsConnectionAppTypes.Application, False, False) 'Application Name is "Application_Template"
                'appName, appType, getAllWarnings, getAllMessages

                If Result = True Then
                    Message.Add("Connected to the Application Network as " & ApplicationInfo.Name & vbCrLf)
                    client.Endpoint.Binding.SendTimeout = New System.TimeSpan(1, 0, 0) 'Restore the send timeaout to 1 hour
                    btnOnline.Text = "Online"
                    btnOnline.ForeColor = Color.ForestGreen
                    ConnectedToAppnet = True
                    SendApplicationInfo()
                Else
                    Message.Add("Connection to the Application Network failed!" & vbCrLf)
                    client.Endpoint.Binding.SendTimeout = New System.TimeSpan(1, 0, 0) 'Restore the send timeaout to 1 hour
                End If
            Catch ex As System.TimeoutException
                Message.Add("Timeout error. Check if the Application Network is running." & vbCrLf)
            Catch ex As Exception
                Message.Add("Error message: " & ex.Message & vbCrLf)
                client.Endpoint.Binding.SendTimeout = New System.TimeSpan(1, 0, 0) 'Restore the send timeaout to 1 hour
            End Try
        End If

    End Sub

    Private Sub DisconnectFromAppNet()
        'Disconnect from the Application Network.

        Dim Result As Boolean

        If IsNothing(client) Then
            Message.Add("Already disconnected from the Application Network." & vbCrLf)
            btnOnline.Text = "Offline"
            btnOnline.ForeColor = Color.Red
            ConnectedToAppnet = False
        Else
            If client.State = ServiceModel.CommunicationState.Faulted Then
                Message.Add("client state is faulted." & vbCrLf)
            Else
                Try
                    Message.Add("Running client.Disconnect(ApplicationName)   ApplicationName = " & ApplicationInfo.Name & vbCrLf)
                    client.Disconnect(ApplicationInfo.Name) 'NOTE: If Application Network has closed, this application freezes at this line! Try Catch EndTry added to fix this.
                    btnOnline.Text = "Offline"
                    btnOnline.ForeColor = Color.Red
                    ConnectedToAppnet = False
                Catch ex As Exception
                    Message.SetWarningStyle()
                    Message.Add("Error disconnecting from Application Network: " & ex.Message & vbCrLf)
                End Try
            End If
        End If
    End Sub

    Private Sub SendApplicationInfo()
        'Send the application information to the Administrator connections.

        If IsNothing(client) Then
            Message.Add("No client connection available!" & vbCrLf)
        Else
            If client.State = ServiceModel.CommunicationState.Faulted Then
                Message.Add("client state is faulted. Message not sent!" & vbCrLf)
            Else
                'Create the XML instructions to send application information.
                Dim decl As New XDeclaration("1.0", "utf-8", "yes")
                Dim doc As New XDocument(decl, Nothing) 'Create an XDocument to store the instructions.
                Dim xmessage As New XElement("XMsg") 'This indicates the start of the message in the XMessage class
                Dim applicationInfo As New XElement("ApplicationInfo")
                Dim name As New XElement("Name", Me.ApplicationInfo.Name)
                applicationInfo.Add(name)

                Dim exePath As New XElement("ExecutablePath", Me.ApplicationInfo.ExecutablePath)
                applicationInfo.Add(exePath)

                Dim directory As New XElement("Directory", Me.ApplicationInfo.ApplicationDir)
                applicationInfo.Add(directory)
                Dim description As New XElement("Description", Me.ApplicationInfo.Description)
                applicationInfo.Add(description)
                xmessage.Add(applicationInfo)
                doc.Add(xmessage)
                client.SendMessage("ApplicationNetwork", doc.ToString)
            End If
        End If

    End Sub

#End Region 'Online/Offline code

#Region " Process XMessages"

    Private Sub XMsg_Instruction(Info As String, Locn As String) Handles XMsg.Instruction
        'Process an XMessage instruction.
        'An XMessage is a simplified XSequence. It is used to exchange information between Andorville™ applications.
        '
        'An XSequence file is an AL-H7™ Information Vector Sequence stored in an XML format.
        'AL-H7™ is the name of a programming system that uses sequences of information and location value pairs to store data items or processing steps.
        'A single information and location value pair is called a knowledge element (or noxel).
        'Any program, mathematical expression or data set can be expressed as an Information Vector Sequence.

        'Add code here to process the XMessage instructions.
        'See other Andorville™ applciations for examples.

        Select Case Locn

            Case "EndOfSequence"
                'End of Information Vector Sequence reached.

            Case Else
                Message.SetWarningStyle()
                Message.Add("Unknown location: " & Locn & vbCrLf)
                Message.SetNormalStyle()

        End Select

    End Sub

    Private Sub SendMessage()
        'Code used to send a message after a timer delay.
        'The message destination is stored in MessageDest
        'The message text is stored in MessageText
        Timer1.Interval = 100 '100ms delay
        Timer1.Enabled = True 'Start the timer.
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If IsNothing(client) Then
            Message.AddWarning("No client connection available!" & vbCrLf)
        Else
            If client.State = ServiceModel.CommunicationState.Faulted Then
                Message.AddWarning("client state is faulted. Message not sent!" & vbCrLf)
            Else
                Try
                    Message.Add("Sending a message. Number of characters: " & MessageText.Length & vbCrLf)
                    'client.SendMessage(MessageDest, MessageText)
                    client.SendMessage(ClientAppName, MessageText)
                    'Message.XAdd(MessageText & vbCrLf) 'NOTE this is displayed in Property InstrReceived
                    MessageText = "" 'Clear the message after it has been sent.
                    ClientAppName = "" 'Clear the Client Application Name after the message has been sent.
                    ClientAppLocn = "" 'Clear the Client Application Location after the message has been sent.
                Catch ex As Exception
                    Message.AddWarning("Error sending message: " & ex.Message & vbCrLf)
                End Try
            End If
        End If

        'Stop timer:
        Timer1.Enabled = False
    End Sub



#End Region 'Process XMessages

    'Private Sub DisplaySelectedItemInfo()
    Private Sub DisplaySelectedItem()
        'Display information about the item selected on trvLibrary

        'If txtCurrentNodeKey.Text.EndsWith(".Code") Then
        If SelectedNodeKey.EndsWith(".Code") Then
            'Message.Add("Selected item is a code section." & vbCrLf)
            txtItemType.Text = "Code section"
            DisplayDefaultDocument()
            'ElseIf txtCurrentNodeKey.Text.EndsWith(".Coll") Then
        ElseIf SelectedNodeKey.EndsWith(".Coll") Then
            'Message.Add("Selected item is a collection." & vbCrLf)
            txtItemType.Text = "Collection"
            DisplayCollectionInfo()
            'Clear the Document display:
            dgvDocument.Hide()
            rtbDocument.Show()
            rtbDocument.Clear()
        ElseIf SelectedNodeKey.EndsWith(".Libr") Then
            txtItemType.Text = "Library"
            DisplayLibraryInfo()
            'Clear the Document display:
            dgvDocument.Hide()
            rtbDocument.Show()
            rtbDocument.Clear()
        ElseIf SelectedNodeKey.EndsWith(".Note") Then
            txtItemType.Text = "Note"
            DisplayNote()
        Else

            'Message.AddWarning("No settings have been found for " & txtCurrentNodeKey.Text & vbCrLf)
            Message.AddWarning("No settings have been found for: " & SelectedNodeKey & vbCrLf)
            'Blank the Document tab:
            dgvDocument.Hide()
            rtbDocument.Show()
            rtbDocument.Clear()
            dgvDocument.Rows.Clear()
            dgvDocument.Columns.Clear()

            'Blank the Document Information tab:
            'txtItemLabel.Text = trvLibrary.SelectedNode.Text
            txtItemLabel.Text = SelectedNodeText
            'txtItemFileName.Text = trvLibrary.SelectedNode.Name
            txtItemFileName.Text = SelectedNodeKey

            txtItemDescription.Text = ""
            txtItemCreationDate.Text = ""
            txtItemLastEditDate.Text = ""
            txtAuthorFile.Text = ""
            txtAuthorSummary.Text = ""
            txtCopyrightFile.Text = ""
            txtCopyrightSummary.Text = ""
            txtLicenseFile.Text = ""
            rtbLicenseNotice.Text = ""

        End If

    End Sub

    Private Sub DocumentLicenseInfo_ApplyAuthor(AuthorFile As String, AuthorSummary As String) Handles DocumentLicenseInfo.ApplyAuthor
        txtAuthorFile.Text = AuthorFile
        txtAuthorSummary.Text = AuthorSummary
    End Sub

    Private Sub DocumentLicenseInfo_ApplyCopyright(CopyrightFile As String, CopyrightSummary As String) Handles DocumentLicenseInfo.ApplyCopyright
        txtCopyrightFile.Text = CopyrightFile
        txtCopyrightSummary.Text = CopyrightSummary
    End Sub

    Private Sub DocumentLicenseInfo_ApplyLicense(LicenseFile As String, LicenseSummary As String) Handles DocumentLicenseInfo.ApplyLicense
        txtLicenseFile.Text = LicenseFile
        rtbLicenseNotice.Text = LicenseSummary
    End Sub

    Public Sub SetDefaultDocInfo(ByVal DocName As String, ByVal DocInfo As DefaultDocInfo)
        'Sets the Default document Update/Version for the selected document name.

        If DefaultDocInfo.ContainsKey(DocName) Then
            DefaultDocInfo(DocName) = DocInfo
        Else
            DefaultDocInfo.Add(DocName, DocInfo)
        End If

        DisplayDefaultDocument()

    End Sub

    Private Sub DisplayDefaultDocument()
        'Display the default Document Update/Version of the selected document name.

        SetUpDocInfoTab()

        'txtCurrentNodeKey.Text contains the selected document name.
        'If DefaultDocInfo.ContainsKey(txtCurrentNodeKey.Text) Then
        If DefaultDocInfo.ContainsKey(SelectedNodeKey) Then
            'Show the Default Document Information:
            'txtItemLabel.Text = DefaultDocInfo(txtCurrentNodeKey.Text).Label
            txtItemLabel.Text = DefaultDocInfo(SelectedNodeKey).Label
            txtItemFileName.Text = DefaultDocInfo(SelectedNodeKey).FileName
            txtItemDescription.Text = DefaultDocInfo(SelectedNodeKey).Description
            txtItemCreationDate.Text = DefaultDocInfo(SelectedNodeKey).CreationDate
            txtItemLastEditDate.Text = DefaultDocInfo(SelectedNodeKey).LastEditDate
            txtAuthorFile.Text = DefaultDocInfo(SelectedNodeKey).AuthorFile
            txtAuthorSummary.Text = DefaultDocInfo(SelectedNodeKey).AuthorSummary
            txtCopyrightFile.Text = DefaultDocInfo(SelectedNodeKey).CopyrightFile
            txtCopyrightSummary.Text = DefaultDocInfo(SelectedNodeKey).CopyrightSummary
            txtLicenseFile.Text = DefaultDocInfo(SelectedNodeKey).LicenseFile
            rtbLicenseNotice.Text = DefaultDocInfo(SelectedNodeKey).LicenseSummary

            If DefaultDocInfo(txtCurrentNodeKey.Text).FileName.EndsWith(".rtf") Then
                dgvDocument.Hide()
                rtbDocument.Show()
                Dim rtbData As New IO.MemoryStream()
                'Project.ReadData(DefaultDocInfo(txtCurrentNodeKey.Text).FileName, rtbData)
                Project.ReadData(DefaultDocInfo(SelectedNodeKey).FileName, rtbData)
                rtbDocument.Clear()
                rtbData.Position = 0
                rtbDocument.LoadFile(rtbData, RichTextBoxStreamType.RichText)
                rtbDocument.BackColor = Color.FromArgb(250, 250, 250)

            End If

        Else
            'Message.AddWarning("No default document has been selected for " & txtCurrentNodeKey.Text & vbCrLf)
            Message.AddWarning("No default document has been selected for " & SelectedNodeKey & vbCrLf)
            'Blank the Document tab:
            dgvDocument.Hide()
            rtbDocument.Show()
            rtbDocument.Clear()
            dgvDocument.Rows.Clear()
            dgvDocument.Columns.Clear()
            'Blank the Document Information tab:
            txtItemLabel.Text = ""
            txtItemFileName.Text = ""
            txtItemDescription.Text = ""
            txtItemCreationDate.Text = ""
            txtItemLastEditDate.Text = ""
            txtAuthorFile.Text = ""
            txtAuthorSummary.Text = ""
            txtCopyrightFile.Text = ""
            txtCopyrightSummary.Text = ""
            txtLicenseFile.Text = ""
            rtbLicenseNotice.Text = ""
        End If

    End Sub

    Private Sub SetUpDocInfoTab()
        'Set up the item information tab to display document information.

        GroupBox3.Text = "Author" 'Author information group box
        GroupBox7.Text = "Copyright" 'Copyright information group box
        GroupBox8.Text = "License" 'License information group box

        btnViewAuthor.Text = "View"
        btnViewCopyright.Text = "View"
        btnViewLicense.Text = "View"

    End Sub

    Private Sub DisplayCollectionInfo()
        'Display information about the selected collection node.

        SetUpCollectionInfoTab()

        'txtNodeKey.Text contains the node key.

        'If CollectionInfo.ContainsKey(txtNodeKey.Text) Then
        If CollectionInfo.ContainsKey(SelectedNodeKey) Then

            'txtItemLabel.Text = txtNodeText.Text
            txtItemLabel.Text = SelectedNodeText
            'txtItemFileName.Text = txtNodeKey.Text
            txtItemFileName.Text = SelectedNodeKey

            'txtItemDescription.Text = CollectionInfo(txtNodeKey.Text).Description
            txtItemDescription.Text = CollectionInfo(SelectedNodeKey).Description
            txtItemCreationDate.Text = CollectionInfo(SelectedNodeKey).CreationDate
            txtItemLastEditDate.Text = CollectionInfo(SelectedNodeKey).LastEditDate
            txtAuthorFile.Text = CollectionInfo(SelectedNodeKey).DefaultAuthorFile
            txtAuthorSummary.Text = CollectionInfo(SelectedNodeKey).DefaultAuthorSummary
            txtCopyrightFile.Text = CollectionInfo(SelectedNodeKey).DefaultCopyrightFile
            txtCopyrightSummary.Text = CollectionInfo(SelectedNodeKey).DefaultCopyrightSummary
            txtLicenseFile.Text = CollectionInfo(SelectedNodeKey).DefaultLicenseFile
            rtbLicenseNotice.Text = CollectionInfo(SelectedNodeKey).DefaultLicenseSummary
        Else
            'Message.AddWarning("No Collection settings have been found for " & txtCurrentNodeKey.Text & vbCrLf)
            Message.AddWarning("No Collection settings have been found for " & SelectedNodeKey & vbCrLf)
            'Blank the Document tab:
            dgvDocument.Hide()
            rtbDocument.Show()
            rtbDocument.Clear()
            dgvDocument.Rows.Clear()
            dgvDocument.Columns.Clear()

            'Blank the Document Information tab:
            'txtItemLabel.Text = trvLibrary.SelectedNode.Text
            txtItemLabel.Text = SelectedNodeText
            'txtItemFileName.Text = trvLibrary.SelectedNode.Name
            txtItemFileName.Text = SelectedNodeKey

            txtItemDescription.Text = ""
            txtItemCreationDate.Text = ""
            txtItemLastEditDate.Text = ""
            txtAuthorFile.Text = ""
            txtAuthorSummary.Text = ""
            txtCopyrightFile.Text = ""
            txtCopyrightSummary.Text = ""
            txtLicenseFile.Text = ""
            rtbLicenseNotice.Text = ""
        End If



    End Sub

    Private Sub SetUpCollectionInfoTab()
        'Set up the item information tab to display collection information.

        GroupBox3.Text = "Default Author" 'Author information group box
        GroupBox7.Text = "Default Copyright" 'Copyright information group box
        GroupBox8.Text = "Default License" 'License information group box

        btnViewAuthor.Text = "View/Edit"
        btnViewCopyright.Text = "View/Edit"
        btnViewLicense.Text = "View/Edit"

    End Sub

    Private Sub DisplayLibraryInfo()
        'Display information about the Library.

        SetUpCollectionInfoTab() 'The Library Info required the same setup as Collection Info.

        txtItemLabel.Text = SelectedNodeText
        txtItemFileName.Text = SelectedNodeKey

        txtItemDescription.Text = LibraryDescription
        txtItemCreationDate.Text = LibraryCreationDate
        txtItemLastEditDate.Text = CollectionInfo(SelectedNodeKey).LastEditDate
        txtAuthorFile.Text = DefaultAuthorFile
        txtAuthorSummary.Text = DefaultAuthorSummary
        txtCopyrightFile.Text = DefaultCopyrightFile
        txtCopyrightSummary.Text = DefaultCopyrightSummary
        txtLicenseFile.Text = DefaultLicenseFile
        rtbLicenseNotice.Text = DefaultLicenseSummary


    End Sub

    Private Sub UpdateLibraryList()
        'Update the list of libraries.

        Dim FileNameList As New ArrayList
        Project.GetDataFileList("Libr", FileNameList)
        lstLibraries.Items.Clear()

        For Each Item In FileNameList
            lstLibraries.Items.Add(Item)
        Next

    End Sub

    Private Sub SetUpNoteInfoTab()
        'Set up the item information tab to display Note information.

    End Sub

    Private Sub DisplayNote()
        'Display the Note.

        SetUpNoteInfoTab()

        dgvDocument.Hide()
        rtbDocument.Show()
        Dim rtbData As New IO.MemoryStream()
        Project.ReadData(SelectedNodeKey, rtbData)
        rtbDocument.Clear()
        rtbData.Position = 0
        rtbDocument.LoadFile(rtbData, RichTextBoxStreamType.RichText)
        'rtbDocument.BackColor = Color.FromArgb(250, 250, 250)
        rtbDocument.BackColor = Color.LemonChiffon

        If UtilDocInfo.ContainsKey(SelectedNodeKey) Then
            txtItemLabel.Text = SelectedNodeText
            txtItemFileName.Text = SelectedNodeKey

            txtItemDescription.Text = UtilDocInfo(SelectedNodeKey).Description
            txtItemCreationDate.Text = UtilDocInfo(SelectedNodeKey).CreationDate
            txtItemLastEditDate.Text = UtilDocInfo(SelectedNodeKey).LastEditDate
            txtAuthorFile.Text = UtilDocInfo(SelectedNodeKey).AuthorFile
            txtAuthorSummary.Text = UtilDocInfo(SelectedNodeKey).AuthorSummary

            txtCopyrightFile.Text = ""
            txtCopyrightSummary.Text = ""
            txtLicenseFile.Text = ""
            rtbLicenseNotice.Text = ""
        Else
            txtItemLabel.Text = SelectedNodeText
            txtItemFileName.Text = SelectedNodeKey

            txtItemDescription.Text = ""
            txtItemCreationDate.Text = ""
            txtItemLastEditDate.Text = ""
            txtAuthorFile.Text = ""
            txtAuthorSummary.Text = ""

            txtCopyrightFile.Text = ""
            txtCopyrightSummary.Text = ""
            txtLicenseFile.Text = ""
            rtbLicenseNotice.Text = ""
        End If

    End Sub

    Private Sub trvLibrary_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles trvLibrary.AfterSelect
        txtSelectedNodePath.Text = e.Node.FullPath
        'txtNodeKey.Text = e.Node.Name
        SelectedNodeKey = e.Node.Name
        'txtNodeText.Text = e.Node.Text
        SelectedNodeText = e.Node.Text
        'txtCurrentNodeKey.Text = e.Node.Name 'This is also the file name for Document items. Collection items do not have file names. 
        'Document item types:
        '  .Code - Source code document. The .Code file contains the Update/Version structure. Each document version is stored in a .rtf file.
        '  .Form - Application form design.
        '  .Pict - Picture
        '
        '
        'Collection item types:
        '  .Coll - Collection - a collection of items of any type.
        '  .Book - Book - a collection of book sections.
        '  .Appl - Application a collection of Code sections and Form sections.
        '  .Pics - Picture album - a collection of pictures.
        '

        'txtCurrentNodeText.Text = e.Node.Text
        If e.Node.Parent Is Nothing Then
            txtParentNodeName.Text = ""
        Else
            txtParentNodeName.Text = e.Node.Parent.Name
        End If
        txtNodeNumber.Text = e.Node.Index
        trvLibrary.SelectedNode = e.Node 'This code selects the node that has just been clicked (including right-click)
        'DisplaySelectedItemInfo()
        DisplaySelectedItem()
        'DisplayDefaultDocument()
    End Sub

    'NOTE: THis code appears to just repeat the After Select code.
    'Private Sub trvLibrary_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles trvLibrary.NodeMouseClick

    '    'NOTE: This code allows for the case where the node is right-clicked and View document updates and versions selected.
    '    '      This code sets the correct value of txtNodeKey.Text used to view the document.

    '    txtSelectedNodePath.Text = e.Node.FullPath
    '    txtNodeKey.Text = e.Node.Name
    '    txtNodeText.Text = e.Node.Text
    '    txtCurrentNodeKey.Text = e.Node.Name
    '    txtCurrentNodeText.Text = e.Node.Text
    '    If e.Node.Parent Is Nothing Then
    '        txtParentNodeName.Text = ""
    '    Else
    '        txtParentNodeName.Text = e.Node.Parent.Name
    '    End If
    '    txtNodeNumber.Text = e.Node.Index

    '    trvLibrary.SelectedNode = e.Node 'This code selects the node that has just been clicked (including right-click)

    '    DisplaySelectedItemInfo()
    '    'DisplayDefaultDocument()

    'End Sub

    Public Sub SaveLibrary(ByVal FileName As String)
        'Save the TreeView library structure in an XML file with the specified file name.

        If FileName = "" Then
            Message.AddWarning("File Name is blank." & vbCrLf)
            Exit Sub
        End If

        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Library Information"))

        Dim myLibrary As New XElement("Library")

        SaveLibNode(myLibrary, "", trvLibrary.Nodes)

        'Save the list of deleted nodes:
        myLibrary.Add(New XComment(""))
        Dim DeletedNodes As New XElement("DeletedNodes")
        For Each item In dgvDeletedItems.Rows
            Dim Node As New XElement("Node")
            Dim nodeText As New XElement("Text", item.Cells(0).Value)
            Node.Add(nodeText)
            Dim nodeKey As New XElement("Key", item.Cells(1).Value)
            Node.Add(nodeKey)
            Dim nodeIndex As New XElement("Index", item.Cells(3).Value)
            Node.Add(nodeIndex)
            Dim nodeParent As New XElement("Parent", item.Cells(2).Value)
            Node.Add(nodeParent)
            Dim delDate As New XElement("DeletedDate", item.Cells(4).Value)
            Node.Add(delDate)
            DeletedNodes.Add(Node)
        Next
        myLibrary.Add(DeletedNodes)

        'Save the Document Display Info dictionary: (This is used to display the document structure forms and the Utility document forms.)
        myLibrary.Add(New XComment(""))
        Dim DocDisplay As New XElement("DocumentDisplayInfo")
        For Each Item In DocDisplayInfo
            Dim Settings As New XElement("Settings")
            Dim DocumentName As New XElement("DocumentName", Item.Key)
            Settings.Add(DocumentName)
            Dim Left As New XElement("Left", Item.Value.Left)
            Settings.Add(Left)
            Dim Top As New XElement("Top", Item.Value.Top)
            Settings.Add(Top)
            Dim Width As New XElement("Width", Item.Value.Width)
            Settings.Add(Width)
            Dim Height As New XElement("Height", Item.Value.Height)
            Settings.Add(Height)
            DocDisplay.Add(Settings)
        Next
        myLibrary.Add(DocDisplay)

        ''Save the Utility Document Display Info Dictionary: (This is used to display the utlity document form.)
        'myLibrary.Add(New XComment(""))
        'Dim UtilDocDisplay As New XElement("UtilityDocumentDisplayInfo")
        'For Each Item In UtilDisplayInfo
        '    Dim Settings As New XElement("Settings")
        '    Dim DocumentName As New XElement("DocumentName", Item.Key)
        '    Settings.Add(DocumentName)
        '    Dim Left As New XElement("Left", Item.Value.Left)
        '    Settings.Add(Left)
        '    Dim Top As New XElement("Top", Item.Value.Top)
        '    Settings.Add(Top)
        '    Dim Width As New XElement("Width", Item.Value.Width)
        '    Settings.Add(Width)
        '    Dim Height As New XElement("Height", Item.Value.Height)
        '    Settings.Add(Height)
        '    UtilDocDisplay.Add(Settings)
        'Next
        'myLibrary.Add(UtilDocDisplay)

        'Save the list of default documents:
        myLibrary.Add(New XComment(""))
        Dim DefaultDocs As New XElement("DefaultDocuments")
        For Each Item In DefaultDocInfo
            Dim Settings As New XElement("Settings")
            Dim DocumentStructure As New XElement("DocumentStructure", Item.Key)
            Settings.Add(DocumentStructure)
            Dim DefaultFileName As New XElement("FileName", Item.Value.FileName)
            Settings.Add(DefaultFileName)
            Dim Label As New XElement("Label", Item.Value.Label)
            Settings.Add(Label)
            Dim Description As New XElement("Description", Item.Value.Description)
            Settings.Add(Description)
            Dim CreationDate As New XElement("CreationDate", Item.Value.CreationDate)
            Settings.Add(CreationDate)
            Dim LastEditDate As New XElement("LastEditDate", Item.Value.LastEditDate)
            Settings.Add(LastEditDate)
            Dim AuthorFile As New XElement("AuthorFile", Item.Value.AuthorFile)
            Settings.Add(AuthorFile)
            Dim AuthorSummary As New XElement("AuthorSummary", Item.Value.AuthorSummary)
            Settings.Add(AuthorSummary)
            Dim CopyrightFile As New XElement("CopyrightFile", Item.Value.CopyrightFile)
            Settings.Add(CopyrightFile)
            Dim CopyrightSummary As New XElement("CopyrightSummary", Item.Value.CopyrightSummary)
            Settings.Add(CopyrightSummary)
            Dim LicenseFile As New XElement("LicenseFile", Item.Value.LicenseFile)
            Settings.Add(LicenseFile)
            Dim LicenseSummary As New XElement("LicenseSummary", Item.Value.LicenseSummary)
            Settings.Add(LicenseSummary)
            DefaultDocs.Add(Settings)
        Next
        myLibrary.Add(DefaultDocs)

        'Save the list of Collections:
        myLibrary.Add(New XComment(""))
        Dim Collections As New XElement("CollectionInfo")
        For Each Item In CollectionInfo
            Dim Settings As New XElement("Settings")
            Dim CollName As New XElement("Name", Item.Key) 'This is the same as the file name. (Collection extensions can be .Coll, .Pics, 
            Settings.Add(CollName)
            'Dim CollLabel As New XElement("Label", Item.Value.Label)
            'Settings.Add(CollLabel)
            Dim CollDescription As New XElement("Description", Item.Value.Description)
            Settings.Add(CollDescription)
            Dim CollCreationDate As New XElement("CreationDate", Item.Value.CreationDate)
            Settings.Add(CollCreationDate)
            Dim CollLastEditDate As New XElement("LastEditDate", Item.Value.LastEditDate)
            Settings.Add(CollLastEditDate)
            Dim CollDefaultAuthorFile As New XElement("DefaultAuthorFile", Item.Value.DefaultAuthorFile)
            Settings.Add(CollDefaultAuthorFile)
            Dim CollDefaultAuthorSummary As New XElement("DefaultAuthorSummary", Item.Value.DefaultAuthorSummary)
            Settings.Add(CollDefaultAuthorSummary)
            Dim CollDefaultCopyrightFile As New XElement("DefaultCopyrightFile", Item.Value.DefaultCopyrightFile)
            Settings.Add(CollDefaultCopyrightFile)
            Dim CollDefaultCopyrightSummary As New XElement("DefaultCopyrightSummary", Item.Value.DefaultCopyrightSummary)
            Settings.Add(CollDefaultCopyrightSummary)
            Dim CollDefaultLicenseFile As New XElement("DefaultLicenseFile", Item.Value.DefaultLicenseFile)
            Settings.Add(CollDefaultLicenseFile)
            Dim CollDefaultLicenseSummary As New XElement("DefaultLicenseSummary", Item.Value.DefaultLicenseSummary)
            Settings.Add(CollDefaultLicenseSummary)
            Collections.Add(Settings)
        Next
        myLibrary.Add(Collections)

        'Save the list of Utility Documents 
        myLibrary.Add(New XComment(""))
        Dim Utilities As New XElement("UtilityDocumentInfo")
        For Each Item In UtilDocInfo
            Dim Settings As New XElement("Settings")
            Dim UtilName As New XElement("Name", Item.Key) 'This is the same as the file name. (Utility Document extensions can be .Note, .Checklist, 
            Settings.Add(UtilName)
            Dim UtilDescription As New XElement("Description", Item.Value.Description)
            Settings.Add(UtilDescription)
            Dim UtilCreationDate As New XElement("CreationDate", Item.Value.CreationDate)
            Settings.Add(UtilCreationDate)
            Dim UtilLastEditDate As New XElement("LastEditDate", Item.Value.LastEditDate)
            Settings.Add(UtilLastEditDate)
            Dim UtilAuthorFile As New XElement("AuthorFile", Item.Value.AuthorFile)
            Settings.Add(UtilAuthorFile)
            Dim UtilAuthorSummary As New XElement("AuthorSummary", Item.Value.AuthorSummary)
            Settings.Add(UtilAuthorSummary)
            Utilities.Add(Settings)
        Next
        myLibrary.Add(Utilities)

        XDoc.Add(myLibrary)

        Project.SaveXmlData(FileName, XDoc)

    End Sub

    Private Sub SaveLibNode(ByRef myElement As XElement, Parent As String, ByRef tnc As TreeNodeCollection)
        'Save the nodes in the TreeNodeCollection in the XElement
        'This method calls itself recursively to save all nodes in trvLibrary.

        Dim I As Integer

        If tnc.Count = 0 Then 'Leaf
        Else
            For I = 0 To tnc.Count - 1
                Dim myNode As New XElement(tnc(I).Name)

                Dim myNodeText As New XElement("Text", tnc(I).Text)
                myNode.Add(myNodeText)

                If tnc(I).Name.EndsWith(".Libr") Then 'This the root node containing information about the Library.
                    'Note: The LibraryName property is the same as the root node Text.
                    'Note: The LibraryFileName property is the same as the root node Name (or Key).
                    Dim myLibDescr As New XElement("Description", txtLibraryDescription.Text)
                    myNode.Add(myLibDescr)
                    Dim myLibCreationDate As New XElement("CreationDate", Format(LibraryCreationDate, "d-MMM-yyyy H:mm:ss"))
                    myNode.Add(myLibCreationDate)
                    Dim myLibLastEditDate As New XElement("LastEditDate", Format(LibraryLastEditDate, "d-MMM-yyyy H:mm:ss"))
                    myNode.Add(myLibLastEditDate)
                    Dim myLibDefaultAuthorFile As New XElement("DefaultAuthorFile", DefaultAuthorFile)
                    myNode.Add(myLibDefaultAuthorFile)
                    Dim myLibDefaultAuthorSummary As New XElement("DefaultAuthorSummary", DefaultAuthorSummary)
                    myNode.Add(myLibDefaultAuthorSummary)
                    Dim myLibDefaultCopyrightFile As New XElement("DefaultCopyrightFile", DefaultCopyrightFile)
                    myNode.Add(myLibDefaultCopyrightFile)
                    Dim myLibDefaultCopyrightSummary As New XElement("DefaultCopyrightSummary", DefaultCopyrightSummary)
                    myNode.Add(myLibDefaultCopyrightSummary)
                    Dim myLibDefaultLicenseFile As New XElement("DefaultLicenseFile", DefaultLicenseFile)
                    myNode.Add(myLibDefaultLicenseFile)
                    Dim myLibDefaultLicenseSummary As New XElement("DefaultLicenseSummary", DefaultLicenseSummary)
                    myNode.Add(myLibDefaultLicenseSummary)
                End If

                SaveLibNode(myNode, tnc(I).Name, tnc(I).Nodes)
                myElement.Add(myNode)
            Next
        End If

    End Sub

    Private Sub OpenLibraryXDoc(ByVal myXDoc As XDocument)
        'Open the Library stored in the XDocument

        trvLibrary.Nodes.Clear() 'Clear the nodes in the Library TreeView
        DocDisplayInfo.Clear() 'Clear the document display info dictionary.
        DefaultDocInfo.Clear() 'Clear the Default document info dictionary.
        dgvDeletedItems.Rows.Clear()
        'dgvDeletedItems.Columns.Clear()

        Dim I As Integer

        'Need to convert the XDocument to an XmlDocument:
        Dim XDoc As New System.Xml.XmlDocument

        XDoc.LoadXml(myXDoc.ToString)

        Dim node As System.Xml.XmlNode
        node = XDoc.DocumentElement

        'Message.Add("Start: Read the library file: " & Trim(txtLibraryFileName.Text) & vbCrLf)

        ProcessLibChildNode(XDoc.DocumentElement, trvLibrary.Nodes, "")
    End Sub

    Private Sub OpenLibrary(ByVal FileName As String)
        'Open the library with the specified file name.
        LibraryFileName = FileName
        Dim XDocLib As XDocument
        Project.ReadXmlData(FileName, XDocLib)
        OpenLibraryXDoc(XDocLib)

    End Sub

    Private Sub ProcessLibChildNode(ByVal xml_Node As System.Xml.XmlNode, ByVal tnc As TreeNodeCollection, ByVal Spaces As String)
        'Opening the .Libr library defintion file. Process the Child Nodes.
        'This subroutine calls itself to process the child node branches.

        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            'Message.Add("TestProcessLibChildNode --------------------------------" & vbCrLf)
            ' Message.Add("ChildNode.Name " & ChildNode.Name & vbCrLf) 'eg My_Library.Libr
            Dim myName As System.Xml.XmlNode
            myName = ChildNode.SelectSingleNode("Text")
            If IsNothing(myName) Then
                'Message.Add("/Text node not found. " & vbCrLf)
                If ChildNode.Name = "DeletedNodes" Then
                    ProcessDeletedNodesList(ChildNode)
                ElseIf ChildNode.Name = "DocumentDisplayInfo" Then
                    ProcessDocDisplayInfoList(ChildNode)
                    'ElseIf ChildNode.Name = "DefaultDocumentInfo" Then
                    'ElseIf ChildNode.Name = "UtilityDocumentDisplayInfo" Then
                    '    ProcessUtilDocDisplayInfoList(ChildNode)
                ElseIf ChildNode.Name = "DefaultDocuments" Then
                    ProcessDefaultDocInfoList(ChildNode)
                ElseIf ChildNode.Name = "CollectionInfo" Then
                    ProcessCollectionInfoList(ChildNode)
                ElseIf ChildNode.Name = "UtilityDocumentInfo" Then
                    ProcessUtilityDocInfoList(ChildNode)
                End If
            Else
                Dim myNodeName As String = myName.InnerText 'eg My Library
                If ChildNode.Name.EndsWith(".Libr") Then
                    'Read Library description:
                    Dim myDescr As System.Xml.XmlNode
                    myDescr = ChildNode.SelectSingleNode("Description")
                    If IsNothing(myDescr) Then
                        'txtLibraryDescription.Text = ""
                        LibraryDescription = ""
                    Else
                        'txtLibraryDescription.Text = myDescr.InnerText
                        LibraryDescription = myDescr.InnerText
                    End If
                    'Read Library creation date:
                    Dim myCreationDate As System.Xml.XmlNode
                    myCreationDate = ChildNode.SelectSingleNode("CreationDate")
                    If myCreationDate Is Nothing Then
                        LibraryCreationDate = Now
                    Else
                        LibraryCreationDate = myCreationDate.InnerText
                    End If
                    'Read Library last edit date:
                    Dim myLastEditDate As System.Xml.XmlNode
                    myLastEditDate = ChildNode.SelectSingleNode("LastEditDate")
                    If myLastEditDate Is Nothing Then
                        LibraryLastEditDate = Now
                    Else
                        LibraryLastEditDate = myLastEditDate.InnerText
                    End If
                    'Read Library default author file:
                    Dim myDefaultAuthorFile As System.Xml.XmlNode
                    myDefaultAuthorFile = ChildNode.SelectSingleNode("DefaultAuthorFile")
                    If myDefaultAuthorFile Is Nothing Then
                        DefaultAuthorFile = ""
                    Else
                        DefaultAuthorFile = myDefaultAuthorFile.InnerText
                    End If
                    'Read Library default author summary:
                    Dim myDefaultAuthorSummary As System.Xml.XmlNode
                    myDefaultAuthorSummary = ChildNode.SelectSingleNode("DefaultAuthorSummary")
                    If myDefaultAuthorSummary Is Nothing Then
                        DefaultAuthorSummary = ""
                    Else
                        DefaultAuthorSummary = myDefaultAuthorSummary.InnerText
                    End If
                    'Read Library default copyright file:
                    Dim myDefaultCopyrightFile As System.Xml.XmlNode
                    myDefaultCopyrightFile = ChildNode.SelectSingleNode("DefaultCopyrightFile")
                    If myDefaultCopyrightFile Is Nothing Then
                        DefaultCopyrightFile = ""
                    Else
                        DefaultCopyrightFile = myDefaultCopyrightFile.InnerText
                    End If
                    'Read Library default copyright summary:
                    Dim myDefaultCopyrightSummary As System.Xml.XmlNode
                    myDefaultCopyrightSummary = ChildNode.SelectSingleNode("DefaultCopyrightSummary")
                    If myDefaultCopyrightSummary Is Nothing Then
                        DefaultCopyrightSummary = ""
                    Else
                        DefaultCopyrightSummary = myDefaultCopyrightSummary.InnerText
                    End If
                    'Read Library default license file:
                    Dim myDefaultLicenseFile As System.Xml.XmlNode
                    myDefaultLicenseFile = ChildNode.SelectSingleNode("DefaultLicenseFile")
                    If myDefaultLicenseFile Is Nothing Then
                        DefaultLicenseFile = ""
                    Else
                        DefaultLicenseFile = myDefaultLicenseFile.InnerText
                    End If
                    'Read Library default license summary:
                    Dim myDefaultLicenseSummary As System.Xml.XmlNode
                    myDefaultLicenseSummary = ChildNode.SelectSingleNode("DefaultLicenseSummary")
                    If myDefaultLicenseSummary Is Nothing Then
                        DefaultLicenseSummary = ""
                    Else
                        DefaultLicenseSummary = myDefaultLicenseSummary.InnerText
                    End If

                    'txtLibraryName.Text = myNodeName 'The Library Name label displayed on the Library tab.
                    'txtLibraryName2.Text = myNodeName 'The Library Name label displayed on the main form.
                    LibraryName = myNodeName
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 0, 1)
                    new_Node.EnsureVisible()

                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)

                ElseIf ChildNode.Name.EndsWith(".Coll") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 2, 3)
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Book") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 4, 5) 'Key, Text, ImageIndex, SelectedImageIndex
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Sect") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 6, 7) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Appl") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 8, 9) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Code") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 10, 11) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    new_Node.ContextMenuStrip = ContextMenuStrip1
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Form") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 12, 13) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Pics") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 14, 15) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Pict") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 16, 17) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Note") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 26, 27) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    new_Node.ContextMenuStrip = ContextMenuStrip2
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                ElseIf ChildNode.Name.EndsWith(".Proc") Then
                    Dim new_Node As TreeNode = tnc.Add(ChildNode.Name, myNodeName, 32, 33) 'Add a node to the tree node collection: Key, Text, ImageIndex, SelectedImageIndex.
                    new_Node.EnsureVisible()
                    ProcessLibChildNode(ChildNode, new_Node.Nodes, Spaces)
                Else
                    Message.AddWarning("Child node type unknown: " & ChildNode.Name & vbCrLf)
                    Beep()
                    'ElseIf ChildNode.Name = "DeletedNodes" Then
                    'ProcessDeletedNodesList(ChildNode)
                End If
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

            Else
                'Unknown Node name.
                Message.AddWarning("Unknown DeletedNodes node name: " & ChildNode.Name & vbCrLf)
            End If
        Next

        dgvDeletedItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvDeletedItems.AutoResizeColumns()

    End Sub

    Private Sub ProcessDocDisplayInfoList(ByVal xml_Node As System.Xml.XmlNode)
        'Process each of the Document Display Information records.
        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            If ChildNode.Name = "Settings" Then
                Dim nodeDocName As System.Xml.XmlNode
                nodeDocName = ChildNode.SelectSingleNode("DocumentName")
                Dim nodeLeft As System.Xml.XmlNode
                nodeLeft = ChildNode.SelectSingleNode("Left")
                Dim nodeTop As System.Xml.XmlNode
                nodeTop = ChildNode.SelectSingleNode("Top")
                Dim nodeWidth As System.Xml.XmlNode
                nodeWidth = ChildNode.SelectSingleNode("Width")
                Dim nodeHeight As System.Xml.XmlNode
                nodeHeight = ChildNode.SelectSingleNode("Height")
                DocDisplayInfo.Add(nodeDocName.InnerText, New DocumentInfo)
                DocDisplayInfo(nodeDocName.InnerText).Left = nodeLeft.InnerText
                DocDisplayInfo(nodeDocName.InnerText).Top = nodeTop.InnerText
                DocDisplayInfo(nodeDocName.InnerText).Width = nodeWidth.InnerText
                DocDisplayInfo(nodeDocName.InnerText).Height = nodeHeight.InnerText
            End If
        Next

    End Sub

    Private Sub ProcessDefaultDocInfoList(ByVal xml_Node As System.Xml.XmlNode)
        'Process each of the Default Document information records.
        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            If ChildNode.Name = "Settings" Then
                Dim nodeDocStructure As System.Xml.XmlNode
                nodeDocStructure = ChildNode.SelectSingleNode("DocumentStructure")
                Dim nodeDefaultDocFileName As System.Xml.XmlNode
                nodeDefaultDocFileName = ChildNode.SelectSingleNode("FileName")
                Dim nodeDefaultDocLabel As System.Xml.XmlNode
                nodeDefaultDocLabel = ChildNode.SelectSingleNode("Label")
                Dim nodeDefaultDocDescription As System.Xml.XmlNode
                nodeDefaultDocDescription = ChildNode.SelectSingleNode("Description")
                Dim nodeDefaultDocCreationDate As System.Xml.XmlNode
                nodeDefaultDocCreationDate = ChildNode.SelectSingleNode("CreationDate")
                Dim nodeDefaultDocLastEditDate As System.Xml.XmlNode
                nodeDefaultDocLastEditDate = ChildNode.SelectSingleNode("LastEditDate")
                Dim nodeDefaultDocAuthorFile As System.Xml.XmlNode
                nodeDefaultDocAuthorFile = ChildNode.SelectSingleNode("AuthorFile")
                Dim nodeDefaultDocAuthorSummary As System.Xml.XmlNode
                nodeDefaultDocAuthorSummary = ChildNode.SelectSingleNode("AuthorSummary")
                Dim nodeDefaultDocCopyrightFile As System.Xml.XmlNode
                nodeDefaultDocCopyrightFile = ChildNode.SelectSingleNode("CopyrightFile")
                Dim nodeDefaultDocCopyrightSummary As System.Xml.XmlNode
                nodeDefaultDocCopyrightSummary = ChildNode.SelectSingleNode("CopyrightSummary")
                Dim nodeDefaultDocLicenseFile As System.Xml.XmlNode
                nodeDefaultDocLicenseFile = ChildNode.SelectSingleNode("LicenseFile")
                Dim nodeDefaultDocLicenseSummary As System.Xml.XmlNode
                nodeDefaultDocLicenseSummary = ChildNode.SelectSingleNode("LicenseSummary")

                'DefaultDocInfo.Add(nodeDocStructure.InnerText, New ADVL_Code_Library_1.DefaultDocInfo)
                DefaultDocInfo.Add(nodeDocStructure.InnerText, New ADVL_Information_Library_1.DefaultDocInfo)
                DefaultDocInfo(nodeDocStructure.InnerText).FileName = nodeDefaultDocFileName.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).Label = nodeDefaultDocLabel.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).Description = nodeDefaultDocDescription.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).CreationDate = nodeDefaultDocCreationDate.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).LastEditDate = nodeDefaultDocLastEditDate.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).AuthorFile = nodeDefaultDocAuthorFile.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).AuthorSummary = nodeDefaultDocAuthorSummary.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).CopyrightFile = nodeDefaultDocCopyrightFile.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).CopyrightSummary = nodeDefaultDocCopyrightSummary.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).LicenseFile = nodeDefaultDocLicenseFile.InnerText
                DefaultDocInfo(nodeDocStructure.InnerText).LicenseSummary = nodeDefaultDocLicenseSummary.InnerText
            End If
        Next
    End Sub

    Private Sub ProcessCollectionInfoList(ByVal xml_Node As System.Xml.XmlNode)
        'Process each of the Collection Information records.

        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            If ChildNode.Name = "Settings" Then
                Dim nodeCollName As System.Xml.XmlNode
                nodeCollName = ChildNode.SelectSingleNode("Name")
                'Dim nodeCollLabel As System.Xml.XmlNode
                'nodeCollLabel = ChildNode.SelectSingleNode("Label")
                Dim nodeCollDescription As System.Xml.XmlNode
                nodeCollDescription = ChildNode.SelectSingleNode("Description")
                Dim nodeCollCreationDate As System.Xml.XmlNode
                nodeCollCreationDate = ChildNode.SelectSingleNode("CreationDate")
                Dim nodeCollLastEditDate As System.Xml.XmlNode
                nodeCollLastEditDate = ChildNode.SelectSingleNode("LastEditDate")
                Dim nodeCollAuthorFile As System.Xml.XmlNode
                nodeCollAuthorFile = ChildNode.SelectSingleNode("DefaultAuthorFile")
                Dim nodeCollAuthorSummary As System.Xml.XmlNode
                nodeCollAuthorSummary = ChildNode.SelectSingleNode("DefaultAuthorSummary")
                Dim nodeCollCopyrightFile As System.Xml.XmlNode
                nodeCollCopyrightFile = ChildNode.SelectSingleNode("DefaultCopyrightFile")
                Dim nodeCollCopyrightSummary As System.Xml.XmlNode
                nodeCollCopyrightSummary = ChildNode.SelectSingleNode("DefaultCopyrightSummary")
                Dim nodeCollLicenseFile As System.Xml.XmlNode
                nodeCollLicenseFile = ChildNode.SelectSingleNode("DefaultLicenseFile")
                Dim nodeCollLicenseSummary As System.Xml.XmlNode
                nodeCollLicenseSummary = ChildNode.SelectSingleNode("DefaultLicenseSummary")

                'CollectionInfo.Add(nodeCollName.InnerText, New ADVL_Code_Library_1.CollectionInfo)
                CollectionInfo.Add(nodeCollName.InnerText, New ADVL_Information_Library_1.CollectionInfo)
                'CollectionInfo(nodeCollName.InnerText).Label = nodeCollLabel.InnerText
                CollectionInfo(nodeCollName.InnerText).Description = nodeCollDescription.InnerText
                CollectionInfo(nodeCollName.InnerText).CreationDate = nodeCollCreationDate.InnerText
                CollectionInfo(nodeCollName.InnerText).LastEditDate = nodeCollLastEditDate.InnerText
                CollectionInfo(nodeCollName.InnerText).DefaultAuthorFile = nodeCollAuthorFile.InnerText
                CollectionInfo(nodeCollName.InnerText).DefaultAuthorSummary = nodeCollAuthorSummary.InnerText
                CollectionInfo(nodeCollName.InnerText).DefaultCopyrightFile = nodeCollCopyrightFile.InnerText
                CollectionInfo(nodeCollName.InnerText).DefaultCopyrightSummary = nodeCollCopyrightSummary.InnerText
                CollectionInfo(nodeCollName.InnerText).DefaultLicenseFile = nodeCollLicenseFile.InnerText
                CollectionInfo(nodeCollName.InnerText).DefaultLicenseSummary = nodeCollLicenseSummary.InnerText

            End If
        Next

    End Sub

    Private Sub ProcessUtilityDocInfoList(ByVal xml_Node As System.Xml.XmlNode)
        'Process each of the Utility Document Information records.

        For Each ChildNode As System.Xml.XmlNode In xml_Node.ChildNodes
            If ChildNode.Name = "Settings" Then
                Dim nodeUtilName As System.Xml.XmlNode
                nodeUtilName = ChildNode.SelectSingleNode("Name")
                Dim nodeUtilDescription As System.Xml.XmlNode
                nodeUtilDescription = ChildNode.SelectSingleNode("Description")
                Dim nodeUtilCreationDate As System.Xml.XmlNode
                nodeUtilCreationDate = ChildNode.SelectSingleNode("CreationDate")
                Dim nodeUtilLastEditDate As System.Xml.XmlNode
                nodeUtilLastEditDate = ChildNode.SelectSingleNode("LastEditDate")
                Dim nodeUtilAuthorFile As System.Xml.XmlNode
                nodeUtilAuthorFile = ChildNode.SelectSingleNode("AuthorFile")
                Dim nodeUtilAuthorSummary As System.Xml.XmlNode
                nodeUtilAuthorSummary = ChildNode.SelectSingleNode("AuthorSummary")

                UtilDocInfo.Add(nodeUtilName.InnerText, New DocItemInfo)
                UtilDocInfo(nodeUtilName.InnerText).Description = nodeUtilDescription.InnerText
                UtilDocInfo(nodeUtilName.InnerText).CreationDate = nodeUtilCreationDate.InnerText
                UtilDocInfo(nodeUtilName.InnerText).LastEditDate = nodeUtilLastEditDate.InnerText
                UtilDocInfo(nodeUtilName.InnerText).AuthorFile = nodeUtilAuthorFile.InnerText
                UtilDocInfo(nodeUtilName.InnerText).AuthorSummary = nodeUtilAuthorSummary.InnerText

            End If
        Next

    End Sub

    Private Sub trvLibrary_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles trvLibrary.AfterExpand
        If e.Node.Name.EndsWith(".Libr") Then
            e.Node.ImageIndex = 1
        ElseIf e.Node.Name.EndsWith(".Coll") Then
            e.Node.ImageIndex = 3
        ElseIf e.Node.Name.EndsWith(".Book") Then
            e.Node.ImageIndex = 5
        ElseIf e.Node.Name.EndsWith(".Sect") Then

        End If
    End Sub

    Private Sub trvLibrary_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles trvLibrary.AfterCollapse
        If e.Node.Name.EndsWith(".Libr") Then
            e.Node.ImageIndex = 0
        ElseIf e.Node.Name.EndsWith(".Coll") Then
            e.Node.ImageIndex = 2
        ElseIf e.Node.Name.EndsWith(".Book") Then
            e.Node.ImageIndex = 4
        ElseIf e.Node.Name.EndsWith(".Sect") Then

        End If
    End Sub


#Region "Node Information - Information about the selected node in the Information Tree" '=====================================================================================================

    Private Sub btnOpenDocStruc_Click(sender As Object, e As EventArgs) Handles btnOpenDocument.Click
        'Open the specified document structure

        If SelectedNodeKey.EndsWith(".Code") Then
            'OpenDocumentStructure()
            OpenDocumentStructure(SelectedNodeKey)
        ElseIf SelectedNodeKey.EndsWith(".Coll") Then
            Message.Add("Collection node selected." & vbCrLf)
        ElseIf SelectedNodeKey.EndsWith(".Note") Then
            Message.Add("Note node selected." & vbCrLf)
            'OpenUtilityDocument()
            OpenUtilityDocument(SelectedNodeKey)
        End If
    End Sub

    'Private Sub OpenDocumentStructure()
    Private Sub OpenDocumentStructure(ByVal FileName As String)
        'Open the document structure, which contains the document updates and versions.

        'If CodeViewList.Count > 0 Then
        If DocViewList.Count > 0 Then
            Dim I As Integer
            'First check if the DocStrucuture is already displayed:
            For I = 0 To DocViewList.Count - 1
                If IsNothing(DocViewList(I)) Then

                Else
                    If DocViewList(I).DocStructure = txtNodeKey.Text Then
                        DocViewList(I).BringToFront
                        Exit Sub
                    End If
                End If
            Next
            'Check if there is a free element in CodeViewList() to use to display the DocStructure:
            For I = 0 To DocViewList.Count - 1
                If IsNothing(DocViewList(I)) Then
                    'CodeView = New frmDocumentView
                    DocView = New frmDocumentView
                    DocViewList(I) = DocView
                    DocViewList(I).FormNo = I
                    DocViewList(I).Show
                    'Message.Add("Opening document structure with filename: " & txtNodeKey.Text & vbCrLf)
                    Message.Add("Opening document structure with filename: " & FileName & vbCrLf)
                    'DocViewList(I).DocStructure = txtNodeKey.Text
                    DocViewList(I).DocStructure = FileName
                    DocViewList(I).SelectCurrentNode
                    Exit Sub
                End If
            Next
            'If this point is reached, there are no unused CodeViewList entries.
            'Add a new CodeView:
            DocView = New frmDocumentView
            Dim EntryNo As Integer = DocViewList.Count
            DocViewList.Add(DocView)
            DocViewList(EntryNo).FormNo = EntryNo
            DocViewList(EntryNo).Show
            'DocViewList(EntryNo).DocStructure = txtNodeKey.Text
            DocViewList(EntryNo).DocStructure = FileName
            DocViewList(EntryNo).SelectCurrentNode
        Else
            DocView = New frmDocumentView
            DocViewList.Add(DocView)
            DocViewList(0).FormNo = 0
            DocViewList(0).Show
            'DocViewList(0).DocStructure = txtNodeKey.Text
            DocViewList(0).DocStructure = FileName
            DocViewList(0).SelectCurrentNode
        End If

    End Sub

    'Private Sub OpenUtilityDocument()
    Private Sub OpenUtilityDocument(ByVal FileName As String)
        'Open the Utility Document.
        'If DocViewList.Count > 0 Then
        If UtilViewList.Count > 0 Then
            Dim I As Integer
            'First check if the Utility Document is already displayed:
            For I = 0 To UtilViewList.Count - 1
                If IsNothing(UtilViewList(I)) Then

                Else
                    'If UtilViewList(I).DocFileName = txtNodeKey.Text Then
                    'If UtilViewList(I).DocFileName = SelectedNodeKey Then
                    If UtilViewList(I).DocFileName = FileName Then
                        UtilViewList(I).BringToFront
                        Exit Sub
                    End If
                End If
            Next
            'Check if there is a free element in UtilViewList() to use to display the Utility Document:
            For I = 0 To UtilViewList.Count - 1
                If IsNothing(UtilViewList(I)) Then
                    'CodeView = New frmDocumentView
                    UtilView = New frmUtilityDocView
                    UtilViewList(I) = UtilView
                    UtilViewList(I).FormNo = I
                    UtilViewList(I).Show
                    'Message.Add("Opening document structure with filename: " & txtNodeKey.Text & vbCrLf)
                    'Message.Add("Opening document structure with filename: " & SelectedNodeKey & vbCrLf)
                    Message.Add("Opening document structure with filename: " & FileName & vbCrLf)
                    'UtilViewList(I).DocLabel = SelectedNodeText
                    UtilViewList(I).DocLabel = HoverNodeText
                    'UtilViewList(I).DocFileName = txtNodeKey.Text
                    'UtilViewList(I).DocFileName = SelectedNodeKey
                    UtilViewList(I).DocFileName = FileName
                    'UtilViewList(I).SelectCurrentNode
                    Exit Sub
                End If
            Next
            'If this point is reached, there are no unused UtilViewList entries.
            'Add a new UtilView:
            'DocView = New frmDocumentView
            UtilView = New frmUtilityDocView
            Dim EntryNo As Integer = UtilViewList.Count
            UtilViewList.Add(UtilView)
            UtilViewList(EntryNo).FormNo = EntryNo
            UtilViewList(EntryNo).Show
            'UtilViewList(EntryNo).DocLabel = SelectedNodeText
            UtilViewList(EntryNo).DocLabel = HoverNodeText
            'UtilViewList(EntryNo).DocStructure = txtNodeKey.Text
            'UtilViewList(EntryNo).DocStructure = SelectedNodeKey
            'UtilViewList(EntryNo).DocFileName = SelectedNodeKey
            UtilViewList(EntryNo).DocFileName = FileName
            'UtilViewList(EntryNo).SelectCurrentNode
        Else
            'DocView = New frmDocumentView
            UtilView = New frmUtilityDocView
            UtilViewList.Add(UtilView)
            UtilViewList(0).FormNo = 0
            UtilViewList(0).Show
            'UtilViewList(0).DocStructure = txtNodeKey.Text
            'UtilViewList(0).DocStructure = SelectedNodeKey
            'UtilViewList(0).DocLabel = SelectedNodeText
            UtilViewList(0).DocLabel = HoverNodeText
            'UtilViewList(0).DocFileName = SelectedNodeKey
            UtilViewList(0).DocFileName = FileName
            'UtilViewList(0).SelectCurrentNode
        End If
    End Sub

    Private Sub btnFindNodeKey_Click(sender As Object, e As EventArgs) Handles btnFindNodeKey.Click
        'Find the node with the key specified in txtNodeKeyToFind.Text

        Dim myNode() As TreeNode = trvLibrary.Nodes.Find(txtNodeKeyToFind.Text, True)

        'trvLibrary.SelectedNode = trvLibrary.Nodes.Find()
        If myNode.Length > 0 Then
            trvLibrary.SelectedNode = myNode(0)
            trvLibrary.Focus()
        Else
            Message.AddWarning("Node key not found: " & txtNodeKeyToFind.Text & vbCrLf)
        End If

    End Sub

#End Region 'Node Information -----------------------------------------------------------------------------------------------------------------------------------------------------------------


#Region "Add Item - Add an item to the Information Tree" '=====================================================================================================================================

    Private Sub btnAddLibraryItem_Click(sender As Object, e As EventArgs) Handles btnAddLibraryItem.Click
        'Add a new library item to the tree view.

        If txtNewLibraryItemFileName.Text.Contains(".") Then
            Message.AddWarning("Please enter a file name without a file extension." & vbCrLf)
            Beep()
            Exit Sub
        End If

        If trvLibrary.SelectedNode Is Nothing Then
            Select Case cmbNewLibraryItemType.SelectedItem.ToString

                Case "Collection"
                    trvLibrary.Nodes.Add(txtNewLibraryItemName.Text.Replace(" ", "_") & ".Coll", txtNewLibraryItemName.Text, 2, 3)

            End Select
        Else
            Select Case cmbNewLibraryItemType.SelectedItem.ToString
                Case "Library"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Libr"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 0, 1)
                    End If
                    'CreateLibrFile(NewFileName, txtNewLibraryItemDescription.Text)

                Case "Collection"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Coll"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 2, 3)
                    End If
                    CreateCollFile(NewFileName, txtNewLibraryItemDescription.Text)
                    'SaveLibraryFile()
                Case "Book"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Book"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 4, 5)
                    End If
                    CreateBookFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                    'SaveLibraryFile()
                Case "  Section"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Sect"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 6, 7)
                    End If
                    CreateBookSectionFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                    'SaveLibraryFile()
                Case "Application"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Appl"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 8, 9)
                    End If
                    CreateApplicationFile(NewFileName, txtNewLibraryItemDescription.Text)
                    'SaveLibraryFile()

                Case "  Code"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Code"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        Dim new_Node As TreeNode = trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 10, 11)
                        new_Node.EnsureVisible()
                        new_Node.ContextMenuStrip = ContextMenuStrip1
                        CreateApplicationCodeFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text, trvLibrary.SelectedNode.Name)
                    End If


                Case "  Form"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Form"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 12, 13)
                    End If
                    'CreateApplicationFormFile(NewFileName, txtNewLibraryItemDescription.Text)

                Case "Picture Album"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Pics"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 14, 15)
                    End If
                    CreatePicAlbumFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                Case "  Picture"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Pict"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 14, 15)
                    End If
                    CreatePictureFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                    'SaveLibraryFile()
                Case "Note"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Note"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 26, 27)
                        CopyAuthor(trvLibrary.SelectedNode.Name, NewFileName)
                    End If
                   ' CreateNoteFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text, trvLibrary.SelectedNode.Name)
                Case "Process"
                    Dim NewFileName As String = txtNewLibraryItemFileName.Text.Replace(" ", "_") & ".Proc"
                    If Project.DataFileExists(NewFileName) Then
                        Message.AddWarning("File name: " & NewFileName & " already exists!" & vbCrLf)
                    Else
                        trvLibrary.SelectedNode.Nodes.Add(NewFileName, txtNewLibraryItemName.Text, 14, 15)
                    End If
                    'CreateProcessFile(NewFileName, txtNewLibraryItemName.Text, txtNewLibraryItemDescription.Text)
                Case "Web"

                Case "  Page"


            End Select
        End If

        SaveLibrary(txtLibraryFileName.Text)

    End Sub

    Private Sub CreatePicAlbumFile(ByVal FileName As String, ByVal Name As String, ByRef Description As String)
        'Create a Picture Album file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Picture Album Node"))
        Dim PicAlbumNode As New XElement("PictureAlbum")
        Dim PicAlbumName As New XElement("Name", Name)
        PicAlbumNode.Add(PicAlbumName)
        Dim Descr As New XElement("Description", txtNewLibraryItemDescription.Text)
        PicAlbumNode.Add(Descr)
        XDoc.Add(PicAlbumNode)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CreatePictureFile(ByVal FileName As String, ByVal Name As String, ByRef Description As String)
        'Create a Picture file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Picture Node"))
        Dim Picture As New XElement("Picture") 'This will contain the picture structure.
        Dim PictureName As New XElement(FileName) 'The Picture Name is also the name of the XML file containing the structure. 
        Dim PictureText As New XElement("Text", Name) 'The Title of the document. 
        PictureName.Add(PictureText)
        Dim Descr As New XElement("Description", txtNewLibraryItemDescription.Text) 'A description of the document.
        PictureName.Add(Descr)
        Dim UpdateName As String
        UpdateName = System.IO.Path.GetFileNameWithoutExtension(FileName)
        UpdateName = UpdateName & ".Update"
        Dim DocFileName As String
        DocFileName = System.IO.Path.GetFileNameWithoutExtension(FileName)
        DocFileName = DocFileName & ".pptx"
        Dim SelectedNode As New XElement("SelectedNode", DocFileName) 'This is the Update/Version of the document that will be opened.
        PictureName.Add(SelectedNode)
        Dim InitialUpdate As New XElement(UpdateName)
        Dim InitialUpdateLabel As New XElement("Text", "Initial Document")
        InitialUpdate.Add(InitialUpdateLabel)
        Dim InitialDocument As New XElement(DocFileName)
        Dim InitialDocumentLabel As New XElement("Text", "Initial Version")
        InitialDocument.Add(InitialDocumentLabel)
        InitialUpdate.Add(InitialDocument)
        PictureName.Add(InitialUpdate)
        Picture.Add(PictureName)
        XDoc.Add(Picture) 'Additional information: This operation would create an incorrectly structured document.
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CreateCollFile(ByVal FileName As String, ByRef Description As String)
        'Create a Collection file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Collection Node"))
        Dim CollNode As New XElement("Collection")
        Dim Descr As New XElement("Description", txtNewLibraryItemDescription.Text)
        CollNode.Add(Descr)
        XDoc.Add(CollNode)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CreateBookFile(ByVal FileName As String, ByVal Name As String, ByRef Description As String)
        'Create a Book file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Book Node"))
        Dim BookNode As New XElement("Book")
        Dim BookName As New XElement("Name", Name)
        BookNode.Add(BookName)
        Dim Descr As New XElement("Description", Description)
        BookNode.Add(Descr)
        XDoc.Add(BookNode)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CreateBookSectionFile(ByVal FileName As String, ByVal Name As String, ByRef Description As String)
        'Create a Book Section file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Book Section Node"))
        Dim Section As New XElement("Section") 'This will contain the section structure.
        Dim SectionName As New XElement(FileName) 'The Section Name is also the name of the XML file containing the structure.
        Dim SectionText As New XElement("Text", Name) 'The Title of the document. 
        SectionName.Add(SectionText)
        Dim Descr As New XElement("Description", Description) 'A description of the document.
        SectionName.Add(Descr)
        Dim UpdateName As String
        UpdateName = System.IO.Path.GetFileNameWithoutExtension(FileName)
        UpdateName = UpdateName & ".Update"
        Dim DocFileName As String
        DocFileName = System.IO.Path.GetFileNameWithoutExtension(FileName)
        DocFileName = DocFileName & ".rtf"
        Dim SelectedNode As New XElement("SelectedNode", DocFileName) 'This is the Update/Version of the document that will be opened.
        SectionName.Add(SelectedNode)
        Dim InitialUpdate As New XElement(UpdateName)
        Dim InitialUpdateLabel As New XElement("Text", "Initial Document")
        InitialUpdate.Add(InitialUpdateLabel)
        Dim InitialDocument As New XElement(DocFileName)
        Dim InitialDocumentLabel As New XElement("Text", "Initial Version")
        InitialDocument.Add(InitialDocumentLabel)
        InitialUpdate.Add(InitialDocument)
        SectionName.Add(InitialUpdate)
        Section.Add(SectionName)
        XDoc.Add(Section) 'Additional information: This operation would create an incorrectly structured document.
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CreateApplicationFile(ByVal FileName As String, ByRef Description As String)
        'Create an Application file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Application Node"))
        Dim Descr As New XElement("Description", Description)
        XDoc.Add(Descr)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CreateApplicationCodeFile(ByVal FileName As String, ByVal Name As String, ByRef Description As String, ByVal ParentFileName As String)
        'Create an Application Source file with the specified file name and containing the specified description entry.
        'The file name will have hte extension .Code

        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Document Structure Definition"))

        Dim CodeName As New XElement(FileName) 'The CodeName is also the name of the XML file containing the document update/version structure.

        Dim CodeText As New XElement("Text", Name) 'The Title of the document.
        CodeName.Add(CodeText)
        Dim Descr As New XElement("Description", Description) 'A description of the document.
        CodeName.Add(Descr)
        Dim CreationDate As New XElement("CreationDate", Format(Now, "d-MMM-yyyy H:mm:ss"))
        CodeName.Add(CreationDate)
        Dim LastEditDate As New XElement("LastEditDate", Format(Now, "d-MMM-yyyy H:mm:ss"))
        CodeName.Add(LastEditDate)
        Dim AuthorFile As New XElement("DefaultAuthorFile", ParentAuthorFile(ParentFileName))
        CodeName.Add(AuthorFile)
        Dim AuthorSummary As New XElement("DefaultAuthorSummary", ParentAuthorSummary(ParentFileName))
        CodeName.Add(AuthorSummary)
        Dim CopyrightFile As New XElement("DefaultCopyrightFile", ParentCopyrightFile(ParentFileName))
        CodeName.Add(CopyrightFile)
        Dim CopyrightSummary As New XElement("DefaultCopyrightSummary", ParentCopyrightSummary(ParentFileName))
        CodeName.Add(CopyrightSummary)
        Dim LicenseFile As New XElement("DefaultLicenseFile", ParentLicenseFile(ParentFileName))
        CodeName.Add(LicenseFile)
        Dim LicenseSummary As New XElement("DefaultLicenseSummary", ParentLicenseSummary(ParentFileName))
        CodeName.Add(LicenseSummary)

        Dim UpdateName As String = System.IO.Path.GetFileNameWithoutExtension(FileName) & ".Update"
        Dim DocFileName As String = System.IO.Path.GetFileNameWithoutExtension(FileName) & ".rtf"
        Dim SelectedNode As New XElement("SelectedNode", DocFileName) 'This is the Update/Version of the document that will be opened.
        CodeName.Add(SelectedNode)

        Dim InitialUpdate As New XElement(UpdateName)
        Dim UpdateLabel As New XElement("Text", "Initial Update")
        InitialUpdate.Add(UpdateLabel)
        Dim UpdateDescription As New XElement("Description", "Initial document update.")
        InitialUpdate.Add(UpdateDescription)
        Dim UpdateCreationDate As New XElement("CreationDate", Format(Now, "d-MMM-yyyy H:mm:ss"))
        InitialUpdate.Add(UpdateCreationDate)
        Dim UpdateLastEditDate As New XElement("LastEditDate", Format(Now, "d-MMM-yyyy H:mm:ss"))
        InitialUpdate.Add(UpdateLastEditDate)
        Dim UpdateAuthorFile As New XElement("DefaultAuthorFile", ParentAuthorFile(ParentFileName))
        InitialUpdate.Add(UpdateAuthorFile)
        Dim UpdateAuthorSummary As New XElement("DefaultAuthorSummary", ParentAuthorSummary(ParentFileName))
        InitialUpdate.Add(UpdateAuthorSummary)
        Dim UpdateCopyrightFile As New XElement("DefaultCopyrightFile", ParentCopyrightFile(ParentFileName))
        InitialUpdate.Add(UpdateCopyrightFile)
        Dim UpdateCopyrightSummary As New XElement("DefaultCopyrightSummary", ParentCopyrightSummary(ParentFileName))
        InitialUpdate.Add(UpdateCopyrightSummary)
        Dim UpdateLicenseFile As New XElement("DefaultLicenseFile", ParentLicenseFile(ParentFileName))
        InitialUpdate.Add(UpdateLicenseFile)
        Dim UpdateLicenseSummary As New XElement("DefaultLicenseSummary", ParentLicenseSummary(ParentFileName))
        InitialUpdate.Add(UpdateLicenseSummary)


        Dim InitialDocument As New XElement(DocFileName)
        Dim DocumentLabel As New XElement("Text", "Inital Version")
        InitialDocument.Add(DocumentLabel)
        Dim DocumentDescription As New XElement("Description", "Initial document version.")
        InitialDocument.Add(DocumentDescription)
        Dim DocumentCreationDate As New XElement("CreationDate", Format(Now, "d-MMM-yyyy H:mm:ss"))
        InitialDocument.Add(DocumentCreationDate)
        Dim DocumentLastEditDate As New XElement("LastEditDate", Format(Now, "d-MMM-yyyy H:mm:ss"))
        InitialDocument.Add(DocumentLastEditDate)
        Dim DocumentAuthorFile As New XElement("AuthorFile", ParentAuthorFile(ParentFileName))
        InitialDocument.Add(DocumentAuthorFile)
        Dim DocumentAuthorSummary As New XElement("AuthorSummary", ParentAuthorSummary(ParentFileName))
        InitialDocument.Add(DocumentAuthorSummary)
        Dim DocumentCopyrightFile As New XElement("CopyrightFile", ParentCopyrightFile(ParentFileName))
        InitialDocument.Add(DocumentCopyrightFile)
        Dim DocumentCopyrightSummary As New XElement("CopyrightSummary", ParentCopyrightSummary(ParentFileName))
        InitialDocument.Add(DocumentCopyrightSummary)
        Dim DocumentLicenseFile As New XElement("LicenseFile", ParentLicenseFile(ParentFileName))
        InitialDocument.Add(DocumentLicenseFile)
        Dim DocumentLicenseSummary As New XElement("LicenseSummary", ParentLicenseSummary(ParentFileName))
        InitialDocument.Add(DocumentLicenseSummary)

        InitialUpdate.Add(InitialDocument)
        CodeName.Add(InitialUpdate)
        XDoc.Add(CodeName)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub CopyAuthor(ByVal FromItem As String, ByVal ToItem As String)
        'Copy the Author settings from the FromItem node name to the ToItem node name.
        'This is used to copy author information into a utility document (Note, Checklist, To Do list etc) that is not stored in a document structure incorporating updates and versions.

        If FromItem = "" Then
            Message.AddWarning("Can not copy the Author settings. The source node is not defined." & vbCrLf)
            Exit Sub
        End If
        If ToItem = "" Then
            Message.AddWarning("Can not copy the Author settings. The destination node is not defined." & vbCrLf)
            Exit Sub
        End If

        'If FromItem = DocStructure Then 'FromItem is the root node.
        If FromItem = LibraryFileName Then 'FromItem is the root node.
            'If DocItem.ContainsKey(ToItem) Then
            'If DefaultDocInfo.ContainsKey(ToItem) Then
            'If DocItem.ContainsKey(ToItem) Then
            If UtilDocInfo.ContainsKey(ToItem) Then
                UtilDocInfo(ToItem).AuthorFile = DefaultAuthorFile
                UtilDocInfo(ToItem).AuthorSummary = DefaultAuthorSummary
            Else
                'DocItem.Add(ToItem, New DocItemInfo)
                'DefaultDocInfo.Add(ToItem, New ADVL_Code_Library_1.DefaultDocInfo)
                UtilDocInfo.Add(ToItem, New DocItemInfo)
                UtilDocInfo(ToItem).Description = txtNewLibraryItemDescription.Text
                UtilDocInfo(ToItem).CreationDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                UtilDocInfo(ToItem).LastEditDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                UtilDocInfo(ToItem).AuthorFile = DefaultAuthorFile
                UtilDocInfo(ToItem).AuthorSummary = DefaultAuthorSummary
            End If
        Else
            'If DocItem.ContainsKey(FromItem) Then
            If DefaultDocInfo.ContainsKey(FromItem) Then
                'If DocItem.ContainsKey(FromItem) Then
                'If DocItem.ContainsKey(ToItem) Then
                'If DefaultDocInfo.ContainsKey(ToItem) Then
                'If DocItem.ContainsKey(ToItem) Then
                If UtilDocInfo.ContainsKey(ToItem) Then
                    UtilDocInfo(ToItem).AuthorFile = DefaultDocInfo(FromItem).AuthorFile
                    UtilDocInfo(ToItem).AuthorSummary = DefaultDocInfo(FromItem).AuthorSummary
                Else
                    'DocItem.Add(ToItem, New DocItemInfo)
                    'DefaultDocInfo.Add(ToItem, New ADVL_Code_Library_1.DefaultDocInfo)
                    UtilDocInfo.Add(ToItem, New DocItemInfo)
                    UtilDocInfo(ToItem).Description = txtNewLibraryItemDescription.Text
                    UtilDocInfo(ToItem).CreationDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                    UtilDocInfo(ToItem).LastEditDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                    UtilDocInfo(ToItem).AuthorFile = DefaultDocInfo(FromItem).AuthorFile
                    UtilDocInfo(ToItem).AuthorSummary = DefaultDocInfo(FromItem).AuthorSummary
                End If
            ElseIf CollectionInfo.ContainsKey(FromItem) Then
                'If DocItem.ContainsKey(ToItem) Then
                If UtilDocInfo.ContainsKey(ToItem) Then
                    UtilDocInfo(ToItem).AuthorFile = CollectionInfo(FromItem).DefaultAuthorFile
                    UtilDocInfo(ToItem).AuthorSummary = CollectionInfo(FromItem).DefaultAuthorSummary
                Else
                    'DocItem.Add(ToItem, New DocItemInfo)
                    'DefaultDocInfo.Add(ToItem, New ADVL_Code_Library_1.DefaultDocInfo)
                    UtilDocInfo.Add(ToItem, New DocItemInfo)
                    UtilDocInfo(ToItem).Description = txtNewLibraryItemDescription.Text
                    UtilDocInfo(ToItem).CreationDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                    UtilDocInfo(ToItem).LastEditDate = Format(Now, "d-MMM-yyyy H:mm:ss")
                    UtilDocInfo(ToItem).AuthorFile = CollectionInfo(FromItem).DefaultAuthorFile
                    UtilDocInfo(ToItem).AuthorSummary = CollectionInfo(FromItem).DefaultAuthorSummary
                End If
            Else
                Message.AddWarning("Can not copy the Author settings. The source node Author is not defined." & vbCrLf)
                Message.AddWarning("The source node name:" & FromItem & vbCrLf)
            End If
        End If


    End Sub

    Private Sub CreateApplicationCodeFile_Old(ByVal FileName As String, ByVal Name As String, ByRef Description As String, ByVal ParentFileName As String)
        'Create an Application Source file with the specified file name and containing the specified description entry.
        'The file name will have hte extension .Code
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Document Structure Definition"))
        Dim CodeName As New XElement(FileName) 'The CodeName is also the name of the XML file containing the document update/version structure.
        Dim CodeText As New XElement("Text", Name) 'The Title of the document.
        CodeName.Add(CodeText)
        Dim Descr As New XElement("Description", Description) 'A description of the document.
        CodeName.Add(Descr)
        Dim UpdateName As String = System.IO.Path.GetFileNameWithoutExtension(FileName) & ".Update"
        Dim DocFileName As String = System.IO.Path.GetFileNameWithoutExtension(FileName) & ".rtf"
        Dim SelectedNode As New XElement("SelectedNode", DocFileName) 'This is the Update/Version of the document that will be opened.
        CodeName.Add(SelectedNode)
        Dim InitialUpdate As New XElement(UpdateName)
        Dim InitialUpdateLabel As New XElement("Text", "Initial Document")
        InitialUpdate.Add(InitialUpdateLabel)
        Dim InitialDocument As New XElement(DocFileName)
        Dim InitialDocumentLabel As New XElement("Text", "Inital Version")
        InitialDocument.Add(InitialDocumentLabel)
        InitialUpdate.Add(InitialDocument)
        CodeName.Add(InitialUpdate)
        XDoc.Add(CodeName)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Function ParentAuthorFile(ByVal ParentFileName As String) As String
        'Returns the Author File of the Parent Library, Collection or Document identified by the ParentFileName.
        If ParentFileName.EndsWith(".Libr") Then
            Return DefaultAuthorFile
        ElseIf ParentFileName.EndsWith(".Coll") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Book") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Appl") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Pics") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorFile
            Else
                Return ""
            End If

        Else
            If DefaultDocInfo.ContainsKey(ParentFileName) Then
                Return DefaultDocInfo(ParentFileName).AuthorFile
            Else
                Return ""
            End If
        End If
    End Function

    Private Function ParentAuthorSummary(ByVal ParentFileName As String) As String
        'Returns the Author Summary of the Parent Library, Collection or Document identified by the ParentFileName.
        If ParentFileName.EndsWith(".Libr") Then
            Return DefaultAuthorSummary
        ElseIf ParentFileName.EndsWith(".Coll") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Book") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Appl") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Pics") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultAuthorSummary
            Else
                Return ""
            End If

        Else
            If DefaultDocInfo.ContainsKey(ParentFileName) Then
                Return DefaultDocInfo(ParentFileName).AuthorSummary
            Else
                Return ""
            End If
        End If
    End Function

    Private Function ParentCopyrightFile(ByVal ParentFileName As String) As String
        'Returns the Copyright File of the Parent Library, Collection or Document identified by the ParentFileName.
        If ParentFileName.EndsWith(".Libr") Then
            Return DefaultCopyrightFile
        ElseIf ParentFileName.EndsWith(".Coll") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Book") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Appl") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Pics") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightFile
            Else
                Return ""
            End If

        Else
            If DefaultDocInfo.ContainsKey(ParentFileName) Then
                Return DefaultDocInfo(ParentFileName).CopyrightFile
            Else
                Return ""
            End If
        End If
    End Function

    Private Function ParentCopyrightSummary(ByVal ParentFileName As String) As String
        'Returns the Copyright Summary of the Parent Library, Collection or Document identified by the ParentFileName.
        If ParentFileName.EndsWith(".Libr") Then
            Return DefaultCopyrightSummary
        ElseIf ParentFileName.EndsWith(".Coll") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Book") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Appl") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Pics") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultCopyrightSummary
            Else
                Return ""
            End If

        Else
            If DefaultDocInfo.ContainsKey(ParentFileName) Then
                Return DefaultDocInfo(ParentFileName).CopyrightSummary
            Else
                Return ""
            End If
        End If
    End Function

    Private Function ParentLicenseFile(ByVal ParentFileName As String) As String
        'Returns the License File of the Parent Library, Collection or Document identified by the ParentFileName.
        If ParentFileName.EndsWith(".Libr") Then
            Return DefaultLicenseFile
        ElseIf ParentFileName.EndsWith(".Coll") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Book") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Appl") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseFile
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Pics") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseFile
            Else
                Return ""
            End If

        Else
            If DefaultDocInfo.ContainsKey(ParentFileName) Then
                Return DefaultDocInfo(ParentFileName).LicenseFile
            Else
                Return ""
            End If
        End If
    End Function

    Private Function ParentLicenseSummary(ByVal ParentFileName As String) As String
        'Returns the License Summary of the Parent Library, Collection or Document identified by the ParentFileName.
        If ParentFileName.EndsWith(".Libr") Then
            Return DefaultLicenseSummary
        ElseIf ParentFileName.EndsWith(".Coll") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Book") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Appl") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseSummary
            Else
                Return ""
            End If
        ElseIf ParentFileName.EndsWith(".Pics") Then
            If CollectionInfo.ContainsKey(ParentFileName) Then
                Return CollectionInfo(ParentFileName).DefaultLicenseSummary
            Else
                Return ""
            End If

        Else
            If DefaultDocInfo.ContainsKey(ParentFileName) Then
                Return DefaultDocInfo(ParentFileName).LicenseSummary
            Else
                Return ""
            End If
        End If
    End Function

    Private Sub CreateApplicationFormFile(ByVal FileName As String, ByRef Description As String)
        'Create an Application Form file with the specified file name and containing the specified description entry.
        Dim decl As New XDeclaration("1.0", "utf-8", "yes")
        Dim XDoc As New XDocument(decl, Nothing)
        XDoc.Add(New XComment(""))
        XDoc.Add(New XComment("Application Form Node"))
        Dim Descr As New XElement("Description", Description)
        XDoc.Add(Descr)
        Project.SaveXmlData(FileName, XDoc)
    End Sub

    Private Sub cmbNewLibraryItemType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNewLibraryItemType.SelectedIndexChanged
        'The selected item type has been changed

        Select Case cmbNewLibraryItemType.SelectedItem.ToString

            Case "Collection"
                'pbIconOpenCollection.Image = ImageList1.Images(2)
                'pbIconClosedCollection.Image = ImageList1.Images(3)
                rbCollection.Checked = True
            Case "Book"
                'pbIconOpenCollection.Image = ImageList1.Images(4)
                'pbIconClosedCollection.Image = ImageList1.Images(5)
                rbBook.Checked = True
            Case "  Section"
                'pbIconOpenCollection.Image = ImageList1.Images(6)
                'pbIconClosedCollection.Image = ImageList1.Images(6)
                rbBookSection.Checked = True
            Case "Application"
                'pbIconOpenCollection.Image = ImageList1.Images(7)
                'pbIconClosedCollection.Image = ImageList1.Images(7)
                rbApplication.Checked = True
            'Case "  Source Code"
            Case "  Code"
                'pbIconOpenCollection.Image = ImageList1.Images(8)
                'pbIconClosedCollection.Image = ImageList1.Images(8)
                rbCode.Checked = True
            Case "  Form"
                rbForm.Checked = True
            Case "Picture Album"
                'pbIconOpenCollection.Image = ImageList1.Images(4)
                'pbIconClosedCollection.Image = ImageList1.Images(5)
                rbAlbum.Checked = True
            Case "  Picture"
                rbPicture.Checked = True
            Case "Note"
                rbNote.Checked = True
            Case "Process"
                rbProcess.Checked = True
                'Case "Web"

                'Case "  Page"

                'Case "Message"

        End Select

    End Sub

    Private Sub rbCollection_CheckedChanged(sender As Object, e As EventArgs) Handles rbCollection.CheckedChanged
        If rbCollection.Checked = True Then
            'Select Collection type
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("Collection")
        End If
    End Sub

    Private Sub rbBook_CheckedChanged(sender As Object, e As EventArgs) Handles rbBook.CheckedChanged
        If rbBook.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("Book")
        End If
    End Sub

    Private Sub rbBookSection_CheckedChanged(sender As Object, e As EventArgs) Handles rbBookSection.CheckedChanged
        If rbBookSection.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("  Section")
        End If
    End Sub

    Private Sub rbApplication_CheckedChanged(sender As Object, e As EventArgs) Handles rbApplication.CheckedChanged
        If rbApplication.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("Application")
        End If
    End Sub

    Private Sub rbCode_CheckedChanged(sender As Object, e As EventArgs) Handles rbCode.CheckedChanged
        If rbCode.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("  Code")
        End If
    End Sub

    Private Sub rbForm_CheckedChanged(sender As Object, e As EventArgs) Handles rbForm.CheckedChanged
        If rbForm.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("  Form")
        End If
    End Sub

    Private Sub rbAlbum_CheckedChanged(sender As Object, e As EventArgs) Handles rbAlbum.CheckedChanged
        If rbAlbum.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("Picture Album")
        End If
    End Sub

    Private Sub rbPicture_CheckedChanged(sender As Object, e As EventArgs) Handles rbPicture.CheckedChanged
        If rbPicture.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("  Picture")
        End If
    End Sub

    Private Sub rbNote_CheckedChanged(sender As Object, e As EventArgs) Handles rbNote.CheckedChanged
        If rbNote.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("Note")
        End If
    End Sub

    Private Sub rbProcess_CheckedChanged(sender As Object, e As EventArgs) Handles rbProcess.CheckedChanged
        If rbProcess.Checked = True Then
            cmbNewLibraryItemType.SelectedIndex = cmbNewLibraryItemType.FindStringExact("Process")
        End If
    End Sub

    Private Sub pbIconCollection_Click(sender As Object, e As EventArgs) Handles pbIconCollection.Click
        rbCollection.Checked = True
    End Sub

    Private Sub pbIconBook_Click(sender As Object, e As EventArgs) Handles pbIconBook.Click
        rbBook.Checked = True
    End Sub

    Private Sub pbIconBookSection_Click(sender As Object, e As EventArgs) Handles pbIconBookSection.Click
        rbBookSection.Checked = True
    End Sub

    Private Sub pbIconApplication_Click(sender As Object, e As EventArgs) Handles pbIconApplication.Click
        rbApplication.Checked = True
    End Sub

    Private Sub pbIconCode_Click(sender As Object, e As EventArgs) Handles pbIconCode.Click
        rbCode.Checked = True
    End Sub

    Private Sub pbIconForm_Click(sender As Object, e As EventArgs) Handles pbIconForm.Click
        rbForm.Checked = True
    End Sub

    Private Sub pbIconAlbum_Click(sender As Object, e As EventArgs) Handles pbIconAlbum.Click
        rbAlbum.Checked = True
    End Sub

    Private Sub pbIconPicture_Click(sender As Object, e As EventArgs) Handles pbIconPicture.Click
        rbPicture.Checked = True
    End Sub

    Private Sub pbIconNote_Click(sender As Object, e As EventArgs) Handles pbIconNote.Click
        rbNote.Checked = True
    End Sub

    Private Sub pbIconProcess_Click(sender As Object, e As EventArgs) Handles pbIconProcess.Click
        rbProcess.Checked = True
    End Sub

#End Region 'Add Item -------------------------------------------------------------------------------------------------------------------------------------------------------------------------



#Region " Edit Items - Edit items in the Information Tree" '===================================================================================================================================

    Private Sub btnSelectNext_Click(sender As Object, e As EventArgs) Handles btnSelectNext.Click
        'Select the next item in the treeview:

        'Dim Nodes As TreeNodeCollection = trvLibrary.Nodes
        If trvLibrary.SelectedNode Is Nothing Then
            'trvLibrary.SelectedNode = Nodes(0)
            'Nodes(0).TreeView.Focus()
        Else
            Dim Node As TreeNode
            Node = trvLibrary.SelectedNode
            If Node.NextNode Is Nothing Then
                Node.TreeView.Focus()
            Else
                trvLibrary.SelectedNode = Node.NextNode
                Node.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnSelectPrev_Click(sender As Object, e As EventArgs) Handles btnSelectPrev.Click
        'Select the previous item in the treeview:

        'Dim Nodes As TreeNodeCollection = trvLibrary.Nodes
        If trvLibrary.SelectedNode Is Nothing Then
            'trvLibrary.SelectedNode = Nodes(0)
            'Nodes(0).TreeView.Focus()
        Else
            Dim Node As TreeNode
            Node = trvLibrary.SelectedNode
            If Node.PrevNode Is Nothing Then
                Node.TreeView.Focus()
            Else
                trvLibrary.SelectedNode = Node.PrevNode
                Node.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnDeleteItem_Click(sender As Object, e As EventArgs) Handles btnDeleteItem.Click
        'Deleted the selected node.

        If trvLibrary.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            Dim Node As TreeNode
            Node = trvLibrary.SelectedNode
            'Dim index As Integer = Node.Index
            If Node.Nodes.Count > 0 Then
                Message.AddWarning("The selected node has child nodes. Delete the child nodes before deleting this node." & vbCrLf)
            Else
                'dgvDeletedItems.Rows.Add(Node.Text, Node.Name, Node.Parent.Name, Node.Index)
                dgvDeletedItems.Rows.Add(Node.Text, Node.Name, Node.Parent.Name, Node.Index, Format(Now, "d-MMM-yyyy H:mm:ss"))
                Dim Parent As TreeNode = Node.Parent
                Parent.Nodes.RemoveAt(Node.Index)
            End If
        End If
    End Sub

    Private Sub btnMoveItemUp_Click(sender As Object, e As EventArgs) Handles btnMoveItemUp.Click
        'Move the selected item up on the Information Tree.

        If trvLibrary.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            Dim Node As TreeNode
            Node = trvLibrary.SelectedNode
            Dim index As Integer = Node.Index
            If index = 0 Then
                'Already at the first node.
                Node.TreeView.Focus()
            Else
                Dim Parent As TreeNode = Node.Parent
                Parent.Nodes.RemoveAt(index)
                Parent.Nodes.Insert(index - 1, Node)
                trvLibrary.SelectedNode = Node
                Node.TreeView.Focus()
                'Node.PrevNode.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnMoveItemDown_Click(sender As Object, e As EventArgs) Handles btnMoveItemDown.Click
        'Move the selected item down on the Information Tree.

        If trvLibrary.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            Dim Node As TreeNode
            Node = trvLibrary.SelectedNode
            Dim index As Integer = Node.Index
            Dim Parent As TreeNode = Node.Parent
            If index < Parent.Nodes.Count - 1 Then
                Parent.Nodes.RemoveAt(index)
                Parent.Nodes.Insert(index + 1, Node)
                trvLibrary.SelectedNode = Node
                Node.TreeView.Focus()
                'Node.NextNode.TreeView.Focus()
            Else
                'Already at the last node.
                Node.TreeView.Focus()
            End If
        End If
    End Sub

    Private Sub btnCutItem_Click(sender As Object, e As EventArgs) Handles btnCutItem.Click

    End Sub

    Private Sub btnCancelCut_Click(sender As Object, e As EventArgs) Handles btnCancelCut.Click

    End Sub

    Private Sub btnPasteItem_Click(sender As Object, e As EventArgs) Handles btnPasteItem.Click

    End Sub

    Private Sub btnApplyNodeTextEdit_Click(sender As Object, e As EventArgs) Handles btnApplyNodeTextEdit.Click
        'Change the selected node text.

        If trvLibrary.SelectedNode Is Nothing Then
            'No node has been selected.
        Else
            If Trim(txtNewNodeText.Text) = "" Then

            Else
                Dim Node As TreeNode
                Node = trvLibrary.SelectedNode
                Node.Text = Trim(txtNewNodeText.Text)
                txtCurrentNodeText.Text = Trim(txtNewNodeText.Text)
                SaveLibrary(txtLibraryFileName.Text)
            End If

        End If
    End Sub

    Private Sub btnApplyNodeKeyEdit_Click(sender As Object, e As EventArgs) Handles btnApplyNodeKeyEdit.Click
        'Change the selected node key.

        If trvLibrary.SelectedNode Is Nothing Then
            'No node has been selected.
            Message.AddWarning("No node has been selected." & vbCrLf)
            Beep()
        Else
            If Trim(txtNewNodeKey.Text) = "" Then
                Message.AddWarning("No new node key has been specified." & vbCrLf)
                Beep()
            Else
                'Check if there is already a file with the same name as the new node key in the project.
                If Project.DataFileExists(Trim(txtNewNodeKey.Text)) Then
                    Message.AddWarning("The new node key is already used." & vbCrLf)
                    Beep()
                Else
                    Project.RenameDataFile(txtCurrentNodeKey.Text, Trim(txtNewNodeKey.Text))
                    Dim Node As TreeNode
                    Node = trvLibrary.SelectedNode
                    Node.Name = Trim(txtNewNodeKey.Text)
                    txtCurrentNodeKey.Text = Trim(txtNewNodeKey.Text)
                    SaveLibrary(txtLibraryFileName.Text)
                End If

            End If
        End If

    End Sub


#End Region 'Edit Items -----------------------------------------------------------------------------------------------------------------------------------------------------------------------





#Region " Deleted Items - Methods to shred or restore items deleted from the Information Tree" '===============================================================================================

    Private Sub btnShred_Click(sender As Object, e As EventArgs) Handles btnShred.Click
        'Shred the item selected in the Deleted Items list.

    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        'Restore the item selected in the Deleted Items list.
        'Note that the dgvDeletedItems selection mode has been set to FullRowSelect.
        '   This was set in the Form Load method:   Private Sub Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        '   This is the line of code used to set the selection mode:  dgvDeletedItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        If dgvDeletedItems.SelectedRows.Count > 0 Then
            If dgvDeletedItems.SelectedRows.Count > 1 Then
                'More than one row has been selected.
                Message.AddWarning("Select only one deleted item." & vbCrLf)
            Else
                'Restore the deleted item.
                Dim RowNo As Integer = dgvDeletedItems.SelectedRows(0).Index
                Dim ParentNode As String = dgvDeletedItems.Rows(RowNo).Cells(2).Value
                Dim myNode() As TreeNode = trvLibrary.Nodes.Find(ParentNode, True)
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
            Message.AddWarning("No deleted item has been selected." & vbCrLf)
        End If

    End Sub


#End Region 'Deleted Items --------------------------------------------------------------------------------------------------------------------------------------------------------------------




    Private Sub ViewDocumentUpdatesAndVersionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewDocumentUpdatesAndVersionsToolStripMenuItem.Click

        OpenDocumentStructure(HoverNodeKey)
    End Sub

    'Private Sub ViewUtilityDocumentToolStripMenuItem_Click(sender As Object, e As EventArgs)
    '    OpenUtilityDocument(HoverNodeKey)
    'End Sub

    Private Sub ViewUtilityDocumentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewUtilityDocumentToolStripMenuItem.Click
        OpenUtilityDocument(HoverNodeKey)
    End Sub

    Private Sub trvLibrary_NodeMouseHover(sender As Object, e As TreeNodeMouseHoverEventArgs) Handles trvLibrary.NodeMouseHover
        'Message.Add("e.Node.Name = " & e.Node.Name & vbCrLf)
        HoverNodeKey = e.Node.Name
        HoverNodeText = e.Node.Text
    End Sub


#Region "Index - Index of Libraries" '=========================================================================================================================================================

    Private Sub btnOpenSelectedLibrary_Click(sender As Object, e As EventArgs) Handles btnOpenSelectedLibrary.Click
        'Open the library selected in the list.
        If lstLibraries.SelectedItem Is Nothing Then
            Message.Add("No library h as been selected from the list." & vbCrLf)
        Else
            Message.Add("Selected library: " & lstLibraries.SelectedItem.ToString & vbCrLf)
            'SaveLibraryFile() 'Save the library that is currently open.
            Message.Add("Saving current library with file name: " & LibraryFileName & vbCrLf)
            SaveLibrary(LibraryFileName) 'Save the library that is currently open.
            Message.Add("Opening another library with file name: " & lstLibraries.SelectedItem.ToString & vbCrLf)
            OpenLibrary(lstLibraries.SelectedItem.ToString)
        End If
    End Sub

    Private Sub lstLibraries_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstLibraries.SelectedIndexChanged

    End Sub

    Private Sub trvLibrary_MouseDown(sender As Object, e As MouseEventArgs) Handles trvLibrary.MouseDown

    End Sub

    Private Sub TabPage5_Click(sender As Object, e As EventArgs) Handles TabPage5.Click

    End Sub

    Private Sub TabPage5_Leave(sender As Object, e As EventArgs) Handles TabPage5.Leave
        'Leaving the Item Information tab

        Message.Add("Leaving the Item Information Page." & vbCrLf)

        If txtItemType.Text = "Collection" Then
            If CollectionInfo.ContainsKey(txtNodeKey.Text) Then
                Message.Add("Saving the Collection Information: " & txtItemLabel.Text & "  in CollectionInfo dictionary entry with key: " & txtNodeKey.Text & vbCrLf)
                'Save the Collection Information settings in the exisiting entry.
                'CollectionInfo(txtNodeKey.Text).Label = txtItemLabel.Text 'NOTE: The item label is stored in the trvLibrary treeview
                CollectionInfo(txtNodeKey.Text).Description = txtItemDescription.Text
                Message.Add("Collection description saved: " & txtItemDescription.Text & vbCrLf)
                If txtItemCreationDate.Text = "" Then txtItemCreationDate.Text = Format(Now, "d-MMM-yyyy H:mm:ss")
                CollectionInfo(txtNodeKey.Text).CreationDate = txtItemCreationDate.Text
                If txtItemLastEditDate.Text = "" Then txtItemLastEditDate.Text = Format(Now, "d-MMM-yyyy H:mm:ss")
                CollectionInfo(txtNodeKey.Text).LastEditDate = txtItemLastEditDate.Text
                CollectionInfo(txtNodeKey.Text).DefaultAuthorFile = txtAuthorFile.Text
                CollectionInfo(txtNodeKey.Text).DefaultAuthorSummary = txtAuthorSummary.Text
                CollectionInfo(txtNodeKey.Text).DefaultCopyrightFile = txtCopyrightFile.Text
                CollectionInfo(txtNodeKey.Text).DefaultCopyrightSummary = txtCopyrightSummary.Text
                CollectionInfo(txtNodeKey.Text).DefaultLicenseFile = txtLicenseFile.Text
                CollectionInfo(txtNodeKey.Text).DefaultLicenseSummary = rtbLicenseNotice.Text
            Else
                'Create a new entry in the CollectionInfo dictionary and add the Collection Information settings.
                Message.Add("Saving the Collection Information: " & txtItemLabel.Text & "  in CollectionInfo dictionary entry with new key: " & txtNodeKey.Text & vbCrLf)
                'CollectionInfo.Add(txtNodeKey.Text, New ADVL_Code_Library_1.CollectionInfo)
                CollectionInfo.Add(txtNodeKey.Text, New ADVL_Information_Library_1.CollectionInfo)
                'CollectionInfo(txtNodeKey.Text).Label = txtItemLabel.Text 'NOTE: The item label is stored in the trvLibrary treeview
                CollectionInfo(txtNodeKey.Text).Description = txtItemDescription.Text
                Message.Add("Collection description saved: " & txtItemDescription.Text & vbCrLf)
                If txtItemCreationDate.Text = "" Then txtItemCreationDate.Text = Format(Now, "d-MMM-yyyy H:mm:ss")
                CollectionInfo(txtNodeKey.Text).CreationDate = txtItemCreationDate.Text
                If txtItemLastEditDate.Text = "" Then txtItemLastEditDate.Text = Format(Now, "d-MMM-yyyy H:mm:ss")
                CollectionInfo(txtNodeKey.Text).LastEditDate = txtItemLastEditDate.Text
                CollectionInfo(txtNodeKey.Text).DefaultAuthorFile = txtAuthorFile.Text
                CollectionInfo(txtNodeKey.Text).DefaultAuthorSummary = txtAuthorSummary.Text
                CollectionInfo(txtNodeKey.Text).DefaultCopyrightFile = txtCopyrightFile.Text
                CollectionInfo(txtNodeKey.Text).DefaultCopyrightSummary = txtCopyrightSummary.Text
                CollectionInfo(txtNodeKey.Text).DefaultLicenseFile = txtLicenseFile.Text
                CollectionInfo(txtNodeKey.Text).DefaultLicenseSummary = rtbLicenseNotice.Text
            End If
        End If

    End Sub

    Private Sub txtItemDescription_TextChanged(sender As Object, e As EventArgs) Handles txtItemDescription.TextChanged

    End Sub

    Private Sub txtItemDescription_LostFocus(sender As Object, e As EventArgs) Handles txtItemDescription.LostFocus
        'Save the Description

        Select Case txtItemType.Text
            Case "Note"
                If UtilDocInfo.ContainsKey(txtItemFileName.Text) Then
                    UtilDocInfo(txtItemFileName.Text).Description = txtItemDescription.Text
                Else
                    UtilDocInfo.Add(txtItemFileName.Text, New DocItemInfo)
                    UtilDocInfo(txtItemFileName.Text).Description = txtItemDescription.Text
                End If

        End Select
    End Sub










#End Region 'Index ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

#End Region 'Form Methods ---------------------------------------------------------------------------------------------------------------------------------------------------------------------


End Class

Public Class DocumentInfo

    Private _left As Integer 'The position of the left edge of the Document form.
    Property Left As Integer
        Get
            Return _left
        End Get
        Set(value As Integer)
            _left = value
        End Set
    End Property

    Private _top As Integer 'The position of the top edge of the Document form.
    Property Top As Integer
        Get
            Return _top
        End Get
        Set(value As Integer)
            _top = value
        End Set
    End Property

    Private _width As Integer 'The width of the Document form.
    Property Width As Integer
        Get
            Return _width
        End Get
        Set(value As Integer)
            _width = value
        End Set
    End Property

    Private _height As Integer 'The height of the Document form.
    Property Height As Integer
        Get
            Return _height
        End Get
        Set(value As Integer)
            _height = value
        End Set
    End Property

    'Private _defaultUpdateVersion As String = "" 'The Default Update and Version document to display when the node is selected.
    'Property DefaultUpdateVersion As String
    '    Get
    '        Return _defaultUpdateVersion
    '    End Get
    '    Set(value As String)
    '        _defaultUpdateVersion = value
    '    End Set
    'End Property

End Class

Public Class DefaultDocInfo
    'Information about the default document selected for a document node.
    'Each document node has an associated document structure containing all the updates and version of the document.
    'One of these is selected as the default document to display when the document node is selected.

    Private _fileName As String = "" 'The file name of the default document.
    Property FileName As String
        Get
            Return _fileName
        End Get
        Set(value As String)
            _fileName = value
        End Set
    End Property

    Private _label As String = "" 'The label of the default document. (This is also the text of the node correcponding to the document.)
    Property Label As String
        Get
            Return _label
        End Get
        Set(value As String)
            _label = value
        End Set
    End Property

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

    Private _copyrightFile As String = "" 'The name of a .Copyright file containing information about the copyright.
    Property CopyrightFile As String
        Get
            Return _copyrightFile
        End Get
        Set(value As String)
            _copyrightFile = value
        End Set
    End Property

    Private _copyrightSummary As String = "" 'A summary of the document version copyright. The full details are stored in the Copyright File.
    Property CopyrightSummary As String
        Get
            Return _copyrightSummary
        End Get
        Set(value As String)
            _copyrightSummary = value
        End Set
    End Property

    Private _LicenseFile As String = "" 'The name of a .License file containing information about the license.
    Property LicenseFile As String
        Get
            Return _LicenseFile
        End Get
        Set(value As String)
            _LicenseFile = value
        End Set
    End Property

    Private _licenseSummary As String = "" 'A summary of the document version license. The full details are stored in the License File.
    Property LicenseSummary As String
        Get
            Return _licenseSummary
        End Get
        Set(value As String)
            _licenseSummary = value
        End Set
    End Property

End Class

Public Class CollectionInfo
    'Information about a document collection.

    'NOTE The name is the Key for the CollectionInfo dictionary. It does not need to be repeated in the dictionary. (It is also stored in the trvLibrary treeview.)
    'Private _name As String = "" 'The name of the collection. The name has an extension (.coll, .Book etc) but there is no associated file.
    'Property Name As String
    '    Get
    '        Return _name
    '    End Get
    '    Set(value As String)
    '        _name = value
    '    End Set
    'End Property

    'Note: The label is stored in the trvLibrary treeview.
    'Private _label As String = "" 'The label of the collection. (This is also the text of the node correcponding to the collection.)
    'Property Label As String
    '    Get
    '        Return _label
    '    End Get
    '    Set(value As String)
    '        _label = value
    '    End Set
    'End Property

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

    Private _defaultAuthorFile As String = "" 'The default author file for Collections or Documents added to the Library. The file contains author details.
    Property DefaultAuthorFile As String
        Get
            Return _defaultAuthorFile
        End Get
        Set(value As String)
            _defaultAuthorFile = value
        End Set
    End Property

    Private _defaultAuthorSummary As String = "" 'The default author summary.
    Property DefaultAuthorSummary As String
        Get
            Return _defaultAuthorSummary
        End Get
        Set(value As String)
            _defaultAuthorSummary = value
        End Set
    End Property

    Private _defaultCopyrightFile As String = "" 'The default copyright file. This file contains copyright details.
    Property DefaultCopyrightFile As String
        Get
            Return _defaultCopyrightFile
        End Get
        Set(value As String)
            _defaultCopyrightFile = value
        End Set
    End Property

    Private _defaultCopyrightSummary As String = "" 'The default copyright summary.
    Property DefaultCopyrightSummary As String
        Get
            Return _defaultCopyrightSummary
        End Get
        Set(value As String)
            _defaultCopyrightSummary = value
        End Set
    End Property

    Private _defaultLicenseFile As String = "" 'The default license file. This file contains license details.
    Property DefaultLicenseFile As String
        Get
            Return _defaultLicenseFile
        End Get
        Set(value As String)
            _defaultLicenseFile = value
        End Set
    End Property

    Private _defaultLicenseSummary As String = "" 'The default license summary.
    Property DefaultLicenseSummary As String
        Get
            Return _defaultLicenseSummary
        End Get
        Set(value As String)
            _defaultLicenseSummary = value
        End Set
    End Property

End Class
