using Hazel;
using Reactor;
using Reactor.Extensions;
using UnityEngine;
using Wither.Buttons;
using Wither.MonoBehaviour;
using Wither.Utils;

namespace Wither.CustomRpc
{
    [RegisterCustomRpc]
    public class InstantiateRoseRpc : PlayerCustomRpc<Main, InstantiateRoseRpc.Data>
    {
        public InstantiateRoseRpc(Main plugin) : base(plugin) { }

        public readonly struct Data
        {
            public readonly Vector2 Position;
            
            public Data(Vector2 position)
            {
                Position = position;
            }
        }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.After;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Position);
        }

        public override Data Read(MessageReader reader)
        {
            return new Data(reader.ReadVector2());
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            GameObject rose = AssetBundleLoader.PrefabBundle.LoadAsset<GameObject>("WitherRose");
            GameObject instantiate = Object.Instantiate(rose, ShipStatus.Instance.transform);
            instantiate.transform.position = data.Position;
            instantiate.AddComponent<WitherRose>();
        }
    }
}