using System;
using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.Container;

namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    internal class RegistryImporter : IConnectionImporter<string>
    {
        public void Import(string regPath, ContainerInfo destinationContainer)
        {
            Import(regPath, destinationContainer, false);
        }

        public static void Import(string regPath, ContainerInfo destinationContainer, bool noop = false)
        {
            try
            {
                ContainerInfo importedNode = new()
                {
                    Name = "Imported from PuTTY",
                    IsContainer = true
                };

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath))
                {
                    if (key != null)
                    {
                        foreach (string sub in key.GetSubKeyNames())
                        {
                            if (sub.EndsWith("Default%20Settings")) continue;
                            using RegistryKey subkey = key.OpenSubKey(sub);
                            string Hostname = subkey.GetValue("HostName") as string;
                            string connName = subkey.Name[(key.Name.Length + 1)..];
                            if (!string.IsNullOrEmpty(Hostname))
                            {
                                int Port = 22;
                                string Username = string.Empty;

                                string ProtocolType = subkey.GetValue("Protocol") as string;
                                Connection.Protocol.ProtocolType Protocol = Connection.Protocol.ProtocolType.SSH2;
                                if (ProtocolType == "raw")
                                {
                                    Protocol = Connection.Protocol.ProtocolType.RAW;
                                }

                                try
                                {
                                    Port = int.Parse(subkey.GetValue("PortNumber") as string);
                                }
                                catch { }
                                try
                                {
                                    Username = subkey.GetValue("UserName") as string;
                                }
                                catch { }

                                importedNode.AddChild(new Connection.ConnectionInfo()
                                {
                                    Name = connName,
                                    Hostname = Hostname,
                                    Port = Port == 0 ? 22 : Port,
                                    Protocol = Protocol,
                                    Parent = destinationContainer,
                                    Username = string.IsNullOrEmpty(Username) ? string.Empty : Username
                                });
                            }
                        }
                    }
                }

                destinationContainer.AddChild(importedNode);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Config.Import.Registry.Import() failed.", ex);
            }
        }

    }
}
