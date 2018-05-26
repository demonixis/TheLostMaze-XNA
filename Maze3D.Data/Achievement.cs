using System;

namespace Maze3D.Data
{
    public class Achievement
    {
        public int Id { get; set; }
        public long TimeToUnlock { get; set; }
        public int PointsToUnlock { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public bool Unlocked { get; set; }

        public Achievement()
        {
            TimeToUnlock = 0;
            PointsToUnlock = 0;
            Name = "Achivement";
            Points = 0;
            Unlocked = false;
        }

        public bool CheckIfUnlocked(long elapsedTime, int points)
        {
            if (elapsedTime <= TimeToUnlock || points >= PointsToUnlock)
                Unlocked = true;

            return Unlocked;
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1} points", new object[] { Name, Points });
        }
    }
}
