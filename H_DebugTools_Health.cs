using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch]
internal static class H_DebugTools_Health
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(DebugTools_Health), "AddHediff");
        yield return AccessTools.Method(typeof(DebugTools_Health), "Options_AddHediff");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var insts = instructions.ToList();

        var target = AccessTools.PropertyGetter(typeof(Def), "LabelCap");
        var indexOfConcern = insts.FindIndex(inst => inst.opcode == OpCodes.Callvirt && inst.operand as MethodInfo == target);
        if (indexOfConcern < 0)
        {
            Log.Error("Failed to find the instruction to patch in DebugTools_Health");
            return instructions;
        }

        return DoTranspile(insts, indexOfConcern);
    }

    private static IEnumerable<CodeInstruction> DoTranspile(List<CodeInstruction> insts, int indexOfConcern)
    {
        for (var i = 0; i < indexOfConcern; i++)
        {
            yield return insts[i];
        }

        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Lib), nameof(Lib.ToLabelWithOriginal)));

        for (var i = indexOfConcern + 2; i < insts.Count; i++)
        {
            yield return insts[i];
        }
    }
}
