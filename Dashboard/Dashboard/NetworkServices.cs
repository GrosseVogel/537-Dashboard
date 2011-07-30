using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Dashboard
{
    public static class NetworkServices
    {
        [DllImport("NetworkServices.dll")]
        public static extern UInt32 GetCrc(byte[] data, uint length);
    }
}
