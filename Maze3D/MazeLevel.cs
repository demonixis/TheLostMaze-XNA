using Maze3D.Data;
using Microsoft.Xna.Framework;
using System;
using Yna.Engine.Content;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Terrain;

namespace Maze3D
{
    public class MazeLevel : YnGroup3D
    {
        private int _levelId;
        private Level _level;
        private int[,] _levelTiles;
        private YnGroup3D _groupWalls;
        private YnGroup3D _groupItems;
        private YnMeshGeometry _endGeometry;
        private Vector3 _startPosition;
        private SkyBox _skybox;

        #region Propriétés du niveau

        /// <summary>
        /// Position de départ de la caméra.
        /// </summary>
        public Vector3 StartPosition
        {
            get { return _startPosition; }
        }

        /// <summary>
        /// Obtient le tableau de structure du niveau.
        /// </summary>
        public int[,] Tiles
        {
            get { return _levelTiles; }
        }

        /// <summary>
        /// Obtient le numéro de niveau.
        /// </summary>
        public new int Id
        {
            get { return _levelId; }
        }

        /// <summary>
        /// Obtient l'instance Level utilisé pour générer ce niveau.
        /// </summary>
        public Level Level
        {
            get { return _level; }
        }

        /// <summary>
        /// Obtient la collection des murs.
        /// </summary>
        public YnGroup3D Walls
        {
            get { return _groupWalls; }
        }

        /// <summary>
        /// Obtient la collection des éléments à ramasser.
        /// </summary>
        public YnGroup3D Items
        {
            get { return _groupItems; }
        }

        #endregion

        /// <summary>
        /// Création et construction d'un niveau de labyrinthe.
        /// </summary>
        /// <param name="id">Numéro de niveau.</param>
        public MazeLevel(int id)
            : base(null)
        {
            _levelId = id;
            _groupWalls = new YnGroup3D(this);
            Add(_groupWalls);

            _groupItems = new YnGroup3D(this);
            Add(_groupItems);

            _startPosition = Vector3.Zero;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            InitializeLevel();
            GenerateCubeWalls();
            GenerateBorderWalls();
            GenerateGrounds();
            GenerateSkyBox();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //_endChest.RotateY(0.5f);
            _endGeometry.RotateX(0.5f);
        }

        /// <summary>
        /// Création et chargement de l'entité niveau.
        /// </summary>
        private void InitializeLevel()
        {
            _level = ContentHelper.LoadXMLFromXna<Level>(String.Format("Data/Levels/level_{0}", _levelId));
            _level.Initialize();
            _levelTiles = _level.GetTiles2D();
        }

        /// <summary>
        /// Génération de la Skybox.
        /// </summary>
        private void GenerateSkyBox()
        {
            if (_level.SkyboxType != SkyboxType.None)
            {
                string[] skyAssets;

                if (_level.SkyboxType == SkyboxType.Day)
                    skyAssets = Assets.SkyboxLand;
                else
                    skyAssets = Assets.SkyboxNight;

                _skybox = new SkyBox(Math.Max(_level.Width * _level.BlockSizes.Width, _level.Depth * _level.BlockSizes.Depth) * 4.5f, skyAssets);
                _skybox.LoadContent();
                _skybox.SetLightEnable(false);
                _skybox.Position = new Vector3((_level.Width * _level.BlockSizes.Width), 0, (_level.Depth * _level.BlockSizes.Depth));
                Add(_skybox);
            }
        }

        private void GenerateGrounds()
        {
            bool enableSkybox = _level.SkyboxType != SkyboxType.None;
            if (!enableSkybox)
            {
                PlaneGeometry planG = new PlaneGeometry(new Vector3(_level.Width * _level.BlockSizes.Width, 0, _level.Depth * _level.BlockSizes.Depth));
                planG.InvertFaces = true;
                planG.TextureRepeat = new Vector2(24);

                YnMeshGeometry top = new YnMeshGeometry(planG, new BasicMaterial(_level.TopTexture));
                top.LoadContent();
                top.Position = new Vector3((_level.Width * _level.BlockSizes.Width), _level.BlockSizes.Height * 2, (_level.Depth * _level.BlockSizes.Depth));
                Add(top);
            }

            PlaneGeometry planG2 = new PlaneGeometry(new Vector3(_level.Width * _level.BlockSizes.Width, 0, _level.Depth * _level.BlockSizes.Depth));
            planG2.TextureRepeat = new Vector2(24);

            YnMeshGeometry ground = new YnMeshGeometry(planG2, _level.GroundTexture);
            ground.LoadContent();
            ground.Position = new Vector3((_level.Width * _level.BlockSizes.Width), 0, (_level.Depth * _level.BlockSizes.Depth));
            Add(ground);
        }

        /// <summary>
        /// Génération des murs de bordure.
        /// </summary>
        private void GenerateBorderWalls()
        {
            Vector3[] sizes = new Vector3[4]
            {
                new Vector3(_level.GetWorldWidth(), _level.BlockSizes.Height, 1),
                new Vector3(_level.GetWorldWidth(), _level.BlockSizes.Height, 1),
                new Vector3(1, _level.BlockSizes.Height, _level.GetWorldDepth()),
                new Vector3(1, _level.BlockSizes.Height, _level.GetWorldDepth())
            };

            Vector3[] positions = new Vector3[4] 
            {
                new Vector3(_level.GetWorldWidth(), _level.BlockSizes.Height, 0),
                new Vector3(_level.GetWorldWidth(), _level.BlockSizes.Height, 2 * _level.GetWorldDepth()),
                new Vector3(0, _level.BlockSizes.Height, _level.GetWorldWidth()),
                new Vector3(2 * _level.GetWorldDepth(), _level.BlockSizes.Height, _level.GetWorldWidth())
            };

            Color[] colors = new Color[4]
            {
                Color.Red, Color.Green, Color.Blue, Color.Yellow
            };

            CubeGeometry cube = null;
            YnMeshGeometry mesh = null;

            for (int i = 0; i < 4; i++)
            {
                cube = new CubeGeometry(sizes[i]);
                mesh = new YnMeshGeometry(cube, new BasicMaterial(_level.BorderWallTexture));
                mesh.Position = positions[i];
                mesh.Name = "WALL";
                mesh.TextureRepeat = new Vector2(8, 3);
                mesh.LoadContent();
                _groupWalls.Add(mesh);
            }
        }

        /// <summary>
        /// Génération des blocs du niveau.
        /// </summary>
        private void GenerateCubeWalls()
        {
            YnMeshGeometry wall = null;
            AnimatedItemMesh item = null;

            string texture = _level.WallTexture;
            Vector3 sizes = new Vector3(_level.BlockSizes.Width, _level.BlockSizes.Height, _level.BlockSizes.Depth);
            Vector3 position = Vector3.Zero;

            for (int y = 0; y < _level.Depth; y++)
            {
                for (int x = 0; x < _level.Width; x++)
                {
                    position.X = (_level.BlockSizes.Width / 2) + x * _level.BlockSizes.Width * 2;
                    position.Y = _level.BlockSizes.Height;
                    position.Z = (_level.BlockSizes.Depth / 2) + y * _level.BlockSizes.Depth * 2;

                    if (_levelTiles[x, y] == 2)
                    {
                        wall = new YnMeshGeometry(new CubeGeometry(sizes), texture);
                        wall.Position = position;
                        wall.Name = "WALL";
                        wall.LoadContent();
                        wall.Initialize();
                        _groupWalls.Add(wall);
                        wall.UpdateBoundingVolumes();
                    }
                    else if (_levelTiles[x, y] == 5 || _levelTiles[x, y] == 6)
                    {
                        item = new AnimatedItemMesh("Models/crystals_m");
                        item.Position = position;
                        item.LoadContent();
                        item.Initialize();
                        item.Y = 1;
                        item.Points = _levelTiles[x, y] == 6 ? 25 : 15;
                        _groupItems.Add(item);
                        item.UpdateBoundingVolumes();
                    }
                    else if (_levelTiles[x, y] == 8)
                    {
                        _endGeometry = new YnMeshGeometry(new IcoSphereGeometry(1, 2, false), _level.FinishTexture);
                        _endGeometry.Scale = new Vector3(4);
                        _endGeometry.Position = position;
                        _endGeometry.Name = "END";
                        _endGeometry.LoadContent();
                        _endGeometry.Initialize();
                        _groupWalls.Add(_endGeometry);
                        _endGeometry.UpdateBoundingVolumes();
                    }
                    else if (_levelTiles[x, y] == 9)
                    {
                        _startPosition = position;
                        _startPosition.Y = 10;
                    }
                }
            }
        }
    }
}
