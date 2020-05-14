using ARPlaneServer.Classes;
using ARPlaneServer.Events;
using DarkRift.Server;
using System.Collections.Generic;

namespace ARPlaneServer.Managers {

    /// <summary>
    /// ObjectManager handles generic objects and their transforms. Updates of those objects are sent through the ObjectManager.
    /// </summary>
    public class ObjectManager : Manager {
        
        readonly Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

        public ObjectManager(IClientManager clientManager) : base(clientManager) {

        }

        public void InitializeClient(IClient client) {
            SendObjectStatesEvent(client);
        }

        public void RemoveClient(IClient client) {
            // Remove all objects owned by client
            List<string> remove = new List<string>();
            foreach (KeyValuePair<string, GameObject> entry in objects) {
                if(entry.Value.ownerID == client.ID) {
                    remove.Add(entry.Key);
                }
            }
            remove.ForEach(x => objects.Remove(x));

            // We don't have to send an event because the PlayerDisconnectedEvent should tell the client to remove all client-owned objects.
        }

        // Will overwrite existing objects with the same ID
        public void Register(GameObject gameObject) {
            objects[gameObject.id] = gameObject;
        }

        public void HandleUpdateObjectEvent(IClient client, ObjectUpdateEvent e) {
            // We assume that the client will spawn any object it cannot find
            Register(e.newState);
            SendToOthers(Tag.ObjectUpdate, e, client);
        }

        public void SendObjectStatesEvent(IClient client) {
            ObjectStatesEvent e = new ObjectStatesEvent() {
                objects = objects.Values
            };
            SendToClient(Tag.ObjectStates, e, client);
        }
    }
}
