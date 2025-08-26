using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DebugWindowsOpener), "DrawButtons")]
internal static class H_DebugWindowsOpener
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var inst in instructions)
        {
            if (inst.opcode == OpCodes.Ldstr)
            {
                var operand = inst.operand as string;
                var labels = new List<Label>(inst.labels);
                inst.labels.Clear();

                var mapped = new CodeInstruction(OpCodes.Ldstr, StringToKeyed(operand))
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

    private static string StringToKeyed(string key)
    {
        return key switch
        {
            "Open the debug log." => "DebugMenu_Logs".TranslateSimple(),
            "Open tweakvalues menu.\n\nThis lets you change internal values." => "DebugMenu_TweakValues".TranslateSimple(),
            "Open the view settings.\n\nThis lets you see special debug visuals." => "DebugMenu_ViewSettings".TranslateSimple(),
            "Open debug actions menu.\n\nThis lets you spawn items and force various events." => "DebugMenu_DebugActions".TranslateSimple(),
            "Open debug logging menu." => "DebugMenu_DebugLogging".TranslateSimple(),
            "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables." => "DebugMenu_Inspector".TranslateSimple(),
            "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours." => "DebugMenu_GodMode".TranslateSimple(),
            "God mode" => "GodMode".TranslateSimple(),
            "Toggle the dev palette.\n\nAllows you to setup a palette of debug actions for ease of use." => "DebugMenu_DevPalette".TranslateSimple(),
            "Pause the game when an error is logged." => "DebugMenu_PauseOnError".TranslateSimple(),
            _ => key,
        };
    }
}
