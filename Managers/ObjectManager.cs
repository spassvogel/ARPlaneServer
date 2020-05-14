using ARPlaneServer.Classes;
using ARPlaneServer.Events;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPlaneServer.Managers {
    class ObjectManager : Manager {
        Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

        public ObjectManager(IClientManager clientManager) : base(clientManager) {

        }

        public void HandleUpdateTransformEvent(IClient client, UpdateTransformEvent e) {
            if(!objects.ContainsKey(e.id)) {
                // TODO: figure out a way to print (because we're not a plugin)
            }
            SendToOthers(Tag.UpdateTransform, e, client);
        }
    }
}
