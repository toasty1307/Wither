using HarmonyLib;
using UnityEngine;
using Wither.Buttons;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class ButtonCreatorPatch
    {
        public static TransformButton transformButton;
        public static ReviveButton reviveButton;
        public static SkullButton skullButton;
        public static ExplodeButton explodeButton;
        public static BreakButton breakButton;
        public static BedrockButton bedrockButton;

        public static void Postfix()
        {
            transformButton = new TransformButton(Vector2.one * 0.125f, CustomGameOptions.GameOptions.TransformCooldown);
            reviveButton = new ReviveButton(Vector2.one * 0.125f, CustomGameOptions.GameOptions.ReviveCooldown);
            skullButton = new SkullButton(Vector2.one * 1.250f, CustomGameOptions.GameOptions.SkullCooldown);
            explodeButton = new ExplodeButton(Vector2.right * 0.125f + Vector2.up * 1.250f, CustomGameOptions.GameOptions.ExplodeCooldown);
            breakButton = new BreakButton(Vector2.right * 1.250f + Vector2.up * 0.125f, CustomGameOptions.GameOptions.BreakCooldown);
            bedrockButton = new BedrockButton(Vector2.one * 0.125f, CustomGameOptions.GameOptions.BedrockCooldown);
        }
    }
}