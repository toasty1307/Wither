using System.Runtime.CompilerServices;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.Utils;
using Object = UnityEngine.Object;

namespace Wither.Buttons
{
    public class ExplodeButton : Button
    {
        protected override void OnClick()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), CustomGameOptions.GameOptions.ExplosionRadius);
            foreach (var collider2D in colliders)
            {
                PlayerControl pc = collider2D.gameObject.GetComponent<PlayerControl>();
                if (pc == null) continue;
                Vector2 vector = pc.GetTruePosition() - PlayerControl.LocalPlayer.GetTruePosition();
                float magnitude = vector.magnitude;
                if (pc != PlayerControl.LocalPlayer && !pc.Data.IsDead
                    && !PhysicsHelpers.AnyNonTriggersBetween(PlayerControl.LocalPlayer.GetTruePosition(), vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                    PlayerControl.LocalPlayer.RpcMurderPlayer(pc);
            }

            Rpc<InstantiateExplosionRpc>.Instance.Send(PlayerControl.LocalPlayer.transform.position);
        }

        public static void InstantiateExplosion(Vector2 position)
        {
            GameObject effect = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.Explosion);
            GameObject instantiate = Object.Instantiate(effect, position, Quaternion.identity);
            Object.Destroy(instantiate, 5f);
        }

        public ExplodeButton()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = Vector2.one * 2;
            maxTimer = GameOptions.ExplodeCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(Utils.StringNames.ExplodeImage);
            Initialize();
        }
        protected override bool CouldUse() =>
            PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed && !PlayerControl.LocalPlayer.Data.IsDead;

        protected override bool CanUse()
        {
            return true;
        }
    }
}