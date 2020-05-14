using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Events {
    public abstract class NetworkEvent : IDarkRiftSerializable {
        public abstract void Deserialize(DeserializeEvent e);
        public abstract void Serialize(SerializeEvent e);
    }
}
