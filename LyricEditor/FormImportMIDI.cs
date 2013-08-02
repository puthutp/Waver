using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LyricEditor
{
    public partial class FormImportMIDI : Form
    {
        private double tempo;
        private bool isClosing;
        private bool isDoneParse;
        private bool startFromZero;
        private bool isPlaying;        
        private string fileLocation;
        private Sequencer sequencer;
        private Sequence sequence;
        private OutputDevice outputDevice;
        private List<string> lyrics;
        private List<List<MidiEvent>> textMidiEvents;
        private List<List<string>> textMidiData;

        public List<PitchBar> Pitchbars { get; private set; }

        public FormImportMIDI()
        {
            InitializeComponent();
            tempo = 500000;
            isDoneParse = false;
            isClosing = false;
            isPlaying = false;
            startFromZero = true;
            outputDevice = Program.MIDIPlayer.OutputDevice;
            sequence = new Sequence();
            sequence.Format = 1;
            sequence.LoadCompleted += new EventHandler<AsyncCompletedEventArgs>(sequence_LoadCompleted);
            sequencer = new Sequencer();
            sequencer.Position = 0;
            sequencer.Sequence = this.sequence;
            sequencer.PlayingCompleted += new EventHandler(sequencer_PlayingCompleted);
            sequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(sequencer_ChannelMessagePlayed);
            sequencer.Stopped += new EventHandler<StoppedEventArgs>(sequencer_Stopped);
            sequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(sequencer_SysExMessagePlayed);
            sequencer.Chased += new EventHandler<ChasedEventArgs>(sequencer_Chased);
            timerLocation.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            isClosing = true;
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            timerLocation.Stop();
            sequence.Dispose();
            sequencer.Dispose();
            base.OnClosed(e);
        }

        private void sequence_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // search for lyric
            lyrics = new List<string>();
            textMidiEvents = new List<List<MidiEvent>>();
            textMidiData = new List<List<string>>();
            List<string> trackNames = new List<string>();
            for (int i = 0; i < sequence.Count; i++)
            {                
                trackNames.Add("Track " + (i + 1));
                textMidiData.Add(new List<string>());
                textMidiEvents.Add(new List<MidiEvent>());
                string text = "";
                List<MidiEvent> midiEvents = sequence[i].Iterator().ToList();
                for (int j = 0; j < midiEvents.Count;j++ )
                {
                    MidiEvent midiEvent = midiEvents[j];
                    textMidiData[i].Add("");
                    if (midiEvent.MidiMessage.MessageType == MessageType.Meta)
                    {
                        textMidiEvents[i].Add(midiEvent);
                        MetaMessage metaMessage = (midiEvent.MidiMessage as MetaMessage);
                        if (metaMessage.MetaType == MetaType.Text)
                        {
                            string dataString = "";
                            byte[] bytes = metaMessage.GetBytes();
                            for (int k = 0; k < bytes.Length; k++)
                                dataString += char.ConvertFromUtf32(bytes[k]);
                            textMidiData[i][j] = dataString;
                            text += dataString;
                        }
                        else if(metaMessage.MetaType == MetaType.TrackName)
                        {
                            string dataString = "";
                            byte[] bytes = metaMessage.GetBytes();
                            for (int k = 0; k < bytes.Length; k++)
                                dataString += char.ConvertFromUtf32(bytes[k]);
                            trackNames[i] += ": " + dataString;
                        }
                        else if (metaMessage.MetaType == MetaType.Tempo)
                        {
                            byte[] tempoData = metaMessage.GetBytes();
                            tempoData = new byte[] { (byte)0, tempoData[0], tempoData[1], tempoData[2] };
                            tempo = swapByteOrder(BitConverter.ToInt32(tempoData, 0));
                        }
                    }
                }
                lyrics.Add(text);
            }

            // insert to listbox
            listBoxTrackNumbers.Items.Clear();
            for (int i = 0; i < sequence.Count; i++)
                listBoxTrackNumbers.Items.Add(trackNames[i]);
            if (sequence.Count > 0)
                listBoxTrackNumbers.SelectedIndex = 0;

            Cursor = Cursors.Arrow;
            buttonBrowse.Enabled = false;
            buttonPlay.Enabled = true;
            buttonStop.Enabled = true;
            trackBar1.Maximum = sequence.GetLength();
            numericUpDownLyric.Minimum = 1;
            numericUpDownVocal.Minimum = 1;
            numericUpDownLyric.Maximum = sequence.Count;
            numericUpDownVocal.Maximum = sequence.Count;
            isDoneParse = true;
            buttonOK.Enabled = true;
            toolStripStatusLabel.Text = "Done Loading";
        }

        private void sequencer_Chased(object sender, ChasedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
                outputDevice.Send(message);
        }

        private void sequencer_SysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
            //outDevice.Send(e.Message); Sometimes causes an exception to be thrown because the output device is overloaded       
        }

        private void sequencer_Stopped(object sender, StoppedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
                outputDevice.Send(message);
        }

        private void sequencer_ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            outputDevice.Send(e.Message);
        }

        private void sequencer_PlayingCompleted(object sender, EventArgs e)
        {
            isPlaying = false;
            startFromZero = true;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported Formats|*.mid;*.kar|MIDI File(*.mid)|*.mid|KAR File(*.kar)|*.kar";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    toolStripStatusLabel.Text = "Loading...";
                    fileLocation = openFileDialog.FileName;
                    textBox1.Text = fileLocation;
                    sequencer.Stop();
                    isPlaying = false;
                    sequence.LoadAsync(fileLocation);                    
                    Cursor = Cursors.WaitCursor;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }                
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (isPlaying) // then pause
                {
                    toolStripStatusLabel.Text = "Paused";
                    startFromZero = false;
                    sequencer.Stop();
                }
                else // then play
                {
                    toolStripStatusLabel.Text = "Play";
                    if (startFromZero)
                    {
                        if (listBoxTrackNumbers.SelectedIndex >= 0 & listBoxTrackNumbers.SelectedIndex < sequence.Count)
                            sequencer.Start(listBoxTrackNumbers.SelectedIndex);
                        else
                            sequencer.Start();
                    }
                    else
                    {
                        sequencer.Position = trackBar1.Value;
                        if (listBoxTrackNumbers.SelectedIndex >= 0 & listBoxTrackNumbers.SelectedIndex < sequence.Count)                        
                            sequencer.Continue(listBoxTrackNumbers.SelectedIndex);
                        else
                            sequencer.Continue();
                    }
                }
                isPlaying = !isPlaying;
                trackBar1.Enabled = !isPlaying;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {          
            try
            {
                toolStripStatusLabel.Text = "Stopped";
                trackBar1.Enabled = true;
                isPlaying = false;
                startFromZero = true;
                sequencer.Stop();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void timerLocation_Tick(object sender, EventArgs e)
        {
            if (isPlaying && sequencer.Position < trackBar1.Maximum)
                trackBar1.Value = sequencer.Position;
        }

        private void listBoxTrackNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTrackNumbers.SelectedIndex >= 0 & listBoxTrackNumbers.SelectedIndex < sequence.Count)
            {
                if (isDoneParse)
                    textBoxLyric.Text = lyrics[listBoxTrackNumbers.SelectedIndex];
                if (isPlaying)
                {
                    toolStripStatusLabel.Text = "Changed to Track " + (listBoxTrackNumbers.SelectedIndex + 1);
                    sequencer.Continue(listBoxTrackNumbers.SelectedIndex);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (isDoneParse)
            {
                sequencer.Stop();
                int vocalTrackNumber = (int)numericUpDownVocal.Value - 1;
                int lyricTrackNumber = (int)numericUpDownLyric.Value - 1;
                if (vocalTrackNumber >= 0 && vocalTrackNumber < sequence.Count && lyricTrackNumber >= 0 && lyricTrackNumber < sequence.Count)
                {
                    List<MidiEvent> vocalMidiEvents = sequence[vocalTrackNumber].Iterator().ToList();
                    List<MidiEvent> lyricMidiEvents = textMidiEvents[lyricTrackNumber];
                    Pitchbars = new List<PitchBar>();
                    PitchBar lastPitchBar = null;
                    double currentTime = 0;
                    double currentLyricTime = 0;
                    bool foundLyric = false;
                    MidiEvent lyricMidiEvent = null;
                    int midiEventLyricIndex = -1;
                    double tickMultiplier = tempo / 1000000 / sequence.Division;

                    for (int i = 0; i < vocalMidiEvents.Count; i++)
                    {
                        MidiEvent midiEvent = vocalMidiEvents[i];
                        currentTime = midiEvent.AbsoluteTicks * tickMultiplier;
                        bool isLyricFound = currentTime <= currentLyricTime;
                        foundLyric = (currentTime == currentLyricTime);

                        while (!isLyricFound)
                        {
                            midiEventLyricIndex += 1;
                            if (midiEventLyricIndex < lyricMidiEvents.Count)
                            {
                                lyricMidiEvent = lyricMidiEvents[midiEventLyricIndex];
                                currentLyricTime = lyricMidiEvent.AbsoluteTicks * tickMultiplier;
                                isLyricFound = currentTime <= currentLyricTime;
                                foundLyric = (currentTime == currentLyricTime);
                            }
                            else
                            {
                                foundLyric = false;
                                lyricMidiEvent = null;
                                isLyricFound = true;
                            }
                        }

                        if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                        {
                            ChannelMessage channelMessage = midiEvent.MidiMessage as ChannelMessage;
                            int noteIndex = channelMessage.Data1 + Program.MIDI_OFFSET;
                            int volume = channelMessage.Data2;
                            if ((channelMessage.Command == ChannelCommand.NoteOn && volume == 0) || channelMessage.Command == ChannelCommand.NoteOff) // note off
                            {
                                if (lastPitchBar != null && lastPitchBar.NoteIndex == noteIndex)
                                    lastPitchBar.EndTime = currentTime;
                            }
                            else if (channelMessage.Command == ChannelCommand.NoteOn && volume > 0 && noteIndex >= Program.PANEL_PIANO_START_NOTE_INDEX + Program.MIDI_OFFSET && noteIndex < Program.PANEL_PIANO_START_NOTE_INDEX + Program.PANEL_PIANO_NOTE_COUNT) // note on
                            {
                                PitchBar newPitchBar = new PitchBar();
                                newPitchBar.StartTime = currentTime;
                                newPitchBar.NoteIndex = noteIndex;
                                if (foundLyric && lyricMidiEvent != null && midiEventLyricIndex < textMidiData[lyricTrackNumber].Count)
                                {
                                    newPitchBar.Lyric = textMidiData[lyricTrackNumber][midiEventLyricIndex];
                                    if (lastPitchBar != null)
                                        lastPitchBar.IsLastWord = newPitchBar.Lyric.StartsWith(char.ConvertFromUtf32(47));
                                }
                                else
                                    newPitchBar.Lyric = "";
                                PitchBar.InsertSorted(Pitchbars, newPitchBar);
                                if (lastPitchBar != null)
                                    lastPitchBar.EndTime = currentTime;
                                lastPitchBar = newPitchBar;
                            }
                        }
                    //    if ((idolMidiEvent.CommandIdx == 9 && idolMidiEvent.Volume == 0) || idolMidiEvent.CommandIdx == 8) // note off
                    //    {
                    //        if (lastPitchBar != null && lastPitchBar.NoteIndex == idolMidiEvent.NoteIdx)
                    //            lastPitchBar.EndTime = currentTime;
                    //    }
                    //    else if (idolMidiEvent.CommandIdx == 9 && idolMidiEvent.Volume > 0 && idolMidiEvent.NoteIdx >= Program.PANEL_PIANO_START_NOTE_INDEX && idolMidiEvent.NoteIdx < Program.PANEL_PIANO_START_NOTE_INDEX + Program.PANEL_PIANO_NOTE_COUNT) // note on
                    //    {
                    //        PitchBar newPitchBar = new PitchBar();
                    //        newPitchBar.StartTime = currentTime;
                    //        newPitchBar.NoteIndex = idolMidiEvent.NoteIdx;
                    //        if (idolMIDIEventLyric != null && idolMIDIEventLyric.Metadata != null)
                    //        {
                    //            newPitchBar.Lyric = idolMIDIEventLyric.Metadata;
                    //            if (lastPitchBar != null)
                    //                lastPitchBar.IsLastWord = newPitchBar.Lyric.StartsWith(char.ConvertFromUtf32(47));
                    //        }
                    //        else
                    //            newPitchBar.Lyric = "";
                    //        PitchBar.InsertSorted(Pitchbars, newPitchBar);
                    //        if (lastPitchBar != null)
                    //            lastPitchBar.EndTime = currentTime;
                    //        lastPitchBar = newPitchBar;
                    //    }
                    }
                    DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show("Number of selected tracks is / are invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private static Int32 swapByteOrder(Int32 value)
        {
            Int32 swap = (Int32)((0x000000FF) & (value >> 24)
                | (0x0000FF00) & (value >> 8)
                | (0x00FF0000) & (value << 8)
                | (0xFF000000) & (value << 24));
            return swap;
        }
    }
}
