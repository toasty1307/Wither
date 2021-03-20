using HarmonyLib;
using UnityEngine;
using Wither.Buttons;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class FixedUpdatePatch
    {
        public static void Postfix()
        {
            foreach (var bedrock in BedrockButton.bedrocks)
            {
                var collider = bedrock.GetComponent<Collider2D>();
                if (collider != null)     
                    collider.enabled = PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
            }
            HudManager.Instance.ReportButton.gameObject.SetActive(false);
            HudManager._instance.KillButton.gameObject.SetActive(false);
        }
    }
}