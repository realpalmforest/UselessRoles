using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using System;

namespace UselessRoles;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class UselessRolesPlugin : BasePlugin
{
    public static UselessRolesPlugin Instance { get; private set; }

    public Harmony Harmony { get; } = new Harmony(Id);

    public Random Random { get; } = new Random();

    public override void Load()
    {
        Instance = this;

        Harmony.PatchAll();
    }
}
