using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using mRemoteNG.Connection.Protocol;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class PuttyPasswordPipeTests
    {
        [Test]
        public void SecretPipeNameUsesWholeGuid()
        {
            string first = PuttyBase.CreateSecretPipeName();
            string second = PuttyBase.CreateSecretPipeName();

            Assert.That(first, Does.Match("^mRemoteNGSecretPipe[0-9a-f]{32}$"));
            Assert.That(second, Is.Not.EqualTo(first));
        }

        [Test]
        public async Task PasswordIsWrittenToTheClientThatOpensThePipe()
        {
            string pipeName = PuttyBase.CreateSecretPipeName();
            Task serving = PuttyBase.ServePasswordAsync(pipeName, "pass word", TimeSpan.FromSeconds(10));

            using (NamedPipeClientStream client = new(".", pipeName, PipeDirection.In))
            {
                client.Connect(5000);
                using StreamReader reader = new(client);
                Assert.That(await reader.ReadToEndAsync(), Is.EqualTo("pass word"));
            }

            Assert.That(await Task.WhenAny(serving, Task.Delay(5000)), Is.SameAs(serving));
        }

        [Test]
        public async Task ServingEndsWhenNoClientOpensThePipeInTime()
        {
            Task serving = PuttyBase.ServePasswordAsync(PuttyBase.CreateSecretPipeName(), "secret", TimeSpan.FromMilliseconds(200));

            Assert.That(await Task.WhenAny(serving, Task.Delay(5000)), Is.SameAs(serving));
        }
    }
}
