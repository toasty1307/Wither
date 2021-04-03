using System.Collections.Generic;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    [CustomButton]
    public class BedrockButton : Button
    {
        public static List<GameObject> bedrocks = new List<GameObject>();

        protected override bool CouldUse() => !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;

        protected override void OnClick()
        {
            Rpc<InstantiateBedrockRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.zero;
            maxTimer = GameOptions.BedrockCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.BedrockImage);
        }

        public static void InstantiateBedrock(Vector2 position)
        {
            GameObject bedrock = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.Bedrock);
            GameObject bedrockInstantiated = Object.Instantiate(bedrock, ShipStatus.Instance.transform);
            bedrockInstantiated.transform.localScale /= 3;
            bedrockInstantiated.transform.position = position;
            bedrockInstantiated.layer = 9;
            bedrocks.Add(bedrockInstantiated); 
            Object.Destroy(bedrockInstantiated, GameOptions.DestroyBedrock ? GameOptions.BedrockDestroyTime : float.MaxValue);
        }
    }
}