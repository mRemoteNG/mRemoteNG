using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Import
{
    public static class CredentialImportHelper
    {
        public static void ExtractCredentials(ConnectionInfo connection, ICredentialRepository repository)
        {
            // Check if this specific node has credentials to extract
            if (!string.IsNullOrEmpty(connection.Username) || 
                !string.IsNullOrEmpty(connection.Password) || 
                !string.IsNullOrEmpty(connection.Domain))
            {
                CredentialRecord record = new()
                {
                    Title = string.IsNullOrWhiteSpace(connection.Name) ? "Imported Credential" : connection.Name,
                    Username = connection.Username,
                    Password = connection.Password.ConvertToSecureString(),
                    Domain = connection.Domain
                };

                repository.CredentialRecords.Add(record);
                connection.CredentialId = record.Id.ToString();
                
                // Clear local credentials after extraction
                connection.Username = string.Empty;
                connection.Password = string.Empty;
                connection.Domain = string.Empty;
            }

            // Recurse into children if it's a container
            if (connection is ContainerInfo container)
            {
                foreach (ConnectionInfo child in container.Children)
                {
                    ExtractCredentials(child, repository);
                }
            }
        }

        public static bool HasCredentials(ConnectionInfo connection)
        {
            if (!string.IsNullOrEmpty(connection.Username) || 
                !string.IsNullOrEmpty(connection.Password) || 
                !string.IsNullOrEmpty(connection.Domain))
            {
                return true;
            }

            if (connection is ContainerInfo container)
            {
                return container.Children.Any(HasCredentials);
            }

            return false;
        }
    }
}
