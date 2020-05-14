using ARPlaneServer.Classes;
using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Events {
    class PlayerDisconnectedEvent : NetworkEvent {
        public Player player;

        public override void Deserialize(DeserializeEvent e) {
            player = e.Reader.ReadSerializable<Player>();
        }

        public override void Serialize(SerializeEvent e) {
            e.Writer.Write(player);
        }
    }
}
