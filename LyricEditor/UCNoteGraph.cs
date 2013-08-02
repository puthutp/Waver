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
    public partial class UCNoteGraph : UserControl
    {
        public int PitchStart { get; set; }
        public int PitchRange { get; set; }
        public int NoteWidth { get; set; }
        public double NoteHeight
        {
            get { return (double)Height / PitchRange; }
        }

        private Point lastNoteLocation;
        private BufferedGraphics bufferedGraphics;
        private BufferedGraphicsContext bufferedGraphicsContext;

        public UCNoteGraph()
        {
            InitializeComponent();
            PitchStart = 0;
            PitchRange = 20;
            NoteWidth = 5;
            lastNoteLocation = new Point(0, Height);
            bufferedGraphicsContext = BufferedGraphicsManager.Current;
            bufferedGraphics = bufferedGraphicsContext.Allocate(CreateGraphics(), ClientRectangle);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Clear()
        {
            lastNoteLocation = new Point(0, Height);
            bufferedGraphics.Graphics.Clear(Color.Black);
            Refresh();
        }

        public void AddNote()
        {
            Point newNotePoint = new Point(lastNoteLocation.X + NoteWidth, lastNoteLocation.Y);
            if (newNotePoint.X >= Width)
            {
                newNotePoint.X = NoteWidth;
                bufferedGraphics.Graphics.Clear(Color.Black);
                lastNoteLocation = new Point(0, lastNoteLocation.Y);
            }
            bufferedGraphics.Graphics.DrawLine(Pens.Aqua, lastNoteLocation, newNotePoint);
            lastNoteLocation = newNotePoint;
            Refresh();
        }

        public void AddNote(int pitchIndex)
        {
            Point newNotePoint = new Point(lastNoteLocation.X + NoteWidth, (int)(Height - ((pitchIndex - PitchStart) * NoteHeight)));
            if (newNotePoint.X >= Width)
            {
                newNotePoint.X = NoteWidth;
                bufferedGraphics.Graphics.Clear(Color.Black);
                lastNoteLocation = new Point(0, lastNoteLocation.Y);
            }
            bufferedGraphics.Graphics.DrawLine(Pens.Aqua, lastNoteLocation, newNotePoint);
            lastNoteLocation = newNotePoint;
            Refresh();
        }

        private void UCNoteGraph_SizeChanged(object sender, EventArgs e)
        {
            bufferedGraphics = bufferedGraphicsContext.Allocate(CreateGraphics(), ClientRectangle);
            bufferedGraphics.Graphics.Clear(Color.Black);
            Refresh();
        }

        private void panelGraph_Paint(object sender, PaintEventArgs e)
        {
            bufferedGraphics.Render(e.Graphics);
        }        
    }
}
