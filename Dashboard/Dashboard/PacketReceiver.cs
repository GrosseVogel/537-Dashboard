using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Dashboard
{
    /// <summary>
    /// Receives packets from the robot and allows access to the data
    /// </summary>
    /// \todo Provide a callback or another way to notify of new data
    public class PacketReceiver
    {
        /// <summary>
        /// The offset from the start of the packet to the start of the user data
        /// </summary>
        private const int kUserDataOffset = 0x1b;
        
        private Thread receiverThread;
        private UdpClient udp;
        private byte[] accessBuffer;
        private bool running;

        public PacketReceiver()
        {
            receiverThread = new Thread(ThreadProc);
            accessBuffer = new byte[1018]; // 1018 is the FRC packet size
        }

        /// <summary>
        /// Starts the PacketReceiver and binds it to a particular port
        /// </summary>
        /// <param name="port">The port on which to listen for incoming packets</param>
        public void Start(int port)
        {           
            //! \todo Recycle the UdpClient instead of creating a new one each time
            udp = new UdpClient(port);
            receiverThread.Start(this);
            running = true;
        }

        /// <summary>
        /// Stops the receiver
        /// </summary>
        public void Stop()
        {
            //! \todo Confirm this works properly and doesn't crash everything
            receiverThread.Abort();
            running = false;
        }

        /// <summary>
        /// Procedure for the thread
        /// </summary>
        /// <param name="arg">Thread argument (a reference to the receiver object)</param>
        private static void ThreadProc(object arg)
        {
            PacketReceiver r = (PacketReceiver)arg;
            // We don't need this information, but udp.Receive wants to provide it
            System.Net.IPEndPoint ep = null;

            while (true)
            {
                byte[] recvBuffer = r.udp.Receive(ref ep);
                // Get a mutex lock on the buffer to keep other methods from reading from it
                // on the other thread while we copy over new data
                lock (arg)
                {
                    Buffer.BlockCopy(recvBuffer, 0, r.accessBuffer, 0, recvBuffer.Length);
                }

                //! \todo Validate packet via crc, etc.
            }
        }

        public ushort GetPacketNumber()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return BitConverter.ToUInt16(accessBuffer, 0);
            }
        }

        public byte GetDigitalInputs()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return accessBuffer[2];
            }
        }

        public byte GetDigitalOutputs()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return accessBuffer[3];
            }
        }

        public string GetBatteryVoltage()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return accessBuffer[4].ToString("x") + "." + accessBuffer[5].ToString("x");
            }
        }

        public byte GetStatusByte()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return accessBuffer[6];
            }
        }

        public byte GetErrorByte()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return accessBuffer[7];
            }
        }

        public int GetTeamNumber()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return ((int)accessBuffer[8]) * 100 + (int)accessBuffer[9];
            }
        }

        public string GetDSVersion()
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return BitConverter.ToString(accessBuffer, 0x0a, 8);
            }
        }

        public int GetUserInt(int offset)
        {
            if (!running)
                throw new InvalidOperationException("You cannot get data while the receiver is not running.");
            lock (this)
            {
                return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(accessBuffer, kUserDataOffset + offset));
            }
        }
    }
}
