using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace waver
{
    class ConnectionServer
    {
        private ControllerServer controller;

        public AsyncCallback pfnWorkerCallBack;
        public Socket m_socListener;
        public Socket m_socWorker;

        byte[] waveBuffer;

        public ConnectionServer(ControllerServer _controller)
        {
            controller = _controller;

            waveBuffer = new byte[401018];
        }

        public void TurnOnServer()
        {
            m_socListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 8221);
            //bind to local IP Address...
            m_socListener.Bind(ipLocal);
            //start listening...
            m_socListener.Listen(4);
            // create the call back for any client connections...
            m_socListener.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                m_socWorker = m_socListener.EndAccept(asyn);
                Console.WriteLine("connected");

                WaitForData(m_socWorker);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                Console.WriteLine("!!(01) " + se.Message);
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

                    controller.RetrievePacket(theSockId, iRx);

                    ////Buffer.BlockCopy(theSockId.dataBuffer, 0, waveBuffer, total, iRx);
                    ////total += iRx;
                    //////Console.Write Line(total);

                    ////if (total == 401018/* && !fileWritten*/)
                    ////{
                    ////    //File.WriteAllBytes("tescopy.wav", waveBuffer);
                    ////    //fileWritten = true;
                    ////    IdolWave wave0 = IdolWaveLoader.LoadWaveFile(waveBuffer);

                    ////    List<IdolWaveNote> result0 = IdolWaveLoader.GetWaveNotes(wave0);

                    ////    Scoring scoring = new Scoring();
                    ////    float score = scoring.GetScore(result0);

                    ////    Console.WriteLine();
                    ////    Console.WriteLine("final score: " + score);
                    ////}

                    //char[] chars = new char[iRx + 1];
                    //System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                    //int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                    //System.String szData = new System.String(chars);

                    //Console.WriteLine(szData);

                    WaitForData(m_socWorker);
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
