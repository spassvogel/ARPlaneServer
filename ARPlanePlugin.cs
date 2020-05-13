using DarkRift;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARPlaneServer {
    class ARPlanePlugin : Plugin {
        class Craft {
            public string name;
            public Vector3 position;
            public Vector3 rotation;
        }

        private static void WriteVector3(DarkRiftWriter writer, Vector3 vector) {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }

        private static Vector3 ReadVector3(DarkRiftReader reader) {
            return new Vector3(
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle()
            );
        }

        Dictionary<IClient, Craft> crafts = new Dictionary<IClient, Craft>();

        public ARPlanePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData) {
            ClientManager.ClientConnected += ClientConnected;
            ClientManager.ClientDisconnected += ClientDisconnected;

            
        }
        public override bool ThreadSafe => false;
        public override Version Version => new Version(1, 0, 0);

        void ClientConnected(object sender, ClientConnectedEventArgs e) {
            Print($"Client connected {e.Client.ID}");
            InitializeClient(e.Client);
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
            Print($"Client connected {e.Client.ID}");
        }

        void ClientMessageReceived(object sender, MessageReceivedEventArgs e) {
            // TODO
        }

        void InitializeClient(IClient client) {
            crafts.Add(client, new Craft());

            SendSpawnCraft(client);
            SendSpawnOtherCrafts(client);

            client.MessageReceived += ClientMessageReceived;
        }

        void SendSpawnCraft(IClient client) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                writer.Write(client.ID);

                using (Message message = Message.Create(Tags.SpawnCraft, writer)) {
                    foreach (IClient otherClient in ClientManager.GetAllClients().Where(x => x != client)) {
                        otherClient.SendMessage(message, SendMode.Reliable);
                    }
                }
            }
        }

        // Tell the new client which aircrafts are already in the game
        void SendSpawnOtherCrafts(IClient client) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                foreach (Craft otherCraft in crafts.Values) {
                    writer.Write(client.ID);
                    WriteVector3(writer, otherCraft.position);
                    WriteVector3(writer, otherCraft.rotation);
                }

                using (Message playerMessage = Message.Create(Tags.SpawnCraft, writer)) {
                    client.SendMessage(playerMessage, SendMode.Reliable);
                }
            }
        }

        // Debugging
        private void Print(string message) {
            WriteEvent(message, LogType.Info);
        }
    }
}
