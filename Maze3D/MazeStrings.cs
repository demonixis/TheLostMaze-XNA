using System;
using System.Collections.Generic;
using Maze3D.Translation;

namespace Maze3D
{
    public sealed class MazeStrings
    {
        public const string YnaProjectPage = "http://yna.codeplex.com";
        public const string MonoGameProjectPage = "http://www.monogame.net";
        public static Dictionary<string, string> GameTexts { get; protected set; }

        public static void InitializeForFrench()
        {
            GameTexts = new MazeStringsFR().GameTexts;
        }

        public static void InitializeForEnglish()
        {
            GameTexts = new MazeStringsEN().GameTexts;
        }
    }
}
