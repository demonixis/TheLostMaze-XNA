// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Scene
{
    /// <summary>
    /// Represent an abstract scene with a collection of basic objects like timers and nothing else
    /// </summary>
    public class YnScene : YnGameEntity
    {
        protected List<Engine.YnEntity> _baseList;
        protected YnGameEntityCollection _entities;

        /// <summary>
        /// Gets or sets basic objects
        /// </summary>
        public List<Engine.YnEntity> BaseObjects
        {
            get { return _baseList; }
        }

        public List<YnGameEntity> Entities
        {
            get { return _entities.Members; }
        }


        public YnScene()
        {
            _baseList = new List<Engine.YnEntity>();
            _entities = new YnGameEntityCollection();
        }

        public override void Initialize()
        {
            _entities.Initialize();
            _initialized = true;
        }

        public override void LoadContent()
        {
            _entities.LoadContent();
            _assetLoaded = true;
        }

        public override void UnloadContent()
        {
            _baseList.Clear();
            _entities.UnloadContent();
            _entities.Clear();
            _assetLoaded = false;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var basic in _baseList)
                basic.Update(gameTime);

            _entities.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _entities.Draw(gameTime, spriteBatch);
        }

        #region Collection methods

        /// <summary>
        /// Add a basic object
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public virtual void Add(Engine.YnEntity basicObject)
        {
            _baseList.Add(basicObject);
        }

        public virtual void Add(YnGameEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Remove a basic object
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public virtual void Remove(Engine.YnEntity basicObject)
        {
            _baseList.Remove(basicObject);
        }

        public virtual void Remove(YnEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual void Clear()
        {
            _baseList.Clear();
            _entities.Clear();
        }

        /// <summary>
        /// Gets an YnBase object by its name
        /// </summary>
        /// <param name="name">Name of the object</param>
        /// <returns>An YnBase object or null if don't exists</returns>
        public virtual YnEntity GetMemberByName(string name)
        {
            foreach (YnEntity entity in _baseList)
                if (entity.Name == name)
                    return entity;

            foreach (YnEntity entity in _entities)
                if (entity.Name == name)
                    return entity;

            return null;
        }

        #endregion
    }
}
