using Reactor.Extensions;
using UnityEngine;
using Wither.Components.Button;
using Wither.CustomGameOptions;
using Wither.Utils;

namespace Wither.Buttons
{
    public class MilkButton : Button
    {        
        protected override void OnClick()
        {
            Withering.DrinkMilk(PlayerControl.LocalPlayer);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(1.716975f, 0.5863363f);
            maxTimer = GameOptions.MilkCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("MilkImage");
            hasLimitedUse = true;
            maxUses = GameOptions.MilkUses;
        }

        protected override bool CouldUse() => !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;
    }
}