using ARPlaneServer.Classes;
using DarkRift;
using System.Collections.Generic;
using System.Linq;

namespace ARPlaneServer.Events {
    class ObjectStatesEvent : NetworkEvent {
        public IEnumerable<GameObject> objects;

        public override void Deserialize(DeserializeEvent e) {
            objects = e.Reader.ReadSerializables<ARCraft>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(objects.ToArray());
        }
    }
}
