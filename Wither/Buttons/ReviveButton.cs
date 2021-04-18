using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Button;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.Buttons
{
    public class ReviveButton : Button
    {
        protected override void OnClick()
        {
            Rpc<ReviveRpc>.Instance.Send(PlayerControl.LocalPlayer.PlayerId);
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(0.6169749f, 0.5863363f);
            maxTimer = GameOptions.ReviveCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("ReviveImage");
            hasLimitedUse = true;
            maxUses = GameOptions.CrewLives;
        }

        protected override bool CouldUse()
        {
            return !PlayerControl.LocalPlayer.Data.IsImpostor && PlayerControl.LocalPlayer.Data.IsDead;
        }

        protected override bool CanUse()
        {
            foreach (var rose in Object.FindObjectsOfType<WitherRose>())
            {
                if (Vector3.Distance(rose.transform.position, PlayerControl.LocalPlayer.transform.position) <= 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}