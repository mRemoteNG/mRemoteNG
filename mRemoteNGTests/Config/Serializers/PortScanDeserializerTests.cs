using System.Collections.Generic;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class PortScanDeserializerTests
    {
        private PortScanDeserializer _deserializer;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var host = new ScanHost("10.20.30.40");
            _deserializer = new PortScanDeserializer(new [] {host}, ProtocolType.SSH2);
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            _deserializer = null;
        }

        [Test]
        public void test()
        {
            
        }
    }
}