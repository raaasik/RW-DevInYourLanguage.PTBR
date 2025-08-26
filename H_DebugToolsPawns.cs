using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DebugToolsPawns), "PawnGearDevOptions")]
internal class H_DebugToolsPawns
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return TranspileHelper.MapStrings(instructions, StringToKeyed);
    }

    private static string StringToKeyed(string key)
    {
        return key switch
        {
            "Add to inventory" => "GearTab_AddToInventory".TranslateSimple(),
            "Damage random apparel" => "GearTab_DamageRandomApparel".TranslateSimple(),
            "Set primary" => "GearTab_SetPrimary".TranslateSimple(),
            "Wear" => "GearTab_Wear".TranslateSimple(),
            _ => key,
        };
    }
}
