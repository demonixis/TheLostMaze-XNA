// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Audio;
using Yna.Engine.State;
using Yna.Engine.Helpers;
using Yna.Engine.Input;
using Yna.Engine.Storage;

namespace Yna.Engine
{
    /// <summary>
    /// The game class
    /// </summary>
    public class YnGame : Game
    {
        protected GraphicsDeviceManager _graphicsDevice = null;
        protected SpriteBatch m_SpriteBatch = null;
        protected StateManager _stateManager = null;
        public static string GameTitle = "Yna Game";
        public static string GameVersion = "1.0.0.0";

        #region Constructors

        /// <summary>
        /// Create and setup the game engine
        /// Graphics, Services and helpers are initialized
        /// </summary>
        public YnGame()
            : base()
        {
            Content.RootDirectory = "Content";

            _graphicsDevice = new GraphicsDeviceManager(this);
            _stateManager = new StateManager(this);

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDeviceManager = _graphicsDevice;
            YnG.Keys = new YnKeyboard(this); ;
            YnG.Mouse = new YnMouse(this); ;
            YnG.Gamepad = new YnGamepad(this); ;
            YnG.Touch = new YnTouch(this); ;
            YnG.StateManager = _stateManager;
            YnG.StorageManager = new StorageManager(this);
            YnG.AudioManager = new AudioManager();
            ScreenHelper.ScreenWidthReference = _graphicsDevice.PreferredBackBufferWidth;
            ScreenHelper.ScreenHeightReference = _graphicsDevice.PreferredBackBufferHeight;
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Load assets from content manager
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.Viewport = new Viewport(0, 0, _graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            Window.Title = String.Format("{0} - v{1}", GameTitle, GameVersion);
        }

        /// <summary>
        /// Unload assets off content manager and suspend managers
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            YnG.AudioManager.Dispose();
        }

        #endregion

        #region Resolution setup

        /// <summary>
        /// Change the screen resolution
        /// </summary>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public virtual void SetScreenResolution(int width, int height)
        {
            this._graphicsDevice.PreferredBackBufferWidth = width;
            this._graphicsDevice.PreferredBackBufferHeight = height;
            this._graphicsDevice.ApplyChanges();
        }

        /// <summary>
        /// Set maximum resolution supported by the device, It use the desktop resolution
        /// </summary>
        /// <param name="fullscreen">Toggle in fullscreen mode</param>
        public virtual void DetermineBestResolution(bool fullscreen)
        {
            SetScreenResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            
            if (!_graphicsDevice.IsFullScreen && fullscreen)
                _graphicsDevice.ToggleFullScreen();
        }

        #endregion
    }
}

