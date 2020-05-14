using ARPlaneServer.Classes;
using ARPlaneServer.Events;
using DarkRift.Server;
using System.Collections.Generic;
using System.Linq;

namespace ARPlaneServer.Managers {

    /// <summary>
    /// ARCraftManager handles ARCraft spawning events when player connected.
    /// Note: despawning is assumed to be handled by the client on PlayerDisconnected.
    /// </summary>
    public class ARCraftManager : Manager {

        public ARCraftManager(IClientManager clientManager) : base(clientManager) {
            
        }

        readonly Dictionary<IClient, ARCraft> arCrafts = new Dictionary<IClient, ARCraft>();

        public void InitializeClient(IClient client) {
            ARCraft arCraft = CreateARCraft(client);

            SendSpawnCraftEvent(client, arCraft);
            SendSpawnOtherCraftsEvent(client);
        }

        public void RemoveClient(IClient client) {
            arCrafts.Remove(client);

            // We don't have to send an event because the PlayerDisconnectedEvent should tell the client to remove all client-owned objects.
        }

        ARCraft CreateARCraft(IClient client) {
            ARCraft arCraft = new ARCraft() {
                id = $"{client.ID}:arcraft",
                ownerID = client.ID
            };
            arCrafts[client] = arCraft;

            return arCraft;
        }

        public ARCraft[] GetARCrafts() {
            return arCrafts.Values.ToArray();
        }

        void SendSpawnCraftEvent(IClient client, ARCraft arCraft) {
            ARCraftSpawnEvent spawnEvent = new ARCraftSpawnEvent() {
                arCraft = arCraft
            };
            SendToClient(Tag.ARCraftSpawn, spawnEvent, client);
        }

        // Tell the new client which aircrafts are already in the game
        public void SendSpawnOtherCraftsEvent(IClient client) {
            ARCraftStatesEvent spawnEvent = new ARCraftStatesEvent() {
                otherCrafts = GetARCrafts().Where(x => x.ownerID != client.ID)
            };
            SendToClient(Tag.ARCraftStates, spawnEvent, client);
        }
    }
}
