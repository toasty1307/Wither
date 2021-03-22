using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Wither.Buttons;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class EndGamePatch
    {
        public static void Postfix()
        {
            GameObject.Destroy(ButtonCreatorPatch.transformButton.button.KillButtonManager.gameObject);
            GameObject.Destroy(ButtonCreatorPatch.explodeButton.button.KillButtonManager.gameObject);
            GameObject.Destroy(ButtonCreatorPatch.skullButton.button.KillButtonManager.gameObject);
            GameObject.Destroy(ButtonCreatorPatch.bedrockButton.button.KillButtonManager.gameObject);
            GameObject.Destroy(ButtonCreatorPatch.breakButton.button.KillButtonManager.gameObject);
            GameObject.Destroy(ButtonCreatorPatch.reviveButton.button.KillButtonManager.gameObject);
            ButtonCreatorPatch.transformButton = null;
            ButtonCreatorPatch.explodeButton = null;
            ButtonCreatorPatch.skullButton = null;
            ButtonCreatorPatch.bedrockButton = null;
            ButtonCreatorPatch.breakButton = null;
            ButtonCreatorPatch.reviveButton = null;
            AssetBundleLoader.ButtonTextureBundle.Unload(true);
            AssetBundleLoader.PrefabBundle.Unload(true);
            TransformButton.isTransformed = false;
            BedrockButton.bedrocks = new List<GameObject>();
            foreach (var witherRose in Object.FindObjectsOfType<WitherRose>())
            {
                Object.Destroy(witherRose.gameObject);
            }
            foreach (var witherSkull in Object.FindObjectsOfType<WitherSkull>())
            {
                Object.Destroy(witherSkull.gameObject);
            }
        }
    }
}