Public Class FormSettings
    '    'This class stores a list of for settings.
    '    'Methods are also included to save and restore the list from a file in the Project directory.

    '    Public Settings As New Dictionary(Of String, FormSettingsItems)

    '#Region "Methods" '=======================================================================================================

    '    'Clear the list.
    '    Public Sub Clear()
    '        Settings.Clear()
    '        'ListFileName = ""
    '        'Description = ""
    '    End Sub

    '    'Load the XML data in the XDoc into the Form Settings List.
    '    Public Sub LoadXml(ByRef XDoc As System.Xml.Linq.XDocument)

    '        'CreationDate = XDoc.<SettingsList>.<CreationDate>.Value
    '        'LastEditDate = XDoc.<SettingsList>.<LastEditDate>.Value
    '        'Description = XDoc.<SettingsList>.<Description>.Value

    '        Dim FormSettings = From item In XDoc.<SettingsList>.<FormSettings>

    '        Settings.Clear()

    '        For Each item In FormSettings
    '            Dim DocName As String = item.<Name>.Value
    '            Dim NewSettings As New FormSettingsItems
    '            NewSettings.Left = item.<Left>.Value
    '            NewSettings.Top = item.<Top>.Value
    '            NewSettings.Height = item.<Height>.Value
    '            NewSettings.Width = item.<Width>.Value

    '            Settings.Add(DocName, NewSettings)

    '            'List.Add(NewSettings)
    '        Next

    '    End Sub

    '    'Function to return the list of Form Settings as an XDocument
    '    Public Function ToXDoc() As System.Xml.Linq.XDocument
    '        Dim XDoc = <?xml version="1.0" encoding="utf-8"?>
    '                   <!---->
    '                   <!--Form Settings list file-->
    '                   <SettingsList>
    '                       <!---->
    '                       <%= From item In Settings
    '                           Select
    '                           <FormSettings>
    '                               <Name><%= item.Key %></Name>
    '                               <Left><%= item.Value.Left %></Left>
    '                               <Top><%= item.Value.Top %></Top>
    '                               <Height><%= item.Value.Height %></Height>
    '                               <Width><%= item.Value.Width %></Width>
    '                           </FormSettings> %>
    '                   </SettingsList>
    '        Return XDoc
    '    End Function


    '    'Insert a Settings entry at the specified location in the dictionary.
    '    Public Sub InsertSettings(ByVal Name As String, ByVal Item As FormSettingsItems)

    '        If Settings.ContainsKey(Name) Then
    '            Settings(Name) = Item
    '        Else
    '            Settings.Add(Name, Item)
    '        End If
    '    End Sub



    '#End Region 'Methods -----------------------------------------------------------------------------------------------------


    '#Region "Events" '========================================================================================================

    '    Event ErrorMessage(ByVal Message As String) 'Send an error message.
    '    Event Message(ByVal Message As String) 'Send a normal message.

    '#End Region 'Events ------------------------------------------------------------------------------------------------------

End Class
