Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Reflection

Namespace Tools
    Public Class PropertyGridCommandSite
        Implements IMenuCommandService, ISite

        Protected TheObject As Object

        Sub New([object] As Object)
            TheObject = [object]
        End Sub

        Public ReadOnly Property Verbs As DesignerVerbCollection Implements IMenuCommandService.Verbs
            Get
                Dim objectVerbs As New DesignerVerbCollection()
                ' ReSharper disable VBPossibleMistakenCallToGetType.2
                Dim methods() As MethodInfo =
                        TheObject.GetType().GetMethods(BindingFlags.Public Or BindingFlags.Instance)
                ' ReSharper restore VBPossibleMistakenCallToGetType.2
                For Each method As MethodInfo In methods
                    Dim commandAttributes() As Object = method.GetCustomAttributes(GetType(CommandAttribute), True)
                    If commandAttributes Is Nothing OrElse commandAttributes.Length = 0 Then Continue For

                    Dim commandAttribute = CType(commandAttributes(0), CommandAttribute)
                    If Not commandAttribute.Command Then Continue For

                    Dim displayName As String = method.Name
                    Dim displayNameAttributes() As Object = method.GetCustomAttributes(GetType(DisplayNameAttribute),
                                                                                       True)
                    If Not (displayNameAttributes Is Nothing OrElse displayNameAttributes.Length = 0) Then
                        Dim displayNameAttribute = CType(displayNameAttributes(0), DisplayNameAttribute)
                        If Not String.IsNullOrEmpty(displayNameAttribute.DisplayName) Then
                            displayName = displayNameAttribute.DisplayName
                        End If
                    End If
                    objectVerbs.Add(New DesignerVerb(displayName, New EventHandler(AddressOf VerbEventHandler)))
                Next

                Return objectVerbs
            End Get
        End Property

        Private Sub VerbEventHandler(sender As Object, e As EventArgs)
            Dim verb = TryCast(sender, DesignerVerb)
            If verb Is Nothing Then Return
            ' ReSharper disable VBPossibleMistakenCallToGetType.2
            Dim methods() As MethodInfo = TheObject.GetType().GetMethods(BindingFlags.Public Or BindingFlags.Instance)
            ' ReSharper restore VBPossibleMistakenCallToGetType.2
            For Each method As MethodInfo In methods
                Dim commandAttributes() As Object = method.GetCustomAttributes(GetType(CommandAttribute), True)
                If commandAttributes Is Nothing OrElse commandAttributes.Length = 0 Then Continue For

                Dim commandAttribute = CType(commandAttributes(0), CommandAttribute)
                If Not commandAttribute.Command Then Continue For

                Dim displayName As String = method.Name
                Dim displayNameAttributes() As Object = method.GetCustomAttributes(GetType(DisplayNameAttribute), True)
                If Not (displayNameAttributes Is Nothing OrElse displayNameAttributes.Length = 0) Then
                    Dim displayNameAttribute = CType(displayNameAttributes(0), DisplayNameAttribute)
                    If Not String.IsNullOrEmpty(displayNameAttribute.DisplayName) Then
                        displayName = displayNameAttribute.DisplayName
                    End If
                End If

                If verb.Text = displayName Then
                    method.Invoke(TheObject, Nothing)
                    Return
                End If
            Next
        End Sub

        Public Function GetService(serviceType As Type) As Object Implements IServiceProvider.GetService
            If serviceType Is GetType(IMenuCommandService) Then
                Return Me
            Else
                Return Nothing
            End If
        End Function

        Public ReadOnly Property Component As IComponent Implements ISite.Component
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property Container As IContainer Implements ISite.Container
            Get
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property DesignMode As Boolean Implements ISite.DesignMode
            Get
                Return True
            End Get
        End Property

        Public Property Name As String Implements ISite.Name
            Get
                Throw New NotSupportedException()
            End Get
            Set
                Throw New NotImplementedException()
            End Set
        End Property

        Public Sub AddCommand(command As MenuCommand) Implements IMenuCommandService.AddCommand
            Throw New NotImplementedException()
        End Sub

        Public Sub AddVerb(verb As DesignerVerb) Implements IMenuCommandService.AddVerb
            Throw New NotImplementedException()
        End Sub

        Public Function FindCommand(commandId As CommandID) As MenuCommand Implements IMenuCommandService.FindCommand
            Throw New NotImplementedException()
        End Function

        Public Function GlobalInvoke(commandId As CommandID) As Boolean Implements IMenuCommandService.GlobalInvoke
            Throw New NotImplementedException()
        End Function

        Public Sub RemoveCommand(command As MenuCommand) Implements IMenuCommandService.RemoveCommand
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveVerb(verb As DesignerVerb) Implements IMenuCommandService.RemoveVerb
            Throw New NotImplementedException()
        End Sub

        Public Sub ShowContextMenu(menuId As CommandID, x As Integer, y As Integer) _
            Implements IMenuCommandService.ShowContextMenu
            Throw New NotImplementedException()
        End Sub
    End Class

    Public Class CommandAttribute
        Inherits Attribute
        Public Property Command As Boolean = False

        Sub New(Optional ByVal isCommand As Boolean = True)
            Command = isCommand
        End Sub
    End Class
End Namespace