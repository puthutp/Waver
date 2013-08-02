using GameLibrary.Input.Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LyricEditor
{
    public partial class FormListener : Form
    {
        public delegate void NewNoteCapturedHandler(string toWrite, int noteIndex);
        public delegate void NoNoteCapturedHandler(string toWrite);

        private const int noteOffset = 0;
        private const int smaWindowWidth = 5;
        private const int minimumLength = 5;
        private const int unregisteredNote = int.MinValue;

        public List<Note> CapturedNotes { get; private set; }

        private int notifyPerSecond;
        private SoundListener soundListener;
        private bool firstTimeListen;
        private int lastSangNote;
        private int lastCapturedNote;
        private Note lastNote;
        private List<int> bufferNotes;
        private double startTime;
        private double endTime;
        private bool isUserClosing;
        private double currentTime;
        private double CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;                
                labelTimeline.Text = Program.DoubleToTimespanString(currentTime);
            }
        }        

        public FormListener()
        {
            InitializeComponent();
            isUserClosing = false;
            notifyPerSecond = (int)(1 / (6000 / Program.BeatPerMinute / 4 / (double)numericUpDownSpeed.Value));
            bufferNotes = new List<int>();
            windowsMediaPlayer.settings.autoStart = false;
            windowsMediaPlayer.settings.volume = Program.Volume;
            windowsMediaPlayer.URL = Program.MediaLocation;
            trackBarTimeline.Maximum = trackBarTimeline.Width;
            windowsMediaPlayer.settings.rate = (double)numericUpDownSpeed.Value / 100;
            CurrentTime = 0;
            ucNoteGraph.PitchStart = 3; // C2
            ucNoteGraph.PitchRange = 48; // up to C6
            firstTimeListen = true;
        }

        //private void TranslateBuffer()
        //{
        //    if (bufferNotes.Count > 0)
        //    {
        //        double speed = (double)numericUpDownSpeed.Value / 100;
        //        for (int i = 0; i < bufferNotes.Count; i++)
        //        {
        //            if (bufferNotes[i] != unregisteredNote)
        //            {
        //                Note newNote = new Note();
        //                newNote.NoteIndex = bufferNotes[i];
        //                newNote.StartPoint = ((double)i / notifyPerSecond * speed) + startTime;
        //                newNote.EndPoint = ((double)(i + 1) / notifyPerSecond * speed) + startTime;
        //                CapturedNotes.Add(newNote);
        //            }
        //        }
        //    }
        //}

        private void TranslateBuffer()
        {
            if (bufferNotes.Count > 0)
            {
                double speed = (double)numericUpDownSpeed.Value / 100;
                for (int i = 0; i < bufferNotes.Count; i++)
                {
                    if ((firstTimeListen || bufferNotes[i] != lastSangNote) && bufferNotes[i] >= Program.PANEL_PIANO_START_NOTE_INDEX && bufferNotes[i] < Program.RegisteredNotes.Count)
                    {
                        if (lastNote != null)
                            lastNote.EndPoint = ((double)(i + 1) / notifyPerSecond * speed) + startTime;
                        Note newNote = new Note();
                        newNote.NoteIndex = bufferNotes[i];
                        newNote.StartPoint = ((double)i / notifyPerSecond * speed) + startTime;
                        lastNote = newNote;
                        CapturedNotes.Add(newNote);
                        lastSangNote = newNote.NoteIndex;
                        firstTimeListen = false;
                    }
                }
            }
        }

        private void NewNoteCaptured(string toWrite, int noteIndex)
        {
            textBox.AppendText(toWrite + "\n");
            ucNoteGraph.AddNote(noteIndex - ucNoteGraph.PitchStart);
        }

        private void NoNoteCaptured(string toWrite)
        {
            textBox.AppendText(toWrite + "\n");
            ucNoteGraph.AddNote();
        }      

        private void SoundListener_Listen(object sender, double frequency, int volume)
        {
            int newNote;
            if (volume >= Program.MinimumVolume) // volume above minimal
            {
                if (frequency != SoundListener.INFRASONIC_FREQ) // new note
                {
                    newNote = SoundListener.FindClosestNote(frequency) + noteOffset;
                    lastCapturedNote = newNote;
                    try
                    {
                        string writeToTextBox = Program.DoubleToTimespanString(CurrentTime) + ": " + Program.RegisteredNotes[newNote];
                        Invoke(new NewNoteCapturedHandler(NewNoteCaptured), new object[] { writeToTextBox, newNote });
                    }
                    catch (Exception) { }
                }
                else // unregistered note 
                {
                    newNote = lastCapturedNote;
                    try
                    {
                        string writeToTextBox = Program.DoubleToTimespanString(CurrentTime) + ": (unregistered)";
                        Invoke(new NewNoteCapturedHandler(NewNoteCaptured), new object[] { writeToTextBox, newNote });
                    }
                    catch (Exception) { }
                }
            }
            else // volume drop below minimal
            {
                newNote = lastCapturedNote;
                try
                {
                    string writeToTextBox = Program.DoubleToTimespanString(CurrentTime) + ": (volume drop)";
                    Invoke(new NewNoteCapturedHandler(NewNoteCaptured), new object[] { writeToTextBox, newNote });
                }
                catch (Exception) { }
            }            
            bufferNotes.Add(newNote);
        }

        private void FormListener_Load(object sender, EventArgs e)
        {
            if (Program.MediaLocation == "")
            {
                MessageBox.Show("Audio File Location have not yet been determined!", "No Audio File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }

        private void FormListener_SizeChanged(object sender, EventArgs e)
        {
            trackBarTimeline.Maximum = trackBarTimeline.Width;
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            CurrentTime = windowsMediaPlayer.Ctlcontrols.currentPosition;
            if (windowsMediaPlayer.Ctlcontrols.currentItem.duration > 0)
                trackBarTimeline.Value = (int)(CurrentTime / windowsMediaPlayer.Ctlcontrols.currentItem.duration * trackBarTimeline.Maximum);
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (soundListener == null)
            {
                soundListener = new SoundListener(Program.SoundCaptureDevice);
                soundListener.NotifyPointsInSecond = notifyPerSecond;
                soundListener.Listen += new SoundListener.ListenHandler(SoundListener_Listen);
            }
            ucNoteGraph.Clear();
            windowsMediaPlayer.Ctlcontrols.currentPosition = (double)trackBarTimeline.Value / trackBarTimeline.Maximum * windowsMediaPlayer.Ctlcontrols.currentItem.duration;
            timerRefresh.Start();
            windowsMediaPlayer.Ctlcontrols.play();
            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;
            buttonPlay.Enabled = false;
            buttonListen.Enabled = true;
            buttonStop.Enabled = true;
            numericUpDownSpeed.Enabled = false;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (soundListener != null)
            {
                soundListener.StopListen();
                soundListener.Listen -= new SoundListener.ListenHandler(SoundListener_Listen);
                soundListener = null;
            }
            endTime = CurrentTime;
            firstTimeListen = true;
            windowsMediaPlayer.Ctlcontrols.stop();
            timerRefresh.Stop();
            TranslateBuffer();
            buttonOK.Enabled = true;
            buttonCancel.Enabled = true;
            buttonPlay.Enabled = true;
            buttonListen.Enabled = false;
            buttonStop.Enabled = false;
            numericUpDownSpeed.Enabled = true;
        }

        private void buttonListen_Click(object sender, EventArgs e)
        {
            CapturedNotes = new List<Note>();
            textBox.Clear();
            CapturedNotes = new List<Note>();
            lastNote = null;
            lastCapturedNote = unregisteredNote;
            startTime = CurrentTime;
            soundListener.StartListen();
            buttonListen.Enabled = false;
        }

        private void trackBarTimeline_ValueChanged(object sender, EventArgs e)
        {
            CurrentTime = (double)trackBarTimeline.Value / trackBarTimeline.Maximum * windowsMediaPlayer.Ctlcontrols.currentItem.duration;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            isUserClosing = true;
            buttonStop_Click(buttonStop, EventArgs.Empty);
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            isUserClosing = true;
            buttonStop_Click(buttonStop, EventArgs.Empty);
            DialogResult = DialogResult.Cancel;
        }

        private void numericUpDownSpeed_ValueChanged(object sender, EventArgs e)
        {
            windowsMediaPlayer.settings.rate = (double)numericUpDownSpeed.Value / 100;
            notifyPerSecond = (int)(1 / (6000 / Program.BeatPerMinute / 4 / (double)numericUpDownSpeed.Value));
        }

        protected override void  OnClosing(CancelEventArgs e)
        {
            e.Cancel = !isUserClosing;
        }

        private void windowsMediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
                buttonStop_Click(buttonStop, EventArgs.Empty);
        }

        //private static bool IsDifferentNote(double freq1, double freq2)
        //{
        //    int freq1NoteIndex = SoundListener.FindClosestNote(freq1) + noteOffset;
        //    if (freq1NoteIndex >= 0 && freq1NoteIndex < SoundListener.Frequencies.Length)
        //    {
        //        int freqAbove = 0;
        //        int freqBelow = 0;
        //        GetClosestFreq(freq1, out freqBelow, out freqAbove);
        //        Console.WriteLine(freq1 + ": (" + freqBelow + "-" + freqAbove + ")--> " + freq2);
        //        return (freq2 < freqBelow || freq2 > freqAbove);
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //private static void GetClosestFreq(double freq, out int below, out int above)
        //{
        //    for (int i = 0; i < SoundListener.Frequencies.Length; i++)
        //    {
        //        if (SoundListener.Frequencies[i] < freq)
        //        {
        //            below = SoundListener.Frequencies[i + 1];
        //            above = SoundListener.Frequencies[i - 1];
        //            return;
        //        }
        //    }
        //    below = 0;
        //    above = int.MaxValue;
        //}
    }
}
