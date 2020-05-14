using DarkRift;

namespace ARPlaneServer.Events {
    public abstract class NetworkEvent : IDarkRiftSerializable {
        public abstract void Deserialize(DeserializeEvent e);
        public abstract void Serialize(SerializeEvent e);
    }
}
