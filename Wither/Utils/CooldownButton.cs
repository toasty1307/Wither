using HarmonyLib;
using System;
using System.Collections.Generic;
using InnerNet;
using Reactor.Extensions;
using UnityEngine;

namespace Wither.Utils
{
    public class CooldownButton
    {
        public static List<CooldownButton> buttons = new List<CooldownButton>();
        public KillButtonManager KillButtonManager;
        public Vector2 PositionOffset = Vector2.zero;
        public float MaxTimer = 0f;
        public float Timer = 0f;
        public float EffectDuration = 0f;
        public bool IsEffectActive;
        public bool HasEffectDuration;
        public bool Enabled = true;
        public Func<bool> UseTester;
        private Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();
        private Sprite Sprite_;
        public Action OnClick;
        public Action OnEffectEnd;
        public HudManager HudManager;
        public bool CanUse_;

        public CooldownButton(Action onClick, float cooldown, Sprite image, Vector2 positionOffset, Func<bool> useTester, HudManager hudManager, float effectDuration, Action onEffectEnd)
        {
            SetVars(onClick, cooldown, image, positionOffset, useTester, hudManager);
            this.HasEffectDuration = true;
            this.IsEffectActive = false;
            this.OnEffectEnd = onEffectEnd;
            this.EffectDuration = effectDuration;
        }

        public CooldownButton(Action onClick, float cooldown, Sprite image, Vector2 positionOffset, Func<bool> useTester, HudManager hudManager)
        {
            SetVars(onClick, cooldown, image, positionOffset, useTester, hudManager);
            this.HasEffectDuration = false;
            Update();
        }
        private void SetVars(Action onClick, float cooldown, Sprite image, Vector2 positionOffset, Func<bool> useTester, HudManager hudManager)
        {
            buttons.Add(this);
            this.HudManager = hudManager;
            this.OnClick = onClick;
            this.PositionOffset = positionOffset;
            this.UseTester = useTester;
            this.MaxTimer = cooldown;
            this.Timer = MaxTimer;
            this.Sprite_ = image;
            this.KillButtonManager = UnityEngine.Object.Instantiate(HudManager.KillButton, HudManager.transform);
            Update();
        }
        internal bool CanUse()
        {
            if (PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data == null) return false;
            CanUse_ = UseTester();
            return true;
        }
        internal static void HudUpdate()
        {
            buttons.RemoveAll(item => item.KillButtonManager == null);
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].CanUse())
                    buttons[i].Update();
            }
        }
        private void Update()
        {
            if (KillButtonManager.transform.localPosition.x > 0f)
                KillButtonManager.transform.localPosition = new Vector3((KillButtonManager.transform.localPosition.x + 1.3f) * -1, KillButtonManager.transform.localPosition.y, KillButtonManager.transform.localPosition.z) + new Vector3(PositionOffset.x, PositionOffset.y);
            PassiveButton button = KillButtonManager.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)listener);
            void listener()
            {
                if (Timer < 0f && CanUse_ && Enabled)
                {
                    KillButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
                    if (HasEffectDuration)
                    {
                        IsEffectActive = true;
                        Timer = EffectDuration;
                        KillButtonManager.TimerText.Color = new Color(0, 255, 0);
                    }
                    else
                    {
                        Timer = MaxTimer;
                    }
                    OnClick();
                }
            }
            KillButtonManager.renderer.sprite = this.Sprite_;
            if (Timer < 0f)
            {
                if (Enabled)
                    KillButtonManager.renderer.color = new Color(1f, 1f, 1f, 1f);
                else
                    KillButtonManager.renderer.color = new Color(1f, 1f, 1f, .3f);
                if (IsEffectActive)
                {
                    KillButtonManager.TimerText.Color = new Color(255, 255, 255);
                    Timer = MaxTimer;
                    IsEffectActive = false;
                    OnEffectEnd();
                }
            }
            else
            {
                if (CanUse_ && (PlayerControl.LocalPlayer.CanMove || IsEffectActive))
                    Timer -= Time.deltaTime;
                KillButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
            }
            KillButtonManager.gameObject.SetActive(CanUse_);
            KillButtonManager.renderer.enabled = CanUse_;
            if (CanUse_)
            {
                KillButtonManager.renderer.material.SetFloat("_Desat", 0f);
                KillButtonManager.SetCoolDown(Timer, MaxTimer);
            }
        }
        public static string GetHashSHA1(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                return string.Concat(sha1.ComputeHash(data).Join(x => x.ToString("X2")));
            }
        }
        // Credit: http://jon-martin.com/?p=114
        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
    }
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class ButtonUpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            CooldownButton.HudUpdate();
        }
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class ButtonResetPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            CooldownButton.buttons.RemoveAll(item => item.KillButtonManager == null);
            for (int i = 0; i < CooldownButton.buttons.Count; i++)
            {
                CooldownButton.buttons[i].KillButtonManager.TimerText.Color = new Color(255, 255, 255);
                CooldownButton.buttons[i].Timer = CooldownButton.buttons[i].MaxTimer;
                CooldownButton.buttons[i].IsEffectActive = false;
                CooldownButton.buttons[i].OnEffectEnd();
            }
        }
    }
    
    public class Button
    {
        public float cooldown;
        public Vector2 offset;
        public Sprite sprite;
        public CooldownButton button;

        public Button(Vector2 _offset, string spriteName, float _cooldown)
        {
            offset = _offset;
            sprite = AssetBundleLoader.ButtonTextureBundle.LoadAsset<Sprite>(spriteName);
            cooldown = _cooldown;
            button = new CooldownButton(
                OnClick,
                cooldown,
                sprite,
                offset,
                CanUse,
                HudManager.Instance
            );
        }

        ~Button()
        {
            CooldownButton.buttons.Remove(button);
            button = null;
        }

        protected virtual void OnClick()
        {
            
        }

        protected virtual bool CanUse() 
        {
            return !PlayerControl.LocalPlayer.Data.IsDead && (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started || AmongUsClient.Instance.GameMode == GameModes.FreePlay);
        }
    }
}