using HarmonyLib;

namespace Wither.Components.Buttons
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdate
    {
        public static void Postfix()
        {
            try
            {
                Button.SUpdate();
            }
            catch { /* ignored */}
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStartPatch
    {
        public static BedrockButton BedrockButton;
        public static BreakButton BreakButton;
        public static ExplodeButton ExplodeButton;
        public static MilkButton MilkButton;
        public static ReviveButton ReviveButton;
        public static SkullButton SkullButton;
        public static TransformButton TransformButton;
        public static void Postfix()
        {
            BedrockButton = new BedrockButton();
            BreakButton = new BreakButton();
            ExplodeButton = new ExplodeButton();
            MilkButton = new MilkButton();
            ReviveButton = new ReviveButton();
            SkullButton = new SkullButton();
            TransformButton = new TransformButton();
        }
    }
        
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
    public static class DestroyPatch
    {
        public static void Postfix() => Button.allButtons.ForEach(x => x.Dispose());
    }
}