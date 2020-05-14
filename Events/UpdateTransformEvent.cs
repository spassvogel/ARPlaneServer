using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Events {
    class UpdateTransformEvent : IDarkRiftSerializable {
        public string id;
        public Vector3 position;
        public Vector3 rotation;

        public void Deserialize(DeserializeEvent e) {
            id = e.Reader.ReadString();
            position = e.Reader.ReadSerializable<Vector3>();
            rotation = e.Reader.ReadSerializable<Vector3>();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(id);
            e.Writer.Write(position);
            e.Writer.Write(rotation);
        }
    }
}
