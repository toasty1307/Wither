using System.Linq;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Buttons
{
    public class SkullButton : Button
    {
        protected override void OnClick()
        {
            Rpc<InstantiateSkullRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        public static void InstantiateSkull(Vector2 position)
        {
            GameObject skull = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.WitherSkull);
            GameObject instantiate = Object.Instantiate(skull, ShipStatus.Instance.transform);
            instantiate.transform.position = position;
            instantiate.transform.localScale /= 2;
            instantiate.AddComponent<WitherSkull>();
        }

        protected override void SetVars()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.right * 2;
            maxTimer = GameOptions.SkullCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.SkullImage);
        }

        protected override bool CouldUse() =>
            PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse() => true;
    }
}