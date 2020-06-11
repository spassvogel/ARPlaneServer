using ARPlaneServer.Classes;
using DarkRift;
using System.Collections.Generic;
using System.Linq;

namespace ARPlaneServer.Events {
    public class ARCraftStatesEvent : NetworkEvent, IDarkRiftSerializable {
        public IEnumerable<ARCraft> otherCrafts;

        public override void Deserialize(DeserializeEvent e) {
            otherCrafts = e.Reader.ReadSerializables<ARCraft>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(otherCrafts.ToArray());
        }
    }
}
