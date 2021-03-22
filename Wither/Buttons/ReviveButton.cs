using InnerNet;
using Reactor;
using UnityEngine;
using Wither.CustomRpc;
using Wither.Utils;

namespace Wither.Buttons
{
    public class ReviveButton : Button
    {
        public ReviveButton(Vector2 _offset, float cooldown) : base(_offset, "ReviveImage", cooldown) { }

        private static int Lives = CustomGameOptions.CrewLives;
        
        protected override void OnClick()
        {
            Rpc<ReviveRpc>.Instance.Send(new ReviveRpc.Data(PlayerControl.LocalPlayer.PlayerId));
            Lives--;
        }

        protected override bool CanUse()
        {
            return PlayerControl.LocalPlayer.Data.IsDead && (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started || AmongUsClient.Instance.GameMode == GameModes.FreePlay)
                && !PlayerControl.LocalPlayer.Data.IsImpostor && Lives > 0;
        }
    }
}