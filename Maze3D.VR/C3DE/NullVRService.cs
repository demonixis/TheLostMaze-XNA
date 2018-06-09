﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace C3DE.VR
{
    /// <summary>
    /// A service used to simulate the VR view.
    /// </summary>
    public class NullVRService : VRService
    {
        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private Vector3 _headRotation;
        private Vector2 _delta;
        private float _rotationSpeed;

        public Matrix ViewMatrix { get; set; } = Matrix.Identity;
        public Matrix ProjectionMatrix { get; set; } = Matrix.Identity;

        public NullVRService(Game game, float rotationSpeed = 0.25f)
            : base(game)
        {
            _rotationSpeed = rotationSpeed;

            var pp = Game.GraphicsDevice.PresentationParameters;
            ProjectionMatrix = Matrix.CreatePerspective(pp.BackBufferWidth, pp.BackBufferHeight, 0.1f, 1000.0f);
        }

        public override int TryInitialize()
        {
            //DistortionEffect = Game.Content.Load<Effect>("Shaders/PostProcessing/OsvrDistortion");
            //DistortionCorrectionRequired = true;
            Game.Components.Add(this);
            return 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _mouseState = Mouse.GetState();

            _delta.X = _mouseState.X - _previousMouseState.X;
            _delta.Y = _mouseState.Y - _previousMouseState.Y;

            // Rotation.
            _headRotation.X -= _delta.Y * gameTime.ElapsedGameTime.Milliseconds * _rotationSpeed;
            _headRotation.Y -= _delta.X * gameTime.ElapsedGameTime.Milliseconds * _rotationSpeed;

            _previousMouseState = _mouseState;
        }

        public override RenderTarget2D CreateRenderTargetForEye(int eye)
        {
            var pp = Game.GraphicsDevice.PresentationParameters;
            return new RenderTarget2D(Game.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }

        public override Matrix GetProjectionMatrix(int eye) => ProjectionMatrix;

        public override Matrix GetViewMatrix(int eye, Matrix playerPose)
        {
            var rotationMatrix = Matrix.CreateFromYawPitchRoll(_headRotation.Y, _headRotation.X, 0.0f);
            var target = Vector3.Transform(Vector3.Forward, rotationMatrix);
            return Matrix.CreateLookAt(Vector3.Zero, target, Vector3.Up);
        }

        public override float GetRenderTargetAspectRatio(int eye) => 1.0f;

        public override int SubmitRenderTargets(RenderTarget2D renderTargetLeft, RenderTarget2D renderTargetRight) => 0;

        public override void ApplyDistortion(RenderTarget2D renderTarget, int eye)
        {
            /* DistortionEffect.Parameters["TargetTexture"].SetValue(renderTarget);
             DistortionEffect.Parameters["K1_Red"].SetValue(1f);
             DistortionEffect.Parameters["K1_Green"].SetValue(1f);
             DistortionEffect.Parameters["K1_Blue"].SetValue(1f);
             DistortionEffect.Parameters["Center"].SetValue(new Vector2(0.5f, 0.5f));
             DistortionEffect.Techniques[0].Passes[0].Apply();*/
        }

        public override uint[] GetRenderTargetSize()
        {
            var pp = Game.GraphicsDevice.PresentationParameters;
            return new uint[] { (uint)pp.BackBufferWidth, (uint)pp.BackBufferHeight };
        }
    }
}