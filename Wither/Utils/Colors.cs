using System.Linq;
using System.Threading;
using Il2CppSystem.Dynamic.Utils;
using Reactor;
using UnhollowerBaseLib;
using UnityEngine;

namespace Wither.Utils
{
    public static class Colors
    {
        public static void SetUpColors()
        {
            global::StringNames[] stringNames =
            {
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor"),
                CustomStringName.Register("NoColor")
            };
            
            global::StringNames[] shortStringNames =
            {
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL"),
                CustomStringName.Register("NCL")
            };
            
            Color32[] colors =
            {
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255)
            };
            
            Color32[] shadowColors =
            {
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255),
                new(247, 120, 255, 255)
            };
            
            MedScanMinigame.ColorNames = MedScanMinigame.ColorNames.Concat(stringNames).ToArray();
            Palette.ColorNames = Palette.ColorNames.Concat(stringNames).ToArray();
            Palette.ShortColorNames = Palette.ShortColorNames.Concat(shortStringNames).ToArray();
            Palette.PlayerColors = Palette.PlayerColors.Concat(colors).ToArray();
            Palette.ShadowColors = Palette.ShadowColors.Concat(shadowColors).ToArray();
        }
    }
}