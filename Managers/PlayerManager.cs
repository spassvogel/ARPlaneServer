using ARPlaneServer.Classes;
using ARPlaneServer.Events;
using DarkRift.Server;
using System.Collections.Generic;
using System.Linq;

namespace ARPlaneServer.Managers {
    /// <summary>
    /// PlayerManager handles Player-related data (e.g. name), connection and disconnection events.
    /// </summary>
    class PlayerManager : Manager {

        // For now, we're not really using this, but we'll probably need it later (e.g. for uniqueness checks, etc)
        readonly Dictionary<IClient, Player> players = new Dictionary<IClient, Player>();

        public PlayerManager(IClientManager clientManager) : base(clientManager) {

        }

        public void InitializeClient(IClient client) {
            Player player = new Player() {
                id = client.ID
            };
            players[client] = player;

            SendPlayerStatesEvent(client);
            SendToOthers(Tag.PlayerConnected, new PlayerConnectedEvent() {
                player = player
            }, client);
        }

        public void RemoveClient(IClient client) {
            SendToOthers(Tag.PlayerDisconnected, new PlayerDisconnectedEvent() {
                player = players[client]
            }, client);

            players.Remove(client);
        }

        public Player[] GetPlayers() {
            return players.Values.ToArray();
        }

        public void HandlePlayerUpdateEvent(IClient client, PlayerUpdateEvent e) {
            players[client] = e.newPlayerState;
            SendToOthers(Tag.PlayerUpdate, e, client);
        }

        public void SendPlayerStatesEvent(IClient client) {
            PlayerStatesEvent spawnEvent = new PlayerStatesEvent() {
                players = players.Values
            };
            SendToClient(Tag.PlayerStates, spawnEvent, client);
        }
    }
}
