// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Cameras;

namespace Yna.Engine.Graphics3D
{
    public struct Intersection
    {
        public float Distance;
        public YnEntity3D Object3D;
    }

    public class YnG3
    {
        #region Get 2D/3D coordinates

        /// <summary>
        /// Get 2D position (on screen) from 3D position (on world)
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="position">Position 3D</param>
        /// <returns>The screen position</returns>
        public static Vector2 GetWorldToScreenPosition(Camera camera, ref Vector3 position)
        {
            var p2d = YnG.GraphicsDevice.Viewport.Project(position, camera.Projection, camera.View, Matrix.Identity);
            return new Vector2(p2d.X, p2d.Y);
        }

        /// <summary>
        /// Get 3D world position of from the 2D position (on screen)
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="position">Position on world</param>
        /// <returns>Position on 3D world</returns>
        public static Vector3 GetScreenToWorldPosition(Camera camera, ref Vector2 position)
        {
            return YnG.GraphicsDevice.Viewport.Unproject(new Vector3(position, 0.0f), camera.Projection, camera.View, Matrix.Identity);
        }

        #endregion

        #region Mouse cursor collision

        /// <summary>
        /// Get a Ray from the mouse coordinate
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <returns>A ray</returns>
        public static Ray GetMouseRay(Cameras.Camera camera)
        {
            var nearPoint = new Vector3(YnG.Mouse.Position, 0);
            var farPoint = new Vector3(YnG.Mouse.Position, 1);

            nearPoint = YnG.GraphicsDevice.Viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
            farPoint = YnG.GraphicsDevice.Viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

            // Get the direction
            var direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Get the distance between the mouse cursor and an object3D
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="object3D">Object3D</param>
        /// <returns>The distance between the object and the mouse cursor, -1 if not collide</returns>
        public static float MouseCollideWithObject(Cameras.Camera camera, YnEntity3D object3D)
        {
            float? distance = null;

            if (!object3D.Static)
                object3D.UpdateBoundingVolumes();

            distance = GetMouseRay(camera).Intersects(object3D.BoundingBox);

            return distance != null ? (float)distance : -1.0f;
        }

        /// <summary>
        /// Check if the mouse cursor collide with a group of object on the scene
        /// </summary>
        /// <param name="camera">Active camera</param>
        /// <param name="group">Group of object</param>
        /// <returns></returns>
        public static Intersection[] MouseCollideWithGroup(Cameras.Camera camera, YnGroup3D group)
        {
            var collides = new List<Intersection>();
            var groupSize = group.Count;

            for (var i = 0; i < groupSize; i++)
            {
                var distance = MouseCollideWithObject(camera, group[i]);
                if (distance > -1)
                    collides.Add(new Intersection() { Object3D = group[i], Distance = distance });
            }

            return collides.ToArray();
        }

        #endregion

        #region Models/Camera collision

        /// <summary>
        /// check if two models colliding
        /// </summary>
        /// <param name="modelA">First model</param>
        /// <param name="modelB">Second model</param>
        /// <returns>True if modelA collinding modelB else false</returns>
        public static bool SphereCollide(YnMeshModel modelA, YnMeshModel modelB)
        {
            var collide = false;
            var j = 0;
            var countMeshA = modelA.Model.Meshes.Count;
            var countMeshB = modelB.Model.Meshes.Count;

            for (var i = 0; i < countMeshA; i++)
            {
                var meshABS = modelA.Model.Meshes[i].BoundingSphere;
                meshABS.Center += modelA.Position;

                while (j < countMeshB && !collide)
                {
                    var meshBBS = modelB.Model.Meshes[j].BoundingSphere;
                    meshBBS.Center += modelB.Position;

                    if (meshABS.Intersects(meshBBS))
                        collide = true;

                    j++;
                }
            }

            return collide;
        }

        /// <summary>
        /// Test if the model colliding another objects
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="group">A collection of models</param>
        /// <returns>Array of models that collides with model</returns>
        public static YnMeshModel[] SphereCollide(YnMeshModel model, YnGroup3D group)
        {
            var collides = new List<YnMeshModel>();
            var groupSize = group.Count;

            for (var i = 0; i < groupSize; i++)
                if (group[i] is YnMeshModel && SphereCollide(model, group[i] as YnMeshModel))
                    collides.Add(group[i] as YnMeshModel);

            return collides.ToArray();
        }

        public static YnEntity3D[] SphereCollide(Cameras.Camera camera, YnGroup3D group)
        {
            var collides = new List<YnEntity3D>();
            var groupSize = group.Count;

            for (var i = 0; i < groupSize; i++)
            {
                if (camera.BoundingSphere.Intersects(group[i].BoundingSphere))
                    collides.Add(group[i]);
            }

            return collides.ToArray();
        }

        /// <summary>
        /// check if two models colliding with there bounding box
        /// </summary>
        /// <param name="modelA">First model</param>
        /// <param name="modelB">Second model</param>
        /// <returns>True if modelA collinding modelB else false</returns>
        public static bool CubeCollide(YnMeshModel modelA, YnMeshModel modelB)
        {
            if (!modelA.Static)
                modelA.UpdateBoundingVolumes();

            if (!modelB.Static)
                modelB.UpdateBoundingVolumes();

            return modelA.BoundingBox.Intersects(modelB.BoundingBox);
        }

        /// <summary>
        /// Test if the model colliding another objects with there bounding box
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="group">A collection of models</param>
        /// <returns>Array of models that collides with model</returns>
        public static YnMeshModel[] CubeCollide(YnMeshModel model, YnGroup3D group)
        {
            var collides = new List<YnMeshModel>();
            var groupSize = group.Count;

            for (var i = 0; i < groupSize; i++)
            {
                if (group[i] is YnMeshModel)
                {
                    if (CubeCollide(model, group[i] as YnMeshModel))
                        collides.Add(group[i] as YnMeshModel);
                }
            }

            return collides.ToArray();
        }

        public static YnEntity3D[] CubeCollide(Cameras.Camera camera, YnGroup3D group)
        {
            var collides = new List<YnEntity3D>();
            var groupSize = group.Count;

            for (var i = 0; i < groupSize; i++)
            {
                if (camera.BoundingBox.Intersects(group[i].BoundingBox))
                    collides.Add(group[i]);
            }

            return collides.ToArray();
        }

        #endregion

        public static void RestoreGraphicsDeviceStates()
        {
            YnG.GraphicsDevice.BlendState = BlendState.Opaque;
            YnG.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            YnG.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
}
