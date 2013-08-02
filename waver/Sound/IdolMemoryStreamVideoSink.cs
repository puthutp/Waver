using System;
using System.IO;

namespace KaraokeIdol
{
    public class IdolMemoryStreamVideoSink : VideoSink
    {
        public VideoFormat CapturedFormat { get; private set; }
        public MemoryStream CapturedVideo { get; private set; }

        protected override void OnCaptureStarted()
        {
            CapturedVideo = new MemoryStream();
        }  
        
        protected override void OnCaptureStopped()
        {
        }
        
        protected override void OnFormatChange(VideoFormat videoFormat)  
        {  
            if (CapturedFormat != null)  
            {  
                throw new InvalidOperationException("Can't cope with change!");  
            }  
            CapturedFormat = videoFormat;   
        }  
        
        protected override void OnSample(long sampleTime, long frameDuration, byte[] sampleData)  
        { 
            //for phase 1, video not recorded yet
            //CapturedVideo.Write(sampleData, 0, sampleData.Length);  
        }  
    }
}
