using System;
using System.Collections.Generic;
using System.Linq;
using InnerNet;
using Reactor;
using UnityEngine;

namespace Wither.MonoBehaviour
{
    [RegisterInIl2Cpp]
    public class Crack : UnityEngine.MonoBehaviour
    {
        public Crack(IntPtr ptr) : base(ptr) { }

        public float radius;

        private void Start()
        {
            radius = GetComponent<CircleCollider2D>().radius;
            InvokeRepeating(nameof(CheckForCollision), 0f, 0.1f);
        }

        public List<PlayerControl> playerInside = new List<PlayerControl>();

        private void CheckForCollision()
        {
            foreach (PlayerControl player in playerInside.ToArray())
            {
                player.Collider.enabled = !(Vector3.Distance(transform.position, player.GetTruePosition()) <= radius);
                if (player.Collider.enabled) playerInside.Remove(player);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var pc = other.GetComponent<PlayerControl>();
            if (pc == null) return;
            pc.Collider.enabled = false;
            playerInside.Add(pc);
        }
    }
}