using Maze3D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Event;

namespace Maze3D.Screen
{
    public abstract class BaseMenu : YnState2D
    {
        protected Yna.Engine.Graphics.YnEntity _background;					// Fond
        protected YnText _title;                        // Titre
        protected List<MenuItem> items;                 // Elements du menu

        /// <summary>
        /// Représente un écran de menu standard avec un titre et des items
        /// </summary>
        public BaseMenu(string name, string title, int numItems)
            : base(name)
        {
            _background = new Yna.Engine.Graphics.YnEntity(Assets.TextureMenuBase);
            Add(_background);

            // 1 - Le titre
            _title = new YnText(Assets.FontKozuka30, title);
            _title.Color = Color.GhostWhite;
            _title.Scale = new Vector2(1.5f, 1.5f);
            Add(_title);

            items = new List<MenuItem>(numItems);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            foreach (MenuItem item in items)
                item.MouseClicked += item_MouseJustClicked;
        }

        protected override void OnDesactivated(EventArgs e)
        {
            base.OnDesactivated(e);
            foreach (MenuItem item in items)
                item.MouseClicked -= item_MouseJustClicked;
        }

        // Lorsqu'on click sur un item du menu
        protected virtual void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            _title.Position = new Vector2(YnG.Width / 2 - _title.Width / 2, 25);
            _background.SetFullScreen();
        }
    }
}
