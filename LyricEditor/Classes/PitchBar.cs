using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LyricEditor
{
    public class PitchBar
    {
        //
        // delegates
        //
        public delegate void ResizingHandler(object sender);
        public event ResizingHandler Resizing;
        public delegate void ClickHandler(object sender);
        public event ClickHandler Click;

        private static Pen BorderBackground;
        private static Brush BrushBackground;
        private static Brush BrushBackgroundLastSentences;
        private static Brush BrushBackgroundLong;
        private static Font LyricFont;
        private static StringFormat LyricStringFormat;
        private readonly static int noteOffset = 33;
        private enum CursorState { Nothing, WaitingResizeLeft, WaitingResizeRight, WaitingMoveVertically, IsResizingLeft, IsResizingRight, IsMovingVertically }
        //
        // public data members
        //
        public bool Visible { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public double StartTime { get; set; }
        public double Length
        {
            get { return EndTime - StartTime; }
        }
        public double EndTime { get; set; }
        public int NoteIndex { get; set; }
        public int GroupIndex { get; set; }
        public int WordIndex { get; set; }
        public string Lyric { get; set; }
        public string StringValue
        {
            get { return ToString(); }
        }
        public bool IsLastWord { get; set; }
        public bool IsTooLong { get; set; }

        //
        // private data members
        //
        private bool isClick;
        private double tempStartTime;
        private double tempEndTime;
        private int tempWidth;
        private int tempLeft;
        private int resizeFirstClickX;
        private CursorState cursorState;

        //
        // constructors
        //
        static PitchBar()
        {            
            BorderBackground = new Pen(Color.Black, 1);
            BrushBackground = Brushes.White;
            BrushBackgroundLastSentences = Brushes.Yellow;
            BrushBackgroundLong = Brushes.OrangeRed;
            LyricFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            LyricStringFormat = new StringFormat();
            LyricStringFormat.Alignment = StringAlignment.Center;
            LyricStringFormat.LineAlignment = StringAlignment.Center;
        }

        public PitchBar()
        {
            cursorState = CursorState.Nothing;
            Height = Program.GRID_VERTICAL - 2;
            Visible = true;
            Lyric = "";
        }

        //
        // methods
        //
        private void DrawRoundedRectangle(Graphics gfx, Rectangle Bounds, int CornerRadius, Pen DrawPen, Brush BrushColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(DrawPen.Width));
            Bounds = Rectangle.Inflate(Bounds, -strokeOffset, -strokeOffset);

            DrawPen.EndCap = DrawPen.StartCap = LineCap.Round;

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(Bounds.X, Bounds.Y, CornerRadius, CornerRadius, 180, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y, CornerRadius, CornerRadius, 270, 90);
            gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
            gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
            gfxPath.CloseAllFigures();

            gfx.FillPath(BrushColor, gfxPath);
            gfx.DrawPath(DrawPen, gfxPath);
        }

        public void Draw(Graphics graphics)
        {
            if (Visible)
            {
                if (IsTooLong)
                {
                    DrawRoundedRectangle(graphics, new Rectangle(Left, Top, Width, Height), 10, BorderBackground, BrushBackgroundLong);
                }
                else if (IsLastWord)
                {
                    DrawRoundedRectangle(graphics, new Rectangle(Left, Top, Width, Height), 10, BorderBackground, BrushBackgroundLastSentences);
                }
                else
                {
                    DrawRoundedRectangle(graphics, new Rectangle(Left, Top, Width, Height), 10, BorderBackground, BrushBackground);
                }

                graphics.DrawString(Lyric, LyricFont, Brushes.Black, new RectangleF(Left, Top, Width, Height), LyricStringFormat);
            }
        }

        public bool IsInsideBound(double leftTime, double rightTime)
        {
            return (StartTime >= leftTime && StartTime <= rightTime) || (EndTime >= leftTime && EndTime <= rightTime);
        }

        public bool IsHovered(int x, int y)
        {
            return (x >= Left && x <= Left + Width && y >= Top && y <= Top + Height);
        }

        public void ResizeAccordingToPixelPerSecond()
        {
            Width = (int)(Length * Program.PixelPerSecond);
        }

        public void PlayNote()
        {
            Program.MIDIPlayer.PlayNoteMomentarily(NoteIndex - Program.MIDI_OFFSET);
        }

        public void AddToTrack(Track track, double secondToTickMultiplier)
        {
            int startAbsoluteTick = (int)((StartTime + Program.MIDIPlaybackOffset) / secondToTickMultiplier);
            int endAbsoluteTick = (int)((EndTime + Program.MIDIPlaybackOffset) / secondToTickMultiplier);
            ChannelMessage startMessage = new ChannelMessage(ChannelCommand.NoteOn, 0, NoteIndex - Program.MIDI_OFFSET, Program.MIDIPlayer.Volume);
            ChannelMessage endMessage = new ChannelMessage(ChannelCommand.NoteOff, 0, NoteIndex - Program.MIDI_OFFSET);
            try
            {
                track.Insert(startAbsoluteTick, startMessage);
                track.Insert(endAbsoluteTick, endMessage);
            }
            catch (Exception) { }
        }

        public void MouseDown(MouseEventArgs e)
        {
            OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, e.X - Left, e.Y - Top, e.Delta));
        }

        public void MouseMove(MouseEventArgs e)
        {
            OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, e.X - Left, e.Y - Top, e.Delta));
        }

        public void MouseUp(MouseEventArgs e)
        {
            OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, e.X - Left, e.Y - Top, e.Delta));
        }

        public override string ToString()
        {
            string result = "";
            result += Program.DoubleToTimespanString(StartTime);
            result += " - " + Program.DoubleToTimespanString(EndTime);
            result += " : " + Lyric;            
            return result;
        }

        protected void OnMouseDown(MouseEventArgs mevent)
        {
            if (cursorState == CursorState.WaitingResizeLeft || cursorState == CursorState.WaitingResizeRight)
            {
                if (cursorState == CursorState.WaitingResizeLeft)
                    cursorState = CursorState.IsResizingLeft;
                else
                    cursorState = CursorState.IsResizingRight;
                resizeFirstClickX = mevent.X;
                tempStartTime = StartTime;
                tempEndTime = EndTime;
                tempWidth = Width;
                tempLeft = Left;
            }
            else if (cursorState == CursorState.WaitingMoveVertically)
            {
                cursorState = CursorState.IsMovingVertically;
            }
            isClick = true;
        }

        protected void OnMouseMove(MouseEventArgs mevent)
        {
            if (cursorState == CursorState.Nothing || cursorState == CursorState.WaitingMoveVertically || cursorState == CursorState.WaitingResizeLeft || cursorState == CursorState.WaitingResizeRight)
            {
                int widthHover = Width / 4;
                if (widthHover > 20)
                    widthHover = 20;
                if (mevent.X <= widthHover)
                {
                    Cursor.Current = Cursors.SizeWE;
                    cursorState = CursorState.WaitingResizeLeft;
                }
                else if (mevent.X >= (Width - widthHover))
                {
                    Cursor.Current = Cursors.SizeWE;
                    cursorState = CursorState.WaitingResizeRight;
                }
                else
                {
                    Cursor.Current = Cursors.SizeNS;
                    cursorState = CursorState.WaitingMoveVertically;
                }
            }
            else if (cursorState == CursorState.IsMovingVertically)
            {
                int deltaIndex = 0;
                if (mevent.Y < 0)
                    deltaIndex = (mevent.Y - Program.GRID_VERTICAL) / Program.GRID_VERTICAL;
                else
                    deltaIndex = mevent.Y / Program.GRID_VERTICAL;
                NoteIndex -= deltaIndex;
                NoteIndex = (NoteIndex < Program.PANEL_PIANO_START_NOTE_INDEX) ? Program.PANEL_PIANO_START_NOTE_INDEX : ((NoteIndex >= Program.RegisteredNotes.Count) ? Program.RegisteredNotes.Count - 1 : NoteIndex);
                Top += deltaIndex * Program.GRID_VERTICAL;
                if (Resizing != null)
                    Resizing(this);
            }
            else // is resizing
            {
                isClick = false;
                if (cursorState == CursorState.IsResizingLeft)
                {
                    int realEX = Left - tempLeft + mevent.X;
                    Left = tempLeft + realEX - resizeFirstClickX;
                    Width = tempWidth + tempLeft - Left;
                    StartTime = tempStartTime + ((Left - tempLeft) / Program.PixelPerSecond);
                }
                else if (cursorState == CursorState.IsResizingRight)
                {
                    Width = tempWidth + mevent.X - resizeFirstClickX;
                }
                if (Resizing != null)
                    Resizing(this);
            }
        }

        protected void OnMouseUp(MouseEventArgs mevent)
        {
            if (isClick && Click != null)
            {
                cursorState = CursorState.Nothing;
                Click(this);
            }
            else
            {
                if (cursorState == CursorState.IsResizingLeft)
                {
                    int realEX = Left - tempLeft + mevent.X;
                    Left = tempLeft + realEX - resizeFirstClickX;
                    Width = tempWidth + tempLeft - Left;
                    StartTime = tempStartTime + ((Left - tempLeft) / Program.PixelPerSecond);
                    PitchBar.ResizeToNeighbour(Program.PitchBars, this);
                    if (Length < Program.MinimumLength) // check collision
                    {
                        StartTime = tempStartTime;
                        EndTime = tempEndTime;
                        Width = tempWidth;
                    }
                    if (Resizing != null)
                        Resizing(this);
                }
                else if (cursorState == CursorState.IsResizingRight)
                {
                    Width = tempWidth + mevent.X - resizeFirstClickX;
                    EndTime = StartTime + (Width / Program.PixelPerSecond);
                    PitchBar.ResizeToNeighbour(Program.PitchBars, this);
                    if (Length < Program.MinimumLength) // check collision
                    {
                        StartTime = tempStartTime;
                        EndTime = tempEndTime;
                        Width = tempWidth;
                    }
                    if (Resizing != null)
                        Resizing(this);
                }
                else if (cursorState == CursorState.IsMovingVertically)
                {
                    int deltaIndex = 0;
                    if (mevent.Y < 0)
                        deltaIndex = (mevent.Y - Program.GRID_VERTICAL) / Program.GRID_VERTICAL;
                    else
                        deltaIndex = mevent.Y / Program.GRID_VERTICAL;
                    NoteIndex -= deltaIndex;
                    NoteIndex = (NoteIndex < Program.PANEL_PIANO_START_NOTE_INDEX) ? Program.PANEL_PIANO_START_NOTE_INDEX : ((NoteIndex >= Program.RegisteredNotes.Count) ? Program.RegisteredNotes.Count - 1 : NoteIndex);
                    Top += deltaIndex * Program.GRID_VERTICAL;
                    if (Resizing != null)
                        Resizing(this);
                }
                cursorState = CursorState.Nothing;
            }
        }

        //
        // static methods
        //
        public static void InsertSorted(List<PitchBar> listPitchBar, PitchBar newPitchBar)
        {
            int index = 0;
            if (listPitchBar != null)
            {
                while (index < listPitchBar.Count && listPitchBar[index].StartTime < newPitchBar.StartTime)
                    index++;
                listPitchBar.Insert(index, newPitchBar);
            }
        }

        public static int CalculateTotalPitchbarLength(List<PitchBar> pitchBars)
        {
            double result = 0;
            for (int i = 0; i < pitchBars.Count; i++)
                result += pitchBars[i].Length;
            return (int)(result * 1000);
        }

        public static List<PitchBar> GetPitchBarsInSentences(List<PitchBar> listPitchBar, PitchBar pitchBar)
        {
            int indexDecreement = listPitchBar.IndexOf(pitchBar) - 1;
            bool isFound = false;
            while (!isFound && indexDecreement >= 0)
            {
                isFound = listPitchBar[indexDecreement].IsLastWord;
                if (!isFound)
                    indexDecreement--;
            }
            if (indexDecreement < 0)
                indexDecreement = 0;
            else
                indexDecreement += 1;
            int indexIncreement = listPitchBar.IndexOf(pitchBar);
            isFound = false;
            while (!isFound && indexIncreement < listPitchBar.Count)
            {
                isFound = listPitchBar[indexIncreement].IsLastWord;
                if (!isFound)
                    indexIncreement++;
            }
            if (indexIncreement >= listPitchBar.Count)
                indexIncreement = listPitchBar.Count - 1;
            return listPitchBar.GetRange(indexDecreement, indexIncreement - indexDecreement + 1);            
        }

        public static void ResizeToNeighbour(List<PitchBar> listPitchBar, PitchBar newPitchBar)
        {
            int index = listPitchBar.IndexOf(newPitchBar);
            bool isNeedResize = false;
            // check left
            if (index - 1 >= 0)
            {
                PitchBar leftPitchBar = listPitchBar[index - 1];
                if (newPitchBar.StartTime < leftPitchBar.EndTime)
                {
                    newPitchBar.StartTime = leftPitchBar.EndTime;
                    newPitchBar.Left = leftPitchBar.Left + leftPitchBar.Width;
                    isNeedResize = true;
                }
            }
            // check right
            if (index + 1 < listPitchBar.Count)
            {
                PitchBar rightPitchBar = listPitchBar[index + 1];
                if (newPitchBar.EndTime > rightPitchBar.StartTime)
                {
                    newPitchBar.EndTime = rightPitchBar.StartTime;
                    isNeedResize = true;
                }
            }
            // check need to resize
            if (isNeedResize)
                newPitchBar.ResizeAccordingToPixelPerSecond();
        }
    }
}
