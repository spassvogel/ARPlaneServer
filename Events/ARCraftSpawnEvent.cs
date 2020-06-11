using ARPlaneServer.Classes;
using DarkRift;

namespace ARPlaneServer.Events {
    public class ARCraftSpawnEvent : NetworkEvent, IDarkRiftSerializable {
        public ARCraft arCraft;

        public override void Deserialize(DeserializeEvent e) {
            arCraft = e.Reader.ReadSerializable<ARCraft>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(arCraft);
        }
    }
}
