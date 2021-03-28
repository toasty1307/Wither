using HarmonyLib;
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
        public static MilkButton milkButton;

        public static void Postfix()
        {
            bedrockButton = new BedrockButton();
            transformButton = new TransformButton();
            reviveButton = new ReviveButton();
            skullButton = new SkullButton();
            breakButton = new BreakButton();
            explodeButton = new ExplodeButton();
            milkButton = new MilkButton();
        }
    }
}