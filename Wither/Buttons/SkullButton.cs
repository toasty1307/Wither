using System.Linq;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Components.Buttons
{
    public class SkullButton : Button
    {
        protected override void OnClick()
        {
            Rpc<InstantiateSkullRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(1.716975f, 0.5863363f);
            maxTimer = GameOptions.SkullCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.SkullImage);
        }

        public static void InstantiateSkull(Vector2 position)
        {
            GameObject skull = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.WitherSkull);
            GameObject instantiate = Object.Instantiate(skull, ShipStatus.Instance.transform);
            instantiate.transform.position = position;
            instantiate.transform.localScale /= 2;
            instantiate.AddComponent<WitherSkull>();
        }

        protected override bool CouldUse() =>
            PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;
    }
}