using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;
using System.Diagnostics;

namespace waver
{
    class Program
    {
        public static string fileName;
        public static string logNote;
        public static string logScore;

        static void Main(string[] args)
        {
            Console.WriteLine("Scoring wav file(s) server");
            Console.WriteLine();

            /***************************************************************/

            Console.WriteLine("Reading setting file");
            WSettings.DefaultSetting();
            if (!WSettings.TryReadSettingFile())
            {
                Console.WriteLine("Read setting failed, creating new setting file");
                WSettings.CreateSettingFile();
            }

            Console.WriteLine();

            ControllerServer server = new ControllerServer();
            server.StartServer();

            Console.WriteLine();

            /***************************************************************/

//#if TRACE
//            fileName = Path.GetFileNameWithoutExtension(args[0]);
//            logNote = "log-" + fileName + "-notes.txt";
//            logScore = "log-" + fileName + "-score.txt";
//#endif

//            ////Stopwatch stopwatch = new Stopwatch();
//            ////stopwatch.Start();

//            List<string> sampleList = Directory.GetFiles("sample", "*_p.wav").ToList();

//            foreach (var sample in sampleList)
//            {
//                IdolWave wave0 = IdolWaveLoader.LoadWaveFile(sample);

//                List<IdolWaveNote> result0 = IdolWaveLoader.GetWaveNotes(wave0);

//                Scoring scoring = new Scoring();
//                float score = scoring.GetScore(result0);

//                Console.WriteLine();
//                Console.WriteLine(sample);
//                Console.WriteLine("final score: " + score);
//            }

            /***********************/

            //IdolWave wave0 = IdolWaveLoader.LoadWaveFile(args[0]);

            //List<IdolWaveNote> result0 = IdolWaveLoader.GetWaveNotes(wave0);

            //Scoring scoring = new Scoring();
            //float score = scoring.GetScore(result0);

            //Console.WriteLine(args[0]);

//#if TRACE 
//            Console.WriteLine("");
//            File.WriteAllText(logNote, "");

//            int ax = 0;
//            while (ax < result0.Count)
//            {
//                Console.WriteLine(result0[ax].NoteName + " " + result0[ax].freq + " " + result0[ax].max);
//                File.AppendAllText(logNote, result0[ax].NoteName + " " + result0[ax].freq + " " + result0[ax].max + "\n");

//                ax++;
//                if (ax % 8 == 0)
//                {
//                    File.AppendAllText(logNote, (ax / 8).ToString() + "\n");
//                    Console.WriteLine();
//                }
//            }
//            Console.WriteLine();

//            File.AppendAllText(logScore, "\n");
//            File.AppendAllText(logScore, "final score: " + score + "\n");
//#endif

            //Console.WriteLine();
            //Console.WriteLine("final score: " + score);

            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);

            /***************************************************************/

            Console.ReadKey();
        }
    }
}
