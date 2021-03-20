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
            InvokeRepeating(nameof(CheckForCollision), 0, 0.25f);
        }

        private void CheckForCollision()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 0.25f);
            foreach (Collider2D collider2D in collider2Ds)
            {
                var pc = collider2D.gameObject.GetComponent<PlayerControl>();
                if (pc != null && pc != wither)
                {
                    if (Utils.Coroutines.currentlyWithered.Contains(pc)) continue;
                    Coroutines.Start(Utils.Coroutines.Wither(wither, pc));
                }
            }
        }
    }
}