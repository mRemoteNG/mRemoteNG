Imports mRemoteNG.App.Runtime
Imports System.Xml
Imports System.IO

Namespace Config
    Namespace Settings
        Public Class Save
#Region "Public Properties"
            Private _MainForm As frmMain
            Public Property MainForm() As frmMain
                Get
                    Return Me._MainForm
                End Get
                Set(ByVal value As frmMain)
                    Me._MainForm = value
                End Set
            End Property
#End Region

#Region "Public Methods"
            Public Sub New(ByVal MainForm As frmMain)
                Me._MainForm = MainForm
            End Sub

            Public Sub Save()
                Try
                    With Me._MainForm
                        Dim windowPlacement As New Tools.WindowPlacement(_MainForm)
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

                        My.Settings.MainFormKiosk = Tools.Misc.Fullscreen.FullscreenActive

                        My.Settings.FirstStart = False
                        My.Settings.ResetPanels = False
                        My.Settings.ResetToolbars = False
                        My.Settings.NoReconnect = False

                        My.Settings.ExtAppsTBLocation = .tsExtAppsToolbar.Location
                        If .tsExtAppsToolbar.Parent IsNot Nothing Then
                            My.Settings.ExtAppsTBParentDock = .tsExtAppsToolbar.Parent.Dock.ToString
                        End If
                        My.Settings.ExtAppsTBVisible = .tsExtAppsToolbar.Visible
                        My.Settings.ExtAppsTBShowText = .cMenToolbarShowText.Checked

                        My.Settings.QuickyTBLocation = .tsQuickConnect.Location
                        If .tsQuickConnect.Parent IsNot Nothing Then
                            My.Settings.QuickyTBParentDock = .tsQuickConnect.Parent.Dock.ToString
                        End If
                        My.Settings.QuickyTBVisible = .tsQuickConnect.Visible

                        If App.Editions.Spanlink.Enabled = False Then
                            My.Settings.ConDefaultPassword = Security.Crypt.Encrypt(My.Settings.ConDefaultPassword, App.Info.General.EncryptionKey)
                        Else
                            My.Settings.LoadConsFromCustomLocation = False
                            My.Settings.CustomConsPath = ""
                        End If

                        My.Settings.Save()
                    End With

                    Me.SavePanelsToXML()
                    Me.SaveExternalAppsToXML()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Saving settings failed" & vbNewLine & vbNewLine & ex.Message, False)
                End Try
            End Sub

            Public Sub SavePanelsToXML()
                Try
                    If Directory.Exists(App.Info.Settings.SettingsPath) = False Then
                        Directory.CreateDirectory(App.Info.Settings.SettingsPath)
                    End If

                    MainForm.pnlDock.SaveAsXml(App.Info.Settings.SettingsPath & "\" & App.Info.Settings.LayoutFileName)
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SavePanelsToXML failed" & vbNewLine & vbNewLine & ex.Message, False)
                End Try
            End Sub

            Public Sub SaveExternalAppsToXML()
                Try
                    If Directory.Exists(App.Info.Settings.SettingsPath) = False Then
                        Directory.CreateDirectory(App.Info.Settings.SettingsPath)
                    End If

                    Dim xW As New XmlTextWriter(App.Info.Settings.SettingsPath & "\" & App.Info.Settings.ExtAppsFilesName, System.Text.Encoding.UTF8)
                    xW.Formatting = Formatting.Indented
                    xW.Indentation = 4

                    xW.WriteStartDocument()
                    xW.WriteStartElement("Apps")

                    For Each extA As Tools.ExternalApp In ExtApps
                        xW.WriteStartElement("App")
                        xW.WriteAttributeString("DisplayName", "", extA.DisplayName)
                        xW.WriteAttributeString("FileName", "", extA.FileName)
                        xW.WriteAttributeString("Arguments", "", extA.Arguments)
                        xW.WriteAttributeString("WaitForExit", "", extA.WaitForExit)
                        xW.WriteAttributeString("TryToIntegrate", "", extA.TryIntegrate)
                        xW.WriteEndElement()
                    Next

                    xW.WriteEndElement()
                    xW.WriteEndDocument()

                    xW.Close()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SaveExternalAppsToXML failed" & vbNewLine & vbNewLine & ex.Message, False)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace