Namespace Connection
    Namespace Protocol
        Public Class HTTPS
            Inherits Connection.Protocol.HTTPBase

            Public Sub New(ByVal RenderingEngine As RenderingEngine)
                MyBase.New(RenderingEngine)
            End Sub

            Public Overrides Sub NewExtended()
                MyBase.NewExtended()

                httpOrS = "https"
                defaultPort = Defaults.Port
            End Sub

            Public Enum Defaults
                Port = 443
            End Enum
        End Class
    End Namespace
End Namespace
