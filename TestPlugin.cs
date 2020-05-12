using System;
using System.Collections.Generic;
using System.Linq;
using DarkRift;
using DarkRift.Server;

public class TestPlugin : Plugin
{
    class Craft
    {
        public string name;
        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;
        public float rotZ;
    }

    Dictionary<IClient, Craft> crafts = new Dictionary<IClient, Craft>();

    public TestPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
        ClientManager.ClientConnected += ClientConnected;
        ClientManager.ClientDisconnected += ClientDisconnected;
    }

    public override bool ThreadSafe => false;
    public override Version Version => new Version(1, 0, 0);

    void ClientConnected(object sender, ClientConnectedEventArgs e)
    {
        print($"client connected {e.Client.ID}");
        var craft = new Craft();

        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(e.Client.ID);

            using (Message message = Message.Create(Tags.SpawnCraft, writer))
            {
                // Tell the other clients someone new has joined
                foreach (IClient client in ClientManager.GetAllClients().Where(x => x != e.Client))
                {
                    client.SendMessage(message, SendMode.Reliable);
                }
            }

            using (DarkRiftWriter otherCraftWriter = DarkRiftWriter.Create())
            {
                // Tell the client who was already in the game
                foreach (Craft otherCraft in crafts.Values) 
                {
                    otherCraftWriter.Write(e.Client.ID);
                }

                using (Message playerMessage = Message.Create(Tags.SpawnCraft, otherCraftWriter))
                {
                    e.Client.SendMessage(playerMessage, SendMode.Reliable);
                }
            }
            crafts.Add(e.Client, craft);

        }

        e.Client.MessageReceived += ClientMessageReceived;
    }

    void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        crafts.Remove(e.Client);

        // Inform all clients someone has disconnected
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(e.Client.ID);

            using (Message message = Message.Create(Tags.DespawnCraft, writer))
            {
                foreach (IClient client in ClientManager.GetAllClients())
                {
                    client.SendMessage(message, SendMode.Reliable);
                }                    
            }
        }
    }

    private void ClientMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Craft craft = crafts[e.Client]; // Message came from this craft

        using (Message message = e.GetMessage() as Message)
        {
            if (message.Tag == Tags.UpdateName)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    string name = reader.ReadString();
                    craft.name = name;
                    crafts[e.Client] = craft;
                }
            }
            else if (message.Tag == Tags.UpdateTransform)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    float posX = reader.ReadSingle();
                    float posY = reader.ReadSingle();
                    float posZ = reader.ReadSingle();
                    float rotX = reader.ReadSingle();
                    float rotY = reader.ReadSingle();
                    float rotZ = reader.ReadSingle();
                    craft.posX = posX;
                    craft.posY = posY;
                    craft.posZ = posZ;
                    craft.rotX = rotX;
                    craft.rotY = rotY;
                    craft.rotZ = rotZ;
                    crafts[e.Client] = craft;

                    // Inform other clients this craft is updated
                    using (DarkRiftWriter writer = DarkRiftWriter.Create())
                    {
                        writer.Write(e.Client.ID);
                        writer.Write(posX);
                        writer.Write(posY);
                        writer.Write(posZ);
                        writer.Write(rotX);
                        writer.Write(rotY);
                        writer.Write(rotZ);
                        message.Serialize(writer);
                    }

                    foreach (IClient c in ClientManager.GetAllClients().Where(x => x != e.Client))
                    {
                        c.SendMessage(message, SendMode.Unreliable);
                    }
                }
            }
        }
    }

    // Debugging
    private void print(string message)
    {
        WriteEvent(message, LogType.Info);
    }
}
