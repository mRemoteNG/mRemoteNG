using System;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using static System.IO.FileMode;

namespace mRemoteNG.Plugins.SshTransfer;

internal sealed class SecureTransferService : IDisposable
{
    private readonly string _host;
    private readonly string _user;
    private readonly string _password;
    private readonly int _port;

    public SecureTransferService(string host, string user, string password, int port, SshTransferProtocol protocol, string sourceFile, string destinationFile)
    {
        _host = host;
        _user = user;
        _password = password;
        _port = port;
        Protocol = protocol;
        SourceFile = sourceFile;
        DestinationFile = destinationFile;
    }

    public SshTransferProtocol Protocol { get; }

    public string SourceFile { get; }

    public string DestinationFile { get; }

    public ScpClient? ScpClient { get; private set; }

    public SftpClient? SftpClient { get; private set; }

    public SftpUploadAsyncResult? AsyncResult { get; private set; }

    public AsyncCallback? AsyncCallback { get; set; }

    public void Connect()
    {
        if (Protocol == SshTransferProtocol.Scp)
        {
            ScpClient = new ScpClient(_host, _port, _user, _password);
            ScpClient.Connect();
            return;
        }

        SftpClient = new SftpClient(_host, _port, _user, _password);
        SftpClient.Connect();
    }

    public void Disconnect()
    {
        if (Protocol == SshTransferProtocol.Scp)
        {
            ScpClient?.Disconnect();
            return;
        }

        SftpClient?.Disconnect();
    }

    public void Upload()
    {
        if (Protocol == SshTransferProtocol.Scp)
        {
            if (ScpClient?.IsConnected != true)
            {
                throw new InvalidOperationException("SCP Not Connected!");
            }

            ScpClient.Upload(new FileInfo(SourceFile), DestinationFile);
            return;
        }

        if (SftpClient?.IsConnected != true)
        {
            throw new InvalidOperationException("SFTP Not Connected!");
        }

        AsyncResult = (SftpUploadAsyncResult)SftpClient.BeginUploadFile(new FileStream(SourceFile, Open), DestinationFile, AsyncCallback);
    }

    public void Dispose()
    {
        ScpClient?.Dispose();
        SftpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
