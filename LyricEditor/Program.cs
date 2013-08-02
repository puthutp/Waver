using GameLibrary.Input.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LyricEditor
{
    static class Program
    {        
        //
        // public static UI const
        //
        public readonly static double MAXIMUM_DURATION_IN_SENTENCE = 8;
        public readonly static int MAXIMUM_CHARACTER_WIDTH_IN_SENTENCE = 480;
        public readonly static int PANEL_PIANO_START_NOTE_INDEX = 3;
        public readonly static int PANEL_PIANO_NOTE_COUNT = 60;
        public readonly static int GRID_HORIZONTAL = 80;
        public readonly static int GRID_VERTICAL = 20;
        public readonly static int NOTE_INTERVAL = 12;
        public readonly static int MIDI_OFFSET = -33;
        public readonly static string JOIN_WORD_CHAR = "-";

        //
        // public static properties
        //
        public static List<PitchBar> PitchBars { get; set; }
        public static double DelayLength { get; set; }
        public static List<string> RegisteredNotes { get; set; }
        public static double CurrentPlayingTimeline { get; set; }
        public static int BackgroundWidth
        {
            get { return GridInterval * GRID_HORIZONTAL; }
        }        
        public static int BackgroundHeight
        {
            get { return NOTE_INTERVAL * GRID_VERTICAL; }
        }
        public static double PixelPerSecond { get; private set; }
        public static MIDIPlayer MIDIPlayer { get; set; }
        public static object[] GridIntervals { get; private set; }
        public static SoundCaptureDevice SoundCaptureDevice { get; set; }
        public static int MinimumVolume { get; set; }

        public static int SentenceCount { get; set; }

        //
        // public static setting properties
        //
        public static double MIDIPlaybackOffset { get; set; }
        public static string FileLocation { get; set; }
        public static double BeatPerMinute { get; private set; }
        public static int GridInterval { get; private set; }
        public static int Volume { get; set; }
        public static byte MIDIVolume { get; set; }
        public static bool IsAutoApply { get; set; }
        public static double MinimumLength { get; set; }
        public static int AudioRate { get; set; }

        //
        // public static media properties
        //
        public static double TimelineLength { get; set; }
        public static string TitleString { get; set; }
        public static string ArtistString { get; set; }
        public static string GenreString { get; set; }
        public static string MediaLocation { get; set; }
        public static string Minus1Location { get; set; }

        public static string PictureLocation { get; set; }
        public static string Difficulty { get; set; }
        public static int Unlock { get; set; }

        //
        // public static database and ftp properties
        //
        public static string DBServer { get; set; }
        //public static string DBName { get; set; }
        //public static string DBUsername { get; set; }
        //public static string DBPassword { get; set; }
        public static string FTPServer { get; set; }
        public static string FTPUsername { get; set; }
        public static string FTPPassword { get; set; }

        // 
        // constructors
        //
        static Program()
        {
            //SoundCaptureDevice = SoundCaptureDevice.GetDefaultDevice();
            SoundCaptureDevice = null;
            MinimumLength = 0.05;
            IsAutoApply = true;
            GridIntervals = new object[2];
            GridIntervals[0] = 3;
            GridIntervals[1] = 4;
            RegisteredNotes = new List<string>();
            RegisteredNotes.Add("A1");      // 0
            RegisteredNotes.Add("A#1");     // 1
            RegisteredNotes.Add("B1");      // 2
            RegisteredNotes.Add("C2");      // 3
            RegisteredNotes.Add("C#2");
            RegisteredNotes.Add("D2");
            RegisteredNotes.Add("D#2");
            RegisteredNotes.Add("E2");
            RegisteredNotes.Add("F2");
            RegisteredNotes.Add("F#2");
            RegisteredNotes.Add("G2");
            RegisteredNotes.Add("G#2");
            RegisteredNotes.Add("A2");
            RegisteredNotes.Add("A#2");
            RegisteredNotes.Add("B2");
            RegisteredNotes.Add("C3");      // 15
            RegisteredNotes.Add("C#3");
            RegisteredNotes.Add("D3");
            RegisteredNotes.Add("D#3");
            RegisteredNotes.Add("E3");
            RegisteredNotes.Add("F3");
            RegisteredNotes.Add("F#3");
            RegisteredNotes.Add("G3");
            RegisteredNotes.Add("G#3");
            RegisteredNotes.Add("A3");
            RegisteredNotes.Add("A#3");
            RegisteredNotes.Add("B3");
            RegisteredNotes.Add("C4");      // 27
            RegisteredNotes.Add("C#4");
            RegisteredNotes.Add("D4");
            RegisteredNotes.Add("D#4");
            RegisteredNotes.Add("E4");
            RegisteredNotes.Add("F4");
            RegisteredNotes.Add("F#4");
            RegisteredNotes.Add("G4");
            RegisteredNotes.Add("G#4");
            RegisteredNotes.Add("A4");
            RegisteredNotes.Add("A#4");
            RegisteredNotes.Add("B4");
            RegisteredNotes.Add("C5");
            RegisteredNotes.Add("C#5");
            RegisteredNotes.Add("D5");
            RegisteredNotes.Add("D#5");
            RegisteredNotes.Add("E5");
            RegisteredNotes.Add("F5");
            RegisteredNotes.Add("F#5");
            RegisteredNotes.Add("G5");
            RegisteredNotes.Add("G#5");
            RegisteredNotes.Add("A5");
            RegisteredNotes.Add("A#5");
            RegisteredNotes.Add("B5");
            RegisteredNotes.Add("C6");
            RegisteredNotes.Add("C#6");
            RegisteredNotes.Add("D6");
            RegisteredNotes.Add("D#6");
            RegisteredNotes.Add("E6");
            RegisteredNotes.Add("F6");
            RegisteredNotes.Add("F#6");
            RegisteredNotes.Add("G6");
            RegisteredNotes.Add("G#6");
            RegisteredNotes.Add("A6");
            RegisteredNotes.Add("A#6");
            RegisteredNotes.Add("B6");
            //RegisteredNotes.Add("C7");
            //RegisteredNotes.Add("C#7");
            //RegisteredNotes.Add("D7");
            //RegisteredNotes.Add("D#7");
            //RegisteredNotes.Add("E7");
            //RegisteredNotes.Add("F7");
            //RegisteredNotes.Add("F#7");
            //RegisteredNotes.Add("G7");
            //RegisteredNotes.Add("G#7");
            //RegisteredNotes.Add("A7");
            //RegisteredNotes.Add("A#7");
            //RegisteredNotes.Add("B7");
            //RegisteredNotes.Add("C8");
            //RegisteredNotes.Add("C#8");
            //RegisteredNotes.Add("D8");
            //RegisteredNotes.Add("D#8");
            //RegisteredNotes.Add("E8");
            //RegisteredNotes.Add("F8");
            //RegisteredNotes.Add("F#8");
            //RegisteredNotes.Add("G8");
            //RegisteredNotes.Add("G#8");
            //RegisteredNotes.Add("A8");
            //RegisteredNotes.Add("A#8");
            //RegisteredNotes.Add("B8");
            Reset();
            if (!FileManager.TryReadSettingFile())
                FileManager.CreateSettingFile();
        }

        //
        // methods
        //
        public static void SetBeat(double beatPerMinute, int gridInterval)
        {
            BeatPerMinute = beatPerMinute;
            GridInterval = gridInterval;
            PixelPerSecond = BeatPerMinute / 60 * GRID_HORIZONTAL;
        }

        public static void Reset()
        {
            FileLocation = "";
            TitleString = "Unknown";
            ArtistString = "Unknown";
            GenreString = "Unknown";
            MIDIPlaybackOffset = 0;
            MediaLocation = "";
            Minus1Location = "";
            PictureLocation = "";
            Difficulty = "3";
            TimelineLength = 180;
            DelayLength = 0;
            CurrentPlayingTimeline = 0;
            AudioRate = 100;
            Unlock = 0;
            SetBeat(120, 4);
            SentenceCount = 0;
        }

        public static bool TryTimespanStringToDouble(string input, out double result)
        {
            try
            {
                int titikduaIndex = input.IndexOf(":", 0);
                int firstNumbers = int.Parse(input.Substring(0, titikduaIndex));
                input = input.Substring(titikduaIndex + 1, input.Length - titikduaIndex - 1);
                titikduaIndex = input.IndexOf(".", 0);
                int secondNumbers = int.Parse(input.Substring(0, titikduaIndex));
                int lastNumber = int.Parse(AddZero(input.Substring(titikduaIndex + 1, input.Length - titikduaIndex - 1)));
                result = firstNumbers * 60 + secondNumbers + ((double)lastNumber / 1000);
                return true;
            }
            catch (Exception)
            {
                result = 0;
                return false;
            }
        }

        public static string DoubleToTimespanString(double seconds)
        {
            TimeSpan timespan = TimeSpan.FromSeconds(seconds);
            return String.Format("{0:00}:{1:00}.{2:000}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
        }

        public static List<PitchBar> NotesToPitchBars(List<Note> notes)
        {
            List<PitchBar> result = new List<PitchBar>();
            for (int i = 0; i < notes.Count; i++)
            {
                PitchBar newPitchBar = new PitchBar();
                newPitchBar.StartTime = notes[i].StartPoint;
                newPitchBar.EndTime = notes[i].EndPoint;
                newPitchBar.NoteIndex = notes[i].NoteIndex;
                result.Add(newPitchBar);
            }
            return result;
        }

        public static List<string> ReadLyricFromPitchBars(List<PitchBar> input)
        {
            List<string> result = new List<string>();
            int i = 0;
            while (i < input.Count)
            {
                string newLine = "";
                do
                {
                    newLine += CutDashIfExist(input[i].Lyric, true);
                    i++;
                }
                while (i < input.Count && !input[i - 1].IsLastWord);
                result.Add(newLine);
            }
            return result;
        }

        public static void ProcessGroupAndWordIndex(List<PitchBar> sortedPitchBar)
        {
            int currentGroupIndex = 0;
            int currentWordIndex = 0;
            for (int i = 0; i < sortedPitchBar.Count; i++)
            {
                sortedPitchBar[i].GroupIndex = currentGroupIndex;
                sortedPitchBar[i].WordIndex = currentWordIndex;
                if (sortedPitchBar[i].IsLastWord)
                {
                    currentGroupIndex++;
                    currentWordIndex = 0;
                }
                else
                    if (!sortedPitchBar[i].Lyric.EndsWith(JOIN_WORD_CHAR))
                        currentWordIndex++;
            }
        }

        public static List<double> ProcessSentenceDuration(List<PitchBar> sortedPitchBar)
        {
            List<double> durationList = new List<double>();

            int startSentenceIndex = 0;

            double startSentencePoint;
            double endSentencePoint;
            double totalDuration;

            for (int i = 0; i < sortedPitchBar.Count; i++)
            {
                if (sortedPitchBar[i].IsLastWord || i == sortedPitchBar.Count - 1)
                {
                    startSentencePoint = sortedPitchBar[startSentenceIndex].StartTime;
                    endSentencePoint = sortedPitchBar[i].EndTime;
                    totalDuration = endSentencePoint - startSentencePoint;

                    durationList.Add(totalDuration);

                    if (totalDuration > 8)
                    {
                        for (int j = startSentenceIndex; j <= i; j++)
                        {
                            sortedPitchBar[j].IsTooLong = true;
                        }
                    }
                    else sortedPitchBar[i].IsTooLong = false;

                    startSentenceIndex = i + 1;
                } else
                {
                    sortedPitchBar[i].IsTooLong = false;
                }
            }

            return durationList;
        }

        public static string GetBeatIndex(int input)
        {
            return ((input / GridInterval) + 1) + "." + ((input % GridInterval + 1));
        }

        public static bool TryParseNoteIndex(string input, out int noteIndex)
        {
            if (RegisteredNotes.Contains(input))
            {
                noteIndex = RegisteredNotes.IndexOf(input);
                return true;
            }
            else
            {
                noteIndex = 0;
                return false;
            }
        }

        public static string CutDashIfExist(string input, bool addSpace)
        {
            if (input != null)
            {
                if (input.EndsWith(JOIN_WORD_CHAR))
                    return input.Substring(0, input.Length - 1);
                else if (addSpace)
                    return input + " ";
                else
                    return input;
            }
            else
                return null;
        }

        private static string AddZero(string input)
        {
            while (input.Length < 3)
                input += "0";
            return input;
        }

        //
        // entry point
        //
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            //Application.Run(new FormImportMIDI());
        }
    }
}
