using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Classes {
    public class Vector3 : IDarkRiftSerializable {
        public float x = 0;
        public float y = 0;
        public float z = 0;

        public Vector3() {

        }

        public Vector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Deserialize(DeserializeEvent e) {
            x = e.Reader.ReadSingle();
            y = e.Reader.ReadSingle();
            z = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(x);
            e.Writer.Write(y);
            e.Writer.Write(z);
        }
    }
}
