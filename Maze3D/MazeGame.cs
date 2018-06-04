using Maze3D.Data;
using Maze3D.Screen;
using System;
using System.Globalization;
using Yna.Engine;
using Yna.Engine.Content;
using Yna.Engine.Helpers;

namespace Maze3D
{
    public class MazeGame : YnGame
    {
        private bool _gameFinished;
        private int _nextLevel;
        private PlayerManager _playerManager;

        public int NextLevel
        {
            get => _nextLevel;
            set
            {
                if (value < 1)
                    _nextLevel = 1;
                else if (value > GameConfiguration.LevelCount)
                    _nextLevel = GameConfiguration.LevelCount;
                else
                    _nextLevel = value;
            }
        }

        #region Events

        public event EventHandler<EventArgs> QuitRequested = null;
        public event EventHandler<LevelFinishEventArgs> LevelFinished = null;

        #endregion

        public MazeGame()
            : base(GameConfiguration.ScreenWidth, GameConfiguration.ScreenHeight, "The Lost Maze", true)
        {
            InitializeLanguage(String.Empty);
            GameTitle = "The Lost Maze";

            if (GameConfiguration.EnabledFullScreen && !graphics.IsFullScreen)
                graphics.ToggleFullScreen();

            if (GameConfiguration.DetermineBestResolution)
                YnG.DetermineBestResolution(GameConfiguration.EnabledFullScreen);

            ScreenHelper.ScreenWidthReference = 1280;
            ScreenHelper.ScreenHeightReference = 720;

            graphics.PreferredBackBufferWidth = GameConfiguration.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameConfiguration.ScreenHeight;
            graphics.ApplyChanges();

            _gameFinished = false;
            _playerManager = new PlayerManager();
        }

        protected override void Initialize()
        {
            _playerManager.Load();
            _nextLevel = GameConfiguration.LevelStart;

            m_StateManager.Add(new SplashState("splash"), true);
            m_StateManager.Add(new MenuState("menu"), false);
            m_StateManager.Add(new SelectionState("selection"), false);
            m_StateManager.Add(new OptionsState("options"), false);
            m_StateManager.Add(new AboutState("about"), false);

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            _playerManager.Save();
            base.UnloadContent();
        }

        private void InitializeLanguage(string useLang)
        {
            var userLanguage = String.Empty;
            var selectedLanguage = "fr";

            if (useLang == String.Empty)
                userLanguage = CultureInfo.CurrentCulture.Name.Split('-')[0];

            if (userLanguage == "fr")
                selectedLanguage = "fr";
            else
                selectedLanguage = "en";

            var path = String.Format("Data/Translations/translation.{0}", selectedLanguage);

            MazeLang.Text = ContentHelper.LoadXMLFromXna<GameText>(path);
        }

        public void PrepareNewLevel(int levelId, bool isStarted)
        {
            var level = (LevelState)m_StateManager.Get("level");
            var launchLevelId = GameConfiguration.LevelStart;

            if (level != null)
            {
                m_StateManager.Remove(level);

                level.ExitRequest -= OnExitRequest;
                level.LevelFinished -= OnLevelFinished;
                level = null;
                launchLevelId = levelId;
            }

            level = new LevelState("level", launchLevelId);
            level.ExitRequest += OnExitRequest;
            level.LevelFinished += OnLevelFinished;

            m_StateManager.Add(level, isStarted);
            m_StateManager.SetActive("level", true);
        }

        #region Event Management

        private void OnLevelFinished(object sender, LevelFinishEventArgs e)
        {
            YnG.AudioManager.StopMusic();

            var score = e.Score.PartyScore;
            var points = score > 0 ? score + " points" : "0 point";

            if (e.GameFinished)
            {
                NextLevel = 1;
                GameConfiguration.LevelsUnlocked = GameConfiguration.LevelCount;
                YnG.StateManager.SetActive("menu", true);
            }
            else
            {
                NextLevel = e.NextLevel;
                GameConfiguration.LevelsUnlocked = NextLevel;

                var level = (LevelState)m_StateManager.Get("level");
                level.Active = false;
                PrepareNewLevel(_nextLevel, true);
            }

            _gameFinished = e.GameFinished;
            _playerManager.AddScore(e.Score);
            _playerManager.Save();
        }

        private void OnExitRequest(object sender, EventArgs e)
        {
            YnG.AudioManager.StopMusic();
            YnG.StateManager.SetActive("menu", true);
        }

        public void OnQuitRequested(EventArgs e) => QuitRequested?.Invoke(this, e);
        public void OnLevelFinished(LevelFinishEventArgs e) => LevelFinished(this, e);

        #endregion
    }
}
