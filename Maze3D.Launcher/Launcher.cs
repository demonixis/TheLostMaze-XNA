using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Maze3D.Launcher
{
    public partial class Launcher : Form
    {
        Dictionary<string, string> gameConfiguration;

        public Launcher()
        {
            InitializeComponent();

            gameConfiguration = new Dictionary<string, string>();

            gameConfiguration.Add("width", "1024");
            gameConfiguration.Add("height", "768");
            gameConfiguration.Add("auto", "false");
            gameConfiguration.Add("sound", "true");
            gameConfiguration.Add("difficulty", "easy");
            gameConfiguration.Add("virtualpad", "true");
            gameConfiguration.Add("gamepad", "true");
            gameConfiguration.Add("mouse", "true");
            gameConfiguration.Add("mode", "new");
            gameConfiguration.Add("level", "1");

            rendererChoice.SelectedIndex = 0;
        }

        private void detectAuto_CheckedChanged(object sender, EventArgs e)
        {
            screenConfig.Enabled = !detectAuto.Checked;
            gameConfiguration["auto"] = detectAuto.Checked.ToString();

            if (screenConfig.Enabled)
                gameConfiguration["fullscreen"] = true.ToString();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (checkValues())
            {
                string cmdParams = "";

                foreach (KeyValuePair<string, string> keyValue in gameConfiguration)
                    cmdParams += String.Format(" {0}={1}", keyValue.Key.ToString(), keyValue.Value.ToString());

                LaunchGameWithOptions(cmdParams);
            }
        }

        private void LaunchGameWithOptions(string options)
        {
            string folder = "DirectX";
            if (rendererChoice.SelectedIndex > 0)
                folder = "OpenGL";

            Process process = new Process();
            process.StartInfo.Arguments = options;
            process.StartInfo.FileName = Path.Combine(folder, "Maze3D.exe");
            process.Start();
        }

        private bool checkValues()
        {
            bool isOk = true;

            if (detectAuto.Checked)
                gameConfiguration["auto"] = "true";
            else
            {
                gameConfiguration["width"] = screenWidth.Text.ToString();
                gameConfiguration["height"] = screenHeight.Text.ToString();
                gameConfiguration["fullscreen"] = useFullscreen.Checked.ToString();
                gameConfiguration["auto"] = "false";

                if (int.Parse(gameConfiguration["width"]) < 640 || int.Parse(gameConfiguration["height"]) < 480)
                    isOk = false;
            }

            gameConfiguration["sound"] = useSound.Checked.ToString();
            gameConfiguration["gamepad"] = useGamepad.Checked.ToString();
            gameConfiguration["mouse"] = useMouse.Checked.ToString();
            gameConfiguration["virtualpad"] = useVirtualPad.Checked.ToString();

            switch (selectDifficulty.SelectedIndex)
            {
                case 0: gameConfiguration["difficulty"] = "very-easy"; break;
                case 1: gameConfiguration["difficulty"] = "easy"; break;
                case 2: gameConfiguration["difficulty"] = "normal"; break;
                case 3: gameConfiguration["difficulty"] = "hard"; break;
                default: gameConfiguration["difficulty"] = "easy"; break;
            }

            switch (selectMode.SelectedIndex)
            {
                case 0: gameConfiguration["mode"] = "new"; break;
                case 1: gameConfiguration["mode"] = "old"; break;
                default: gameConfiguration["mode"] = "new"; break;
            }

            int levelId = selectLevel.SelectedIndex;
            gameConfiguration["level"] = (++levelId).ToString();

            if (!isOk)
                MessageBox.Show("Merci d'utiliser des valeurs justes pour le choix de la résolution pour l'écran", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return isOk;
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            selectDifficulty.SelectedIndex = 0;
            selectMode.SelectedIndex = 0;
            selectLevel.SelectedIndex = 0;
        }

        private void useFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            gameConfiguration["fullscreen"] = useFullscreen.Checked.ToString();
        }
    }
}
