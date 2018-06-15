﻿using Maze3D.Control;
using Maze3D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Helpers;

namespace Maze3D.Screen
{
    public enum MazeGameState
    {
        Playing = 0, Ending, Waiting, Terminated
    }

    public class LevelState : YnState3D
    {
        private YnTimer soundTimer;
        private long elapsedPartyTime;
        private MazeGameState gameState;
        private Score score;
        private BoundingSphere groundPlayerBoundingSphere;
        private MazeLevel _mazeLevel;
        private GameHUD _gameHUD;
        private MazeController _control;
        private VirtualPad _virtualPad;

        public event Action<Score, int, bool> LevelFinished = null;
        public event Action ExitRequest = null;

        public LevelState(string name, int startLevel)
            : base(name)
        {
            Camera = new FirstPersonCamera();
            Camera.BoundingRadius = 2.0f;
            Camera.UpdateBoundingVolumes();

            var settings = GameSettings.Instance;

            YnG.ShowMouse = (settings.EnabledMouse ? true : false);
            YnG.AudioManager.SoundEnabled = settings.EnabledSound;
            YnG.AudioManager.MusicVolume = settings.MusicVolume;

            groundPlayerBoundingSphere = new BoundingSphere(Camera.Position, Camera.BoundingRadius);

            _mazeLevel = new MazeLevel(startLevel);
            Add(_mazeLevel);

            _gameHUD = new GameHUD();

            score = new Score(startLevel);

            gameState = MazeGameState.Playing;
            elapsedPartyTime = 0;

            soundTimer = new YnTimer(1000, 0);

            if (YnG.AudioManager.SoundEnabled)
                soundTimer.Completed += (s, e) => YnG.AudioManager.SoundEnabled = true;
            else
                soundTimer.Completed += (s, e) => { };

            _control = new MazeController(Camera);

            SceneLight.AmbientIntensity = 1.0f;
            SceneLight.AmbientColor = new Vector3(0.6f);
            SceneLight.DirectionalLights[0].Enabled = true;
            SceneLight.DirectionalLights[0].Direction = new Vector3(-1, 1, 0);
            SceneLight.DirectionalLights[0].DiffuseColor = Color.Wheat.ToVector3();
            SceneLight.DirectionalLights[0].SpecularColor = Color.Silver.ToVector3();

            SetFog(true, Color.LightGray, 25, 250);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Camera.Position = _mazeLevel.StartPosition;

            _gameHUD.LoadContent();
            _gameHUD.Initialize();
            _gameHUD.InitializeMinimap(_mazeLevel);

            _virtualPad = new VirtualPad();
            _virtualPad.LoadContent();

            var vpZoomValue = 1.0f;
            var settings = GameSettings.Instance;

            switch (settings.VirtualPadSize)
            {
                case VirtualPadSize.Small: vpZoomValue = 0.9f; break;
                case VirtualPadSize.Normal: vpZoomValue = 1.2f; break;
                case VirtualPadSize.Big: vpZoomValue = 1.7f; break;
            }

            vpZoomValue *= ScreenHelper.GetScale().X;
            _virtualPad.UpdateScale(vpZoomValue);
            _virtualPad.Position = new Vector2(YnG.Width - _virtualPad.Width * vpZoomValue - 10, YnG.Height - _virtualPad.Height * vpZoomValue - 10);
            _virtualPad.UpdateLayoutPosition();

            if (settings.ControlMode == ControlMode.New)
                _virtualPad.Pressed += (d) => _control.SetControlDirection(d);
            else
                _virtualPad.JustPressed += (d) => _control.SetControlDirection(d);

            _virtualPad.Active = settings.EnabledVirtualPad;

            if (settings.EnabledMusic)
                YnG.AudioManager.PlayMusic("Audio/Lost_in_dark_way", true);
        }

        private void UpdateScore(int addScore)
        {
            score.GameScore += addScore;
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

                _gameHUD.Update(gameTime);

                var time = TimeSpan.FromMilliseconds(elapsedPartyTime);

                _gameHUD.Time = $"{time.Minutes} : {time.Seconds}";

                _control.Update(gameTime);
#if DEBUG
                if (YnG.Keys.JustPressed(Keys.F8))
                    OnLevelFinished(score, (_mazeLevel.Id + 1), false);
#endif
                string collider = _control.ValidatePosition(_mazeLevel.Walls);

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

                foreach (YnEntity3D model in _mazeLevel.Items)
                {
                    if (model.Active)
                    {
                        if (groundPlayerBoundingSphere.Intersects(model.BoundingSphere))
                        {
                            YnG.AudioManager.PlaySound("Audio/kristal", 1.0f, 1.0f, 0.0f);
                            UpdateScore((model as AnimatedItemMesh).Points);
                            _gameHUD.MiniMap.UpdateItemStatus((float)Camera.X, (float)Camera.Z);
                            _gameHUD.UpdateNbCrystals();
                            model.Active = false;
                        }
                    }
                }

                if (_gameHUD.MiniMap.Enabled)
                    _gameHUD.MiniMap.UpdatePlayerPosition((float)Camera.X, (float)Camera.Z);

                if (_virtualPad.Enabled)
                    _virtualPad.Update(gameTime);
            }
            else if (gameState == MazeGameState.Ending)
            {
                gameState = MazeGameState.Terminated;

                var finishedGame = false;
                var nextLevel = (_mazeLevel.Id + 1);

                if (nextLevel > GameSettings.LevelCount)
                {
                    nextLevel = 1;
                    finishedGame = true;
                }

                score.ElapsedTime = elapsedPartyTime;

                OnLevelFinished(score, nextLevel, finishedGame);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            YnG.RestoreGraphicsDeviceStates();

            base.Draw(gameTime);

            spriteBatch.Begin();

            _gameHUD.Draw(gameTime, spriteBatch);

            if (_virtualPad.Visible)
                _virtualPad.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void OnLevelFinished(Score score, int nextLevel, bool gameFinished) => LevelFinished?.Invoke(score, nextLevel, gameFinished);

        public void OnExitRequest(EventArgs e) => ExitRequest?.Invoke();
    }
}
