using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomRpc;
using Wither.Utils;

namespace Wither.Buttons
{
    public class BedrockButton : Button
    {
        public BedrockButton(Vector2 _offset, float cooldown) : base(_offset, "BedrockImage", cooldown) { }

        protected override void OnClick()
        {
            Rpc<InstantiateBedrockRpc>.Instance.Send(new InstantiateBedrockRpc.Data(PlayerControl.LocalPlayer.transform.position));
        }

        protected override bool CanUse()
        {
            return base.CanUse() && !PlayerControl.LocalPlayer.Data.IsImpostor;
        }

        public static void InstantiateBedrock(Vector2 position)
        {
            GameObject bedrock = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>("Bedrock");
            GameObject bedrockInstantiated = Object.Instantiate(bedrock, ShipStatus.Instance.transform);
            bedrockInstantiated.transform.localScale /= 3;
            bedrockInstantiated.transform.position = position;
            bedrockInstantiated.layer = Constants.ShipOnlyMask;
            GlobalVars.bedrocks.Add(bedrockInstantiated);
        }
    }
}