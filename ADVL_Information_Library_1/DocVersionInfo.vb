Public Class DocVersionInfo
    ''Class used to store different versions of a Document.
    ''NOTE: THIS CODE IS NO LONGER USED!!!!
    ''   VERSIONS ARE STORED WITHIN A DOCUMENT STRUCTURE FILE.

    ''LIST OF PROPERTIES:
    ''VersionName            The version of the Document.
    ''VersionDesc            A description of the Version of the Document.
    ''Language
    ''Author
    ''FileName               The name of the file containing the Document in rich text format.

    'Private _versionName As String = "" 'The Name of the version of the Document.
    'Property VersionName As String
    '    Get
    '        Return _versionName
    '    End Get
    '    Set(value As String)
    '        _versionName = value
    '    End Set
    'End Property

    'Private _versionDesc As String = "" 'A description of the Version of the Document.
    'Property VersionDesc As String
    '    Get
    '        Return _versionDesc
    '    End Get
    '    Set(value As String)
    '        _versionDesc = value
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

    'Private _fileName As String = "" 'The file name used to store the Document in rich text format.
    'Property FileName As String
    '    Get
    '        Return _fileName
    '    End Get
    '    Set(value As String)
    '        _fileName = value
    '    End Set
    'End Property

End Class
