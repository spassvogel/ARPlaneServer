using ARPlaneServer.Classes;
using DarkRift;
using System.Collections.Generic;
using System.Linq;

namespace ARPlaneServer.Events {
    class PlayerStatesEvent : NetworkEvent {
        public IEnumerable<Player> players;

        public override void Deserialize(DeserializeEvent e) {
            players = e.Reader.ReadSerializables<Player>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(players.ToArray());
        }
    }
}
