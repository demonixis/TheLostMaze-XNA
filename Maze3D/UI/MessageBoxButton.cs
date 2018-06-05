using Microsoft.Xna.Framework;
using Yna.Engine.Graphics;

namespace Maze3D.UI
{
    public enum MessageBoxButtonType
    {
        Validate, Cancel
    }

    public class MessageBoxButton : YnGroup
    {
        private YnEntity2D button;
        private YnText label;

        public string FontName
        {
            get { return label.AssetName; }
            set { label.AssetName = value; }
        }

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public MessageBoxButton(MessageBoxButtonType type, string text)
        {
            button = new YnEntity2D("Misc/button");
            Add(button);

            label = new YnText("Font/kozuka_20", text);
            label.Color = Color.White;
            Add(label);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            button.Position = new Vector2(X, Y);
            label.Position = new Vector2(button.X + button.ScaledWidth / 2 - label.ScaledWidth / 2, button.Y + button.ScaledHeight / 2 - label.ScaledHeight / 2);
            Width = (int)button.ScaledWidth;
            Height = (int)button.ScaledHeight;
        }
    }
}
