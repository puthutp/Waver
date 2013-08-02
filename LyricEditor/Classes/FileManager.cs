using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LyricEditor
{
    public class FileManager
    {
        //DES
        private static string LineEndSpecialChar = Char.ConvertFromUtf32(14);
        private static byte[] BytesKey = ASCIIEncoding.ASCII.GetBytes("KEY8CHAR");
        private static DESCryptoHelper desCryptoHelper;

        // RSA
        private const string StringKey = "KEY";
        private const string SettingFileName = "settings.txt";
        private const string RecentFileName = "recent.txt";
        private const string PublicKeyFileName = "public.xml";
        private const string PrivateKeyFileName = "private.xml";
        private static RSACryptoHelper rsaCryptoHelper;

        private static int averageCharWidth = 9;
        private static Dictionary<char, int> characterWidth;

        private static int currentGroupNo = 0;

        private static Dictionary<string, string> propertyDict;
        private static int currentLine;

        static FileManager()
        {
            desCryptoHelper = new DESCryptoHelper();
            rsaCryptoHelper = new RSACryptoHelper();
            characterWidth = new Dictionary<char, int>();
            characterWidth.Add('a', 9);
            characterWidth.Add('b', 10);
            characterWidth.Add('c', 10);
            characterWidth.Add('d', 9);
            characterWidth.Add('e', 10);
            characterWidth.Add('f', 6);
            characterWidth.Add('g', 8);
            characterWidth.Add('h', 10);
            characterWidth.Add('i', 4);
            characterWidth.Add('j', 5);
            characterWidth.Add('k', 10);
            characterWidth.Add('l', 4);
            characterWidth.Add('m', 14);
            characterWidth.Add('n', 10);
            characterWidth.Add('o', 10);
            characterWidth.Add('p', 9);
            characterWidth.Add('q', 10);
            characterWidth.Add('r', 7);
            characterWidth.Add('s', 8);
            characterWidth.Add('t', 6);
            characterWidth.Add('u', 9);
            characterWidth.Add('v', 9);
            characterWidth.Add('w', 13);
            characterWidth.Add('x', 9);
            characterWidth.Add('y', 9);
            characterWidth.Add('z', 9);
            characterWidth.Add('A', 13);
            characterWidth.Add('B', 11);
            characterWidth.Add('C', 13);
            characterWidth.Add('D', 12);
            characterWidth.Add('E', 12);
            characterWidth.Add('F', 10);
            characterWidth.Add('G', 14);
            characterWidth.Add('H', 12);
            characterWidth.Add('I', 5);
            characterWidth.Add('J', 9);
            characterWidth.Add('K', 12);
            characterWidth.Add('L', 10);
            characterWidth.Add('M', 14);
            characterWidth.Add('N', 13);
            characterWidth.Add('O', 13);
            characterWidth.Add('P', 12);
            characterWidth.Add('Q', 13);
            characterWidth.Add('R', 13);
            characterWidth.Add('S', 11);
            characterWidth.Add('T', 11);
            characterWidth.Add('U', 13);
            characterWidth.Add('V', 12);
            characterWidth.Add('W', 16);
            characterWidth.Add('X', 13);
            characterWidth.Add('Y', 11);
            characterWidth.Add('Z', 12);
            characterWidth.Add('1', 9);
            characterWidth.Add('2', 10);
            characterWidth.Add('3', 9);
            characterWidth.Add('4', 9);
            characterWidth.Add('5', 10);
            characterWidth.Add('6', 9);
            characterWidth.Add('7', 9);
            characterWidth.Add('8', 10);
            characterWidth.Add('9', 9);
            characterWidth.Add('0', 9);
            characterWidth.Add('.', 5);
            characterWidth.Add(',', 5);
            characterWidth.Add('-', 10);
            characterWidth.Add('_', 10);
            characterWidth.Add('~', 10);
            characterWidth.Add(' ', 6);
            characterWidth.Add('?', 9);
            characterWidth.Add('!', 5);
            characterWidth.Add('/', 5);
            characterWidth.Add('(', 5);
            characterWidth.Add(')', 5);
            characterWidth.Add('&', 11);
        }

        public static bool TryReadSettingFile()
        {
            try
            {
                string[] document = File.ReadAllLines(SettingFileName);
                int lastIndex = document[0].IndexOf("]");
                Program.MIDIVolume = byte.Parse(document[0].Substring(lastIndex + 1, document[0].Length - lastIndex - 1));
                lastIndex = document[1].IndexOf("]");
                Program.Volume = int.Parse(document[1].Substring(lastIndex + 1, document[1].Length - lastIndex - 1));
                lastIndex = document[2].IndexOf("]");
                Program.MinimumVolume = int.Parse(document[2].Substring(lastIndex + 1, document[2].Length - lastIndex - 1));
                lastIndex = document[3].IndexOf("]");
                //Program.MIDIPlaybackOffset = double.Parse(document[3].Substring(lastIndex + 1, document[3].Length - lastIndex - 1));
                //lastIndex = document[4].IndexOf("]");
                Program.DBServer = document[3].Substring(lastIndex + 1, document[3].Length - lastIndex - 1);
                //lastIndex = document[5].IndexOf("]");
                //Program.DBName = document[5].Substring(lastIndex + 1, document[5].Length - lastIndex - 1);
                //lastIndex = document[6].IndexOf("]");
                //Program.DBUsername = document[6].Substring(lastIndex + 1, document[6].Length - lastIndex - 1);
                //lastIndex = document[7].IndexOf("]");
                //Program.DBPassword = document[7].Substring(lastIndex + 1, document[7].Length - lastIndex - 1);
                lastIndex = document[4].IndexOf("]");
                Program.FTPServer = document[4].Substring(lastIndex + 1, document[4].Length - lastIndex - 1);
                lastIndex = document[5].IndexOf("]");
                Program.FTPUsername = document[5].Substring(lastIndex + 1, document[5].Length - lastIndex - 1);
                lastIndex = document[6].IndexOf("]");
                Program.FTPPassword = document[6].Substring(lastIndex + 1, document[6].Length - lastIndex - 1);

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
            result.Add("[MIDIVolume]" + Program.MIDIVolume);
            result.Add("[Volume]" + Program.Volume);
            result.Add("[MinimalVolume]" + Program.MinimumVolume);
            //result.Add("[MIDIPlaybackOffset]" + Program.MIDIPlaybackOffset);

            result.Add("[DBServer]" + Program.DBServer);
            //result.Add("[DBName]" + Program.DBName);
            //result.Add("[DBUsername]" + Program.DBUsername);
            //result.Add("[DBPassword]" + Program.DBPassword);
            result.Add("[FTPServer]" + Program.FTPServer);
            result.Add("[FTPUsername]" + Program.FTPUsername);
            result.Add("[FTPPassword]" + Program.FTPPassword);

            File.WriteAllLines(SettingFileName, result.ToArray());
        }

        // no enc multiline
        public static void LoadFromFile(string filePath)
        {
            propertyDict = new Dictionary<string, string>();

            string[] document = File.ReadAllLines(filePath);
            
            currentLine = 0;

            ReadProperties(document);
            ReadFileHeader();
            ReadAppSetting();
            ReadContent(FileManager.GetContent(document));
        }

        // DES one line
        //public static void LoadFromFile(string filePath)
        //{
        //    string[] document = File.ReadAllLines(filePath);
        //    string stringDocument = desCryptoHelper.Decrypt(document[0], BytesKey);
        //    List<string> decryptedDocument = StringToListString(stringDocument);
        //    ReadFileHeader(FileManager.GetHeader(decryptedDocument.ToArray()));
        //    ReadAppSetting(FileManager.GetAppSetting(decryptedDocument.ToArray()));
        //    ReadContent(FileManager.GetContent(decryptedDocument.ToArray()));
        //}

        // RSA one line
        //public static void LoadFromFile(string filePath)
        //{
        //    CreateRSAXMLKey(PrivateKeyFileName, true);
        //    string[] document = File.ReadAllLines(filePath);
        //    string stringDocument = rsaCryptoHelper.Decrypt(document[0], PrivateKeyFileName);
        //    List<string> decryptedDocument = StringToListString(document[0]);
        //    //CreateRSAXMLKey(PrivateKeyFileName, true);
        //    //for (int i = 0; i < document.Length; i++)
        //    //    document[i] = desCryptoHelper.Decrypt(document[i], BytesKey);
        //    ReadFileHeader(FileManager.GetHeader(decryptedDocument.ToArray()));
        //    ReadAppSetting(FileManager.GetAppSetting(decryptedDocument.ToArray()));
        //    ReadContent(FileManager.GetContent(decryptedDocument.ToArray()));
        //}

        public static void SaveToFile(string filePath)
        {
            //CreateRSAXMLKey(PublicKeyFileName, false);
            List<string> head = FileManager.CreateFileHeader();
            List<string> appSetting = FileManager.CreateFileAppSetting();
            List<string> content = FileManager.CreateContent();
            head.AddRange(appSetting);
            head.AddRange(content);

            // No Encryption multiline
            File.WriteAllLines(Program.FileLocation, head.ToArray());

            // No Encryption one line
            //string stringToWrite = ListStringToString(head);
            ///File.WriteAllLines(Program.FileLocation, head.ToArray());


            // DES multi line
            //for (int i = 0; i < head.Count; i++)
            //    head[i] = desCryptoHelper.Encrypt(head[i], BytesKey);
            //File.WriteAllLines(Program.FileLocation, head.ToArray());

            // DES one line
            //string stringToWrite = ListStringToString(head);
            //stringToWrite = desCryptoHelper.Encrypt(stringToWrite, BytesKey);
            //StreamWriter streamWriter = new StreamWriter(Program.FileLocation);
            //streamWriter.Write(stringToWrite);
            //streamWriter.Flush();
            //streamWriter.Close();

            // RSA one line
            //string stringToWrite = ListStringToString(head);
            //stringToWrite = rsaCryptoHelper.Encrypt(stringToWrite, PublicKeyFileName);
            //StreamWriter streamWriter = new StreamWriter(Program.FileLocation);
            //streamWriter.Write(stringToWrite);
            //streamWriter.Flush();
            //streamWriter.Close();
        }

        public static int CalculateWordWidthInPixel(string input)
        {
            int result = 0;
            char[] charArray = input.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (characterWidth.Keys.Contains(charArray[i]))
                    result += characterWidth[charArray[i]];
                else
                    result += averageCharWidth;
            }
            return result;
        }

        private static void CreateRSAXMLKey(string path, bool isPrivate)
        {
            if (!File.Exists(path))
                rsaCryptoHelper.CreateXMLKey(StringKey, path, isPrivate);
        }

        private static void ReadProperties(string[] input)
        {
            int idxEnd;

            while (input[currentLine].IndexOf("# ") == 0)
            {
                idxEnd = input[currentLine].IndexOf(" : ");
                if (idxEnd > 2)
                {
                    propertyDict[input[currentLine].Substring(2, idxEnd - 2)] = input[currentLine].Substring(idxEnd + 3);
                }
                currentLine++;
            }

            while (input[currentLine].IndexOf("[") == 0)
            {
                idxEnd = input[currentLine].IndexOf("]");
                propertyDict[input[currentLine].Substring(1, idxEnd - 1)] = input[currentLine].Substring(idxEnd + 1);
                currentLine++;
            }
        }

        private static void ReadFileHeader()
        {
            string str;
            if (propertyDict.TryGetValue("Title", out str))
            {
                Program.TitleString = str;
            }
            if (propertyDict.TryGetValue("Artist", out str))
            {
                Program.ArtistString = str;
            }
            if (propertyDict.TryGetValue("Genre", out str))
            {
                Program.GenreString = str;
            }
            if (propertyDict.TryGetValue("Duration", out str))
            {
                Program.TimelineLength = double.Parse(str) / 1000;
            }
            if (propertyDict.TryGetValue("Offset", out str))
            {
                Program.MIDIPlaybackOffset = double.Parse(str);
            }
        }

        private static void ReadAppSetting()
        {
            string str;

            double beatPerMinute = 0;
            int gridInterval = 0;

            if (propertyDict.TryGetValue("MediaLocation", out str))
            {
                Program.MediaLocation = str;
            }
            if (propertyDict.TryGetValue("Minus1Location", out str))
            {
                Program.Minus1Location = str;
            }
            if (propertyDict.TryGetValue("PictureLocation", out str))
            {
                Program.PictureLocation = str;
            }
            if (propertyDict.TryGetValue("Difficulty", out str))
            {
                Program.Difficulty = str;
            }
            if (propertyDict.TryGetValue("Volume", out str))
            {
                Program.Volume = int.Parse(str);
            }
            if (propertyDict.TryGetValue("MidiVolume", out str))
            {
                Program.MIDIVolume = byte.Parse(str);
            }
            if (propertyDict.TryGetValue("BeatPerMinute", out str))
            {
                beatPerMinute = double.Parse(str);
            }
            if (propertyDict.TryGetValue("GridInterval", out str))
            {
                gridInterval = int.Parse(str);
            }
            if (propertyDict.TryGetValue("DelayLength", out str))
            {
                Program.DelayLength = double.Parse(str);
            }
            if (propertyDict.TryGetValue("Unlock", out str))
            {
                Program.Unlock = int.Parse(str);
            }
            Program.SetBeat(beatPerMinute, gridInterval);
        }

        private static string[] GetContent(string[] input)
        {
            string[] result = new string[input.Length - currentLine];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = input[i + currentLine];
            }
            return result;
        }

        private static void ReadContent(string[] input)
        {
            List<PitchBar> result = new List<PitchBar>();
            for (int i = 0; i < input.Length; i++)
            {
                string[] stringParts = input[i].Split(new char[] {' '});
                if (stringParts[0].Equals("-"))
                {
                    switch (stringParts[1])
                    {
                        case "start":
                            currentGroupNo = 0;
                            break;
                        case "break":
                            currentGroupNo++;
                            break;
                        case "end":
                            break;
                    }
                }
                else if (stringParts[0].Equals(":"))
                {
                    result.Add(ParseToPitchBar(stringParts));
                }
            }
            UpdateLyricBasedOnWordIndex(result);
            Program.PitchBars = result;
        }

        private static PitchBar ParseToPitchBar(string[] input)
        {
            PitchBar result = new PitchBar();

            result.StartTime = double.Parse(input[1]) / 1000;
            result.EndTime = double.Parse(input[2]) / 1000;
            result.NoteIndex = int.Parse(input[3]);
            result.GroupIndex = currentGroupNo;
            result.WordIndex = int.Parse(input[4]);
            result.Lyric = input[5];

            return result;
        }

        private static List<string> CreateFileHeader()
        {
            List<string> result = new List<string>();
            result.Add("# Title : " + Program.TitleString);
            result.Add("# Artist : " + Program.ArtistString);
            result.Add("# Genre : " + Program.GenreString);
            result.Add("# Duration : " + (Program.TimelineLength * 1000));
            result.Add("# Offset : " + Program.MIDIPlaybackOffset);
            return result;
        }

        private static List<string> CreateFileAppSetting()
        {
            List<string> result = new List<string>();
            result.Add("[MediaLocation]" + Program.MediaLocation);
            result.Add("[Minus1Location]" + Program.Minus1Location);
            result.Add("[PictureLocation]" + Program.PictureLocation);
            result.Add("[Difficulty]" + Program.Difficulty);
            result.Add("[NoteTotalLength]" + PitchBar.CalculateTotalPitchbarLength(Program.PitchBars));
            result.Add("[Volume]" + Program.Volume);
            result.Add("[MidiVolume]" + Program.MIDIVolume);
            result.Add("[BeatPerMinute]" + Program.BeatPerMinute.ToString());
            result.Add("[GridInterval]" + Program.GridInterval.ToString());
            result.Add("[DelayLength]" + Program.DelayLength.ToString());
            result.Add("[Unlock]" + Program.Unlock.ToString());
            return result;
        }

        private static List<string> CreateContent()
        {
            List<string> result = new List<string>();

            if (Program.PitchBars.Count > 0)
            {
                result.Add("- start " + Program.PitchBars[0].StartTime * 1000);

                currentGroupNo = Program.PitchBars[0].GroupIndex;
                for (int i = 0; i < Program.PitchBars.Count; i++)
                {
                    if (Program.PitchBars[i].GroupIndex > currentGroupNo)
                    {
                        currentGroupNo = Program.PitchBars[i].GroupIndex;
                        result.Add("- break " + (Program.PitchBars[i].StartTime * 1000 - Program.PitchBars[i - 1].EndTime * 1000));
                    }
                    result.Add(PitchBarToString(Program.PitchBars[i]));
                }
            }
            else
            {
                result.Add("- start 0");
            }
            result.Add("- end");
            return result;
        }

        private static string PitchBarToString(PitchBar pitchBars)
        {
            string result = "";
            result += ":";
            result += " " + (pitchBars.StartTime * 1000);
            result += " " + (pitchBars.EndTime * 1000);
            result += " " + pitchBars.NoteIndex;
            result += " " + pitchBars.WordIndex;
            result += " " + Program.CutDashIfExist(pitchBars.Lyric, false);

            return result;
        }

        private static void UpdateLyricBasedOnWordIndex(List<PitchBar> input)
        {
            if (input.Count > 1)
            {
                int lastGroupIndex = input[0].GroupIndex;
                for (int i = 1; i < input.Count; i++)
                {
                    if (input[i].WordIndex == input[i - 1].WordIndex)
                        input[i - 1].Lyric += Program.JOIN_WORD_CHAR;
                    if (input[i].GroupIndex != lastGroupIndex)
                    {
                        input[i - 1].IsLastWord = true;
                        lastGroupIndex = input[i].GroupIndex;
                    }
                }
            }
        }

        //private static void GetPitchBarTime(string input, out string left, out string right)
        //{
        //    int index = input.IndexOf(":");
        //    left = input.Substring(0, index);
        //    right = input.Substring(index + 1, input.Length - index - 1);
        //}

        //private static bool IsStartTimeOnly(string input)
        //{
        //    int lastIndex = input.IndexOf("]");
        //    return (input.Length - 1 == lastIndex);
        //}

        //private static double GetStartTime(string input)
        //{
        //    int firstIndex = input.IndexOf("[");
        //    int lastIndex = input.IndexOf("]");
        //    string startTimeString = input.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
        //    return double.Parse(startTimeString) / 1000;
        //}

        private static string ListStringToString(List<string> input)
        {
            for (int i = 0; i < input.Count; i++)
                input[i] += LineEndSpecialChar;
            string result = String.Concat(input.ToArray());
            return result;
        }
        
        private static List<string> StringToListString(string input)
        {
            List<string> result = new List<string>();
            char[] chars = input.ToCharArray();
            int lastIndex = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].ToString() == LineEndSpecialChar)
                {
                    result.Add(input.Substring(lastIndex, i - lastIndex));
                    lastIndex = i + 1;
                }
            }
            return result;
        }               

        public static string GetSafeFileName(string input)
        {
            try
            {
                int index = input.LastIndexOf('\\');
                return input.Substring(index + 1, input.Length - index - 1);
            }
            catch (Exception)
            {
                return "-";
            }
        }

        public static string GetFileExtension(string input)
        {
            try
            {
                int index = input.LastIndexOf('.');
                return input.Substring(index + 1, input.Length - index - 1);
            }
            catch (Exception)
            {
                return "-";
            }
        }

        // recent file voids
        public static bool GetRecentFile(out string[] result)
        {
            result = new string[0];
            try
            {
                result = File.ReadAllLines(RecentFileName);
                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.WriteAllLines(RecentFileName, result);
                }
                catch (Exception) { }
                return false;
            }
        }

        public static void AppendToRecent(string filePath)
        {
            try
            {
                List<string> result = File.ReadAllLines(RecentFileName).ToList();
                bool found = false;
                int i = 0;
                while (i < result.Count && !found)
                {
                    if (result[i] == filePath)
                    {
                        found = true;
                    }
                    i++;
                }
                if (!found) result.Add(filePath);
                File.WriteAllLines(RecentFileName, result.ToArray());
            }
            catch (Exception) { }
        }
    }
}
