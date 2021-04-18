using System;
using System.Linq;
using Reactor;
using UnityEngine;
using Wither.Utils;

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

            root = transform.GetChild(0);
            gameObject.layer = 9;
        }

        private void FixedUpdate()
        {
            if (target == null) return;
            root.LookAt(target.GetTruePosition());
            if (target.Data.IsDead || target.Data.Disconnected)
                target = wither.FindClosestTarget();
            transform.position += root.forward * (PlayerControl.GameOptions.PlayerSpeedMod * CustomGameOptions.GameOptions.WitherSkullSpeedMultiplier);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var pc = other.gameObject.GetComponent<PlayerControl>();
            if (pc == null || pc == wither || pc.Data.IsDead) return;
            Withering.wither = wither ? wither : PlayerControl.LocalPlayer;
            Withering.Wither(pc);
            Destroy(gameObject);
        }
    }
}