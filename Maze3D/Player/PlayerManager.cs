using Maze3D.Control;
using Maze3D.Data;
using Maze3D.UI;
using System;
using System.Collections.Generic;
using Yna.Engine;

namespace Maze3D
{
    [Serializable]
    public class PlayerManager
    {
        private Player _player;
        private int _maxScoreStoring = 10;

        public List<Score> Scores
        {
            protected set { _player.Scores = value; }
            get { return _player.Scores; }
        }

        public List<Achievement> Achivements
        {
            protected set { _player.Achivements = value; }
            get { return _player.Achivements; }
        }

        public PlayerManager()
        {
            _player = new Player();
        }

        public void AddScore(Score playerScore)
        {
            if (_player.Scores.Count == _maxScoreStoring)
                _player.Scores.RemoveAt(0);

            _player.Scores.Add(playerScore);
        }

        public bool AddAchivement(Achievement achivement)
        {
            if (_player.Achivements.Contains(achivement))
                return false;

            _player.Achivements.Add(achivement);
            return true;
        }

        public void ResetScores()
        {
            _player.Scores.Clear();
        }

        public void ResetAchivements()
        {
            _player.Achivements.Clear();
        }

        public void Save()
        {
            _player.Configuration.ControlMode = (int)GameConfiguration.ControlMode;
            _player.Configuration.DetermineBestResolution = GameConfiguration.DetermineBestResolution;
            _player.Configuration.Difficulty = (int)GameConfiguration.Difficulty;
            _player.Configuration.EnabledFullScreen = GameConfiguration.EnabledFullScreen;
            _player.Configuration.EnabledGamePad = GameConfiguration.EnabledGamePad;
            _player.Configuration.EnabledMinimap = GameConfiguration.EnabledMinimap;
            _player.Configuration.EnabledMinimapItems = GameConfiguration.EnabledMinimapItems;
            _player.Configuration.EnabledMinimapUpdate = GameConfiguration.EnabledMinimapUpdate;
            _player.Configuration.EnabledMouse = GameConfiguration.EnabledMouse;
            _player.Configuration.EnabledMusic = GameConfiguration.EnabledMusic;
            _player.Configuration.EnabledSound = GameConfiguration.EnabledSound;
            _player.Configuration.EnabledVirtualPad = GameConfiguration.EnabledVirtualPad;
            _player.Configuration.LevelsUnlocked = GameConfiguration.LevelsUnlocked;
            _player.Configuration.MapCeilSize = (int)GameConfiguration.MapCeilSize;
            _player.Configuration.MusicVolume = GameConfiguration.MusicVolume;
            _player.Configuration.ScreenHeight = GameConfiguration.ScreenHeight;
            _player.Configuration.ScreenWidth = GameConfiguration.ScreenWidth;
            _player.Configuration.SoundVolume = GameConfiguration.SoundVolume;
            _player.Configuration.VirtualPadSize = (int)GameConfiguration.VirtualPadSize;
            _player.Configuration.VirtualPadStyle = (int)GameConfiguration.VirtualPadStyle;

            YnG.StorageManager.Save<Player>(String.Empty, "player.dat", _player);
        }

        public bool Load()
        {
            var player = YnG.StorageManager.Load<Player>(String.Empty, "player.dat");

            if (player != null)
            {
                _player = player;

                GameConfiguration.ControlMode = (ControlMode)_player.Configuration.ControlMode;
                GameConfiguration.DetermineBestResolution = _player.Configuration.DetermineBestResolution;
                GameConfiguration.Difficulty = (Difficulty)_player.Configuration.Difficulty;
                GameConfiguration.EnabledFullScreen = _player.Configuration.EnabledFullScreen;
                GameConfiguration.EnabledGamePad = _player.Configuration.EnabledGamePad;
                GameConfiguration.EnabledMinimap = _player.Configuration.EnabledMinimap;
                GameConfiguration.EnabledMinimapItems = _player.Configuration.EnabledMinimapItems;
                GameConfiguration.EnabledMinimapUpdate = _player.Configuration.EnabledMinimapUpdate;
                GameConfiguration.EnabledMouse = _player.Configuration.EnabledMouse;
                GameConfiguration.EnabledMusic = _player.Configuration.EnabledMusic;
                GameConfiguration.EnabledSound = _player.Configuration.EnabledSound;
                GameConfiguration.EnabledVirtualPad = _player.Configuration.EnabledVirtualPad;
                GameConfiguration.LevelsUnlocked = _player.Configuration.LevelsUnlocked;
                GameConfiguration.MapCeilSize = (MapCeilSize)_player.Configuration.MapCeilSize;
                GameConfiguration.MusicVolume = _player.Configuration.MusicVolume;
                GameConfiguration.ScreenHeight = _player.Configuration.ScreenHeight;
                GameConfiguration.ScreenWidth = _player.Configuration.ScreenWidth;
                GameConfiguration.SoundVolume = _player.Configuration.SoundVolume;
                GameConfiguration.VirtualPadSize = (VirtualPadSize)_player.Configuration.VirtualPadSize;
                GameConfiguration.VirtualPadStyle = (VirtualPadStyle)_player.Configuration.VirtualPadStyle;

                return true;
            }

            return false;
        }
    }
}
