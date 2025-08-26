using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace DevInYourLanguage;

internal static class TranspileHelper
{
    public static IEnumerable<CodeInstruction> DefNameToLabel(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var inst in instructions)
        {
            if (inst.opcode == OpCodes.Ldfld && inst.operand as FieldInfo == AccessTools.Field(typeof(Def), "defName"))
            {
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Lib), nameof(Lib.ToLabelWithOriginal)));
            }
            else
            {
                yield return inst;
            }
        }
    }

    public static IEnumerable<CodeInstruction> MapStrings(IEnumerable<CodeInstruction> instructions, Func<string, string> replacer)
    {
        foreach (var inst in instructions)
        {
            if (inst.opcode == OpCodes.Ldstr)
            {
                var operand = inst.operand as string;
                var labels = new List<Label>(inst.labels);
                inst.labels.Clear();

                var mapped = new CodeInstruction(OpCodes.Ldstr, replacer(operand))
                {
                    labels = labels
                };

                yield return mapped;
            }
            else
            {
                yield return inst;
            }
        }
    }
}