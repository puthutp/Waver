using System.Collections.Generic;

namespace KaraokeIdol
{
    public class IdolMidi
    {
        public int Type;
        public int NumTracks;
        public float TicksPerQuarterNote;
        public List<List<IdolMidiEvent>> MidiTracks;

        public IdolMidi()
        {
            MidiTracks = new List<List<IdolMidiEvent>>();  
        }
    }
}
