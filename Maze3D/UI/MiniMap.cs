using Microsoft.Xna.Framework;
using System;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace Maze3D.UI
{
    public enum MapCeilSize
    {
        Small = 0, Normal, Big
    }

    public class MiniMap : YnGroup
    {
        private Color _wallColor;
        private Color _groundColor; 
        private YnSprite _player;
        private int _sizeX;
        private int _sizeY;
        private int _mapWidth;
        private int _mapHeight;
        private int _ceilSize;
        private int[,] _tiles;
        private float _zoomFactor;

        public new int Width
        {
            get { return _mapWidth * _ceilSize; }
        }

        public new int Height
        {
            get { return _mapHeight * _ceilSize; }
        }

        public MiniMap(int[,] tiles, int sizeX, int sizeY)
        {
            _wallColor = new Color(170, 144, 111);
            _groundColor = new Color(213, 209, 205);

            _sizeX = sizeX;
            _sizeY = sizeY;
            _tiles = tiles;
            _mapWidth = tiles.GetLength(0);
            _mapHeight = tiles.GetLength(1);
            _ceilSize = 6;
            _zoomFactor = 0.11f;
            X = YnG.Width - (_mapWidth * _ceilSize);
            Y = 0;
        }

        private void DetermineCeilSize()
        {
            switch (GameConfiguration.MapCeilSize)
            {
                case MapCeilSize.Small: _zoomFactor = 0.09f; break;
                case MapCeilSize.Normal: _zoomFactor = 0.16f; break;
                case MapCeilSize.Big: _zoomFactor = 0.26f; break;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            GenerateMap(null);
        }

        public void GenerateMap(int[,] tiles)
        {
            _tiles = tiles != null ? tiles : _tiles;
            Clear();

            DetermineCeilSize();

            _ceilSize = (int)(YnG.Width / _mapWidth * _zoomFactor);

            X = YnG.Width - (_mapWidth * _ceilSize);
            Y = 0;

            Rectangle rectangle = Rectangle.Empty;
            Color color = _groundColor;

            Rectangle rectanglePlayer = new Rectangle(0, 0, _ceilSize, _ceilSize);

            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    rectangle = new Rectangle((int)X + x * _ceilSize, (int)Y + y * _ceilSize, _ceilSize, _ceilSize);
                    YnSprite ceil = null;

                    switch (_tiles[x, y])
                    {
                        case 1: color = _groundColor; break;
                        case 2: color = _wallColor; break;
                        case 5: color = GameConfiguration.EnabledMinimapItems ? Color.Blue : _groundColor; break;
                        case 6: color = GameConfiguration.EnabledMinimapItems ? Color.Orange : _groundColor; break;
                        case 8: color = Color.Red; break;
                        case 9:
                            rectanglePlayer.X = x;
                            rectanglePlayer.Y = y;
                            color = _groundColor;
                            break;
                    }
                    ceil = new YnSprite(rectangle, color);
                    Add(ceil);
                }
            }

            _player = new YnSprite(rectanglePlayer, Color.Green);
            Add(_player);

            foreach (Yna.Engine.Graphics.YnEntity2D sceneObject in Members)
                sceneObject.Alpha = 0.6f;
        }

        public void UpdatePlayerPosition(float cameraX, float cameraZ)
        {
            int x = Math.Abs((int)Math.Floor(((cameraX / _sizeX) / 2)));
            int y = Math.Abs((int)Math.Floor(((cameraZ / _sizeY) / 2)));

            _player.X = (int)(X + x * _ceilSize);
            _player.Y = (int)(Y + y * _ceilSize);
        }

        public void UpdateItemStatus(float cameraX, float cameraZ)
        {
            int x = Math.Abs((int)Math.Floor(((cameraX / _sizeX) / 2)));
            int y = Math.Abs((int)Math.Floor(((cameraZ / _sizeY) / 2)));

            this[x + y * _mapWidth].Texture = YnGraphics.CreateTexture(_groundColor, _ceilSize, _ceilSize);
        }
    }
}
