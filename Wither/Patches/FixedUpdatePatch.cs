using System.Linq;
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
            foreach (var collider in BedrockButton.bedrocks.Select(bedrock => bedrock.GetComponent<Collider2D>()).Where(collider => collider != null))
            {
                collider.enabled = PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
            }

            HudManager.Instance.ReportButton.gameObject.SetActive(false);
            HudManager.Instance.KillButton.gameObject.SetActive(false);
            HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
            ButtonCreatorPatch.breakButton.button.KillButtonManager.renderer.material.SetFloat("_Desat", 1f);
        }
    }
}