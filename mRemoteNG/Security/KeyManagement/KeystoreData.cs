using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mRemoteNG.Security.KeyManagement
{
    /// <summary>
    /// Root data structure persisted in the keystore JSON file.
    /// Contains KDF parameters and one or more wrapped DEK entries.
    /// </summary>
    public sealed class KeystoreData
    {
        [JsonPropertyName("version")]
        public int Version { get; set; } = 1;

        [JsonPropertyName("kdfAlgorithm")]
        public string KdfAlgorithm { get; set; } = "PBKDF2-SHA256";

        [JsonPropertyName("kdfIterations")]
        public int KdfIterations { get; set; } = 600_000;

        [JsonPropertyName("wrappedKeys")]
        public List<WrappedKeyEntry> WrappedKeys { get; set; } = [];
    }

    /// <summary>
    /// A single wrapped DEK entry, created by a specific <see cref="IMasterKeyProvider"/>.
    /// </summary>
    public sealed class WrappedKeyEntry
    {
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; } = "password";

        [JsonPropertyName("salt")]
        public string Salt { get; set; }

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        [JsonPropertyName("wrappedDek")]
        public string WrappedDek { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}
