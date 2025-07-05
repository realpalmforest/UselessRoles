namespace UselessRoles.Patches;

[HarmonyPatch(typeof(LogicGameFlow), nameof(LogicGameFlow.CheckEndCriteria))]
[HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlow.CheckEndCriteria))]
[HarmonyPriority(Priority.First)]
internal static class GameEndPatch
{
    internal static bool Prefix() => false;
}