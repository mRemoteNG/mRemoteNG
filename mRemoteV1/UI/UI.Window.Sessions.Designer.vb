Namespace UI
    Namespace Window
        Partial Public Class Sessions
            Inherits Base
#Region " Windows Form Designer generated code "
            Private components As System.ComponentModel.IContainer
            Friend WithEvents sessionMenuRetrieve As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents sessionMenuLogoff As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents sessionList As System.Windows.Forms.ListView

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container()
                Dim sessionMenu As System.Windows.Forms.ContextMenuStrip
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Sessions))
                Me.sessionMenuRetrieve = New System.Windows.Forms.ToolStripMenuItem()
                Me.sessionMenuLogoff = New System.Windows.Forms.ToolStripMenuItem()
                Me.sessionList = New System.Windows.Forms.ListView()
                Me.sessionUsernameColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.sessionActivityColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.sessionTypeColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                sessionMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
                sessionMenu.SuspendLayout()
                Me.SuspendLayout()
                '
                'sessionMenu
                '
                sessionMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sessionMenuRetrieve, Me.sessionMenuLogoff})
                sessionMenu.Name = "cMenSession"
                sessionMenu.Size = New System.Drawing.Size(153, 70)
                AddHandler sessionMenu.Opening, AddressOf Me.menuSession_Opening
                '
                'sessionMenuRetrieve
                '
                Me.sessionMenuRetrieve.Image = Global.mRemoteNG.My.Resources.Resources.Refresh
                Me.sessionMenuRetrieve.Name = "sessionMenuRetrieve"
                Me.sessionMenuRetrieve.Size = New System.Drawing.Size(152, 22)
                Me.sessionMenuRetrieve.Text = "Retrieve"
                '
                'sessionMenuLogoff
                '
                Me.sessionMenuLogoff.Image = Global.mRemoteNG.My.Resources.Resources.Session_LogOff
                Me.sessionMenuLogoff.Name = "sessionMenuLogoff"
                Me.sessionMenuLogoff.Size = New System.Drawing.Size(152, 22)
                Me.sessionMenuLogoff.Text = Global.mRemoteNG.My.Language.strLogOff
                '
                'sessionList
                '
                Me.sessionList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.sessionList.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.sessionList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.sessionUsernameColumn, Me.sessionActivityColumn, Me.sessionTypeColumn})
                Me.sessionList.ContextMenuStrip = sessionMenu
                Me.sessionList.FullRowSelect = True
                Me.sessionList.GridLines = True
                Me.sessionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                Me.sessionList.Location = New System.Drawing.Point(0, -1)
                Me.sessionList.MultiSelect = False
                Me.sessionList.Name = "sessionList"
                Me.sessionList.ShowGroups = False
                Me.sessionList.Size = New System.Drawing.Size(242, 174)
                Me.sessionList.TabIndex = 0
                Me.sessionList.UseCompatibleStateImageBehavior = False
                Me.sessionList.View = System.Windows.Forms.View.Details
                '
                'sessionUsernameColumn
                '
                Me.sessionUsernameColumn.Text = Global.mRemoteNG.My.Language.strColumnUsername
                Me.sessionUsernameColumn.Width = 80
                '
                'sessionActivityColumn
                '
                Me.sessionActivityColumn.Text = Global.mRemoteNG.My.Language.strActivity
                '
                'sessionTypeColumn
                '
                Me.sessionTypeColumn.Text = Global.mRemoteNG.My.Language.strType
                Me.sessionTypeColumn.Width = 80
                '
                'Sessions
                '
                Me.ClientSize = New System.Drawing.Size(242, 173)
                Me.Controls.Add(Me.sessionList)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "Sessions"
                Me.TabText = Global.mRemoteNG.My.Language.strMenuSessions
                Me.Text = "Sessions"
                sessionMenu.ResumeLayout(False)
                Me.ResumeLayout(False)

            End Sub
            Friend WithEvents sessionUsernameColumn As System.Windows.Forms.ColumnHeader
            Friend WithEvents sessionActivityColumn As System.Windows.Forms.ColumnHeader
            Friend WithEvents sessionTypeColumn As System.Windows.Forms.ColumnHeader
#End Region
        End Class
    End Namespace
End Namespace
