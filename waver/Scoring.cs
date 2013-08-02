using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace waver
{
    class Scoring
    {
        string[] refNoteName = { "C4", "D4", "E4", "F4", "G4", "A4", "B4", "C5" };
        int[] refNoteIdx = { 27, 29, 31, 32, 34, 36, 38, 39 };
        int[] refNoteIdxLow = { 15, 17, 19, 20, 22, 24, 26, 27 };

        List<List<int>> harmonicsList = new List<List<int>>() {
            new List<int>() {12, 24, 31},
            new List<int>() {14, 26, 33},
            new List<int>() {16, 28, 35},
            new List<int>() {17, 29, 36},
            new List<int>() {19, 31, 38},
            new List<int>() {21, 33, 40},
            new List<int>() {23, 35, 42},
            new List<int>() {24, 36, 43}
        };

        int toleranceOut = 2;
        int toleranceIn = 2;

        int noteIdx;
        int amountPerBlock;

        float blockSum = 0.0f;
        int blockCount = 0;

        List<float> pointPrevList = new List<float>();
        List<float> pointList = new List<float>();
        List<float> pointNextList = new List<float>();

        List<float> tempScoreList = new List<float>();
        List<float> scoreList = new List<float>();

        double threshold = 50.0f;

        public float GetScore(List<IdolWaveNote> noteList)
        {
            float retval = 0.0f;

            Trim(noteList);
            //DeleteSilence(noteList);

            if (noteList.Count < 8) return 0;
            
            amountPerBlock = noteList.Count / refNoteName.Length;
            toleranceIn = Math.Min((int)(amountPerBlock * 0.3f), 3);
            toleranceOut = Math.Min((int)(amountPerBlock * 0.3f), 3);

//#if TRACE
//            File.WriteAllText(Program.logScore, "");
//#endif

//            retval = ScoreByPoint(noteList, refNoteIdx, 0);

//#if TRACE
//            File.AppendAllText(Program.logScore, "\n");
//            File.AppendAllText(Program.logScore, "rescore with lower note\n");
//            File.AppendAllText(Program.logScore, "\n");
//            Console.WriteLine();
//            Console.WriteLine("rescore with lower note");
//            Console.WriteLine();
//#endif

//            float tempScore = ScoreByPoint(noteList, refNoteIdxLow, 0);
//            if (retval < tempScore) retval = tempScore;

            retval = BruteForceScoring(noteList);

            return retval;
        }

        private float ScoreByPoint(List<IdolWaveNote> noteList, int[] usedIdxRef, int plus)
        {
            float standardSum;
            float prevSum;
            float nextSum;

            int standardCount;
            int prevCount;
            int nextCount;

            int offset = (noteList.Count % refNoteName.Length) / 4;
            //offset = 0;

            scoreList.Clear();
            noteIdx = offset;

            for (int refIdx = 1; refIdx <= refNoteName.Length; refIdx++)
            {
                pointPrevList.Clear();
                pointList.Clear();
                pointNextList.Clear();

                tempScoreList.Clear();

                for (int n = 1; n <= toleranceOut; n++)
                {
                    if (noteIdx - n >= 0)
                    {
                        AddPoint(noteList, noteIdx - n, refIdx - 1, pointPrevList, usedIdxRef, plus);
                    }
                }

                while (noteIdx < amountPerBlock * refIdx + offset)
                {
#if TRACE
                    Console.WriteLine(noteList[noteIdx].NoteName + " " + noteList[noteIdx].freq + " " + noteList[noteIdx].max + " " + noteList[noteIdx].NoteIdx);
                    File.AppendAllText(Program.logScore, noteList[noteIdx].NoteName + " " + noteList[noteIdx].freq + " " + noteList[noteIdx].max + " " + noteList[noteIdx].NoteIdx + "\n");
#endif
                    AddPoint(noteList, noteIdx, refIdx - 1, pointList, usedIdxRef, plus);

                    noteIdx++;
                }

                for (int n = 0; n < toleranceOut; n++)
                {
                    if (noteIdx + n < noteList.Count)
                    {
                        AddPoint(noteList, noteIdx + n, refIdx - 1, pointNextList, usedIdxRef, plus);
                    }
                }

                standardSum = 0;
                standardCount = 0;
                foreach (float point in pointList)
                {
                    if (point >= 0.0f)
                    {
                        standardSum += point;
                        standardCount++;
                    }
                }

                if (standardCount > 0)
                {
                    prevSum = 0;
                    prevCount = 0;
                    for (int ax = 0; ax < pointPrevList.Count; ax++)
                    {
                        if (pointPrevList[ax] >= 0.0f)
                        {
                            prevSum += pointPrevList[ax];
                            prevCount++;
                            blockSum = standardSum + prevSum;
                            blockCount = standardCount + prevCount;
                            tempScoreList.Add(blockSum / blockCount);

                            nextSum = 0;
                            nextCount = 0;
                            for (int bx = 0; bx < pointNextList.Count; bx++)
                            {
                                if (pointNextList[bx] >= 0.0f)
                                {
                                    nextSum += pointNextList[bx];
                                    nextCount++;
                                    tempScoreList.Add((blockSum + nextSum) / (blockCount + nextCount));
                                }
                            }

                            nextSum = 0;
                            nextCount = 0;
                            for (int cx = 1; cx <= toleranceIn; cx++)
                            {
                                if (pointList[pointList.Count - cx] >= 0.0f)
                                {
                                    nextSum += pointList[pointList.Count - cx];
                                    nextCount++;
                                    tempScoreList.Add((blockSum - nextSum) / (blockCount - nextCount));
                                }
                            }
                        }
                    }
                    /**************/
                    blockSum = standardSum;
                    blockCount = standardCount;
                    tempScoreList.Add(blockSum / blockCount);

                    nextSum = 0;
                    nextCount = 0;
                    for (int bx = 0; bx < pointNextList.Count; bx++)
                    {
                        if (pointNextList[bx] >= 0.0f)
                        {
                            nextSum += pointNextList[bx];
                            nextCount++;
                            tempScoreList.Add((blockSum + nextSum) / (blockCount + nextCount));
                        }
                    }

                    nextSum = 0;
                    nextCount = 0;
                    for (int cx = 1; cx <= toleranceIn; cx++)
                    {
                        if (pointList[pointList.Count - cx] >= 0.0f)
                        {
                            nextSum += pointList[pointList.Count - cx];
                            nextCount++;
                            tempScoreList.Add((blockSum - nextSum) / (blockCount - nextCount));
                        }
                    }
                    /**************/
                    prevSum = 0;
                    prevCount = 0;
                    for (int ax = 0; ax < toleranceIn; ax++)
                    {
                        if (pointList[ax] >= 0.0f)
                        {
                            prevSum += pointList[ax];
                            prevCount++;
                            blockSum = standardSum - prevSum;
                            blockCount = standardCount - prevCount;
                            tempScoreList.Add(blockSum / blockCount);

                            nextSum = 0;
                            nextCount = 0;
                            for (int bx = 0; bx < pointNextList.Count; bx++)
                            {
                                if (pointNextList[bx] >= 0.0f)
                                {
                                    nextSum += pointNextList[bx];
                                    nextCount++;
                                    tempScoreList.Add((blockSum + nextSum) / (blockCount + nextCount));
                                }
                            }

                            nextSum = 0;
                            nextCount = 0;
                            for (int cx = 1; cx <= toleranceIn; cx++)
                            {
                                if (pointList[pointList.Count - cx] >= 0.0f)
                                {
                                    nextSum += pointList[pointList.Count - cx];
                                    nextCount++;
                                    tempScoreList.Add((blockSum - nextSum) / (blockCount - nextCount));
                                }
                            }
                        }
                    }

                    scoreList.Add(tempScoreList.Max());
                }
                else
                {
                    scoreList.Add(0);
                }

#if TRACE
                Console.WriteLine("--- " + scoreList[scoreList.Count - 1] + " ---");
                File.AppendAllText(Program.logScore, "--- " + scoreList[scoreList.Count - 1] + " ---\n");
#endif

            }

            return scoreList.Average() * 100;
        }


        private void AddPoint(List<IdolWaveNote> noteList, int idx, int idxRef, List<float> pointList, int[] refNoteIdx, int plus)
        {
            if (noteList[idx].max >= threshold)
            {
                if (noteList[idx].NoteIdx - plus == (refNoteIdx[idxRef]))
                {
                    pointList.Add(1.0f);
                }
                else if (harmonicsList[idxRef].Contains(noteList[idx].NoteIdx - plus))
                {
                    pointList.Add(0.75f);
                }
                else if (Math.Abs((noteList[idx].NoteIdx - plus) - refNoteIdx[idxRef]) == 1)
                {
                    pointList.Add(0.5f);
                }
                else if (Math.Abs((noteList[idx].NoteIdx - plus) - refNoteIdx[idxRef]) == 12)
                {
                    pointList.Add(0.5f);
                }
                else
                {
                    pointList.Add(0.0f);
                }
            }
            else
            {
                pointList.Add(-99.0f);
            }
        }

        private void Trim(List<IdolWaveNote> noteList)
        {
            int maxSound = 2;

            int idx = 0;
            int numSound = 0;
            int lastSilence = -1;

            while (idx < noteList.Count && numSound <= maxSound)
            {
                if (noteList[idx].max >= threshold && noteList[idx].NoteIdx > 0)
                {
                    numSound++;
                }
                else
                {
                    numSound = 0;
                    lastSilence = idx;
                }

                idx++;
            }
            if (lastSilence > -1)
            {
                noteList.RemoveRange(0, lastSilence + 1);
            }

            idx = noteList.Count - 1;
            numSound = 0;
            lastSilence = -1;

            while (idx >= 0 && numSound <= maxSound)
            {
                if (noteList[idx].max >= threshold && noteList[idx].NoteIdx > 0)
                {
                    numSound++;
                }
                else
                {
                    numSound = 0;
                    lastSilence = idx;
                }

                idx--;
            }
            if (lastSilence > -1)
            {
                noteList.RemoveRange(lastSilence, noteList.Count - lastSilence);
            }

            #region simpletrim
            //while (noteList.Count > 0 && (noteList[0].NoteIdx < 0 || noteList[0].max < threshold))
            //{
            //    noteList.RemoveAt(0);
            //}

            //while (noteList.Count > 0 && (noteList[noteList.Count - 1].NoteIdx < 0 || noteList[noteList.Count - 1].max < threshold))
            //{
            //    noteList.RemoveAt(noteList.Count - 1);
            //}
            #endregion
        }

        private float BruteForceScoring(List<IdolWaveNote> noteList)
        {
            int maxIter = 27;//22;
            int curIter = 0;
            List<float> bruteScoreList = new List<float>();

            int[] baseNoteIdx = { 12, 14, 16, 17, 19, 21, 23, 24 };

            for (curIter = -9; curIter <= maxIter; curIter++)
            {
                bruteScoreList.Add(ScoreByPoint(noteList, baseNoteIdx, curIter));
            }

            return bruteScoreList.Max();
        }

        private void DeleteSilence(List<IdolWaveNote> noteList)
        {
            List<IdolWaveNote> toDeleteList = new List<IdolWaveNote>();

            foreach (var note in noteList)
            {
                if (note.max < threshold)
                {
                    toDeleteList.Add(note);
                }
            }

            foreach (var note in toDeleteList)
            {
                noteList.Remove(note);
            }

            toDeleteList.Clear();
        }
    }
}
