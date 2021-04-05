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
    public static class CreateButtonsPatch
    {
        public static void Postfix() => CustomButton.CreateButtons();
    }
        
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
    public static class DestroyPatch
    {
        public static void Postfix() => Button.allButtons.ForEach(x => x.Dispose());
    }
}