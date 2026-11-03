using System.Collections.Generic;
using System.Runtime.Versioning;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Describes a single variable that can be used in external tool fields
    /// (filename, arguments, working directory). Used both by the argument
    /// parser and by the UI variable-completion helper so that the list of
    /// available variables stays in a single place.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class ExternalToolVariable(string name, string description)
    {
        /// <summary>
        /// The variable name without the surrounding percent signs (e.g. "Hostname").
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// A human readable, localized description of the variable.
        /// </summary>
        public string Description { get; } = description;

        /// <summary>
        /// The variable as it should be inserted into a text field, including the
        /// surrounding percent signs (e.g. "%Hostname%").
        /// </summary>
        public string Token => $"%{Name}%";

        /// <summary>
        /// The ordered list of connection variables that the external tool
        /// argument parser understands.
        /// </summary>
        public static IReadOnlyList<ExternalToolVariable> SupportedVariables { get; } =
        [
            new ExternalToolVariable("Name", Language.Name),
            new ExternalToolVariable("Hostname", Language.Hostname),
            new ExternalToolVariable("Port", Language.Port),
            new ExternalToolVariable("Username", Language.Username),
            new ExternalToolVariable("Password", Language.Password),
            new ExternalToolVariable("Domain", Language.Domain),
            new ExternalToolVariable("Description", Language.Description),
            new ExternalToolVariable("MacAddress", Language.MacAddress),
            new ExternalToolVariable("UserField", Language.UserField)
        ];
    }
}
