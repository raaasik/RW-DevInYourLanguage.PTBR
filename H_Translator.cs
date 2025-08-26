using HarmonyLib;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(Translator), "PseudoTranslated")]
internal class H_Translator
{
    [HarmonyPrefix]
    public static bool PseudoTranslated(string original, ref string __result)
    {
        if (Settings.pseudol10n)
        {
            return true;
        }

        __result = original;
        return false;
    }
}
