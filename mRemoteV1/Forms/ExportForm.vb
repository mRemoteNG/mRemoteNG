Imports System.ComponentModel
Imports mRemoteNG.Config.Connections
Imports mRemoteNG.App
Imports mRemoteNG.My

Namespace Forms
    Public Class ExportForm
        Inherits Form

#Region "Public Properties"
        Public Property FileName As String
            Get
                Return txtFileName.Text
            End Get
            Set(value As String)
                txtFileName.Text = value
            End Set
        End Property

        Public Property SaveFormat As Config.Connections.Save.Format
            Get
                Dim exportFormat As ExportFormat = TryCast(cboFileFormat.SelectedItem, ExportFormat)
                If exportFormat Is Nothing Then
                    Return Config.Connections.Save.Format.mRXML
                Else
                    Return exportFormat.Format
                End If
            End Get
            Set(value As Config.Connections.Save.Format)
                For Each item As Object In cboFileFormat.Items
                    Dim exportFormat As ExportFormat = TryCast(item, ExportFormat)
                    If exportFormat Is Nothing Then Continue For
                    If exportFormat.Format = value Then
                        cboFileFormat.SelectedItem = item
                        Exit For
                    End If
                Next
            End Set
        End Property

        Public Property Scope As ExportScope
            Get
                If rdoExportSelectedFolder.Checked Then
                    Return ExportScope.SelectedFolder
                ElseIf rdoExportSelectedConnection.Checked Then
                    Return ExportScope.SelectedConnection
                Else
                    Return ExportScope.Everything
                End If
            End Get
            Set(value As ExportScope)
                Select Case value
                    Case ExportScope.Everything
                        rdoExportEverything.Checked = True
                    Case ExportScope.SelectedFolder
                        rdoExportSelectedFolder.Checked = True
                    Case ExportScope.SelectedConnection
                        rdoExportSelectedConnection.Checked = True
                End Select
            End Set
        End Property

        Private _selectedFolder As TreeNode
        Public Property SelectedFolder As TreeNode
            Get
                Return _selectedFolder
            End Get
            Set(value As TreeNode)
                _selectedFolder = value
                If value Is Nothing Then
                    lblSelectedFolder.Text = String.Empty
                Else
                    lblSelectedFolder.Text = value.Text
                End If
                rdoExportSelectedFolder.Enabled = (value IsNot Nothing)
            End Set
        End Property

        Private _selectedConnection As TreeNode
        Public Property SelectedConnection As TreeNode
            Get
                Return _selectedConnection
            End Get
            Set(value As TreeNode)
                _selectedConnection = value
                If value Is Nothing Then
                    lblSelectedConnection.Text = String.Empty
                Else
                    lblSelectedConnection.Text = value.Text
                End If
                rdoExportSelectedConnection.Enabled = (value IsNot Nothing)
            End Set
        End Property

        Public Property IncludeUsername As Boolean
            Get
                Return chkUsername.Checked
            End Get
            Set(value As Boolean)
                chkUsername.Checked = value
            End Set
        End Property

        Public Property IncludePassword As Boolean
            Get
                Return chkPassword.Checked
            End Get
            Set(value As Boolean)
                chkPassword.Checked = value
            End Set
        End Property

        Public Property IncludeDomain As Boolean
            Get
                Return chkDomain.Checked
            End Get
            Set(value As Boolean)
                chkDomain.Checked = value
            End Set
        End Property

        Public Property IncludeInheritance As Boolean
            Get
                Return chkInheritance.Checked
            End Get
            Set(value As Boolean)
                chkInheritance.Checked = value
            End Set
        End Property
#End Region

#Region "Constructors"
        Public Sub New()
            InitializeComponent()

            Runtime.FontOverride(Me)

            SelectedFolder = Nothing
            SelectedConnection = Nothing

            btnOK.Enabled = False
        End Sub
#End Region

#Region "Private Methods"
#Region "Event Handlers"
        Private Sub ExportForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            cboFileFormat.Items.Clear()
            cboFileFormat.Items.Add(New ExportFormat(Save.Format.mRXML))
            cboFileFormat.Items.Add(New ExportFormat(Save.Format.mRCSV))
            cboFileFormat.Items.Add(New ExportFormat(Save.Format.vRDCSV))
            cboFileFormat.SelectedIndex = 0

            ApplyLanguage()
        End Sub

        Private Sub txtFileName_TextChanged(sender As System.Object, e As EventArgs) Handles txtFileName.TextChanged
            btnOK.Enabled = Not String.IsNullOrEmpty(txtFileName.Text)
        End Sub

        Private Sub btnBrowse_Click(sender As System.Object, e As EventArgs) Handles btnBrowse.Click
            Using saveFileDialog As New SaveFileDialog()
                With saveFileDialog
                    .CheckPathExists = True
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                    .OverwritePrompt = True

                    Dim fileTypes As New List(Of String)
                    fileTypes.AddRange({Language.strFiltermRemoteXML, "*.xml"})
                    fileTypes.AddRange({Language.strFiltermRemoteCSV, "*.csv"})
                    fileTypes.AddRange({Language.strFiltervRD2008CSV, "*.csv"})
                    fileTypes.AddRange({Language.strFilterAll, "*.*"})

                    .Filter = String.Join("|", fileTypes.ToArray())
                End With

                If Not saveFileDialog.ShowDialog(Me) = DialogResult.OK Then Return

                txtFileName.Text = saveFileDialog.FileName
            End Using
        End Sub

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnOK.Click
            DialogResult = DialogResult.OK
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnCancel.Click
            DialogResult = DialogResult.Cancel
        End Sub
#End Region

        Private Sub ApplyLanguage()
            Text = Language.strExport

            grpFile.Text = Language.strExportFile
            lblFileName.Text = Language.strLabelFilename
            btnBrowse.Text = Language.strButtonBrowse
            lblFileFormat.Text = Language.strFileFormatLabel

            grpItems.Text = Language.strExportItems
            rdoExportEverything.Text = Language.strExportEverything
            rdoExportSelectedFolder.Text = Language.strExportSelectedFolder
            rdoExportSelectedConnection.Text = Language.strExportSelectedConnection

            grpProperties.Text = Language.strExportProperties
            chkUsername.Text = Language.strCheckboxUsername
            chkPassword.Text = Language.strCheckboxPassword
            chkDomain.Text = Language.strCheckboxDomain
            chkInheritance.Text = Language.strCheckboxInheritance
            lblUncheckProperties.Text = Language.strUncheckProperties

            btnOK.Text = Language.strButtonOK
            btnCancel.Text = Language.strButtonCancel
        End Sub
#End Region

#Region "Public Enumerations"
        Public Enum ExportScope As Integer
            Everything
            SelectedFolder
            SelectedConnection
        End Enum
#End Region

#Region "Private Classes"
        <ImmutableObject(True)> _
        Private Class ExportFormat
#Region "Public Properties"
            Private ReadOnly _format As Config.Connections.Save.Format
            Public ReadOnly Property Format As Config.Connections.Save.Format
                Get
                    Return _format
                End Get
            End Property
#End Region

#Region "Constructors"
            Public Sub New(ByVal format As Config.Connections.Save.Format)
                _format = format
            End Sub
#End Region

#Region "Public Methods"
            Public Overrides Function ToString() As String
                Select Case Format
                    Case Config.Connections.Save.Format.mRXML
                        Return Language.strMremoteNgXml
                    Case Config.Connections.Save.Format.mRCSV
                        Return Language.strMremoteNgCsv
                    Case Config.Connections.Save.Format.vRDCSV
                        Return Language.strVisionAppRemoteDesktopCsv
                    Case Else
                        Return Format.ToString()
                End Select
            End Function
#End Region
        End Class
#End Region
    End Class
End Namespace