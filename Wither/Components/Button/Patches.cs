using System.Linq;
using HarmonyLib;

namespace Wither.Components.Buttons
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HudManager.Start))]
        public static void Start()
        {
            Button.taskCompleteOverlayTextRenderer = HudManager.Instance.TaskCompleteOverlay.gameObject.GetComponent<TextRenderer>();
            Button.MakeButtonsAgain();
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Update()
        {
            try { Button.SUpdate(); }
            catch { /* ok, wat */ }        
        }
    }

    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlOnDestroyPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControl.OnDestroy))]
        public static void OnDestroy()
        {
            Button.allButtons.ToArray().ToList().ForEach(x => x.Dispose());
        }
    }
}