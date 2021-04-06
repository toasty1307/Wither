using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    public class MilkButton : Button
    {        
        protected override void OnClick()
        {
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(1.716975f, 0.5863363f);
            maxTimer = GameOptions.MilkCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.MilkImage);
            hasLimitedUse = true;
            maxUses = 3;
        }

        protected override bool CouldUse() => !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;
    }
}