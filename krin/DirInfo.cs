using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace krin
{
    class DirInfo
    {
        private const string testedWavFileName = "test.txt";
        private const string testedDirFileName = "dir.txt";

        public static string CurrentDir { get; set; }

        private static List<string> dateDirs;

        public static HashSet<string> TestedFiles { get; set; }
        public static HashSet<string> TestedDirs { get; set; }

        public static List<string> UntestedDirs { get; set; }

        public static void Initialize()
        {
            RetrieveDateDirs();
            RetrieveUntestedDirs();
        }

        public static void RetrieveDateDirs()
        {
            //string[] dir = Directory.GetDirectories(Settings.MainDir, "*", SearchOption.AllDirectories);

            //string[] monthDir = Directory.GetDirectories(Settings.MainDir);

            //dateDirs = new List<string>();
            //foreach (string str in dir)
            //{
            //    if (!monthDir.Contains(str))
            //    {
            //        dateDirs.Add(str);
            //    }
            //}

            dateDirs = Directory.GetDirectories(Settings.MainDir).ToList();
        }

        public static void RetrieveUntestedDirs()
        {
            TestedDirs = ReadTestedFile(/*Settings.MainDir + "/" + */testedDirFileName);
            UntestedDirs = new List<string>();

            foreach (string dirName in dateDirs)
            {
                if (!TestedDirs.Contains(dirName))
                {
                    UntestedDirs.Add(dirName);
                }
            }
        }

        public static void AppendTestedDir(string dirName)
        {
            using (StreamWriter output = File.AppendText(/*Settings.MainDir + "/" + */testedDirFileName))
            {
                output.WriteLine(dirName);
            }
        }

        public static void AppendTestedFile(string fileName, float fileScore)
        {
            using (StreamWriter output = File.AppendText(/*CurrentDir + "/" + */testedWavFileName))
            {
                output.WriteLine(fileName + " " + fileScore);
            }
        }

        public static HashSet<string> ReadTestedFile(string testName)
        {
            try
            {
                string[] document = File.ReadAllLines(testName);

                HashSet<string> hashSet = new HashSet<string>();

                int idx;
                
                foreach (var doc in document)
                {
                    idx = Math.Max(0, doc.LastIndexOf(" "));
                    hashSet.Add(doc.Substring(0, idx));
                }

                return hashSet;
            }
            catch (FileNotFoundException)
            {
                using (File.Create(testName)) { }

                return new HashSet<string>();
            }
        }

        public static void SetTestedFile()
        {
            TestedFiles = ReadTestedFile(/*CurrentDir + "/" + */testedWavFileName);
        }

    }
}
