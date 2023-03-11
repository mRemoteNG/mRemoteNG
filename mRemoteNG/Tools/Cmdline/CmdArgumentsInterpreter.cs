using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using mRemoteNG.App;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Tools.Cmdline
{
    [SupportedOSPlatform("windows")]
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
        private readonly StringDictionary _parameters;

        // Retrieve a parameter value if it exists
        public string this[string param] => (_parameters[param]);

        public CmdArgumentsInterpreter(IEnumerable<string> args)
        {
            _parameters = new StringDictionary();
            var spliter = new Regex("^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var remover = new Regex("^[\'\"]?(.*?)[\'\"]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string parameter = null;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'

            try
            {
                foreach (var txt in args)
                {
                    // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                    var Parts = spliter.Split(txt, 3);
                    switch (Parts.Length)
                    {
                        case 1:
                            // Found a value (for the last parameter found (space separator))
                            if (parameter != null)
                            {
                                if (!_parameters.ContainsKey(parameter))
                                {
                                    Parts[0] = remover.Replace(Parts[0], "$1");
                                    _parameters.Add(parameter, Parts[0]);
                                }

                                parameter = null;
                            }

                            // else Error: no parameter waiting for a value (skipped)
                            break;
                        case 2:
                            // Found just a parameter
                            // The last parameter is still waiting. With no value, set it to true.
                            if (parameter != null)
                            {
                                if (!_parameters.ContainsKey(parameter))
                                {
                                    _parameters.Add(parameter, "true");
                                }
                            }

                            parameter = Parts[1];
                            break;
                        case 3:
                            // Parameter with enclosed value
                            // The last parameter is still waiting. With no value, set it to true.
                            if (parameter != null)
                            {
                                if (!_parameters.ContainsKey(parameter))
                                {
                                    _parameters.Add(parameter, "true");
                                }
                            }

                            parameter = Parts[1];
                            // Remove possible enclosing characters (",')
                            if (!_parameters.ContainsKey(parameter))
                            {
                                Parts[2] = remover.Replace(Parts[2], "$1");
                                _parameters.Add(parameter, Parts[2]);
                            }

                            parameter = null;
                            break;
                    }
                }

                // In case a parameter is still waiting
                if (parameter == null) return;
                if (!_parameters.ContainsKey(parameter))
                    _parameters.Add(parameter, "true");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Creating new Args failed", ex);
            }
        }
    }
}