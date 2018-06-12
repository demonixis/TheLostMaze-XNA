using Maze3D.Control;

namespace Maze3D
{
    static class Program
    {
        /// <summary>
        /// Point d’entrée principal pour l’application.
        /// </summary>
        static void Main(string[] args)
        {
            int size = args.Length;

            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                    PrepareParams(args[i]);
            }

            using (MazeGame game = new MazeGame())
                game.Run();
        }

        static void PrepareParams(string param)
        {
            var temp = param.Split(new char [] { '=' });
            var name = temp[0];
            var value = temp[1];
            var settings = GameSettings.Instance;

            switch (name)
            {
                case "width": settings.ScreenWidth = int.Parse(value); break;
                case "height": settings.ScreenHeight = int.Parse(value); break;
                case "fullscreen": settings.EnabledFullScreen = bool.Parse(value); break;
                case "auto": settings.DetermineBestResolution = bool.Parse(value); break;
                case "sound": settings.EnabledSound = bool.Parse(value); break;
                case "difficulty":
                    switch (value)
                    {
                        case "very-easy": settings.SetDifficulty(Difficulty.VeryEasy); break;
                        case "easy": settings.SetDifficulty(Difficulty.Easy); break;
                        case "normal": settings.SetDifficulty(Difficulty.Normal); break;
                        case "hard": settings.SetDifficulty(Difficulty.Hard); break;
                    }
                    break;
                case "virtualpad": settings.EnabledVirtualPad = bool.Parse(value); break;
                case "mouse": settings.EnabledMouse = bool.Parse(value); break;
                case "gamepad": settings.EnabledGamePad = bool.Parse(value); break;
                case "mode":
                    if (value == "old")
                        settings.ControlMode = ControlMode.Old;
                    else
                        settings.ControlMode = ControlMode.New;
                    break;
                case "level":
                    settings.SetStartLevel(int.Parse(value)); 
                    break;
                default: break;
            }
        }
    }
}

