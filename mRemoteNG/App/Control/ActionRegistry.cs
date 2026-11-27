using System;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.App.AgentControl
{
    /// <summary>Allowlist for the initial read-only control surface.</summary>
    internal sealed class ActionRegistry
    {
        private static readonly ControlActionDescriptor[] Actions =
        [
            new(ControlActionNames.ActionsList, "actions.read", true),
            new(ControlActionNames.ConnectionsList, "connections.read", true),
            new(ControlActionNames.ActionsPreview, "actions.preview", true),
            new(ControlActionNames.OperationsGet, "operations.read", true)
        ];

        public IReadOnlyList<ControlActionDescriptor> List() => Array.AsReadOnly(Actions);

        public bool TryGet(string name, out ControlActionDescriptor descriptor)
        {
            descriptor = Actions.FirstOrDefault(action => string.Equals(action.Name, name, StringComparison.Ordinal));
            return descriptor != null;
        }
    }
}
