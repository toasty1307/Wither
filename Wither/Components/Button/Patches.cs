using System.Linq;
using HarmonyLib;

namespace Wither.Components.Buttons
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdatePatch
    {
        public static void Postfix()
        {
            try
            {
                Button.SUpdate();
            }
            catch
            {
                /* ok, wat */
            }
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStartPatch
    {
        public static void Postfix()
        {
            Button.taskCompleteOverlayTextRenderer =
                HudManager.Instance.TaskCompleteOverlay.gameObject.GetComponent<TextRenderer>();
            Button.MakeButtonsAgain();
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
    public static class PlayerControlOnDestroyPatch
    {
        public static void Postfix() => Button.allButtons.ToArray().ToList().ForEach(x => x.Dispose());
    }
}