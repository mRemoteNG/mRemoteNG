using System.Linq;
using System.Windows.Forms;
using mRemoteNG.UI.Controls.PageSequence;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    public class PageSequenceTests
    {
        private PageSequence _pageSequence;
        private Control _parentControl;

        [SetUp]
        public void Setup()
        {
            _parentControl = new Control();
        }

        [Test]
        public void PageListAddedToSequence()
        {
            var pages = new[] {new SequencedControl(), new SequencedControl(), new SequencedControl()};
            _pageSequence = new PageSequence(_parentControl, pages);
            Assert.That(_pageSequence.Pages, Is.EquivalentTo(pages));
        }

        [Test]
        public void PageParamsAddedToSequence()
        {
            _pageSequence = new PageSequence(_parentControl,
                new SequencedControl(),
                new SequencedControl(),
                new SequencedControl()
            );
            Assert.That(_pageSequence.Pages.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CallingNextAdvancesPage()
        {
            var pages = new[] { new SequencedControl(), new SequencedControl(), new SequencedControl() };
            _pageSequence = new PageSequence(_parentControl, pages);
            _pageSequence.NextPage();
            Assert.That(_pageSequence.CurrentPageIndex, Is.EqualTo(1));
        }

        [Test]
        public void CallingPreviousGoesBackAPage()
        {
            var pages = new[] { new SequencedControl(), new SequencedControl(), new SequencedControl() };
            _pageSequence = new PageSequence(_parentControl, pages);
            _pageSequence.NextPage();
            _pageSequence.NextPage();
            _pageSequence.PreviousPage();
            Assert.That(_pageSequence.CurrentPageIndex, Is.EqualTo(1));
        }
    }
}