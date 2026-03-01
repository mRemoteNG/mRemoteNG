// Parallel test execution is handled at the PROCESS level, not the NUnit level.
// The production code uses shared mutable singletons (DefaultConnectionInheritance.Instance,
// Runtime.ConnectionsService, Runtime.EncryptionKey) that are not thread-safe.
// Instead of NUnit fixture-level parallelism, run-tests.ps1 launches multiple dotnet test
// processes in parallel, each with a namespace filter. Each process has isolated static state.
//
// DO NOT add [assembly: Parallelizable] here - it causes race conditions on shared state.
