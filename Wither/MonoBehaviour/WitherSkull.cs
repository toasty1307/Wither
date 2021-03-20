using System;
using System.Linq;
using Il2CppSystem.Diagnostics.Tracing;
using Reactor;
using UnityEngine;

namespace Wither.MonoBehaviour
{
    [RegisterInIl2Cpp]
    public class WitherSkull : UnityEngine.MonoBehaviour
    {
        public WitherSkull(IntPtr ptr) : base(ptr) { }

        public PlayerControl target;

        public PlayerControl wither;

        public Transform root;

        private void Start()
        {
            wither = GameData.Instance.AllPlayers.ToArray().Where(x => x.IsImpostor && !x.Disconnected).ToArray()[0]._object;
            target = wither.FindClosestTarget();
            transform.position += Vector3.forward * 0.001f;
            root = transform.GetChild(0);
            gameObject.layer = 9;
            InvokeRepeating(nameof(CheckForCollision), 0, 0.1f);
        }

        private void FixedUpdate()
        {
            root.LookAt(target.gameObject.transform);
            if (target.Data.IsDead || target.Data.Disconnected)
                target = wither.FindClosestTarget();
            transform.position += root.forward * 0.1f;
        }

        private void CheckForCollision()
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 0.25f);
            foreach (Collider2D collider2D in collider2Ds)
            {
                var pc = collider2D.gameObject.GetComponent<PlayerControl>();
                if (pc != null && pc != wither)
                {
                    Coroutines.Start(Utils.Coroutines.Wither(wither, target));
                    Destroy(gameObject);
                }
            }
        }
    }
}