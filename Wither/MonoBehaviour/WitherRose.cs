using System;
using System.Linq;
using Reactor;
using UnityEngine;

namespace Wither.MonoBehaviour
{
    [RegisterInIl2Cpp]
    public class WitherRose : UnityEngine.MonoBehaviour
    {
        public WitherRose(IntPtr ptr) : base(ptr) { }

        public PlayerControl wither;

        private void Start()
        {
            wither = GameData.Instance.AllPlayers.ToArray().Where(x => x.IsImpostor && !x.Disconnected).ToArray()[0]._object;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var pc = other.gameObject.GetComponent<PlayerControl>();
            if (pc == null || pc == wither || pc.Data.IsDead) return;
            if (Utils.Coroutines.currentlyWithered.Contains(pc)) return;
            Coroutines.Start(Utils.Coroutines.Wither(wither, pc));
        }
    }
}