using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace mRemoteNG.Tools.Cmdline
{
	// Adapted from http://qntm.org/cmd
	public class CommandLineArguments
	{
		protected List<Argument> Arguments = new List<Argument>();
			
		public bool EscapeForShell {get; set;}
			
        #region Public Methods
		public void Add(string argument, bool forceQuotes = false)
		{
			Arguments.Add(new Argument(argument, false, forceQuotes));
		}
			
		public void Add(params string[] argumentArray)
		{
			foreach (var argument in argumentArray)
			{
				Add(argument);
			}
		}
			
		public void AddFileName(string fileName, bool forceQuotes = false)
		{
			Arguments.Add(new Argument(fileName, true, forceQuotes));
		}
			
		public override string ToString()
		{
		    var argList = Arguments.Select(argument => ProcessArgument(argument, EscapeForShell));
            return string.Join(" ", argList.ToArray());
		}
			
		public static string PrefixFileName(string argument)
		{
			if (string.IsNullOrEmpty(argument))
				return argument;
				
			if (argument.StartsWith("-"))
				argument = ".\\" + argument;
				
			return argument;
		}
			
		public static string EscapeBackslashes(string argument)
		{
			if (string.IsNullOrEmpty(argument))
			{
				return argument;
			}
				
			// Sequence of backslashes followed by a double quote:
			//     double up all the backslashes and escape the double quote
			return Regex.Replace(argument, "(\\\\*)\"", "$1$1\\\"");
		}
			
		public static string EscapeBackslashesForTrailingQuote(string argument)
		{
			if (string.IsNullOrEmpty(argument))
			{
				return argument;
			}
				
			// Sequence of backslashes followed by the end of the string
			// (which will become a double quote):
			//     double up all the backslashes
			return Regex.Replace(argument, "(\\\\*)$", "$1$1");
		}
			
		public static string QuoteArgument(string argument, bool forceQuotes = false)
		{
			if (!forceQuotes && !string.IsNullOrEmpty(argument) && !argument.Contains(" "))
			{
				return argument;
			}
				
			return "\"" + EscapeBackslashesForTrailingQuote(argument) + "\"";
		}
			
		public static string EscapeShellMetacharacters(string argument)
		{
		    return string.IsNullOrEmpty(argument) ? argument : Regex.Replace(argument, "([()%!^\"<>&|])", "^$1");
		}

	    #endregion
			
        #region Protected Methods
		protected static string ProcessArgument(Argument argument, bool escapeForShell = false)
		{
			var text = argument.Text;
				
			if (argument.IsFileName)
			{
				text = PrefixFileName(text);
			}
			text = EscapeBackslashes(text);
			text = QuoteArgument(text, argument.ForceQuotes);
			if (escapeForShell)
			{
				text = EscapeShellMetacharacters(text);
			}
				
			return text;
		}
        #endregion
			
        #region Protected Classes
		protected class Argument
		{
			public Argument(string text, bool isFileName = false, bool forceQuotes = false)
			{
				Text = text;
				IsFileName = isFileName;
				ForceQuotes = forceQuotes;
			}
				
			public string Text {get; set;}
			public bool IsFileName {get; set;}
			public bool ForceQuotes {get; set;}
		}
        #endregion
	}
}
