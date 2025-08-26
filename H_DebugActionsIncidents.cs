using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DebugActionsIncidents))]
internal class H_DebugActionsIncidents
{
    private static FastInvokeHandler GetIncidentTargetLabel;

    [HarmonyTranspiler]
    [HarmonyPatch("GetIncidentWithPointsDebugAction")]
    public static IEnumerable<CodeInstruction> GetIncidentWithPointsDebugActionTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var insts = instructions.ToList();

        // skip all instructions to the first stloc_0, which is just before
        // if (attribute.actionType == DebugActionType.ToolMap || attribute.actionType == DebugActionType.ToolMapForPawns ...
        var indexOfConcern = insts.FindIndex(inst => inst.opcode == OpCodes.Ldftn);
        if (indexOfConcern < 0)
        {
            Log.Error("Failed to find the instruction to patch in DebugTabMenu_Actions.GenerateCacheForMethod");
            return instructions;
        }

        return DoTranspile(insts, indexOfConcern);
    }

    [HarmonyTranspiler]
    [HarmonyPatch("IncidentsYielder", MethodType.Enumerator)]
    public static IEnumerable<CodeInstruction> IncidentsYielderTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return TranspileHelper.MapStrings(instructions, StringToKeyed);
    }

    private static IEnumerable<CodeInstruction> DoTranspile(List<CodeInstruction> insts, int indexOfConcern)
    {
        for (var i = 0; i < indexOfConcern; i++)
        {
            yield return insts[i];
        }

        yield return new CodeInstruction(OpCodes.Ldftn, AccessTools.Method(typeof(H_DebugActionsIncidents), nameof(PointedIncidentLabelGetter)));

        for (var i = indexOfConcern + 1; i < insts.Count; i++)
        {
            yield return insts[i];
        }
    }

    private static string PointedIncidentLabelGetter()
    {
        GetIncidentTargetLabel ??= MethodInvoker.GetHandler(AccessTools.Method(typeof(DebugActionsIncidents), "GetIncidentTargetLabel"));
        return "DebugAction_DoIncidentPointed".TranslateSimple() + " (" + (string)GetIncidentTargetLabel.Invoke(null, null) + ")";
    }

    private static string StringToKeyed(string key)
    {
        return key switch
        {
            "Do incident" => "DebugAction_DoIncident".TranslateSimple(),
            "Do incident x10" => "DebugAction_DoIncidentX10".TranslateSimple(),
            _ => key,
        };
    }
}
