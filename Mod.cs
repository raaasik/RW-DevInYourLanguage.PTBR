using UnityEngine;
using Verse;

namespace DevInYourLanguage;

internal class Mod: Verse.Mod
{
    private readonly Settings settings;

    public Mod(ModContentPack content) : base(content)
    {
        settings = GetSettings<Settings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var viewRect = new Rect(inRect.x, 30f, inRect.width, inRect.height - 30f);
        settings.DoSettingsWindowContents(viewRect);
    }

    public override string SettingsCategory()
    {
        return "Dev In Your Language";
    }
}
