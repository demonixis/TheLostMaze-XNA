using Microsoft.Xna.Framework;
using ns3DRudder;
using NS3DRudder;
using System;

namespace Maze3D.VR
{
    public sealed class RudderController
    {
        private Axis m_Axis;
        private Rudder m_Rudder;
        private ModeAxis m_ModeAxis;

        public bool Connected { get; private set; }

        public event Action<bool> ConnectionChanged = null;

        public RudderController()
        {
            m_Axis = new Axis();
            m_Rudder = s3DRudderManager.Instance.GetRudder(0);
            m_ModeAxis = ModeAxis.NormalizedValue;
        }

        public void UpdateTransform(ref Vector3 translation, ref Vector3 rotation, bool updateUpTranslation = true)
        {
            if (!s3DRudderManager.Instance.IsDeviceConnected(0))
                return;

            if (m_Rudder == null)
                m_Rudder = s3DRudderManager.Instance.GetRudder(0);

            if (m_Rudder == null)
                return;

            if (!Connected)
            {
                if (m_Rudder.IsDeviceConnected())
                {
                    Connected = true;
                    ConnectionChanged?.Invoke(true);
                }
            }

            m_Axis = m_Rudder.GetAxis(m_ModeAxis);

            translation.X += -m_Axis.GetXAxis();
            translation.Z += m_Axis.GetYAxis();

            if (updateUpTranslation)
            {
                var y = m_Axis.GetZAxis();
                if (Math.Abs(y) > 0.65f)
                    translation.Y += y;
            }

            rotation.Y += -m_Axis.GetZRotation();
        }
    }
}