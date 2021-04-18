using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wither.CustomGameOptions;
using Wither.CustomRpc;

namespace Wither.Utils
{
    public static class Withering
    {
        public static readonly Dictionary<byte, float> sadBois = new Dictionary<byte, float>();
        
        public static PlayerControl wither;

        public static void Wither(PlayerControl target)
        {
            if (sadBois.ContainsKey(target.PlayerId)) return;
            sadBois.Add(target.PlayerId, GameOptions.WitherDeathTime);
        }

        public static void DrinkMilk(PlayerControl luckBoi)
        {
            if (!sadBois.ContainsKey(luckBoi.PlayerId)) return;
            sadBois.Remove(luckBoi.PlayerId);
        }

        public static void Countdown(PlayerControl player)
        {
            if (player != PlayerControl.LocalPlayer) return;
            var bois = sadBois.Select(sadBoi => sadBoi.Key).ToList();
            
            foreach (byte boi in bois)
            {
                sadBois[boi] -= Time.fixedDeltaTime;
            }
            
            Check();
            Effect();
        }

        public static void Check()
        {
            var boisToYeet = (from sadBoi in sadBois where sadBois[sadBoi.Key] <= 0 select sadBoi.Key).ToList();

            foreach (byte boiToYeet in boisToYeet)
            {
                sadBois.Remove(boiToYeet);
                var player = GameData.Instance.GetPlayerById(boiToYeet)._object;
                player.myRend.color = Color.white;
                wither.RpcMurderPlayer(player);
                InstantiateRoseRpc.CoolSend(player.transform.position);
            }
        }

        private static float lastTime = GameOptions.WitherDeathTime;
        public static void Effect()
        {
            lastTime -= Time.fixedDeltaTime;
            if (lastTime <= 0) lastTime = GameOptions.WitherDeathTime;
            if (Mathf.Round(lastTime) % 2 == 0)
            {
                foreach (var player in sadBois.Select(sadBoi => GameData.Instance.GetPlayerById(sadBoi.Key)._object))
                {
                    player.myRend.color = Color.red;
                }
                return;
            }
            foreach (var player in sadBois.Select(sadBoi => GameData.Instance.GetPlayerById(sadBoi.Key)._object))
            {
                player.myRend.color = Color.white;
            }
        }
    }
}