using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Maze3D.Data;
using Newtonsoft.Json;

namespace Maze3D.Serializer
{
    public partial class LevelSerializerGUI : Form
    {
        static int CurrentLevelId = 1;
        Dictionary<string, string> datas;
        Level tempLevel = null;
        bool initialized;
        Level level;
        Achievement achievement;

        public LevelSerializerGUI()
        {
            InitializeComponent();
            datas = new Dictionary<string, string>();
            initialized = false;

            string[] skyboxTypes = Enum.GetNames(typeof(SkyboxType));
            foreach (string type in skyboxTypes)
                skyboxType.Items.Add(type);
            skyboxType.SelectedIndex = 0;
        }

        private int[,] ParseLevel(string levelJSON)
        {
            string[] badChars = new string[] 
            {
                "\"A\"", "\"D\"", "'A'", "'D'"
            };

            string[] goodChars = new string[]
            {
                "8", "9", "8", "9"
            };

            for (int i = 0; i < 4; i++)
                levelJSON = levelJSON.Replace(badChars[i], goodChars[i]);

            return JsonConvert.DeserializeObject<int[,]>(levelJSON);
        }

        private void SerializeLevel()
        {
            string levelPath = "Textures/";

            Level level = new Level(int.Parse(datas["id"]));

            int[,] tiles = ParseLevel(datas["niveau"]);

            level.SetTiles2D(tiles);
            level.Width = tiles.GetLength(0);
            level.Depth = tiles.GetLength(1);
            level.WallTexture = levelPath + datas["textureMur"];
            level.GroundTexture = levelPath + datas["textureSol"];
            level.TopTexture = levelPath + datas["texturePlafond"];
            level.FinishTexture = levelPath + datas["textureArrivee"];
            level.SkyboxType = (SkyboxType)skyboxType.SelectedIndex;
            level.BlockSizes = new Size3(int.Parse(datas["tileWidth"]), int.Parse(datas["tileHeight"]), int.Parse(datas["tileDepth"]));
            tempLevel = level;
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(String.Format("level_{0}.xml", level.Id), settings))
            {
                IntermediateSerializer.Serialize<Level>(writer, level, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tempLevel != null)
            {
                string s = "";
                int[,] level = tempLevel.GetTiles2D();

                int width = level.GetLength(1);
                int depth = level.GetLength(0);

                for (int y = 0; y < depth; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        s += level[y, x] + " ";
                    }
                    s += "\n";
                }

                textContentLevel.Text = s;
            }
        }

        private void textContentLevel_TextChanged(object sender, EventArgs e)
        {
            levelID.Text = (CurrentLevelId++).ToString();
        }

        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!initialized)
            {
                datas.Add("id", levelID.Text.ToString());
                datas.Add("tileWidth", tileWidth.Text.ToString());
                datas.Add("tileHeight", tileHeight.Text.ToString());
                datas.Add("tileDepth", tileDepth.Text.ToString());
                datas.Add("skyboxType", skyboxType.SelectedIndex.ToString());
                datas.Add("texturePlafond", textureTop.Text.ToString());
                datas.Add("textureSol", textureGround.Text.ToString());
                datas.Add("textureArrivee", textureEnd.Text.ToString());
                datas.Add("textureMur", textureWall.Text.ToString());
                datas.Add("textureBorderWall", textureBorderWalls.Text.ToString());
                datas.Add("niveau", textContentLevel.Text.ToString());
                initialized = true;
            }
            else
            {
                datas["id"] = levelID.Text.ToString();
                datas["tileWidth"] = tileWidth.Text.ToString();
                datas["tileHeight"] = tileHeight.Text.ToString();
                datas["tileDepth"] = tileDepth.Text.ToString();
                datas["skyboxType"] = skyboxType.SelectedIndex.ToString();
                datas["texturePlafond"] = textureTop.Text.ToString();
                datas["textureSol"] = textureGround.Text.ToString();
                datas["textureArrivee"] = textureEnd.Text.ToString();
                datas["textureMur"] = textureWall.Text.ToString();
                datas["textureBorderWall"] = textureBorderWalls.Text.ToString();
                datas["niveau"] = textContentLevel.Text.ToString();
            }

            SerializeLevel();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
