using System.Net.Sockets;

namespace waver
{
    public class CSocketPacket
    {
        public Socket thisSocket;
        public byte[] dataBuffer = new byte[65536];
    }
}
