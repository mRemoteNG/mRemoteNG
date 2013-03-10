Imports System.IO
Imports System.Xml
Imports System.Reflection

Namespace Config
    Public Class ThemeSerializer
        Public Shared Sub SaveToXmlFile(theme As Theme, filename As String)
            Dim themeList As New List(Of Theme)
            themeList.Add(theme)
            SaveToXmlFile(themeList, filename)
        End Sub

        Public Shared Sub SaveToXmlFile(themes As List(Of Theme), filename As String)
            Dim tempFileName As String = Path.GetTempFileName()
            Dim xmlTextWriter As New XmlTextWriter(tempFileName, System.Text.Encoding.UTF8)

            xmlTextWriter.Formatting = Formatting.Indented
            xmlTextWriter.Indentation = 4

            xmlTextWriter.WriteStartDocument()

            xmlTextWriter.WriteStartElement("mRemoteNG")

            xmlTextWriter.WriteStartElement("FileInfo")
            xmlTextWriter.WriteAttributeString("Version", "1.0")
            xmlTextWriter.WriteElementString("FileType", "Theme")
            xmlTextWriter.WriteElementString("FileTypeVersion", "1.0")
            xmlTextWriter.WriteEndElement() ' FileInfo

            Dim themeType As Type = (New Theme).GetType()
            Dim colorType As Type = (New Color).GetType()
            Dim color As Color
            For Each theme As Theme In themes
                xmlTextWriter.WriteStartElement("Theme")
                xmlTextWriter.WriteAttributeString("Name", theme.Name)

                For Each propertyInfo As PropertyInfo In themeType.GetProperties()
                    If Not propertyInfo.PropertyType Is colorType Then Continue For
                    color = propertyInfo.GetValue(theme, Nothing)
                    xmlTextWriter.WriteStartElement("Color")
                    xmlTextWriter.WriteAttributeString("Name", propertyInfo.Name)
                    xmlTextWriter.WriteAttributeString("Value", color.Name)
                    xmlTextWriter.WriteEndElement() ' Color
                Next

                xmlTextWriter.WriteEndElement() ' Theme
            Next

            xmlTextWriter.WriteEndElement() ' mRemoteNG

            xmlTextWriter.Close()

            File.Delete(filename)
            File.Move(tempFileName, filename)
        End Sub

        Public Shared Function LoadFromXmlFile(filename As String) As List(Of Theme)
            Dim xmlDocument As New XmlDocument()
            xmlDocument.Load(filename)

            Dim fileInfoNode As XmlNode = xmlDocument.SelectSingleNode("/mRemoteNG/FileInfo")
            Dim fileInfoVersion As New Version(fileInfoNode.Attributes("Version").Value)
            If fileInfoVersion > New Version(1, 0) Then
                Throw New FileFormatException(String.Format("Unsupported FileInfo version ({0}).", fileInfoVersion))
            End If

            Dim fileTypeNode As XmlNode = fileInfoNode.SelectSingleNode("./FileType")
            Dim fileType As String = fileTypeNode.InnerText
            If Not fileType = "Theme" Then
                Throw New FileFormatException(String.Format("Incorrect FileType ({0}). Expected ""Theme"".", fileType))
            End If

            Dim fileTypeVersion As New Version(fileInfoNode.SelectSingleNode("./FileTypeVersion").InnerText)
            If fileTypeVersion > New Version(1, 0) Then
                Throw New FileFormatException(String.Format("Unsupported FileTypeVersion ({0}).", fileTypeVersion))
            End If

            Dim themeNodes As XmlNodeList = xmlDocument.SelectNodes("/mRemoteNG/Theme")
            Dim themes As New List(Of Theme)
            Dim theme As Theme
            Dim themeType As Type = (New Theme).GetType()
            Dim colorType As Type = (New Color).GetType()
            Dim colorName As String
            Dim colorValue As String
            Dim propertyInfo As PropertyInfo
            For Each themeNode As XmlNode In themeNodes
                theme = New Theme
                theme.Name = themeNode.Attributes("Name").Value
                For Each colorNode As XmlNode In themeNode.SelectNodes("./Color")
                    colorName = colorNode.Attributes("Name").Value
                    colorValue = colorNode.Attributes("Value").Value
                    propertyInfo = themeType.GetProperty(colorName)
                    If Not propertyInfo.PropertyType Is colorType Then Continue For
                    propertyInfo.SetValue(theme, Color.FromName(colorValue), Nothing)
                Next
                themes.Add(theme)
            Next

            Return themes
        End Function
    End Class
End Namespace
