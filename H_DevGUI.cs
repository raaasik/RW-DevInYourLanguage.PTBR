using System;

using HarmonyLib;
using LudeonTK;
using UnityEngine;
using Verse;

namespace DevInYourLanguage;

[HarmonyPatch(typeof(DevGUI), "Label")]
internal static class H_DevGUI
{
    [HarmonyPrefix]
    public static bool Label(Rect rect, string label)
    {
        var num = Prefs.UIScale / 2f;
        var position = Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > Single.Epsilon ? UIScaling.AdjustRectToUIScaling(rect) : rect;

        string labelStyled = label;
        if (label.Contains("{"))
        {
            var labelSplit = label.Split(['{', '}']);
            labelStyled = $"{labelSplit[0]} <size=12><color=#979797>{labelSplit[1]}</color></size>";
        }

        GUI.Label(position, " " + labelStyled.TrimStart(), Text.CurFontStyle);
        return false;
    }
}
