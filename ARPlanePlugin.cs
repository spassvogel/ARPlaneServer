using ARPlaneServer.Classes;
using ARPlaneServer.Events;
using ARPlaneServer.Managers;
using DarkRift;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ARPlaneServer {
    class ARPlanePlugin : Plugin {
        PlayerManager playerManager;
        ARCraftManager arCraftManager;
        ObjectManager objectManager;

        public ARPlanePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData) {
            ClientManager.ClientConnected += ClientConnected;
            ClientManager.ClientDisconnected += ClientDisconnected;

            playerManager = new PlayerManager(ClientManager);
            arCraftManager = new ARCraftManager(ClientManager);
            objectManager = new ObjectManager(ClientManager);
        }

        public override bool ThreadSafe => false;
        public override Version Version => new Version(1, 0, 0);

        void ClientConnected(object sender, ClientConnectedEventArgs e) {
            Print($"Client connected {e.Client.ID}");

            Player player = playerManager.InitializeClient(e.Client);
            arCraftManager.InitializeClient(e.Client);

            e.Client.MessageReceived += ClientMessageReceived;
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e) {
            Print($"Client connected {e.Client.ID}");
        }

        void ClientMessageReceived(object sender, MessageReceivedEventArgs e) {
            using (Message message = e.GetMessage() as Message) {
                using (DarkRiftReader reader = message.GetReader()) {
                    switch(message.Tag) {
                        case (ushort)Tag.UpdateTransform:
                            objectManager.HandleUpdateTransformEvent(e.Client, reader.ReadSerializable<UpdateTransformEvent>());
                            break;
                        // TODO: match all other events to their corresponding managers
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
