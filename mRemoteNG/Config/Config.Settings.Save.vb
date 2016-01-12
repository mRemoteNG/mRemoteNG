Imports mRemoteNG.App.Runtime
Imports System.Xml
Imports System.IO

Namespace Config
    Namespace Settings
        Public Class Save
#Region "Public Methods"
            Public Shared Sub Save()
                Try
                    With frmMain
                        Dim windowPlacement As New Tools.WindowPlacement(frmMain)
                        If .WindowState = FormWindowState.Minimized And windowPlacement.RestoreToMaximized Then
                            .Opacity = 0
                            .WindowState = FormWindowState.Maximized
                        End If

                        My.Settings.MainFormLocation = .Location
                        My.Settings.MainFormSize = .Size

                        If Not .WindowState = FormWindowState.Normal Then
                            My.Settings.MainFormRestoreLocation = .RestoreBounds.Location
                            My.Settings.MainFormRestoreSize = .RestoreBounds.Size
                        End If

                        My.Settings.MainFormState = .WindowState

                        My.Settings.MainFormKiosk = .Fullscreen.Value

                        My.Settings.FirstStart = False
                        My.Settings.ResetPanels = False
                        My.Settings.ResetToolbars = False
                        My.Settings.NoReconnect = False

                        My.Settings.ExtAppsTBLocation = .tsExternalTools.Location
                        If .tsExternalTools.Parent IsNot Nothing Then
                            My.Settings.ExtAppsTBParentDock = .tsExternalTools.Parent.Dock.ToString
                        End If
                        My.Settings.ExtAppsTBVisible = .tsExternalTools.Visible
                        My.Settings.ExtAppsTBShowText = .cMenToolbarShowText.Checked

                        My.Settings.QuickyTBLocation = .tsQuickConnect.Location
                        If .tsQuickConnect.Parent IsNot Nothing Then
                            My.Settings.QuickyTBParentDock = .tsQuickConnect.Parent.Dock.ToString
                        End If
                        My.Settings.QuickyTBVisible = .tsQuickConnect.Visible

                        My.Settings.ConDefaultPassword = Security.Crypt.Encrypt(My.Settings.ConDefaultPassword, App.Info.General.EncryptionKey)

                        My.Settings.Save()
                    End With

                    SavePanelsToXML()
                    SaveExternalAppsToXML()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Saving settings failed" & vbNewLine & vbNewLine & ex.Message, False)
                End Try
            End Sub

            Public Shared Sub SavePanelsToXML()
                Try
                    If Directory.Exists(App.Info.Settings.SettingsPath) = False Then
                        Directory.CreateDirectory(App.Info.Settings.SettingsPath)
                    End If

                    frmMain.pnlDock.SaveAsXml(App.Info.Settings.SettingsPath & "\" & App.Info.Settings.LayoutFileName)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SavePanelsToXML failed" & vbNewLine & vbNewLine & ex.Message, False)
                End Try
            End Sub

            Public Shared Sub SaveExternalAppsToXML()
                Try
                    If Directory.Exists(App.Info.Settings.SettingsPath) = False Then
                        Directory.CreateDirectory(App.Info.Settings.SettingsPath)
                    End If

                    Dim xmlTextWriter As New XmlTextWriter(App.Info.Settings.SettingsPath & "\" & App.Info.Settings.ExtAppsFilesName, System.Text.Encoding.UTF8)
                    xmlTextWriter.Formatting = Formatting.Indented
                    xmlTextWriter.Indentation = 4

                    xmlTextWriter.WriteStartDocument()
                    xmlTextWriter.WriteStartElement("Apps")

                    For Each extA As Tools.ExternalTool In ExternalTools
                        xmlTextWriter.WriteStartElement("App")
                        xmlTextWriter.WriteAttributeString("DisplayName", "", extA.DisplayName)
                        xmlTextWriter.WriteAttributeString("FileName", "", extA.FileName)
                        xmlTextWriter.WriteAttributeString("Arguments", "", extA.Arguments)
                        xmlTextWriter.WriteAttributeString("WaitForExit", "", extA.WaitForExit)
                        xmlTextWriter.WriteAttributeString("TryToIntegrate", "", extA.TryIntegrate)
                        xmlTextWriter.WriteEndElement()
                    Next

                    xmlTextWriter.WriteEndElement()
                    xmlTextWriter.WriteEndDocument()

                    xmlTextWriter.Close()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveExternalAppsToXML failed" & vbNewLine & vbNewLine & ex.Message, False)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace