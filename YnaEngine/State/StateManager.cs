// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Yna.Engine.State
{
    /// <summary>
    /// The StateManager is responsible off managing the various screens that composes the game.
    /// A state represents a game screen as a menu, a scene or a score screen. 
    /// The state manager can add, delete, and work with registered states.
    /// </summary>
    public class StateManager : DrawableGameComponent
    {
        #region Private declarations

        private List<YnState> _scenes;
        private Dictionary<string, int> _statesDictionary;

        private bool _initialized;
        private bool _assetLoaded;
        private SpriteBatch _spriteBatch;
        private Color _clearColor;

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the color used to clear the screen before each frame
        /// </summary>
        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
        }

        /// <summary>
        /// Get the SpriteBatch
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        /// <summary>
        /// Get the screen at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public YnState this[int index]
        {
            get
            {
                if (index < 0 || index > _scenes.Count - 1)
                    return null;
                else
                    return _scenes[index] as YnState;
            }
            set
            {
                if (index < 0 || index > _scenes.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _scenes[index] = value;
            }
        }

        #endregion

        #region Constructor

        public StateManager(Game game)
            : base(game)
        {
            _clearColor = Color.Black;

            _scenes = new List<YnState>();
            _statesDictionary = new Dictionary<string, int>();

            _initialized = false;
            _assetLoaded = false;
        }

        #endregion

        #region GameState pattern

        public override void Initialize()
        {
            base.Initialize();

            if (!_initialized)
            {
                foreach (YnState screen in _scenes)
                    screen.Initialize();

                _initialized = true;
            }
        }

        protected override void LoadContent()
        {
            if (!_assetLoaded)
            {
                int nbScreens = _scenes.Count;

                _spriteBatch = new SpriteBatch(GraphicsDevice);

                foreach (YnState screen in _scenes)
                    screen.LoadContent();

                _assetLoaded = true;
            }
        }

        protected override void UnloadContent()
        {
            if (_assetLoaded && _scenes.Count > 0)
            {
                foreach (YnState screen in _scenes)
                    screen.UnloadContent();

                _assetLoaded = false;
            }
        }

        /// <summary>
        /// Update logic of enabled states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            foreach (var scene in _scenes)
                if (scene.Enabled)
                    scene.Update(gameTime);
        }

        /// <summary>
        /// Draw visible states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            GraphicsDevice.Clear(_clearColor);

            foreach (var scene in _scenes)
                if (scene.Enabled)
                    scene.Draw(gameTime);
        }

        #endregion

        #region State management methods

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="name">State name</param>
        /// <returns>State index</returns>
        public int IndexOf(string name)
        {
            var state = Get(name);

            if (state != null)
                return _scenes.IndexOf(state);

            return -1;
        }

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="scene">State</param>
        /// <returns>State index</returns>
        public int IndexOf(YnState scene) => _scenes.IndexOf(scene);

        /// <summary>
        /// Replace a state by another state
        /// </summary>
        /// <param name="oldState">Old state in the collection</param>
        /// <param name="newState">New state</param>
        /// <returns>True if for success then false</returns>
        public bool Replace(YnState oldState, YnState newState)
        {
            var index = _scenes.IndexOf(oldState);

            if (index < 0)
                return false;

            newState.StateManager = this;
            _scenes[index] = newState;

            if (_initialized && !newState.Initialized)
                newState.Initialize();

            if (_assetLoaded && !newState.AssetLoaded)
                newState.LoadContent();

            return true;
        }


        /// <summary>
        /// Active a screen and desactive other screens if needed.
        /// </summary>
        /// <param name="index">Index of the screen in the collection</param>
        /// <param name="desactiveOtherStates">Desactive or not others screens</param>
        public void SetActive(int index, bool desactiveOtherStates)
        {
            int size = _scenes.Count;

            if (index < 0 || index > size - 1)
                throw new IndexOutOfRangeException("[ScreenManager] The screen doesn't exist at this index");

            _scenes[index].Active = true;

            if (desactiveOtherStates)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i != index)
                        _scenes[i].Active = false;
                }
            }
        }

        public void SetActive(string name, bool desactiveOtherScreens)
        {
            if (!_statesDictionary.ContainsKey(name))
                throw new Exception("This screen name doesn't exists");

            var activableState = _scenes[_statesDictionary[name]];
            activableState.Active = true;

            if (desactiveOtherScreens)
            {
                foreach (YnState screen in _scenes)
                    if (activableState != screen)
                        screen.Active = false;
            }
        }

        /// <summary>
        /// Gets a state by its name
        /// </summary>
        /// <param name="name">The name used by the state</param>
        /// <returns>The state if exists otherwise return null</returns>
        public YnState Get(string name)
        {
            if (_statesDictionary.ContainsKey(name))
                return _scenes[_statesDictionary[name]];

            return null;
        }

        /// <summary>
        /// Update internal mapping with de Dictionary and the State collection
        /// </summary>
        protected void UpdateDictionaryStates()
        {
            _statesDictionary.Clear();

            foreach (var screen in _scenes)
            {
                if (_statesDictionary.ContainsKey(screen.Name))
                    throw new Exception("Two screens can't have the same name, it's forbiden and it's bad :(");

                _statesDictionary.Add(screen.Name, _scenes.IndexOf(screen));
            }
        }

        /// <summary>
        /// Sets all state in pause by desabling them.
        /// </summary>
        public void PauseAllStates()
        {
            foreach (var state in _scenes)
                state.Active = false;
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a new state to the manager. The screen is not activated or desactivated, you must manage it yourself
        /// </summary>
        /// <param name="state">Screen to add</param>
        public void Add(YnState state)
        {
            state.StateManager = this;

            if (_initialized)
                state.Initialize();

            if (_assetLoaded)
                state.LoadContent();

            _scenes.Add(state);
            _statesDictionary.Add(state.Name, _scenes.IndexOf(state));
        }

        /// <summary>
        /// Add a state to the manager and active or desactive it.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isActive"></param>
        public void Add(YnState state, bool isActive)
        {
            if (state.Active != isActive)
            {
                state.Enabled = isActive;
                state.Visible = isActive;
            }

            Add(state);
        }

        /// <summary>
        /// Remove a screen to the Manager
        /// </summary>
        /// <param name="state">Screen to remove</param>
        public void Remove(YnState state)
        {
            _scenes.Remove(state);
            _statesDictionary.Remove(state.Name);
        }

        /// <summary>
        /// Clear all the Screens in the Manager
        /// </summary>
        public void Clear()
        {
            if (_scenes.Count > 0)
            {
                for (int i = _scenes.Count - 1; i >= 0; i--)
                    _scenes[i].Active = false;

                _scenes.Clear();
                _statesDictionary.Clear();
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnState screen in _scenes)
                yield return screen;
        }

        #endregion
    }
}
