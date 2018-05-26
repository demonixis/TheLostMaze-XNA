using Microsoft.Xna.Framework;
using Yna.Engine.Graphics;

namespace Maze3D.UI
{
    public class MenuItem : YnGroup
    {
        private YnSprite _buttonNormal;
        private YnText _contentText;
        private bool _selected;
        private int _itemPosition;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public int ItemPosition
        {
            get { return _itemPosition; }
        }

        public void SetSelected(bool selected)
        {
            _selected = selected;
        }

        public Vector2 ItemTextSize
        {
            get { return _contentText.Scale; }
            set { _contentText.Scale = value; }
        }

        public MenuItem(string text, int itemPosition, bool selected)
        {
            _itemPosition = itemPosition;
            _selected = selected;

            _buttonNormal = new YnSprite(Vector2.Zero, "Misc/button");
            Add(_buttonNormal);

            _contentText = new YnText("Font/kozuka_20", text);
            _contentText.Color = Color.White;
            Add(_contentText);

            MouseOver += _buttonNormal_MouseOver;
            MouseLeave += _buttonNormal_MouseLeave;
        }

        void _buttonNormal_MouseLeave(object sender, Yna.Engine.Graphics.Event.MouseLeaveEntityEventArgs e)
        {
            _buttonNormal.Color = Color.White;
        }

        void _buttonNormal_MouseOver(object sender, Yna.Engine.Graphics.Event.MouseOverEntityEventArgs e)
        {
            _buttonNormal.Color = Color.DarkGoldenrod;
        }

        public void SetButtonNormal()
        {
            _buttonNormal.Color = Color.White;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            CenterText();
        }

        private void CenterText()
        {
            _contentText.Position = new Vector2(
                (_buttonNormal.X + _buttonNormal.ScaledWidth / 2) - _contentText.ScaledWidth / 2,
                (_buttonNormal.Y + _buttonNormal.ScaledHeight / 2) - _contentText.ScaledHeight / 2);
        }
    }
}
