using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Helpers;

namespace Maze3D.Screen
{
    public class MenuState : BaseMenu
    {
        private YnText[] menuItems;
        private int menuItemIndex;
        private Yna.Engine.Graphics.YnEntity menuItemSelector;
        private bool _checkForNext;
        private bool _checkForHover;

        public MenuState(string name)
            : base(name, "", 3)
        {
            YnG.ShowMouse = true;

            _checkForNext = false;
            _checkForHover = false;
            menuItemIndex = 0;

            menuItemSelector = new Yna.Engine.Graphics.YnEntity("UI/menuItemBar");
            Add(menuItemSelector);

            string[] itemNames = new string[5]
            {
                MazeLang.Text.Menus.New,
                MazeLang.Text.Menus.Choose,
                MazeLang.Text.Menus.Options,
                MazeLang.Text.Menus.Credits,
                MazeLang.Text.Menus.Exit
            };

            menuItems = new YnText[5];
            for (int i = 0, l = itemNames.Length; i < l; i++)
            {
                menuItems[i] = new YnText("Font/kozuka_30", itemNames[i]);
                menuItems[i].Color = Color.White;
                menuItems[i].Name = "mi_" + i;
                menuItems[i].MouseClicked += MenuState_MouseClicked;
                menuItems[i].MouseOver += MenuState_MouseOver;

                if (i > 2)
                    menuItems[i].Scale = new Vector2(0.75f);

                Add(menuItems[i]);
            }

            _background.AssetName = Assets.TextureMenuTitle;
        }

        public override void Initialize()
        {
            base.Initialize();

            int x = (YnG.Width / 2) - menuItems[0].Width / 2;
            int y = 0;
            int topOffset = (int)ScreenHelper.GetScaleY(290);

            for (int i = 0; i < 3; i++)
            {
                y = (int)(topOffset + i * menuItems[i].ScaledHeight * 2.25f);
                menuItems[i].Position = new Vector2(x, y);
            }

            menuItems[4].Position = new Vector2(YnG.Width - menuItems[4].ScaledWidth - ScreenHelper.GetScaleX(45), YnG.Height - menuItems[4].ScaledHeight - ScreenHelper.GetScaleY(5));
            menuItems[3].Position = new Vector2(menuItems[4].X - menuItems[4].ScaledWidth - ScreenHelper.GetScaleX(45), menuItems[4].Y);
            menuItemSelector.Position = new Vector2(0, menuItems[0].Y + menuItems[0].ScaledHeight / 2 - menuItemSelector.ScaledHeight / 2);
        }

        void MenuState_MouseClicked(object sender, MouseClickEntityEventArgs e)
        {
            _checkForNext = true;
            menuItemIndex = GetIndex(sender as YnText);
        }

        void MenuState_MouseOver(object sender, MouseOverEntityEventArgs e)
        {
            _checkForHover = true;
            menuItemIndex = GetIndex(sender as YnText);
        }

        private int GetIndex(YnText item)
        {
            return int.Parse(item.Name.Split(new char[] { '_' })[1]);
        }

        private void ResetColor()
        {
            for (int i = 0; i < 5; i++)
                menuItems[i].Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Up) || YnG.Keys.JustPressed(Keys.Down) || YnG.Keys.JustPressed(Keys.Left) || YnG.Keys.JustPressed(Keys.Right) || _checkForHover)
            {
                if (YnG.Keys.JustPressed(Keys.Up) || YnG.Keys.JustPressed(Keys.Left))
                    menuItemIndex--;

                else if (YnG.Keys.JustPressed(Keys.Down) || YnG.Keys.JustPressed(Keys.Right))
                    menuItemIndex++;

                menuItemIndex = menuItemIndex < 0 ? (menuItems.Length - 1) : menuItemIndex;
                menuItemIndex = menuItemIndex > (menuItems.Length - 1) ? 0 : menuItemIndex;

                ResetColor();

                menuItems[menuItemIndex].Color = Color.DodgerBlue;

                if (menuItemIndex < 3)
                {
                    menuItemSelector.Position = new Vector2(0, menuItems[menuItemIndex].Y + menuItems[menuItemIndex].ScaledHeight / 2 - menuItemSelector.ScaledHeight / 2);
                    menuItemSelector.Visible = true;
                }
                else
                    menuItemSelector.Visible = false;

                _checkForHover = false;
            }
            else if (YnG.Keys.JustPressed(Keys.Enter) || YnG.Keys.JustPressed(Keys.Space) || _checkForNext)
            {
                string nextState = String.Empty;

                switch (menuItemIndex)
                {
                    case 0: (YnG.Game as MazeGame).PrepareNewLevel(1, true); break;
                    case 1: nextState = "selection"; break;
                    case 2: nextState = "options"; break;
                    case 3: nextState = "about"; break;
                    case 4: YnG.Exit(); break;
                }

                if (nextState != String.Empty)
                    stateManager.SetActive(nextState, true);

                _checkForNext = false;
            }
        }
    }
}
