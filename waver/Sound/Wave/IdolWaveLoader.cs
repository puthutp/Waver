using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
#if DEBUG
using System.Diagnostics;
#endif

namespace waver
{
    public static class IdolWaveLoader
    {
        class Peak
        {
            public int Index { get; set; }
            public double Freq { get; set; }
            public double Amp { get; set; }
        }

        private const int bufferSize = 2048;

        static int i, j;//, k;
        public static IdolWave LoadWaveFile(string _wavePath)
        {
            byte[] waveBytes;
            
            using (Stream waveStream = File.OpenRead(_wavePath))
            {
                waveBytes = new byte[waveStream.Length];
                waveStream.Read(waveBytes, 0, waveBytes.Length);
            }

            return LoadWaveFile(waveBytes);
        }

        public static IdolWave LoadWaveFile(byte[] waveBytes)
        {
            IdolWave result = new IdolWave();

            ParseWave(result, waveBytes);

            return result;
        }

        private static void ParseWave(IdolWave result, byte[] waveBytes)
        {
            i = 0;

            //read ChunkID
            result.ChunkID = BitConverter.ToString(waveBytes, i, 4);

            i += 4;

            //read ChunkSize
            if (BitConverter.IsLittleEndian)
            {
                result.ChunkSize = BitConverter.ToInt32(waveBytes, i);
            }
            else
            {
                result.ChunkSize = swapByteOrder(BitConverter.ToInt32(waveBytes, i));
            }

            i += 4;

            //read Format
            result.Format = BitConverter.ToString(waveBytes, i, 4);

            i += 4;

            //read Subchunk1ID
            result.Subchunk1ID = BitConverter.ToString(waveBytes, i, 4);

            i += 4;

            //read Subchunk1Size
            if (BitConverter.IsLittleEndian)
            {
                result.Subchunk1Size = BitConverter.ToInt32(waveBytes, i);
            }
            else
            {
                result.Subchunk1Size = swapByteOrder(BitConverter.ToInt32(waveBytes, i));
            }

            i += 4;

            //read AudioFormat
            if (BitConverter.IsLittleEndian)
            {
                result.AudioFormat = BitConverter.ToUInt16(waveBytes, i);
            }
            else
            {
                result.AudioFormat = swapByteOrder(BitConverter.ToUInt16(waveBytes, i));
            }

            i += 2;

            //read NumChannels
            if (BitConverter.IsLittleEndian)
            {
                result.NumChannels = BitConverter.ToUInt16(waveBytes, i);
            }
            else
            {
                result.NumChannels = swapByteOrder(BitConverter.ToUInt16(waveBytes, i));
            }

            i += 2;

            //read SampleRate
            if (BitConverter.IsLittleEndian)
            {
                result.SampleRate = BitConverter.ToInt32(waveBytes, i);
            }
            else
            {
                result.SampleRate = swapByteOrder(BitConverter.ToInt32(waveBytes, i));
            }

            i += 4;

            //read ByteRate
            if (BitConverter.IsLittleEndian)
            {
                result.ByteRate = BitConverter.ToInt32(waveBytes, i);
            }
            else
            {
                result.ByteRate = swapByteOrder(BitConverter.ToInt32(waveBytes, i));
            }

            i += 4;

            //read BlockAlign
            if (BitConverter.IsLittleEndian)
            {
                result.BlockAlign = BitConverter.ToUInt16(waveBytes, i);
            }
            else
            {
                result.BlockAlign = swapByteOrder(BitConverter.ToUInt16(waveBytes, i));
            }

            i += 2;

            //read BitsPerSample
            if (BitConverter.IsLittleEndian)
            {
                result.BitsPerSample = BitConverter.ToUInt16(waveBytes, i);
            }
            else
            {
                result.BitsPerSample = swapByteOrder(BitConverter.ToUInt16(waveBytes, i));
            }

            i += 2;

            //read Subchunk2ID
            result.Subchunk2ID = BitConverter.ToString(waveBytes, i, 4);

            i += 4;

            //read Subchunk2Size
            if (BitConverter.IsLittleEndian)
            {
                result.Subchunk2Size = BitConverter.ToInt32(waveBytes, i);
            }
            else
            {
                result.Subchunk2Size = swapByteOrder(BitConverter.ToInt32(waveBytes, i));
            }

            i += 4;

            //read data
            //result.Data = new byte[result.Subchunk2Size];
            result.Data = new byte[waveBytes.Length - i];
            j = 0;

            while (i < waveBytes.Length - 1)
            {
                result.Data[j] = waveBytes[i];

                i++;
                j++;
            }
        }

        static int closestFreq;
        public static List<IdolWaveNote> GetWaveNotes(IdolWave _wave)
        {
            List<IdolWaveNote> result = new List<IdolWaveNote>();

            /******/
            //GetNotes(0, _wave.Data.Length - 1, _wave, result);

            #region parallel

            int chunk = _wave.Data.Length / bufferSize;

            List<IdolWaveNote> result1 = new List<IdolWaveNote>();
            List<IdolWaveNote> result2 = new List<IdolWaveNote>();
            List<IdolWaveNote> result3 = new List<IdolWaveNote>();
            List<IdolWaveNote> result4 = new List<IdolWaveNote>();

            Parallel.Do(delegate()
            {
                GetNotes(0, chunk / 2 * bufferSize - 1, _wave, result1);
            }
            ,
            delegate()
            {
                GetNotes(chunk / 2 * bufferSize, _wave.Data.Length - 1, _wave, result2);
            }
            );

            //Parallel.Do(delegate()
            //{
            //    GetNotes(0, chunk / 4 * bufferSize - 1, _wave, result1);
            //},
            //delegate()
            //{
            //    GetNotes(chunk / 4 * bufferSize, 2 * chunk / 4 * bufferSize - 1, _wave, result2);
            //}
            //,
            //delegate()
            //{
            //    GetNotes(2 * chunk / 4 * bufferSize, 3 * chunk / 4 * bufferSize - 1, _wave, result3);
            //},
            //delegate()
            //{
            //    GetNotes(3 * chunk / 4 * bufferSize, _wave.Data.Length - 1, _wave, result4);
            //}
            //);

            result.AddRange(result1);
            result.AddRange(result2);
            //result.AddRange(result3);
            //result.AddRange(result4);
            #endregion

            /******/

            return result;
        }

        private static void GetNotes(int startIdx, int endIdx, IdolWave _wave, List<IdolWaveNote> result)
        {
            IdolWaveNote waveNote;
            //short[] waveShortBuffer = new short[bufferSize];
            //float timeLength = 0.4f;
            //short[] waveShortBuffer = new short[(int)(_wave.ByteRate / 2 * timeLength)];
            //byte[] waveByteBuffer = new byte[(int)(_wave.ByteRate / 2 * timeLength)];
            byte[] waveByteBuffer = new byte[bufferSize];

            int waveIdx = startIdx;
            int bufferIdx = 0;

            AudioFrame audioFrame = new AudioFrame(false);

            double MinFreq = 80;
            double MaxFreq = 800;

            int defaultIdx = -99;

            int batchNo = 0;
            while (waveIdx < endIdx)
            {
                batchNo++;
                bufferIdx = 0;

                /************************************************/
                
                //while (waveIdx < endIdx &&
                //        bufferIdx < waveShortBuffer.Length)
                //{
                //    waveShortBuffer[bufferIdx] = (Int16)swapByteOrder(BitConverter.ToUInt16(_wave.Data, waveIdx));

                //    waveIdx += _wave.NumChannels;
                //    bufferIdx++;
                //}

                //waveNote = IdolSoundAnalyzer.ProcessData(waveShortBuffer, _wave.SampleRate);

                //IdolSoundAnalyzer.FindClosestNote((int)Math.Round(waveNote.freq), out closestFreq, out waveNote.NoteIdx, out waveNote.NoteName);
                
                /************************************************/

                while (waveIdx < endIdx &&
                        bufferIdx < waveByteBuffer.Length)
                {
                    waveByteBuffer[bufferIdx] = _wave.Data[waveIdx];

                    waveIdx ++;
                    bufferIdx++;
                }

                audioFrame.ProcessMono(ref waveByteBuffer);

                int fftBufferLength = audioFrame._fftLeft.Length;

                int minSpectr = (int)(MinFreq * (fftBufferLength * 2) / _wave.SampleRate) + 1;
                int index = minSpectr;
                double max = audioFrame._fftLeft[index];
                int usefullMaxSpectr = Math.Min((fftBufferLength * 2),
                    (int)(MaxFreq * (fftBufferLength * 2) / _wave.SampleRate) + 1);

                List<Peak> peakList = new List<Peak>();
                double current = max;
                double last = max;
                double last2 = max;

                for (int i = index; i < usefullMaxSpectr; i++)
                {
                    last2 = last;
                    last = current;
                    current = audioFrame._fftLeft[i];

                    if (last > 51)
                    {
                        if (last > last2 && last > current)
                        {
                            Peak peak = new Peak();
                            peak.Index = i - 1;
                            peak.Freq = _wave.SampleRate * (i - 1) / (audioFrame._fftLeft.Length * 2);
                            peak.Amp = audioFrame._fftLeft[i - 1];
                            peakList.Add(peak);
                        }
                    }

                    if (max < audioFrame._fftLeft[i])
                    {
                        max = audioFrame._fftLeft[i]; index = i;
                    }
                }

                double freq = 0;
                if (peakList.Count > 0)
                {
                    int i = 0;
                    bool found = false;
                    while (i < peakList.Count && !found)
                    {
                        if (peakList[i].Amp >= 58)
                        {
                            index = peakList[i].Index;
                            found = true;
                        }
                        else i++;
                    }
                    if (!found)
                    {
                        index = peakList[0].Index;
                    }
                    else
                    {
                            for (int j = 0; j < i; j++)
                            {
                                double ratio = peakList[i].Freq / peakList[j].Freq;
                                if (ratio >= 1 && (ratio % 1.0f < 0.1f || ratio % 1.0f > 0.9f))
                                {
                                    index = peakList[j].Index;
                                    if (peakList[j].Freq == 93)
                                    {
                                        peakList[j].Freq *= 1;
                                    }
                                    break;
                                }
                            }
                    }
                }
                freq = _wave.SampleRate * index / (audioFrame._fftLeft.Length * 2);
                if (freq < MinFreq) freq = 0;

                waveNote = new IdolWaveNote();
                waveNote.freq = freq;
                waveNote.max = audioFrame._fftLeft[index];

                IdolSoundAnalyzer.FindClosestNote((int)Math.Round(waveNote.freq), out closestFreq, out waveNote.NoteIdx, out waveNote.NoteName);

                /************************************************/

                result.Add(waveNote);
            }
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
