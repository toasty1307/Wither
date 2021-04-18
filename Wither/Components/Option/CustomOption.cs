using System;
using System.Collections.Generic;
using System.Linq;
using Reactor;
using Reactor.Networking;
using UnhollowerBaseLib;
using UnityEngine;

namespace Wither.Components.Option
{
    public abstract class CustomOption
    {
        public static readonly List<CustomOption> Options = new();
        public static readonly List<OptionBehaviour> OptionBehaviours = new();
        public readonly string Name;
        public readonly string Id;
        public readonly OptionType Type;
        public readonly bool ShowValue;
        public object Value;
        public Color Color;
        public static bool ShowDefaultGameOptions = false;
        public static GameOptionsMenu menu;
        public static NumberOption NumberOptionPrefab;
        public static ToggleOption ToggleOptionPrefab;
        public static ToggleOption SmolToggleOptionPrefab;
        public static StringOption StringOptionPrefab;
        public OptionBehaviour Data;
        public static float LowestY;

        private static float GetLowestY()
        {
            LowestY -= .5f;
            return menu.Children.Last().transform.localPosition.y + LowestY;
        }

        public static readonly Dictionary<string, float> LocalLowestYs = new();
        
        private static float GetLowestY(string id)
        {
            CustomOption o = null;
            foreach (var customOption in Options.Where(customOption => customOption.Id == id))
            {
                o = customOption;
            }

            if (!LocalLowestYs.ContainsKey(o!.Id))
            {
                LocalLowestYs.Add(o.Id, 0);
            }

            return o!.Data.transform.localPosition.y - (LocalLowestYs[o.Id] += 0.5f);
        }
        
        public static void Position(Transform element)
        {
            element.localPosition = new Vector3(element.transform.localPosition.x, GetLowestY(), element.localPosition.z);
        }
        
        public static void Position(Transform element, string id)
        {
            element.localPosition = new Vector3(element.localPosition.x, GetLowestY(id), element.localPosition.z);
        }

        public static void PositionAll()
        {
            LowestY = 0f;
            foreach (var customOption in Options)
            {
                Position(customOption.Data.transform);
                if (customOption is CustomDropdownOption {IsOpen: true} o)
                {
                    LowestY -= o.Values.Count * 0.5f;
                    LocalLowestYs[o.Id] = 0;
                    foreach (var toggleOption in o.SubOptions)
                    {
                        Position(toggleOption.transform, o.Id);
                    }
                }
            }
        }

        protected CustomOption(string id, string name, OptionType type, object value, bool showValue = true)
        {
            Id = id;
            Name = name;
            Type = type;
            Value = value;
            ShowValue = showValue;
            Color = Color.white;
            Options.Add(this);
        }

        protected CustomOption(string id, string name, OptionType type, object value, Color color, bool showValue = true)
        {
            Id = id;
            Name = name;
            Type = type;
            Value = value;
            ShowValue = showValue;
            Color = color;
            Options.Add(this);
        }

        public static void Prefabs()
        {
            if (!AmongUsClient.Instance.AmHost) return;
            NumberOptionPrefab = menu.GetComponentsInChildren<NumberOption>().Last();
            ToggleOptionPrefab = menu.GetComponentsInChildren<ToggleOption>().Last();
            SmolToggleOptionPrefab = menu.GetComponentsInChildren<ToggleOption>().First();
            StringOptionPrefab = menu.GetComponentsInChildren<StringOption>().Last();
        }
        
        public static void Create()
        {
            if (!AmongUsClient.Instance.AmHost) return;
            LowestY = 0f;
            OptionBehaviours.Clear();
            foreach (var customOption in Options)
            {
                customOption.CreateOption();
                OptionBehaviours.Add(customOption.Data);
                menu.Children = new Il2CppReferenceArray<OptionBehaviour>(OptionBehaviours.ToArray().Concat(menu.Children).ToArray());
            }
            menu.GetComponentInParent<Scroller>().YBounds.max += (Options.Count) * .5f;
        }

        public static string ToString(string s)
        {
            string options = string.Empty;
            foreach (var customOption in Options.ToList().Where(x => x.ShowValue))
            {
                string html = customOption.Color.ToHtmlString();
                options += $"<color={html}>{customOption.Name}: {customOption.Value}</color>\n";
            }

            if (!ShowDefaultGameOptions) return options;
            return s + options;
        }
        
        public static void SyncAll()
        {
            if (!AmongUsClient.Instance.AmHost || !PlayerControl.LocalPlayer) return;

            try
            {
                foreach (var customOption in Options)
                {
                    string id = customOption.Id;
                    byte type = (byte) customOption.Type;
                    switch (customOption)
                    {
                        case CustomToggleOption toggleOption:
                            Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(id, type, toggleOption.Value));
                            continue;
                        case CustomStringOption stringOption:
                            Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(id, type,
                                stringOption.ByteValue));
                            continue;
                        case CustomNumberOption numberOption:
                            Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(id, type, numberOption.Value));
                            continue;
                    }
                }
            }
            catch
            {
                Logger<WitherPlugin>.Instance.LogError("Error Sending Custom Game Options");
            }
        }

        public static void UpdateAll()
        {
            foreach (var customOption in Options)
            {
                switch (customOption)
                {
                    case CustomToggleOption toggleOption:
                        toggleOption.RaiseOnValueChanged(toggleOption.Value);
                        continue;
                    case CustomStringOption stringOption:
                        stringOption.RaiseOnValueChanged(stringOption.ByteValue);
                        continue;
                    case CustomNumberOption numberOption:
                        numberOption.RaiseOnValueChanged(numberOption.Value);
                        continue;
                }
            }
        }

        public abstract void CreateOption();
    }

    public enum OptionType : byte
    {
        Toggle,
        Number,
        String,
        Dropdown
    }

    public class OnValueChangedEventArgs : EventArgs
    {
        public readonly object Value;

        public OnValueChangedEventArgs(object value)
        {
            Value = value;
        }
    }
    
    public static class Extensions
    {
        public static string ToHtmlString(this Color color)
        {
            static byte ToByte(float f)
            {
                f = Mathf.Clamp01(f);
                return (byte) (f * 255);
            }
            return $"#{ToByte(color.r):X2}{ToByte(color.g):X2}{ToByte(color.b):X2}{ToByte(color.a):X2}";
        }
    }
}