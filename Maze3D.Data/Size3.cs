using System;

namespace Maze3D.Data
{
    [Serializable]
    public class Size3
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public Size3()
        {
        }

        public Size3(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
}
