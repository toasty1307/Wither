using HarmonyLib;
using UnityEngine;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(DoorConsole), nameof(DoorConsole.Use))]
    public static class DoorUsePatch
    {
        public static bool Prefix(DoorConsole __instance)
        {
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out bool flag, out bool _);
            if (!flag)
            {
                return false;
            }
            PlayerControl.LocalPlayer.NetTransform.Halt();
            Minigame minigame = UnityEngine.Object.Instantiate<Minigame>(__instance.MinigamePrefab, Camera.main.transform);
            minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
            (minigame).Cast<IDoorMinigame>().SetDoor(__instance.MyDoor);
            minigame.Begin(null);
            return false;
        }
    }
}