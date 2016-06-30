using System.Collections.Specialized;
using mRemoteNG.Credential;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    [TestFixture]
    public class CredentialListTests
    {
        private CredentialList _credentialList;
        private CredentialInfo _testCredential1;
        private CredentialInfo _testCredential2;
        private CredentialInfo _testCredential3;

        [SetUp]
        public void Setup()
        {
            _credentialList = new CredentialList();
            PrepareTestCredentials();
        }

        [TearDown]
        public void Teardown()
        {
            _credentialList = null;
            _testCredential1 = null;
            _testCredential2 = null;
            _testCredential3 = null;
        }

        private void PrepareTestCredentials()
        {
            _testCredential1 = new CredentialInfo { Name = "TestCred1", Username = "HoopyFrood", Domain = "Betelgeuse" };
            _testCredential2 = new CredentialInfo { Name = "TestCred2", Username = "adent" };
            _testCredential3 = new CredentialInfo { Name = "TestCred3", Username = "Marvin", Domain = "whocares" };
        }

        [Test]
        public void CredentialListIsEmptyWhenInitialized()
        {
            Assert.That(_credentialList, Is.Empty);
        }

        [Test]
        public void CredentialListItemPresentAfterCallingAdd()
        {
            _credentialList.Add(_testCredential1);
            Assert.That(_credentialList, Has.Member(_testCredential1));
        }

        [Test]
        public void AllCredentialListItemsPresentAfterCallingAddRange()
        {
            _credentialList.AddRange(new[] { _testCredential1, _testCredential2 });
            Assert.That(_credentialList, Has.Member(_testCredential1).And.Member(_testCredential2));
        }

        [Test]
        public void CredentialListEmptyAfterClearing()
        {
            _credentialList.AddRange(new [] {_testCredential1, _testCredential2, _testCredential3});
            _credentialList.Clear();
            Assert.That(_credentialList, Is.Empty);
        }

        [Test]
        public void CredentialListContainsTrueIfItemPresent()
        {
            _credentialList.Add(_testCredential1);
            Assert.That(_credentialList.Contains(_testCredential1), Is.True);
        }

        [Test]
        public void CredentialListContainsFalseIfItemNotPresent()
        {
            _credentialList.Add(_testCredential3);
            Assert.That(_credentialList.Contains(_testCredential1), Is.False);
        }

        [Test]
        public void CredentialListContainsFalseIfListIsEmpty()
        {
            Assert.That(_credentialList.Contains(_testCredential1), Is.False);
        }

        [Test]
        public void CopyingCredentialListCreatesExactCopy()
        {
            _credentialList.Add(_testCredential2);
            _credentialList.Add(_testCredential3);
            var tempCredentialList = _credentialList.Copy();
            Assert.That(tempCredentialList, Is.EquivalentTo(_credentialList));
        }

        [Test]
        public void CredentialListItemNotPresentAfterCallingRemove()
        {
            _credentialList.Add(_testCredential1);
            _credentialList.Remove(_testCredential1);
            Assert.That(_credentialList, Has.No.Member(_testCredential1));
        }

        [Test]
        public void CallingRemoveOnEmptyCredentialListDoesNothing()
        {
            _credentialList.Remove(_testCredential1);
            Assert.That(_credentialList, Is.Empty);
        }

        [Test]
        public void CredentialListItemIsUpdatedAfterCallingReplace()
        {
            var newDescription = "testing stuff";
            _credentialList.Add(_testCredential3);
            _testCredential3.Description = newDescription;
            _credentialList.Replace(_testCredential3);
            Assert.That(_credentialList[_testCredential3].Description, Is.EqualTo(newDescription));
        }

        [Test]
        public void CredentialListItemInSamePositionAfterCallingReplace()
        {
            var newDescription = "testing stuff";
            _credentialList.Add(_testCredential1);
            _credentialList.Add(_testCredential2);
            _credentialList.Add(_testCredential3);
            _testCredential2.Description = newDescription;
            _credentialList.Replace(_testCredential2);
            Assert.That(_credentialList[1].Description, Is.EqualTo(newDescription));
        }

        [Test]
        public void CollectionChangedEventRaisedOnAdd()
        {
            var wasCalled = false;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    wasCalled = true;
            };
            _credentialList.Add(_testCredential1);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void CollectionChangedEventRaisedOnAddRange()
        {
            var wasCalled = false;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    wasCalled = true;
            };
            _credentialList.AddRange(new[] { _testCredential1 });
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void CollectionChangedEventRaisedForEachItemInAddRange()
        {
            var eventRaisedCount = 0;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                    eventRaisedCount++;
            };
            _credentialList.AddRange(new[] { _testCredential1, _testCredential2, _testCredential3 });
            Assert.That(eventRaisedCount, Is.EqualTo(3));
        }

        [Test]
        public void CollectionChangedEventRaisedOnClear()
        {
            var wasCalled = false;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                    wasCalled = true;
            };
            _credentialList.Clear();
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void CollectionChangedEventRaisedOnRemove()
        {
            var wasCalled = false;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                    wasCalled = true;
            };
            _credentialList.Add(_testCredential1);
            _credentialList.Remove(_testCredential1);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void CollectionChangedEventNotRaisedWhenThereIsNoItemToRemove()
        {
            var wasCalled = false;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                    wasCalled = true;
            };
            _credentialList.Remove(_testCredential1);
            Assert.That(wasCalled, Is.False);
        }

        [Test]
        public void CollectionChangedEventRaisedOnReplace()
        {
            var wasCalled = false;
            _credentialList.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Replace)
                    wasCalled = true;
            };
            _credentialList.Add(_testCredential1);
            _credentialList.Replace(_testCredential1);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void CountReturnsCorrectNumberOfListElements()
        {
            var list = new[] {_testCredential1, _testCredential2};
            _credentialList.AddRange(list);
            Assert.That(_credentialList.Count, Is.EqualTo(list.Length));
        }
    }
}