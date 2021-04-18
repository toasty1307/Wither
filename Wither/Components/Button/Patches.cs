using System;
using System.Linq;
using HarmonyLib;
using Reactor;
using TMPro;

namespace Wither.Components.Button
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HudManager.Start))]
        public static void Start()
        {
            Button.taskCompleteOverlayTextRenderer = HudManager.Instance.TaskCompleteOverlay.gameObject.GetComponent<TextMeshPro>();
            Button.MakeButtonsAgain();
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(nameof(HudManager.Update))]
        public static void Update()
        {
            try { Button.SUpdate(); }
            catch (Exception e) { Logger<WitherPlugin>.Instance.LogError(e); }        
        }
    }

    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlOnDestroyPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
        public static void OnDestroy()
        {
            Button.buttons.ToArray().ToList().ForEach(x => x.Dispose());
        }
    }
    
    [HarmonyPatch(typeof(MeetingHud))]
    public static class MeetingHudPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(MeetingHud.Close))]
        public static void Close()
        {
            Button.ResetAll();
        }
    }
}