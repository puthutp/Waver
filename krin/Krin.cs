using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace krin
{
    class Krin
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("reading setting file");
#endif

            Settings.DefaultSetting();
            if (!Settings.TryReadSettingFile())
            {
#if DEBUG
                Console.WriteLine("read setting failed, creating new setting file");
#endif
                Settings.CreateSettingFile();
            }
            
            DirInfo.Initialize();

            if (DirInfo.UntestedDirs.Count > 0)
            {
#if DEBUG
                Console.WriteLine("connecting to server");
#endif
                String szIPSelected = Settings.Address;
                String szPort = Settings.Port;

                ControllerClient client = new ControllerClient();
                client.StartConnect(szIPSelected, szPort);

                if (client.IsReady())
                {
                    foreach (string dirName in DirInfo.UntestedDirs)
                    {
                        DirInfo.CurrentDir = dirName;
                        DirInfo.SetTestedFile();

                        client.RetrieveFilePaths(DirInfo.TestedFiles);
                        DirInfo.TestedFiles.Clear();

#if DEBUG
                        Console.WriteLine("sending files");
#endif
                        client.ScoreNextFile();

                        while (!client.IsReadyToClose())
                        {
                            
                        }

                        if (client.IsEnding()) break;
                    }

                    while (!client.IsReadyToClose())
                    {
                        
                    }

                    DirInfo.SetTestedFile();
#if DEBUG
                    Console.WriteLine("closing connection to server");
#endif
                    client.EndConnect();
                }
            }

#if DEBUG
            Console.WriteLine("done");
#endif

            Console.ReadKey();
        }
    }
}
