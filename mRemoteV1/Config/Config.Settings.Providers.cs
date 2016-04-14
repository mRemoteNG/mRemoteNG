using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;


namespace mRemoteNG.Config.Settings.Providers
{
	public class ChooseProvider : PortableSettingsProvider
	{
        #if !PORTABLE
        #else
        #endif
	}
				
	public class PortableSettingsProvider : SettingsProvider
	{
		const string SETTINGSROOT = "Settings"; //XML Root Node
		public override void Initialize(string name, NameValueCollection col)
		{
			base.Initialize(this.ApplicationName, col);
		}
					
        public override string ApplicationName
		{
			get
			{
				if (System.Windows.Forms.Application.ProductName.Trim().Length > 0)
				{
					return System.Windows.Forms.Application.ProductName;
				}
				else
				{
					System.IO.FileInfo fi = new System.IO.FileInfo(System.Windows.Forms.Application.ExecutablePath);
					return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
				}
			}
			set
			{
				//Do nothing
			}
		}
					
		virtual public string GetAppSettingsPath()
		{
			//Used to determine where to store the settings
			System.IO.FileInfo fi = new System.IO.FileInfo(Application.ExecutablePath);
			return fi.DirectoryName;
		}
					
		virtual public string GetAppSettingsFilename()
		{
			//Used to determine the filename to store the settings
			return "portable.config"; //ApplicationName & ".settings"
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
				SettingsXML.Save(System.IO.Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
			}
			catch (Exception)
			{
				//Ignore if cant save, device been ejected
			}
		}
					
		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
		{
			//Create new collection of values
			SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();
						
			//Iterate through the settings to be retrieved
			foreach (SettingsProperty setting in props)
			{
							
				SettingsPropertyValue value = new SettingsPropertyValue(setting);
				value.IsDirty = false;
				value.SerializedValue = GetValue(setting);
				values.Add(value);
			}
			return values;
		}
					
		private System.Xml.XmlDocument m_SettingsXML = null;
					
        private System.Xml.XmlDocument SettingsXML
		{
			get
			{
				//If we dont hold an xml document, try opening one.
				//If it doesnt exist then create a new one ready.
				if (m_SettingsXML == null)
				{
					m_SettingsXML = new System.Xml.XmlDocument();
								
					try
					{
						m_SettingsXML.Load(System.IO.Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
					}
					catch (Exception)
					{
						//Create new document
						XmlDeclaration dec = m_SettingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
						m_SettingsXML.AppendChild(dec);
									
						XmlNode nodeRoot = default(XmlNode);
									
						nodeRoot = m_SettingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, "");
						m_SettingsXML.AppendChild(nodeRoot);
					}
				}
				return m_SettingsXML;
			}
		}
					
		private string GetValue(SettingsProperty setting)
		{
			string ret = "";
						
			try
			{
				if (IsRoaming(setting))
				{
					ret = SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + setting.Name).InnerText;
				}
				else
				{
					ret = SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + (new Microsoft.VisualBasic.Devices.Computer()).Name + "/" + setting.Name).InnerText;
				}
			}
			catch (Exception)
			{
				if (!(setting.DefaultValue == null))
				{
					ret = setting.DefaultValue.ToString();
				}
				else
				{
					ret = "";
				}
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
					SettingNode = (XmlElement) (SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name));
				}
				else
				{
					SettingNode = (XmlElement) (SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + (new Microsoft.VisualBasic.Devices.Computer()).Name + "/" + propVal.Name));
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
					SettingNode = SettingsXML.CreateElement(propVal.Name);
					if (propVal.SerializedValue != null)
					{
						SettingNode.InnerText = propVal.SerializedValue.ToString();
					}
					SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(SettingNode);
				}
				else
				{
					//Its machine specific, store as an element of the machine name node,
					//creating a new machine name node if one doesnt exist.
					try
					{
						MachineNode = (XmlElement) (SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + (new Microsoft.VisualBasic.Devices.Computer()).Name));
					}
					catch (Exception)
					{
						MachineNode = SettingsXML.CreateElement((new Microsoft.VisualBasic.Devices.Computer()).Name);
						SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
					}
								
					if (MachineNode == null)
					{
						MachineNode = SettingsXML.CreateElement((new Microsoft.VisualBasic.Devices.Computer()).Name);
						SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
					}
								
					SettingNode = SettingsXML.CreateElement(propVal.Name);
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