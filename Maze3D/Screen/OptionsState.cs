using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics.Event;

namespace Maze3D.Screen
{
    public class OptionsState : BaseMenu
    {
        public OptionsState(string name)
            : base(name, "Options", 0)
        {

        }

        public override void Update(GameTime gameTime)
        {
			base.Update(gameTime);

            if (YnG.Gamepad.JustPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.Back) || YnG.Keys.JustPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                YnG.StateManager.SetActive("menu", true);
        }

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {

        }
    }
}
