using ARPlaneServer.Classes;
using DarkRift;

namespace ARPlaneServer.Events {
    public class ObjectUpdateEvent : NetworkEvent, IDarkRiftSerializable {
        public GameObject newState;

        public override void Deserialize(DeserializeEvent e) {
            newState = e.Reader.ReadSerializable<GameObject>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(newState);
        }
    }
}
