Imports System.ComponentModel
Imports mRemote3G.App
Imports mRemote3G.Config.Connections

Namespace Forms
    Public Class ExportForm
        Inherits Form

#Region "Public Properties"

        Public Property FileName As String
            Get
                Return txtFileName.Text
            End Get
            Set
                txtFileName.Text = value
            End Set
        End Property

        Public Property SaveFormat As ConnectionsSave.Format
            Get
                Dim exportFormat = TryCast(cboFileFormat.SelectedItem, ExportFormat)
                If exportFormat Is Nothing Then
                    Return ConnectionsSave.Format.mRXML
                Else
                    Return exportFormat.Format
                End If
            End Get
            Set
                For Each item As Object In cboFileFormat.Items
                    Dim exportFormat = TryCast(item, ExportFormat)
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
            Set
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
            Set
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
            Set
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
            Set
                chkUsername.Checked = value
            End Set
        End Property

        Public Property IncludePassword As Boolean
            Get
                Return chkPassword.Checked
            End Get
            Set
                chkPassword.Checked = value
            End Set
        End Property

        Public Property IncludeDomain As Boolean
            Get
                Return chkDomain.Checked
            End Get
            Set
                chkDomain.Checked = value
            End Set
        End Property

        Public Property IncludeInheritance As Boolean
            Get
                Return chkInheritance.Checked
            End Get
            Set
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

        Private Sub ExportForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            cboFileFormat.Items.Clear()
            cboFileFormat.Items.Add(New ExportFormat(ConnectionsSave.Format.mRXML))
            cboFileFormat.Items.Add(New ExportFormat(ConnectionsSave.Format.mRCSV))
            cboFileFormat.Items.Add(New ExportFormat(ConnectionsSave.Format.vRDCSV))
            cboFileFormat.SelectedIndex = 0

            ApplyLanguage()
        End Sub

        Private Sub txtFileName_TextChanged(sender As Object, e As EventArgs) Handles txtFileName.TextChanged
            btnOK.Enabled = Not String.IsNullOrEmpty(txtFileName.Text)
        End Sub

        Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
            Using saveFileDialog As New SaveFileDialog()
                With saveFileDialog
                    .CheckPathExists = True
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                    .OverwritePrompt = True

                    Dim fileTypes As New List(Of String)
                    fileTypes.AddRange({Language.Language.strFiltermRemoteXML, "*.xml"})
                    fileTypes.AddRange({Language.Language.strFiltermRemoteCSV, "*.csv"})
                    fileTypes.AddRange({Language.Language.strFiltervRD2008CSV, "*.csv"})
                    fileTypes.AddRange({Language.Language.strFilterAll, "*.*"})

                    .Filter = String.Join("|", fileTypes.ToArray())
                End With

                If Not saveFileDialog.ShowDialog(Me) = DialogResult.OK Then Return

                txtFileName.Text = saveFileDialog.FileName
            End Using
        End Sub

        Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
            DialogResult = DialogResult.OK
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            DialogResult = DialogResult.Cancel
        End Sub

#End Region

        Private Sub ApplyLanguage()
            Text = Language.Language.strExport

            grpFile.Text = Language.Language.strExportFile
            lblFileName.Text = Language.Language.strLabelFilename
            btnBrowse.Text = Language.Language.strButtonBrowse
            lblFileFormat.Text = Language.Language.strFileFormatLabel

            grpItems.Text = Language.Language.strExportItems
            rdoExportEverything.Text = Language.Language.strExportEverything
            rdoExportSelectedFolder.Text = Language.Language.strExportSelectedFolder
            rdoExportSelectedConnection.Text = Language.Language.strExportSelectedConnection

            grpProperties.Text = Language.Language.strExportProperties
            chkUsername.Text = Language.Language.strCheckboxUsername
            chkPassword.Text = Language.Language.strCheckboxPassword
            chkDomain.Text = Language.Language.strCheckboxDomain
            chkInheritance.Text = Language.Language.strCheckboxInheritance
            lblUncheckProperties.Text = Language.Language.strUncheckProperties

            btnOK.Text = Language.Language.strButtonOK
            btnCancel.Text = Language.Language.strButtonCancel
        End Sub

#End Region

#Region "Public Enumerations"

        Public Enum ExportScope
            Everything
            SelectedFolder
            SelectedConnection
        End Enum

#End Region

#Region "Private Classes"

        <ImmutableObject(True)>
        Private Class ExportFormat

#Region "Public Properties"

            Private ReadOnly _format As ConnectionsSave.Format

            Public ReadOnly Property Format As ConnectionsSave.Format
                Get
                    Return _format
                End Get
            End Property

#End Region

#Region "Constructors"

            Public Sub New(format As ConnectionsSave.Format)
                _format = format
            End Sub

#End Region

#Region "Public Methods"

            Public Overrides Function ToString() As String
                Select Case Format
                    Case ConnectionsSave.Format.mRXML
                        Return Language.Language.strMremoteNgXml
                    Case ConnectionsSave.Format.mRCSV
                        Return Language.Language.strMremoteNgCsv
                    Case ConnectionsSave.Format.vRDCSV
                        Return Language.Language.strVisionAppRemoteDesktopCsv
                    Case Else
                        Return Format.ToString()
                End Select
            End Function

#End Region
        End Class

#End Region
    End Class
End Namespace