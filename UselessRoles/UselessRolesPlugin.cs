using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Utilities;

namespace UselessRoles;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class UselessRolesPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new Harmony(Id);

    public override void Load()
    {
        ReactorCredits.Register<UselessRolesPlugin>(ReactorCredits.AlwaysShow);
        Harmony.PatchAll();
    }
}
