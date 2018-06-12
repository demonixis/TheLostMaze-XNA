using System;

namespace Maze3D.Data
{
    [Serializable]
    public enum SkyboxType
    {
        Day = 0, Night, Montain, None
    }

    [Serializable]
    public class Level
    {
        public int Id { get; set; }
        public Size3 BlockSizes { get; set; }
        public string WallTexture { get; set; }
        public string GroundTexture { get; set; }
        public string TopTexture { get; set; }
        public string FinishTexture { get; set; }
        public string BorderWallTexture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public int[] Tiles { get; set; }
        public string TilesStr { get; set; }
        public SkyboxType SkyboxType { get; set; }

        public int WorldWidth => Width * BlockSizes.Width;
        public int WorldHeight => Height * BlockSizes.Height;
        public int WorldDepth => Depth * BlockSizes.Depth;

        public Level()
        {
            Id = 0;
            WallTexture = "Textures/aztec01";
            GroundTexture = "Textures/tarmac02";
            TopTexture = "Textures/slab03";
            FinishTexture = "Textures/tarmac02";
            BorderWallTexture = "Textures/aztec01";
            Width = 0;
            Height = 0;
            Depth = 0;
            Tiles = new int[Width * Depth];
            BlockSizes = new Size3();
            SkyboxType = SkyboxType.None;
            TilesStr = String.Empty;
        }

        public Level(int id) : this() => Id = id;
       
        public void Initialize()
        {
            Tiles = new int[Width * Depth];

            if (TilesStr != String.Empty)
            {
                var tmp = TilesStr.Split(' ');
                var size = tmp.Length;

                if (Tiles.Length != size)
                    throw new Exception("The Array of tiles havn't the same value as the String of tiles.");

                for (int i = 0, l = tmp.Length; i < l; i++)
                    Tiles[i] = int.Parse(tmp[i].ToString());
            }
        }

        public void SetTiles2D(int[,] tiles)
        {
            Width = tiles.GetLength(0);
            Depth = tiles.GetLength(1);

            var tiles1D = new int[Width * Depth];

            Tiles = new int[Width * Depth];

            for (int y = 0; y < Depth; y++)
                for (int x = 0; x < Width; x++)
                    Tiles[x + y * Width] = tiles[x, y];
        }

        public int[,] GetTiles2D()
        {
            var tiles2D = new int[Width, Depth];

            for (int y = 0; y < Depth; y++)
                for (int x = 0; x < Width; x++)
                    tiles2D[x, y] = Tiles[x + y * Width];

            return tiles2D;
        }
    }
}
