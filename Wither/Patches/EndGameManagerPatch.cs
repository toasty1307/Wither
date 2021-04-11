using System.Linq;
using HarmonyLib;
using UnityEngine;
using Wither.Components.Buttons;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(EndGameManager))]
    public static class EndGameManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EndGameManager.Start))]
        public static void Start()
        {
            TransformButton.isTransformed = false;
            BedrockButton.bedrocks.ForEach(Object.Destroy);
            BedrockButton.bedrocks.Clear();
            Object.FindObjectsOfType<WitherRose>().ToList().ForEach(x => Object.Destroy(x.gameObject));
            Object.FindObjectsOfType<WitherSkull>().ToList().ForEach(x => Object.Destroy(x.gameObject));
        }
    }
}