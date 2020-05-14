using ARPlaneServer.Classes;
using DarkRift;

namespace ARPlaneServer.Events {
    public class ARCraftSpawnEvent : NetworkEvent, IDarkRiftSerializable {
        public Player player;
        public ARCraft arCraft;

        public override void Deserialize(DeserializeEvent e) {
            player = e.Reader.ReadSerializable<Player>();
            arCraft = e.Reader.ReadSerializable<ARCraft>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(player);
            e.Writer.Write(arCraft);
        }
    }
}
