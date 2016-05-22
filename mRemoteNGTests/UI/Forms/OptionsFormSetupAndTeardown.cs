using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Forms
{
    public class OptionsFormSetupAndTeardown
    {
        protected frmOptions _optionsForm;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            Runtime.MessageCollector = new MessageCollector(new ErrorAndInfoWindow(new DockContent()));
        }

        [SetUp]
        public void Setup()
        {
            _optionsForm = new frmOptions();
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