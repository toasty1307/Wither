using System.IO;
using System.Reflection;
using HarmonyLib;
using Reactor.Extensions;
using UnityEngine;
using Wither.MonoBehaviour;

namespace Wither.Utils
{
    public static class AssetBundleLoader
    {
        public static AssetBundle ButtonTextureBundle = null;

        public static AssetBundle PrefabBundle = null;
        
        public static void LoadBundles()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream;
            
            
            // ButtonTextureBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.buttontextures"))
                ButtonTextureBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // ButtonTextureBundle
            
            // PrefabBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.prefabs"))
                PrefabBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // PrefabBundle
        }
    }
}