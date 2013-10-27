Imports System.ComponentModel
Imports mRemoteNG.Tools

Namespace Config.Putty
    Public Class Sessions
        Private Shared _providers As List(Of Provider)
        Private Shared ReadOnly Property Providers() As List(Of Provider)
            Get
                If _providers Is Nothing OrElse _providers.Count = 0 Then AddProviders()
                Return _providers
            End Get
        End Property

        Private Shared Sub AddProviders()
            _providers = New List(Of Provider)()
            _providers.Add(New DefaultSettingsProvider())
            _providers.Add(New RegistryProvider())
            _providers.Add(New XmingProvider())
        End Sub

        Private Delegate Sub AddSessionsToTreeDelegate()
        Public Shared Sub AddSessionsToTree()
            Dim treeView As TreeView = Tree.Node.TreeView
            If treeView Is Nothing Then Return
            If treeView.InvokeRequired Then
                treeView.Invoke(New AddSessionsToTreeDelegate(AddressOf AddSessionsToTree))
                Return
            End If

            Dim puttyType As PuttyTypeDetector.PuttyType = PuttyTypeDetector.GetPuttyType()

            For Each provider As Provider In Providers
                Dim enabled As Boolean = True
                Select Case puttyType
                    Case PuttyTypeDetector.PuttyType.Xming
                        If TypeOf (provider) Is RegistryProvider Then enabled = False
                    Case Else
                        If TypeOf (provider) Is XmingProvider Then enabled = False
                End Select

                Dim rootTreeNode As TreeNode = provider.RootTreeNode
                Dim inUpdate As Boolean = False

                Dim savedSessions As New List(Of Connection.Info)(provider.GetSessions())
                If Not enabled Or savedSessions Is Nothing OrElse savedSessions.Count = 0 Then
                    If rootTreeNode IsNot Nothing AndAlso treeView.Nodes.Contains(rootTreeNode) Then
                        treeView.BeginUpdate()
                        treeView.Nodes.Remove(rootTreeNode)
                        treeView.EndUpdate()
                    End If
                    Continue For
                End If

                If Not treeView.Nodes.Contains(rootTreeNode) Then
                    If Not inUpdate Then
                        treeView.BeginUpdate()
                        inUpdate = True
                    End If
                    treeView.Nodes.Add(rootTreeNode)
                End If

                Dim newTreeNodes As New List(Of TreeNode)
                For Each sessionInfo As Connection.PuttySession.Info In savedSessions
                    Dim treeNode As TreeNode
                    Dim isNewNode As Boolean
                    If rootTreeNode.Nodes.ContainsKey(sessionInfo.Name) Then
                        treeNode = rootTreeNode.Nodes.Item(sessionInfo.Name)
                        isNewNode = False
                    Else
                        treeNode = Tree.Node.AddNode(Tree.Node.Type.PuttySession, sessionInfo.Name)
                        If treeNode Is Nothing Then Continue For
                        treeNode.Name = treeNode.Text
                        treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                        treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                        isNewNode = True
                    End If

                    sessionInfo.RootPuttySessionsInfo = provider.RootInfo
                    sessionInfo.TreeNode = treeNode
                    sessionInfo.Inherit.TurnOffInheritanceCompletely()

                    treeNode.Tag = sessionInfo

                    If isNewNode Then newTreeNodes.Add(treeNode)
                Next

                For Each treeNode As TreeNode In rootTreeNode.Nodes
                    If Not savedSessions.Contains(treeNode.Tag) Then
                        If Not inUpdate Then
                            treeView.BeginUpdate()
                            inUpdate = True
                        End If
                        rootTreeNode.Nodes.Remove(treeNode)
                    End If
                Next

                If Not newTreeNodes.Count = 0 Then
                    If Not inUpdate Then
                        treeView.BeginUpdate()
                        inUpdate = True
                    End If
                    rootTreeNode.Nodes.AddRange(newTreeNodes.ToArray())
                End If

                If inUpdate Then
                    Tree.Node.Sort(rootTreeNode, SortOrder.Ascending)
                    rootTreeNode.Expand()
                    treeView.EndUpdate()
                End If
            Next
        End Sub

        Protected Shared Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
            Dim sessionNames As New List(Of String)
            For Each provider As Provider In Providers
                sessionNames.AddRange(provider.GetSessionNames())
            Next
            Return sessionNames.ToArray()
        End Function

        Public Shared Sub StartWatcher()
            For Each provider As Provider In Providers
                provider.StartWatcher()
                AddHandler provider.SessionChanged, AddressOf SessionChanged
            Next
        End Sub

        Public Shared Sub StopWatcher()
            For Each provider As Provider In Providers
                provider.StopWatcher()
                RemoveHandler provider.SessionChanged, AddressOf SessionChanged
            Next
        End Sub

        Public Shared Sub SessionChanged(ByVal sender As Object, ByVal e As Provider.SessionChangedEventArgs)
            AddSessionsToTree()
        End Sub

        Public Class SessionList
            Inherits StringConverter

            Public Shared ReadOnly Property Names() As String()
                Get
                    Return GetSessionNames()
                End Get
            End Property

            Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
                Return New StandardValuesCollection(Names)
            End Function

            Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                Return True
            End Function
        End Class
    End Class
End Namespace
