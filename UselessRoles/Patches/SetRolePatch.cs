﻿using AmongUs.GameOptions;

namespace UselessRoles.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetRole))]
internal static class SetRolePatch
{
    internal static void Prefix(ref RoleTypes roleType) => roleType = RoleTypes.Crewmate;
}