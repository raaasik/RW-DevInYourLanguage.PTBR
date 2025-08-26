using HarmonyLib;
using LudeonTK;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DebugTabMenu_Actions), "GenerateCacheForMethod")]
internal class H_DebugTabMenu_Actions
{
    private static readonly HashSet<string> supportedAssemblies = ["Assembly-CSharp", "DevL10N"];

#if DEBUG
    [HarmonyPrefix]
    public static bool GenerateCacheForMethod_Debug(MethodInfo method, DebugActionAttribute attribute)
    {
        if (!supportedAssemblies.Contains(method.DeclaringType.Assembly.GetName().Name))
        {
            return true;
        }

        var value = TranslatableFromMethodInfo(method, attribute).Split(['{'])[0];
        Log.Message(Lib.ToTagAsKey(method.Name, value, "DebugAction"));
        return true;
    }
#endif

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> GenerateCacheForMethod(IEnumerable<CodeInstruction> instructions)
    {
        var insts = instructions.ToList();

        // skip all instructions to the first stloc_0, which is just before
        // if (attribute.actionType == DebugActionType.ToolMap || attribute.actionType == DebugActionType.ToolMapForPawns ...
        var indexOfConcern = insts.FindIndex(inst => inst.opcode == OpCodes.Stloc_0);
        if (indexOfConcern < 0)
        {
            Log.Error("Failed to find the instruction to patch in DebugTabMenu_Actions.GenerateCacheForMethod");
            return instructions;
        }

        return DoTranspile(insts, indexOfConcern);
    }

    private static IEnumerable<CodeInstruction> DoTranspile(List<CodeInstruction> insts, int indexOfConcern)
    {
        yield return new CodeInstruction(OpCodes.Ldarg_1);
        yield return new CodeInstruction(OpCodes.Ldarg_2);
        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(H_DebugTabMenu_Actions), nameof(TranslatableFromMethodInfo)));
        yield return new CodeInstruction(OpCodes.Stloc_0);

        for (var i = indexOfConcern + 1; i < insts.Count; i++)
        {
            yield return insts[i];
        }
    }

    private static string TranslatableFromMethodInfo(MethodInfo mi, DebugActionAttribute attr)
    {
        var attrName = attr?.name;
        var originalLabel = string.IsNullOrEmpty(attrName) ? GenText.SplitCamelCase(mi.Name) : attrName;
        var translationKey = "DebugAction_" + mi.Name;

        if (supportedAssemblies.Contains(mi.DeclaringType.Assembly.GetName().Name))
        {
            return $"{translationKey.TranslateSimple()}{{{originalLabel}}}";
        }

        var canBeTranslated = Translator.CanTranslate(translationKey);
        if (canBeTranslated)
        {
            return $"{translationKey.TranslateSimple()}{{{originalLabel}}}";
        }

        if (string.IsNullOrEmpty(attrName))
        {
            return GenText.SplitCamelCase(mi.Name);
        }

        return attrName;
    }
}
