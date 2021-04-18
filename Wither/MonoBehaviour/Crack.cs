using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using InnerNet;
using Reactor;
using UnityEngine;
using Wither.Buttons;
using Object = UnityEngine.Object;

namespace Wither.MonoBehaviour
{
    [RegisterInIl2Cpp]
    public class Crack : UnityEngine.MonoBehaviour
    {
        public Crack(IntPtr ptr) : base(ptr) { }

        public int id;

        public float radius;

        private void Start()
        {
            radius = GetComponent<CircleCollider2D>().radius;
            InvokeRepeating(nameof(CheckForCollision), 0f, 0.1f);
        }

        public List<PlayerControl> playerInside = new();

        private void CheckForCollision()
        {
            foreach (var player in playerInside.ToArray())
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

        private void OnDestroy()
        {
            foreach (var player in playerInside.ToArray())
            {
                player.Collider.enabled = true;
            }
            playerInside.Clear();
        }

        public static int GetAvailableId()
        {
            return BreakButton.cracks.Count;
        }
        
        public static Crack GetCrackById(int id)
        {
            return FindObjectsOfType<Crack>().FirstOrDefault(crack => crack.id == id);
        }
    }
}