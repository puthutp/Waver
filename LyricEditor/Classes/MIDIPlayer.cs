using CarlsMIDITools;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Forms = System.Windows.Forms;

namespace LyricEditor
{
    public class MIDIPlayer : IDisposable
    {
        private const int PLAY_NOTE_DURATION = 500;

        private Forms.Timer timerStopper;
        private bool isPlaying;
        private int outputDeviceID = 0;
        private Thread threadWorker;
        private double currentTime;
        private PitchBar nextPitchBarToPlay;
        private int lastNote;

        public byte Volume { get; set; }
        public OutputDevice OutputDevice { get; private set; }

        public MIDIPlayer()
        {
            OutputDevice = new OutputDevice(outputDeviceID);
            timerStopper = new Forms.Timer();
            timerStopper.Interval = PLAY_NOTE_DURATION;
            timerStopper.Tick += new EventHandler(timerStopper_Tick);
        }

        public static bool CheckDeviceAvailability()
        {
            return Instrument.OutDeviceNames().Length > 0;
        }

        public void PlayNoteMomentarily(int keyValueIndex)
        {
            if (isPlaying)
            {
                timerStopper.Stop();
                OutputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, lastNote));
            }
            OutputDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, keyValueIndex, Volume));
            lastNote = keyValueIndex;
            isPlaying = true;
            timerStopper.Start();
        }

        public void PlayNote(int keyValueIndex)
        {
            OutputDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, keyValueIndex, Volume));
        }

        public void StopNote(int keyValueIndex)
        {
            OutputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, keyValueIndex));
        }

        public void Start(double time)
        {
            if (threadWorker == null || !threadWorker.IsAlive)
            {
                currentTime = time;
                SearchNextPitchBarToPlay(time);
                threadWorker = new Thread(new ThreadStart(DelegateThreadWorker));
                threadWorker.Start();
            }
        }

        public void UpdateTime(double time)
        {
            currentTime = time;
        }

        public void Stop()
        {
            if (threadWorker != null && threadWorker.IsAlive)
                threadWorker.Abort();
        }

        void timerStopper_Tick(object sender, EventArgs e)
        {
            if (isPlaying)
                OutputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, lastNote));
            isPlaying = false;
            timerStopper.Stop();
        }

        private void SearchNextPitchBarToPlay(double time)
        {
            int i = 0;
            bool isFound = false;
            while (!isFound && i < Program.PitchBars.Count)
            {
                if (Program.PitchBars[i].StartTime > time)
                    isFound = true;
                else
                    i++;
                if (i < Program.PitchBars.Count)
                    nextPitchBarToPlay = Program.PitchBars[i];
                else
                    nextPitchBarToPlay = null;
            }
        }

        private void DelegateThreadWorker()
        {
            while (nextPitchBarToPlay != null)
            {
                if (nextPitchBarToPlay.StartTime <= currentTime)
                {
                    int index = Program.PitchBars.IndexOf(nextPitchBarToPlay);
                    if (index + 1 < Program.PitchBars.Count)
                        nextPitchBarToPlay = Program.PitchBars[index + 1];
                    else
                        nextPitchBarToPlay = null;
                }
            }
        }
    
        #region IDisposable Members

        public void  Dispose()
        {
            if (OutputDevice != null)
                OutputDevice.Dispose();
            if (threadWorker != null && threadWorker.IsAlive)
                threadWorker.Abort();
        }

        #endregion
    }
}
