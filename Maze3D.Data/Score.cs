using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze3D
{
#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public class Score
    {
        private int _score;
        private long _elapsedTime;
        private int _levelId;

        public int PartyScore
        {
            get { return _score; }
            set { _score = value; }
        }

        public long ElapsedTime
        {
            get { return _elapsedTime; }
            set { _elapsedTime = value; }
        }

        public int LevelId
        {
            get { return _levelId; }
            set { _levelId = value; }
        }

        public Score()
        {
            _levelId = 1;
            _elapsedTime = 0;
            _score = 0;
        }

        public Score(int levelId)
            : this()
        {
            _levelId = levelId;
        }

        public string GetElapsedTime()
        {
            TimeSpan time = new TimeSpan(_elapsedTime);
            return String.Format("{0} min {1} secs", new object[] { time.Minutes, time.Seconds });
        }

        public string GetPartyScore()
        {
            return String.Format("{0} point{1}", new object[] { _score, (_score > 1 ? "s" : "").ToString() });
        }

        public override string ToString()
        {
            return String.Format("Level {0} - {1} - {2}", new object[] { _levelId, GetPartyScore(), GetElapsedTime() });  
        }
    }
}
