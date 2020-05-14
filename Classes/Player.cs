using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Classes {
    public class Player : IDarkRiftSerializable {
        public ushort id;
        public string name;

        public void Deserialize(DeserializeEvent e) {
            id = e.Reader.ReadUInt16();
            name = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(id);
            e.Writer.Write(name);
        }
    }
}
