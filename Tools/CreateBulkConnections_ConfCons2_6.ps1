#####################################
# Authors: David Sparer & Jack Denton
# Summary:
#   This is intended to be a template for creating connections in bulk. This uses the serializers directly from the mRemoteNG binaries.
#   You will still need to create the connection info objects, but the library will handle serialization. It is expected that you
#   are familiar with PowerShell. If this is not the case, reach out to the mRemoteNG community for help.
# Usage:
#   Replace or modify the examples that are shown toward the end of the script to create your own connection info objects.
#####################################

foreach ($Path in 'HKLM:\SOFTWARE\WOW6432Node\mRemoteNG', 'HKLM:\SOFTWARE\WOW6432Node\mRemoteNG') {
    Try {
        $mRNGPath = (Get-ItemProperty -Path $Path -Name InstallDir -ErrorAction Stop).InstallDir
        break
    }
    Catch {
        continue
    }
}
$null = [System.Reflection.Assembly]::LoadFile((Join-Path -Path $mRNGPath -ChildPath "mRemoteNG.exe"))
Add-Type -Path (Join-Path -Path $mRNGPath -ChildPath "BouncyCastle.Crypto.dll")



function ConvertTo-mRNGSerializedXml {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory)]
        [mRemoteNG.Connection.ConnectionInfo[]]
        $Xml,

        [Parameter()]
        [pscredential]
        $Credential = (Get-Credential -Message 'Enter the encryption key you would like to use. This must match the encryption key used by the rest of the confCons file.' -UserName $ENV:USERNAME)
    )

    $CryptoProvider = [mRemoteNG.Security.SymmetricEncryption.AeadCryptographyProvider]::new()
    $SaveFilter = [mRemoteNG.Security.SaveFilter]::new()
    $ConnectionNodeSerializer = [mRemoteNG.Config.Serializers.Xml.XmlConnectionNodeSerializer26]::new($CryptoProvider, $Credential.Password, $SaveFilter)
    $XmlSerializer = [mRemoteNG.Config.Serializers.Xml.XmlConnectionsSerializer]::new($CryptoProvider, $ConnectionNodeSerializer)

    $RootNode = [mRemoteNG.Tree.Root.RootNodeInfo]::new('Connection')
    foreach ($Node in $Xml) {
        $RootNode.AddChild($Node)
    }
    $XmlSerializer.Serialize($RootNode)
}

function New-mRNGConnection {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory)]
        [string]
        $Name,

        [Parameter(Mandatory)]
        [string]
        $Hostname,

        [Parameter(Mandatory)]
        [mRemoteNG.Connection.Protocol.ProtocolType]
        $Protocol,

        [Parameter()]
        [switch]
        $InheritCredential,

        [Parameter()]
        [mRemoteNG.Container.ContainerInfo]
        $ParentContainer,

        [Parameter()]
        [switch]
        $PassThru
    )

    $Connection = [mRemoteNG.Connection.ConnectionInfo]@{
        Name     = $Name
        Hostname = $Hostname
        Protocol = $Protocol
    }

    if ($PSBoundParameters.ContainsKey('InheritCredential')) {
        $Connection.Inheritance.Username = $true
        $Connection.Inheritance.Domain = $true
        $Connection.Inheritance.Password = $true
    }

    if ($ParentContainer) {
        $ParentContainer.AddChild($Connection)

        if ($PSBoundParameters.ContainsKey('PassThru')) {
            $Connection
        }
    }
    else {
        $Connection
    }
}

function New-mRNGContainer {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory)]
        [string]
        $Name,

        [Parameter()]
        [switch]
        $InheritCredential,

        [Parameter()]
        [mRemoteNG.Container.ContainerInfo]
        $ParentContainer
    )

    $Container = [mRemoteNG.Container.ContainerInfo]@{
        Name = $Name
    }

    if ($PSBoundParameters.ContainsKey('InheritCredential')) {
        $Container.Inheritance.Username = $true
        $Container.Inheritance.Domain = $true
        $Container.Inheritance.Password = $true
    }

    if ($ParentContainer) {
        $ParentContainer.AddChild($Container)
    }
    
    $Container
}

function Export-mRNGXml {
    [CmdletBinding()]
    param (
        [Parameter()]
        [string]
        $Path,

        [Parameter()]
        [string]
        $SerializedXml
    )

    $FilePathProvider = [mRemoteNG.Config.DataProviders.FileDataProvider]::new($Path)
    $filePathProvider.Save($SerializedXml)
}




#----------------------------------------------------------------
# Example 1: serialize many connections, no containers
# Here you can define the number of connection info objects to create
# You can also provide a list of desired hostnames and iterate over those

$Connections = foreach ($i in 1..5) {
    # Create new connection
    $Splat = @{
        Name              = 'Server-{0:D2}' -f $i
        Hostname          = 'Server-{0:D2}' -f $i
        Protocol          = 'RDP'
        InheritCredential = $true
    }
    New-mRNGConnection @Splat
}

# Serialize the connections
$SerializedXml = ConvertTo-mRNGSerializedXml -Xml $Connections

# Write the XML to a file ready to import into mRemoteNG
Export-mRNGXml -Path "$ENV:APPDATA\mRemoteNG\PowerShellGenerated.xml" -SerializedXml $SerializedXml

# Now open up mRemoteNG and press Ctrl+O and open up the exported XML file




#----------------------------------------------------------------
# Example 2: serialize a container which has connections
# You can also create containers and add connections and containers to them, which will be nested correctly when serialized
# If you specify the ParentContainer parameter for new connections then there will be no output unless the PassThru parameter is also used

$ProdServers = New-mRNGContainer -Name 'ProdServers'

foreach ($i in 1..3) {
    # Create new connection
    $Splat = @{
        Name              = 'Server-{0:D2}' -f $i
        Hostname          = 'Server-{0:D2}' -f $i
        Protocol          = 'RDP'
        InheritCredential = $true
        ParentContainer   = $ProdServers
    }
    New-mRNGConnection @Splat
}

$ProdWebServers = New-mRNGContainer -Name 'WebServers' -ParentContainer $ProdServers

foreach ($i in 1..3) {
    # Create new connection
    $Splat = @{
        Name              = 'WebServer-{0:D2}' -f $i
        Hostname          = 'WebServer-{0:D2}' -f $i
        Protocol          = 'SSH1'
        InheritCredential = $true
        ParentContainer   = $ProdWebServers
    }
    New-mRNGConnection @Splat
}

$DevServers = New-mRNGContainer -Name 'DevServers'

foreach ($i in 1..3) {
    # Create new connection
    $Splat = @{
        Name              = 'DevServer-{0:D2}' -f $i
        Hostname          = 'DevServer-{0:D2}' -f $i
        Protocol          = 'RDP'
        InheritCredential = $true
        ParentContainer   = $DevServers
        PassThru          = $true
    }

    # Specified the PassThru parameter in order to catch the connection and change a property
    $Connection = New-mRNGConnection @Splat
    $Connection.Resolution = 'FullScreen'
}

# Serialize the container
$SerializedXml = ConvertTo-mRNGSerializedXml -Xml $ProdServers, $DevServers

# Write the XML to a file ready to import into mRemoteNG
Export-mRNGXml -Path "$ENV:APPDATA\mRemoteNG\PowerShellGenerated.xml" -SerializedXml $SerializedXml

# Now open up mRemoteNG and press Ctrl+O and open up the exported XML file
