using Reactor.Extensions;
using Reactor.Networking;
using UnityEngine;
using Wither.Components.Button;
using Wither.Components.Role;
using Wither.CustomGameOptions;
using Wither.CustomRpc;
using Wither.MonoBehaviour;
using Wither.Roles;
using Wither.Utils;
using Object = UnityEngine.Object;

namespace Wither.Buttons
{
    public class FixButton : Button
    {
        public static Crack currentCrack;
        
        protected override bool CouldUse()
        {
            return !PlayerControl.LocalPlayer.Data.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead &&
                   Role.GetRole(PlayerControl.LocalPlayer) == nameof(Mechanic);
        }

        protected override bool CanUse()
        {
            return currentCrack;
        }

        protected override void OnClick()
        {
            if (currentCrack)
                Rpc<DestroyCrackRpc>.Instance.Send(currentCrack.id);
        }

        public static void Fix(int id)
        {
            BreakButton.cracks.RemoveAt(id);
            Object.Destroy(Crack.GetCrackById(id).gameObject);
        }

        protected override void Update()
        {
            currentCrack = null;
            float closest = float.MaxValue;
            foreach (var crack in BreakButton.cracks)
            {
                float distance = Vector3.Distance(crack.transform.position, PlayerControl.LocalPlayer.transform.position);
                if (!(distance <= closest) ||
                    !(distance <= PlayerControl.GameOptions.KillDistance * GameOptions.FixRange)) continue;
                closest = distance;
                currentCrack = crack;
            }
        }

        protected override void Init()
        {
            edgeAlignment = AspectPosition.EdgeAlignments.LeftBottom;
            offset = new Vector2(1.716975f, 1.686336f);
            maxTimer = GameOptions.FixCooldown;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>("MilkImage");
        }
    }
}