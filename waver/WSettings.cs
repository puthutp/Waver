using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace waver
{
    class WSettings
    {
        private const string SettingFileName = "server.txt";

        public static string Port { get; set; }
        public static string Address { get; set; }

        public static void DefaultSetting()
        {
            Port = "7313";
            Address = "";
        }

        public static bool TryReadSettingFile()
        {
            try
            {
                string[] document = File.ReadAllLines(SettingFileName);
                int lastIndex = document[0].IndexOf("]");
                Port = document[0].Substring(lastIndex + 1, document[0].Length - lastIndex - 1);
                lastIndex = document[1].IndexOf("]");
                Address = document[1].Substring(lastIndex + 1, document[1].Length - lastIndex - 1);

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
            result.Add("[Port]" + Port);
            result.Add("[Address]" + Address);

            File.WriteAllLines(SettingFileName, result.ToArray());
        }

    }
}
