using System;

namespace waver
{
    public class IdolWave
    {
        //"WAVE" format starts with the RIFF header:
        public string ChunkID;          //"RIFF"
        public Int32 ChunkSize;         //36 + Subchunk2Size
        public string Format;           //"WAVE"

        //"WAVE" format consists of 2 subchunks: "fmt " and "data":

        //"fmt" describes sound data's format:
        public string Subchunk1ID;      //"fmt "
        public Int32 Subchunk1Size;     //16 for PCM
        public UInt16 AudioFormat;      //PCM = 1
        public UInt16 NumChannels;      //Mono = 1, Stereo = 2
        public Int32 SampleRate;        //8000, 44100, etc.
        public Int32 ByteRate;          //SampleRate * NumChannels * BitsPerSample/8
        public UInt16 BlockAlign;       //NumChannels * BitsPerSample/8
        public UInt16 BitsPerSample;    //8 bits = 8, 16 bits = 16, etc.

        //"data" describes contains size of data & actual sound:
        public string Subchunk2ID;      //"data"
        public Int32 Subchunk2Size;     //NumSamples * NumChannels * BitsPerSample/8
        public byte[] Data;            //the actual sound data

        public IdolWave()
        {
        }
    }
}
