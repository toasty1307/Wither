using System.IO;
using System.Reflection;
using HarmonyLib;
using Reactor.Extensions;
using UnityEngine;
using Wither.MonoBehaviour;

namespace Wither.Utils
{
    [HarmonyPatch(typeof(AlwaysActive), nameof(AlwaysActive.Start_))]
    public static class AssetBundleLoader
    {
        public static AssetBundle ButtonTextureBundle = null;

        public static AssetBundle PrefabBundle = null;

        public static void LoadBundles()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream;
            
            Main.Logger.LogInfo("ASSET BUNDLES GO BRRRR!!!!");

            // ButtonTextureBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.buttontextures"))
                ButtonTextureBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // ButtonTextureBundle
            
            // PrefabBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.prefabs"))
                PrefabBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // PrefabBundle
        }

        public static void Postfix() => LoadBundles();
    }
}