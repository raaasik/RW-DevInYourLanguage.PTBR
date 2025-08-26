using System.Reflection;

using HarmonyLib;
using Verse;

namespace DevInYourLanguage;

[StaticConstructorOnStartup]
public class DIYL
{
    static DIYL()
    {
        new Harmony("latta.diyl").PatchAll(Assembly.GetExecutingAssembly());
    }
}
