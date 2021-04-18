using System.Collections.Generic;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Button;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;

namespace Wither.Buttons
{
    public class BedrockButton : Button
    {
        public static readonly List<GameObject> bedrocks = new();

        protected override bool CouldUse() => !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;

        protected override void OnClick()
        {
            Rpc<InstantiateBedrockRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 0.5863363f);
            maxTimer = GameOptions.BedrockCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("BedrockImage");
        }

        public static void InstantiateBedrock(Vector2 position)
        {
            var bedrock = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>("Bedrock");
            var bedrockInstantiated = Object.Instantiate(bedrock, ShipStatus.Instance.transform);
            bedrockInstantiated.transform.localScale /= 3;
            bedrockInstantiated.transform.position = position;
            bedrockInstantiated.layer = 9;
            bedrocks.Add(bedrockInstantiated); 
            Object.Destroy(bedrockInstantiated, GameOptions.DestroyBedrock ? GameOptions.BedrockDestroyTime : float.MaxValue);
        }
    }
}