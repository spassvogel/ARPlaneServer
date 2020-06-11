using ARPlaneServer.Events;
using ARPlaneServer.Managers;
using DarkRift;
using DarkRift.Server;
using System;

namespace ARPlaneServer {
    class ARPlanePlugin : Plugin {
        readonly PlayerManager playerManager;
        readonly ObjectManager objectManager;

        public ARPlanePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData) {
            ClientManager.ClientConnected += ClientConnected;
            ClientManager.ClientDisconnected += ClientDisconnected;

            playerManager = new PlayerManager(ClientManager);
            objectManager = new ObjectManager(ClientManager);
        }

        public override bool ThreadSafe => false;
        public override Version Version => new Version(1, 0, 0);

        void ClientConnected(object sender, ClientConnectedEventArgs e) {
            Print($"Client connected: {e.Client.ID}");

            playerManager.InitializeClient(e.Client);
            objectManager.InitializeClient(e.Client);

            e.Client.MessageReceived += ClientMessageReceived;
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
            Print($"Client disconnected: {e.Client.ID}");

            playerManager.RemoveClient(e.Client);
            objectManager.RemoveClient(e.Client);
        }

        void ClientMessageReceived(object sender, MessageReceivedEventArgs e) {
            using (Message message = e.GetMessage() as Message) {
                using (DarkRiftReader reader = message.GetReader()) {
                    switch(message.Tag) {

                        case (ushort)Tag.ObjectUpdate:
                            objectManager.HandleUpdateObjectEvent(e.Client, reader.ReadSerializable<ObjectUpdateEvent>());
                            break;
                        case (ushort)Tag.PlayerUpdate:
                            playerManager.HandlePlayerUpdateEvent(e.Client, reader.ReadSerializable<PlayerUpdateEvent>());
                            break;

                        // Add any client message handlers here

                        default: break;
                    }
                }
            }
        }

        // Debugging
        void Print(string message) {
            WriteEvent(message, LogType.Info);
        }
    }
}
