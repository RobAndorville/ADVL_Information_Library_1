Public Class DocViewSettingsList
    '    'Stores a list of Document View form settings.
    '    'NOTE: THIS CLASS IS NO LONGER USED.
    '    '  SEE FormSettings.

    '    Public List As New List(Of DVSettings) 'List of Document View form settings.

    '    Public FileLocation As New ADVL_Utilities_Library_1.FileLocation 'The location of the list file.

    '#Region " Properties" '===================================================================================================

    '    Private _listFileName As String = ""
    '    Property ListFileName As String 'The file name (with extension) of the list file.
    '        Get
    '            Return _listFileName
    '        End Get
    '        Set(value As String)
    '            _listFileName = value
    '        End Set
    '    End Property

    '    Private _creationDate As DateTime = Now 'The date of creation of the list.
    '    Property CreationDate As DateTime
    '        Get
    '            Return _creationDate
    '        End Get
    '        Set(value As DateTime)
    '            _creationDate = value
    '        End Set
    '    End Property

    '    Private _lastEditDate As DateTime = Now 'The last edit date of the list.
    '    Property LastEditDate As DateTime
    '        Get
    '            Return _lastEditDate
    '        End Get
    '        Set(value As DateTime)
    '            _lastEditDate = value
    '        End Set
    '    End Property

    '    Private _description As String = "" 'A description of the list.
    '    Property Description As String
    '        Get
    '            Return _description
    '        End Get
    '        Set(value As String)
    '            _description = value
    '        End Set
    '    End Property

    '    Private _nRecords As Integer = 0 'The number of record in the list
    '    ReadOnly Property NRecords As Integer
    '        Get
    '            _nRecords = List.Count
    '            Return _nRecords
    '        End Get
    '    End Property

    '#End Region 'Properties --------------------------------------------------------------------------------------------------

    '#Region "Methods" '=======================================================================================================

    '    'Clear the list.
    '    Public Sub Clear()
    '        List.Clear()
    '        ListFileName = ""
    '        Description = ""
    '    End Sub

    '    'Load the XML data in the XDoc into the Data View Settings List.
    '    Public Sub LoadXml(ByRef XDoc As System.Xml.Linq.XDocument)

    '        CreationDate = XDoc.<SettingsList>.<CreationDate>.Value
    '        LastEditDate = XDoc.<SettingsList>.<LastEditDate>.Value
    '        Description = XDoc.<SettingsList>.<Description>.Value

    '        Dim Settings = From item In XDoc.<SettingsList>.<CodeExampleSettings>

    '        List.Clear()

    '        For Each item In Settings
    '            Dim NewSettings As New DVSettings
    '            NewSettings.Left = item.<Left>.Value
    '            NewSettings.Top = item.<Top>.Value
    '            NewSettings.Height = item.<Height>.Value
    '            NewSettings.Width = item.<Width>.Value
    '            NewSettings.VersionNo = item.<VersionNo>.Value
    '            NewSettings.VersionName = item.<VersionName>.Value
    '            NewSettings.VersionDesc = item.<VersionDesc>.Value
    '            NewSettings.Description = item.<Description>.Value
    '            NewSettings.Language = item.<Language>.Value
    '            NewSettings.Author = item.<Author>.Value
    '            NewSettings.FileName = item.<FileName>.Value
    '            NewSettings.SelectedTab = item.<SelectedTab>.Value

    '            For Each versItem In item.<VersionList>.<Version>
    '                Dim NewVersion As New DocVersionInfo
    '                NewVersion.VersionName = versItem.<Name>.Value
    '                NewVersion.VersionDesc = versItem.<Description>.Value
    '                NewVersion.Language = versItem.<Language>.Value
    '                NewVersion.Author = versItem.<Author>.Value
    '                NewVersion.FileName = versItem.<FileName>.Value
    '                NewSettings.Versions.Add(NewVersion)
    '            Next

    '            List.Add(NewSettings)
    '        Next

    '    End Sub

    '    'Load the list from the selected list file.
    '    Public Sub LoadFile()
    '        If ListFileName = "" Then 'No list file has been selected.
    '            RaiseEvent ErrorMessage("No list file name has been specified!" & vbCrLf)
    '            Exit Sub
    '        End If

    '        Dim XDoc As System.Xml.Linq.XDocument
    '        FileLocation.ReadXmlData(ListFileName, XDoc)

    '        If IsNothing(XDoc) Then
    '            RaiseEvent ErrorMessage("The specified list file contains no data!" & vbCrLf)
    '            Exit Sub
    '        End If

    '        LoadXml(XDoc)

    '    End Sub

    '    'Function to return the list of Document View Settings as an XDocument
    '    Public Function ToXDoc() As System.Xml.Linq.XDocument
    '        Dim XDoc = <?xml version="1.0" encoding="utf-8"?>
    '                   <!---->
    '                   <!--Document View Settings list file-->
    '                   <SettingsList>
    '                       <CreationDate><%= Format(CreationDate, "d-MMM-yyyy H:mm:ss") %></CreationDate>
    '                       <LastEditDate><%= Format(LastEditDate, "d-MMM-yyyy H:mm:ss") %></LastEditDate>
    '                       <Description><%= Description %></Description>
    '                       <!---->
    '                       <%= From item In List
    '                           Select
    '                           <CodeExampleSettings>
    '                               <Left><%= item.Left %></Left>
    '                               <Top><%= item.Top %></Top>
    '                               <Height><%= item.Height %></Height>
    '                               <Width><%= item.Width %></Width>
    '                               <Description><%= item.Description %></Description>
    '                               <VersionNo><%= item.VersionNo %></VersionNo>
    '                               <VersionName><%= item.VersionName %></VersionName>
    '                               <VersionDesc><%= item.VersionDesc %></VersionDesc>
    '                               <Language><%= item.Language %></Language>
    '                               <Author><%= item.Author %></Author>
    '                               <FileName><%= item.FileName %></FileName>
    '                               <SelectedTab><%= item.SelectedTab %></SelectedTab>
    '                               <VersionList>
    '                                   <%= From versItem In item.Versions
    '                                       Select
    '                                       <Version>
    '                                           <Name><%= versItem.VersionName %></Name>
    '                                           <Description><%= versItem.VersionDesc %></Description>
    '                                           <Language><%= versItem.Language %></Language>
    '                                           <Author><%= versItem.Author %></Author>
    '                                           <FileName><%= versItem.FileName %></FileName>
    '                                       </Version> %>
    '                               </VersionList>
    '                           </CodeExampleSettings> %>
    '                   </SettingsList>
    '        Return XDoc
    '    End Function

    '    'Save the list in the selected list file.
    '    Public Sub SaveFile()
    '        If ListFileName = "" Then 'No list file has been selected.
    '            RaiseEvent ErrorMessage("No list file name has been specified!" & vbCrLf)
    '            Exit Sub
    '        End If

    '        FileLocation.SaveXmlData(ListFileName, ToXDoc)

    '    End Sub

    '    'Insert a Settings entry at the specified position in the list.
    '    Public Sub InsertSettings(ByVal Index As Integer, Item As DVSettings)
    '        'If Index + 1 = List.Count Then
    '        If Index = List.Count Then
    '            'Append the Settings to the end of the List:
    '            List.Add(Item)
    '            'ElseIf Index + 1 > List.Count Then
    '        ElseIf Index > List.Count Then
    '            RaiseEvent ErrorMessage("Index position is too large. Cannot insert the settings into the list." & vbCrLf)
    '        ElseIf Index < 0 Then
    '            RaiseEvent ErrorMessage("Index position is less than zero. Cannot insert the settings into the list." & vbCrLf)
    '        Else
    '            'Move existing entries to make space for the new settings:
    '            Dim LastIndex As Integer = List.Count - 1
    '            List.Add(List(LastIndex)) 'Append a copy of the last settings to the end of the list.
    '            Dim I As Integer
    '            For I = LastIndex To Index + 1 Step -1
    '                List(I) = List(I - 1)
    '            Next
    '            List(Index) = Item
    '        End If
    '    End Sub

    '    'Update a Settings entry at the specified position in the list.
    '    Public Sub UpdateSettings(ByVal Index As Integer, Item As DVSettings)
    '        If Index + 1 > List.Count Then
    '            RaiseEvent ErrorMessage("Index position is too large. Cannot modify the settings in the list." & vbCrLf)
    '        ElseIf Index < 0 Then
    '            RaiseEvent ErrorMessage("Index position is less than zero. Cannot modify the settings in the list." & vbCrLf)
    '        Else
    '            List(Index) = Item
    '        End If
    '    End Sub

    '#End Region 'Methods -----------------------------------------------------------------------------------------------------

    '#Region "Events" '========================================================================================================
    '    Event ErrorMessage(ByVal Message As String) 'Send an error message.
    '    Event Message(ByVal Message As String) 'Send a normal message.
    '#End Region 'Events ------------------------------------------------------------------------------------------------------


End Class
