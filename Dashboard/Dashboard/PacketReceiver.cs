using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Dashboard
{
    /// <summary>
    /// 
    /// </summary>
    public class PacketReceiver
    {
        private const int kUserDataOffset = 0x1b;

        private Thread receiverThread;
        private UdpClient udp;
        private byte[] accessBuffer;

        public PacketReceiver()
        {
            receiverThread = new Thread(ThreadProc);
            accessBuffer = new byte[1018]; // 1018 is the FRC packet size
        }

        public void Start(int port)
        {
            receiverThread.Start(this);
            udp = new UdpClient(port);
        }

        public void Stop()
        {
            receiverThread.Abort();
        }

        private static void ThreadProc(object arg)
        {
            PacketReceiver r = (PacketReceiver)arg;
            System.Net.IPEndPoint ep = null;

            while (true)
            {
                byte[] recvBuffer = r.udp.Receive(ref ep);
                Buffer.BlockCopy(recvBuffer, 0, r.accessBuffer, 0, recvBuffer.Length);

                // TODO: Validate crc
            }
        }

        public ushort GetPacketNumber()
        {
            return BitConverter.ToUInt16(accessBuffer, 0);
        }

        public byte GetDigitalInputs()
        {
            return accessBuffer[2];
        }

        public byte GetDigitalOutputs()
        {
            return accessBuffer[3];
        }

        public string GetBatteryVoltage()
        {
            return accessBuffer[4].ToString("x") + "." + accessBuffer[5].ToString("x");
        }

        public byte GetStatusByte()
        {
            return accessBuffer[6];
        }

        public byte GetErrorByte()
        {
            return accessBuffer[7];
        }

        public uint GetTeamNumber()
        {
            return ((uint)accessBuffer[8]) * 100 + (uint)accessBuffer[9];
        }

        public string GetDSVersion()
        {
            return BitConverter.ToString(accessBuffer, 0x0a, 8);
        }

        public int GetUserInt(int offset)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(accessBuffer, kUserDataOffset + offset));
        }
    }
}
