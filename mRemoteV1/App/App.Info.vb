Imports System.Environment

Namespace App
    Namespace Info
        Public Class General
            Public Shared ReadOnly URLHome As String = "http://www.mRemote.org/"
            Public Shared ReadOnly URLDonate As String = "https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=felix%2edeimel%40gmail%2ecom&item_name=mRemote%20Donations&buyer_credit_promo_code=&buyer_credit_product_category=&buyer_credit_shipping_method=&buyer_credit_user_address_change=&no_shipping=0&no_note=1&tax=0&currency_code=EUR&lc=US&bn=PP%2dDonationsBF&charset=UTF%2d8"
            Public Shared ReadOnly URLBugs As String = "http://bugs.mremote.org/"
            Public Shared ReadOnly URLAnnouncement As String = "http://update.mRemote.org/mRemote_Announcment.txt"
            Public Shared ReadOnly LogFile As String = My.Application.Info.DirectoryPath & "\mRemote.log"
            Public Shared ReadOnly HomePath As String = My.Application.Info.DirectoryPath
            Public Shared EncryptionKey As String = "mR3m"
            Public Shared ReportingFilePath As String = ""
            Public Shared SmartCodeURL As String = "http://www.s-code.com/products/viewerx/"
            Public Shared FamFamFamURL As String = "http://www.famfamfam.com/"
        End Class

        Public Class Settings
            'Exchange to make portable/normal
            Public Shared ReadOnly SettingsPath As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\" & My.Application.Info.ProductName
            'Public Shared ReadOnly SettingsPath As String = My.Application.Info.DirectoryPath

            Public Shared ReadOnly LayoutFileName As String = "pnlLayout.xml"
            Public Shared ReadOnly ExtAppsFilesName As String = "extApps.xml"
        End Class

        Public Class Update
            Public Shared ReadOnly URL As String = "http://update.mRemote.org/"
#If DEBUG Then
            Public Shared ReadOnly File As String = "mRemote_Update_Debug.txt"
#Else
            Public Shared ReadOnly File As String = "mRemote_Update.txt"
#End If
        End Class

        Public Class Connections
            Public Shared ReadOnly DefaultConnectionsPath As String = App.Info.Settings.SettingsPath
            Public Shared ReadOnly DefaultConnectionsFile As String = "confCons.xml"
            Public Shared ReadOnly DefaultConnectionsFileNew As String = "confConsNew.xml"
            Public Shared ReadOnly ConnectionFileVersion As Double = 2.1
        End Class

        Public Class Credentials
            Public Shared ReadOnly CredentialsPath As String = App.Info.Settings.SettingsPath
            Public Shared ReadOnly CredentialsFile As String = "confCreds.xml"
            Public Shared ReadOnly CredentialsFileNew As String = "confCredsNew.xml"
            Public Shared ReadOnly CredentialsFileVersion As Double = 1.0
        End Class
    End Namespace
End Namespace
