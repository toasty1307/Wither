using HarmonyLib;
using UnityEngine;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class FixedUpdatePatch
    {
        public static void Postfix()
        {
            foreach (var bedrock in GlobalVars.bedrocks)
            {
                bedrock.GetComponent<Collider2D>().enabled = GlobalVars.isTransformed;
            }
        }
    }
}