using System;
using mRemoteNG.UI;
using NUnit.Framework;
using mRemoteNG.UI.Window;

namespace mRemoteNGTests.UI
{
    [TestFixture]
    public class WindowListTests
    {
        private WindowList _windowList;

        [SetUp]
        public void Setup()
        {
            _windowList = new WindowList();
        }

        [TearDown]
        public void Teardown()
        {
            _windowList = null;
        }

        [Test]
        public void EmptyWhenInitialized()
        {
            Assert.That(_windowList, Is.Empty);
        }

        [Test]
        public void WindowIsInListAfterBeingAdded()
        {
            BaseWindow window = new BaseWindow();
            _windowList.Add(window);
            Assert.That(_windowList, Has.Member(window));
        }

        [Test]
        public void WindowsAreInListAfterCallingAddRange()
        {
            BaseWindow window1 = new BaseWindow();
            BaseWindow window2 = new BaseWindow();
            BaseWindow window3 = new BaseWindow();
            BaseWindow[] windowArray = new BaseWindow[] {window1, window2, window3};
            _windowList.AddRange(windowArray);
            Assert.That(_windowList, Contains.Item(window1));
            Assert.That(_windowList, Contains.Item(window2));
            Assert.That(_windowList, Contains.Item(window3));
        }

        [Test]
        public void CountReturnsCorrectNumberOfElements()
        {
            _windowList.Add(new BaseWindow());
            _windowList.Add(new BaseWindow());
            _windowList.Add(new BaseWindow());
            Assert.That(_windowList.Count, Is.EqualTo(3));
        }

        [Test]
        public void IndexingByObjectReturnsTheCorrectObject()
        {
            BaseWindow window1 = new BaseWindow();
            BaseWindow window2 = new BaseWindow();
            _windowList.Add(window1);
            _windowList.Add(window2);
            Assert.That(_windowList[window1], Is.EqualTo(window1));
        }

        [Test]
        public void IndexingByNumberReturnsTheCorrectObject()
        {
            BaseWindow window1 = new BaseWindow();
            BaseWindow window2 = new BaseWindow();
            _windowList.Add(window1);
            _windowList.Add(window2);
            Assert.That(_windowList[1], Is.EqualTo(window2));
        }

        [Test]
        public void ThrowsExceptionWhenIndexingByObjectFails()
        {
            BaseWindow window1 = new BaseWindow();
            Assert.That(() => _windowList[window1], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ItemIsNotInListAfterBeingRemoved()
        {
            BaseWindow window1 = new BaseWindow();
            BaseWindow window2 = new BaseWindow();
            _windowList.Add(window1);
            _windowList.Add(window2);
            _windowList.Remove(window1);
            Assert.That(() => _windowList[window1], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ThrowsExceptionWhenIndexingPastListBounds()
        {
            BaseWindow window1 = new BaseWindow();
            _windowList.Add(window1);
            Assert.That(() => _windowList[100], Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CountIsReducedAfterItemRemoved()
        {
            BaseWindow window1 = new BaseWindow();
            BaseWindow window2 = new BaseWindow();
            _windowList.Add(window1);
            _windowList.Add(window2);
            _windowList.Remove(window1);
            Assert.That(_windowList.Count, Is.EqualTo(1));
        }

        [Test]
        public void ListIsEmptyAfterLastItemRemoved()
        {
            BaseWindow window1 = new BaseWindow();
            _windowList.Add(window1);
            _windowList.Remove(window1);
            Assert.That(_windowList, Is.Empty);
        }

        [Test]
        public void GetWindowFromStringReturnsCorrectObject()
        {
            BaseWindow window1 = new BaseWindow();
            window1.SetFormText("TheWindowWeWant");
            BaseWindow window2 = new BaseWindow();
            window2.SetFormText("WeDontWantThisWindow");
            _windowList.Add(window1);
            _windowList.Add(window2);
            Assert.That(_windowList.FromString("TheWindowWeWant"), Is.EqualTo(window1));
        }
    }
}