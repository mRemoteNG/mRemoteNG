using System;
using System.Runtime.Versioning;
using Microsoft.Win32;


namespace mRemoteNG.Tools.WindowsRegistry
{
    /// <summary>
    /// This class provides a convenient way to work with Windows Registry keys
    /// by encapsulating information about a registry key, including its path,
    /// name, value, and registry hive/type.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WindowsRegistryKey
    {
        #region properties

        #region Property registryHive
        public RegistryHive Hive
        {
            get { return _Hive; }
            set
            {
                if (value == 0)
                    throw new ArgumentNullException("RegistryHive unknown.");

                _Hive = value;
            }
        }
        private RegistryHive _Hive { get; set; }
        #endregion

        #region Property path
        public string Path
        {
            get { return _Path; }
            set
            {
                value.ThrowIfNull(nameof(value));
                _Path = value;
            }
        }
        private string _Path  { get; set; }
        #endregion

        #region Property name
        public string Name
        {
            get { return _Name; }
            set
            {
                value.ThrowIfNull(nameof(value));
                _Name = value;
            }
        }
        private string _Name { get; set; }
        #endregion

        #region Property valueKind
        public RegistryValueKind ValueKind
        {
            get { return _ValueKind; }
            set {
                if (value == 0)
                    throw new ArgumentNullException("ValueKind unknown.");

                _ValueKind = value;

                // Check if Type is uninitialized (null)
                if (_Type == null)
                    // Initialize Type if it's uninitialized
                    _Type = ConvertRegistryValueKindToType(value); 
            }
        }
        private RegistryValueKind _ValueKind;
        #endregion

        #region Property type
        public Type Type
        {
            get { return _Type; }
            set {
                _Type = value;
               
                // Check if ValueKind is uninitialized(0)
                if (_ValueKind == 0)
                    // Initialize ValueKind if it's uninitialized
                    _ValueKind = ConvertTypeToRegistryValueKind(value); 
            }
        }
        private Type _Type;
        #endregion

        public string Value { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// Checks if the Windows Registry key is ready for reading by ensuring that the hive,
        /// path, and name properties are set.
        /// </summary>
        /// <returns>True if the key is ready for reading, otherwise false.</returns>
        public bool IsKeyReadable() {
            return (IsHiveSet() && IsPathSet() && IsNameSet());
        }

        /// <summary>
        /// Checks if the Windows Registry key is ready for a write operation.
        /// The key is considered write-ready if none of the following conditions are met:
        /// - The hive is set
        /// - The registry value type is set
        /// - The key path is set
        /// - The value name is set
        /// </summary>
        /// <returns>Returns true if the key is write-ready, otherwise false.</returns>
        public bool IsKeyWritable() {
            return (IsHiveSet() && IsValueKindSet() && IsPathSet() && IsNameSet());
        }
        #endregion

        #region private methods
        /// <summary>
        /// Converts a .NET data type to the corresponding RegistryValueKind.
        /// </summary>
        /// <param name="valueType">The .NET data type to convert.</param>
        /// <returns>The corresponding RegistryValueKind.</returns>
        private RegistryValueKind ConvertTypeToRegistryValueKind(Type valueType)
        {
            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.String:
                    return RegistryValueKind.String;
                case TypeCode.Int32:
                    return RegistryValueKind.DWord;
                case TypeCode.Int64:
                    return RegistryValueKind.QWord;
                case TypeCode.Boolean:
                    return RegistryValueKind.DWord;
                case TypeCode.Byte:
                    return RegistryValueKind.Binary;
                /*
                case TypeCode.Single:
                    return RegistryValueKind;
                case TypeCode.Double:
                    return RegistryValueKind.String;
                case TypeCode.DateTime:
                    return RegistryValueKind.String; // DateTime can be stored as a string or other types
                case TypeCode.Char:
                    return RegistryValueKind.String; // Char can be stored as a string or other types
                case TypeCode.Decimal:
                     return RegistryValueKind.String; // Decimal can be stored as a string or other types
                */
                default:
                    return RegistryValueKind.String; // Default to String for unsupported types
            }
        }

        /// <summary>
        /// Converts a RegistryValueKind enumeration value to its corresponding .NET Type.
        /// </summary>
        /// <param name="valueKind">The RegistryValueKind value to be converted.</param>
        /// <returns>The .NET Type that corresponds to the given RegistryValueKind.</returns>
        private Type ConvertRegistryValueKindToType(RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return typeof(string);
                case RegistryValueKind.DWord:
                    return typeof(int);
                case RegistryValueKind.QWord:
                    return typeof(long);
                case RegistryValueKind.Binary:
                    return typeof(byte[]);
                case RegistryValueKind.MultiString:
                    return typeof(string[]);
                case RegistryValueKind.Unknown:
                default:
                    return typeof(object);
            }
        }

        private bool IsHiveSet()
        {
            return Hive != 0;
        }

        private bool IsValueKindSet()
        {
            return ValueKind != 0;
        }

        private bool IsPathSet()
        {
            return Path != null; ;
            //return !string.IsNullOrEmpty(Path);
        }

        private bool IsNameSet()
        {
            return Name != null;
        }

        private bool IsValueSet()
        {
            return !string.IsNullOrEmpty(Value);
        }
        #endregion
    }
}
