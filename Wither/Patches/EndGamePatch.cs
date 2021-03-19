using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
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
            ButtonCreatorPatch.transformButton = null;
            ButtonCreatorPatch.explodeButton = null;
            ButtonCreatorPatch.skullButton = null;
            ButtonCreatorPatch.bedrockButton = null;
            ButtonCreatorPatch.breakButton = null;
            AssetBundleLoader.ButtonTextureBundle.Unload(true);
            AssetBundleLoader.PrefabBundle.Unload(true);
            GlobalVars.isTransformed = false;
            GlobalVars.bedrocks = new List<GameObject>();
        }
    }
}