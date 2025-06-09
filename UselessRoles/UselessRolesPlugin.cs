using BepInEx;
using BepInEx.Logging;
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
    public Harmony Harmony { get; } = new(Id);

    public static Random Random = new Random();
    public static ManualLogSource Logger { get; private set; }


    public override void Load()
    {
        Logger = BepInEx.Logging.Logger.CreateLogSource("UselessRoles");

        Harmony.PatchAll();
    }
}
