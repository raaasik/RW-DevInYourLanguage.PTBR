using Verse;

namespace DevInYourLanguage;

internal static class Lib
{
    public static string ToLabelWithOriginal(this Def def)
    {
        return $"{def.label.CapitalizeFirst()}{{{def.defName}}}";
    }

    public static string ToTagAsKey(this string key, string val)
    {
        return $"<{key}>{val}</{key}>";
    }
    public static string ToTagAsKey(this string key, string val, string prefix)
    {
        var tagName = $"{prefix}_{key}";
        return $"<{tagName}>{val}</{tagName}>";
    }
}
