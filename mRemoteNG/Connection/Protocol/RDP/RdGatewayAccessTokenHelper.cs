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

            if (cookieBytes != null)
            {
                return Convert.ToBase64String(cookieBytes);
            }

            return null;
        }

        public static string DecryptAuthCookieString(string cookieString) //TODO: decrypt is newer use, should we remove it?
        {
            return TsCryptDecryptString(Convert.FromBase64String(cookieString));
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
        private static extern bool CryptProtectData(
            ref DataBlob dataIn,
            IntPtr description,
            IntPtr optionalEntropy,
            IntPtr reserved,
            IntPtr promptStruct,
            int flags,
            out DataBlob dataOut);

        [DllImport("crypt32.dll", CharSet = CharSet.Unicode)] //TODO: decrypt is newer use, should we remove it?
        private static extern bool CryptUnprotectData(
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
            byte[] outputData = null;

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

        private static string TsCryptDecryptString(byte[] inputBytes) //TODO: decrypt is newer use, should we remove it?
        {
            DataBlob inputBlob;
            DataBlob outputBlob;
            byte[] outputData = null;

            inputBlob.Size = inputBytes.Length;
            inputBlob.Data = Marshal.AllocHGlobal(inputBytes.Length);
            Marshal.Copy(inputBytes, 0, inputBlob.Data, inputBlob.Size);

            if (CryptUnprotectData(ref inputBlob, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero, CRYPTPROTECT_UI_FORBIDDEN, out outputBlob))
            {
                outputData = new byte[outputBlob.Size];
                Marshal.Copy(outputBlob.Data, outputData, 0, outputBlob.Size);
            }

            Marshal.FreeHGlobal(inputBlob.Data);
            Marshal.FreeHGlobal(outputBlob.Data);

            if (outputData != null)
            {
                return Encoding.Unicode.GetString(outputData).TrimEnd((Char)0);
            }

            return null;
        }

    }
}
