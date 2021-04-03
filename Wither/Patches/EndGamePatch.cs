using System.Linq;
using HarmonyLib;
using UnityEngine;
using Wither.Components.Buttons;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class EndGamePatch
    {
        public static void Postfix()
        {
            TransformButton.isTransformed = false;
            BedrockButton.bedrocks.ForEach(Object.Destroy);
            BedrockButton.bedrocks.Clear();
            Object.FindObjectsOfType<WitherRose>().ToList().ForEach(x => Object.Destroy(x.gameObject));
            Object.FindObjectsOfType<WitherSkull>().ToList().ForEach(x => Object.Destroy(x.gameObject));
        }
    }
}