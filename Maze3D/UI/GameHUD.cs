using Microsoft.Xna.Framework;
using System;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Helpers;

namespace Maze3D.UI
{
    public class GameHUD : YnGroup
    {
        private Vector2 _scaleFactor;
        private YnText scoreText;
        private YnEntity scoreCounterEntity;

        private YnText timeText;
        private YnEntity timeCounterWheelEntity;
        private YnEntity timeCounterNeedleEntity;

        private YnText itemsCounter;
        private YnEntity itemsCounterEntity;
        private int itemsCount;
        private int nbItemsCollected;
        private YnTimer highlightTimer;

        private MiniMap miniMap;
        private YnEntity miniMapLeftBorder;
        private YnEntity miniMapBottomBorder;

        private YnEntity bottomBar;
        private YnEntity bottomLogo;

        private readonly Color TextColor = new Color(207, 210, 215);

        public MiniMap MiniMap
        {
            get { return miniMap; }
        }

        public string Score
        {
            set { scoreText.Text = value; }
        }

        public string Time
        {
            set { timeText.Text = value; }
        }

        public string ItemCounter
        {
            get { return itemsCounter.Text; }
            set { itemsCounter.Text = value; }
        }

        public GameHUD()
        {
            itemsCount = 0;
            nbItemsCollected = 0;

            itemsCounter = new YnText("Font/Desiree_20", "{0} / {1} Cristaux");
            itemsCounter.Scale = new Vector2(1.2f);
            itemsCounter.Color = TextColor;
            Add(itemsCounter);

            scoreText = new YnText("Font/Desiree_20", "0 pt");
            scoreText.Scale = new Vector2(1.4f);
            scoreText.Color = TextColor;
            Add(scoreText);

            scoreCounterEntity = new YnEntity("UI/needle");
            Add(scoreCounterEntity);

            timeText = new YnText("Font/Desiree_30", "00 : 00");
            timeText.Scale = new Vector2(2.0f);
            timeText.Color = TextColor;
            Add(timeText);

            itemsCounterEntity = new YnEntity("UI/needle");
            Add(itemsCounterEntity);

            timeCounterWheelEntity = new YnEntity("UI/topWheel");
            Add(timeCounterWheelEntity);

            timeCounterNeedleEntity = new YnEntity("UI/needle-watch");
            Add(timeCounterNeedleEntity);

            miniMapLeftBorder = new YnEntity("UI/mapBorderLeft");
            Add(miniMapLeftBorder);

            miniMapBottomBorder = new YnEntity("UI/mapBorderBottom");
            Add(miniMapBottomBorder);

            bottomBar = new YnEntity(new Rectangle(0, 0, YnG.Width, (int)ScreenHelper.GetScaleY(40)), Color.Black);
            bottomBar.Alpha = 0.4f;
            Add(bottomBar);

            bottomLogo = new YnEntity("UI/bottomWheel");
            Add(bottomLogo);

            highlightTimer = new YnTimer(1000);
            highlightTimer.Completed += highlightTimer_Completed;
        }

        public void InitializeMinimap(MazeLevel mazeLevel)
        {
            itemsCount = mazeLevel.Items.Count;
            itemsCounter.Text = String.Format("{0} / {1}", new object[] { 0, itemsCount }); 

            miniMap = new MiniMap(mazeLevel.Tiles, mazeLevel.Level.BlockSizes.Width, mazeLevel.Level.BlockSizes.Depth);
            miniMap.LoadContent();
            miniMap.Enabled = GameConfiguration.EnabledMinimapUpdate;
            miniMap.Visible = GameConfiguration.EnabledMinimap;
            Add(miniMap);

            miniMapLeftBorder.Active = GameConfiguration.EnabledMinimap;
            miniMapBottomBorder.Active = GameConfiguration.EnabledMinimap;

            miniMapBottomBorder.Width = (int)(miniMap.Width);
            miniMapBottomBorder.Position = new Vector2(miniMap.X, miniMap.Y + miniMap.Height);

            miniMapLeftBorder.Height = (int)(miniMap.Height + miniMapBottomBorder.ScaledHeight);
            miniMapLeftBorder.Position = new Vector2(miniMap.X - miniMapLeftBorder.ScaledWidth, miniMap.Y);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            timeCounterWheelEntity.SetOrigin(SpriteOrigin.Center);
        }

        public override void Initialize()
        {
            base.Initialize();

            _scaleFactor = ScreenHelper.GetScale();

            timeCounterWheelEntity.Scale = _scaleFactor;
            timeCounterWheelEntity.Position = new Vector2(YnG.Width / 2 - timeCounterWheelEntity.ScaledWidth / 2 + (timeCounterWheelEntity.Origin.X * timeCounterWheelEntity.Scale.X), ScreenHelper.GetScaleY(-70));
            timeCounterNeedleEntity.Scale = _scaleFactor;
            timeCounterNeedleEntity.Position = new Vector2(YnG.Width / 2 - timeCounterNeedleEntity.ScaledWidth / 2, 0);
            timeText.Scale *= _scaleFactor;
            timeText.Position = new Vector2(YnG.Width / 2 - timeText.ScaledWidth / 2, timeCounterNeedleEntity.ScaledHeight + ScreenHelper.GetScaleY(10));

            scoreCounterEntity.Scale = _scaleFactor;
            scoreCounterEntity.Position = new Vector2(0, ScreenHelper.GetScaleY(15));
            scoreText.Scale *= _scaleFactor;
            scoreText.Position = new Vector2(scoreCounterEntity.ScaledWidth + ScreenHelper.GetScaleX(10), scoreCounterEntity.Y + scoreCounterEntity.ScaledHeight / 2 - scoreText.ScaledHeight / 2);

            itemsCounterEntity.Scale = _scaleFactor;
            itemsCounterEntity.Position = new Vector2(0, scoreCounterEntity.Y + scoreCounterEntity.ScaledHeight);
            itemsCounter.Scale *= _scaleFactor;
            itemsCounter.Position = new Vector2(itemsCounterEntity.ScaledWidth + ScreenHelper.GetScaleX(10), itemsCounterEntity.Y + itemsCounterEntity.ScaledHeight / 2 - scoreText.ScaledHeight / 2);

            bottomBar.Rectangle = new Rectangle(0, (int)(YnG.Height - bottomBar.ScaledHeight), YnG.Width, bottomBar.Height);
            bottomLogo.Position = new Vector2(YnG.Width / 2 - bottomLogo.ScaledWidth / 2, YnG.Height - bottomLogo.ScaledHeight);
        }

        public void InitializePhoneLayout()
        {
            bottomBar.Active = false;
            bottomLogo.Active = false;
        }

        public void UpdateNbCrystals()
        {
            nbItemsCollected++;
            itemsCounter.Text = String.Format("{0} / {1}", new object[] { nbItemsCollected, itemsCount });

            if (!highlightTimer.Enabled)
            {
                itemsCounter.Color = Color.Yellow;
                highlightTimer.Start();
            }
        }

        void highlightTimer_Completed(object sender, EventArgs e)
        {
            itemsCounter.Color = TextColor;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            highlightTimer.Update(gameTime);
            timeCounterWheelEntity.Rotation += gameTime.ElapsedGameTime.Milliseconds * 0.00025f;
        }
    }
}
