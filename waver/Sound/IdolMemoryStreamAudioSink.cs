using System;
using System.IO;

namespace KaraokeIdol
{
    public class IdolMemoryStreamAudioSink : AudioSink
    {
        public MemoryStream Stream;  
        AudioFormat audioFormat; 

        protected override void OnCaptureStarted()  
        {
            Stream = new MemoryStream();
        }
        
        protected override void OnCaptureStopped()  
        {
        }

        public AudioFormat AudioFormat  
        {
            get
            {
                return (audioFormat);
            }
        }
        
        public MemoryStream AudioData
        {
            get
            {
                return (Stream);
            }  
        }  

        protected override void OnFormatChange(AudioFormat audioFormat)
        {  
            if (this.audioFormat == null)  
            {  
                this.audioFormat = audioFormat;  
            }  
            else  
            {  
                throw new InvalidOperationException();  
            }  
        }

        protected override void OnSamples(long sampleTime, long sampleDuration, byte[] sampleData)
        {
            Stream.Write(sampleData, 0, sampleData.Length);
        }  
    }
}
