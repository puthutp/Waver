using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace LyricEditor
{
    public partial class FormMain : Form
    {
        //
        // private data members
        //
        private int newFlowLayoutPanelPianoLocation;
        private CurrencyManager currencyManager;
        private double startTimeProperty;
        private double endTimeProperty;

        FormSong formSong = new FormSong();

        //
        // private flags
        //
        private bool isMediaPlaying;
        private bool isHScrollMoved;
        private bool isVScrollMoved;
        
        private List<Label> labelTimelines;
        private List<Label> labelBeatIndices;

        // constructors
        public FormMain()
        {
            InitializeComponent();
            KeyPreview = true;
            if (!MIDIPlayer.CheckDeviceAvailability())
                MessageBox.Show("No MIDI Device Detected");
            Program.MIDIPlayer = new MIDIPlayer();
            Program.MIDIPlayer.Volume = Program.MIDIVolume;
            ucEditor.MIDIOutPutDevice = Program.MIDIPlayer.OutputDevice;
            InitializeObjects();
            FillPanelPiano();
            CreatePanelTimeline();
            UpdateScrollbars();
            timerRefresh.Start();
        }

        // methods
        private void InitializeObjects()
        {
            SaveManager.isDirty = false;
            SaveManager.hasSavedToRecent = false;
            isMediaPlaying = false;
            isHScrollMoved = false;
            isVScrollMoved = false;
            FillComboBoxNoteIndex();
            windowsMediaPlayer.settings.autoStart = false;
            windowsMediaPlayer.settings.volume = Program.Volume;
            SetMediaPlayerUrl();
            ucEditor.OpenNew();
            Program.PitchBars = ucEditor.PitchBars;
            UpdateStatus("Ready");
        }

        private void UpdateStatus(string input)
        {
            toolStripLabelStatus.Text = input;
        }

        private void ShowWelcomeScreen()
        {
            FormWelcome formWelcome = new FormWelcome();
            DialogResult dialogResult = formWelcome.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    ucEditor.OpenNew();
                    Program.FileLocation = formWelcome.FileName;
                    FileManager.LoadFromFile(formWelcome.FileName);
                    ucEditor.OpenNew();
                    ucEditor.LoadPitchBars(Program.PitchBars);
                    Program.ProcessSentenceDuration(ucEditor.PitchBars);
                    ucEditor.RefreshScreen();
                    NextFocusedPitchbar();
                    SetMediaPlayerUrl();
                    windowsMediaPlayer.settings.volume = Program.Volume;
                    RefreshPanelTimeline();
                    UpdateScrollbars();
                    UpdateStatus("Opened Succesfully");
                    SaveManager.isDirty = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error load");
                }
            }
            else if (dialogResult == DialogResult.Abort)            
                propertiesToolStripMenuItem_Click(propertiesToolStripMenuItem, EventArgs.Empty);
        }

        private void FillComboBoxNoteIndex()
        {
            for (int i = Program.RegisteredNotes.Count - 1; i >= Program.PANEL_PIANO_START_NOTE_INDEX; i--)
                comboBoxNoteIndex.Items.Add(Program.RegisteredNotes[i]);
        }

        private void FillPanelPiano()
        {
            flowLayoutPanelPiano.Size = new Size(panelPianoOuter.Width, Program.PANEL_PIANO_NOTE_COUNT * Program.GRID_VERTICAL);
            for (int i = 0; i < Program.PANEL_PIANO_NOTE_COUNT; i++)
            {
                Label newLabel = new Label();
                newLabel.Margin = new Padding(0);
                newLabel.Size = new Size(flowLayoutPanelPiano.Width, Program.GRID_VERTICAL);
                newLabel.Text = Program.RegisteredNotes[Program.RegisteredNotes.Count - i - 1];
                newLabel.TextAlign = ContentAlignment.MiddleCenter;
                flowLayoutPanelPiano.Controls.Add(newLabel);
            }
        }

        private void UpdateScrollbars()
        {
            hScrollBar.LargeChange = ucEditor.Width;
            vScrollBar.LargeChange = ucEditor.Height;
            hScrollBar.SmallChange = ucEditor.Width / 4;
            vScrollBar.SmallChange = ucEditor.Height / 4;
            hScrollBar.Maximum = ucEditor.realWidth - panelTimelineOuter.Width + vScrollBar.Width + ucEditor.Width - 1;
            vScrollBar.Maximum = ucEditor.realHeight - panelPianoOuter.Height + hScrollBar.Height + ucEditor.Height - 1;
        }

        private void CreatePanelTimeline()
        {
            labelTimelines = new List<Label>();
            labelBeatIndices = new List<Label>();
            flowLayoutPanelTimeline.Controls.Clear();
            flowLayoutPanelTimeline.Location = new Point(-Program.GRID_HORIZONTAL / 2, 0);
            int timelineCount = panelTimelineOuter.Width / Program.GRID_HORIZONTAL + 3;
            int panelWidth = (timelineCount) * Program.GRID_HORIZONTAL;
            flowLayoutPanelTimeline.Size = new Size(panelWidth, panelTimelineOuter.Height);
            for (int i = 0; i < timelineCount; i++)
            {
                Panel panelContainer = new Panel();
                panelContainer.Size = new Size(Program.GRID_HORIZONTAL, flowLayoutPanelTimeline.Height);
                panelContainer.Margin = new Padding(0);

                Label newLabelBeatIndex = new Label();
                newLabelBeatIndex.Margin = new Padding(0);
                newLabelBeatIndex.Size = new Size(Program.GRID_HORIZONTAL, flowLayoutPanelTimeline.Height / 2);
                newLabelBeatIndex.TextAlign = ContentAlignment.BottomCenter;
                labelBeatIndices.Add(newLabelBeatIndex);
                panelContainer.Controls.Add(newLabelBeatIndex);

                Label newLabelTimeline = new Label();
                newLabelTimeline.Margin = new Padding(0);
                newLabelTimeline.Size = new Size(Program.GRID_HORIZONTAL, flowLayoutPanelTimeline.Height / 2);
                newLabelTimeline.Location = new Point(0, flowLayoutPanelTimeline.Height / 2);
                newLabelTimeline.TextAlign = ContentAlignment.BottomCenter;
                labelTimelines.Add(newLabelTimeline);
                panelContainer.Controls.Add(newLabelTimeline);

                flowLayoutPanelTimeline.Controls.Add(panelContainer);
            }
            RefreshPanelTimeline();
        }

        private void RefreshPanelTimeline()
        {
            int offset = ucEditor.X % Program.GRID_HORIZONTAL + (Program.GRID_HORIZONTAL / 2);
            flowLayoutPanelTimeline.Location = new Point(-offset, flowLayoutPanelTimeline.Location.Y);
            double current = ucEditor.X / Program.GRID_HORIZONTAL / Program.BeatPerMinute * 60;
            for (int i = 0; i < labelTimelines.Count; i++)
            {
                double currentTime = current + Program.DelayLength + ((double)i * 60 / Program.BeatPerMinute);
                labelTimelines[i].Text = Program.DoubleToTimespanString(currentTime);
                labelBeatIndices[i].Text = Program.GetBeatIndex((int)Math.Round((currentTime - Program.DelayLength) / (60 / Program.BeatPerMinute))).ToString();
            }
        }

        private bool ValidatePropertyPanel()
        {
            if (Program.TryTimespanStringToDouble(textBoxStartTime.Text, out startTimeProperty))
            {
                if (Program.TryTimespanStringToDouble(textBoxEndTime.Text, out endTimeProperty))
                {
                    if (endTimeProperty - startTimeProperty >= Program.MinimumLength)                    
                        return true;
                    else
                        MessageBox.Show("End time is earlier than start time or is smaller than minimum length", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("End time is not a valid timespan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Start time is not a valid timespan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void CheckIsUICompatible(List<PitchBar> pitchBarInSentence)
        {
            double startSentencePoint = pitchBarInSentence[0].StartTime;
            double endSentencePoint = pitchBarInSentence[pitchBarInSentence.Count - 1].EndTime;
            double totalDuration = endSentencePoint - startSentencePoint;
            //List<string> lyric = Program.ReadLyricFromPitchBars(pitchBarInSentence);
            //int lyricWidth = FileManager.CalculateWordWidthInPixel(lyric[0]);
            if (totalDuration > 8 /*|| lyricWidth > Program.MAXIMUM_CHARACTER_WIDTH_IN_SENTENCE*/)
            {
                //MessageBox.Show(
                //    "Sentence's total duration exceeds maximum duration threshold (" + 8 + " sec)"// +
                //    //", or Sentence's total character width exceed " + Program.MAXIMUM_CHARACTER_WIDTH_IN_SENTENCE + " pixels." +
                //    //"\n" + "Character width: " + lyricWidth + " pixels, Sentence: " + lyric[0] + "." + "\n" +
                //    //"Total duration: " + totalDuration + " secs (" + Program.DoubleToTimespanString(startSentencePoint) + " - " + Program.DoubleToTimespanString(endSentencePoint) + ")."
                //    , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CheckIsUICompatible(PitchBar pitchBar)
        {
            List<double> durationList;

            durationList = Program.ProcessSentenceDuration(ucEditor.PitchBars);

            if (Program.SentenceCount != durationList.Count)
            {
                if (durationList[pitchBar.GroupIndex] > 8)
                {
                    MessageBox.Show(
                        "Sentence's total duration exceeds maximum duration threshold (" + 8 + " sec)"// +
                        //", or Sentence's total character width exceed " + Program.MAXIMUM_CHARACTER_WIDTH_IN_SENTENCE + " pixels." +
                        //"\n" + "Character width: " + lyricWidth + " pixels, Sentence: " + lyric[0] + "." + "\n" +
                        //"Total duration: " + totalDuration + " secs (" + Program.DoubleToTimespanString(startSentencePoint) + " - " + Program.DoubleToTimespanString(endSentencePoint) + ")."
                        , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            Program.SentenceCount = durationList.Count;
            
            durationList.Clear();
        }

        private void ApplyPropertyPanel(PitchBar pitchBar)
        {
            if (pitchBar != null && ValidatePropertyPanel())
            {
                // apply properties
                pitchBar.StartTime = startTimeProperty;
                pitchBar.EndTime = endTimeProperty;
                pitchBar.NoteIndex = Program.RegisteredNotes.Count - 1 - comboBoxNoteIndex.SelectedIndex;
                pitchBar.Lyric = textBoxLyric.Text;
                pitchBar.IsLastWord = checkBoxIsEnd.Checked;
                pitchBar.ResizeAccordingToPixelPerSecond();

                Program.ProcessGroupAndWordIndex(ucEditor.PitchBars);
                CheckIsUICompatible(pitchBar);
                ucEditor.RefreshScreen();
                UpdateStatus("Property Applied");
                currencyManager.Refresh();
                SaveManager.isDirty = true;

                // check UI compatible
                if (pitchBar.Lyric != null && pitchBar.Lyric != "")
                {
                    //CheckIsUICompatible();
                    //List<PitchBar> pitchBarInSentence = PitchBar.GetPitchBarsInSentences(ucEditor.PitchBars, pitchBar);
                    //CheckIsUICompatible(pitchBarInSentence);
                }
            }
        }

        private void UpdatePropertyPanel(PitchBar selectedPitchBar)
        {
            if (selectedPitchBar == null)
            {
                textBoxStartTime.Text = "";
                textBoxLength.Text = "";
                textBoxLyric.Text = "";
                comboBoxNoteIndex.SelectedIndex = 0;
                textBoxEndTime.Text = "";
                checkBoxIsEnd.Checked = false;
                UpdateStatus("None Selected");
            }
            else
            {
                textBoxStartTime.Text = Program.DoubleToTimespanString(selectedPitchBar.StartTime);
                textBoxLength.Text = Program.DoubleToTimespanString(selectedPitchBar.Length);
                textBoxLyric.Text = selectedPitchBar.Lyric;
                comboBoxNoteIndex.SelectedIndex = Program.PANEL_PIANO_NOTE_COUNT - selectedPitchBar.NoteIndex + Program.PANEL_PIANO_START_NOTE_INDEX - 1;
                textBoxEndTime.Text = Program.DoubleToTimespanString(selectedPitchBar.EndTime);
                checkBoxIsEnd.Checked = selectedPitchBar.IsLastWord;
                UpdateStatus("Selected: " + Program.RegisteredNotes[selectedPitchBar.NoteIndex]);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (SaveManager.isDirty)
            {
                if (MessageBox.Show("Any unsaved changes will be lost.\nDo you want to continue?", "Close Notepad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Program.MIDIPlayer.Dispose();
                    base.OnClosing(e);
                }
                else
                    e.Cancel = true;
            }
            else
            {
                Program.MIDIPlayer.Dispose();
                base.OnClosing(e);
            }
        }

        //
        // event handlers
        //
        private void FormMain_Load(object sender, EventArgs e)
        {
            Show();
            ShowWelcomeScreen();
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            UpdateScrollbars();
            CreatePanelTimeline();
            ucEditor.RefreshScreen();
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                buttonDelete_Click(buttonDelete, EventArgs.Empty);
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            ucEditor.X = e.NewValue;
            isHScrollMoved = true;
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            ucEditor.Y = e.NewValue;
            newFlowLayoutPanelPianoLocation = -e.NewValue;
            isVScrollMoved = true;
        }

        private void ucEditor_PitchBarChanged(object sender, List<PitchBar> newPitchBars)
        {
            listBoxBarnodes.DataSource = newPitchBars;
            listBoxBarnodes.DisplayMember = "StringValue";
            currencyManager = (CurrencyManager)listBoxBarnodes.BindingContext[newPitchBars];
        }

        private void ucEditor_SelectedPitchBarChanging(object sender, PitchBar oldPitchBar, PitchBar newPitchBar)
        {
            if (Program.IsAutoApply && oldPitchBar != null)
                ApplyPropertyPanel(oldPitchBar);
            UpdatePropertyPanel(newPitchBar);
            textBoxLyric.Focus();
            listBoxBarnodes.SelectedIndex = ucEditor.PitchBars.IndexOf(newPitchBar);
            UpdateStatus("");
        }

        private void ucEditor_PitchBarMemberChanged(object sender)
        {
            currencyManager.Refresh();
        }

        private void ucEditor_TimelineChanged(object sender)
        {
            RefreshPanelTimeline();
            hScrollBar.Value = ucEditor.X;
        }

        private void ucEditor_SelectedPitchBarUpdating(object sender, PitchBar newPitchBar)
        {
            UpdatePropertyPanel(newPitchBar);
            textBoxLyric.Focus();
            currencyManager.Refresh();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // check scroll move
            if (isHScrollMoved)
                RefreshPanelTimeline();
            if (isVScrollMoved)
                flowLayoutPanelPiano.Location = new Point(flowLayoutPanelPiano.Location.X, newFlowLayoutPanelPianoLocation);

            // check media play
            if (isMediaPlaying)
            {
                double current = windowsMediaPlayer.Ctlcontrols.currentPosition;
                Program.CurrentPlayingTimeline = current;
                Program.MIDIPlayer.UpdateTime(current - Program.MIDIPlaybackOffset);
                labelCurrentTime.Text = Program.DoubleToTimespanString(current);

                // cara baru, pake marker
                ucEditor.MarkerLocation = (int)((current - Program.DelayLength) * Program.PixelPerSecond);
                ucEditor.RefreshScreen();

                //// cara lama, gerakin frame semua
                //ucEditor.X = (int)(current * Program.PixelPerSecond);
                //ucEditor.RefreshScreen();
                //RefreshPanelTimeline();
            }

            // refresh toolstrip
            toolStripStatusLabelHover.Text = Program.RegisteredNotes[ucEditor.hoveredNote] + " ==> " + Program.DoubleToTimespanString(ucEditor.hoveredTimeline); 

            // reset flags

            if (isHScrollMoved || isVScrollMoved)
                ucEditor.RefreshScreen();
            isHScrollMoved = false;
            isVScrollMoved = false;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopMusic();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            StartMusic();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete?", "Delete pitchbar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                ucEditor.DeleteSelectedPitchBar();
                ucEditor.RefreshScreen();
                UpdateStatus("Deleted");
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplyPropertyPanel(ucEditor.SelectedPitchBar);
        }

        private void listBoxBarnodes_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxBarnodes.SelectedIndex >= 0 && listBoxBarnodes.SelectedIndex < ucEditor.PitchBars.Count)
                ucEditor.ChangeSelectedPitchBar(listBoxBarnodes.SelectedIndex);
        }

        //
        // menu strip event handlers
        //
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveManager.isDirty)
            {
                if (MessageBox.Show("Any unsaved changes will be lost.\nDo you want to continue?", "Open New Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    StartNewFile();
                }
            }
            else
            {
                StartNewFile();
            }
        }

        private void StartNewFile()
        {
            Program.Reset();
            InitializeObjects();
            FillPanelPiano();
            CreatePanelTimeline();
            UpdateScrollbars();
            UpdatePropertyPanel(ucEditor.SelectedPitchBar);
            propertiesToolStripMenuItem_Click(propertiesToolStripMenuItem, EventArgs.Empty);
            SetMediaPlayerUrl();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Lyric File|*.lrc";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    Program.FileLocation = openFileDialog.FileName;
                    FileManager.LoadFromFile(openFileDialog.FileName);
                    ucEditor.OpenNew();
                    ucEditor.LoadPitchBars(Program.PitchBars);
                    Program.ProcessSentenceDuration(ucEditor.PitchBars);
                    ucEditor.RefreshScreen();
                    NextFocusedPitchbar();
                    SetMediaPlayerUrl();
                    windowsMediaPlayer.settings.volume = Program.Volume;
                    RefreshPanelTimeline();
                    UpdateScrollbars();
                    UpdateStatus("Opened Succesfully");
                    SaveManager.hasSavedToRecent = false;
                    SaveManager.isDirty = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error load");
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveManager.Save(this, ucEditor.PitchBars);
            UpdateStatus("Saved Succesfully");
        }

        private void saToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveManager.SaveAs(this, ucEditor.PitchBars);
            UpdateStatus("Saved Succesfully");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFormSong(0);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFormSong(1);
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFormSong(2);
        }

        private void previewLyricToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ProcessGroupAndWordIndex(ucEditor.PitchBars);
            List<string> lyric = Program.ReadLyricFromPitchBars(ucEditor.PitchBars);
            FormPreview preview = new FormPreview(lyric);
            preview.ShowDialog(this);
        }

        private void playPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonPlay_Click(buttonPlay, EventArgs.Empty);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStop_Click(buttonStop, EventArgs.Empty);
        }

        private void importFromMIDIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImportMIDI formImportMIDI = new FormImportMIDI();
            if (formImportMIDI.ShowDialog(this) == DialogResult.OK && formImportMIDI.Pitchbars != null)
                ucEditor.AddRangePitchBars(formImportMIDI.Pitchbars);                    
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLibrary formLibrary = new FormLibrary();
            formLibrary.ShowDialog(this);
        }

        private void textBoxLyric_TextChanged(object sender, EventArgs e)
        {
            ////ucEditor.SelectedPitchBar.Lyric = textBoxLyric.Text;
            ////ucEditor.RefreshScreen();
            ////ApplyPropertyPanel(ucEditor.SelectedPitchBar);

            //PitchBar pitchBar = ucEditor.SelectedPitchBar;
            //if (pitchBar != null/*&& ValidatePropertyPanel()*/)
            //{
            //    // apply properties
            //    //pitchBar.StartTime = startTimeProperty;
            //    //pitchBar.EndTime = endTimeProperty;
            //    //pitchBar.NoteIndex = Program.RegisteredNotes.Count - 1 - comboBoxNoteIndex.SelectedIndex;

            //    System.Diagnostics.Debug.WriteLine("idx " + ucEditor.PitchBars.IndexOf(ucEditor.SelectedPitchBar));

            //    pitchBar.Lyric = textBoxLyric.Text;
            //    //pitchBar.IsLastWord = checkBoxIsEnd.Checked;
            //    pitchBar.ResizeAccordingToPixelPerSecond();
            //    ucEditor.RefreshScreen();
            //    UpdateStatus("Property Applied");
            //    currencyManager.Refresh();
            //    isDirty = true;

            //    // check UI compatible
            //    if (pitchBar.Lyric != null && pitchBar.Lyric != "")
            //    {
            //        List<PitchBar> pitchBarInSentence = PitchBar.GetPitchBarsInSentences(ucEditor.PitchBars, pitchBar);
            //        CheckIsUICompatible(pitchBarInSentence);
            //    }
            //}
        }

        private void OpenFormSong(int index)
        {
            //FormSong formSong = new FormSong();
            formSong.InitializeObjects();
            formSong.SelectIndexTab(index);
            if (formSong.ShowDialog(this) == DialogResult.OK)
            {
                formSong.SetProperties();
                SetMediaPlayerUrl();
                ucEditor.ChangeSize();
                ucEditor.ResizeAllPitchbars();
                ucEditor.RefreshScreen();
                RefreshPanelTimeline();
                UpdateScrollbars();
                windowsMediaPlayer.settings.volume = Program.Volume;
                windowsMediaPlayer.settings.rate = (double)Program.AudioRate / 100;
                UpdateStatus("Settings Saved");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (textBoxLyric.Focused)
            {
                if (keyData == Keys.Tab)
                {
                    NextFocusedPitchbar();

                    return true;
                }
                else if (keyData == Keys.Enter)
                {
                    checkBoxIsEnd.Checked = !checkBoxIsEnd.Checked;
                    return true;
                }
                else if (keyData == (Keys.Alt | Keys.P))
                {
                    StartMusic();
                    return true;
                }
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void NextFocusedPitchbar()
        {
            if (Program.PitchBars.Count > 0)
            {
                ucEditor.NextSelectedPitchBar();

                int newX = ucEditor.X;

                if ((ucEditor.SelectedPitchBar.StartTime * Program.PixelPerSecond < ucEditor.X) ||
                    (ucEditor.SelectedPitchBar.StartTime * Program.PixelPerSecond >= (ucEditor.X + ucEditor.Width * 0.75)) ||
                    (ucEditor.SelectedPitchBar.EndTime * Program.PixelPerSecond >= (ucEditor.X + ucEditor.Width))
                   )
                {
                    newX = (int)(ucEditor.SelectedPitchBar.StartTime * Program.PixelPerSecond - ucEditor.Width * 0.1);
                }

                if (newX < 0)
                    newX = 0;
                else if (newX + ucEditor.Width >= ucEditor.realWidth)
                    newX = ucEditor.realWidth - ucEditor.Width;

                ucEditor.X = newX;
                ucEditor.RefreshScreen();

                RefreshPanelTimeline();
                hScrollBar.Value = ucEditor.X;

                //

                int newY = ucEditor.Y;

                if (ucEditor.SelectedPitchBar.Top > ucEditor.Height)// (ucEditor.Height * 0.8))
                {
                    newY = (int)(ucEditor.SelectedPitchBar.Top - ucEditor.Height * 0.4);
                }
                else if (ucEditor.SelectedPitchBar.Top < 0)//(ucEditor.Height * 0.2))
                {
                    newY = (int)(ucEditor.SelectedPitchBar.Top - ucEditor.Height * 0.4);
                }

                if (newY < 0)
                    newY = 0;
                else if (newY + ucEditor.Height >= ucEditor.realHeight)
                    newY = ucEditor.realHeight - ucEditor.Height;

                ucEditor.Y = newY;
                ucEditor.RefreshScreen();

                RefreshPanelTimeline();
                vScrollBar.Value = ucEditor.Y;
            }
        }

        private void StartMusic()
        {
            if (windowsMediaPlayer.playState == WMPPlayState.wmppsReady || windowsMediaPlayer.playState == WMPPlayState.wmppsStopped)
            {
                if (ucEditor.HasStartPoint && ucEditor.PlayFromStartPoint)
                {
                    windowsMediaPlayer.Ctlcontrols.currentPosition = ucEditor.StartPoint;
                    ucEditor.Play(windowsMediaPlayer.Ctlcontrols.currentPosition);
                    windowsMediaPlayer.Ctlcontrols.play();
                    if (ucEditor.StartPoint < ucEditor.LeftMostTime || ucEditor.StartPoint >= ucEditor.RightMostTime)
                        ucEditor.X = (int)((ucEditor.StartPoint - Program.DelayLength) * Program.PixelPerSecond);
                }
                else
                {
                    double validStartPoint = Program.DelayLength < 0 ? 0 : Program.DelayLength;
                    windowsMediaPlayer.Ctlcontrols.currentPosition = validStartPoint;
                    ucEditor.Play(windowsMediaPlayer.Ctlcontrols.currentPosition);
                    windowsMediaPlayer.Ctlcontrols.play();
                    ucEditor.X = 0;
                }
                ucEditor.RefreshScreen();
                RefreshPanelTimeline();
                ucEditor.IsMediaPlaying = true;
                isMediaPlaying = true;
                UpdateStatus("Playing");

                hScrollBar.Value = ucEditor.X;
            }
            else if (windowsMediaPlayer.playState == WMPPlayState.wmppsPaused)
            {
                ucEditor.Play(windowsMediaPlayer.Ctlcontrols.currentPosition);
                windowsMediaPlayer.Ctlcontrols.play();
                // if marker is outside screen
                if (ucEditor.LeftMostTime > windowsMediaPlayer.Ctlcontrols.currentPosition || ucEditor.RightMostTime <= windowsMediaPlayer.Ctlcontrols.currentPosition)
                {
                    ucEditor.X = (int)(windowsMediaPlayer.Ctlcontrols.currentPosition * Program.PixelPerSecond);
                    ucEditor.RefreshScreen();
                    RefreshPanelTimeline();
                }
                ucEditor.IsMediaPlaying = true;
                isMediaPlaying = true;
                UpdateStatus("Playing");
            }
            else if (windowsMediaPlayer.playState == WMPPlayState.wmppsPlaying)
            {
                ucEditor.Stop();
                windowsMediaPlayer.Ctlcontrols.pause();
                ucEditor.IsMediaPlaying = false;
                isMediaPlaying = false;
                UpdateStatus("Paused");
            }
        }

        private void StopMusic()
        {
            windowsMediaPlayer.Ctlcontrols.stop();
            isMediaPlaying = false;
            ucEditor.IsMediaPlaying = false;
            ucEditor.Stop();
            UpdateStatus("Stopped");
        }

        private void SetMediaPlayerUrl()
        {
            windowsMediaPlayer.URL = Program.MediaLocation;
            buttonPlay.Enabled = (windowsMediaPlayer.URL != "");
            buttonStop.Enabled = buttonPlay.Enabled;
        }

        private void ucEditor_PitchBarChanged(object sender)
        {

        }
        
    }
}
