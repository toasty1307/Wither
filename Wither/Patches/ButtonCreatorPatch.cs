using HarmonyLib;
using UnityEngine;
using Wither.Buttons;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class ButtonCreatorPatch
    {
        public static TransformButton transformButton;

        public static void Postfix()
        {
            transformButton = new TransformButton(Vector2.one * 0.125f, CustomGameOptions.TransformCooldown);
        }
    }
}