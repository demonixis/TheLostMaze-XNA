using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yna.Engine.Graphics3D.Cameras;

namespace Yna.Engine.Graphics3D.Materials
{
    public class SkyboxMaterial : Material
    {
        private Skybox _skybox;
        private EffectPass m_DefaultPass;
        private EffectParameter m_EPWorld;
        private EffectParameter m_EPView;
        private EffectParameter m_EPProjection;
        private EffectParameter m_EPMainTexture;
        private EffectParameter m_EPEyePosition;
        private EffectParameter m_EPFogEnabled;
        private EffectParameter m_EPFogColor;
        private EffectParameter m_EPFogData;

        public SkyboxMaterial(Skybox skybox)
        {

        }

        public override void LoadContent()
        {
            if (_effectLoaded)
                return;

            var suffix = "ogl";
#if DIRECTX
            suffix = "dx11";
#endif
#if WINDOWS_STOREAPP
            var assembly = typeof(NormalMapMaterial).GetTypeInfo().Assembly;
#else
            var assembly = Assembly.GetExecutingAssembly();
#endif
            var stream = assembly.GetManifestResourceStream($"Yna.Engine.Graphics3D.Material.Resources.Skybox_{suffix}.mgfxo");
            byte[] shaderCode;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                shaderCode = ms.ToArray();
            }

            _effect = new Effect(YnG.GraphicsDevice, shaderCode);
            _effectName = "Skybox";
            _effectLoaded = true;
        }

        public override void Update(Camera camera, ref Matrix world)
        {
           
        }
    }
}
