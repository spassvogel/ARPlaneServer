using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Classes {
    // TODO maybe we need a better name to prevent ambiguity with UnityEngine.GameObject
    public class GameObject : IDarkRiftSerializable {
        public string id;
        public ushort ownerID;
        public string type; // can be used e.g. to find the right prefab
        public Vector3 position = new Vector3(0,0,0);
        public Vector3 rotation = new Vector3(0,0,0);

        public void Deserialize(DeserializeEvent e) {
            id = e.Reader.ReadString();
            ownerID = e.Reader.ReadUInt16();
            type = e.Reader.ReadString();
            position = e.Reader.ReadSerializable<Vector3>();
            rotation = e.Reader.ReadSerializable<Vector3>();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(id ?? "");
            e.Writer.Write(ownerID);
            e.Writer.Write(type ?? "");
            e.Writer.Write(position);
            e.Writer.Write(rotation);
        }
    }
}
