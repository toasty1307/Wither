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
        protected float currentUses = 0;

        public Button()
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

        public static void Update()
        {
            foreach (var button in allButtons)
            {
                button.ButtonUpdate();
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
                spriteRenderer.color = Palette.DisabledColor;
                spriteRenderer.material.SetFloat("_Desat", 1f);
                return;
            }
            spriteRenderer.color = Palette.EnabledColor;
            spriteRenderer.material.SetFloat("_Desat", 0f);
            if (!hasLimitedUse) return;
            if (currentUses > 0)
            {
                spriteRenderer.color = Palette.EnabledColor;
                spriteRenderer.material.SetFloat("_Desat", 0f);
                return;
            }
            spriteRenderer.color = Palette.DisabledColor;
            spriteRenderer.material.SetFloat("_Desat", 1f);
        }

        protected void OnClickListener()
        { 
            if (!gameObject.active) return;
            if (!CanUse() || isCoolingDown) return;
            SetCoolDown(timer = maxTimer, maxTimer);
            if (hasLimitedUse)
            {
                if (!(currentUses > 0)) return;
                CanUse();
                currentUses--;
                return;
            }
            OnClick();
        }
        
        public void SetCoolDown(float timer, float maxTimer)
        {
	        float num = Mathf.Clamp01(timer / maxTimer);
            spriteRenderer.material.SetFloat("_Percent", num);
	        isCoolingDown = (num > 0f);
	        if (isCoolingDown)
	        {
		        timerText.Text = Mathf.CeilToInt(timer).ToString();
		        timerText.gameObject.SetActive(true);
		        return;
	        }
	        timerText.gameObject.SetActive(false);
        }
        
        protected abstract bool CouldUse();
        protected abstract bool CanUse();
        protected abstract void OnClick();
        protected abstract void Init();
        
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

