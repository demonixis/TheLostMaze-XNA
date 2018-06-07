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
        private CurveArray m_curveArray;

        private ns3DRudder.Curve m_curve_roll ; 
        private ns3DRudder.Curve m_curve_pitch ;
        private ns3DRudder.Curve m_curve_updown ;
        private ns3DRudder.Curve m_curve_yaw ;


        private ModeAxis m_ModeAxis;

        public bool Connected { get; private set; }

        public event Action<bool> ConnectionChanged = null;

        public RudderController()
        {
            m_Axis = new Axis();
            m_curveArray = new CurveArray();
            m_Rudder = s3DRudderManager.Instance.GetRudder(0);
            ushort vvSDK = s3DRudderManager.Instance.GetSDKVersion();
            m_ModeAxis = ModeAxis.ValueWithCurveNonSymmetricalPitch;

          
            //-- Sensivity (angle Speed Max) 
            //-- Value normalized [0,1]
            //--------------------------------------
            float XSat_roll     = 0.6f ;  // 60%
            float XSat_pitch    = 0.4f;
            float XSat_yaw      = 0.8f;
            float XSat_updown   = 0.0f;               // Not used in this game


            //-- Death zone (%)
            //--------------------------------------
            float deathzone_roll    = 0.25f * XSat_roll; // 10 %
            float deathzone_pitch   = 0.25f * XSat_pitch;
            float deathzone_yaw     = 0.1f * XSat_yaw;
            float deathzone_upDown  = 0.0f; // Not used in this game

            //-- Define  curves
            //--------------------------------------
            m_curve_roll    = new ns3DRudder.Curve(deathzone_roll, XSat_roll, 1.0f, 1.0f);
            m_curve_pitch   = new ns3DRudder.Curve(deathzone_pitch, XSat_pitch, 1.0f, 1.0f);
            m_curve_yaw     = new ns3DRudder.Curve(deathzone_yaw, XSat_yaw, 1.0f, 1.0f);
            m_curve_updown  = new ns3DRudder.Curve(deathzone_upDown, XSat_updown, 1.0f, 2.0f); // Not used in this game

            m_curveArray.SetCurve(CurveType.CurvePitch, m_curve_pitch) ;
            m_curveArray.SetCurve(CurveType.CurveRoll, m_curve_roll) ;
            m_curveArray.SetCurve(CurveType.CurveYaw, m_curve_yaw) ;
            m_curveArray.SetCurve(CurveType.CurveUpDown, m_curve_updown) ;

        }

        public void UpdateTransform(ref Vector3 translation, ref Vector3 rotation,float speed_trans, float speed_rot, bool updateUpTranslation = true)
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


            //m_Axis = m_Rudder.GetAxis(m_ModeAxis);
            m_Axis = m_Rudder.GetAxisWithCurve(m_ModeAxis,m_curveArray);

            /*
            translation.X += -m_Axis.GetXAxis();
            translation.Z += m_Axis.GetYAxis();
            */

            translation.X += -m_Axis.GetXAxis() * speed_trans;
            translation.Z += m_Axis.GetYAxis() * speed_trans;
            
            if (updateUpTranslation)
            {
                var y = m_Axis.GetZAxis();
                if (Math.Abs(y) > 0.65f)
                    translation.Y += y;
            }

            rotation.Y += -m_Axis.GetZRotation()* speed_rot;
            
        }
    }
}