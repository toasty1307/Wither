using System;
using System.Collections.Generic;
using System.Reflection;
using InnerNet;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Wither.Components.Buttons
{
    public abstract class Button : IDisposable
    {
        public static List<Button> allButtons = new List<Button>();

        protected GameObject gameObject;
        protected KillButtonManager killButtonManager;
        protected AspectPosition aspectPosition;
        protected AspectPosition.EdgeAlignments edgeAlignment;
        protected SpriteRenderer spriteRenderer;
        protected TextRenderer timerText;
        protected PassiveButton button;
        protected Vector2 offset;
        protected Sprite sprite;
        protected float timer;
        protected float maxTimer;
        protected bool isCoolingDown;
        protected bool commonCanUse;
        protected bool hasLimitedUse = false;
        protected float maxUses = 0;
        protected float currentUses;
        private static readonly int Desat = Shader.PropertyToID("_Desat");
        private static readonly int Percent = Shader.PropertyToID("_Percent");

        protected Button()
        {
            Initialize();
            Init();
            InitializePart2();
        }

        protected void Initialize()
        {
            gameObject = Object.Instantiate(HudManager.Instance.KillButton.gameObject, HudManager.Instance.transform);
            gameObject.SetActive(true);
            killButtonManager = gameObject.GetComponent<KillButtonManager>();
            timerText = killButtonManager.TimerText;
            aspectPosition = gameObject.GetComponent<AspectPosition>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            button = gameObject.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityAction)OnClickListener);
            aspectPosition.AdjustPosition();
            commonCanUse = (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started || AmongUsClient.Instance.GameMode == GameModes.FreePlay);
            allButtons.Add(this);
        }

        protected void InitializePart2()
        {
            SetCoolDown(timer = maxTimer, maxTimer);
            aspectPosition.Alignment = edgeAlignment;
            spriteRenderer.sprite = sprite;
            aspectPosition.DistanceFromEdge = (spriteRenderer.size / 2) + offset;
            currentUses = maxUses;
        }

        public static void SUpdate()
        {
            foreach (var button in allButtons)
            {
                try { button.ButtonUpdate(); }
                catch { /* WitherPlugin.Logger.LogInfo("EXCEPTION UPDATING A BUTTON");*/ }
            }
        }

        private void ButtonUpdate()
        {
            gameObject.SetActive(CouldUse() && commonCanUse);
            if (!gameObject.active) return;
            if (PlayerControl.LocalPlayer.CanMove)
                timer -= Time.deltaTime;
            SetCoolDown(timer, maxTimer);
            spriteRenderer.sprite = sprite;
            aspectPosition.AdjustPosition();
            if (!CanUse())
            {
                spriteRenderer.color = Palette.DisabledClear;
                spriteRenderer.material.SetFloat(Desat, 1f);
                return;
            }
            spriteRenderer.color = Palette.EnabledColor;
            spriteRenderer.material.SetFloat(Desat, 0f);
            if (!hasLimitedUse) return;
            if (currentUses > 0)
            {
                spriteRenderer.color = Palette.EnabledColor;
                spriteRenderer.material.SetFloat(Desat, 0f);
                return;
            }
            spriteRenderer.color = Palette.DisabledClear;
            spriteRenderer.material.SetFloat(Desat, 1f);
        }

        protected void OnClickListener()
        { 
            if (!gameObject.active || !CanUse() || isCoolingDown) return;
            if (hasLimitedUse)
            {
                if (!(currentUses > 0)) return;
                currentUses--;
                SetCoolDown(timer = maxTimer, maxTimer);
                OnClick();
                return;
            }
            SetCoolDown(timer = maxTimer, maxTimer);
            OnClick();
        }
        
        public void SetCoolDown(float _timer, float _maxTimer)
        {
	        float num = Mathf.Clamp01(_timer / _maxTimer);
            spriteRenderer.material.SetFloat(Percent, num);
	        isCoolingDown = num > 0f;
	        if (isCoolingDown)
	        {
		        timerText.Text = Mathf.CeilToInt(_timer).ToString();
		        timerText.gameObject.SetActive(true);
		        return;
	        }
	        timerText.gameObject.SetActive(false);
        }
        
        protected abstract bool CouldUse();
        protected abstract bool CanUse();
        protected abstract void OnClick();
        protected abstract void Init();
        protected virtual void Update() { }
        
        public void Dispose()
        {
            allButtons.Remove(this);
            Object.Destroy(gameObject);
        }
    }
    
            
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomButton : Attribute
    {
        public static void CreateButtons()
        {
            Assembly assembly = typeof(WitherPlugin).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute(typeof(CustomButton)) == null || !type.IsSubclassOf(typeof(Button))) continue;
                type.GetConstructor(new Type[0])?.Invoke(new object[0]);
            }
        }
    }
}

