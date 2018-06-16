using Maze3D.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Yna.Engine.Content;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Geometries;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Materials;
using Yna.Engine.Graphics3D.Terrains;

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
        public Vector3 StartPosition => _startPosition;

        /// <summary>
        /// Obtient le tableau de structure du niveau.
        /// </summary>
        public int[,] Tiles => _levelTiles;

        /// <summary>
        /// Obtient le numéro de niveau.
        /// </summary>
        public new int Id => _levelId;

        /// <summary>
        /// Obtient l'instance Level utilisé pour générer ce niveau.
        /// </summary>
        public Level Level => _level;

        /// <summary>
        /// Obtient la collection des murs.
        /// </summary>
        public YnGroup3D Walls => _groupWalls;

        /// <summary>
        /// Obtient la collection des éléments à ramasser.
        /// </summary>
        public YnGroup3D Items => _groupItems;

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
            GenerateCubeWalls(true);
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
            if (_level.SkyboxType == SkyboxType.None)
                return;

            var skyAssets = _level.SkyboxType == SkyboxType.Day ? Assets.SkyboxLand : Assets.SkyboxNight;

            _skybox = new SkyBox(Math.Max(_level.Width * _level.BlockSizes.Width, _level.Depth * _level.BlockSizes.Depth) * 4.5f, skyAssets);
            _skybox.LoadContent();
            _skybox.Position = new Vector3((_level.Width * _level.BlockSizes.Width), 0, (_level.Depth * _level.BlockSizes.Depth));
            Add(_skybox);
        }

        private void GenerateGrounds()
        {
            var enableSkybox = _level.SkyboxType != SkyboxType.None;
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

            var terrain = new Terrain(_level.GroundTexture, _level.Width * _level.BlockSizes.Width, _level.Depth * _level.BlockSizes.Depth, 4, 4);
            terrain.Geometry.TextureRepeat = new Vector2(24);
            terrain.Position = new Vector3((_level.Width * _level.BlockSizes.Width), 0, (_level.Depth * _level.BlockSizes.Depth)) * -1.0f;
            Add(terrain);
#if TEST
            var planG2 = new PlaneGeometry(new Vector3(_level.Width * _level.BlockSizes.Width, 0, _level.Depth * _level.BlockSizes.Depth));
            planG2.TextureRepeat = new Vector2(24);

            var ground = new YnMeshGeometry(planG2, _level.GroundTexture);
            ground.LoadContent();
            ground.Position = new Vector3((_level.Width * _level.BlockSizes.Width), 0, (_level.Depth * _level.BlockSizes.Depth));
            Add(ground);
#endif
        }

        /// <summary>
        /// Génération des murs de bordure.
        /// </summary>
        private void GenerateBorderWalls()
        {
            var sizes = new Vector3[4]
            {
                new Vector3(_level.WorldWidth, _level.BlockSizes.Height, 1),
                new Vector3(_level.WorldWidth, _level.BlockSizes.Height, 1),
                new Vector3(1, _level.BlockSizes.Height, _level.WorldDepth),
                new Vector3(1, _level.BlockSizes.Height, _level.WorldDepth)
            };

            var positions = new Vector3[4]
            {
                new Vector3(_level.WorldWidth, _level.BlockSizes.Height, 0),
                new Vector3(_level.WorldWidth, _level.BlockSizes.Height, 2 * _level.WorldDepth),
                new Vector3(0, _level.BlockSizes.Height, _level.WorldWidth),
                new Vector3(2 * _level.WorldDepth, _level.BlockSizes.Height, _level.WorldWidth)
            };

            var colors = new Color[4]
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
        private void GenerateCubeWalls(bool mergeWalls)
        {
            YnMeshGeometry wall = null;
            AnimatedItemMesh item = null;

            var sizes = new Vector3(_level.BlockSizes.Width, _level.BlockSizes.Height, _level.BlockSizes.Depth);
            var position = Vector3.Zero;
            var walls = new List<YnMeshGeometry>();
            var wallMaterial = new BasicMaterial(_level.WallTexture);

            for (int y = 0; y < _level.Depth; y++)
            {
                for (int x = 0; x < _level.Width; x++)
                {
                    position.X = (_level.BlockSizes.Width / 2) + x * _level.BlockSizes.Width * 2;
                    position.Y = _level.BlockSizes.Height;
                    position.Z = (_level.BlockSizes.Depth / 2) + y * _level.BlockSizes.Depth * 2;

                    if (_levelTiles[x, y] == 2)
                    {
                        wall = new YnMeshGeometry(new CubeGeometry(sizes), wallMaterial);
                        wall.Position = position;
                        wall.Name = "WALL";
                        wall.LoadContent();
                        wall.Initialize();
                        _groupWalls.Add(wall);
                        wall.UpdateBoundingVolumes();
                        wall.Visible = !mergeWalls;
                        walls.Add(wall);
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

            if (mergeWalls)
            {
                var merged = Geometry.MergeMeshes(walls.ToArray());
                wall = new YnMeshGeometry(merged, wallMaterial);
                Add(wall);
            }
        }
    }
}
