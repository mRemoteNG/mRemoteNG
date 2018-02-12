namespace mRemoteNG.Connection.Protocol.RDP
{
	public class RdpProtocolFactory
	{
	    public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
	    {
	        return CreateProtocol(connectionInfo.RdpProtocolVersion, connectionInfo);
	    }

        public ProtocolBase CreateProtocol(RdpVersionEnum version, ConnectionInfo connectionInfo)
		{
			RdpProtocol6 newProtocol = null;

			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (version)
			{
				case RdpVersionEnum.Rdc6:
					newProtocol = new RdpProtocol6(connectionInfo);
					break;
			    case RdpVersionEnum.Rdc7:
			        newProtocol = new RdpProtocol7(connectionInfo);
			        break;
                case RdpVersionEnum.Rdc8:
					newProtocol = new RdpProtocol8(connectionInfo);
					break;
			    case RdpVersionEnum.Rdc9:
			        newProtocol = new RdpProtocol9(connectionInfo);
			        break;
			    case RdpVersionEnum.Rdc10:
			        newProtocol = new RdpProtocol10(connectionInfo);
			        break;
            }

			if (newProtocol != null)
			{
				newProtocol.LoadBalanceInfoUseUtf8 = Settings.Default.RdpLoadBalanceInfoUseUtf8;
			}

			return newProtocol;
		}
	}
}
