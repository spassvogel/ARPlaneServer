using ARPlaneServer.Classes;
using ARPlaneServer.Events;
using DarkRift;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARPlaneServer.Managers {
    public class ARCraftManager : Manager {

        public ARCraftManager(IClientManager clientManager) : base(clientManager) {

        }

        Dictionary<IClient, ARCraft> crafts = new Dictionary<IClient, ARCraft>();

        public void InitializeClient(IClient client) {
            SendSpawnCraft(client);
            SendSpawnOtherCrafts(client);
        }

        public ARCraft[] GetARCrafts() {
            return crafts.Values.ToArray();
        }

        void SendSpawnCraft(IClient client) {
            SpawnCraftEvent spawnEvent = new SpawnCraftEvent() {

            };
            SendToClient(Tag.SpawnCraft, spawnEvent, client);
        }

        // Tell the new client which aircrafts are already in the game
        public void SendSpawnOtherCrafts(IClient client) {
            SpawnOtherCraftsEvent spawnEvent = new SpawnOtherCraftsEvent() {
                otherCrafts = GetARCrafts().Where(x => x.ownerID != client.ID)
            };
            SendToClient(Tag.SpawnCraft, spawnEvent, client);
        }
    }
}
