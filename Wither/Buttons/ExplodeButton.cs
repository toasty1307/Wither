using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.CustomRpc;
using Wither.Utils;
using Object = UnityEngine.Object;

namespace Wither.Buttons
{
    public class ExplodeButton : Button
    {
        public ExplodeButton(Vector2 _offset, float cooldown) : base(_offset, Utils.StringNames.ExplodeImage, cooldown) { }

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

            Rpc<InstantiateExplosionRpc>.Instance.Send(new InstantiateExplosionRpc.Data(PlayerControl.LocalPlayer.transform.position));
        }

        public static void InstantiateExplosion(Vector2 position)
        {
            GameObject effect = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>(Utils.StringNames.Explosion);
            GameObject instantiate = Object.Instantiate(effect, position, Quaternion.identity);
            Object.Destroy(instantiate, 5f);
        }

        protected override bool CanUse()
        {
            return base.CanUse() && PlayerControl.LocalPlayer.Data.IsImpostor && TransformButton.isTransformed;
        }
    }
}