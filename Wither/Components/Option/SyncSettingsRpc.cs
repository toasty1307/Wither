using System;
using Hazel;
using Reactor;
using Reactor.Networking;

namespace Wither.Components.Option
{
    [RegisterCustomRpc((uint) CustomRpc.CustomRpc.SyncSettings)]
    public class SyncSettingsRpc : PlayerCustomRpc<WitherPlugin, SyncSettingsRpc.Data>
    {
        public readonly struct Data
        {
            public readonly string Id;
            public readonly byte Type;
            public readonly object Value;

            public Data(string id, byte type, object value)
            {
                Id = id;
                Type = type;
                Value = value;
            }
        }

        public SyncSettingsRpc(WitherPlugin plugin, uint id) : base(plugin, id) { }

        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

        public override void Write(MessageWriter writer, Data data)
        {
            writer.Write(data.Id);
            writer.Write(data.Type);
            switch ((OptionType) data.Type)
            {
                case OptionType.Number:
                    writer.Write((float) data.Value);
                    return;
                case OptionType.Toggle:
                    writer.Write((bool) data.Value);
                    return;
                case OptionType.String:
                    writer.Write((byte) data.Value);
                    break;
                case OptionType.Dropdown:
                    writer.Write((byte) data.Value);
                    break;
                default:
                    throw new ArgumentException("bruh");
            }
        }

        public override Data Read(MessageReader reader)
        {
            string id = reader.ReadString();
            byte type = reader.ReadByte();
            object value = (OptionType) type switch
            {
                OptionType.Number => reader.ReadSingle(),
                OptionType.Toggle => reader.ReadBoolean(),
                OptionType.String => reader.ReadByte(),
                OptionType.Dropdown => reader.ReadByte(),
                _ => throw new ArgumentException("bruh")
            };

            return new Data(id, type, value);
        }

        public override void Handle(PlayerControl innerNetObject, Data data)
        {
            foreach (var customOption in CustomOption.Options)
            {
                if (customOption.Id == data.Id)
                {
                    switch (customOption)
                    {
                        case CustomNumberOption numberOption:
                            numberOption.RaiseOnValueChanged((float) data.Value);
                            return;
                        case CustomStringOption stringOption:
                            stringOption.RaiseOnValueChanged((byte) data.Value);
                            return;
                        case CustomToggleOption toggleOption:
                            toggleOption.RaiseOnValueChanged((bool) data.Value);
                            return;
                        case CustomDropdownOption dropdownOption:
                            dropdownOption.RaiseOnValueChanged((byte) data.Value);
                            return;
                    }
                }
            }
        }
    }
}