using Maze3D.Control;
using Maze3D.UI;
using Yna.Engine;

namespace Maze3D
{
    public enum Difficulty
    {
        VeryEasy = 0, Easy, Normal, Hard
    }

    public class GameSettings
    {
        public static readonly int LevelCount = 8;
        private static GameSettings s_Instance = null;

        public static GameSettings Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var settings = YnG.StorageManager.Load<GameSettings>("Settings", "Settings.data");
                    s_Instance = settings != null ? settings : new GameSettings();
                }

                return s_Instance;
            }
        }

        public bool TabletMode = false;

        // Niveaux
        public int LevelStart = 1;
        public int LevelsUnlocked = 1;

        // Minimap 
        public bool EnabledMinimap = true;
        public bool EnabledMinimapUpdate = true;
        public bool EnabledMinimapItems = true;
        public MapCeilSize MapCeilSize = MapCeilSize.Small;
        public Difficulty Difficulty = Difficulty.VeryEasy;

        // Contrôles
        public ControlMode ControlMode = ControlMode.New;
        public bool EnabledVirtualPad = true;
        public bool EnabledMouse = true;
        public bool EnabledGamePad = true;
        public VirtualPadSize VirtualPadSize = Control.VirtualPadSize.Small;
        public VirtualPadStyle VirtualPadStyle = Control.VirtualPadStyle.Modern;

        // Audio
        public bool EnabledSound = true;
        public bool EnabledMusic = true;
        public float MusicVolume = 0.1f;
        public float SoundVolume = 0.75f;

        // Affichage
        public bool EnabledFullScreen = false;
        public bool DetermineBestResolution = false;
        public int ScreenWidth = 640;
        public int ScreenHeight = 400;

        // Niveau
        public void SetStartLevel(int level)
        {
            if (level <= LevelCount)
                LevelStart = level;
        }

        // Difficulté
        public void SetDifficulty(Difficulty difficulty)
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

        public void SetNextDifficulty()
        {
            int nextDifficulty = ((int)Difficulty) + 1;
            if (nextDifficulty <= 4)
                SetDifficulty((Difficulty)nextDifficulty);
        }

        public void Save()
        {
        }
    }
}
