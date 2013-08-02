using System;
using System.Collections.Generic;

namespace waver
{
    public class IdolSoundAnalyzer
    {
        // Reasonable limits for the singing voice are defined by the musical staff, which ranges from F2 (87.31 Hz) just below the bass staff, to G5 (784 Hz) just above the treble staff.
        // http://www.dlib.org/dlib/may97/meldex/05witten.html
        const double MinFreq = 60;
        const double MaxFreq = 1100;

        const int magicNumber = 2;

        const int defaultIdx = -99;

        public static IdolWaveNote ProcessData(short[] data, int sampleRate)
        {
            double[] x = new double[data.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = data[i];
            }

            double[] spectr = IdolFFTAlgorithm.Calculate(x);
            int minSpectr = (int)(MinFreq * spectr.Length / sampleRate / magicNumber) + 1;
            int index = 1;
            double max = spectr[index];
            int usefullMaxSpectr = Math.Min(spectr.Length,
                (int)(MaxFreq * spectr.Length / sampleRate / magicNumber) + 1);
            //Console.WriteLine(usefullMaxSpectr + " " + spectr.Length + " " + usefullMaxSpectr * sampleRate / spectr.Length);
            //Console.WriteLine("= " + max);

            for (int i = index; i < usefullMaxSpectr; i++)
            {
                if (max < spectr[i])
                {
                    max = spectr[i]; index = i;
                }
            }
            
            //Console.WriteLine("- " + max + " " + index);

            double freq = (double) magicNumber * sampleRate * index / spectr.Length;
             if (freq < MinFreq) freq = 0;

            IdolWaveNote waveNote = new IdolWaveNote();
            waveNote.freq = freq;
            waveNote.max = max;

            return waveNote;
            //return freq;
        }

        public static double[] GetFFTValues(short[] data)
        {
            double[] x = new double[data.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = data[i];
            }

            double[] spectr = IdolFFTAlgorithm.Calculate(x);

            return spectr;
        }

        #region notedictionary
        public static readonly Dictionary<int, string> NOTE_NAMES = new Dictionary<int, string>()
        { 
           {4978, "D#8"},
           {4699, "D8"},
           {4435, "C#8"},
           {4186, "C8"},
           {3951, "B7"},
           {3729, "A#7"},
           {3520, "A7"},
           {3322, "G#7"},
           {3136, "G7"},
           {2960, "F#7"},
           {2794, "F7"},
           {2637, "E7"},
           {2489, "D#7"},
           {2349, "D7"},
           {2217, "C#7"},
           {2093, "C7"},
           {1976, "B6"},
           {1865, "A#6"},
           {1760, "A6"},
           {1661, "G#6"},
           {1568, "G6"},
           {1480, "F#6"},
           {1397, "F6"},
           {1319, "E6"},
           {1245, "D#6"},
           {1175, "D6"},
           {1109, "C#6"},
           {1046, "C6"},
           {988, "B5"},
           {932, "A#5"},
           {880, "A5"},
           {831, "G#5"},
           {784, "G5"},
           {740, "F#5"},
           {698, "F5"},
           {659, "E5"},
           {622, "D#5"},
           {587, "D5"},
           {554, "C#5"},
           {523, "C5"},
           {494, "B4"},
           {466, "A#4"},
           {440, "A4"},
           {415, "G#4"},
           {392, "G4"},
           {370, "F#4"},
           {349, "F4"},
           {330, "E4"},
           {311, "D#4"},
           {294, "D4"},
           {277, "C#4"},
           {262, "C4"},
           {247, "B3"},
           {233, "A#3"},
           {220, "A3"},
           {208, "G#3"},
           {196, "G3"},
           {185, "F#3"},
           {175, "F3"},
           {165, "E3"},
           {156, "D#3"},
           {147, "D3"},
           {139, "C#3"},
           {131, "C3"},
           {123, "B2"},
           {117, "A#2"},
           {110, "A2"},
           {104, "G#2"},
           {98, "G2"},
           {92, "F#2"},
           {87, "F2"},
           {82, "E2"},
           {78, "D#2"},
           {73, "D2"},
           {69, "C#2"},
           {65, "C2"},
           {62, "B1"},
           {58, "A#1"},
           {55, "A1"}
        };

        public static readonly Dictionary<int, int> NOTE_IDX = new Dictionary<int, int>()
        {
           {4978, 78},      // "D#8"},
           {4699, 77},      // "D8"},
           {4435, 76},      // "C#8"},
           {4186, 75},      // "C8"},
           {3951, 74},      // "B7"},
           {3729, 73},      // "A#7"},
           {3520, 72},      // "A7"},
           {3322, 71},      // "G#7"},
           {3136, 70},      // "G7"},
           {2960, 69},      // "F#7"},
           {2794, 68},      // "F7"},
           {2637, 67},      // "E7"},
           {2489, 66},      // "D#7"},
           {2349, 65},      // "D7"},
           {2217, 64},      // "C#7"},
           {2093, 63},      // "C7"},
           {1976, 62},      // "B6"},
           {1865, 61},      // "A#6"},
           {1760, 60},      // "A6"},
           {1661, 59},      // "G#6"},
           {1568, 58},      // "G6"},
           {1480, 57},      // "F#6"},
           {1397, 56},      // "F6"},
           {1319, 55},      // "E6"},
           {1245, 54},      // "D#6"},
           {1175, 53},      // "D6"},
           {1109, 52},      // "C#6"},
           {1046, 51},      // "C6"},
           {988, 50},       // "B5"},
           {932, 49},       // "A#5"},
           {880, 48},       // "A5"},
           {831, 47},       // "G#5"},
           {784, 46},       // "G5"},
           {740, 45},       // "F#5"},
           {698, 44},       // "F5"},
           {659, 43},       // "E5"},
           {622, 42},       // "D#5"},
           {587, 41},       // "D5"},
           {554, 40},       // "C#5"},
           {523, 39},       // "C5"},
           {494, 38},       // "B4"},
           {466, 37},       // "A#4"},
           {440, 36},       // "A4"},
           {415, 35},       // "G#4"},
           {392, 34},       // "G4"},
           {370, 33},       // "F#4"},
           {349, 32},       // "F4"},
           {330, 31},       // "E4"},
           {311, 30},       // "D#4"},
           {294, 29},       // "D4"},
           {277, 28},       // "C#4"},
           {262, 27},       // "C4"},
           {247, 26},       // "B3"},
           {233, 25},       // "A#3"},
           {220, 24},       //"A3"},
           {208, 23},       // "G#3"},
           {196, 22},       // "G3"},
           {185, 21},       // "F#3"},
           {175, 20},       // "F3"},
           {165, 19},       // "E3"},
           {156, 18},       // "D#3"},
           {147, 17},       // "D3"},
           {139, 16},       // "C#3"},
           {131, 15},       // "C3"},
           {123, 14},       // "B2"},
           {117, 13},       // "A#2"},
           {110, 12},       // "A2"},
           {104, 11},       // "G#2"},
           {98, 10},        // "G2"},
           {92, 9},         // "F#2"},
           {87, 8},         // "F2"},
           {82, 7},         //"E2"},
           {78, 6},         //"D#2"},
           {73, 5},         // "D2"},
           {69, 4},         // "C#2"},
           {65, 3},         // "C2"},
           {62, 2},         // "B1"},
           {58, 1},         // "A#1"},
           {55, 0},         // "A1"
        };
        #endregion

        static double ToneStep = Math.Pow(2, 1.0 / 12);

        public static void FindClosestNote(int frequency, out int closestFrequency, out string noteName)
        {
            const double AFrequency = 440.0;
            //const int ToneIndexOffsetToPositives = 120;

            if (frequency != 0)
            {
                int toneIndex = (int)Math.Round(Math.Log(frequency / AFrequency, ToneStep));
                //noteName = NoteNames[(ToneIndexOffsetToPositives + toneIndex) % NoteNames.Length];
                closestFrequency = (int)Math.Round(Math.Pow(ToneStep, toneIndex) * AFrequency);
                if (NOTE_NAMES.ContainsKey(closestFrequency))
                {
                    noteName = NOTE_NAMES[closestFrequency];
                }
                else
                {
                    noteName = "*";
                }
            }
            else
            {
                noteName = "*";
                closestFrequency = 0;
            }
        }

        public static void FindClosestNote(int frequency, out int closestFrequency, out int noteIdx)
        {
            const double AFrequency = 440.0;
            //const int ToneIndexOffsetToPositives = 120;

            if (frequency != 0)
            {
                int toneIndex = (int)Math.Round(Math.Log(frequency / AFrequency, ToneStep));
                //noteName = NoteNames[(ToneIndexOffsetToPositives + toneIndex) % NoteNames.Length];
                closestFrequency = (int)Math.Round(Math.Pow(ToneStep, toneIndex) * AFrequency);
                if (NOTE_IDX.ContainsKey(closestFrequency))
                {
                    noteIdx = NOTE_IDX[closestFrequency];
                }
                else
                {
                    noteIdx = defaultIdx;
                }
            }
            else
            {
                noteIdx = defaultIdx;
                closestFrequency = 0;
            }
        }

        public static void FindClosestNote(int frequency, out int closestFrequency, out int noteIdx, out string noteName)
        {
            const double AFrequency = 440.0;
            //const int ToneIndexOffsetToPositives = 120;

            if (frequency != 0)
            {
                int toneIndex = (int)Math.Round(Math.Log(frequency / AFrequency, ToneStep));
                //noteName = NoteNames[(ToneIndexOffsetToPositives + toneIndex) % NoteNames.Length];
                closestFrequency = (int)Math.Round(Math.Pow(ToneStep, toneIndex) * AFrequency);
                if (NOTE_IDX.ContainsKey(closestFrequency))
                {
                    noteIdx = NOTE_IDX[closestFrequency];
                    noteName = NOTE_NAMES[closestFrequency];
                }
                else
                {
                    noteIdx = defaultIdx;
                    noteName = "";
                }
            }
            else
            {
                //Console.WriteLine("hai");
                noteIdx = defaultIdx;
                noteName = "";
                closestFrequency = 0;
            }
        }
    }
}
