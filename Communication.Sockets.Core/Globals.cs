using System.Net.Sockets;

namespace Communication.Sockets.Core
{
    public delegate void TCPTerminal_MessageRecivedDel(Socket socket, byte[] message, int length);
    public delegate void TCPTerminal_ConnectDel(Socket socket);
    public delegate void TCPTerminal_DisconnectDel(Socket socket);
}
