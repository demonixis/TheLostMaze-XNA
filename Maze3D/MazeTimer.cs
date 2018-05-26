using System;

namespace Maze3D
{
    public class MazeTimer
    {
        public long ElapsedTime { get; set; }

        public MazeTimer()
        {
            ElapsedTime = 0;
        }

        public void Update(long elapsedTime)
        {
            ElapsedTime += elapsedTime;
        }

        public override string ToString()
        {
            int minutes = (int)(ElapsedTime * 0.000016667) % 60;
            int seconds = (int)(ElapsedTime / 1000) % 60;

            string sMinutes = minutes < 10 ? "0" + minutes : minutes.ToString();
            string sSeconds = seconds < 10 ? "0" + seconds : seconds.ToString();

            return String.Format("{0} : {1}", sMinutes, sSeconds);
        }
    }
}
