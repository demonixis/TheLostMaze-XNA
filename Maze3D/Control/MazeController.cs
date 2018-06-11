using Maze3D.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RudderMonoGame;
using System;
using Yna.Engine;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Controls;
using Yna.Engine.Input;

namespace Maze3D.Control
{
    public enum ControlMode
    {
        New = 0, Old
    }

    public enum ControlDirection
    {
        Up = 0, Down, Left, Right, StrafeLeft, StrafeRight, None
    }

    public class MazeController : BaseControl
    {
        private Vector3 _nextDirection;
        private Vector3 _nextPosition;
        private bool EnableVirtualPad;
        private Vector3 _virtualPadDirection;
        private float _vpMoveSpeed;
        private float _vpRotateSpeed;
        private float _vpStrafeSpeed;
        private RudderController _3drudderController;

        // Prevention du garbage collector
        private string _colliderName;
        private BoundingSphere _nppBSphere;
        private Vector3 _validDirection;

#if DEBUG
        public bool Wallthrough { get; set; } = false;
#endif

        public MazeController(Camera camera)
            : base(camera)
        {
            _nextDirection = Vector3.Zero;
            _nextPosition = Camera.Position;
            _virtualPadDirection = Vector3.Zero;

            _colliderName = String.Empty;
            _nppBSphere = camera.BoundingSphere;
            _validDirection = Vector3.Zero;

            EnableGamepad = GameConfiguration.EnabledGamePad;
            EnableMouse = GameConfiguration.EnabledMouse;
            EnableVirtualPad = GameConfiguration.EnabledVirtualPad;

            if (GameConfiguration.ControlMode == ControlMode.New)
            {
                _moveSpeed = 0.05f;
                _rotationSpeed = 0.15f;
                _strafeSpeed = 0.05f;
                _vpMoveSpeed = 1.15f;
                _vpRotateSpeed = 2.55f;
                _vpStrafeSpeed = 0.95f;

                PhysicsPosition.MaxVelocity = 0.95f;
                PhysicsRotation.MaxVelocity = 0.95f;
            }
            else
            {
                _moveSpeed = 15.0f;
                _rotationSpeed = 90.0f;
                _strafeSpeed = 5.0f;

                _vpMoveSpeed = 15.0f;
                _vpRotateSpeed = 90.0f;
                _vpStrafeSpeed = 15.0f;
            }

            _3drudderController = new RudderController();
        }

        public override void Update(GameTime gameTime)
        {
            _nextDirection = Vector3.Zero;
            _nextPosition = Vector3.Zero;

            if (EnableMouse)
                UpdateMouseInput(gameTime);

            if (EnableVirtualPad)
                UpdateVirtualPadInput(gameTime);

            if (GameConfiguration.ControlMode == ControlMode.Old)
            {
                if (EnableGamepad)
                    UpdateOldGamepadInput(gameTime);

                if (EnableKeyboard)
                    UpdateOldKeyboardInput(gameTime);
            }
            else
            {
                if (EnableGamepad)
                    UpdateGamepadInput(gameTime);

                if (EnableKeyboard)
                    UpdateKeyboardInput(gameTime);
            }
#if DEBUG
            if (YnG.Keys.Pressed(Keys.A))
                Camera.Y--;
            else if (YnG.Keys.Pressed(Keys.E))
                Camera.Y++;
#endif

            if (!_3drudderController.Available)
                return;

            var delta = gameTime.ElapsedGameTime.Milliseconds;
            var rotation = Vector3.Zero;
            _3drudderController.UpdateTransform(ref _nextDirection, ref rotation, _moveSpeed * delta, _rotationSpeed * delta, false);
            Camera.RotateY(rotation.Y * _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }

        public bool ValidatePosition(Level level)
        {
            var collide = false;
            var x = Math.Abs((int)Math.Floor((Camera.X / (float)level.BlockSizes.Width / 2)));
            var y = Math.Abs((int)Math.Floor((Camera.Z / (float)level.BlockSizes.Depth / 2)));

            if (x < level.Width && y < level.Depth)
            {
                if (level.Tiles[x + y * level.Width] == 2)
                {
                    collide = true;
                    Camera.Position = Camera.PreviousPosition;
                }
            }

            return collide;
        }

        public string ValidatePosition(YnGroup3D group)
        {
            _colliderName = String.Empty;
            _validDirection.X = 0;
            _validDirection.Y = 0;
            _validDirection.Z = 0;

            ApplyTransformToNextPosition(Camera.Position, Camera.Yaw, ref _nextDirection, ref _nextPosition);

            _nppBSphere.Center = _nextPosition;
            _nppBSphere.Radius = Camera.BoundingRadius;

            int size = group.Count;
            int i = 0;

            while (i < size && _colliderName == String.Empty)
            {
                if (_nppBSphere.Contains(group[i].BoundingBox) != ContainmentType.Disjoint)
                {
                    _colliderName = group[i].Name;
                }
                i++;
            }

            var valid = _colliderName == string.Empty;

#if DEBUG
            if (Wallthrough)
                valid = true;
#endif
            if (valid)
                Camera.Position = _nextPosition;

            return _colliderName;
        }

        private void ApplyTransformToNextPosition(Vector3 position, float rotation, ref Vector3 direction, ref Vector3 nextPositionVector)
        {
            var forwardMovement = Matrix.CreateRotationY(rotation);
            var transformedPosition = Vector3.Add(position, Vector3.Transform(direction, forwardMovement));

            nextPositionVector.X = transformedPosition.X;
            nextPositionVector.Y = transformedPosition.Y;
            nextPositionVector.Z = transformedPosition.Z;
        }

        #region New style

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {
            // Translation Forward/backward
            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                _nextDirection.Z += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                _nextDirection.Z += -_moveSpeed * gameTime.ElapsedGameTime.Milliseconds;

            // Translation Left/Right
            if (YnG.Keys.Pressed(Keys.Q))
                _nextDirection.X += _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds;
            else if (YnG.Keys.Pressed(Keys.D))
                _nextDirection.X += -_strafeSpeed * gameTime.ElapsedGameTime.Milliseconds;

            // Rotation Left/Right
            if (YnG.Keys.Left)
                Camera.RotateY(_rotationSpeed * gameTime.ElapsedGameTime.Milliseconds);
            else if (YnG.Keys.Right)
                Camera.RotateY(-_rotationSpeed * gameTime.ElapsedGameTime.Milliseconds);

            // Look Up/Down
            if (YnG.Keys.Pressed(Keys.PageUp) || YnG.Keys.Pressed(Keys.R))
                Camera.RotateX(-_pitchSpeed * gameTime.ElapsedGameTime.Milliseconds);
            else if (YnG.Keys.Pressed(Keys.PageDown) || YnG.Keys.Pressed(Keys.F))
                Camera.RotateX(_pitchSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            // Translate/Rotate/Picth
            _nextDirection.X += -leftStickValue.X * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            _nextDirection.Z += leftStickValue.Y * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds;

            Camera.RotateY(-rightStickValue.X * _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds);

            if (Camera.Pitch > -1 && Camera.Pitch < 1)
                Camera.RotateX(-rightStickValue.Y * _pitchSpeed * 1.5f * gameTime.ElapsedGameTime.Milliseconds);
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            if (YnG.Mouse.Click(MouseButton.Left))
            {
                Camera.RotateX(-YnG.Mouse.Delta.Y * 0.1f * gameTime.ElapsedGameTime.Milliseconds);
                Camera.RotateY(YnG.Mouse.Delta.X * 0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        #endregion

        #region Old style

        protected void UpdateOldKeyboardInput(GameTime gameTime)
        {
            // Translation Forward/backward
            if (YnG.Keys.JustPressed(Keys.Z) || YnG.Keys.JustPressed(Keys.Up))
                _nextDirection.Z += _moveSpeed;
            else if (YnG.Keys.JustPressed(Keys.S) || YnG.Keys.JustPressed(Keys.Down))
                _nextDirection.Z += -_moveSpeed;

            // Translation Left/Right
            if (YnG.Keys.JustPressed(Keys.Q))
                _nextDirection.X += _strafeSpeed;
            else if (YnG.Keys.JustPressed(Keys.D))
                _nextDirection.X += -_strafeSpeed;

            // Rotation Left/Right
            if (YnG.Keys.JustPressed(Keys.Left))
                Camera.RotateY(_rotationSpeed);
            else if (YnG.Keys.JustPressed(Keys.Right))
                Camera.RotateY(-_rotationSpeed);

            // Look Up/Down
            if (YnG.Keys.Pressed(Keys.PageUp))
                Camera.RotateX(-_pitchSpeed);
            else if (YnG.Keys.Pressed(Keys.PageDown))
                Camera.RotateX(_pitchSpeed);
        }

        protected void UpdateOldGamepadInput(GameTime gameTime)
        {
            if (YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.RightThumbstickUp) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadUp))
                _nextDirection.Z = _moveSpeed;

            else if (YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.RightThumbstickDown) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadDown))
                _nextDirection.Z = -_moveSpeed;

            else if (YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.RightThumbstickLeft) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadLeft))
                _nextDirection.X = _moveSpeed;

            else if (YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.RightThumbstickRight) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadRight))
                _nextDirection.X = -_moveSpeed;

            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            Camera.RotateY(-rightStickValue.X * _rotationSpeed);

            Camera.RotateX(-rightStickValue.Y * _pitchSpeed);
        }

        #endregion

        #region Virtual Pad

        private void UpdateVirtualPadInput(GameTime gameTime)
        {
            _nextDirection += _virtualPadDirection;
            _virtualPadDirection = Vector3.Zero;
        }

        public void SetControlDirection(ControlDirection direction)
        {
            switch (direction)
            {
                case ControlDirection.Up: _virtualPadDirection.Z = _vpMoveSpeed; break;
                case ControlDirection.Down: _virtualPadDirection.Z = -_vpMoveSpeed; break;
                case ControlDirection.StrafeLeft: _virtualPadDirection.X = _vpStrafeSpeed; break;
                case ControlDirection.StrafeRight: _virtualPadDirection.X = -_vpStrafeSpeed; break;
                case ControlDirection.Left: Camera.RotateY(_vpRotateSpeed); break;
                case ControlDirection.Right: Camera.RotateY(-_vpRotateSpeed); break;
                default: break;
            }
        }

        #endregion
    }
}
