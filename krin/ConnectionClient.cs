using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net.Sockets;
using System.IO;
using waver;

namespace krin
{
    class ConnectionClient
    {
        private ControllerClient controller;

        public Socket m_socClient;

        public AsyncCallback pfnWorkerCallBack;

        public ConnectionClient(ControllerClient _controller)
        {
            controller = _controller;
        }

        public void ConnectToServer(string _ipAddress, string _port)
        {
            try
            {
                //create a new client socket ...
                m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                String szIPSelected = _ipAddress;
                String szPort = _port;
                int alPort = System.Convert.ToInt16(szPort, 10);

                System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(szIPSelected);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, alPort);
                m_socClient.Connect(remoteEndPoint);

                //String szData  = "Hello There";
                //byte[] byData = System.Text.Encoding.ASCII.GetBytes(szData);
                //m_socClient.Send(byData);

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void SendString(String _data)
        {
            try
            {
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(_data);
                m_socClient.Send(byData);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void SendData()
        {
            try
            {
                Stream waveStream = File.OpenRead("sample/f_legato.wav");

                byte[] waveBytes = new byte[waveStream.Length];
                waveStream.Read(waveBytes, 0, waveBytes.Length);

                m_socClient.Send(waveBytes);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void ReceiveData(System.Windows.Forms.TextBox txtDataRx)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int iRx = m_socClient.Receive(buffer);
                char[] chars = new char[iRx];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                txtDataRx.Text = szData;
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void CloseConnection()
        {
            if (m_socClient != null && m_socClient.Connected)
            {
                m_socClient.Shutdown(SocketShutdown.Both);
                m_socClient.Close();
            }
        }

        public void WaitForData(Socket soc)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                }
                CSocketPacket theSocPkt = new CSocketPacket();
                theSocPkt.thisSocket = soc;
                // now start to listen for any data...
                soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                Console.WriteLine("!!(02) " + se.Message);
            }

        }

        int total = 0;
        bool fileWritten = false;
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                {
                    CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState;
                    //end receive...
                    int iRx = 0;
                    iRx = theSockId.thisSocket.EndReceive(asyn);

                    //controller.RetrievePacket(theSockId, iRx);

                    //WaitForData(m_socServer);
                }
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                Console.WriteLine("!!(03) " + se.Message);
            }
        }
    }
}
