using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DebugActionsMapManagement), "SetTerrainRect")]
internal class H_DebugActionsMapManagement
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> SetTerrainRect(IEnumerable<CodeInstruction> instructions)
    {
        return TranspileHelper.DefNameToLabel(instructions);
    }
}
