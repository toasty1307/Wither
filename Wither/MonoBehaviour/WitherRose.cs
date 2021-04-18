using System;
using System.Linq;
using Reactor;
using UnityEngine;
using Wither.Utils;

namespace Wither.MonoBehaviour
{
    [RegisterInIl2Cpp]
    public class WitherRose : UnityEngine.MonoBehaviour
    {
        public WitherRose(IntPtr ptr) : base(ptr) { }

        public PlayerControl wither;

        private void Start()
        {
            try
            {
                wither = GameData.Instance.AllPlayers.ToArray().Where(x => x.IsImpostor && !x.Disconnected).ToArray().First()._object;
            }
            catch
            {
                Logger<WitherPlugin>.Instance.LogError("aight, so there is no imp it seems, why");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var pc = other.gameObject.GetComponent<PlayerControl>();
            if (pc == null || pc == wither || pc.Data.IsDead) return;
            Logger<WitherPlugin>.Instance.LogInfo($"Withering: {pc == PlayerControl.LocalPlayer}");
            Withering.wither = wither ? wither : PlayerControl.LocalPlayer;
            Withering.Wither(pc);
        }
    }
}