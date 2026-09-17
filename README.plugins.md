# Plugin build and install workflow

This repository now separates the app solution from the plugin solution.

## Build the plugins

From the repository root:

```powershell
dotnet build mRemoteNG.Plugins.sln -c Release -p:Platform=x64
```

This builds the shared contract assembly and the plugin assemblies:

- `mRp.Contracts.dll`
- `mRp.PortScan.dll`
- `mRp.SshTransfer.dll`
- `mRp.AWS.dll`
- `mRp.MultiAddress.dll`

## Install into the app

After building, copy the generated plugin DLLs into the app's default plugin folder, while keeping the shared contracts assembly in the app's `Assemblies` folder:

```powershell
Copy-Item .\mRemoteNG.PluginContracts\bin\x64\Release\net10.0-windows10.0.26100.0\mRp.Contracts.dll .\mRemoteNG\bin\x64\Release\Assemblies\
Copy-Item .\mRemoteNG.Plugins.PortScan\bin\x64\Release\net10.0-windows10.0.26100.0\mRp.PortScan.dll .\mRemoteNG\bin\x64\Release\Plugins\
Copy-Item .\mRemoteNG.Plugins.SshTransfer\bin\x64\Release\net10.0-windows10.0.26100.0\mRp.SshTransfer.dll .\mRemoteNG\bin\x64\Release\Plugins\
Copy-Item .\mRemoteNG.Plugins.AWS\bin\x64\Release\net10.0-windows10.0.26100.0\mRp.AWS.dll .\mRemoteNG\bin\x64\Release\Plugins\
Copy-Item .\mRemoteNG.Plugins.MultiAddress\bin\x64\Release\net10.0-windows10.0.26100.0\mRp.MultiAddress.dll .\mRemoteNG\bin\x64\Release\Plugins\
```

The app looks for plugin DLLs in:

```text
<app output folder>\Plugins
```

If the app is configured with a custom plugin folder in settings, that folder is used instead.

## Important notes

- Keep `mRp.Contracts.dll` in the app's `Assemblies` directory so it resolves like the other runtime dependencies.
- Do not keep plugin projects inside the main `mRemoteNG.sln` unless you intentionally want the app solution to build them too.
- The app project does not reference the plugin projects directly anymore; it loads them from the plugin folder at runtime.
