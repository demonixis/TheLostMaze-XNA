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
            string [] temp = param.Split(new char [] { '=' });
            string name = temp[0];
            string value = temp[1];

            switch (name)
            {
                case "width": GameConfiguration.ScreenWidth = int.Parse(value); break;
                case "height": GameConfiguration.ScreenHeight = int.Parse(value); break;
                case "fullscreen": GameConfiguration.EnabledFullScreen = bool.Parse(value); break;
                case "auto": GameConfiguration.DetermineBestResolution = bool.Parse(value); break;
                case "sound": GameConfiguration.EnabledSound = bool.Parse(value); break;
                case "difficulty":
                    switch (value)
                    {
                        case "very-easy": GameConfiguration.SetDifficulty(Difficulty.VeryEasy); break;
                        case "easy": GameConfiguration.SetDifficulty(Difficulty.Easy); break;
                        case "normal": GameConfiguration.SetDifficulty(Difficulty.Normal); break;
                        case "hard": GameConfiguration.SetDifficulty(Difficulty.Hard); break;
                    }
                    break;
                case "virtualpad": GameConfiguration.EnabledVirtualPad = bool.Parse(value); break;
                case "mouse": GameConfiguration.EnabledMouse = bool.Parse(value); break;
                case "gamepad": GameConfiguration.EnabledGamePad = bool.Parse(value); break;
                case "mode":
                    if (value == "old")
                        GameConfiguration.ControlMode = ControlMode.Old;
                    else
                        GameConfiguration.ControlMode = ControlMode.New;
                    break;
                case "level": 
                    GameConfiguration.SetStartLevel(int.Parse(value)); 
                    break;
                default: break;
            }
        }
    }
}

