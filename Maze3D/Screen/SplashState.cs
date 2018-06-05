using Yna.Engine;
using Yna.Engine.Graphics;

namespace Maze3D.Screen
{
    public class SplashState : YnState2D
    {
        private Yna.Engine.Graphics.YnEntity2D splashScreen;
        private YnTimer waitTimer;

        public SplashState(string name)
            : base(name)
        {
            splashScreen = new YnEntity2D(Assets.TextureSplashScreen);
            Add(splashScreen);

            waitTimer = new YnTimer(1500, 0);
            waitTimer.Completed += (s, e) => stateManager.SetActive("menu", true);
            waitTimer.Start();
            Add(waitTimer);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            splashScreen.SetFullScreen();
        }
    }
}
