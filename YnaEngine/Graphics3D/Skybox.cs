using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D
{
    public class Skybox
    {
        private SkyboxMaterial m_ShaderMaterial;
        private Matrix m_World;
        private Matrix _scaleMatrix;
        private CubeGeometry m_Geometry;
        private TextureCube m_MainTexture;
        private RasterizerState m_SkyboxRasterizerState;
        private RasterizerState m_CurrentRasterizerState;

        public TextureCube Texture
        {
            get { return m_MainTexture; }
            set { m_MainTexture = value; }
        }

        public Matrix WorldMatrix => m_World;
        public bool FogSupported { get; set; } = false;
        public bool Enabled { get; set; }

        public Skybox()
        {
            m_Geometry = new CubeGeometry();
            m_World = Matrix.Identity;
            _scaleMatrix = Matrix.CreateScale(1.0f);
            m_SkyboxRasterizerState = new RasterizerState();
            m_SkyboxRasterizerState.CullMode = CullMode.None;
        }

        public void Generate(GraphicsDevice device, Texture2D[] textures, float size = 250.0f)
        {
            if (textures.Length != 6)
                throw new Exception("The array of texture names must contains 6 elements.");

            m_Geometry.Size = new Vector3(size);
            m_Geometry.GenerateGeometry();

            m_MainTexture = new TextureCube(device, textures[0].Width, false, SurfaceFormat.Color);
            Color[] textureData;

            for (int i = 0; i < 6; i++)
            {
                textureData = new Color[textures[i].Width * textures[i].Height];
                textures[i].GetData<Color>(textureData);
                m_MainTexture.SetData<Color>((CubeMapFace)i, textureData);
            }

            Enabled = true;
        }

        public void Generate(GraphicsDevice device, string[] textureNames, float size = 250.0f)
        {
            var textures = new Texture2D[6];

            for (int i = 0; i < 6; i++)
                textures[i] = YnG.Content.Load<Texture2D>(textureNames[i]);

            Generate(device, textures, size);
        }

        public void Generate(float size = 250.0f)
        {
            var skyTop = YnGraphics.CreateTexture(new Color(168, 189, 255), 64, 64);
            var skySide = YnGraphics.CreateGradiantTexture(new Color(168, 189, 255), Color.White, 64, 64);
            var skyBottom = YnGraphics.CreateTexture(Color.White, 64, 64);

            Generate(YnG.GraphicsDevice, new Texture2D[] {
                skySide,
                skySide,
                skyTop,
                skyBottom,
                skySide,
                skySide
            }, size);
        }

        public void Draw(GraphicsDevice device, Camera camera)
        {
            m_CurrentRasterizerState = device.RasterizerState;
            device.RasterizerState = m_SkyboxRasterizerState;

            m_World = _scaleMatrix * Matrix.CreateTranslation(camera.Position);

            //m_ShaderMaterial.PrePass(camera);

            device.SetVertexBuffer(m_Geometry.VertexBuffer);
            device.Indices = m_Geometry.IndexBuffer;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_Geometry.Indices.Length / 3);
            device.RasterizerState = m_CurrentRasterizerState;
        }

        public void DrawNoEffect(GraphicsDevice device)
        {
            m_CurrentRasterizerState = device.RasterizerState;
            device.RasterizerState = m_SkyboxRasterizerState;
            device.SetVertexBuffer(m_Geometry.VertexBuffer);
            device.Indices = m_Geometry.IndexBuffer;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_Geometry.Indices.Length / 3);
            device.RasterizerState = m_CurrentRasterizerState;
        }
    }
}
