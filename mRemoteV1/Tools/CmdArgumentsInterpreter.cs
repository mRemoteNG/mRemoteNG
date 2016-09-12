using mRemoteNG.App;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace mRemoteNG.Tools
{
    //
    //* Arguments class: application arguments interpreter
    //*
    //* Authors:		R. LOPES
    //* Contributors:	R. LOPES
    //* Created:		25 October 2002
    //* Modified:		28 October 2002
    //*
    //* Version:		1.0
    //
    public class CmdArgumentsInterpreter
    {
        private StringDictionary Parameters;

        // Retrieve a parameter value if it exists
        public string this[string Param] => (Parameters[Param]);

        public CmdArgumentsInterpreter(string[] Args)
        {
            Parameters = new StringDictionary();
            Regex Spliter = new Regex("^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex Remover = new Regex("^[\'\"]?(.*?)[\'\"]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string Parameter = null;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'

            try
            {
                foreach (string Txt in Args)
                {
                    // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                    var Parts = Spliter.Split(Txt, 3);
                    switch (Parts.Length)
                    {
                        case 1:
                            // Found a value (for the last parameter found (space separator))
                            if (Parameter != null)
                            {
                                if (!Parameters.ContainsKey(Parameter))
                                {
                                    Parts[0] = Remover.Replace(Parts[0], "$1");
                                    Parameters.Add(Parameter, Parts[0]);
                                }
                                Parameter = null;
                            }
                            // else Error: no parameter waiting for a value (skipped)
                            break;
                        case 2:
                            // Found just a parameter
                            // The last parameter is still waiting. With no value, set it to true.
                            if (Parameter != null)
                            {
                                if (!Parameters.ContainsKey(Parameter))
                                {
                                    Parameters.Add(Parameter, "true");
                                }
                            }
                            Parameter = Parts[1];
                            break;
                        case 3:
                            // Parameter with enclosed value
                            // The last parameter is still waiting. With no value, set it to true.
                            if (Parameter != null)
                            {
                                if (!Parameters.ContainsKey(Parameter))
                                {
                                    Parameters.Add(Parameter, "true");
                                }
                            }
                            Parameter = Parts[1];
                            // Remove possible enclosing characters (",')
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[2] = Remover.Replace(Parts[2], "$1");
                                Parameters.Add(Parameter, Parts[2]);
                            }
                            Parameter = null;
                            break;
                    }
                }
                // In case a parameter is still waiting
                if (Parameter != null)
                {
                    if (!Parameters.ContainsKey(Parameter))
                    {
                        Parameters.Add(Parameter, "true");
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Creating new Args failed" + Environment.NewLine + ex.Message, true);
            }
        }
    }
}