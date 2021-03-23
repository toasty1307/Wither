using System.Linq;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Buttons
{
    public class SkullButton : Button
    {
        public SkullButton(Vector2 _offset, float cooldown) : base(_offset, Utils.StringNames.SkullImage, cooldown) { }

        protected override void OnClick()
        {
            Rpc<InstantiateSkullRpc>.Instance.Send(new InstantiateSkullRpc.Data(PlayerControl.LocalPlayer.transform.position));
        }

        public static void InstantiateSkull(Vector2 position)
        {
            GameObject skull = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.WitherSkull);
            GameObject instantiate = Object.Instantiate(skull, ShipStatus.Instance.transform);
            instantiate.transform.position = position;
            instantiate.transform.localScale /= 2;
            instantiate.AddComponent<WitherSkull>();
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
        }
    }
}