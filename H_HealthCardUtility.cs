using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(HealthCardUtility), "DoDebugOptions")]
internal class H_HealthCardUtility
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return TranspileHelper.MapStrings(instructions, StringToKeyed);
    }

    private static string StringToKeyed(string key)
    {
        return key switch
        {
            "Add hediff" => "HealthTab_AddHediff".TranslateSimple(),
            "Hediff debug tooltips" => "HealthTab_DebugTooltip".TranslateSimple(),
            "Hover over hediffs in the health window to get extra debug information about them." => "HealthTab_DebugTooltipDesc".TranslateSimple(),
            "Show hidden Hediffs" => "HealthTab_ShowHidden".TranslateSimple(),
            _ => key,
        };
    }
}
