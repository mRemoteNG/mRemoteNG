using System.Runtime.CompilerServices;

// Expose mRemoteNG internal types (notably the generated Options*Page settings
// classes, which are emitted as `internal sealed`) to the unit test assembly so
// tests can drive internal/UI-setting-dependent code paths.
[assembly: InternalsVisibleTo("mRemoteNGTests")]
