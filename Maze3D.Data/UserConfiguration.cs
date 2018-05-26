using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze3D.Data
{
#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public class UserConfiguration
    {
        public int LevelsUnlocked { get; set; }
        public bool EnabledMinimap { get; set; }
        public bool EnabledMinimapItems { get; set; }
        public bool EnabledMinimapUpdate { get; set; }
        public int MapCeilSize { get; set; }
        public int Difficulty { get; set; }

        // Contrôles
        public int ControlMode { get; set; }
        public bool EnabledVirtualPad { get; set; }
        public bool EnabledMouse { get; set; }
        public bool EnabledGamePad { get; set; }
        public int VirtualPadSize { get; set; }
        public int VirtualPadStyle  { get; set; }

        // Audio
        public bool EnabledSound { get; set; }
        public bool EnabledMusic { get; set; }
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }

        // Affichage
        public bool EnabledFullScreen { get; set; }
        public bool DetermineBestResolution { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public UserConfiguration()
        {
            LevelsUnlocked = 0;
            EnabledMinimap = true;
            EnabledMinimapItems = true;
            EnabledMinimapUpdate = true;
            MapCeilSize = 0;
            Difficulty = 0;
            ControlMode = 0;
            EnabledVirtualPad = true;
            EnabledMouse = true;
            EnabledGamePad = true;
            VirtualPadSize = 0;
            VirtualPadStyle = 0;
            EnabledSound = true;
            EnabledMusic = true;
            SoundVolume = 0.1f;
            MusicVolume = 0.75f;
            EnabledFullScreen = false;
            DetermineBestResolution = false;
            ScreenWidth = 1280;
            ScreenHeight = 768;
        }
    }
}
