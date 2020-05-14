using System;
using ARPlaneServer;
using DarkRift;

namespace ExtensionMethods
{
    public static class DarkRiftWriterExtensions
    {
        public static void Write(this DarkRiftWriter writer, Vector3 vector)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }
    }
}
