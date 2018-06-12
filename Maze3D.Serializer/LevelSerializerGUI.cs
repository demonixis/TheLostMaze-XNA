using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Maze3D.Data;
using Newtonsoft.Json;
using System.Text;

namespace Maze3D.Serializer
{
    public partial class LevelSerializerGUI : Form
    {
        static int CurrentLevelId = 1;
        private Dictionary<string, string> _datas;
        private Level tempLevel = null;
        private bool _initialized;

        public LevelSerializerGUI()
        {
            InitializeComponent();

            _datas = new Dictionary<string, string>();
            _initialized = false;

            var skyboxTypes = Enum.GetNames(typeof(SkyboxType));
            foreach (string type in skyboxTypes)
                skyboxType.Items.Add(type);

            skyboxType.SelectedIndex = 0;
        }

        private int[,] ParseLevel(string levelJSON)
        {
            var badChars = new string[]
            {
                "\"A\"", "\"D\"", "'A'", "'D'"
            };

            var goodChars = new string[]
            {
                "8", "9", "8", "9"
            };

            for (var i = 0; i < 4; i++)
                levelJSON = levelJSON.Replace(badChars[i], goodChars[i]);

            return JsonConvert.DeserializeObject<int[,]>(levelJSON);
        }

        private void SerializeLevel()
        {
            var levelPath = "Textures/";
            var level = new Level(int.Parse(_datas["id"]));
            var tiles = ParseLevel(_datas["niveau"]);

            level.SetTiles2D(tiles);
            level.Width = tiles.GetLength(0);
            level.Depth = tiles.GetLength(1);
            level.WallTexture = levelPath + _datas["textureMur"];
            level.GroundTexture = levelPath + _datas["textureSol"];
            level.TopTexture = levelPath + _datas["texturePlafond"];
            level.FinishTexture = levelPath + _datas["textureArrivee"];
            level.SkyboxType = (SkyboxType)skyboxType.SelectedIndex;
            level.BlockSizes = new Size3(int.Parse(_datas["tileWidth"]), int.Parse(_datas["tileHeight"]), int.Parse(_datas["tileDepth"]));
            tempLevel = level;

            var settings = new XmlWriterSettings() { Indent = true };

            using (XmlWriter writer = XmlWriter.Create(String.Format("level_{0}.xml", level.Id), settings))
                IntermediateSerializer.Serialize<Level>(writer, level, null);
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (tempLevel == null)
                return;

            var sb = new StringBuilder();
            var level = tempLevel.GetTiles2D();
            var width = level.GetLength(1);
            var depth = level.GetLength(0);

            for (var y = 0; y < depth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sb.Append(level[y, x]);
                    sb.Append(" ");
                }

                sb.Append("\r\n");
            }

            textContentLevel.Text = sb.ToString();
        }

        private void OnContentLevelChanged(object sender, EventArgs e) => levelID.Text = (CurrentLevelId++).ToString();

        private void OnSaveLevelClicked(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                _datas.Add("id", levelID.Text.ToString());
                _datas.Add("tileWidth", tileWidth.Text.ToString());
                _datas.Add("tileHeight", tileHeight.Text.ToString());
                _datas.Add("tileDepth", tileDepth.Text.ToString());
                _datas.Add("skyboxType", skyboxType.SelectedIndex.ToString());
                _datas.Add("texturePlafond", textureTop.Text.ToString());
                _datas.Add("textureSol", textureGround.Text.ToString());
                _datas.Add("textureArrivee", textureEnd.Text.ToString());
                _datas.Add("textureMur", textureWall.Text.ToString());
                _datas.Add("textureBorderWall", textureBorderWalls.Text.ToString());
                _datas.Add("niveau", textContentLevel.Text.ToString());
                _initialized = true;
            }
            else
            {
                _datas["id"] = levelID.Text.ToString();
                _datas["tileWidth"] = tileWidth.Text.ToString();
                _datas["tileHeight"] = tileHeight.Text.ToString();
                _datas["tileDepth"] = tileDepth.Text.ToString();
                _datas["skyboxType"] = skyboxType.SelectedIndex.ToString();
                _datas["texturePlafond"] = textureTop.Text.ToString();
                _datas["textureSol"] = textureGround.Text.ToString();
                _datas["textureArrivee"] = textureEnd.Text.ToString();
                _datas["textureMur"] = textureWall.Text.ToString();
                _datas["textureBorderWall"] = textureBorderWalls.Text.ToString();
                _datas["niveau"] = textContentLevel.Text.ToString();
            }

            SerializeLevel();
        }

        private void OnQuitButtonClicked(object sender, EventArgs e) => Application.Exit();
    }
}
