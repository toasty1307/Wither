using Hazel;
using Reactor;
using UnityEngine;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc]
    public class ReviveRpc : PlayerCustomRpc<WitherPlugin, byte>
    {
        public ReviveRpc(WitherPlugin plugin) : base(plugin) { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;
        public override void Write(MessageWriter writer, byte data) => writer.Write(data);
        public override byte Read(MessageReader reader) => reader.ReadByte();

        public override void Handle(PlayerControl innerNetObject, byte data)
        {
            GameData.Instance.GetPlayerById(data)._object.Revive();
            Utils.Coroutines.colors.TryGetValue(GameData.Instance.GetPlayerById(data)._object, out Color32 color);
            if (new Color32().Equals(color)) GameData.Instance.GetPlayerById(data)._object.myRend.color = color;
        }
    }
}