using AmongUs.GameOptions;
using HarmonyLib;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetRole))]
public static class SetRolePatch
{
    public static void Prefix(ref RoleTypes roleType)
    {
        roleType = RoleTypes.Crewmate;

        if (!AmongUsClient.Instance.AmHost)
            return;

        foreach (var player in PlayerControl.AllPlayerControls)
        {
            RoleManager.AssignRole(player);
        }
    }
}