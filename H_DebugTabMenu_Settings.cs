using HarmonyLib;
using LudeonTK;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DebugTabMenu_Settings), "LegibleFieldName")]
internal static class H_DebugTabMenu_Settings
{
    private static readonly HashSet<string> supportedAssemblies = ["Assembly-CSharp", "DevL10N"];

    [HarmonyPrefix]
    public static bool LegibleFieldName(FieldInfo fi, ref string __result)
    {
        if (supportedAssemblies.Contains(fi.DeclaringType.Assembly.GetName().Name))
        {
#if DEBUG
            Log.Message(fi.Name.CapitalizeFirst().ToTagAsKey(fi.Name.CapitalizeFirst().TranslateSimple()));
#endif
            __result = fi.Name.CapitalizeFirst().TranslateSimple();
            return false;
        }

        return true;
    }
}
