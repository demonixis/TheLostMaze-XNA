using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze3D.Data
{
#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public class Player
    {
        private List<Score> _scores;
        private List<Achievement> _achivements;
        private UserConfiguration _configuration;

        public List<Score> Scores
        {
            set { _scores = value; }
            get { return _scores; }
        }

        public List<Achievement> Achivements
        {
            set { _achivements = value; }
            get { return _achivements; }
        }

        public UserConfiguration Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        public Player()
        {
            _scores = new List<Score>();
            _achivements = new List<Achievement>();
            _configuration = new UserConfiguration();
        }
    }
}
