// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Yna.Engine.Graphics.Gui;
using Yna.Engine.Graphics.Gui.Widgets;
using Yna.Engine.State;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// This is a State object that contains the scene.
    /// That allows you to add different types of objects.
    /// Timers, basic objects (which have an update method) and entities
    /// </summary>
    public class YnState2D : YnState
    {
        protected List<YnEntity> _baseList;
        protected List<YnGameEntity> _entities;
        protected YnGui _guiManager;
        protected YnCamera2D _camera;

        /// <summary>
        /// Gets or sets the gui
        /// </summary>
        public YnGui Gui
        {
            get { return _guiManager; }
            protected set { _guiManager = value; }
        }

        #region Properties

        /// <summary>
        /// Gets basic objects
        /// </summary>
        public List<YnEntity> BaseObjects => _baseList;

        /// <summary>
        /// Gets members attached to the scene
        /// </summary>
        public List<YnGameEntity> GameEntities => _entities;

        /// <summary>
        /// Gets or sets the spriteBatchCamera used for add effect on the scene like 
        /// displacement, rotation and zoom
        /// </summary>
        public YnCamera2D Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a 2D state.
        /// </summary>
        /// <param name="name">The state name</param>
        /// <param name="active">Set to true to activate the state</param>
        /// <param name="enableGui">Set to true tu enable GUI on this state</param>
        public YnState2D(string name, bool active, bool enableGui)
            : base(name)
        {
            _enabled = active;
            _visible = active;
            _camera = new YnCamera2D();
            _baseList = new List<YnEntity>();
            _entities = new List<YnGameEntity>();
            _guiManager = new YnGui();
        }

        /// <summary>
        /// Create a 2D state without GUI.
        /// </summary>
        /// <param name="name">The state name</param>
        /// <param name="active">Set to true to activate the state</param>
        public YnState2D(string name, bool active)
            : this(name, active, false)
        {
        }

        /// <summary>
        ///  Create a 2D state without GUI.
        /// </summary>
        /// <param name="name">The state name</param>
        public YnState2D(string name)
            : this(name, true, false)
        {
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Initialize the state
        /// </summary>
        public override void Initialize()
        {
            if (_initialized)
                return;

            foreach (var entity in _entities)
                entity.Initialize();

            _guiManager.Initialize();
        }

        /// <summary>
        /// Load content
        /// </summary>
        public override void LoadContent()
        {
            if (_assetLoaded)
                return;

            base.LoadContent();

            foreach (var entity in _entities)
                entity.LoadContent();

            _guiManager.LoadContent();

            _assetLoaded = true;
        }

        /// <summary>
        /// Unload content
        /// </summary>
        public override void UnloadContent()
        {
            if (!_assetLoaded)
                return;

            _baseList.Clear();

            foreach (var entity in _entities)
                entity.UnloadContent();

            _guiManager.UnloadContent();

            _entities.Clear();
        }

        /// <summary>
        /// Update the camera and the scene who will update BasicObjects, Entities and Gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);

            foreach (var entity in _baseList)
                entity.Update(gameTime);

            foreach (var entity in _entities)
                entity.Update(gameTime);
        }

        /// <summary>
        /// Draw all entities and the gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetTransformMatrix());
                entity.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }

            _guiManager.Draw(gameTime, spriteBatch);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Add(YnEntity basicObject) => _baseList.Add(basicObject);


        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="entity">An entitiy</param>
        public void Add(YnEntity2D entity)
        {
            if (_entities.Contains(entity))
                return;

            if (Initialized && !entity.Initialized)
                entity.Initialize();

            if (AssetLoaded && !entity.AssetLoaded)
                entity.LoadContent();

            _entities.Add(entity);
        }

        public void Add(YnWidget widget) => _guiManager.Add(widget);

        public void Remove(YnWidget widget) => _guiManager.Remove(widget);

        /// <summary>
        /// Remove a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Remove(YnEntity basicObject) => _baseList.Remove(basicObject);

        /// <summary>
        /// Remove an entity to the scene
        /// </summary>
        /// <param name="entity">An entitiy</param>
        public void Remove(YnEntity2D entity) => _entities.Remove(entity);

        public YnEntity2D GetMemberByName(string name)
        {
            foreach (YnEntity2D entity in _baseList)
                if (entity.Name == name)
                    return entity;

            foreach (YnEntity2D entity in _entities)
                if (entity.Name == name)
                    return entity;

            return null;
        }

        #endregion
    }
}
