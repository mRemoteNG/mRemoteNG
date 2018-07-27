// The MIT License (MIT)
//
// Copyright(c) crdx
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// https://raw.githubusercontent.com/crdx/PortableSettingsProvider
//

using System;
using System.Collections;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
//using mRemoteNG.App;

namespace mRemoteNG.Config.Settings.Providers
{
    public class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string _rootNodeName = "settings";
        private const string _localSettingsNodeName = "localSettings";
        private const string _globalSettingsNodeName = "globalSettings";
        private const string _className = "PortableSettingsProvider";
        private XmlDocument _xmlDocument;

        private string _filePath => Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? throw new InvalidOperationException(), $"{ApplicationName}.settings");

        private XmlNode _localSettingsNode => GetSettingsNode(_localSettingsNodeName);

        private XmlNode _globalSettingsNode => GetSettingsNode(_globalSettingsNodeName);

        private XmlNode _rootNode => _rootDocument.SelectSingleNode(_rootNodeName);

        private XmlDocument _rootDocument
        {
            get
            {
                if (_xmlDocument != null) return _xmlDocument;
                try
                {
                    _xmlDocument = new XmlDocument();
                    _xmlDocument.Load(_filePath);
                }
                catch (Exception /*ex*/)
                {
                    // This casues hundreds of unit tests to fail for some reason...
                    //Runtime.MessageCollector.AddExceptionStackTrace("PortableSettingsProvider: Error getting XML", ex);
                }

                if (_xmlDocument?.SelectSingleNode(_rootNodeName) != null)
                    return _xmlDocument;

                _xmlDocument = GetBlankXmlDocument();

                return _xmlDocument;
            }
        }

        public override string ApplicationName
        {
            get => Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            set { }
        }

        public override string Name => _className;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(Name, config);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
                SetValue(propertyValue);

            try
            {
                _rootDocument.Save(_filePath);
            }
            catch (Exception)
            {
                /* 
                 * If this is a portable application and the device has been 
                 * removed then this will fail, so don't do anything. It's 
                 * probably better for the application to stop saving settings 
                 * rather than just crashing outright. Probably.
                 */
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            var values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in collection)
            {
                values.Add(new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                });
            }

            return values;
        }

        private void SetValue(SettingsPropertyValue propertyValue)
        {
            var targetNode = IsGlobal(propertyValue.Property) ? _globalSettingsNode : _localSettingsNode;

            var settingNode = targetNode.SelectSingleNode($"setting[@name='{propertyValue.Name}']");

            if (settingNode != null)
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            else
            {
                settingNode = _rootDocument.CreateElement("setting");

                var nameAttribute = _rootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;

                settingNode.Attributes?.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();

                targetNode.AppendChild(settingNode);
            }
        }

        private string GetValue(SettingsProperty property)
        {
            var targetNode = IsGlobal(property) ? _globalSettingsNode : _localSettingsNode;
            var settingNode = targetNode.SelectSingleNode($"setting[@name='{property.Name}']");

            if (settingNode == null)
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;

            return settingNode.InnerText;
        }

        private static bool IsGlobal(SettingsProperty property)
        {
            foreach (DictionaryEntry attribute in property.Attributes)
            {
                if ((Attribute)attribute.Value is SettingsManageabilityAttribute)
                    return true;
            }

            return false;
        }

        private XmlNode GetSettingsNode(string name)
        {
            var settingsNode = _rootNode.SelectSingleNode(name);

            if (settingsNode != null) return settingsNode;
            settingsNode = _rootDocument.CreateElement(name);
            _rootNode.AppendChild(settingsNode);

            return settingsNode;
        }

        private static XmlDocument GetBlankXmlDocument()
        {
            var blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(_rootNodeName));

            return blankXmlDocument;
        }

        public void Reset(SettingsContext context)
        {
            _localSettingsNode.RemoveAll();
            _globalSettingsNode.RemoveAll();

            _xmlDocument.Save(_filePath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }
    }
}