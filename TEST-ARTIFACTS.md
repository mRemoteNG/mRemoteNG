# Test artifacts

`dotnet test` writes TRX results and collector attachments to `tools-mRemoteNG/<run>/dotnet/<project>/<framework>` under the test root. Coverlet MSBuild output uses its `coverage` subfolder. Build `bin` and `obj` stay unchanged.

The root is `TEST_ARTIFACTS_ROOT`, then the existing `TEST_OUTPUT_ROOT` alias. Local Windows defaults to `C:\data\tests`; CI and other platforms default to `artifacts/tests` in this checkout. Set `TEST_RUN_ID` to group a run, or let each evaluation create a unique ID. MSBuild callers can override `TestArtifactsRoot`, `TestRunId`, `VSTestResultsDirectory`, or `CoverletOutput` on the command line.

CI artifact upload steps must use the configured root. For a fixed CI path, set `TEST_ARTIFACTS_ROOT` and `TEST_RUN_ID` in the job. Existing explicit CLI output options take precedence.

Direct `vstest.console.exe` or NUnit console invocations bypass MSBuild. Pass their results/work-directory options explicitly under the same root. Visual Studio must use the project configuration or a selected runsettings file. Arbitrary external runner commands and shell redirection cannot be controlled by this project import.

## Disposable SQL persistence profile

Run `pwsh -File Tools/Test-SqlPersistence.ps1` from a Visual Studio Developer PowerShell with .NET 11, Podman and Windows OpenSSH installed. Start the local Podman machine first. The runner builds Release x64 using Visual Studio MSBuild; use `-NoBuild` only after a matching build. Results default to `C:\data\tests\tools-mRemoteNG\sql-persistence-<timestamp>`; `-ResultsDirectory` overrides that location.

The runner creates fresh SQL Server 2019 Developer and MySQL 8.4 containers, generates a synthetic password in process memory, and publishes random loopback-only ports. A temporary SSH tunnel handles Windows/WSL port forwarding without changing machine settings. Containers, their volumes and the tunnel are removed in `finally`; pre-existing containers and the Podman machine remain running.

`SqlPersistenceIntegrationTests` is opt-in and skipped during ordinary test runs. It requires the runner's `MREMOTENG_DISPOSABLE_SQL=1` profile, explicit loopback endpoints and a synthetic password. Each test creates a uniquely named `mremoteng_test_<guid>` database, then drops it. No saved connection factory or saved credentials are used. The tests temporarily change only in-memory SQL type/read-only/database-name options and restore them without saving settings.

The tests exercise production loading, saving and synchronization, including concurrent writers, late rollback, password rotation during authentication, revision overflow, unsupported writer protocols and draft retention. They bootstrap a new schema; they do not validate a production schema cutover or backward migration. Existing shared databases still require the coordinated schema 3.4 cutover described in the implementation plan; do not point this profile at them.
