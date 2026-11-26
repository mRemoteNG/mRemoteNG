using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    /// <summary>
    /// Launches real processes through <see cref="ExternalTool"/> and checks the exact
    /// argument values they receive, for batch files (parsed by cmd.exe) and executables.
    /// </summary>
    [TestFixture]
    public class ExternalToolProcessArgumentsTests
    {
        private string _workDir;
        private string _batchFile;
        private string _psScript;
        private string _batchOutput;
        private string _exeOutput;
        private string _marker;

        [SetUp]
        public void Setup()
        {
            _workDir = Path.Combine(Path.GetTempPath(), "mRNG_ExtTool_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_workDir);
            _batchOutput = Path.Combine(_workDir, "batch_out.txt");
            _exeOutput = Path.Combine(_workDir, "exe_out.txt");
            _marker = Path.Combine(_workDir, "MARKER.txt");

            // Assigning inside quotes and printing with 'set' reads the values without cmd re-parsing them
            _batchFile = Path.Combine(_workDir, "args.cmd");
            File.WriteAllText(_batchFile,
                "@echo off\r\n" +
                "set \"A1=%~1\"\r\n" +
                "set \"A2=%~2\"\r\n" +
                $">\"{_batchOutput}\" set A\r\n");

            _psScript = Path.Combine(_workDir, "args.ps1");
            File.WriteAllText(_psScript,
                $"$args | ForEach-Object {{ \"[$_]\" }} | Set-Content -LiteralPath '{_exeOutput}' -Encoding UTF8\r\n");
        }

        [TearDown]
        public void Teardown()
        {
            try { Directory.Delete(_workDir, true); } catch (IOException) { } catch (UnauthorizedAccessException) { }
        }

        private Dictionary<string, string> RunBatch(string arguments, ConnectionInfo connection)
        {
            var tool = new ExternalTool { FileName = _batchFile, Arguments = arguments, WaitForExit = true };
            tool.Start(connection);
            return File.Exists(_batchOutput)
                ? File.ReadAllLines(_batchOutput)
                      .Where(l => l.StartsWith("A1=") || l.StartsWith("A2="))
                      .ToDictionary(l => l.Substring(0, 2), l => l.Substring(3))
                : new Dictionary<string, string>();
        }

        private string[] RunExe(string arguments, ConnectionInfo connection)
        {
            string powershell = Path.Combine(Environment.SystemDirectory, @"WindowsPowerShell\v1.0\powershell.exe");
            var tool = new ExternalTool
            {
                FileName = powershell,
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{_psScript}\" {arguments}",
                WaitForExit = true
            };
            tool.Start(connection);
            return File.Exists(_exeOutput) ? File.ReadAllLines(_exeOutput) : Array.Empty<string>();
        }

        [TestCase("1234,56789")]
        [TestCase("pa;ss")]
        [TestCase("a=b")]
        [TestCase("p&w")]
        [TestCase("p|w")]
        [TestCase("p<w>x")]
        [TestCase("p^w")]
        [TestCase("p%PATH%w")]
        [TestCase("100%")]
        [TestCase("p w")]
        [TestCase("plain")]
        public void BatchFileReceivesPasswordAsOneUnchangedArgument(string password)
        {
            var values = RunBatch("%PASSWORD%", new ConnectionInfo { Password = password });

            Assert.That(values.TryGetValue("A1", out var first) ? first : null, Is.EqualTo(password));
            Assert.That(values.ContainsKey("A2"), Is.False, "value was split into more than one argument");
        }

        [TestCase("1234,56789")]
        [TestCase("p w")]
        [TestCase("p&w")]
        public void BatchFileReceivesQuotedPasswordAsOneUnchangedArgument(string password)
        {
            // the documented way to keep a value together: quote the variable in the tool's arguments
            var values = RunBatch("\"%PASSWORD%\"", new ConnectionInfo { Password = password });

            Assert.That(values.TryGetValue("A1", out var first) ? first : null, Is.EqualTo(password));
            Assert.That(values.ContainsKey("A2"), Is.False, "value was split into more than one argument");
        }

        [TestCase("%DESCRIPTION%")]
        [TestCase("%-DESCRIPTION%")]
        [TestCase("\"%DESCRIPTION%\"")]
        [TestCase("\"%!DESCRIPTION%\"")]
        public void BatchFileVariablesStayInsideTheirQuotes(string arguments)
        {
            // A value containing quotes and & must stay one value for cmd.exe.
            // No spaces in the value, so argument splitting cannot accidentally break it up.
            Assume.That(_marker, Does.Not.Contain(" "), "temp path contains spaces");
            var connection = new ConnectionInfo { Description = $"x\"&mkdir {_marker}&\"y" };

            RunBatch(arguments, connection);

            Assert.That(Directory.Exists(_marker), Is.False, "part of the value was run by cmd.exe");
        }

        [TestCase("1234,56789")]
        [TestCase("p w")]
        [TestCase(@"C:\path\")]
        public void ExecutableReceivesQuotedPasswordAsOneUnchangedArgument(string password)
        {
            string[] received = RunExe("\"%PASSWORD%\"", new ConnectionInfo { Password = password });

            Assert.That(received, Is.EqualTo(new[] { $"[{password}]" }));
        }

        [TestCase("p&w")]
        [TestCase("p^w")]
        public void ExecutableReceivesUnescapedPasswordUnchanged(string password)
        {
            // %!NAME% turns off shell-metacharacter escaping, which only makes sense for cmd
            string[] received = RunExe("\"%!PASSWORD%\"", new ConnectionInfo { Password = password });

            Assert.That(received, Is.EqualTo(new[] { $"[{password}]" }));
        }
    }
}
