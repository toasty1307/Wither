using HarmonyLib;
using Reactor.Extensions;
using UnityEngine;
using Wither.Buttons;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class ButtonCreatorPatch
    {
        public static TransformButton transformButton;
        public static SkullButton skullButton;
        public static ExplodeButton explodeButton;
        public static BreakButton breakButton;
        public static BedrockButton bedrockButton;

        public static void Postfix()
        {
            transformButton = new TransformButton(Vector2.one * 0.125f, CustomGameOptions.TransformCooldown);
            skullButton = new SkullButton(Vector2.one * 1.250f, CustomGameOptions.SkullCooldown);
            explodeButton = new ExplodeButton(Vector2.right * 0.125f + Vector2.up * 1.250f, CustomGameOptions.ExplodeCooldown);
            breakButton = new BreakButton(Vector2.right * 1.250f + Vector2.up * 0.125f, CustomGameOptions.BreakCooldown);
            bedrockButton = new BedrockButton(Vector2.one * 0.125f, CustomGameOptions.BedrockCooldown);
        }
    }
}