using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Provides path validation to prevent path traversal attacks
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class PathValidator
    {
        /// <summary>
        /// Validates that a file path does not contain path traversal sequences
        /// </summary>
        /// <param name="path">The path to validate</param>
        /// <returns>True if the path is safe, false otherwise</returns>
        public static bool IsValidPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // Check for path traversal sequences
            if (path.Contains("../", StringComparison.Ordinal) || path.Contains("..\\", StringComparison.Ordinal))
                return false;

            // Also check for encoded versions of path traversal
            if (path.Contains("%2e%2e", StringComparison.Ordinal) || path.Contains("%2E%2E", StringComparison.Ordinal))
                return false;

            return true;
        }

        /// <summary>
        /// Validates a file path and throws an exception if invalid
        /// </summary>
        /// <param name="path">The path to validate</param>
        /// <param name="parameterName">The name of the parameter being validated</param>
        /// <exception cref="ArgumentException">Thrown when the path contains path traversal sequences</exception>
        public static void ValidatePathOrThrow(string path, string parameterName = "path")
        {
            if (!IsValidPath(path))
                throw new ArgumentException("Invalid file path: path traversal sequences are not allowed", parameterName);
        }

        /// <summary>
        /// Validates that a file path is safe to execute and doesn't contain command injection characters
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <returns>True if the path is safe to execute, false otherwise</returns>
        public static bool IsValidExecutablePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            // First check basic path validation
            if (!IsValidPath(filePath))
                return false;

            // Check for shell metacharacters that could be used for command injection
            // These characters are dangerous when UseShellExecute is true
            char[] dangerousChars = ['&', '|', ';', '<', '>', '(', ')', '^', '\n', '\r'];
            if (filePath.Any(c => dangerousChars.Contains(c)))
                return false;

            // Check for multiple consecutive quotes which could be used to break out of quoting
            if (filePath.Contains("\"\"", StringComparison.Ordinal) || filePath.Contains("''", StringComparison.Ordinal))
                return false;

            try
            {
                // Validate that the path doesn't contain invalid path characters
                string fileName = Path.GetFileName(filePath);
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                // Check if path contains invalid characters
                char[] invalidChars = Path.GetInvalidPathChars();
                if (filePath.Any(c => invalidChars.Contains(c)))
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates an executable file path and throws an exception if invalid
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <param name="parameterName">The name of the parameter being validated</param>
        /// <exception cref="ArgumentException">Thrown when the path is not safe to execute</exception>
        public static void ValidateExecutablePathOrThrow(string filePath, string parameterName = "filePath")
        {
            if (!IsValidExecutablePath(filePath))
                throw new ArgumentException("Invalid executable path: path contains potentially dangerous characters or sequences", parameterName);
        }
    }
}
