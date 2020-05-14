using ARPlaneServer.Events;
using DarkRift;
using DarkRift.Server;
using System.Linq;

namespace ARPlaneServer.Managers {
    public class Manager {
        IClientManager clientManager;

        public Manager(IClientManager clientManager) {
            this.clientManager = clientManager;
        }

        protected void SendToClient(Tag tag, NetworkEvent networkEvent, IClient client) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                writer.Write(networkEvent);
                SendToClient(tag, writer, client);
            }
        }

        protected void SendToClient(Tag tag, DarkRiftWriter writer, IClient client) {
            using (Message playerMessage = Message.Create((ushort)tag, writer)) {
                client.SendMessage(playerMessage, SendMode.Reliable);
            }
        }

        protected void SendToOthers(Tag tag, NetworkEvent networkEvent, IClient exceptClient) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                writer.Write(networkEvent);
                SendToOthers(tag, writer, exceptClient);
            }
        }

        protected void SendToOthers(Tag tag, DarkRiftWriter writer, IClient exceptClient) {
            using (Message message = Message.Create((ushort)tag, writer)) {
                foreach (IClient otherClient in clientManager.GetAllClients().Where(x => x != exceptClient)) {
                    otherClient.SendMessage(message, SendMode.Reliable);
                }
            }
        }

        // Debugging
        protected void Print(string message) {
            WriteEvent(message, LogType.Info);
        }
    }
}
