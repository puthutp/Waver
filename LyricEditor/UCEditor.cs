using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LyricEditor
{
    public partial class UCEditor : UserControl
    {
        //
        // private const
        //
        private const int SEQUENCE_DIVISION = 96;
        private const int SEQUENCE_TEMPO = 625000;
        private const int PLAY_FROM_START_POINT = 0;
        private const int PLAY_FROM_ZERO = 1;
        private readonly byte[] SetTempoData = new byte[] { (byte)9, (byte)137, (byte)104 };
        private readonly byte[] TimeSignatureData = new byte[] { (byte)4, (byte)2, (byte)24, (byte)8 };

        //
        // delegates
        //
        public delegate void PitchbarMemberChangedHandler(object sender);
        public event PitchbarMemberChangedHandler PitchbarMemberChanged;
        public delegate void PitchbarChangedHandler(object sender, List<PitchBar> newPitchbar);
        public event PitchbarChangedHandler PitchbarChanged;
        public delegate void SelectedPitchbarChangingHandler(object sender, PitchBar oldPitchBar, PitchBar newPitchBar);
        public event SelectedPitchbarChangingHandler SelectedPitchbarChanging;
        public delegate void SelectedPitchbarUpdatingHandler(object sender, PitchBar newPitchBar);
        public event SelectedPitchbarUpdatingHandler SelectedPitchbarUpdating;
        public delegate void TimelineChangedHandler(object sender);
        public event TimelineChangedHandler TimelineChanged;

        private static Pen gridPen;
        private static Pen gridBeatPen;
        private static Pen gridFourthBeatPen;
        private static Pen gridNoteDivider;
        private static Pen markerPen;
        private static Brush gridBrush;
        private static Brush gridSharpBrush;

        private BufferedGraphics bufferedGraphics;
        private BufferedGraphicsContext bufferedGraphicsContext;
        private Sequencer sequencer;
        private Sequence sequence;
        private Track pitchBarTrack;

        public int X;
        public int Y;
        public int realWidth;
        public int realHeight;
        public int hoveredNote;
        public double hoveredTimeline;

        public double LeftMostTime
        {
            get { return X / Program.PixelPerSecond + Program.DelayLength; }
        }
        public double RightMostTime
        {
            get { return (X + Width) / Program.PixelPerSecond + Program.DelayLength; }
        }
        public int TopMostNoteIndex
        {
            get { return Y / Program.GRID_VERTICAL; }
        }

        public OutputDevice MIDIOutPutDevice { get; set; }
        public PitchBar LastPitchBar { get; set; }
        private PitchBar selectedPitchBar;
        public PitchBar SelectedPitchBar 
        {
            get { return selectedPitchBar; }
            private set
            {
                LastPitchBar = SelectedPitchBar;
                selectedPitchBar = value;
            }
        }
        public List<PitchBar> PitchBars { get; private set; }
        public bool IsMediaPlaying { get; set; }
        public bool HasStartPoint { get; set; }
        public bool PlayFromStartPoint { get; set; }
        private double TickMultiplier;
        private double startPoint;
        public double StartPoint 
        {
            get { return startPoint; }
            set
            {
                if (value < 0)
                    startPoint = 0;
                else
                    startPoint = value;
            }
        }
        public int MarkerLocation
        {
            get { return markerLocation; }
            set
            {
                markerLocation = value;
                if (IsMediaPlaying && markerLocation - X > Width)
                {
                    X += Width;
                    markerLeft = markerLeft - Width;
                    if (TimelineChanged != null)
                        TimelineChanged(this);
                }
                else
                    markerLeft = markerLocation - X;
            }
        }

        private int markerLeft;
        private int markerLocation;
        private int hoverX;
        private int hoverY;
        private int offsetYToNextNoteIndex;
        private int createPitchBarFirstClick;
        private bool isCreatingPitchBar;
        private bool isClickingPitchBar;
        private PitchBar pitchBarInCreation;
        private PitchBar hoveredPitchBar;

        static UCEditor()
        {
            gridPen = new Pen(Color.Gray, 1);
            gridBeatPen = new Pen(Color.Gray, 2);
            gridFourthBeatPen = new Pen(Color.Black, 2);
            gridNoteDivider = new Pen(Color.Black, 1);
            markerPen = new Pen(Color.Red, 1);
            gridBrush = Brushes.LightBlue;
            gridSharpBrush = Brushes.Blue;
        }

        public UCEditor()
        {
            InitializeComponent();
            TickMultiplier = SEQUENCE_TEMPO / 10000000D / SEQUENCE_DIVISION * Program.AudioRate;
            sequence = new Sequence(SEQUENCE_DIVISION);   
            sequence.Format = 1;
            sequencer = new Sequencer();
            sequencer.Sequence = sequence;
            sequencer.Position = 0;
            sequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(sequencer_ChannelMessagePlayed);
            sequencer.Stopped += new EventHandler<StoppedEventArgs>(sequencer_Stopped);
            sequencer.Chased += new EventHandler<ChasedEventArgs>(sequencer_Chased);
            bufferedGraphicsContext = BufferedGraphicsManager.Current;
            bufferedGraphics = bufferedGraphicsContext.Allocate(CreateGraphics(), ClientRectangle);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            MouseWheel += new MouseEventHandler(UCEditor_MouseWheel);
            OpenNew();
            toolStripComboBoxFrom.SelectedIndex = PLAY_FROM_START_POINT;
        }

        public void OpenNew()
        {
            InitializeObjects();
            RefreshScreen();
        }

        private void InitializeObjects()
        {
            isCreatingPitchBar = false;
            isClickingPitchBar = false;
            ChangeSize();
            LastPitchBar = null;
            SelectedPitchBar = null;
            hoveredPitchBar = null;
            pitchBarInCreation = null;
            PitchBars = new List<PitchBar>();
            sequence.Clear();
            SetMIDITempo();
            pitchBarTrack = new Track();
            sequence.Add(pitchBarTrack);
            if (PitchbarChanged != null)
                PitchbarChanged(this, PitchBars);
        }

        public void Play()
        {
            Play(0);
        }

        public void Play(double timePositionInSecond)
        {
            BufferPitchbarsToMIDI();
            sequencer.Position = (int)((timePositionInSecond) / TickMultiplier);
            sequencer.Continue();
        }

        public void Stop()
        {
            sequencer.Stop();
        }

        public void ChangeSize()
        {
            realWidth = (int)((Program.TimelineLength - Program.DelayLength) * Program.PixelPerSecond);
            realHeight = Program.PANEL_PIANO_NOTE_COUNT * Program.GRID_VERTICAL;
        }

        public void ResizeAllPitchbars()
        {
            for (int i = 0; i < PitchBars.Count; i++)
                PitchBars[i].ResizeAccordingToPixelPerSecond();
        }

        public void LoadPitchBars(List<PitchBar> pitchBars)
        {
            PitchBars = pitchBars;
            for (int i = 0; i < PitchBars.Count; i++)
            {
                PitchBars[i].ResizeAccordingToPixelPerSecond();
                PitchBars[i].Click += new PitchBar.ClickHandler(pitchBarInCreation_Click);
                PitchBars[i].Resizing += new PitchBar.ResizingHandler(pitchBarInCreation_Resizing);
            }
            if (PitchbarChanged != null)
                PitchbarChanged(this, PitchBars);
        }

        public void AddRangePitchBars(List<PitchBar> pitchBars)
        {
            for (int i = 0; i < pitchBars.Count; i++)
            {
                PitchBar.InsertSorted(PitchBars, pitchBars[i]);
                pitchBars[i].ResizeAccordingToPixelPerSecond();
                PitchBar.ResizeToNeighbour(PitchBars, pitchBars[i]);
                if (pitchBars[i].Length >= Program.MinimumLength) // check collide here
                {
                    pitchBars[i].Click += new PitchBar.ClickHandler(pitchBarInCreation_Click);
                    pitchBars[i].Resizing += new PitchBar.ResizingHandler(pitchBarInCreation_Resizing);
                }
                else // collide
                {
                    pitchBars[i].Width = 0;
                    pitchBars[i].Draw(bufferedGraphics.Graphics);
                    PitchBars.Remove(pitchBars[i]);
                    RefreshScreen();
                }
            }
            if (PitchbarMemberChanged != null)
                PitchbarMemberChanged(this);
        }

        public void BufferPitchbarsToMIDI()
        {
            pitchBarTrack.Clear();
            TickMultiplier = SEQUENCE_TEMPO / 100000000D / SEQUENCE_DIVISION * Program.AudioRate;
            for (int i = 0; i < PitchBars.Count; i++)
                PitchBars[i].AddToTrack(pitchBarTrack, TickMultiplier);
        }

        public void ChangeSelectedPitchBar(int index)
        {
            if (PitchBars != null && index < PitchBars.Count)
            {
                SelectedPitchBar = PitchBars[index];
                if (SelectedPitchbarChanging != null)
                    SelectedPitchbarChanging(this, LastPitchBar, SelectedPitchBar);
                
            }
        }

        public void NextSelectedPitchBar()
        {
            if (SelectedPitchBar != null && 1 < PitchBars.Count)
            {
                int index = PitchBars.IndexOf(SelectedPitchBar);
                ChangeSelectedPitchBar((index + 1) % PitchBars.Count);
            }
            else if (PitchBars.Count > 0)
            {
                ChangeSelectedPitchBar(0);
            }
        }

        public void DeleteSelectedPitchBar()
        {
            if (SelectedPitchBar != null)
            {
                int index = PitchBars.IndexOf(SelectedPitchBar);
                PitchBars.Remove(SelectedPitchBar);
                if (PitchbarMemberChanged != null)
                    PitchbarMemberChanged(this);
                if (index < PitchBars.Count)
                    SelectedPitchBar = PitchBars[index];
                else
                {
                    index -= 1;
                    if (index < PitchBars.Count && index >= 0)
                        SelectedPitchBar = PitchBars[index];
                    else
                        SelectedPitchBar = null;
                }
                if (SelectedPitchbarChanging != null)
                    SelectedPitchbarChanging(this, null, SelectedPitchBar);

            }
        }

        public void RefreshScreen()
        {
            bufferedGraphics.Graphics.Clear(Color.White);
            DrawBackground();
            RelocatePitchBars();
            DrawPitchBars();
            DrawPitchBarInCreation();
            DrawMarker();
            Refresh();
        }

        public void RelocatePitchBars() // note the plural, calls RelocatePitchBar singular
        {
            for (int i = 0; i < PitchBars.Count; i++)
            {
                PitchBar currentPitchBar = PitchBars[i];
                if (currentPitchBar.IsInsideBound(LeftMostTime, RightMostTime))
                {
                    currentPitchBar.Visible = true;
                    RelocatePitchBar(currentPitchBar);
                }
                else
                {
                    currentPitchBar.Visible = false;
                }
            }
        }

        public bool IsPitchBarChanged()
        {
            if (SelectedPitchBar != null || LastPitchBar != null)
            {
                if (SelectedPitchBar != LastPitchBar)
                {
                    return true;
                }
            }

            return false;
        }

        public void RelocatePitchBar(PitchBar pitchBar) // note the singular
        {
            pitchBar.Left = (int)((pitchBar.StartTime - LeftMostTime) * Program.PixelPerSecond);
            pitchBar.Top = ((Program.RegisteredNotes.Count - 1 - pitchBar.NoteIndex - TopMostNoteIndex) * Program.GRID_VERTICAL) - offsetYToNextNoteIndex;
        }

        private void DrawPitchBarInCreation()
        {
            if (pitchBarInCreation != null)
                pitchBarInCreation.Draw(bufferedGraphics.Graphics);
        }

        private void DrawPitchBars()
        {
            for (int i = 0; i < PitchBars.Count; i++)
                PitchBars[i].Draw(bufferedGraphics.Graphics);
        }

        private void DrawBackground()
        {
            int backgroundHorizontalCount = Width / Program.BackgroundWidth + 2;
            int backgroundVerticalCount = Height / Program.BackgroundHeight + 2;
            offsetYToNextNoteIndex = Y % Program.GRID_VERTICAL;
            int xOffset = -(X % (Program.BackgroundWidth));
            int yOffset = -(Y % (Program.BackgroundHeight));
            for (int i = 0; i < backgroundVerticalCount; i++)
                DrawNoteLines(yOffset + (i * Program.BackgroundHeight));
            for (int i = 0; i < backgroundHorizontalCount; i++)
                DrawGrid(xOffset + (i * Program.BackgroundWidth));
        }

        private void DrawNoteLines(int yOffset)
        {
            int yLocation = yOffset;
            for (int i = 0; i < Program.NOTE_INTERVAL; i++)
            {
                if (i == 0 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11) // normal notes
                    bufferedGraphics.Graphics.FillRectangle(gridBrush, new Rectangle(0, yLocation, Width, Program.GRID_VERTICAL - 1));
                else // sharp notes
                    bufferedGraphics.Graphics.FillRectangle(gridSharpBrush, new Rectangle(0, yLocation, Width, Program.GRID_VERTICAL - 1));
                yLocation += Program.GRID_VERTICAL;
                bufferedGraphics.Graphics.DrawLine(gridNoteDivider, 0, yLocation, Width, yLocation);
            }
        }

        private void DrawMarker()
        {
            if (IsMediaPlaying)
                bufferedGraphics.Graphics.DrawLine(markerPen, markerLeft, 0, markerLeft, Height);
            else if (HasStartPoint)
            {
                MarkerLocation = (int)((StartPoint - Program.DelayLength) * Program.PixelPerSecond);
                bufferedGraphics.Graphics.DrawLine(markerPen, markerLeft, 0, markerLeft, Height);
            }
        }

        private void DrawGrid(int xOffset)
        {
            for (int i = 0; i < Program.GridInterval; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int xLocation = (((i * 4) + j) * (Program.GRID_HORIZONTAL / 4)) + xOffset;
                    if (i == 0 && j == 0)
                        bufferedGraphics.Graphics.DrawLine(gridFourthBeatPen, new Point(xLocation, 0), new Point(xLocation, Height));
                    else if (i != 0 && j == 0)
                        bufferedGraphics.Graphics.DrawLine(gridBeatPen, new Point(xLocation, 0), new Point(xLocation, Height));
                    else
                        bufferedGraphics.Graphics.DrawLine(gridPen, new Point(xLocation, 0), new Point(xLocation, Height));
                }
            }
        }

        private void CheckHover(MouseEventArgs e)
        {
            bool isHoveringPitchBar = false;
            int pitchBarIndex = 0;
            while (!isHoveringPitchBar && pitchBarIndex < PitchBars.Count)
            {
                if (PitchBars[pitchBarIndex].IsInsideBound(LeftMostTime, RightMostTime))
                {
                    isHoveringPitchBar = PitchBars[pitchBarIndex].IsHovered(e.X, e.Y);
                }
                if (!isHoveringPitchBar)
                    pitchBarIndex++;
            }
            if (isHoveringPitchBar)
            {
                hoveredPitchBar = PitchBars[pitchBarIndex];
            }
            else
                hoveredPitchBar = null;
        }

        private void UCEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            int newX = X - e.Delta;
            if (newX < 0)
                newX = 0;
            else if (newX + Width >= realWidth)
                newX = realWidth - Width;
            X = newX;
            RefreshScreen();
            if (TimelineChanged != null)
                TimelineChanged(this);
        }

        private void SetMIDITempo()
        {
            Track setTempoTrack = new Track();
            setTempoTrack.Insert(0, new MetaMessage(MetaType.TimeSignature, TimeSignatureData));
            setTempoTrack.Insert(0, new MetaMessage(MetaType.Tempo, SetTempoData));
            sequence.Add(setTempoTrack);
        }

        private void UCEditor_Paint(object sender, PaintEventArgs e)
        {
            bufferedGraphics.Render(e.Graphics);
        }

        private void UCEditor_SizeChanged(object sender, EventArgs e)
        {
            bufferedGraphics = bufferedGraphicsContext.Allocate(CreateGraphics(), ClientRectangle);
            RefreshScreen();
        }

        private void pitchBarInCreation_Resizing(object sender)
        {
            SelectedPitchBar = sender as PitchBar;
            RefreshScreen();
            if (SelectedPitchbarUpdating != null)
                SelectedPitchbarUpdating(this, SelectedPitchBar);
        }

        private void pitchBarInCreation_Click(object sender)
        {
            if (SelectedPitchbarChanging != null)
                SelectedPitchbarChanging(this, SelectedPitchBar, sender as PitchBar);
            SelectedPitchBar = sender as PitchBar;
            SelectedPitchBar.PlayNote();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (!isClickingPitchBar && hoveredPitchBar != null)
                {
                    hoveredPitchBar.MouseDown(e);
                    isClickingPitchBar = true;
                }
                else // not hovering pitchbar
                {
                    if (!isCreatingPitchBar)
                    {
                        isCreatingPitchBar = true;
                        createPitchBarFirstClick = e.X;
                        pitchBarInCreation = new PitchBar();
                        pitchBarInCreation.StartTime = hoveredTimeline;
                        pitchBarInCreation.NoteIndex = hoveredNote;
                        pitchBarInCreation.Left = e.X;
                        pitchBarInCreation.Top = ((e.Y + offsetYToNextNoteIndex) / Program.GRID_VERTICAL * Program.GRID_VERTICAL) - offsetYToNextNoteIndex;
                        pitchBarInCreation.Click += new PitchBar.ClickHandler(pitchBarInCreation_Click);
                        pitchBarInCreation.Resizing += new PitchBar.ResizingHandler(pitchBarInCreation_Resizing);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
                contextMenuStrip.Show(this, e.Location);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e); 
            hoverX = X + e.X;
            hoverY = Y + e.Y;
            hoveredTimeline = ((double)hoverX / Program.PixelPerSecond) + Program.DelayLength;
            hoveredNote = Program.RegisteredNotes.Count - 1 - (hoverY / Program.GRID_VERTICAL);
            hoveredNote = (hoveredNote < Program.PANEL_PIANO_START_NOTE_INDEX) ? Program.PANEL_PIANO_START_NOTE_INDEX : ((hoveredNote >= Program.RegisteredNotes.Count) ? Program.RegisteredNotes.Count - 1 : hoveredNote);
            if (!isClickingPitchBar && !isCreatingPitchBar)
                CheckHover(e);
            if (hoveredPitchBar != null)
                hoveredPitchBar.MouseMove(e);
            else if (isCreatingPitchBar)
            {
                if (e.X >= createPitchBarFirstClick) // resize normally
                {
                    pitchBarInCreation.Left = createPitchBarFirstClick;
                    pitchBarInCreation.Width = e.X - createPitchBarFirstClick;
                }
                else // resize while reposition
                {
                    pitchBarInCreation.Left = e.X;
                    pitchBarInCreation.Width = createPitchBarFirstClick - e.X;
                    pitchBarInCreation.StartTime = hoveredTimeline;
                }
                RefreshScreen();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (isClickingPitchBar)
            {
                hoveredPitchBar.MouseUp(e);
                isClickingPitchBar = false;
            }
            else if (isCreatingPitchBar)
            {
                isCreatingPitchBar = false;
                pitchBarInCreation.EndTime = pitchBarInCreation.StartTime + (pitchBarInCreation.Width / Program.PixelPerSecond);
                PitchBar.InsertSorted(PitchBars, pitchBarInCreation);
                PitchBar.ResizeToNeighbour(PitchBars, pitchBarInCreation);
                if (pitchBarInCreation.Length >= Program.MinimumLength) // check collide here
                {                    
                    pitchBarInCreation.PlayNote();
                    if (PitchbarMemberChanged != null)
                        PitchbarMemberChanged(this);
                    if (SelectedPitchbarChanging != null)
                        SelectedPitchbarChanging(this, SelectedPitchBar, pitchBarInCreation);
                    SelectedPitchBar = pitchBarInCreation;
                }
                else // collide
                {
                    pitchBarInCreation.Width = 0;
                    pitchBarInCreation.Draw(bufferedGraphics.Graphics);
                    PitchBars.Remove(pitchBarInCreation);
                    RefreshScreen();
                }
                RefreshScreen();
            }
        }

        private void placeStartPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HasStartPoint = true;
            StartPoint = hoveredTimeline;
            RefreshScreen();
        }

        private void deleteStartPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HasStartPoint = false;
            RefreshScreen();
        }

        private void toolStripComboBoxFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlayFromStartPoint = toolStripComboBoxFrom.SelectedIndex == PLAY_FROM_START_POINT;
            RefreshScreen();
        }

        private void sequencer_Chased(object sender, ChasedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
                MIDIOutPutDevice.Send(message);
        }

        private void sequencer_Stopped(object sender, StoppedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
                MIDIOutPutDevice.Send(message);
        }

        private void sequencer_ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            MIDIOutPutDevice.Send(e.Message);
        }
    }
}
