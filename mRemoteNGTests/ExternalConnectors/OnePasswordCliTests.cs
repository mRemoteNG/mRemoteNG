using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ExternalConnectors.OP;
using NUnit.Framework;

namespace mRemoteNGTests.ExternalConnectors;

public class OnePasswordCliTests
{
    private const string NotAReference = "not a reference";

    [Test]
    public void StandardErrorIsReadWhileTheCommandRuns()
    {
        // 5000 lines on stderr fill its pipe long before the command ends. Reading stdout to the end first
        // waits for an end that never comes, so the read runs on another thread and the test has a limit.
        Task<(int ExitCode, string Output, string Error)> run = Task.Run(() =>
        {
            int exitCode = OnePasswordCli.RunCommand("cmd.exe",
                ["/c", "for /l %i in (1,1,5000) do @echo 0123456789012345678901234567890123456789 1>&2"],
                out string output, out string error);
            return (exitCode, output, error);
        });

        Assert.That(run.Wait(TimeSpan.FromSeconds(60)), Is.True, "RunCommand did not return");
        Assert.Multiple(() =>
        {
            Assert.That(run.Result.ExitCode, Is.Zero);
            Assert.That(run.Result.Output, Is.Empty);
            Assert.That(run.Result.Error.Split('\n', StringSplitOptions.RemoveEmptyEntries), Has.Length.EqualTo(5000));
        });
    }

    [Test]
    public void CommandThatDoesNotFinishIsStopped()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        OnePasswordCliException ex = Assert.Throws<OnePasswordCliException>(() =>
            OnePasswordCli.RunCommand("cmd.exe", ["/c", "ping -n 30 127.0.0.1"], out _, out _, timeoutMilliseconds: 1000));

        Assert.Multiple(() =>
        {
            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromSeconds(20)));
            Assert.That(ex.Arguments, Is.EqualTo("cmd.exe /c ping -n 30 127.0.0.1"));
        });
    }

    [Test]
    public async Task ReadAheadThrowsWhatTheReadThrows()
    {
        OnePasswordRead readAhead = await OnePasswordCli.ReadPasswordAsync(NotAReference);
        string username = "from the connection", password = "from the connection", domain = "from the connection", privateKey = "";

        Assert.Throws<UriFormatException>(() => OnePasswordCli.ReadPassword(NotAReference, out _, out _, out _, out _));
        Assert.Throws<UriFormatException>(() =>
            OnePasswordCli.ReadPassword(readAhead, NotAReference, ref username, ref password, ref domain, ref privateKey));
        Assert.Multiple(() =>
        {
            Assert.That(username, Is.EqualTo("from the connection"));
            Assert.That(password, Is.EqualTo("from the connection"));
            Assert.That(domain, Is.EqualTo("from the connection"));
            Assert.That(privateKey, Is.Empty);
        });
    }

    [Test]
    public async Task ReadAheadOfAnotherReferenceIsNotUsed()
    {
        OnePasswordRead readAhead = await OnePasswordCli.ReadPasswordAsync(NotAReference);
        string username = "", password = "", domain = "", privateKey = "";

        // Reading null throws ArgumentNullException; the read ahead would throw UriFormatException.
        Assert.Throws<ArgumentNullException>(() =>
            OnePasswordCli.ReadPassword(readAhead, null, ref username, ref password, ref domain, ref privateKey));
    }
}
