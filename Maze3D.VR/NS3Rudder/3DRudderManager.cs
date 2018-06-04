using ns3DRudder;
using System;
using System.Threading;

namespace NS3DRudder
{
    public class s3DRudderManager : CSdk
    {
        private const int ThreadSleepMs = 100;
        public static readonly int SDKMaxDevice = i3DR._3DRUDDER_SDK_MAX_DEVICE;
        public static readonly int SDKVersion = i3DR._3DRUDDER_SDK_VERSION;
        private static s3DRudderManager s_Instance;
        private Thread m_Thread;
        private Rudder[] m_Rudders;
        private bool[] m_IsConnected;
        
        private bool m_Quit = false;

        public static s3DRudderManager Instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new s3DRudderManager();

                return s_Instance;
            }
        }

        public EventRudder Events { get; private set; }

        private s3DRudderManager()
        {
            Console.WriteLine("init s3DRudderManager");
       
            Init();

            // Init States
            m_Rudders = new Rudder[SDKMaxDevice];
            m_IsConnected = new bool[SDKMaxDevice];

            for (uint i = 0; i < m_Rudders.Length; ++i)
            {
                m_Rudders[i] = new Rudder(i, this);
                m_IsConnected[i] = false;
            }

            Console.WriteLine("SDK version : {0:X4}" , GetSDKVersion());
          
            Events = new EventRudder();
            Events.OnConnectEvent += (portNumber) => Console.WriteLine("3dRudder {0} connected, firmware : {1:X4}", portNumber, GetVersion(portNumber));
            Events.OnDisconnectEvent += (portNumber) => Console.WriteLine("3dRudder {0} disconnected, firmware : {1:X4}", portNumber, GetVersion(portNumber));
            SetEvents(Events);
        }

        private void Update()
        {
            while (!m_Quit)
            {
                for (uint i = 0; i < m_Rudders.Length; ++i)
                {
                    var isConnected = m_Rudders[i].IsDeviceConnected();
         
                    if (isConnected != m_IsConnected[i])
                    {
                        if (isConnected)
                            Events.OnConnect(i);
                        else
                            Events.OnDisconnect(i);

                        m_IsConnected[i] = isConnected;
                    }
                }
                // 10 FPS = 100 ms
                Thread.Sleep(ThreadSleepMs);
            }
        }

        public void ShutDown() => Dispose();

        public override void Dispose()
        {
            m_Quit = true;
            m_Thread.Join();

            ClearEvents();
        
            base.Dispose();

            foreach (Rudder r in m_Rudders)
                r.Dispose();

            s_Instance = null;
  
            GC.SuppressFinalize(this);
        }
    
        private void SetEvents(EventRudder events)
        {
            if (m_Thread == null)
            {
                m_Thread = new Thread(Update);
                m_Thread.Start();
            }

            Events = events;
        }

        private void ClearEvents()
        {
            if (m_Thread != null)
                m_Thread.Abort();

            Events.Dispose();
            Events = null;
        }

        public Rudder GetRudder(int portNumber) => m_Rudders[portNumber];
    }
}