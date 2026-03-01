using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using NUnit.Framework;
using CategoryAttribute = System.ComponentModel.CategoryAttribute;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class ColorPropertyAttributeTests
    {
        [Test]
        public void ColorPropertyHasTabColorConverter()
        {
            // Get the Color property
            var propertyInfo = typeof(ConnectionInfo).GetProperty("Color");
            Assert.That(propertyInfo, Is.Not.Null, "Color property should exist");

            // Get the TypeConverter attribute
            var typeConverterAttr = propertyInfo.GetCustomAttributes(typeof(TypeConverterAttribute), true)
                .FirstOrDefault() as TypeConverterAttribute;

            Assert.That(typeConverterAttr, Is.Not.Null, "Color property should have TypeConverter attribute");
            Assert.That(typeConverterAttr.ConverterTypeName, Does.Contain("TabColorConverter"), 
                "Color property should use TabColorConverter");
        }

        [Test]
        public void TabColorPropertyHasTabColorConverter()
        {
            // Get the TabColor property
            var propertyInfo = typeof(ConnectionInfo).GetProperty("TabColor");
            Assert.That(propertyInfo, Is.Not.Null, "TabColor property should exist");

            // Get the TypeConverter attribute
            var typeConverterAttr = propertyInfo.GetCustomAttributes(typeof(TypeConverterAttribute), true)
                .FirstOrDefault() as TypeConverterAttribute;

            Assert.That(typeConverterAttr, Is.Not.Null, "TabColor property should have TypeConverter attribute");
            Assert.That(typeConverterAttr.ConverterTypeName, Does.Contain("TabColorConverter"),
                "TabColor property should use TabColorConverter");
        }

        [Test]
        public void ColorPropertyHasCategoryAttribute()
        {
            var propertyInfo = typeof(ConnectionInfo).GetProperty("Color");
            Assert.That(propertyInfo, Is.Not.Null);

            var categoryAttr = propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), true)
                .FirstOrDefault() as CategoryAttribute;

            Assert.That(categoryAttr, Is.Not.Null, "Color property should have Category attribute");
        }

        [Test]
        public void TabColorPropertyHasCategoryAttribute()
        {
            var propertyInfo = typeof(ConnectionInfo).GetProperty("TabColor");
            Assert.That(propertyInfo, Is.Not.Null);

            var categoryAttr = propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), true)
                .FirstOrDefault() as CategoryAttribute;

            Assert.That(categoryAttr, Is.Not.Null, "TabColor property should have Category attribute");
        }

        [Test]
        public void ColorInheritancePropertyHasCategoryAttribute()
        {
            var propertyInfo = typeof(ConnectionInfoInheritance).GetProperty("Color");
            Assert.That(propertyInfo, Is.Not.Null);

            var categoryAttr = propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), true)
                .FirstOrDefault() as CategoryAttribute;

            Assert.That(categoryAttr, Is.Not.Null, "Color inheritance property should have Category attribute");
        }

        [Test]
        public void TabColorInheritancePropertyHasCategoryAttribute()
        {
            var propertyInfo = typeof(ConnectionInfoInheritance).GetProperty("TabColor");
            Assert.That(propertyInfo, Is.Not.Null);

            var categoryAttr = propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), true)
                .FirstOrDefault() as CategoryAttribute;

            Assert.That(categoryAttr, Is.Not.Null, "TabColor inheritance property should have Category attribute");
        }
    }
}
