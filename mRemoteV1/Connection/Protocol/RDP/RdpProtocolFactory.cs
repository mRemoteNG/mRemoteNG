namespace mRemoteNG.Connection.Protocol.RDP
{
	public class RdpProtocolFactory
	{
		public ProtocolBase CreateProtocol(ConnectionInfo connectionInfo)
		{
			RdpProtocol6 newProtocol = null;

			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (connectionInfo.RdpProtocolVersion)
			{
				case RdpVersionEnum.Rdc6:
					newProtocol = new RdpProtocol6();
					break;
				case RdpVersionEnum.Rdc8:
					newProtocol = new RdpProtocol8();
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
