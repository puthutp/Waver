using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Communication.Sockets.Core.Client;
using Newtonsoft.Json;
using waver;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace krin
{
    class ControllerClient
    {
        public const int STATE_INIT = 0;
        public const int STATE_READY = 1;
        public const int STATE_INFO = 2;
        public const int STATE_SENDFILE = 3;
        public const int STATE_CLOSING = 4;
        public const int STATE_END = 5;

        public int State { get; set; }

        ClientTerminal m_terminal = new ClientTerminal();

        private string[] filePaths;
        private int scoredFile = 0;

        private int curIteration = 0;

        private CFileInfo currentFileInfo = new CFileInfo();
        private Stream waveStream;
        private byte[] waveBytes;

#if DEBUG
        private Stopwatch stopwatch = new Stopwatch();
#endif

        public ControllerClient()
        {
            m_terminal.Connected += m_TerminalClient_Connected;
            m_terminal.Disconncted += m_TerminalClient_ConnectionDroped;
            m_terminal.MessageRecived += m_TerminalClient_MessageRecived;

            ChangeState(STATE_INIT);
        }

        public void ChangeState(int _newState)
        {
            State = _newState;
        }

        public void StartConnect(string _ipAddress, string _port)
        {
            int alPort = System.Convert.ToInt16(_port, 10);
            IPAddress remoteIPAddress = System.Net.IPAddress.Parse(_ipAddress);

            try
            {
                m_terminal.Connect(remoteIPAddress, alPort);

                ChangeState(STATE_READY);
            }
            catch (SocketException e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif
                ChangeState(STATE_END);
            }
        }

        public void RetrieveFilePaths(HashSet<string> filterList)
        {
            List<string> tempList = Directory.GetFiles(DirInfo.CurrentDir, "*_p.wav").ToList();
            List<string> resultList = new List<string>();

            foreach (string fileName in tempList)
            {
                if (!filterList.Contains(fileName))
                {
                    resultList.Add(fileName);
                }
            }

            filePaths = resultList.ToArray();

            scoredFile = 0;
            ChangeState(STATE_READY);
        }

        public bool IsReadyToClose()
        {
            return State == STATE_CLOSING || State == STATE_END;
        }

        public bool IsEnding()
        {
            return State == STATE_END;
        }

        public bool IsReady()
        {
            return State == STATE_READY;
        }

        public void EndConnect()
        {
            ChangeState(STATE_END);

            m_terminal.Close();
        }

        public void ScoreNextFile()
        {
            switch (State)
            {
                case ControllerClient.STATE_READY:
#if DEBUG
                    if (scoredFile == 0/* && curIteration == 0*/)
                    {
                        stopwatch.Start();
                    }
#endif

                    if (scoredFile < filePaths.Length)
                    {
                        waveStream = File.OpenRead(filePaths[scoredFile]);

                        currentFileInfo.FileName = filePaths[scoredFile];
                        currentFileInfo.FileSize = (int)waveStream.Length;

                        SendFileInfo();

                        scoredFile++;
                    }
                    else
                    {
                        scoredFile = 0;

                        curIteration++;
                        if (curIteration < int.Parse(Settings.Iteration))
                        {
                            ScoreNextFile();
                        }
                        else
                        {
#if DEBUG
                            stopwatch.Stop();

                            List<string> log = new List<string>();

                            log.Add("elapsed time = " + stopwatch.Elapsed + " == " + stopwatch.ElapsedMilliseconds);
                            log.Add("file processed = " + (int.Parse(Settings.Iteration) * filePaths.Length));
                            
                            Console.WriteLine(log[0]);
                            Console.WriteLine(log[1]);

                            //File.WriteAllLines("log.txt", log.ToArray());

                            stopwatch.Reset();
#endif

                        ChangeState(STATE_CLOSING);

                        curIteration = 0;
                        }
                    }
                    
                    break;
                default:
                    ChangeState(STATE_CLOSING);
                    break;
            }
        }

        public void SendFileInfo()
        {
            ChangeState(STATE_INFO);

            string json = JsonConvert.SerializeObject(currentFileInfo);

            m_terminal.SendMessage(json);
        }

        public void SendFile()
        {
            waveBytes = new byte[waveStream.Length];
            waveStream.Read(waveBytes, 0, waveBytes.Length);

            m_terminal.SendMessage(waveBytes);

            ChangeState(STATE_SENDFILE);
        }

        void m_TerminalClient_Connected(Socket socket)
        {
            m_terminal.StartListen();

            Debug.WriteLine("Start listening to server messages");
        }

        void m_TerminalClient_ConnectionDroped(Socket socket)
        {
            Debug.WriteLine("Server has been disconnected!");

            //ChangeState(STATE_INIT);
            ChangeState(STATE_CLOSING);
        }

        void m_TerminalClient_MessageRecived(Socket socket, byte[] message, int length)
        {
            //Debug.WriteLine("Message received from server");

            char[] chars;
            Decoder d;
            int charLen;
            String szData;

            switch (State)
            {
                case STATE_INFO:
                    chars = new char[length];
                    d = Encoding.UTF8.GetDecoder();
                    charLen = d.GetChars(message, 0, length, chars, 0);
                    szData = new String(chars);

                    if (szData.Equals("OK"))
                    {
                        SendFile();
                        ChangeState(STATE_SENDFILE);
                    } 
                    else if (szData.Equals("NEXT"))
                    {
#if DEBUG
                        Console.WriteLine("info-next");
#endif

                        EndSendFile();
                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine("info-ending");
#endif

                        EndTransfer();
                    }

                    break;

                case STATE_SENDFILE:
                    chars = new char[length];
                    d = Encoding.UTF8.GetDecoder();
                    charLen = d.GetChars(message, 0, length, chars, 0);
                    szData = new String(chars);

                    if (szData == "NEXT")
                    {
#if DEBUG
                        Console.WriteLine("send-next");
#endif

                        EndSendFile();
                    }
                    else if (szData == "END")
                    {
#if DEBUG
                        Console.WriteLine("send-ending");
#endif

                        EndTransfer();
                    }
                    else
                    {
                        try
                        {
                            CFileScore fileScore = JsonConvert.DeserializeObject<CFileScore>(szData);

                            EndSendFile(fileScore.FileName, fileScore.FileScore);
                        }
                        catch (JsonException)
                        {
#if DEBUG
                            Console.WriteLine("failed getting score for " + currentFileInfo.FileName);
                            Console.WriteLine("next");
#endif
                            ChangeState(STATE_READY);
                            ScoreNextFile();
                        }
                    }
                    break;
            }
        }

        private void EndSendFile(string fileName, float fileScore)
        {
            DirInfo.AppendTestedFile(fileName, fileScore);

            EndSendFile();
        }

        private void EndSendFile()
        {
            waveStream.Close();

            ChangeState(STATE_READY);

            ScoreNextFile();
        }

        private void EndTransfer()
        {
            waveStream.Close();

            ChangeState(STATE_END);
        }
    }
}
