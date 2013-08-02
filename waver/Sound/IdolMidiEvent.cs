
namespace KaraokeIdol
{
    public class IdolMidiEvent
    {
        public float TimeStamp;
        public int CommandIdx;
        public int Channel;
        public int NoteIdx;
        public string NoteName;
        public int Volume;

        public IdolMidiEvent()
        {
        }

        public IdolMidiEvent(float _timeStamp, int _commandIdx, int _channel, int _noteIdx, string _noteName, int _volume)
        {
            TimeStamp = _timeStamp;
            CommandIdx = _commandIdx;
            Channel = _channel;
            NoteIdx = _noteIdx;
            NoteName = _noteName;
            Volume = _volume;
        }
    }
}
