using UnityEngine;
using Verse;

namespace DevInYourLanguage;

internal class Settings : ModSettings
{
    public static bool pseudol10n = true;

    public void DoSettingsWindowContents(Rect rect)
    {
        var list = new Listing_Standard
        {
            ColumnWidth = rect.width
        };
        list.Begin(rect);

        list.CheckboxLabeled("DIYL.PseudoL10N".Translate(), ref pseudol10n);
        list.Label("DIYL.PseudoL10N.Description".Translate().Colorize(ColoredText.SubtleGrayColor));

        list.Label("UntranslatedKeyPreview".Translate());

        list.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref pseudol10n, "pseudol10n", true);
    }
}
