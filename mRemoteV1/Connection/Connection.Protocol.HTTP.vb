Namespace Connection

    Namespace Protocol
        Public Class HTTP
            Inherits HTTPBase

            Public Sub New(RenderingEngine As RenderingEngine)
                MyBase.New(RenderingEngine)
            End Sub

            Public Overrides Sub NewExtended()
                MyBase.NewExtended()

                httpOrS = "http"
                defaultPort = Defaults.Port
            End Sub

            Public Enum Defaults
                None = 0
                Port = 80
            End Enum
        End Class
    End Namespace

End Namespace
