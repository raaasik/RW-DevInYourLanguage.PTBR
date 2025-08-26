using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch]
internal static class H_DebugThingPlaceHelper
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(DebugThingPlaceHelper), "SpawnOptions");
        yield return AccessTools.Method(typeof(DebugThingPlaceHelper), "TryAbandonOptionsForStackCount");
        yield return AccessTools.Method(typeof(DebugThingPlaceHelper), "TryPlaceOptionsForBaseMarketValue");
        yield return AccessTools.Method(typeof(DebugThingPlaceHelper), "TryPlaceOptionsForStackCount");
        yield return AccessTools.Method(typeof(DebugThingPlaceHelper), "TryPlaceOptionsUnminified");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return TranspileHelper.DefNameToLabel(instructions);
    }
}
