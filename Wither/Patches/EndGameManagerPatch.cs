using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Wither.Buttons;
using Wither.MonoBehaviour;

namespace Wither.Patches
{ 
    [HarmonyPatch]
    public static class EndGameManagerPatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(PlayerControl), nameof(PlayerControl.OnDestroy));
            yield return AccessTools.Method(typeof(EndGameManager), nameof(EndGameManager.Start));
        }
        
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