using System.Collections;
using UselessRoles.Roles;
using UselessRoles.Utility;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(TutorialManager._RunTutorial_d__3), nameof(TutorialManager._RunTutorial_d__3.MoveNext))]
public static class RunTutorialPatch
{
    public static void Postfix(TutorialManager._RunTutorial_d__3 __instance, ref bool __result)
    {
        // __result is false when the Coroutine is done
        if (__result) return;
        
        ModRoleManager.IntroShown = true;
        
        if(PlayerControl.LocalPlayer.isDummy)
            return;
            
        // Assign crewmate role to all dummies
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if(player.HasRole())
                return;

            ModRoleManager.RpcSetRole(player, (uint)RoleType.Crewmate);
        }
        
        Logger<UselessRolesPlugin>.Info($"Assigned CREWMATE role to all dummies");
    }
}

