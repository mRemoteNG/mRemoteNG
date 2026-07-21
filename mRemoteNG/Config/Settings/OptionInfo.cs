using System;

namespace mRemoteNG.Config.Settings
{
    /// <summary>
    /// Represents a dev-only option that can be managed at runtime.
    /// Options can store application settings, user preferences, and plugin configuration.
    /// </summary>
    public class OptionInfo
    {
        /// <summary>
        /// Unique identifier for the option.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The key/name of the option (must be unique).
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value of the option.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Optional category to group related options.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Optional description of what this option does.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of the option (e.g., "string", "int", "bool").
        /// </summary>
        public string OptionType { get; set; }

        /// <summary>
        /// Timestamp when the option was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Timestamp when the option was last modified.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public OptionInfo()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            OptionType = "string";
        }

        public override string ToString()
        {
            return $"{Key} = {Value}";
        }

        public override bool Equals(object obj)
        {
            if (obj is not OptionInfo other)
                return false;
            return Id == other.Id && Key == other.Key;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Key);
        }
    }
}
