using Microsoft.Xna.Framework;
using System;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Materials;

namespace Maze3D
{
    public enum AnimationState
    {
        FadeIn = 0, FadeOut
    }

    public class AnimatedItemMesh : YnModel
    {
        private float _minY;
        private float _maxY;
        private int _yDirection;
        private BasicMaterial _basicMaterial;

        public int Points { get; set; } = 15;

        public AnimatedItemMesh(string assetName)
            : base(assetName)
        {
            _minY = 0.0f;
            _maxY = 2.5f;
            _yDirection = 1;
            _rotation.X = (float)-Math.PI / 2;
            Name = "ITEM";
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _basicMaterial = (BasicMaterial)_material;

            if (Points == 15)
                _basicMaterial.EmissiveColor = Color.Blue.ToVector3();
            else
                _basicMaterial.EmissiveColor = Color.Green.ToVector3();

            _basicMaterial.EmissiveIntensity = 1.3f;
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
