using System;
using mRemoteNG.Messages;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins;

internal sealed class PluginMessageWriter : IMessageWriter
{
    public void Info(string message, bool logOnly = false)
    {
        App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, message, logOnly);
    }

    public void Warning(string message, bool logOnly = false)
    {
        App.Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, message, logOnly);
    }

    public void Error(string message, bool logOnly = false)
    {
        App.Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, message, logOnly);
    }

    public void Exception(string message, Exception exception)
    {
        App.Runtime.MessageCollector.AddExceptionMessage(message, exception);
    }
}
