using Maze3D.Data;
using Maze3D.Screen;
using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Threading;
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

        private SplashState splashState;
        private MenuState menuState;
        private SelectionState selectionState;
        private OptionsState optionsState;
        private AboutState aboutState;
        private PopupState popupState;

        public int NextLevel
        {
            get { return _nextLevel; }
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

        public void OnQuitRequested(EventArgs e)
        {
            if (QuitRequested != null)
                QuitRequested(this, e);
        }

        public void OnLevelFinished(LevelFinishEventArgs e)
        {
            if (LevelFinished != null)
                LevelFinished(this, e);
        }

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

        private void InitializeLanguage(string useLang)
        {
            string userLanguage = String.Empty;

            if (useLang == String.Empty)
                userLanguage = CultureInfo.CurrentCulture.Name.Split(new char[] { '-' })[0];

            // Français par défaut
            string selectedLanguage = "fr";

            if (userLanguage == "fr")
                selectedLanguage = "fr";
            else
                selectedLanguage = "en";

            var path = String.Format("Data/Translations/translation.{0}", selectedLanguage);
            MazeLang.Text = ContentHelper.LoadXMLFromXna<GameText>(path);
        }

        private void InitializeScreenStates()
        {
            splashState = new SplashState("splash");
            menuState = new MenuState("menu");
            selectionState = new SelectionState("selection");
            optionsState = new OptionsState("options");
            aboutState = new AboutState("about");

            popupState = new PopupState("popup");
            popupState.ActionMenu += popupState_Action;
            popupState.ActionNext += popupState_Action;
            popupState.StateManager = YnG.StateManager;
            popupState.LoadContent();
            popupState.Initialize();
            popupState.Active = false;
        }

        private void popupState_Action(object sender, MessageBoxEventArgs e)
        {
            PopupState popupState = sender as PopupState;
            LevelState level = stateManager.Get("level") as LevelState;

            level.Active = false;

            if (e.CancelAction)
            {
                NextLevel--;
                popupState.Active = false;
                stateManager.SetActive("menu", true);
            }
            else
            {
                if (popupState.MessageBoxLabelB == MazeLang.Text.Messages.NewPlusAction)
                    GameConfiguration.SetNextDifficulty();

                popupState.ShowWaitMessage();
                popupState.Enabled = false;
                Thread prepareLevelThread = new Thread(new ParameterizedThreadStart((o) =>
                {
                    PrepareNewLevel(_nextLevel, true);
                    popupState.HideWaitMessage();
                    popupState.Active = false;
                }));

                prepareLevelThread.Start();
            }
        }

        public void PrepareNewLevel(int levelId, bool isStarted)
        {
            LevelState level = stateManager.Get("level") as LevelState;

            int launchLevelId = GameConfiguration.LevelStart;

            if (level != null)
            {
                stateManager.Remove(level);

                level.ExitRequest -= level_ExitRequest;
                level.LevelFinished -= level_LevelFinished;
                level = null;
                launchLevelId = levelId;
            }

            level = new LevelState("level", launchLevelId);
            level.ExitRequest += new EventHandler<EventArgs>(level_ExitRequest);
            level.LevelFinished += new EventHandler<LevelFinishEventArgs>(level_LevelFinished);

            stateManager.Add(level, isStarted);
            stateManager.SetActive("level", true);
        }

        private void level_LevelFinished(object sender, LevelFinishEventArgs e)
        {
            YnG.AudioManager.StopMusic();

            int score = e.Score.PartyScore;
            string wPoint = score > 0 ? score + " points" : "0 point";

            if (e.GameFinished)
            {
                popupState.SetMessage(MazeLang.Text.Messages.GameFinished.Label, String.Format(MazeLang.Text.Messages.GameFinished.Content, (e.NextLevel - 1).ToString(), wPoint));
                popupState.SetActions(MazeLang.Text.Messages.MenuAction, MazeLang.Text.Messages.NewPlusAction);
                NextLevel = 1;

                // Le jouer peut jouer à tous les niveaux
                GameConfiguration.LevelsUnlocked = GameConfiguration.LevelCount;
            }
            else
            {
                popupState.SetMessage(MazeLang.Text.Messages.LevelFinished.Label, String.Format(MazeLang.Text.Messages.LevelFinished.Content, (e.NextLevel - 1).ToString(), wPoint));
                popupState.SetActions(MazeLang.Text.Messages.MenuAction, MazeLang.Text.Messages.NextAction);
                NextLevel = e.NextLevel;

                // Le niveau que le joueur vient de terminer est disponible dans le menu de selection
                GameConfiguration.LevelsUnlocked = NextLevel;
            }

            _gameFinished = e.GameFinished;
            popupState.Active = true;

            _playerManager.AddScore(e.Score);
            _playerManager.Save();
        }

        private void level_ExitRequest(object sender, EventArgs e)
        {
            YnG.AudioManager.StopMusic();
            stateManager.SetActive("menu", true);
            popupState.Active = false;
        }

        protected override void Initialize()
        {
            _playerManager.Load();
            _nextLevel = GameConfiguration.LevelStart;
            InitializeScreenStates();
            stateManager.Add(splashState, true);
            stateManager.Add(menuState, false);
            stateManager.Add(selectionState, false);
            stateManager.Add(optionsState, false);
            stateManager.Add(aboutState, false);
            base.Initialize();
        }

        protected override void UnloadContent()
        {
            _playerManager.Save();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (popupState.Enabled)
                popupState.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (popupState.Visible)
                popupState.Draw(gameTime);
        }
    }
}
