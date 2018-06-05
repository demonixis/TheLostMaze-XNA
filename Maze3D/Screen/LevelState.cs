using Maze3D.Control;
using Maze3D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Helpers;

namespace Maze3D.Screen
{
    public enum MazeGameState
    {
        Playing = 0, Ending, Waiting, Terminated
    }

    public class LevelState : YnState3D
    {
        private MazeTimer timeTimer;
        private YnTimer soundTimer;
        private long elapsedPartyTime;

        private MazeGameState gameState;
        private Score score;

        private BoundingSphere groundPlayerBoundingSphere;

        private MazeLevel _mazeLevel;
        private GameHUD _gameHUD;

        private MazeController control;
        private VirtualPad virtualPad;

        public event EventHandler<LevelFinishEventArgs> LevelFinished = null;
        public event EventHandler<EventArgs> ExitRequest = null;

        private void OnLevelFinished(LevelFinishEventArgs e) => LevelFinished?.Invoke(this, e);

        public void OnExitRequest(EventArgs e) => ExitRequest?.Invoke(this, e);

        public LevelState(string name, int startLevel)
            : base(name)
        {
            Camera = new FirstPersonCamera();
            Camera.BoundingRadius = 2.0f;
            Camera.UpdateBoundingVolumes();

            YnG.ShowMouse = (GameConfiguration.EnabledMouse ? true : false);
            YnG.AudioManager.SoundEnabled = GameConfiguration.EnabledSound;
            YnG.AudioManager.MusicVolume = GameConfiguration.MusicVolume;

            groundPlayerBoundingSphere = new BoundingSphere(Camera.Position, Camera.BoundingRadius);

            _mazeLevel = new MazeLevel(startLevel);
            Add(_mazeLevel);

            _gameHUD = new GameHUD();

            timeTimer = new MazeTimer();

            score = new Score(startLevel);

            gameState = MazeGameState.Playing;
            elapsedPartyTime = 0;

            soundTimer = new YnTimer(1000, 0);

            if (YnG.AudioManager.SoundEnabled)
                soundTimer.Completed += (s, e) => YnG.AudioManager.SoundEnabled = true;
            else
                soundTimer.Completed += (s, e) => { };

            control = new MazeController(Camera);

            SceneLight.AmbientIntensity = 0.85f;
            SceneLight.DirectionalLights[0].Enabled = true;
            SceneLight.DirectionalLights[0].Direction = new Vector3(-1, 0.75f, -1);
            SceneLight.DirectionalLights[0].DiffuseColor = Color.WhiteSmoke.ToVector3();
            SceneLight.DirectionalLights[0].DiffuseIntensity = 1.0f;
            SceneLight.DirectionalLights[0].SpecularColor = new Vector3(233, 33, 33);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Camera.Position = _mazeLevel.StartPosition;

            _gameHUD.LoadContent();
            _gameHUD.Initialize();
            _gameHUD.InitializeMinimap(_mazeLevel);

            virtualPad = new VirtualPad();
            virtualPad.LoadContent();

            float vpZoomValue = 1.0f;

            switch (GameConfiguration.VirtualPadSize)
            {
                case VirtualPadSize.Small: vpZoomValue = 0.9f; break;
                case VirtualPadSize.Normal: vpZoomValue = 1.2f; break;
                case VirtualPadSize.Big: vpZoomValue = 1.7f; break;
            }

            vpZoomValue *= ScreenHelper.GetScale().X;
            virtualPad.UpdateScale(vpZoomValue);
            virtualPad.Position = new Vector2(YnG.Width - virtualPad.Width * vpZoomValue - 10, YnG.Height - virtualPad.Height * vpZoomValue - 10);
            virtualPad.UpdateLayoutPosition();

            if (GameConfiguration.ControlMode == ControlMode.New)
                virtualPad.Pressed += (s, e) => control.SetControlDirection(e.Direction);
            else
                virtualPad.JustPressed += (s, e) => control.SetControlDirection(e.Direction);

            virtualPad.Active = GameConfiguration.EnabledVirtualPad;

            if (GameConfiguration.EnabledMusic)
                YnG.AudioManager.PlayMusic("Audio/Lost_in_dark_way", true);
        }

        private void UpdateScore(int addScore)
        {
            score.PartyScore += addScore;
            _gameHUD.Score = score.GetPartyScore();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameState == MazeGameState.Playing)
            {
                if (YnG.Keys.JustPressed(Keys.Escape) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.Back))
                {
                    gameState = MazeGameState.Terminated;
                    OnExitRequest(EventArgs.Empty);
                }

                soundTimer.Update(gameTime);

                elapsedPartyTime += gameTime.ElapsedGameTime.Milliseconds;

                timeTimer.Update(gameTime.ElapsedGameTime.Milliseconds);

                _gameHUD.Update(gameTime);
                _gameHUD.Time = timeTimer.ToString();

                control.Update(gameTime);
#if DEBUG
                if (YnG.Keys.JustPressed(Keys.F8))
                    OnLevelFinished(new LevelFinishEventArgs(score, (_mazeLevel.Id + 1), false));
#endif
                string collider = control.ValidatePosition(_mazeLevel.Walls);

                if (collider == "END")
                {
                    gameState = MazeGameState.Ending;
                    UpdateScore(50);
                }
                else if (collider == "WALL")
                {
                    if (YnG.AudioManager.SoundEnabled)
                    {
                        YnG.AudioManager.PlaySound("Audio/bat", 1.0f, 1.0f, 0.0f);
                        YnG.AudioManager.SoundEnabled = false;
                        soundTimer.Start();
                    }
                }

                groundPlayerBoundingSphere.Center = new Vector3(Camera.Position.X, 1, Camera.Position.Z);

                foreach (YnMeshModel model in _mazeLevel.Items)
                {
                    if (model.Active)
                    {
                        if (groundPlayerBoundingSphere.Intersects(model.BoundingSphere))
                        {
                            YnG.AudioManager.PlaySound("Audio/kristal", 1.0f, 1.0f, 0.0f);
                            UpdateScore((model as AnimatedItemMesh).Points);
                            _gameHUD.MiniMap.UpdateItemStatus(Camera.X, Camera.Z);
                            _gameHUD.UpdateNbCrystals();
                            model.Active = false;
                        }
                    }
                }

                if (_gameHUD.MiniMap.Enabled)
                    _gameHUD.MiniMap.UpdatePlayerPosition(Camera.X, Camera.Z);

                if (virtualPad.Enabled)
                    virtualPad.Update(gameTime);
            }
            else if (gameState == MazeGameState.Ending)
            {
                gameState = MazeGameState.Terminated;

                bool finishedGame = false;

                int nextLevel = (_mazeLevel.Id + 1);

                if (nextLevel > GameConfiguration.LevelCount)
                {
                    nextLevel = 1;
                    finishedGame = true;
                }

                score.ElapsedTime = elapsedPartyTime;

                OnLevelFinished(new LevelFinishEventArgs(score, nextLevel, finishedGame));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            YnG3.RestoreGraphicsDeviceStates();

            base.Draw(gameTime);

            spriteBatch.Begin();

            _gameHUD.Draw(gameTime, spriteBatch);

            if (virtualPad.Visible)
                virtualPad.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
