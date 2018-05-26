using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze3D.Data
{
#if !NETFX_CORE && !WINDOWS_PHONE
    [Serializable]
#endif
    public class Size3
    {
        public int Width;
        public int Height;
        public int Depth;

        public Size3()
        {
            Width = 0;
            Height = 0;
            Depth = 0;
        }

        public Size3(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
}
