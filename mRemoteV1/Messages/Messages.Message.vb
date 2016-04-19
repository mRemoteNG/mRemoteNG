Namespace Messages
    Public Enum MessageClass
        InformationMsg = 0
        WarningMsg = 1
        ErrorMsg = 2
        ReportMsg = 3
    End Enum

    Public Class Message
        Private _MsgClass As MessageClass

        Public Property MsgClass As MessageClass
            Get
                Return _MsgClass
            End Get
            Set
                _MsgClass = value
            End Set
        End Property

        Private _MsgText As String

        Public Property MsgText As String
            Get
                Return _MsgText
            End Get
            Set
                _MsgText = value
            End Set
        End Property

        Private _MsgDate As Date

        Public Property MsgDate As Date
            Get
                Return _MsgDate
            End Get
            Set
                _MsgDate = value
            End Set
        End Property
    End Class
End Namespace