Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Namespace Themes
    Public Class ThemeSerializer
        Public Shared Sub SaveToXmlFile(themeInfo As ThemeInfo, filename As String)
            Dim themeList As New List(Of ThemeInfo)
            themeList.Add(ThemeInfo)
            SaveToXmlFile(themeList, filename)
        End Sub

        Public Shared Sub SaveToXmlFile(themes As List(Of ThemeInfo), filename As String)
            Dim tempFileName As String = Path.GetTempFileName()
            Dim xmlTextWriter As New XmlTextWriter(tempFileName, Encoding.UTF8)

            xmlTextWriter.Formatting = Formatting.Indented
            xmlTextWriter.Indentation = 4

            xmlTextWriter.WriteStartDocument()

            xmlTextWriter.WriteStartElement("mRemoteNG")

            xmlTextWriter.WriteStartElement("FileInfo")
            xmlTextWriter.WriteAttributeString("Version", "1.0")
            xmlTextWriter.WriteElementString("FileType", "Theme")
            xmlTextWriter.WriteElementString("FileTypeVersion", "1.0")
            xmlTextWriter.WriteEndElement() ' FileInfo

            Dim themeType As Type = (New ThemeInfo).GetType()
            Dim colorType As Type = (New Color).GetType()
            Dim color As Color
            For Each themeInfo As ThemeInfo In themes
                xmlTextWriter.WriteStartElement("Theme")
                xmlTextWriter.WriteAttributeString("Name", themeInfo.Name)

                For Each propertyInfo As PropertyInfo In themeType.GetProperties()
                    If Not propertyInfo.PropertyType Is colorType Then Continue For
                    color = propertyInfo.GetValue(themeInfo, Nothing)
                    xmlTextWriter.WriteStartElement("Color")
                    xmlTextWriter.WriteAttributeString("Name", propertyInfo.Name)
                    xmlTextWriter.WriteAttributeString("Value", EncodeColorName(color))
                    xmlTextWriter.WriteEndElement() ' Color
                Next

                xmlTextWriter.WriteEndElement() ' Theme
            Next

            xmlTextWriter.WriteEndElement() ' mRemoteNG

            xmlTextWriter.Close()

            File.Delete(filename)
            File.Move(tempFileName, filename)
        End Sub

        Public Shared Function LoadFromXmlFile(filename As String) As List(Of ThemeInfo)
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
            Dim themes As New List(Of ThemeInfo)
            Dim themeInfo As ThemeInfo
            Dim themeType As Type = (New ThemeInfo).GetType()
            Dim colorType As Type = (New Color).GetType()
            Dim colorName As String
            Dim colorValue As String
            Dim propertyInfo As PropertyInfo
            For Each themeNode As XmlNode In themeNodes
                themeInfo = New ThemeInfo
                themeInfo.Name = themeNode.Attributes("Name").Value
                For Each colorNode As XmlNode In themeNode.SelectNodes("./Color")
                    colorName = colorNode.Attributes("Name").Value
                    colorValue = colorNode.Attributes("Value").Value
                    propertyInfo = themeType.GetProperty(colorName)
                    If propertyInfo Is Nothing OrElse Not propertyInfo.PropertyType Is colorType Then Continue For
                    propertyInfo.SetValue(themeInfo, DecodeColorName(colorValue), Nothing)
                Next
                themes.Add(themeInfo)
            Next

            Return themes
        End Function

        Private Shared Function EncodeColorName(color As Color) As String
            If color.IsNamedColor Then
                Return color.Name
            Else
                Return Hex(color.ToArgb()).PadLeft(8, "0")
            End If
        End Function

        Private Shared Function DecodeColorName(name As String) As Color
            Dim regex As New Regex("^[0-9a-fA-F]{8}$")
            If regex.Match(name).Success Then
                Return Color.FromArgb(Convert.ToInt32(name, 16))
            Else
                Return Color.FromName(name)
            End If
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
