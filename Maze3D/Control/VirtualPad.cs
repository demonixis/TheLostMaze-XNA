using Microsoft.Xna.Framework;
using System;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Event;

namespace Maze3D.Control
{
    public enum VirtualPadSize
    {
        Small = 0, Normal, Big
    }

    public enum VirtualPadStyle
    {
        Modern = 0, Old
    }

    public class VirtualPad : YnGroup
    {
        private Vector2 _margin;
        private YnSprite _upPad;
        private YnSprite _downPad;
        private YnSprite _leftPad;
        private YnSprite _rightPad;
        private YnSprite _strafeLeftPad;
        private YnSprite _strafeRightPad;

        public Vector2 Margin
        {
            get { return _margin; }
            set { _margin = value; }
        }

        #region Events

        public event EventHandler<VirtualPadPressedEventArgs> Pressed = null;

        public event EventHandler<VirtualPadPressedEventArgs> JustPressed = null;

        public void OnPressed(VirtualPadPressedEventArgs e)
        {
            if (Pressed != null)
                Pressed(this, e);
        }

        public void OnJustPressed(VirtualPadPressedEventArgs e)
        {
            if (JustPressed != null)
                JustPressed(this, e);
        }

        #endregion

        public VirtualPad()
        {
            _margin = new Vector2(3, 2);

            string path = String.Format("{0}/{1}/", "Misc/Pad", (GameConfiguration.VirtualPadStyle == VirtualPadStyle.Modern) ? "Modern" : "Normal");

            _upPad = new YnSprite(path + "kbup");
            _upPad.Name = "Button_" + ((int)ControlDirection.Up).ToString();
            Add(_upPad);

            _downPad = new YnSprite(path + "kbdown");
            _downPad.Name = "Button_" + ((int)ControlDirection.Down).ToString();
            Add(_downPad);

            _strafeLeftPad = new YnSprite(path + "kbstrleft");
            _strafeLeftPad.Name = "Button_" + ((int)ControlDirection.StrafeLeft).ToString();
            Add(_strafeLeftPad);

            _strafeRightPad = new YnSprite(path + "kbstrright");
            _strafeRightPad.Name = "Button_" + ((int)ControlDirection.StrafeRight).ToString();
            Add(_strafeRightPad);

            _leftPad = new YnSprite(path + "kbleft");
            _leftPad.Name = "Button_" + ((int)ControlDirection.Left).ToString();
            Add(_leftPad);

            _rightPad = new YnSprite(path + "kbright");
            _rightPad.Name = "Button_" + ((int)ControlDirection.Right).ToString();
            Add(_rightPad);

            foreach (YnSprite sprite in this)
            {
                sprite.Parent = null;
                if (GameConfiguration.ControlMode == ControlMode.New)
                    sprite.MouseClick += new EventHandler<MouseClickEntityEventArgs>(Pad_Click);
                else
                    sprite.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(Pad_Click);

                sprite.Alpha = 0.7f;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            UpdateLayoutPosition();

            Width = (int)(_leftPad.Width + _upPad.Width + _rightPad.Width + 2 * _margin.X);
            Height = (int)(_leftPad.Height + _strafeLeftPad.Height + _margin.Y);
        }

        public void UpdateLayoutPosition()
        {
            _leftPad.Position = new Vector2(X, Y);
            _upPad.Position = new Vector2(_leftPad.X + _leftPad.ScaledWidth + _margin.X, _leftPad.Y);
            _rightPad.Position = new Vector2(_upPad.X + _upPad.ScaledWidth + _margin.X, _leftPad.Y);

            _strafeLeftPad.Position = new Vector2(X, Y + _leftPad.ScaledHeight + _margin.Y);
            _downPad.Position = new Vector2(_strafeLeftPad.X + _strafeLeftPad.ScaledWidth + _margin.X, _strafeLeftPad.Y);
            _strafeRightPad.Position = new Vector2(_downPad.X + _downPad.ScaledWidth + _margin.X, _strafeLeftPad.Y);
        }

        public void UpdateScale(float scale)
        {
            foreach (YnEntity sceneObject in this)
            {
                sceneObject.Scale = new Vector2(scale);
            }
        }

        private void Pad_Click(object sender, MouseClickEntityEventArgs e)
        {
            YnSprite button = sender as YnSprite;

            if (button != null)
            {
                string [] temp = button.Name.Split(new char [] { '_' });
                int index = int.Parse(temp [1].ToString());

                ControlDirection direction = (ControlDirection)index;
                VirtualPadPressedEventArgs vpEvent = new VirtualPadPressedEventArgs(direction);

                if (e.JustClicked)
                    OnJustPressed(vpEvent);
                else
                    OnPressed(vpEvent);
            }
        }

#if NETFX_CORE
        private TouchCollection touch = TouchPanel.GetState();
        private Point touchPoint = new Point();
        private TouchCollection lastTouch = TouchPanel.GetState();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            lastTouch = touch;
            touch = TouchPanel.GetState();

            if (touch.Count > 0)
            {
                touchPoint = new Point((int)touch[0].Position.X, (int)touch[0].Position.Y);

                foreach (YnSprite sprite in this)
                {
                    if (sprite.Rectangle.Contains(touchPoint))
                    {
                        Pad_Click(sprite, new MouseClickEntityEventArgs(touchPoint.X, touchPoint.Y, MouseButton.Left, false, false));
                    }
                }
            }
        }
#endif
    }
}
