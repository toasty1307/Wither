using System.Collections.Generic;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnhollowerBaseLib;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;
using Random = System.Random;

namespace Wither.Components.Buttons
{
    public class ReviveButton : Button
    {
        private static int Lives = GameOptions.CrewLives;

        protected override void OnClick()
        {
            Rpc<ReviveRpc>.Instance.Send(PlayerControl.LocalPlayer.PlayerId);
            Lives--;
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 1.5863363f);
            maxTimer = GameOptions.ReviveCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.ReviveImage);
        }

        protected override bool CouldUse()
        {
            return !PlayerControl.LocalPlayer.Data.IsImpostor && PlayerControl.LocalPlayer.Data.IsDead && Lives > 0;
        }

        protected override bool CanUse()
        {
            return true;
        }
    }
}