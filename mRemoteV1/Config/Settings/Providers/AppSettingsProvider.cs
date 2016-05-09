#if false
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;


namespace mRemoteNG.Config.Settings.Providers
{
	public class AppSettingsProvider : SettingsProvider
	{
        const string SETTINGSROOT = "Settings"; //XML Root Node

		public override void Initialize(string name, NameValueCollection col)
		{
            if (Application.ProductName.Trim().Length > 0)
            {
                _applicationName = Application.ProductName;
            }
            else
            {
                _applicationName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            }

            base.Initialize(ApplicationName, col);

            /*
            if (!File.Exists(GetDotSettingsFile()))
            {
                // do something smart.
            }
            */
        }

	    private string _applicationName;
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { }
        }

        public virtual string GetDotSettingsFile()
        {
            return Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());
        }


        public virtual string GetAppSettingsPath()
		{
            //Used to determine where to store the settings
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
		}
		
		public virtual string GetAppSettingsFilename()
		{
			//Used to determine the filename to store the settings
			return Application.ProductName + ".settings";
		}
		
		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
		{
			//Iterate through the settings to be stored
			//Only dirty settings are included in propvals, and only ones relevant to this provider
			foreach (SettingsPropertyValue propval in propvals)
			{
				SetValue(propval);
			}
						
			try
			{
				SettingsXml.Save(GetDotSettingsFile());
			}
			catch (Exception)
			{
				//Ignore if cant save, device been ejected
			}
		}
		
		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
		{
			//Create new collection of values
			var values = new SettingsPropertyValueCollection();
						
			//Iterate through the settings to be retrieved
			foreach (SettingsProperty setting in props)
			{

			    var value = new SettingsPropertyValue(setting)
			    {
			        IsDirty = false,
			        SerializedValue = GetValue(setting)
			    };
			    values.Add(value);
			}
			return values;
		}
		
		private XmlDocument _SettingsXml;
		
        private XmlDocument SettingsXml
		{
			get
			{
				//If we dont hold an xml document, try opening one.
				//If it doesnt exist then create a new one ready.
				if (_SettingsXml == null)
				{
					_SettingsXml = new XmlDocument();
								
					try
					{
						_SettingsXml.Load(GetDotSettingsFile());
					}
					catch (Exception)
					{
						//Create new document
						var dec = _SettingsXml.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
						_SettingsXml.AppendChild(dec);

					    var nodeRoot = _SettingsXml.CreateNode(XmlNodeType.Element, SETTINGSROOT, "");
						_SettingsXml.AppendChild(nodeRoot);
					}
				}
				return _SettingsXml;
			}
		}
		
		private string GetValue(SettingsProperty setting)
		{
			var ret = string.Empty;
						
			try
			{
				if (IsRoaming(setting))
				{
					ret = SettingsXml.SelectSingleNode(SETTINGSROOT + "/" + setting.Name).InnerText;
				}
				else
				{
					ret = SettingsXml.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName + "/" + setting.Name).InnerText;
				}
			}
			catch (Exception)
			{
				ret = setting.DefaultValue?.ToString() ?? "";
			}
			return ret;
		}
		
		private void SetValue(SettingsPropertyValue propVal)
		{
			System.Xml.XmlElement MachineNode = default(System.Xml.XmlElement);
			System.Xml.XmlElement SettingNode = default(System.Xml.XmlElement);
						
			//Determine if the setting is roaming.
			//If roaming then the value is stored as an element under the root
			//Otherwise it is stored under a machine name node
			try
			{
				if (IsRoaming(propVal.Property))
				{
					SettingNode = (XmlElement) (SettingsXml.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name));
				}
				else
				{
					SettingNode = (XmlElement) (SettingsXml.SelectSingleNode(SETTINGSROOT + "/" + (new Microsoft.VisualBasic.Devices.Computer()).Name + "/" + propVal.Name));
				}
			}
			catch (Exception)
			{
				SettingNode = null;
			}
						
			//Check to see if the node exists, if so then set its new value
			if (SettingNode != null)
			{
				if (propVal.SerializedValue != null)
				{
					SettingNode.InnerText = propVal.SerializedValue.ToString();
				}
			}
			else
			{
				if (IsRoaming(propVal.Property))
				{
					//Store the value as an element of the Settings Root Node
					SettingNode = SettingsXml.CreateElement(propVal.Name);
					if (propVal.SerializedValue != null)
					{
						SettingNode.InnerText = propVal.SerializedValue.ToString();
					}
					SettingsXml.SelectSingleNode(SETTINGSROOT).AppendChild(SettingNode);
				}
				else
				{
					//Its machine specific, store as an element of the machine name node,
					//creating a new machine name node if one doesnt exist.
					try
					{
						MachineNode = (XmlElement) (SettingsXml.SelectSingleNode(SETTINGSROOT + "/" + (new Microsoft.VisualBasic.Devices.Computer()).Name));
					}
					catch (Exception)
					{
						MachineNode = SettingsXml.CreateElement((new Microsoft.VisualBasic.Devices.Computer()).Name);
						SettingsXml.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
					}
								
					if (MachineNode == null)
					{
						MachineNode = SettingsXml.CreateElement((new Microsoft.VisualBasic.Devices.Computer()).Name);
						SettingsXml.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
					}
								
					SettingNode = SettingsXml.CreateElement(propVal.Name);
					if (propVal.SerializedValue != null)
					{
						SettingNode.InnerText = propVal.SerializedValue.ToString();
					}
					MachineNode.AppendChild(SettingNode);
				}
			}
		}
		
		private bool IsRoaming(SettingsProperty prop)
		{
			//Determine if the setting is marked as Roaming
			//For Each d As DictionaryEntry In prop.Attributes
			//    Dim a As Attribute = DirectCast(d.Value, Attribute)
			//    If TypeOf a Is System.Configuration.SettingsManageabilityAttribute Then
			//        Return True
			//    End If
			//Next
			//Return False
						
			return true;
		}
	}
}
#endif