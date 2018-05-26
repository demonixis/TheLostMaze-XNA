using Maze3D.Control;
using System;

namespace Maze3D
{
    public class LevelFinishEventArgs : EventArgs
    {
        public Score Score;
        public int NextLevel;
        public bool GameFinished;

        public LevelFinishEventArgs()
        {
            Score = new Score();
            NextLevel = 2;
            GameFinished = false;
        }

        public LevelFinishEventArgs(Score score, int nextLevel, bool gameFinished)
        {
            Score = score;
            NextLevel = nextLevel;
            GameFinished = gameFinished;
        }
    }

    public class VirtualPadPressedEventArgs : EventArgs
    {
        public ControlDirection Direction;

        public VirtualPadPressedEventArgs()
        {
            Direction = ControlDirection.None;
        }

        public VirtualPadPressedEventArgs(ControlDirection direction)
        {
            Direction = direction;
        }
    }

    public class MessageBoxEventArgs : EventArgs
    {
        public bool DefaultAction;
        public bool CancelAction;

        public MessageBoxEventArgs()
        {
            DefaultAction = false;
            CancelAction = true;
        }

        // Compatibilité
        public MessageBoxEventArgs(bool defaultAction, bool cancelAction)
        {
            DefaultAction = defaultAction;
            CancelAction = cancelAction;
        }
    }
}
