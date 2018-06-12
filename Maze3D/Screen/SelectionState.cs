using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Helpers;

namespace Maze3D.Screen
{
    public class SelectionState : BaseMenu
    {
        private Yna.Engine.Graphics.YnEntity2D[] _levelTiles;

        public SelectionState(string name)
            : base(name, "Choix du niveau", 0)
        {
            _title.Text = Translation.Get("Levels");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _levelTiles = new YnEntity2D[GameSettings.LevelCount];

            YnText levelText = null;
            int cursor = 0;
            float x = (int)ScreenHelper.GetScaleX(65);
            float y = (int)ScreenHelper.GetScaleX(145);
            float maxWidth = (int)(YnG.Width / 4 - ScreenHelper.GetScaleX(90));
            float offsetX = ScreenHelper.GetScaleX(40);
            float offsetY = ScreenHelper.GetScaleY(35);
            float paddingX = 1.25f;
            YnText.DefaultColor = Color.Yellow;

            for (int i = 0; i < GameSettings.LevelCount; i++)
            {
                _levelTiles[i] = new YnEntity2D("Misc/mapPreview");
                _levelTiles[i].LoadContent();

                if (i == 4)
                {
                    y += (int)(maxWidth + offsetY);
                    cursor = 0;
                }

                x = (cursor++ * maxWidth * paddingX) + offsetX;

                _levelTiles[i].Position = new Vector2(x, y);
                _levelTiles[i].Name = "level_" + (i + 1);
                _levelTiles[i].Scale = new Vector2((float)(maxWidth / _levelTiles[i].Width), (float)(maxWidth / _levelTiles[i].Height));
                Add(_levelTiles[i]);

                levelText = new YnText(Assets.FontKozuka30, (i + 1).ToString());
                levelText.LoadContent();
                levelText.Scale = new Vector2(1.5f);
                levelText.Position = new Vector2(
                    _levelTiles[i].X + _levelTiles[i].ScaledWidth / 2 - levelText.ScaledWidth / 2,
                    _levelTiles[i].Y + _levelTiles[i].ScaledHeight / 2 - levelText.ScaledHeight / 2);
                Add(levelText);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            var settings = GameSettings.Instance;

            for (int i = 0; i < GameSettings.LevelCount; i++)
            {
                if (settings.LevelsUnlocked >= (i + 1))
                {
                    _levelTiles[i].MouseClicked += item_MouseJustClicked;
                    _levelTiles[i].Color = Color.White;
                }
                else
                {
                    _levelTiles[i].MouseClicked -= item_MouseJustClicked;
                    _levelTiles[i].Color = Color.Gray;
                }
            }
        }

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
            var levelScreen = sender as YnEntity2D;
            var settings = GameSettings.Instance;

            if (levelScreen != null)
            {
                YnG.AudioManager.PlaySound(Assets.SoundCrystal);

                var levelId = int.Parse(levelScreen.Name.Split(new char[] { '_' })[1].ToString());
                settings.SetStartLevel(levelId);
                (YnG.Game as MazeGame).PrepareNewLevel(levelId, true);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Gamepad.JustPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.Back) || YnG.Keys.JustPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                YnG.StateManager.SetActive("menu", true);
        }
    }
}
