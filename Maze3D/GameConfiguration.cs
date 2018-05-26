using Maze3D.Control;
using Maze3D.UI;

namespace Maze3D
{
    public enum Difficulty
    {
        VeryEasy = 0, Easy, Normal, Hard
    }

    public class GameConfiguration
    {
        public static bool TabletMode = false;

        // Niveaux
        public static int LevelStart = 1;
        public static int LevelCount = 8;
        public static int LevelsUnlocked = 1;

        // Minimap 
        public static bool EnabledMinimap = true;
        public static bool EnabledMinimapUpdate = true;
        public static bool EnabledMinimapItems = true;
        public static MapCeilSize MapCeilSize = MapCeilSize.Small;
        public static Difficulty Difficulty = Difficulty.VeryEasy;

        // Contrôles
        public static ControlMode ControlMode = ControlMode.New;
        public static bool EnabledVirtualPad = true;
        public static bool EnabledMouse = true;
        public static bool EnabledGamePad = true;
        public static VirtualPadSize VirtualPadSize = Control.VirtualPadSize.Small;
        public static VirtualPadStyle VirtualPadStyle = Control.VirtualPadStyle.Modern;

        // Audio
        public static bool EnabledSound = true;
        public static bool EnabledMusic = true;
        public static float MusicVolume = 0.1f;
        public static float SoundVolume = 0.75f;

        // Affichage
        public static bool EnabledFullScreen = false;
        public static bool DetermineBestResolution = false;
		public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        // Niveau
        public static void SetStartLevel(int level)
        {
            if (level <= LevelCount)
            {
                LevelStart = level;
            }
        }

        // Difficulté
        public static void SetDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.VeryEasy:
                    EnabledMinimap = true;
                    EnabledMinimapItems = true;
                    EnabledMinimapUpdate = true;
                    break;
                case Difficulty.Easy:
                    EnabledMinimap = true;
                    EnabledMinimapItems = false;
                    EnabledMinimapUpdate = true;
                    break;
                case Difficulty.Normal:
                    EnabledMinimap = true;
                    EnabledMinimapItems = false;
                    EnabledMinimapUpdate = false;
                    break;
                case Difficulty.Hard:
                    EnabledMinimap = false;
                    EnabledMinimapItems = false;
                    EnabledMinimapUpdate = false;
                    break;
            }

            Difficulty = difficulty;
        }

        public static void SetNextDifficulty()
        {
            int nextDifficulty = ((int)Difficulty) + 1;
            if (nextDifficulty <= 4)
                GameConfiguration.SetDifficulty((Difficulty)nextDifficulty);
        }
    }
}
