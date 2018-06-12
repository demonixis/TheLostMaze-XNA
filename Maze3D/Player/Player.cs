using System;
using System.Collections.Generic;
using Yna.Engine;

namespace Maze3D
{
    [Serializable]
    public class Player
    {
        private List<Score> m_Scores;
        private int _maxScoreStoring = 10;

        public List<Score> Scores => m_Scores;

        public Player()
        {
            m_Scores = new List<Score>();
        }

        public void AddScore(Score playerScore)
        {
            if (m_Scores.Count == _maxScoreStoring)
                m_Scores.RemoveAt(0);

            m_Scores.Add(playerScore);
        }

        public void ResetScores() => m_Scores.Clear();

        public void Save()
        {
            YnG.StorageManager.Save<List<Score>>("Game", "Scores.dat", m_Scores);
        }

        public void Load()
        {
            var scores = YnG.StorageManager.Load<List<Score>>("Game", "Scores.dat");
            if (scores != null)
                m_Scores = scores;
        }
    }
}
