using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace krin
{
    class Settings
    {
        private const string SettingFileName = "settings.txt";

        public static string Address { get; set; }
        public static string Port { get; set; }
        public static string MainDir { get; set; }
        public static string Iteration { get; set; }

        public static void DefaultSetting()
        {
            Address = "127.0.0.1";
            Port = "7313";
            MainDir = Directory.GetCurrentDirectory();
            Iteration = "1";
        }

        public static bool TryReadSettingFile()
        {
            try
            {
                string[] document = File.ReadAllLines(SettingFileName);
                int lastIndex = document[0].IndexOf("]");
                Address = document[0].Substring(lastIndex + 1, document[0].Length - lastIndex - 1);
                lastIndex = document[1].IndexOf("]");
                Port = document[1].Substring(lastIndex + 1, document[1].Length - lastIndex - 1);
                lastIndex = document[2].IndexOf("]");
                MainDir = document[2].Substring(lastIndex + 1, document[2].Length - lastIndex - 1);
                lastIndex = document[3].IndexOf("]");
                Iteration = document[3].Substring(lastIndex + 1, document[3].Length - lastIndex - 1);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CreateSettingFile()
        {
            List<string> result = new List<string>();
            result.Add("[Address]" + Address);
            result.Add("[Port]" + Port);
            result.Add("[Directory]" + MainDir);
            result.Add("[Iteration]" + Iteration);

            File.WriteAllLines(SettingFileName, result.ToArray());
        }

    }
}
