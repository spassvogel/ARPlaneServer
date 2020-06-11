using ARPlaneServer.Classes;
using DarkRift;

namespace ARPlaneServer.Events {
    public class PlayerUpdateEvent : NetworkEvent {
        public Player newPlayerState;

        public override void Deserialize(DeserializeEvent e) {
            newPlayerState = e.Reader.ReadSerializable<Player>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(newPlayerState);
        }
    }
}
