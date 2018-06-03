using Maze3D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Helpers;

namespace Maze3D.Screen
{
    public class PopupState : YnState2D
    {
        private YnText _title;
        private YnText _content;
        private YnText _waitMessage;
        private YnSprite _background;
        private YnEntity _headerBackground;
        private MessageBoxButton _itemActionA;
        private MessageBoxButton _itemActionB;

        public string MessageBoxLabelA
        {
            get { return _itemActionA.Text; }
        }

        public string MessageBoxLabelB
        {
            get { return _itemActionB.Text; }
        }

        public event EventHandler<MessageBoxEventArgs> ActionNext = null;
        public event EventHandler<MessageBoxEventArgs> ActionMenu = null;

        public void OnActionNext(MessageBoxEventArgs e)
        {
            if (ActionNext != null)
                ActionNext(this, e);
        }

        public void OnActionMenu(MessageBoxEventArgs e)
        {
            if (ActionMenu != null)
                ActionMenu(this, e);
        }

        public PopupState(string name)
            : base(name, false)
        {
            int y = YnG.Height / 2 - ((YnG.Height / 4) / 2);
            _background = new YnSprite(new Rectangle(0, y, YnG.Width, (int)(ScreenHelper.GetScaleY(300))), Color.WhiteSmoke);
            _background.Alpha = 0.9f;
            Add(_background);

            _headerBackground = new YnEntity(new Rectangle((int)_background.X, (int)_background.Y, YnG.Width, (int)ScreenHelper.GetScaleY(50)), Color.DarkGray);
            Add(_headerBackground);

            _title = new YnText(Assets.FontKozuka30, "Fin de partie");
            _title.Color = Color.White;
            _title.Scale = ScreenHelper.GetScale() * 1.25f;
            Add(_title);

            _content = new YnText(Assets.FontKozuka20, "Vous avez terminé le niveau, que voulez vous faire ?");
            _content.Color = Color.Black;
            _content.Scale = ScreenHelper.GetScale() * 1.1f;
            Add(_content);

            _waitMessage = new YnText(Assets.FontKozuka30, "Chargement en cours...");
            _waitMessage.Color = Color.White;
            _waitMessage.Scale = ScreenHelper.GetScale() * 1.2f;
            _waitMessage.Active = false;
            Add(_waitMessage);

            _itemActionA = new MessageBoxButton(MessageBoxButtonType.Cancel, "Menu");
            _itemActionA.Position = new Vector2(_background.Width / 3, _background.Y + _background.Height - ScreenHelper.GetScaleY(100));
            _itemActionA.MouseClicked += (s, e) => OnActionMenu(new MessageBoxEventArgs(false, true));
            _itemActionA.Scale = ScreenHelper.GetScale();
            Add(_itemActionA);

            _itemActionB = new MessageBoxButton(MessageBoxButtonType.Validate, "Suivant");
            _itemActionB.Position = new Vector2((_background.Width / 3) * 2, _itemActionA.Y);
            _itemActionB.MouseClicked += (s, e) => OnActionMenu(new MessageBoxEventArgs(true, false));
            _itemActionB.Scale = ScreenHelper.GetScale();
            Add(_itemActionB);

            _enabled = true;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            _enabled = true;
        }

        public void ShowWaitMessage()
        {
            _waitMessage.Active = true;
        }

        public void HideWaitMessage()
        {
            _waitMessage.Active = false;
        }

        public void SetMessage(string title, string content)
        {
            _title.Text = title;
            _content.Text = content;
            Initialize();
        }

        public void SetActions(string actionA, string actionB)
        {
            _itemActionA.Text = actionA;
            _itemActionB.Text = actionB;
        }

        public override void Initialize()
        {
            base.Initialize();
            _title.Position = new Vector2(YnG.Width / 2 - _title.Width / 2, _headerBackground.Y + _headerBackground.Height / 2 - _title.Height / 2);
            _content.Position = new Vector2(25, _title.Y + 55);
            _waitMessage.Position = new Vector2(YnG.Width / 2 - _waitMessage.Width / 2, _background.Y + _background.Height + ScreenHelper.GetScaleY(25));
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!_enabled)
                return;

            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.Back) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.B))
                OnActionMenu(new MessageBoxEventArgs(false, true));
        }

        public override void Draw(GameTime gameTime)
        {
            if (spriteBatch == null)
                spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(YnG.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
