using System;
using DarkRift.Server;

public class TestPlugin : Plugin
{
    public TestPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
        WriteEvent("constructor", DarkRift.LogType.Info);
    }

    public override bool ThreadSafe => false;

    public override Version Version => new Version(1, 0, 0);
}