using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Event;

namespace Maze3D.Screen
{
    internal class ItemLabelText : YnGroup
    {
        private YnText _label;
        private YnText _content;

        public ItemLabelText(string label, string content)
        {
            _label = new YnText("Font/kozuka_30", label);
            _label.Color = Color.White;
            Add(_label);

            _content = new YnText("Font/kozuka_20", content);
            _content.Color = Color.White;
            Add(_content);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _label.Position = new Vector2(45, 0);
            _content.Position = new Vector2(45, _label.Position.Y + _label.Height + 45);
        }

        public void WrapText()
        {
            _label.Text = _label.GetWrappedText(_label.Text, YnG.Width - 45);
            _content.Text = _content.GetWrappedText(_content.Text, YnG.Width - 45);
        }
    }

    public class AboutState : BaseMenu
    {
        private ItemLabelText _aboutText;
        private ItemLabelText _greetingText;
        
        public AboutState(string name)
            : base(name, Translation.Get("Credits"), 0)
        {
            var sb = new StringBuilder();
            sb.Append(Translation.Get("Txt_Description"));
            sb.Append("\r\n");
            sb.Append(Translation.Get("Txt_Description2"));
            sb.Append("\r\n\r\n");
            sb.Append(Translation.Get("Txt_Description3"));

            _aboutText = new ItemLabelText(Translation.Get("About this game"), sb.ToString());
            Add(_aboutText);

            sb.Clear();
            sb.Append(Translation.Get("Txt_Greeting"));
            sb.Append("\r\n\r\n");
            sb.Append(Translation.Get("Txt_Greeting2"));

            _greetingText = new ItemLabelText(Translation.Get("Greeting"), sb.ToString());
            Add(_greetingText);
        }

        public override void Initialize()
        {
            base.Initialize();

            _aboutText.WrapText();
            _greetingText.WrapText();

            _aboutText.Position = new Vector2(0, 45);
            _greetingText.Position = new Vector2(0, 300);
        }

		protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.Back))
                YnG.StateManager.SetActive("menu", true);
        }
    }
}
