// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
#define DEBUG_VR
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine.State;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Lighting;
using Microsoft.Xna.Framework.Graphics;
using C3DE.VR;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A 3D state who contains a camera manager, a scene manager and a collection of basic objects (timers, controllers, etc...)
    /// </summary>
    public class YnState3D : YnState
    {
        protected SceneLight _sceneLight;
        private Camera _camera;
        private YnGroup3D _scene;
        private List<YnEntity> _basicObjects;
        private VRService _vrService;
        private bool _vrEnabled;
        private RenderTarget2D[] _sceneRenderTargets;

#if DEBUG_VR
        private NullVRService _NullVR;
#endif

        /// <summary>
        /// Gets (protected sets) the collection of basic objects.
        /// </summary>
        public List<YnEntity> BasicObjects
        {
            get { return _basicObjects; }
            protected set { _basicObjects = value; }
        }

        /// <summary>
        /// Gets (protected sets) the scene.
        /// </summary>
        public YnGroup3D Scene
        {
            get { return _scene; }
            protected set { _scene = value; }
        }

        /// <summary>
        /// Gets (protected sets) the active camera.
        /// </summary>
        public Camera Camera
        {
            get { return _camera; }
            protected set { _camera = value; }
        }

        /// <summary>
        /// Gets or sets the basic light of the scene.
        /// </summary>
        public SceneLight SceneLight
        {
            get { return _sceneLight; }
            set { _sceneLight = value; }
        }

        #region Constructors

        /// <summary>
        /// Create a state with a 3D scene and a camera.
        /// </summary>
        /// <param name="camera">Camera to use on this scene.</param>
        public YnState3D(Camera camera = null)
            : base()
        {
            _camera = camera != null ? new Camera() : camera;
            _scene = new YnGroup3D();
            _basicObjects = new List<YnEntity>();
            _sceneLight = new SceneLight();
            _sceneLight.AmbientIntensity = 1f;

#if DEBUG_VR
            _NullVR = new NullVRService(YnG.Game, 1);
            var driver = new VRDriver(_NullVR, true, 0);
            VRManager.AvailableDrivers.Add(driver);
#endif
            _vrService = VRManager.GetVRAvailableVRService(YnG.Game);
            _vrEnabled = _vrService != null;

            if (_vrEnabled)
            {
                var size = _vrService.GetRenderTargetSize();

                _sceneRenderTargets = new RenderTarget2D[2];

                for (var i = 0; i < 2; i++)
                    _sceneRenderTargets[i] = new RenderTarget2D(YnG.GraphicsDevice, (int)size[0], (int)size[1], false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 2, RenderTargetUsage.DiscardContents);
            }
        }

        /// <summary>
        /// Create a state with a 3D scene and a camera.
        /// </summary>
        /// <param name="name">State name.</param>
        /// <param name="camera">Camera to use on this scene.</param>
        public YnState3D(string name, Camera camera)
            : this(camera) => _name = name;

        /// <summary>
        /// Create a state with a 3D scene and a fixed camera.
        /// </summary>
        /// <param name="name">State name.</param>
        public YnState3D(string name)
            : this(name, null) { }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Initialize logic of all scene members.
        /// </summary>
        public override void Initialize() => _scene.Initialize();

        /// <summary>
        /// Load content for all scene members.
        /// </summary>
        public override void LoadContent()
        {
            if (_assetLoaded)
                return;

            base.LoadContent();

            _scene.LoadContent();
            _assetLoaded = true;
        }

        /// <summary>
        /// Unload content of all scene members and clear the scene.
        /// </summary>
        public override void UnloadContent()
        {
            if (!_assetLoaded)
                return;

            _scene.UnloadContent();
            _scene.Clear();
            _assetLoaded = false;
        }

        /// <summary>
        /// Update camera manager, basic objects and scene logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);

            foreach (var basic in _basicObjects)
                basic.Update(gameTime);

            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_vrEnabled)
            {
                var graphics = YnG.GraphicsDevice;
                var oldRT = graphics.GetRenderTargets();

#if DEBUG_VR
                if (_NullVR.ViewMatrix == Matrix.Identity)
                {
                    _NullVR.ViewMatrix = _camera.View;
                    _NullVR.ProjectionMatrix = _camera.Projection;
                }
#endif
                for (var i = 0; i < 2; i++)
                {
                    graphics.SetRenderTarget(_sceneRenderTargets[i]);
                    graphics.Clear(Color.Black);

                    _camera.Projection = _vrService.GetProjectionMatrix(i);
                    _camera.View = _vrService.GetViewMatrix(i, Matrix.Identity);
                    _scene.Draw(gameTime, YnG.GraphicsDevice, _camera, _sceneLight);

                    graphics.SetRenderTarget(null);
                }

                graphics.SetRenderTargets(oldRT);

                DrawVRPreview(0, true);
            }
            else
                _scene.Draw(gameTime, YnG.GraphicsDevice, _camera, _sceneLight);
        }

        private void DrawVRPreview(int eye, bool stereo)
        {
            var graphicsDevice = YnG.GraphicsDevice;
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);

            var pp = graphicsDevice.PresentationParameters;
            var height = pp.BackBufferHeight;
            var width = MathHelper.Min(pp.BackBufferWidth, (int)(height * _vrService.GetRenderTargetAspectRatio(eye)));
            var offset = (pp.BackBufferWidth - width) / 2;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, _vrService.DistortionEffect, null);

            if (stereo || _vrService.DistortionCorrectionRequired)
            {
                width = pp.BackBufferWidth / 2;
                spriteBatch.Draw(_sceneRenderTargets[0], new Rectangle(0, 0, width, height), null, Color.White, 0, Vector2.Zero, _vrService.PreviewRenderEffect, 0);
                _vrService.ApplyDistortion(_sceneRenderTargets[0], 0);

                spriteBatch.Draw(_sceneRenderTargets[1], new Rectangle(width, 0, width, height), null, Color.White, 0, Vector2.Zero, _vrService.PreviewRenderEffect, 0);
                _vrService.ApplyDistortion(_sceneRenderTargets[1], 0);
            }
            else
                spriteBatch.Draw(_sceneRenderTargets[eye], new Rectangle(offset, 0, width, height), null, Color.White, 0, Vector2.Zero, _vrService.PreviewRenderEffect, 0);

            spriteBatch.End();
        }

        #endregion

        #region Collection Management

        /// <summary>
        /// Add a basic object to the scene.
        /// </summary>
        /// <param name="basicObject">A basic object like Timer, Camera, etc...</param>
        /// <returns>Return true if the object has been added, otherwise return false.</returns>
        public void Add(YnEntity basicObject) => _basicObjects.Add(basicObject);

        /// <summary>
        /// Add an object3D on the scene
        /// </summary>
        /// <param name="object3D">An object3D</param>
        public bool Add(YnEntity3D object3D) => _scene.Add(object3D);

        /// <summary>
        /// Add a camera to the scene.
        /// </summary>
        /// <param name="camera">Camera to add.</param>
        /// <returns>Return false if the camera is already added otherwise return true.</returns>
        public void Add(Camera camera) => _camera = camera;

        /// <summary>
        /// Remove a basic object to the scene.
        /// </summary>
        /// <param name="basicObject">Basic object to remove.</param>
        /// <returns>Return true if the object has been succefully removed, otherwise return false.</returns>
        public bool Remove(YnEntity basicObject) => _basicObjects.Remove(basicObject);

        /// <summary>
        /// Remove an object3D of the scene
        /// </summary>
        /// <param name="object3D">Object3D to remove.</param>
        public bool Remove(YnEntity3D object3D) => _scene.Remove(object3D);

        /// <summary>
        /// Clear all objects on the state and on the scene
        /// </summary>
        public void Clear()
        {
            _basicObjects.Clear();
            _scene.Clear();
        }

        #endregion
    }
}
