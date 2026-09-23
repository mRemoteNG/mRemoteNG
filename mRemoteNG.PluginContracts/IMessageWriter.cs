using System;

namespace mRemoteNG.PluginContracts;

public interface IMessageWriter
{
    void Info(string message, bool logOnly = false);

    void Warning(string message, bool logOnly = false);

    void Error(string message, bool logOnly = false);

    void Exception(string message, Exception exception);
}
