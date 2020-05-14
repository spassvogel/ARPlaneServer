using ARPlaneServer.Classes;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARPlaneServer.Managers {
    class PlayerManager : Manager {
        Dictionary<IClient, Player> players = new Dictionary<IClient, Player>();

        public PlayerManager(IClientManager clientManager) : base(clientManager) {

        }

        public Player InitializeClient(IClient client) {
            Player player = new Player() {
                id = client.ID
            };
            players[client] = player;
            return player;
        }

        public Player[] GetPlayers() {
            return players.Values.ToArray();
        }
    }
}
