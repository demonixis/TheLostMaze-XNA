using Microsoft.Xna.Framework;
using System;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Material;

namespace Maze3D
{
    public enum AnimationState
    {
        FadeIn = 0, FadeOut
    }

    public class AnimatedItemMesh : YnMeshModel
    {
        private float _minY;
        private float _maxY;
        private int _yDirection;
        private int _points;

        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public AnimatedItemMesh(string assetName)
            : base(assetName)
        {
            _points = 15;
            _minY = 0.0f;
            _maxY = 2.5f;
            _yDirection = 1;

            _rotation.X = (float)-Math.PI / 2;

            Name = "ITEM";
        }

        public override void LoadContent()
        {
            base.LoadContent();

            BasicMaterial material = (BasicMaterial)_material;

            if (_points == 15)
                material.EmissiveColor = Color.Blue.ToVector3();
            else
                material.EmissiveColor = Color.Green.ToVector3();

            material.EmissiveIntensity = 1.3f;
            material.AmbientIntensity = 1.2f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Y >= _maxY)
                _yDirection = -1;

            else if (Y <= _minY)
                _yDirection = 1;

            Y += 0.0008f * gameTime.ElapsedGameTime.Milliseconds * _yDirection;

            RotateY(1);
        }
    }
}
