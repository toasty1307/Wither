using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Reactor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wither.Components.Option
{
    public class CustomDropdownOption : CustomOption
    {
        public readonly List<string> Values;

        public event EventHandler<OnValueChangedEventArgs> OnValueChanged;
        
        public new ToggleOption Data
        {
            get => (ToggleOption) base.Data;
            set => base.Data = value;
        }

        public readonly ConfigEntry<byte> ConfigEntry;

        public bool IsOpen;
        
        public new string Value
        {
            get => (string) base.Value;
            set => base.Value = value;
        }

        public byte ByteValue
        {
            get => (byte) Values.IndexOf(Value);
            set => Value = Values[value];
        }

        public readonly List<ToggleOption> SubOptions = new();
        
        public CustomDropdownOption(string id, string name, string[] values, byte value, bool showValue = true) : base(id, name, OptionType.Dropdown, value, showValue)
        {
            Values = values.ToList();
            ByteValue = value;
            ConfigEntry = PluginSingleton<WitherPlugin>.Instance.Config.Bind("Custom Game Options", Id, ByteValue);
            ConfigEntry.Value = (byte) Mathf.Clamp(ConfigEntry.Value, 0, Values.Count - 1);
            ByteValue = ConfigEntry.Value;
            OnValueChanged += (_, args) => { ConfigEntry.Value = (byte) args.Value; ByteValue = ConfigEntry.Value; };
        }

        public CustomDropdownOption(string id, string name, string[] values, byte value, Color color, bool showValue = true) : base(id, name, OptionType.Dropdown, value, color, showValue)
        {
            Values = values.ToList();
            ByteValue = value;
            ConfigEntry = PluginSingleton<WitherPlugin>.Instance.Config.Bind("Custom Game Options", Id, ByteValue);
            ConfigEntry.Value = (byte) Mathf.Clamp(ConfigEntry.Value, 0, Values.Count - 1);
            ByteValue = ConfigEntry.Value;
            OnValueChanged += (_, args) => { ConfigEntry.Value = (byte) args.Value; ByteValue = ConfigEntry.Value; };
        }

        public override void CreateOption()
        {
            Data = Object.Instantiate(ToggleOptionPrefab.gameObject, menu.transform).GetComponent<ToggleOption>();
            Data.TitleText.text = $"> {Name}";
            Data.CheckMark.transform.parent.gameObject.SetActive(false);
            
            Position(Data.transform);
        }

        public static bool SToggle(ToggleOption option)
        {
            foreach (var customOption in Options)
            {
                if (customOption is CustomDropdownOption customDropdownOption)
                {
                    if (option == customDropdownOption.Data)
                    {
                        customDropdownOption.Toggle();
                        return false;
                    }
                    if (customDropdownOption.SubOptions.Any(toggleOption => toggleOption == option))
                    {
                        customDropdownOption.ClickOption(option);
                        return false;
                    }
                }
            }
            return true;
        }

        public void Toggle()
        {
            IsOpen = !IsOpen;
            if (IsOpen)
            {
                Data.TitleText.text = $"v {Name}";
                foreach (string value in Values)
                {
                    var toggleOptionButDum = Object.Instantiate(SmolToggleOptionPrefab.gameObject, menu.transform)
                        .GetComponent<ToggleOption>();
                    toggleOptionButDum.TitleText.text = value;
                    Position(toggleOptionButDum.transform, Id);
                    SubOptions.Add(toggleOptionButDum);
                    toggleOptionButDum.CheckMark.enabled = SubOptions.IndexOf(toggleOptionButDum) == ByteValue;
                }
                menu.GetComponentInParent<Scroller>().YBounds.max += SubOptions.Count * .5f;
                PositionAll();
                return;
            }
            Data.TitleText.text = $"> {Name}";
            Close();
        }

        public void Close()
        {
            foreach (var toggleOption in SubOptions)
            {
                Object.Destroy(toggleOption.gameObject);
            }
            menu.GetComponentInParent<Scroller>().YBounds.max -= SubOptions.Count * .5f;
            LowestY += SubOptions.Count * .5f;
            SubOptions.Clear();
            LocalLowestYs.Remove(Id);
            PositionAll();
        }

        public void ClickOption(ToggleOption option)
        {
            foreach (var optionBehaviour in SubOptions)
            {
                bool flag = optionBehaviour == option;
                optionBehaviour.CheckMark.enabled = flag;
                if (flag)
                {
                    RaiseOnValueChanged((byte) SubOptions.IndexOf(optionBehaviour));
                    PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
                }
            }
        }
        
        public void RaiseOnValueChanged(byte value)
        {
            OnValueChanged?.Invoke(null, new OnValueChangedEventArgs(value));
        }
    }
}