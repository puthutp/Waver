using System;
using System.Net.Sockets;
using Communication.Sockets.Core;
using Communication.Sockets.Core.Server;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace waver
{
    class ControllerServer
    {
        public const int STATE_INIT = 0;
        public const int STATE_WAITING = 1;
        public const int STATE_RECEIVING = 3;

        public int State { get; set; }

        ServerTerminal m_serverTerminal;

        CFileInfo currentFileInfo;

        private byte[] waveBuffer;
        private int total;
        private bool fileWritten = false;

        public ControllerServer()
        {
            State = STATE_INIT;
        }

        public void ChangeState(int _newState)
        {
            State = _newState;
        }

        public void StartServer()
        {
            createTerminal(int.Parse(WSettings.Port));
            Console.WriteLine("Terminal opened");

            ChangeState(STATE_WAITING);
        }

        public void RestartServer()
        {
            closeTerminal();

            StartServer();
        }

        void m_Terminal_ClientDisConnected(Socket socket)
        {
            ChangeState(STATE_WAITING);

            Console.WriteLine(string.Format("Client {0} has been disconnected!", socket.LocalEndPoint));
            Console.WriteLine();
        }

        void m_Terminal_ClientConnected(Socket socket)
        {
            Console.WriteLine(string.Format("Client {0} has been connected!", socket.LocalEndPoint));
            Console.WriteLine();
        }

        void m_Terminal_MessageRecived(Socket socket, byte[] message, int length)
        {
            //Console.WriteLine("message received from client");

            switch (State)
            {
                case STATE_WAITING:
                    char[] chars = new char[length + 1];
                    Decoder d = Encoding.UTF8.GetDecoder();
                    int charLen = d.GetChars(message, 0, length, chars, 0);
                    String szData = new String(chars);

                    try
                    {
                        currentFileInfo = JsonConvert.DeserializeObject<CFileInfo>(szData);

                        Console.WriteLine("filename " + currentFileInfo.FileName);
                        Console.WriteLine("filesize " + currentFileInfo.FileSize);

                        if (currentFileInfo.FileSize >= 44)
                        {

                            m_serverTerminal.SendMessage("OK");

                            waveBuffer = new byte[currentFileInfo.FileSize];
                            total = 0;
                            fileWritten = false;
                            ChangeState(STATE_RECEIVING);
                        }
                        else
                        {
                            //UpdateScoreServer(currentFileInfo.FileName, 0);

                            m_serverTerminal.SendMessage("NEXT");
                        }
                    }
                    catch (JsonException)
                    {
                        Console.WriteLine("file info not valid. ending client.");
                        m_serverTerminal.SendMessage("END");
                        Console.WriteLine("restarting server");
                        RestartServer();
                    }

                    break;

                case STATE_RECEIVING:
                    try
                    {
                        Buffer.BlockCopy(message, 0, waveBuffer, total, length);

                        total += length;

                        if (total == currentFileInfo.FileSize && !fileWritten)
                        {
                            fileWritten = true;
                            CalculateSendScore();
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("file receiving failed, ending client");
                        m_serverTerminal.SendMessage("END");
                        ChangeState(STATE_WAITING);
                    }
                    
                    break;
            }
        }

        private void CalculateSendScore()
        {
            IdolWave wave0 = IdolWaveLoader.LoadWaveFile(waveBuffer);

            List<IdolWaveNote> result0 = IdolWaveLoader.GetWaveNotes(wave0);

            Scoring scoring = new Scoring();
            float score = scoring.GetScore(result0);
            
            CFileScore fileScore = new CFileScore() { FileName = currentFileInfo.FileName, FileScore = score };

            string json = JsonConvert.SerializeObject(fileScore);

            UpdateScoreServer(fileScore);

            m_serverTerminal.SendMessage(json);

            Console.WriteLine();
            Console.WriteLine("final score: " + score);
            Console.WriteLine("------------");
            Console.WriteLine();

            ChangeState(STATE_WAITING);
        }

        private void UpdateScoreServer(CFileScore fileScore)
        {
            UpdateScoreServer(fileScore.FileName, fileScore.FileScore);
        }

        private void UpdateScoreServer(string fileName, float score)
        {
            RequestManager requestManager = new RequestManager();
            string fileNameS = Path.GetFileName(fileName).Replace("_p", "_s");

            if (WSettings.Address.Length > 0)
            {
                try
                {
                    requestManager.SendPOSTRequest(WSettings.Address, "filename=" + fileNameS + "&score=" + score.ToString(), "", "", false);
                    Console.WriteLine("POST: filename=" + fileNameS + "&score=" + score.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Post request failed. " + e.Message);
                }
            }
            //requestManager.SendPOSTRequest("http://192.168.0.100/backend/api/pitchscore", "filename=" + fileNameS + "&score=" + fileScore.FileScore.ToString(), "", "", false);
            //requestManager.SendPOSTRequest("http://172.16.1.254/fideatest/index.php/editor/insertscore", "filename=" + fileNameS + "&score=" + fileScore.FileScore.ToString(), "", "", false);
        }

        private void createTerminal(int alPort)
        {
            m_serverTerminal = new ServerTerminal();

            m_serverTerminal.MessageRecived += m_Terminal_MessageRecived;
            m_serverTerminal.ClientConnect += m_Terminal_ClientConnected;
            m_serverTerminal.ClientDisconnect += m_Terminal_ClientDisConnected;

            m_serverTerminal.StartListen(alPort);
        }

        private void closeTerminal()
        {
            m_serverTerminal.MessageRecived -= new TCPTerminal_MessageRecivedDel(m_Terminal_MessageRecived);
            m_serverTerminal.ClientConnect -= new TCPTerminal_ConnectDel(m_Terminal_ClientConnected);
            m_serverTerminal.ClientDisconnect -= new TCPTerminal_DisconnectDel(m_Terminal_ClientDisConnected);

            m_serverTerminal.Close();
        }
    }
}
