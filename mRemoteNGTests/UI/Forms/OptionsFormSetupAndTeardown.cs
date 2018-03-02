using mRemoteNG.Connection;
using mRemoteNG.UI.Forms;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms
{
	public class OptionsFormSetupAndTeardown
    {
        protected frmOptions _optionsForm;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
	        var connectionInitiator = Substitute.For<IConnectionInitiator>();
			_optionsForm = new frmOptions(connectionInitiator, type => {});
            _optionsForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _optionsForm.Dispose();
            while (_optionsForm.Disposing) ;
            _optionsForm = null;
        }
    }
}