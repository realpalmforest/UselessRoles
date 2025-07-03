namespace UselessRoles.Patches;

[HarmonyPatch]
[HarmonyPriority(Priority.First)]
internal static class GameEndPatch
{
    [HarmonyPatch(typeof(LogicGameFlow), nameof(LogicGameFlow.CheckEndCriteria))]
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlow.CheckEndCriteria))]
    [HarmonyPatch(typeof(LogicGameFlowHnS), nameof(LogicGameFlow.CheckEndCriteria))]
    public static bool Prefix()
    {
        return false;
    }
}
