using System.IO;
using System.Reflection;
using Reactor.Extensions;
using UnityEngine;

namespace Wither.Utils
{
    public static class AssetBundleLoader
    {
        public static AssetBundle ButtonTextureBundle { get; private set; }

        public static void LoadBundles()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream;
            
            // ButtonTextureBundle
            using (stream = executingAssembly.GetManifestResourceStream("Wither.AssetBundles.buttontextures"))
                ButtonTextureBundle = AssetBundle.LoadFromMemory(stream.ReadFully());
            // ButtonTextureBundle
        }
    }
}