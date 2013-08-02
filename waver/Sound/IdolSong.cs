using System.Collections.Generic;
using System.IO;

namespace KaraokeIdol
{
    public class IdolNote
    {
        public float BeginSeconds;
        public float EndSeconds;
        public int NoteIdx;
        public float TextWidth
        {
            get
            {
                return Lyric.Length * IdolConstants.STAGE_LYRICS_LETTER_WIDTH;
            }
            set
            {
            }
        }
        public string Lyric;        
        public float LengthSeconds
        {
            get
            {
                return EndSeconds - BeginSeconds;
            }
        }
    }

    public class IdolWord
    {
        public List<IdolNote> Notes;
        int i;

        public float BeginSeconds
        {
            get
            {
                if (Notes.Count > 0)
                {
                    return Notes[0].BeginSeconds;
                }
                return -99;
            }
        }
        public float EndSeconds
        {
            get
            {
                if (Notes.Count > 0)
                {
                    return Notes[Notes.Count - 1].EndSeconds;
                }

                return -99;
            }
        }
        public float LengthSeconds
        {
            get
            {
                if (Notes.Count > 0)
                {
                    return Notes[Notes.Count - 1].EndSeconds - Notes[0].BeginSeconds;
                }

                return 0;
            }
        }

        public IdolWord()
        {
            Notes = new List<IdolNote>();
        }

        int maxNoteIdx = -99;
        public int MaxNoteIdx
        {
            get
            {
                if (maxNoteIdx == -99)
                {
                    if (Notes.Count > 0)
                    {
                        maxNoteIdx = Notes[0].NoteIdx;

                        for (i = 1; i < Notes.Count; i++)
                        {
                            if (maxNoteIdx < Notes[i].NoteIdx)
                            {
                                maxNoteIdx = Notes[i].NoteIdx;
                            }
                        }
                    }
                    else
                    {
                        maxNoteIdx = -99;
                    }
                }

                return maxNoteIdx;
            }
        }

        int minNoteIdx = -99;
        public int MinNoteIdx
        {
            get
            {
                if (minNoteIdx == -99)
                {
                    if (Notes.Count > 0)
                    {
                        minNoteIdx = Notes[0].NoteIdx;

                        for (i = 1; i < Notes.Count; i++)
                        {
                            if (minNoteIdx > Notes[i].NoteIdx)
                            {
                                minNoteIdx = Notes[i].NoteIdx;
                            }
                        }
                    }
                    else
                    {
                        minNoteIdx = -99;
                    }
                }

                return minNoteIdx;
            }
        }
    }

    public class IdolSentence
    {
        public List<IdolWord> Words;
        int i;

        public float BeginSeconds
        {
            get
            {
                if (Words.Count > 0)
                {
                    return Words[0].BeginSeconds;
                }
                return -99;
            }
        }
        public float EndSeconds
        {
            get
            {
                if (Words.Count > 0)
                {
                    return Words[Words.Count - 1].EndSeconds;
                }

                return -99;
            }
        }
        public float LengthSeconds
        {
            get
            {
                if (Words.Count > 0)
                {
                    return Words[Words.Count - 1].EndSeconds - Words[0].BeginSeconds;
                }

                return 0;
            }
        }

        public IdolSentence()
        {
            Words = new List<IdolWord>();
        }

        int maxNoteIdx = -99;
        public int MaxNoteIdx
        {
            get
            {
                if (maxNoteIdx == -99)
                {
                    if (Words.Count > 0)
                    {
                        maxNoteIdx = Words[0].MaxNoteIdx;

                        for (i = 1; i < Words.Count; i++)
                        {
                            if (maxNoteIdx < Words[i].MaxNoteIdx)
                            {
                                maxNoteIdx = Words[i].MaxNoteIdx;
                            }
                        }
                    }
                    else
                    {
                        maxNoteIdx = -99;
                    }
                }

                return maxNoteIdx;
            }
        }

        int minNoteIdx = -99;
        public int MinNoteIdx
        {
            get
            {
                if (minNoteIdx == -99)
                {
                    if (Words.Count > 0)
                    {
                        minNoteIdx = Words[0].MinNoteIdx;

                        for (i = 1; i < Words.Count; i++)
                        {
                            if (minNoteIdx > Words[i].MinNoteIdx)
                            {
                                minNoteIdx = Words[i].MinNoteIdx;
                            }
                        }
                    }
                    else
                    {
                        minNoteIdx = -99;
                    }
                }

                return minNoteIdx;
            }
        }
    }

    public class IdolSong
    {
        public List<IdolSentence> Sentences;
        public float TotalSongTextSeconds;
        public float TotalSongSeconds;
        public string SongDuration;
        public int BeatPerMinute;
        public float CapturingElapsed;

        public IdolSong()
        {
            Sentences = new List<IdolSentence>();
        }

        static int i, j;
        static string songText;
        static int tempInt;
        static int currentSentenceIdx, currentWordIdx;
        static IdolSong currentSong;
        static IdolSentence currentSentence;
        static IdolWord currentWord;
        static IdolNote currentNote;

        int maxNoteIdx = -99;
        public int MaxNoteIdx
        {
            get
            {
                if (maxNoteIdx == -99)
                {
                    if (Sentences.Count > 0)
                    {
                        maxNoteIdx = Sentences[0].MaxNoteIdx;

                        for (i = 1; i < Sentences.Count; i++)
                        {
                            if (maxNoteIdx < Sentences[i].MaxNoteIdx)
                            {
                                maxNoteIdx = Sentences[i].MaxNoteIdx;
                            }
                        }
                    }
                    else
                    {
                        maxNoteIdx = -99;
                    }
                }

                return maxNoteIdx;
            }
        }

        int minNoteIdx = -99;
        public int MinNoteIdx
        {
            get
            {
                if (minNoteIdx == -99)
                {
                    if (Sentences.Count > 0)
                    {
                        minNoteIdx = Sentences[0].MinNoteIdx;

                        for (i = 1; i < Sentences.Count; i++)
                        {
                            if (minNoteIdx > Sentences[i].MinNoteIdx)
                            {
                                minNoteIdx = Sentences[i].MinNoteIdx;
                            }
                        }
                    }
                    else
                    {
                        minNoteIdx = -99;
                    }
                }

                return minNoteIdx;
            }
        }

        int noteIdxRange;
        public int NoteIdxRange
        {
            get
            {
                if (noteIdxRange == -99)
                {
                    if (MaxNoteIdx != -99 &&
                        MinNoteIdx != -99)
                    {
                        noteIdxRange = MaxNoteIdx - MinNoteIdx;
                    }
                    else
                    {
                        noteIdxRange = -99;
                    }
                }

                return noteIdxRange;
            }
        }

        public static IdolSong LoadSong(string _path)
        {
            currentSong = new IdolSong();

            songText = SGateContentManager.GetString(_path);
            currentSentenceIdx = -1;
            currentWordIdx = -1;

            //start parsing lyrics
            i = 0;
            while (i < songText.Length)
            {
                if (songText[i] == '[')
                {
                    //[
                    i++;

                    if (songText[i] == 'N')
                    {
                        //parsing note total length

                        //NoteTotalLength]
                        i += 16;

                        //milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentSong.TotalSongTextSeconds = ((float)tempInt / 1000f);

                        //milliseconds + \r + \n
                        i += j + 1;
                    }
                    else if (songText[i] == 'L')
                    {
                        //parsing songText duration

                        //Length]
                        i += 7;

                        //milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        tempInt = int.Parse(songText.Substring(i, j));
                        if ((tempInt / 1000) / 60 < 10)
                        {
                            if ((tempInt / 1000) % 60 < 10)
                            {
                                currentSong.SongDuration = string.Format("0{0}:0{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                            else
                            {
                                currentSong.SongDuration = string.Format("0{0}:{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                        }
                        else
                        {
                            if ((tempInt / 1000) % 60 < 10)
                            {
                                currentSong.SongDuration = string.Format("{0}:0{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                            else
                            {
                                currentSong.SongDuration = string.Format("{0}:{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                        }

                        currentSong.TotalSongSeconds = ((float)tempInt / 1000f);

                        //milliseconds + \r + \n
                        i += j + 1;
                    }
                    else if (songText[i] == 'B')
                    {
                        //parsing beat per minute

                        //BeatPerMinute]
                        i += 14;

                        //milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentSong.BeatPerMinute = tempInt;
                        currentSong.CapturingElapsed = 0.25f * 60f / (float)tempInt;

                        //milliseconds + \r + \n
                        i += j + 1;
                    }
                    else if (char.IsNumber(songText[i]))
                    {
                        currentNote = new IdolNote();

                        //begin milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != ':');

                        int tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.BeginSeconds = (float)tempInt / 1000f;

                        //begin milliseconds:
                        i += j + 1;

                        //end milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != ']');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.EndSeconds = (float)tempInt / 1000f;

                        //end milliseconds]<
                        i += j + 2;

                        //note idx
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '>');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.NoteIdx = tempInt - 24;

                        //note idx>(
                        i += j + 2;

                        //sentence index
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != ')');

                        tempInt = int.Parse(songText.Substring(i, j));

                        //is this new sentence?
                        if (currentSentenceIdx < tempInt)
                        {
                            currentSentence = new IdolSentence();
                            currentSentenceIdx++;

                            currentWord = new IdolWord();
                            currentWordIdx = -1;

                            currentSong.Sentences.Add(currentSentence);
                        }

                        //sentence idx){
                        i += j + 2;

                        //word idx
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '}');

                        tempInt = int.Parse(songText.Substring(i, j));

                        //is this new word?
                        if (currentWordIdx < tempInt)
                        {
                            currentWord = new IdolWord();
                            currentSentence.Words.Add(currentWord);

                            currentWordIdx++;
                        }

                        //word idx}<
                        i += j + 2;

                        //pixel length
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '>');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.TextWidth = tempInt;

                        //pixel length>
                        i += j + 1;

                        //note lyric
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        currentNote.Lyric = songText.Substring(i, j);

                        currentWord.Notes.Add(currentNote);

                        i += j;
                    }
                }

                i++;
            }

            return currentSong;
        }

        public static IdolSong LoadSong(Stream _stream)
        {
            currentSong = new IdolSong();

            songText = SGateContentManager.GetString(_stream);
            currentSentenceIdx = -1;
            currentWordIdx = -1;

            //start parsing lyrics
            i = 0;
            while (i < songText.Length)
            {
                if (songText[i] == '[')
                {
                    //[
                    i++;

                    if (songText[i] == 'N')
                    {
                        //parsing note total length

                        //NoteTotalLength]
                        i += 16;

                        //milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentSong.TotalSongTextSeconds = ((float)tempInt / 1000f);

                        //milliseconds + \r + \n
                        i += j + 1;
                    }
                    else if (songText[i] == 'L')
                    {
                        //parsing songText duration

                        //Length]
                        i += 7;

                        //milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        tempInt = int.Parse(songText.Substring(i, j));
                        if ((tempInt / 1000) / 60 < 10)
                        {
                            if ((tempInt / 1000) % 60 < 10)
                            {
                                currentSong.SongDuration = string.Format("0{0}:0{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                            else
                            {
                                currentSong.SongDuration = string.Format("0{0}:{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                        }
                        else
                        {
                            if ((tempInt / 1000) % 60 < 10)
                            {
                                currentSong.SongDuration = string.Format("{0}:0{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                            else
                            {
                                currentSong.SongDuration = string.Format("{0}:{1}", (tempInt / 1000) / 60, (tempInt / 1000) % 60);
                            }
                        }

                        currentSong.TotalSongSeconds = ((float)tempInt / 1000f);

                        //milliseconds + \r + \n
                        i += j + 1;
                    }
                    else if (songText[i] == 'B')
                    {
                        //parsing beat per minute

                        //BeatPerMinute]
                        i += 14;

                        //milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentSong.BeatPerMinute = tempInt;
                        currentSong.CapturingElapsed = 0.25f * 60f / (float)tempInt;

                        //milliseconds + \r + \n
                        i += j + 1;
                    }
                    else if (char.IsNumber(songText[i]))
                    {
                        currentNote = new IdolNote();

                        //begin milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != ':');

                        int tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.BeginSeconds = (float)tempInt / 1000f;

                        //begin milliseconds:
                        i += j + 1;

                        //end milliseconds
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != ']');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.EndSeconds = (float)tempInt / 1000f;

                        //end milliseconds]<
                        i += j + 2;

                        //note idx
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '>');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.NoteIdx = tempInt - 24;

                        //note idx>(
                        i += j + 2;

                        //sentence index
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != ')');

                        tempInt = int.Parse(songText.Substring(i, j));

                        //is this new sentence?
                        if (currentSentenceIdx < tempInt)
                        {
                            currentSentence = new IdolSentence();
                            currentSentenceIdx++;

                            currentWord = new IdolWord();
                            currentWordIdx = -1;

                            currentSong.Sentences.Add(currentSentence);
                        }

                        //sentence idx){
                        i += j + 2;

                        //word idx
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '}');

                        tempInt = int.Parse(songText.Substring(i, j));

                        //is this new word?
                        if (currentWordIdx < tempInt)
                        {
                            currentWord = new IdolWord();
                            currentSentence.Words.Add(currentWord);

                            currentWordIdx++;
                        }

                        //word idx}<
                        i += j + 2;

                        //pixel length
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '>');

                        tempInt = int.Parse(songText.Substring(i, j));

                        currentNote.TextWidth = tempInt;

                        //pixel length>
                        i += j + 1;

                        //note lyric
                        j = 0;
                        do
                        {
                            j++;
                        } while (songText[i + j] != '\r');

                        currentNote.Lyric = songText.Substring(i, j);

                        currentWord.Notes.Add(currentNote);

                        i += j;
                    }
                }

                i++;
            }

            return currentSong;
        }
    }
}
