using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.Networking;
using UnhollowerBaseLib;
using UnityEngine;

namespace Wither.Components.Option
{
    public class CustomOption
    {
        public static List<CustomOption> Options = new List<CustomOption>();
        public static List<OptionBehaviour> OptionBehaviours = new List<OptionBehaviour>();
        public readonly string Name;
        public readonly string Id;
        public readonly OptionType Type;
        public readonly bool ShowValue;
        public object Value;
        public static bool ShowDefaultGameOptions = false;
        public static GameOptionsMenu menu;
        public static NumberOption NumberOptionPrefab;
        public static ToggleOption ToggleOptionPrefab;
        public static StringOption StringOptionPrefab;
        public OptionBehaviour Data;
        public static float LowestY;

        private static float GetLowestY()
        {
            LowestY -= .5f;
            return menu.Children.Last().transform.localPosition.y + LowestY;
        }
        
        public static void Position(Transform element)
        {
            element.localPosition = new Vector3(element.transform.localPosition.x, GetLowestY(), element.localPosition.z);
        }

        public CustomOption(string id, string name, OptionType type, object value, bool showValue = true)
        {
            Id = id;
            Name = name;
            Type = type;
            Value = value;
            ShowValue = showValue;
            Options.Add(this);
        }

        public static void Prefabs()
        {
            if (!AmongUsClient.Instance.AmHost) return;
            NumberOptionPrefab = menu.GetComponentsInChildren<NumberOption>().Last();
            ToggleOptionPrefab = menu.GetComponentsInChildren<ToggleOption>().Last();
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
            menu.GetComponentInParent<Scroller>().YBounds.max += Options.Count * .5f;
        }

        public static string ToString(string s)
        {
            string options = string.Empty;
            foreach (var customOption in Options)
            {
                if (customOption.ShowValue)
                    options += customOption.Name + ": " + customOption.Value + "\n";
            }

            if (!ShowDefaultGameOptions) return options;
            s += options;
            return s;
        }

        public static void SyncAll()
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
                        Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(id, type, stringOption.Value));
                        continue;
                    case CustomNumberOption numberOption:
                        Rpc<SyncSettingsRpc>.Instance.Send(new SyncSettingsRpc.Data(id, type, numberOption.Value));
                        continue;
                }
            }
        }

        public virtual void CreateOption() { }
    }

    public enum OptionType : byte
    {
        Toggle,
        Number,
        String
    }

    public class OnValueChangedEventArgs : EventArgs
    {
        public readonly object Value;

        public OnValueChangedEventArgs(object value)
        {
            Value = value;
        }
    }
}