using System;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;

namespace mRemoteNGTests.TestHelpers
{
	internal class ConnectionInfoHelpers
	{
		private static readonly Random _random = new Random();

		/// <summary>
		/// Returns a <see cref="ConnectionInfo"/> object with randomized
		/// values in all fields.
		/// </summary>
		internal static ConnectionInfo GetRandomizedConnectionInfo(bool randomizeInheritance = false)
		{
			var connectionInfo = new ConnectionInfo
			{
				// string types
				Name = RandomString(),
				Hostname = RandomString(),
				Description = RandomString(),
				Domain = RandomString(),
				ExtApp = RandomString(),
				Icon = RandomString(),
				LoadBalanceInfo = RandomString(),
				MacAddress = RandomString(),
				Panel = RandomString(),
				Password = RandomString(),
				PostExtApp = RandomString(),
				PreExtApp = RandomString(),
				PuttySession = RandomString(),
				RDGatewayHostname = RandomString(),
				RDGatewayUsername = RandomString(),
				RDGatewayDomain = RandomString(),
				RDGatewayPassword = RandomString(),
				UserField = RandomString(),
				Username = RandomString(),
				VNCProxyIP = RandomString(),
				VNCProxyPassword = RandomString(),
				VNCProxyUsername = RandomString(),

				// bool types
				AutomaticResize = RandomBool(),
				CacheBitmaps = RandomBool(),
				DisplayThemes = RandomBool(),
				DisplayWallpaper = RandomBool(),
				EnableDesktopComposition = RandomBool(),
				EnableFontSmoothing = RandomBool(),
				IsContainer = RandomBool(),
				IsDefault = RandomBool(),
				IsQuickConnect = RandomBool(),
				PleaseConnect = RandomBool(),
				RDPAlertIdleTimeout = RandomBool(),
				RedirectDiskDrives = RandomBool(),
				RedirectKeys = RandomBool(),
				RedirectPorts = RandomBool(),
				RedirectPrinters = RandomBool(),
				RedirectSmartCards = RandomBool(),
				UseConsoleSession = RandomBool(),
				UseCredSsp = RandomBool(),
				VNCViewOnly = RandomBool(),

				// ints
				Port = RandomInt(),
				RDPMinutesToIdleTimeout = RandomInt(),
				VNCProxyPort = RandomInt(),
				
				// enums
				Colors = RandomEnum<RdpProtocol.RDPColors>(),
				ICAEncryptionStrength = RandomEnum<IcaProtocol.EncryptionStrength> (),
				Protocol = RandomEnum<ProtocolType>(),
				RDGatewayUsageMethod = RandomEnum<RdpProtocol.RDGatewayUsageMethod>(),
				RDGatewayUseConnectionCredentials = RandomEnum<RdpProtocol.RDGatewayUseConnectionCredentials>(),
				RDPAuthenticationLevel = RandomEnum<RdpProtocol.AuthenticationLevel>(),
				RedirectSound = RandomEnum<RdpProtocol.RDPSounds>(),
				RenderingEngine = RandomEnum<HTTPBase.RenderingEngine>(),
				Resolution = RandomEnum<RdpProtocol.RDPResolutions>(),
				SoundQuality = RandomEnum<RdpProtocol.RDPSoundQuality>(),
				VNCAuthMode = RandomEnum<ProtocolVNC.AuthMode>(),
				VNCColors = RandomEnum<ProtocolVNC.Colors>(),
				VNCCompression = RandomEnum<ProtocolVNC.Compression>(),
				VNCEncoding = RandomEnum<ProtocolVNC.Encoding>(),
				VNCProxyType = RandomEnum<ProtocolVNC.ProxyType>(),
				VNCSmartSizeMode = RandomEnum<ProtocolVNC.SmartSizeMode>(),
			};

			if (randomizeInheritance)
				connectionInfo.Inheritance = GetRandomizedInheritance(connectionInfo);

			return connectionInfo;
		}

		internal static ConnectionInfoInheritance GetRandomizedInheritance(ConnectionInfo parent)
		{
			var inheritance = new ConnectionInfoInheritance(parent, true);
			foreach (var property in inheritance.GetProperties())
			{
				property.SetValue(inheritance, RandomBool());
			}
			return inheritance;
		}

		internal static string RandomString()
		{
			return Guid.NewGuid().ToString("N");
		}

		internal static bool RandomBool()
		{
			return _random.Next() % 2 == 0;
		}

		internal static int RandomInt()
		{
			return _random.Next();
		}

		internal static T RandomEnum<T>() where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException("T must be an enum");

			var values = Enum.GetValues(typeof(T));
			return (T)values.GetValue(_random.Next(values.Length));
		}
	}
}
