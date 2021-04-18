using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Networking;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wither.Components.Role
{
    #pragma warning disable 660,661
    public abstract class Role
    #pragma warning restore 660,661
    {
        public abstract string Name { get; }
        public abstract int Count { get; }
        public abstract bool ShowTeam { get; }
        public abstract bool ShowRole { get; }
        public abstract Color32 Color { get; }
        public abstract RoleSide RoleSide { get; }
        public abstract string Description { get; }
        public abstract bool HasTasks { get; }
        
        public readonly List<PlayerControl> players = new();

        public static readonly List<Role> roles = new();
        
        protected Role()
        {
            roles.Add(this);
        }

        public static void SetInfected()
        {
            var players = GameData.Instance.AllPlayers.ToArray().Where(x => !x.Disconnected).ToList();
            var impostors = players.Where(x => x.IsImpostor).ToList();
            var crew = players.Where(x => !x.IsImpostor).ToList();

#if DEBUG
            if (AmongUsClient.Instance.GameMode == GameModes.FreePlay)
            {
                roles[0].players.Add(PlayerControl.LocalPlayer);
                return;
            }
#endif

            if (players.Count == 0 || impostors.Count == 0 || crew.Count == 0) return;
            
            foreach (var role in roles)
            {
                role.players.Clear();
                switch (role.RoleSide)
                {
                    case RoleSide.Crewmate:
                    {
                        for (int i = 0; i < role.Count; i++)
                        {
                            var player = crew[Random.Range(0, crew.Count)]._object;
                            crew.Remove(player.Data);
                            role.players.Add(player);
                        }

                        continue;
                    }
                    case RoleSide.Impostor:
                    {
                        for (int i = 0; i < role.Count; i++)
                        {
                            var player = impostors[Random.Range(0, impostors.Count)]._object;
                            impostors.Remove(player.Data);
                            role.players.Add(player);
                        }
                        continue;
                    }
                    case RoleSide.Solo:
                    {
                        for (int i = 0; i < role.Count; i++)
                        {
                            var player = players[Random.Range(0, players.Count)]._object;
                            players.Remove(player.Data);
                            role.players.Add(player);
                        }
                        continue;
                    }
                    default:
                        throw new ArgumentException("Ok, why did you not set the RoleSide?");
                }
            }

            Rpc<SyncRolesRpc>.Instance.Send((from role in roles from playerControl in role.players select playerControl.PlayerId).ToArray());
        }

        public static void PlayerControlFixedUpdate()
        {
            foreach (var role in roles)
            {
                if (!role.ShowRole && (!role.ShowTeam || !Equals(GetRole(PlayerControl.LocalPlayer), role))) continue;
                foreach (var playerControl in role.players)
                {
                    playerControl.nameText.color = role.Color;
                }
            }
        }

        public static IntroCutscene.Nested_0 IntroCutscene(IntroCutscene.Nested_0 __instance)
        {
            var instance = __instance.__this;
            var role = GetRole(PlayerControl.LocalPlayer);
            if (role == null)
            {
                bool imp = PlayerControl.LocalPlayer.Data.IsImpostor;
                instance.Title.text = imp ? "Impostor" : "Crewmate";
                instance.Title.color = imp ? Palette.ImpostorRed : Palette.CrewmateBlue;
                instance.ImpostorText.gameObject.SetActive(imp);
                instance.BackgroundBar.material.color = imp ? Palette.ImpostorRed : Palette.CrewmateBlue;
                return __instance;
            }
            instance.Title.text = role.Name;
            instance.Title.color = role.Color;
            instance.ImpostorText.text = role.Description;
            instance.ImpostorText.color = role.Color;
            instance.ImpostorText.gameObject.SetActive(true);
            instance.BackgroundBar.material.color = role.Color;
            return __instance;
        }

        public static bool ComputeTasks()
        {
            try
            {
                GameData.Instance.TotalTasks = 0;
                GameData.Instance.CompletedTasks = 0;
                for (int i = 0; i < GameData.Instance.AllPlayers.Count; i++)
                {
                    var playerInfo = (GameData.PlayerInfo) GameData.Instance.AllPlayers[(Index) i];
                    bool flag = true;
                    var role = GetRole(playerInfo._object);
                    if (role != null)
                    {
                        flag = role.HasTasks;
                    }

                    if (!playerInfo.Disconnected &&
                        playerInfo.Tasks != null &&
                        playerInfo.Object &&
                        (PlayerControl.GameOptions.GhostsDoTasks ||
                         !playerInfo.IsDead) &&
                        !playerInfo.IsImpostor &&
                        flag)
                    {
                        for (int j = 0; j < playerInfo.Tasks.Count; j++)
                        {
                            GameData.Instance.TotalTasks++;
                            if (playerInfo.Tasks[(Index) j].Cast<GameData.TaskInfo>().Complete)
                            {
                                GameData.Instance.CompletedTasks++;
                            }
                        }
                    }
                }
            }
            catch { /* no */ }

            return false;
        }

        public static Il2CppSystem.Collections.Generic.List<PlayerControl> IntroCutsceneCoBegin(Il2CppSystem.Collections.Generic.List<PlayerControl> team)
        {
            team.Clear();
            team.Add(PlayerControl.LocalPlayer);
            var role = GetRole(PlayerControl.LocalPlayer);
            if (role == null || !role.ShowTeam)
            {
                foreach (var player in GameData.Instance.AllPlayers.ToArray().Where(x => !x.Disconnected))
                {
                    if (PlayerControl.LocalPlayer.Data.IsImpostor)
                    {
                        if (player.IsImpostor && player._object != PlayerControl.LocalPlayer) team.Add(player._object);
                        continue;
                    }
                    if (player._object != PlayerControl.LocalPlayer) team.Add(player._object);
                }
                return team;
            }
            foreach (var playerControl in role.players.Where(playerControl => playerControl != PlayerControl.LocalPlayer && !team.Contains(playerControl)))
            {
                team.Add(playerControl);
            }
            return team;
        }
        
        public static bool operator ==(Role role, string roleToCompare)
        {
            return role?.Name == roleToCompare;
        }

        public static bool operator !=(Role r, string type)
        {
            return !(r == type);
        }
        
        public static Role GetRole(PlayerControl player)
        {
            foreach (var role in roles)
            {
                foreach (var playerControl in role.players)
                {
                    if (playerControl.PlayerId == player.PlayerId) return role;
                }
            }

            return null;
        }

        public static void CreateRoles()
        {
            var assembly = typeof(WitherPlugin).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsSubclassOf(typeof(Role))) continue;
                type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            }
        }

        public static void ExileBegin(ExileController instance, GameData.PlayerInfo exiled)
        {
            var role = GetRole(exiled._object);
            if (role == null || !PlayerControl.GameOptions.ConfirmImpostor) return;

            string s1 = role.Count > 1 ? "An" : "The";
            instance.completeString = $"{exiled.PlayerName} was {s1} {role.Name}";
        }
    }

    public enum RoleSide : uint
    {
        Impostor = 0,
        Crewmate = 1,
        Solo = 2
    }
}