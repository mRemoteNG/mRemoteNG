using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsSQLPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void SQLPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[6].Text, Does.Match("SQL Server"));
        }

        [Test]
        public void SQLIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[6].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingSQLPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("SQL Server");
            CheckBox checkboxTester = _optionsForm.FindControl<CheckBox>("chkUseSQLServer");
            Assert.That(checkboxTester.Text, Does.Match("Use SQL"));
        }
    }
}