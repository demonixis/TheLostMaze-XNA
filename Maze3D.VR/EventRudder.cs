using ns3DRudder;
using System;

namespace NS3DRudder
{
    public class EventRudder : IEvent
    {
        public Action<uint> OnConnectEvent;
        public Action<uint> OnDisconnectEvent;

        public override void OnConnect(uint nDeviceNumber) => OnConnectEvent?.Invoke(nDeviceNumber);
        public override void OnDisconnect(uint nDeviceNumber) => OnDisconnectEvent(nDeviceNumber);

        public override void Dispose()
        {
            base.Dispose();
            OnConnectEvent = null;
            OnDisconnectEvent = null;
        }
    }
}
