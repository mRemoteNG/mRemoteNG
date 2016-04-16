Imports mRemote3G.App
Imports mRemote3G.Forms.OptionsPages

Namespace Forms
    Public Class OptionsForm

#Region "Constructors"

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.

            Runtime.FontOverride(Me)

            _pages.Add(New StartupExitPage, New PageInfo)
            _pages.Add(New AppearancePage, New PageInfo)
            _pages.Add(New TabsPanelsPage, New PageInfo)
            _pages.Add(New ConnectionsPage, New PageInfo)
            _pages.Add(New SqlServerPage, New PageInfo)
            _pages.Add(New UpdatesPage, New PageInfo)
            _pages.Add(New ThemePage, New PageInfo)
            _pages.Add(New AdvancedPage, New PageInfo)

            _startPage = GetPageFromType(GetType(StartupExitPage))

            _pageIconImageList.ColorDepth = ColorDepth.Depth32Bit
            PageListView.LargeImageList = _pageIconImageList
        End Sub

#End Region

#Region "Public Methods"

        Public Overloads Function ShowDialog(ownerWindow As IWin32Window, pageType As Type) As DialogResult
            _startPage = GetPageFromType(pageType)
            Return ShowDialog(ownerWindow)
        End Function

#End Region

#Region "Private Fields"

        Private ReadOnly _pages As New Dictionary(Of OptionsPage, PageInfo)
        Private ReadOnly _pageIconImageList As New ImageList

        Private _startPage As OptionsPage
        Private _selectedPage As OptionsPage = Nothing

#End Region

#Region "Private Methods"

#Region "Event Handlers"

        Private Sub OptionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            For Each keyValuePair As KeyValuePair(Of OptionsPage, PageInfo) In _pages
                Dim page As OptionsPage = keyValuePair.Key
                Dim pageInfo As PageInfo = keyValuePair.Value
                _pageIconImageList.Images.Add(pageInfo.IconKey, page.PageIcon)
                pageInfo.ListViewItem = PageListView.Items.Add(page.PageName, pageInfo.IconKey)
            Next

            ApplyLanguage()
            LoadSettings()

            ShowPage(_startPage)
        End Sub

        Private Sub OptionsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            If DialogResult = DialogResult.OK Then
                SaveSettings()
            Else
                RevertSettings()
            End If
        End Sub

        Private Sub PageListView_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) _
            Handles PageListView.ItemSelectionChanged
            If Not e.IsSelected Then Return
            If _pages.Count < 1 Then Return
            Dim page As OptionsPage = GetPageFromListViewItem(e.Item)
            If _selectedPage IsNot page Then
                ShowPage(page)
            End If
            SelectNextControl(PageListView, True, True, True, True)
        End Sub

        Private Sub PageListView_MouseUp(sender As Object, e As MouseEventArgs) Handles PageListView.MouseUp
            If PageListView.SelectedIndices.Count = 0 Then
                Dim pageInfo As PageInfo = _pages(_selectedPage)
                pageInfo.ListViewItem.Selected = True
            End If
            SelectNextControl(PageListView, True, True, True, True)
        End Sub

        Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
            DialogResult = DialogResult.OK
            Close()
        End Sub

        Private Sub CancelButtonControl_Click(sender As Object, e As EventArgs) Handles CancelButtonControl.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

#End Region

        Private Sub ApplyLanguage()
            Text = Language.Language.strMenuOptions
            OkButton.Text = Language.Language.strButtonOK
            CancelButtonControl.Text = Language.Language.strButtonCancel

            For Each page As OptionsPage In _pages.Keys
                Try
                    page.ApplyLanguage()
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(
                        String.Format("OptionsPage.ApplyLanguage() failed for page {0}.", page.PageName), ex, , True)
                End Try
            Next
        End Sub

        Private Sub LoadSettings()
            For Each page As OptionsPage In _pages.Keys
                Try
                    page.LoadSettings()
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(
                        String.Format("OptionsPage.LoadSettings() failed for page {0}.", page.PageName), ex, , True)
                End Try
            Next
        End Sub

        Private Sub SaveSettings()
            For Each page As OptionsPage In _pages.Keys
                Try
                    page.SaveSettings()
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(
                        String.Format("OptionsPage.SaveSettings() failed for page {0}.", page.PageName), ex, , True)
                End Try
            Next
        End Sub

        Private Sub RevertSettings()
            For Each page As OptionsPage In _pages.Keys
                Try
                    page.RevertSettings()
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(
                        String.Format("OptionsPage.RevertSettings() failed for page {0}.", page.PageName), ex, , True)
                End Try
            Next
        End Sub

        Private Function GetPageFromType(pageType As Type) As OptionsPage
            For Each page As OptionsPage In _pages.Keys
                If page.GetType() Is pageType Then Return page
            Next
            Return Nothing
        End Function

        Private Function GetPageFromListViewItem(listViewItem As ListViewItem) As OptionsPage
            For Each keyValuePair As KeyValuePair(Of OptionsPage, PageInfo) In _pages
                Dim page As OptionsPage = keyValuePair.Key
                Dim pageInfo As PageInfo = keyValuePair.Value
                If pageInfo.ListViewItem Is listViewItem Then Return page
            Next
            Return Nothing
        End Function

        Private Sub ShowPage(newPage As OptionsPage)
            If _selectedPage IsNot Nothing Then
                Dim oldPage As OptionsPage = _selectedPage
                oldPage.Visible = False
                If _pages.ContainsKey(oldPage) Then
                    Dim oldPageInfo As PageInfo = _pages(oldPage)
                    oldPageInfo.ListViewItem.Selected = False
                End If
            End If

            _selectedPage = newPage

            If newPage IsNot Nothing Then
                newPage.Parent = PagePanel
                newPage.Dock = DockStyle.Fill
                newPage.Visible = True
                If _pages.ContainsKey(newPage) Then
                    Dim newPageInfo As PageInfo = _pages(newPage)
                    newPageInfo.ListViewItem.Selected = True
                End If
            End If
        End Sub

#End Region

#Region "Private Classes"

        Private Class PageInfo
            Public Property IconKey As String
            Public Property ListViewItem As ListViewItem

            Public Sub New()
                IconKey = Guid.NewGuid.ToString()
            End Sub
        End Class

#End Region
    End Class
End Namespace