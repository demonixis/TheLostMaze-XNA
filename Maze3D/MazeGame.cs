using Maze3D.Screen;
using System;
using Yna.Engine;
using Yna.Engine.Helpers;

namespace Maze3D
{
    public class MazeGame : YnGame
    {
        private bool _gameFinished;
        private int _nextLevel;
        private Player _playerManager;

        public int NextLevel
        {
            get => _nextLevel;
            set
            {
                if (value < 1)
                    _nextLevel = 1;
                else if (value > GameSettings.LevelCount)
                    _nextLevel = GameSettings.LevelCount;
                else
                    _nextLevel = value;
            }
        }


        public MazeGame()
            : base()
        {
            GameTitle = "The Lost Maze";
            GameVersion = "1.6.0.0";

            Components.Add(new Translation(this));

            var settings = GameSettings.Instance;

            if (settings.EnabledFullScreen && !_graphicsDevice.IsFullScreen)
                _graphicsDevice.ToggleFullScreen();

            if (settings.DetermineBestResolution)
                YnG.DetermineBestResolution(settings.EnabledFullScreen);

            ScreenHelper.ScreenWidthReference = 1280;
            ScreenHelper.ScreenHeightReference = 720;

            _graphicsDevice.PreferredBackBufferWidth = settings.ScreenWidth;
            _graphicsDevice.PreferredBackBufferHeight = settings.ScreenHeight;
            _graphicsDevice.ApplyChanges();

            _gameFinished = false;
            _playerManager = new Player();
        }

        protected override void Initialize()
        {
            _playerManager.Load();
            _nextLevel = GameSettings.Instance.LevelStart;

            _stateManager.Add(new SplashState("splash"), true);
            _stateManager.Add(new MenuState("menu"), false);
            _stateManager.Add(new SelectionState("selection"), false);
            _stateManager.Add(new OptionsState("options"), false);
            _stateManager.Add(new AboutState("about"), false);

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            _playerManager.Save();
            base.UnloadContent();
        }

        public void PrepareNewLevel(int levelId, bool isStarted)
        {
            var level = (LevelState)_stateManager.Get("level");
            var launchLevelId = GameSettings.Instance.LevelStart;

            if (level != null)
            {
                _stateManager.Remove(level);

                level.ExitRequest -= OnExitRequest;
                level.LevelFinished -= OnLevelFinished;
                level = null;
                launchLevelId = levelId;
            }

            level = new LevelState("level", launchLevelId);
            level.ExitRequest += OnExitRequest;
            level.LevelFinished += OnLevelFinished;

            _stateManager.Add(level, isStarted);
            _stateManager.SetActive("level", true);
        }

        #region Event Management

        private void OnLevelFinished(Score score, int nextLevel, bool gameFinished)
        {
            YnG.AudioManager.StopMusic();

            var points = score.GameScore > 0 ? score + " points" : "0 point";
            var settings = GameSettings.Instance;

            if (gameFinished)
            {
                NextLevel = 1;
                settings.LevelsUnlocked = GameSettings.LevelCount;
                YnG.StateManager.SetActive("menu", true);
            }
            else
            {
                NextLevel = nextLevel;
                settings.LevelsUnlocked = NextLevel;

                var level = (LevelState)_stateManager.Get("level");
                level.Active = false;
                PrepareNewLevel(_nextLevel, true);
            }

            _gameFinished = gameFinished;
            _playerManager.AddScore(score);
            _playerManager.Save();
        }

        private void OnExitRequest()
        {
            YnG.AudioManager.StopMusic();
            YnG.StateManager.SetActive("menu", true);
        }

        #endregion
    }
}
