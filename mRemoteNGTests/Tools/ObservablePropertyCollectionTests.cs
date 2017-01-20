using mRemoteNG.Credential;
using mRemoteNG.Tools;
using NUnit.Framework;


namespace mRemoteNGTests.Tools
{
    public class ObservablePropertyCollectionTests
    {
        private ObservablePropertyCollection<CredentialRecord> _collection;

        [SetUp]
        public void Setup()
        {
            _collection = new ObservablePropertyCollection<CredentialRecord>();
        }

        [Test]
        public void WrapsAnExistingList()
        {
            _collection = new ObservablePropertyCollection<CredentialRecord>(new[] {new CredentialRecord()});
            Assert.That(_collection.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddHooksIntoMemberProperty()
        {
            var wasRaised = false;
            _collection.PropertyChanged += (sender, args) => { wasRaised = true; };
            var newItem = new CredentialRecord();
            _collection.Add(newItem);
            newItem.Title = "something";
            Assert.That(wasRaised);
        }

        [Test]
        public void CanClearList()
        {
            var newItem = new CredentialRecord();
            _collection.Add(newItem);
            _collection.Clear();
            Assert.That(_collection.Count, Is.EqualTo(0));
        }

        [Test]
        public void ContainsTrue()
        {
            var newItem = new CredentialRecord();
            _collection.Add(newItem);
            Assert.That(_collection.Contains(newItem), Is.True);
        }

        [Test]
        public void ContainsFalse()
        {
            var newItem = new CredentialRecord();
            _collection.Add(newItem);
            Assert.That(_collection.Contains(new CredentialRecord()), Is.False);
        }
    }
}