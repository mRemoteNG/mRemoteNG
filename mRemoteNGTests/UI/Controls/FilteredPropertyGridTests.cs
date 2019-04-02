using System;
using System.Linq;
using mRemoteNG.UI.Controls.FilteredPropertyGrid;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
	public class FilteredPropertyGridTests
	{
		[Test]
		public void AllPropertiesVisibleByDefault()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new {Prop1 = "hello"};
			grid.SelectedObject = obj;

			Assert.That(grid.VisibleProperties, Is.EquivalentTo(new []{ nameof(obj.Prop1) }));
		}

		[Test]
		public void PropertiesOnTheHiddenPropertiesListAreNotShown()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = "world" };
			grid.HiddenProperties = new[] { nameof(obj.Prop1) };
			grid.SelectedObject = obj;

			Assert.That(grid.VisibleProperties, Is.EquivalentTo(new[] { nameof(obj.Prop2) }));
		}

		[Test]
		public void OnlyPropertiesOnTheBrowsablePropertiesListAreShown()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = "world" };
			grid.BrowsableProperties = new[] { nameof(obj.Prop1) };
			grid.SelectedObject = obj;

			Assert.That(grid.VisibleProperties, Is.EquivalentTo(new[] { nameof(obj.Prop1) }));
		}

		[Test]
		public void APropertyOnBothTheBrowsableAndHiddenListWillNotBeShown()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = "world", Prop3 = "!" };
			grid.BrowsableProperties = new[] { nameof(obj.Prop1), nameof(obj.Prop2) };
			grid.HiddenProperties = new[] { nameof(obj.Prop1) };
			grid.SelectedObject = obj;

			Assert.That(grid.VisibleProperties, Is.EquivalentTo(new[] { nameof(obj.Prop2) }));
		}

		[Test]
		public void ExceptionThrownWhenNonExistantPropertyFoundInBrowsablePropertiesList()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello" };
			grid.SelectedObject = obj;

			Assert.Throws<InvalidOperationException>(() =>
				grid.BrowsableProperties = new[] {"NonExistantProperty"});
		}

		[Test]
		public void HiddenPropertiesListCanHandleNonExistentProperties()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello" };
			grid.SelectedObject = obj;

			Assert.DoesNotThrow(() => grid.HiddenProperties = new[] { "NonExistantProperty" });
		}

		[Test]
		public void GetVisibleGridItemsReturnsAllExpandedItems()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = new{Prop3 = "world"} };
			grid.SelectedObject = obj;

			var visibleGridItems = grid.GetVisibleGridItems();

			Assert.That(visibleGridItems.Select(i => i.Label), 
				Is.EquivalentTo(new[]
				{
					nameof(obj.Prop1),
					nameof(obj.Prop2)
				}));
		}

		[Test]
		public void CanSelectGridItem()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = "world" };
			grid.SelectedObject = obj;

			grid.SelectGridItem(nameof(obj.Prop2));

			Assert.That(grid.SelectedGridItem.PropertyDescriptor?.Name,
				Is.EqualTo(nameof(obj.Prop2)));
		}

		[Test]
		public void FindNextGridItemPropertyReturnsTheCorrectItem()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = "world" };
			grid.SelectedObject = obj;

			grid.SelectGridItem(nameof(obj.Prop1));
			var nextGridItem = grid.FindNextGridItemProperty(grid.SelectedGridItem);

			Assert.That(nextGridItem?.PropertyDescriptor?.Name,
				Is.EqualTo(nameof(obj.Prop2)));
		}

		[Test]
		public void FindPreviousGridItemPropertyReturnsTheCorrectItem()
		{
			var grid = new FilteredPropertyGrid();
			var obj = new { Prop1 = "hello", Prop2 = "world", Prop3 = "!" };
			grid.SelectedObject = obj;

			grid.SelectGridItem(nameof(obj.Prop3));
			var nextGridItem = grid.FindPreviousGridItemProperty(grid.SelectedGridItem);

			Assert.That(nextGridItem?.PropertyDescriptor?.Name,
				Is.EqualTo(nameof(obj.Prop2)));
		}
	}
}
