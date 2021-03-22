using Hazel;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.Buttons;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc]
    public class ReviveRpc : PlayerCustomRpc<Main, ReviveRpc.Data>
    {
        public ReviveRpc(Main plugin) : base(plugin) { }

        public readonly struct Data
        {
            public readonly byte PlayerID;

            public Data(byte playerID)
            {
                PlayerID = playerID;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.PlayerID);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadByte());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            GameData.Instance.GetPlayerById(data.PlayerID)._object.Revive();
            Utils.Coroutines.colors.TryGetValue(GameData.Instance.GetPlayerById(data.PlayerID)._object,
                out Color32 color);
            GameData.Instance.GetPlayerById(data.PlayerID)._object.myRend.color = color;
        }
    }
}