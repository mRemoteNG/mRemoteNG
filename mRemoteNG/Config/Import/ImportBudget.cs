using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Import
{
    public sealed class ImportBudget
    {
        public const int MaximumNodes = 10_000;
        public const int MaximumDepth = 128;
        public const int MaximumFieldCharacters = 65_536;
        private int _nodes;
        private int _depth;

        public void ConsumeNode()
        {
            if (++_nodes > MaximumNodes)
                throw new InvalidDataException($"Import exceeds the {MaximumNodes} node limit.");
        }

        public IDisposable EnterDepth()
        {
            if (++_depth > MaximumDepth)
            {
                _depth--;
                throw new InvalidDataException($"Import exceeds the {MaximumDepth} container depth limit.");
            }
            return new DepthScope(this);
        }

        public static void ValidateTree(ConnectionTreeModel model, CancellationToken cancellationToken = default)
        {
            if (model == null) throw new InvalidDataException("Importer returned no connection tree.");
            if (!cancellationToken.CanBeCanceled) cancellationToken = ImportCancellationContext.Current;
            cancellationToken.ThrowIfCancellationRequested();
            Stack<(ConnectionInfo Node, int Depth)> pending = new();
            foreach (ConnectionInfo root in model.RootNodes)
                pending.Push((root, 0));

            int nodes = 0;
            while (pending.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                (ConnectionInfo node, int depth) = pending.Pop();
                if (++nodes > MaximumNodes)
                    throw new InvalidDataException($"Import exceeds the {MaximumNodes} node limit.");
                if (depth > MaximumDepth)
                    throw new InvalidDataException($"Import exceeds the {MaximumDepth} container depth limit.");
                ValidateField(node.Name);
                ValidateField(node.Description);
                ValidateField(node.Hostname);
                ValidateField(node.Username);
                if (node is ContainerInfo container)
                    foreach (ConnectionInfo child in container.Children)
                        pending.Push((child, depth + 1));
            }
        }

        private static void ValidateField(string value)
        {
            if (value?.Length > MaximumFieldCharacters)
                throw new InvalidDataException($"Import contains a field longer than {MaximumFieldCharacters} characters.");
        }

        private sealed class DepthScope(ImportBudget budget) : IDisposable
        {
            private ImportBudget _budget = budget;
            public void Dispose()
            {
                ImportBudget currentBudget = _budget;
                _budget = null;
                if (currentBudget != null) currentBudget._depth--;
            }
        }
    }
}
