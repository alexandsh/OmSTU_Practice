namespace PluginWithDep;

﻿using PluginSetup;

[PluginLoad("PluginWithoutDep")]
public class PluginWithDep : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("PluginWithDep");
    }
}
