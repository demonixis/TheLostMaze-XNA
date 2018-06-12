using System;

namespace Maze3D
{
    [Serializable]
    public class Score
    {
        public int GameScore { get; set; }
        public long ElapsedTime { get; set; }
        public int LevelId { get; set; }

        public Score()
        {
            LevelId = 1;
            ElapsedTime = 0;
            GameScore = 0;
        }

        public Score(int levelId) : this() => LevelId = levelId;

        public string GetElapsedTime()
        {
            var time = new TimeSpan(ElapsedTime);
            return $"{time.Minutes} min {time.Seconds} secs";
        }

        public string GetPartyScore() => $"{GameScore} point{(GameScore > 1 ? "s" : "")}";

        public override string ToString() => $"Level {LevelId} - {GetPartyScore()} - {GetElapsedTime()}"; 
    }
}
