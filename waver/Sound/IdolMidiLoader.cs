using System;
using System.IO;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif

namespace KaraokeIdol
{
    public static class IdolMidiLoader
    {
        public static readonly float MICROSECONDS_TO_SECONDS_FACTOR = 1000000;
        public static readonly Dictionary<int, string> NOTE_NAMES = new Dictionary<int,string>()
        { 
           {111, "D#8"},
           {110, "D8"},
           {109, "C#8"},
           {108, "C8"},
           {107, "B7"},
           {106, "A#7"},
           {105, "A7"},
           {104, "G#7"},
           {103, "G7"},
           {102, "F#7"},
           {101, "F7"},
           {100, "E7"},
           {99, "D#7"},
           {98, "D7"},
           {97, "C#7"},
           {96, "C7"},
           {95, "B6"},
           {94, "A#6"},
           {93, "A6"},
           {92, "G#6"},
           {91, "G6"},
           {90, "F#6"},
           {89, "F6"},
           {88, "E6"},
           {87, "D#6"},
           {86, "D6"},
           {85, "C#6"},
           {84, "C6"},
           {83, "B5"},
           {82, "A#5"},
           {81, "A5"},
           {80, "G#5"},
           {79, "G5"},
           {78, "F#5"},
           {77, "F5"},
           {76, "E5"},
           {75, "D#5"},
           {74, "D5"},
           {73, "C#5"},
           {72, "C5"},
           {71, "B4"},
           {70, "A#4"},
           {69, "A4"},
           {68, "G#4"},
           {67, "G4"},
           {66, "F#4"},
           {65, "F4"},
           {64, "E4"},
           {63, "D#4"},
           {62, "D4"},
           {61, "C#4"},
           {60, "C4"},
           {59, "B3"},
           {58, "A#3"},
           {57, "A3"},
           {56, "G#3"},
           {55, "G3"},
           {54, "F#3"},
           {53, "F3"},
           {52, "E3"},
           {51, "D#3"},
           {50, "D3"},
           {49, "C#3"},
           {48, "C3"},
           {47, "B2"},
           {46, "A#2"},
           {45, "A2"},
           {44, "G#2"},
           {43, "G2"},
           {42, "F#2"},
           {41, "F2"},
           {40, "E2"},
           {39, "D#2"},
           {38, "D2"},
           {37, "C#2"},
           {36, "C2"},
           {35, "B1"},
           {34, "A#1"},
           {33, "A1"}
        };
        public static readonly Dictionary<int, int> NOTE_IDX = new Dictionary<int, int>()
        { 
           {111, 78},   // "D#8"},
           {110, 77},   // "D8"},
           {109, 76},   // "C#8"},
           {108, 75},   // "C8"},
           {107, 74},   // "B7"},
           {106, 73},   // "A#7"},
           {105, 72},   // "A7"},
           {104, 71},   // "G#7"},
           {103, 70},   // "G7"},
           {102, 69},   // "F#7"},
           {101, 68},   // "F7"},
           {100, 67},   // "E7"},
           {99, 66},    // "D#7"},
           {98, 65},    // "D7"},
           {97, 64},    // "C#7"},
           {96, 63},    // "C7"},
           {95, 62},    // "B6"},
           {94, 61},    // "A#6"},
           {93, 60},    // "A6"},
           {92, 59},    // "G#6"},
           {91, 58},    // "G6"},
           {90, 57},    // "F#6"},
           {89, 56},    // "F6"},
           {88, 55},    // "E6"},
           {87, 54},    // "D#6"},
           {86, 53},    // "D6"},
           {85, 52},    // "C#6"},
           {84, 51},    // "C6"},
           {83, 50},    // "B5"},
           {82, 49},    // "A#5"},
           {81, 48},    // "A5"},
           {80, 47},    // "G#5"},
           {79, 46},    // "G5"},
           {78, 45},    // "F#5"},
           {77, 44},    // "F5"},
           {76, 43},    // "E5"},
           {75, 42},    // "D#5"},
           {74, 41},    // "D5"},
           {73, 40},    // "C#5"},
           {72, 39},    // "C5"},
           {71, 38},    // "B4"},
           {70, 37},    // "A#4"},
           {69, 36},    // "A4"},
           {68, 35},    // "G#4"},
           {67, 34},    // "G4"},
           {66, 33},    // "F#4"},
           {65, 32},    // "F4"},
           {64, 31},    // "E4"},
           {63, 30},    // "D#4"},
           {62, 29},    // "D4"},
           {61, 28},    // "C#4"},
           {60, 27},    // "C4"},
           {59, 26},    // "B3"},
           {58, 25},    // "A#3"},
           {57, 24},    // "A3"},
           {56, 23},    // "G#3"},
           {55, 22},    // "G3"},
           {54, 21},    // "F#3"},
           {53, 20},    // "F3"},
           {52, 19},    // "E3"},
           {51, 18},    // "D#3"},
           {50, 17},    // "D3"},
           {49, 16},    // "C#3"},
           {48, 15},    // "C3"},
           {47, 14},    // "B2"},
           {46, 13},    // "A#2"},
           {45, 12},    // "A2"},
           {44, 11},    // "G#2"},
           {43, 10},    // "G2"},
           {42, 9},     // "F#2"},
           {41, 8},     // "F2"},
           {40, 7},     // "E2"},
           {39, 6},     // "D#2"},
           {38, 5},     // "D2"},
           {37, 4},     // "C#2"},
           {36, 3},     // "C2"},
           {35, 2},     // "B1"},
           {34, 1},     // "A#1"},
           {33, 0}      // "A1"}
        };

        static int i, j, k, l;
        static Int32 tempInt32_1, trackNumBytes, lastCommandIdx, lastChannel;
        static List<byte> tempBytes = new List<byte>();
        static float currentTempo = 500000;
        static float secondsPerTick;
        public static IdolMidi LoadMidiFile(Stream _midiStream)
        {
#if DEBUG
            Debug.WriteLine("******** Load Midi File ********");
#endif

            IdolMidi midiResult = new IdolMidi();

            byte[] buffer = new byte[_midiStream.Length];
            _midiStream.Read(buffer, 0, (int)_midiStream.Length);

            //read type
            if (BitConverter.IsLittleEndian)
            {
                midiResult.Type = swapByteOrder((UInt16)BitConverter.ToInt16(buffer, 8));
            }
            else
            {
                midiResult.Type = BitConverter.ToInt16(buffer, 8);
            }

#if DEBUG
            Debug.WriteLine("midiResult.Type = " + midiResult.Type);
#endif

            //read num of tracks
            if (BitConverter.IsLittleEndian)
            {
                midiResult.NumTracks = swapByteOrder((UInt16)BitConverter.ToInt16(buffer, 10));
            }
            else
            {
                midiResult.NumTracks = BitConverter.ToInt16(buffer, 10);
            }

#if DEBUG
            Debug.WriteLine("midiResult.NumTracks = " + midiResult.NumTracks);
#endif

            //read speed of music
            if (BitConverter.IsLittleEndian)
            {
                midiResult.TicksPerQuarterNote = (float)swapByteOrder((UInt16)BitConverter.ToInt16(buffer, 12));
            }
            else
            {
                midiResult.TicksPerQuarterNote = (float)BitConverter.ToInt16(buffer, 12);
            }

#if DEBUG
            Debug.WriteLine("midiResult.SecondsPerQuarterNote = " + midiResult.TicksPerQuarterNote);
#endif

            //read track data
            i = 14;
            List<IdolMidiEvent> midiTrack;
            for (j = 0; j < midiResult.NumTracks; j++)
            {
#if DEBUG
                Debug.WriteLine("**** j =" + j);
#endif

                midiTrack = new List<IdolMidiEvent>();

                //find track header
                do
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        tempInt32_1 = swapByteOrder(BitConverter.ToInt32(buffer, i));
                    }
                    else
                    {
                        tempInt32_1 = BitConverter.ToInt32(buffer, i);
                    }

                    i++;
                } while (tempInt32_1 != 1297379947);

#if DEBUG
                Debug.WriteLine("midiResult track header = " + tempInt32_1);
#endif

                i += 3;

                //find track num data bytes
                if (BitConverter.IsLittleEndian)
                {
                    trackNumBytes = swapByteOrder(BitConverter.ToInt32(buffer, i)) - 4;
                }
                else
                {
                    trackNumBytes = BitConverter.ToInt32(buffer, i) - 4;
                }

                i += 4;

                //track name
                //if (buffer[i + 1] == 255)
                //{
                //    i += 3;

                //    trackNumBytes -= 4 + buffer[i];
                //    i += buffer[i] + 1;
                //}

#if DEBUG
                Debug.WriteLine("trackNumBytes = " + trackNumBytes);
#endif

                //find track data
                IdolMidiEvent midiEvent;
                for (k = i; k < i + trackNumBytes; k++)
                {
                    midiEvent = new IdolMidiEvent();

                    //timestamp
                    if (buffer[k] < 128)
                    {
                        midiEvent.TimeStamp = buffer[k];
                    }
                    else
                    {
                        tempBytes.Clear();

                        tempBytes.Add(buffer[k]);
                        l = k;
                        do
                        {
                            l++;

                            tempBytes.Add(buffer[l]);
                        } while (l < buffer.Length && buffer[l] >= 128);

                        k = l;

                        //convert to timestamp
                        int m = tempBytes.Count;
                        midiEvent.TimeStamp = 0;
                        for (l = 0; l < m - 1; l++)
                        {
                            midiEvent.TimeStamp += (tempBytes[l] - 128) * (int)Math.Pow(2, 7 * (m - l - 1));
                        }
                        midiEvent.TimeStamp += tempBytes[m - 1];
                    }

                    midiEvent.TimeStamp *= secondsPerTick;

#if DEBUG
                    Debug.WriteLine("midiEvent.TimeStamp = " + midiEvent.TimeStamp);
#endif

                    k += 1;

                    //handle running status
                    if (buffer[k] < 128)
                    {
                        //command idx
                        midiEvent.CommandIdx = lastCommandIdx;

#if DEBUG
                        Debug.WriteLine("running status midiEvent.CommandIdx = " + midiEvent.CommandIdx);
#endif

                        //channel
                        midiEvent.Channel = lastChannel;

#if DEBUG
                        Debug.WriteLine("running status midiEvent.Channel = " + midiEvent.Channel);
#endif

                        if (midiEvent.CommandIdx != 12 &&
                            midiEvent.CommandIdx != 13)
                        {
                            //note idx
                            if (NOTE_IDX.ContainsKey(buffer[k] - 24))
                            {
                                midiEvent.NoteIdx = NOTE_IDX[buffer[k] - 24];
                            }
                            else
                            {
                                midiEvent.NoteIdx = IdolConstants.UNDEFINED_MIDI_NOTE_IDX;
                            }

#if DEBUG
                            Debug.WriteLine("running status midiEvent.NoteIdx = " + midiEvent.NoteIdx);
#endif

                            //note name
                            if (NOTE_NAMES.ContainsKey(buffer[k] - 24))
                            {
                                midiEvent.NoteName = NOTE_NAMES[buffer[k] - 24];
                            }
                            else
                            {
                                midiEvent.NoteName = "";
                            }

#if DEBUG
                            Debug.WriteLine("running status midiEvent.NoteName = " + midiEvent.NoteName);
#endif

                            k += 1;

                            //volume
                            midiEvent.Volume = buffer[k];

#if DEBUG
                            Debug.WriteLine("running status midiEvent.Volume = " + midiEvent.Volume);
#endif

                            //k += 1;
                        }
                        else
                        {
                            //value
                            midiEvent.Volume = buffer[k];

#if DEBUG
                            Debug.WriteLine("running status midiEvent.Value = " + midiEvent.Volume);
#endif

                            //k += 1;
                        }

                        midiTrack.Add(midiEvent);
                    }
                    else
                    {
                        //handle meta data
                        if (buffer[k] == 255)
                        {
                            k += 1;

                            switch (buffer[k])
                            {
                                case 3:
                                    {
                                        k += 1;

                                        k += buffer[k];

                                        break;
                                    }
                                case 81:
                                    {
                                        k += 2;

                                        tempBytes.Clear();

                                        tempBytes.Add(0);
                                        tempBytes.Add(buffer[k]);
                                        tempBytes.Add(buffer[k + 1]);
                                        tempBytes.Add(buffer[k + 2]);

                                        if (BitConverter.IsLittleEndian)
                                        {
                                            currentTempo = swapByteOrder(BitConverter.ToInt32(tempBytes.ToArray(), 0));
                                        }
                                        else
                                        {
                                            currentTempo = BitConverter.ToInt32(tempBytes.ToArray(), 0);
                                        }

                                        secondsPerTick = (currentTempo / midiResult.TicksPerQuarterNote) / MICROSECONDS_TO_SECONDS_FACTOR;

                                        k += 2;

                                        break;
                                    }
                                case 127:
                                    {
                                        k += 1;

                                        k += buffer[k];

                                        break;
                                    }
                            }

                            #region switch (buffer[k])
                            //switch (buffer[k])
                            //{
                            //case 0: //sequence number
                            //    {
                            //        break;
                            //    }
                            //case 1: //text event
                            //    {
                            //        break;
                            //    }
                            //case 2: //copyright notice
                            //    {
                            //        break;
                            //    }
                            //case 3: //track name
                            //    {
                            //        break;
                            //    }
                            //case 4: //instrument name
                            //    {
                            //        break;
                            //    }
                            //case 5: //lyrics
                            //    {
                            //        break;
                            //    }
                            //case 6: //marker
                            //    {
                            //        break;
                            //    }
                            //case 7: //cue point
                            //    {
                            //        break;
                            //    }
                            //case 32: //MIDI Channel Prefix
                            //    {
                            //        break;
                            //    }
                            //case 47:    //End of Track
                            //    {
                            //        break;
                            //    }
                            //case 81:    //set tempo
                            //    {
                            //        k += 2;

                            //        tempBytes.Clear();

                            //        tempBytes.Add(0);
                            //        tempBytes.Add(buffer[k]);
                            //        tempBytes.Add(buffer[k + 1]);
                            //        tempBytes.Add(buffer[k + 2]);

                            //        if (BitConverter.IsLittleEndian)
                            //        {
                            //            currentTempo = swapByteOrder(BitConverter.ToInt32(tempBytes.ToArray(), 0));
                            //        }
                            //        else
                            //        {
                            //            currentTempo = BitConverter.ToInt32(tempBytes.ToArray(), 0);
                            //        }

                            //        secondsPerTick = (currentTempo / midiResult.TicksPerQuarterNote) / MICROSECONDS_TO_SECONDS_FACTOR;

                            //        break;
                            //    }
                            //case 84:    //SMPTE offset
                            //    {
                            //        break;
                            //    }
                            //case 88:    //time signature
                            //    {
                            //        break;
                            //    }
                            //case 89:    //key signature
                            //    {
                            //        break;
                            //    }
                            //case 127:   //sequencer specific
                            //    {
                            //        break;
                            //    }
                            //}
                            #endregion
                        }
                        else
                        {
                            //command idx
                            midiEvent.CommandIdx = buffer[k] / 16;
                            lastCommandIdx = midiEvent.CommandIdx;

#if DEBUG
                            Debug.WriteLine("midiEvent.CommandIdx = " + midiEvent.CommandIdx);
#endif

                            //channel
                            midiEvent.Channel = buffer[k] % 16;
                            lastChannel = midiEvent.Channel;

#if DEBUG
                            Debug.WriteLine("midiEvent.Channel = " + midiEvent.Channel);
#endif

                            k += 1;

                            if (midiEvent.CommandIdx != 12 &&
                                midiEvent.CommandIdx != 13)
                            {
                                //note idx
                                if (NOTE_IDX.ContainsKey(buffer[k] - 24))
                                {
                                    midiEvent.NoteIdx = NOTE_IDX[buffer[k] - 24];
                                }
                                else
                                {
                                    midiEvent.NoteIdx = IdolConstants.UNDEFINED_MIDI_NOTE_IDX;
                                }

#if DEBUG
                                Debug.WriteLine("midiEvent.NoteIdx = " + midiEvent.NoteIdx);
#endif

                                //note name
                                if (NOTE_NAMES.ContainsKey(buffer[k] - 24))
                                {
                                    midiEvent.NoteName = NOTE_NAMES[buffer[k] - 24];
                                }
                                else
                                {
                                    midiEvent.NoteName = "";
                                }

#if DEBUG
                                Debug.WriteLine("midiEvent.NoteName = " + midiEvent.NoteName);
#endif

                                k += 1;

                                //volume
                                midiEvent.Volume = buffer[k];

#if DEBUG
                                Debug.WriteLine("midiEvent.Volume = " + midiEvent.Volume);
#endif

                                //k += 1;
                            }
                            else
                            {
                                //value
                                midiEvent.Volume = buffer[k];

#if DEBUG
                                Debug.WriteLine("midiEvent.Value = " + midiEvent.Volume);
#endif

                                //k += 1;
                            }

                            midiTrack.Add(midiEvent);
                        }
                    }
                }

                midiResult.MidiTracks.Add(midiTrack);

                i += trackNumBytes + 4;
            }

            return midiResult;
        }

        public static IdolMidi LoadMidiFile(string _midiPath)
        {
            Stream midiStream = SGateContentManager.GetStream(_midiPath);

            return LoadMidiFile(midiStream);
        }

        private static UInt16 swapByteOrder(UInt16 value)
        {
            return (UInt16)((0x00FF & (value >> 8))
                | (0xFF00 & (value << 8)));
        }

        private static Int32 swapByteOrder(Int32 value)
        {
            Int32 swap = (Int32)((0x000000FF) & (value >> 24)
                | (0x0000FF00) & (value >> 8)
                | (0x00FF0000) & (value << 8)
                | (0xFF000000) & (value << 24));
            return swap;
        }

        private static Int64 swapByteOrder(Int64 value)
        {
            UInt64 uvalue = (UInt64)value;
            UInt64 swap = ((0x00000000000000FF) & (uvalue >> 56)
            | (0x000000000000FF00) & (uvalue >> 40)
            | (0x0000000000FF0000) & (uvalue >> 24)
            | (0x00000000FF000000) & (uvalue >> 8)
            | (0x000000FF00000000) & (uvalue << 8)
            | (0x0000FF0000000000) & (uvalue << 24)
            | (0x00FF000000000000) & (uvalue << 40)
            | (0xFF00000000000000) & (uvalue << 56));

            return (Int64)swap;
        }
    }
}
