Public Class DVSettings
    ''Class used to store the Settings of a Document View form.

    ''NOTE: NO LONGER USED.
    ''  SEE FormSettingsItems

    'Public Versions As New List(Of DocVersionInfo) 'Stores a list of other versions of the Document View.

    ''LIST OF PROPERTIES:
    ''Left               Stores the position of the Left of a form.
    ''Top                Stores the position of the Top of a form.
    ''Width              Stores the Width of a form.
    ''Height             Stores the Height of the form
    ''Name               The name of the Code Example
    ''Description        A description of the Code Example.
    ''Language           The coding language.
    ''Author             The autor of the Code Example.
    ''VersionNo          The selected version number of the Code Example.
    ''VersionName        The name of the selected version.
    ''VersionDesc        A description of the selected version
    ''UpdateNo
    ''UpdateName
    ''UpdateDesc
    ''FileName           The name of the file used to save the version of the Document in rich text format.
    ''SelectedTab        The tab selected on the Document View form.

    'Private _left As Integer = 10 'Stores the position of the Left of a form.
    'Property Left As Integer
    '    Get
    '        Return _left
    '    End Get
    '    Set(value As Integer)
    '        _left = value
    '    End Set
    'End Property

    'Private _top As Integer = 10 'Stores the position of the Top of a form.
    'Property Top As Integer
    '    Get
    '        Return _top
    '    End Get
    '    Set(value As Integer)
    '        _top = value
    '    End Set
    'End Property

    'Private _width As Integer = 1000 'Stores the Width of a form.
    'Property Width As Integer
    '    Get
    '        Return _width
    '    End Get
    '    Set(value As Integer)
    '        _width = value
    '    End Set
    'End Property

    'Private _height As Integer = 500 'Stores the Height of the form
    'Property Height As Integer
    '    Get
    '        Return _height
    '    End Get
    '    Set(value As Integer)
    '        _height = value
    '    End Set
    'End Property

    'Private _name As String 'The name of the document
    'Property Name As String
    '    Get
    '        Return _name
    '    End Get
    '    Set(value As String)
    '        _name = value
    '    End Set
    'End Property

    'Private _description As String = "" 'A description of a document.
    'Property Description As String
    '    Get
    '        Return _description
    '    End Get
    '    Set(value As String)
    '        _description = value
    '    End Set
    'End Property

    'Private _type As String 'The type of document.
    'Property Type As String
    '    Get
    '        Return _type
    '    End Get
    '    Set(value As String)
    '        _type = value
    '    End Set
    'End Property

    'Private _language As String = "" 'The coding language used.
    'Property Language As String
    '    Get
    '        Return _language
    '    End Get
    '    Set(value As String)
    '        _language = value
    '    End Set
    'End Property

    'Private _author As String = "" 'The author of the Document.
    'Property Author As String
    '    Get
    '        Return _author
    '    End Get
    '    Set(value As String)
    '        _author = value
    '    End Set
    'End Property

    'Private _license As String 'The document license.
    'Property License As String
    '    Get
    '        Return _license
    '    End Get
    '    Set(value As String)
    '        _license = value
    '    End Set
    'End Property

    'Private _versionNo As Integer = 0 'The number of the selected version of the document.
    'Property VersionNo As Integer
    '    Get
    '        Return _versionNo
    '    End Get
    '    Set(value As Integer)
    '        _versionNo = value
    '    End Set
    'End Property

    'Private _versionName As String = "" 'The Name of the selected version of the document.
    'Property VersionName As String
    '    Get
    '        Return _versionName
    '    End Get
    '    Set(value As String)
    '        _versionName = value
    '    End Set
    'End Property

    'Private _versionDesc As String = "" 'A description of the Version of the document.
    'Property VersionDesc As String
    '    Get
    '        Return _versionDesc
    '    End Get
    '    Set(value As String)
    '        _versionDesc = value
    '    End Set
    'End Property

    'Private _updateNo As Integer 'The number of the update of the selected document.
    'Property UpdateNo As Integer
    '    Get
    '        Return _updateNo
    '    End Get
    '    Set(value As Integer)
    '        _updateNo = value
    '    End Set
    'End Property

    'Private _updateName As String 'The name of the update of the selected document.
    'Property UpdateName As String
    '    Get
    '        Return _updateName
    '    End Get
    '    Set(value As String)
    '        _updateName = value
    '    End Set
    'End Property

    'Private _updateDesc As String 'A description of the update of the selected document.
    'Property UpdateDesc As String
    '    Get
    '        Return _updateDesc
    '    End Get
    '    Set(value As String)
    '        _updateDesc = value
    '    End Set
    'End Property

    'Private _fileName As String = "" 'The file name used to store the rich text.
    'Property FileName As String
    '    Get
    '        Return _fileName
    '    End Get
    '    Set(value As String)
    '        _fileName = value
    '    End Set
    'End Property

    'Private _selectedTab As Integer = 0 'The selected Query or Information tab.
    'Property SelectedTab As Integer
    '    Get
    '        Return _selectedTab
    '    End Get
    '    Set(value As Integer)
    '        _selectedTab = value
    '    End Set
    'End Property

    'Private _creationDate As DateTime = Now
    'Property CreationDate As DateTime
    '    Get
    '        Return _creationDate
    '    End Get
    '    Set(value As DateTime)
    '        _creationDate = value
    '    End Set
    'End Property

    'Private _lastEditDate As DateTime = Now
    'Property LastEditDate As DateTime
    '    Get
    '        Return _lastEditDate
    '    End Get
    '    Set(value As DateTime)
    '        _lastEditDate = value
    '    End Set
    'End Property

End Class
