using System.Collections;
using System.Collections.Generic;
using Reactor;
using UnityEngine;
using Wither.CustomRpc;

namespace Wither.Utils
{
    public static class Coroutines
    {
        public static List<PlayerControl> currentlyWithered = new List<PlayerControl>();
        public static IEnumerator Wither(PlayerControl source, PlayerControl target)
        {
            Reactor.Coroutines.Start(TakeDamage(target));
            currentlyWithered.Add(target);
            yield return new WaitForSeconds(CustomGameOptions.WitherDeathTime);
            if (!target.Data.IsDead) source.RpcMurderPlayer(target);
            currentlyWithered.Remove(target);
            Rpc<InstantiateRoseRpc>.Instance.Send(new InstantiateRoseRpc.Data(target.transform.position));
        }

        public static Dictionary<PlayerControl, Color32> colors = new Dictionary<PlayerControl, Color32>();
        
        public static IEnumerator TakeDamage(PlayerControl target)
        {
            while (!target.Data.IsDead)
            {
                var original = target.myRend.color;
                colors.Add(target, original);
                target.myRend.color = Color.red;
                yield return new WaitForSeconds(0.5f);
                target.myRend.color = original;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}