using ARPlaneServer.Classes;
using DarkRift;

namespace ARPlaneServer.Events {
    class PlayerConnectedEvent : NetworkEvent {
        public Player player;

        public override void Deserialize(DeserializeEvent e) {
            player = e.Reader.ReadSerializable<Player>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(player);
        }
    }
}
