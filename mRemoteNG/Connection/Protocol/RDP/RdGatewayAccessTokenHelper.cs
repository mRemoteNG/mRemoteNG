using System;
using System.Runtime.InteropServices;
using System.Text;


namespace mRemoteNG.Connection.Protocol.RDP
{
    internal static class RdGatewayAccessTokenHelper
    {
        public static string EncryptAuthCookieString(string cookieString)
        {
            byte[] cookieBytes = TsCryptEncryptString(cookieString);
            return Convert.ToBase64String(cookieBytes);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CryptProtectPromptStruct
        {
            public int Size;
            public int Flags;
            public IntPtr Window;
            public string Message;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DataBlob
        {
            public int Size;
            public IntPtr Data;
        }

        private const int CRYPTPROTECT_LOCAL_MACHINE = 0x00000004;
        private const int CRYPTPROTECT_UI_FORBIDDEN = 0x00000001;
        private const int CRYPTPROTECT_AUDIT = 0x00000010;

        [DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern bool CryptProtectData(
            ref DataBlob dataIn,
            IntPtr description,
            IntPtr optionalEntropy,
            IntPtr reserved,
            IntPtr promptStruct,
            int flags,
            out DataBlob dataOut);

        private static byte[] TsCryptEncryptString(string inputString)
        {
            DataBlob inputBlob;
            DataBlob outputBlob;
            byte[] outputData = [];

            byte[] stringBytes = Encoding.Unicode.GetBytes(inputString);
            byte[] inputData = new byte[stringBytes.Length + 2];
            Buffer.BlockCopy(stringBytes, 0, inputData, 0, stringBytes.Length);

            inputBlob.Size = inputData.Length;
            inputBlob.Data = Marshal.AllocHGlobal(inputData.Length);
            Marshal.Copy(inputData, 0, inputBlob.Data, inputBlob.Size);

            if (CryptProtectData(ref inputBlob, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero, CRYPTPROTECT_UI_FORBIDDEN, out outputBlob))
            {
                outputData = new byte[outputBlob.Size];
                Marshal.Copy(outputBlob.Data, outputData, 0, outputBlob.Size);
            }

            Marshal.FreeHGlobal(inputBlob.Data);
            Marshal.FreeHGlobal(outputBlob.Data);

            return outputData;
        }

    }
}
