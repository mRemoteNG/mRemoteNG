using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config.DatabaseConnectors;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DatabaseConnectors
{
    [TestFixture]
    public class LiteDbRepositoryTests
    {
        private LiteDbRepository<TestDocument> _repo = null!;

        private class TestDocument
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            _repo = new LiteDbRepository<TestDocument>(":memory:", "test");
        }

        [TearDown]
        public void TearDown()
        {
            _repo.Dispose();
        }

        [Test]
        public void Constructor_NullConnectionString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LiteDbRepository<TestDocument>(null!, "col"));
        }

        [Test]
        public void Constructor_EmptyConnectionString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LiteDbRepository<TestDocument>("   ", "col"));
        }

        [Test]
        public void Constructor_NullCollectionName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LiteDbRepository<TestDocument>(":memory:", null!));
        }

        [Test]
        public void Count_EmptyCollection_ReturnsZero()
        {
            Assert.That(_repo.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Insert_SingleItem_CountIsOne()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "alpha", Value = 1 });
            Assert.That(_repo.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Insert_MultipleItems_CountMatchesInserted()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "a", Value = 1 });
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "b", Value = 2 });
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "c", Value = 3 });
            Assert.That(_repo.Count(), Is.EqualTo(3));
        }

        [Test]
        public void FindAll_AfterInsert_ReturnsAllItems()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "x", Value = 10 });
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "y", Value = 20 });

            List<TestDocument> results = _repo.FindAll().ToList();

            Assert.That(results, Has.Count.EqualTo(2));
        }

        [Test]
        public void FindAll_EmptyCollection_ReturnsEmptyEnumerable()
        {
            Assert.That(_repo.FindAll(), Is.Empty);
        }

        [Test]
        public void Find_MatchingPredicate_ReturnsMatchingItems()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "match", Value = 42 });
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "other", Value = 1 });

            List<TestDocument> results = _repo.Find(d => d.Value == 42).ToList();

            Assert.That(results, Has.Count.EqualTo(1));
            Assert.That(results[0].Name, Is.EqualTo("match"));
        }

        [Test]
        public void Find_NoMatchingPredicate_ReturnsEmptyEnumerable()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "item", Value = 5 });

            Assert.That(_repo.Find(d => d.Value == 999), Is.Empty);
        }

        [Test]
        public void Update_ExistingItem_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            _repo.Insert(new TestDocument { Id = id, Name = "before", Value = 0 });

            bool updated = _repo.Update(new TestDocument { Id = id, Name = "after", Value = 99 });

            Assert.That(updated, Is.True);
        }

        [Test]
        public void Update_ExistingItem_ChangesStoredValue()
        {
            var id = Guid.NewGuid();
            _repo.Insert(new TestDocument { Id = id, Name = "before", Value = 0 });
            _repo.Update(new TestDocument { Id = id, Name = "after", Value = 99 });

            List<TestDocument> found = _repo.Find(d => d.Id == id).ToList();

            Assert.That(found, Has.Count.EqualTo(1));
            Assert.That(found[0].Name, Is.EqualTo("after"));
            Assert.That(found[0].Value, Is.EqualTo(99));
        }

        [Test]
        public void Update_NonExistentItem_ReturnsFalse()
        {
            bool updated = _repo.Update(new TestDocument { Id = Guid.NewGuid(), Name = "ghost", Value = 0 });
            Assert.That(updated, Is.False);
        }

        [Test]
        public void DeleteMany_MatchingPredicate_RemovesDocuments()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "remove", Value = 1 });
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "remove", Value = 1 });
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "keep", Value = 2 });

            int deleted = _repo.DeleteMany(d => d.Name == "remove");

            Assert.That(deleted, Is.EqualTo(2));
            Assert.That(_repo.Count(), Is.EqualTo(1));
        }

        [Test]
        public void DeleteMany_NoMatch_ReturnsZero()
        {
            _repo.Insert(new TestDocument { Id = Guid.NewGuid(), Name = "item", Value = 1 });

            int deleted = _repo.DeleteMany(d => d.Name == "nonexistent");

            Assert.That(deleted, Is.EqualTo(0));
            Assert.That(_repo.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ImplementsILiteDbRepositoryInterface()
        {
            Assert.That(_repo, Is.InstanceOf<ILiteDbRepository<TestDocument>>());
        }

        [Test]
        public void ImplementsIDisposableInterface()
        {
            Assert.That(_repo, Is.InstanceOf<IDisposable>());
        }

        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            var repo = new LiteDbRepository<TestDocument>(":memory:", "col");
            repo.Dispose();
            Assert.DoesNotThrow(() => repo.Dispose());
        }
    }
}
