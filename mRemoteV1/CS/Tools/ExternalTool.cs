using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using mRemoteNG.App;
using System.IO;
using System.ComponentModel;


namespace mRemoteNG.Tools
{
	public class ExternalTool
	{
        #region Public Properties
		public string DisplayName {get; set;}
		public string FileName {get; set;}
		public bool WaitForExit {get; set;}
		public string Arguments {get; set;}
		public bool TryIntegrate {get; set;}
		public Connection.Info ConnectionInfo {get; set;}
			
        public Icon Icon
		{
			get
			{
				if (File.Exists(FileName))
				{
					return Misc.GetIconFromFile(FileName);
				}
				else
				{
					return null;
				}
			}
		}
			
        public Image Image
		{
			get
			{
				if (Icon != null)
				{
					return Icon.ToBitmap();
				}
				else
				{
					return null;
				}
			}
		}
        #endregion
			
        #region Constructors
		public ExternalTool(string displayName = "", string fileName = "", string arguments = "")
		{
			this.DisplayName = displayName;
			this.FileName = fileName;
			this.Arguments = arguments;
		}
        #endregion
			
        #region Public Methods
		// Start external app
		public void Start(Connection.Info startConnectionInfo = null)
		{
			try
			{
				if (string.IsNullOrEmpty(FileName))
				{
					throw (new InvalidOperationException("FileName cannot be blank."));
				}
					
				ConnectionInfo = startConnectionInfo;
					
				if (TryIntegrate)
				{
					StartIntegrated();
					return ;
				}
					
				Process process = new Process();
				process.StartInfo.UseShellExecute = true;
				process.StartInfo.FileName = ParseArguments(FileName);
				process.StartInfo.Arguments = ParseArguments(Arguments);
					
				process.Start();
					
				if (WaitForExit)
				{
					process.WaitForExit();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("ExternalApp.Start() failed.", ex);
			}
		}
			
		// Start external app integrated
		public void StartIntegrated()
		{
			try
			{
				Connection.Info newConnectionInfo = default(Connection.Info);
				if (ConnectionInfo == null)
				{
					newConnectionInfo = new Connection.Info();
				}
				else
				{
					newConnectionInfo = ConnectionInfo.Copy();
				}
					
				newConnectionInfo.Protocol = Connection.Protocol.Protocols.IntApp;
				newConnectionInfo.ExtApp = DisplayName;
				newConnectionInfo.Name = DisplayName;
				newConnectionInfo.Panel = My.Language.strMenuExternalTools;
					
				OpenConnection(newConnectionInfo);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "ExternalApp.StartIntegrated() failed.", ex: ex, logOnly: true);
			}
		}
			
		private enum EscapeType
		{
			All,
			ShellMetacharacters,
			None
		}
			
		private struct Replacement
		{
			public Replacement(int start, int length, string value)
			{
				this.Start = start;
				this.Length = length;
				this.Value = value;
			}
				
			public int Start {get; set;}
			public int Length {get; set;}
			public string Value {get; set;}
		}
			
		public string ParseArguments(string input)
		{
			int index = 0;
			List<Replacement> replacements = new List<Replacement>();
				
			do
			{
				int tokenStart = input.IndexOf("%", index, StringComparison.InvariantCulture);
				if (tokenStart == -1)
				{
					break;
				}
					
				int tokenEnd = input.IndexOf("%", tokenStart + 1, StringComparison.InvariantCulture);
				if (tokenEnd == -1)
				{
					break;
				}
					
				int tokenLength = tokenEnd - tokenStart + 1;
					
				int variableNameStart = tokenStart + 1;
				int variableNameLength = tokenLength - 2;
					
				bool isEnvironmentVariable = false;
					
				string variableName = "";
					
				if (tokenStart > 0)
				{
					char tokenStartPrefix = input.Substring(tokenStart - 1, 1);
					char tokenEndPrefix = input.Substring(tokenEnd - 1, 1);
						
					if (tokenStartPrefix == '\\' && tokenEndPrefix == '\\')
					{
						isEnvironmentVariable = true;
							
						// Add the first backslash to the token
						tokenStart--;
						tokenLength++;
							
						// Remove the last backslash from the name
						variableNameLength--;
					}
					else if (tokenStartPrefix == '^' && tokenEndPrefix == '^')
					{
						// Add the first caret to the token
						tokenStart--;
						tokenLength++;
							
						// Remove the last caret from the name
						variableNameLength--;
							
						variableName = input.Substring(variableNameStart, variableNameLength);
						replacements.Add(new Replacement(tokenStart, tokenLength, string.Format("%{0}%", variableName)));
							
						index = tokenEnd;
						continue;
					}
				}
					
				string token = input.Substring(tokenStart, tokenLength);
					
				EscapeType escape = EscapeType.All;
				string prefix = input.Substring(variableNameStart, 1);
				switch (prefix)
				{
					case "-":
						escape = EscapeType.ShellMetacharacters;
						break;
					case "!":
						escape = EscapeType.None;
						break;
				}
					
				if (!(escape == EscapeType.All))
				{
					// Remove the escape character from the name
					variableNameStart++;
					variableNameLength--;
				}
					
				if (variableNameLength == 0)
				{
					index = tokenEnd;
					continue;
				}
					
				variableName = input.Substring(variableNameStart, variableNameLength);
					
				string replacementValue = token;
				if (!isEnvironmentVariable)
				{
					replacementValue = GetVariableReplacement(variableName, token);
				}
					
				bool haveReplacement = false;
					
				if (!(replacementValue == token))
				{
					haveReplacement = true;
				}
				else
				{
					replacementValue = Environment.GetEnvironmentVariable(variableName);
					if (replacementValue != null)
					{
						haveReplacement = true;
					}
				}
					
				if (haveReplacement)
				{
					char trailing = '\0';
					if (tokenEnd + 2 <= input.Length)
					{
						trailing = input.Substring(tokenEnd + 1, 1);
					}
					else
					{
						trailing = string.Empty;
					}
						
					if (escape == EscapeType.All)
					{
						replacementValue = CommandLineArguments.EscapeBackslashes(replacementValue);
						if (trailing == '\'')
						{
							replacementValue = CommandLineArguments.EscapeBackslashesForTrailingQuote(replacementValue);
						}
					}
						
					if (escape == EscapeType.All | escape == EscapeType.ShellMetacharacters)
					{
						replacementValue = CommandLineArguments.EscapeShellMetacharacters(replacementValue);
					}
						
					replacements.Add(new Replacement(tokenStart, tokenLength, replacementValue));
					index = tokenEnd + 1;
				}
				else
				{
					index = tokenEnd;
				}
			} while (true);
				
			string result = input;
				
			for (index = result.Length; index >= 0; index--)
			{
				foreach (Replacement replacement in replacements)
				{
					if (!(replacement.Start == index))
					{
						continue;
					}
						
					string before = result.Substring(0, replacement.Start);
					string after = result.Substring(replacement.Start + replacement.Length);
					result = before + replacement.Value + after;
				}
			}
				
			return result;
		}
        #endregion
			
        #region Private Methods
		private string GetVariableReplacement(string variable, string original)
		{
			string replacement = "";
			switch (variable.ToLowerInvariant())
			{
				case "name":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.Name;
					}
					break;
				case "hostname":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.Hostname;
					}
					break;
				case "port":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = System.Convert.ToString(ConnectionInfo.Port);
					}
					break;
				case "username":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.Username;
					}
					break;
				case "password":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.Password;
					}
					break;
				case "domain":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.Domain;
					}
					break;
				case "description":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.Description;
					}
					break;
					// ReSharper disable once StringLiteralTypo
				case "macaddress":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.MacAddress;
					}
					break;
					// ReSharper disable once StringLiteralTypo
				case "userfield":
					if (ConnectionInfo == null)
					{
						replacement = "";
					}
					else
					{
						replacement = ConnectionInfo.UserField;
					}
					break;
				default:
					return original;
			}
			return replacement;
		}
        #endregion
	}
		
	public class ExternalToolsTypeConverter : StringConverter
	{
			
        public static string[] ExternalTools
		{
			get
			{
				List<string> externalToolList = new List<string>();
					
				// Add a blank entry to signify that no external tool is selected
				externalToolList.Add(string.Empty);
					
				foreach (ExternalTool externalTool in App.Runtime.ExternalTools)
				{
					externalToolList.Add(externalTool.DisplayName);
				}
					
				return externalToolList.ToArray();
			}
		}
			
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(ExternalTools);
		}
			
		public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
		{
			return true;
		}
			
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
