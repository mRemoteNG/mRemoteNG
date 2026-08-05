using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
    public class ColorStringEditorTests
    {
        private ColorStringEditor _editor;

        /// <summary>
        /// Without an <see cref="System.Windows.Forms.Design.IWindowsFormsEditorService"/>
        /// the base <see cref="ColorEditor"/> shows no dialog and echoes the value it was
        /// given, which exercises the round trip without any UI.
        /// </summary>
        private sealed class EmptyServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType) => null;
        }

        [SetUp]
        public void Setup()
        {
            _editor = new ColorStringEditor();
        }

        [TestCase("Red")]
        [TestCase("#804020")]
        [TestCase("")]
        public void EditValueReturnsAString(string value)
        {
            var result = _editor.EditValue(null, new EmptyServiceProvider(), value);
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void EditValueOfNullDoesNotThrow()
        {
            Assert.DoesNotThrow(() => _editor.EditValue(null, new EmptyServiceProvider(), null));
        }

        [TestCase(nameof(ConnectionInfo.Color))]
        [TestCase(nameof(ConnectionInfo.TabColor))]
        public void ColorPropertiesUseTheStringAwareEditor(string propertyName)
        {
            var editor = TypeDescriptor.GetProperties(typeof(ConnectionInfo))[propertyName]
                                       ?.GetEditor(typeof(UITypeEditor));
            Assert.That(editor, Is.InstanceOf<ColorStringEditor>());
        }

        [Test]
        public void StandardValuesAreStringsSoTheDropDownCanBeCommitted()
        {
            var standardValues = new MiscTools.TabColorConverter().GetStandardValues(null);
            Assert.That(standardValues, Is.Not.Null);
            Assert.That(standardValues.Cast<object>(), Is.All.InstanceOf<string>());
        }
    }
}
