namespace mRemoteNG.Security.KeyDerivation
{
    public enum KeyDerivationAlgorithms
    {
        /// <summary>
        /// PBKDF2-HMAC-SHA1 over the low byte of each password character. Used by every file written
        /// before the algorithm was recorded, so it is what a missing KdfAlgorithm attribute means.
        /// </summary>
        Pbkdf2Sha1Legacy,

        /// <summary>
        /// PBKDF2-HMAC-SHA256 over the UTF-8 bytes of the password.
        /// </summary>
        Pbkdf2Sha256
    }

    public static class KeyDerivationDefaults
    {
        public const KeyDerivationAlgorithms Algorithm = KeyDerivationAlgorithms.Pbkdf2Sha256;

        // OWASP 2023 recommendation for PBKDF2-HMAC-SHA256
        public const int MinimumIterations = 600_000;
    }
}
