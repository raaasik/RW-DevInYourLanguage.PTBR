using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch]
internal static class H_DebugToolsSpawning
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnPawn");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnPawnWithLifestage");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnAtDevelopmentalStages");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "TryPlaceNearThingWithStyle");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnWeapon");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnApparel");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "CreateMealWithSpecifics");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnSite");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnSiteWithPoints");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "SpawnWorldObject");
        yield return AccessTools.Method(typeof(DebugToolsSpawning), "WaterEmergePawn");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return TranspileHelper.DefNameToLabel(instructions);
    }
}
