using System;
using System.Collections.Generic;
using HarmonyLib;
using InnerNet;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Wither.Utils
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
        

        public Button()
        {
            SetVars();
            gameObject = Object.Instantiate(HudManager.Instance.KillButton.gameObject, HudManager.Instance.transform);
            gameObject.SetActive(true);
            killButtonManager = gameObject.GetComponent<KillButtonManager>();
            timerText = killButtonManager.TimerText;
            aspectPosition = gameObject.GetComponent<AspectPosition>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            button = gameObject.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityAction)OnClickListener);
            spriteRenderer.sprite = sprite;
            aspectPosition.Alignment = edgeAlignment;
            aspectPosition.DistanceFromEdge = (spriteRenderer.size / 2) + offset;
            aspectPosition.AdjustPosition();
            SetCoolDown(timer = maxTimer, maxTimer);
            commonCanUse = (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started || AmongUsClient.Instance.GameMode == GameModes.FreePlay);
            allButtons.Add(this);
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
            spriteRenderer.color = CanUse() ?  Palette.EnabledColor : Palette.DisabledColor;
            spriteRenderer.material.SetFloat("_Desat", CanUse() ? 0f : 1f);
            spriteRenderer.sprite = sprite;
            aspectPosition.AdjustPosition();
        }

        protected void OnClickListener()
        { 
            if (!gameObject.active) return;
            if (!CanUse() || isCoolingDown) return;
            SetCoolDown(timer = maxTimer, maxTimer);
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
        
        protected abstract void SetVars();
        protected abstract bool CouldUse();
        protected abstract bool CanUse();
        protected abstract void OnClick();
        
        public void Dispose()
        {
            allButtons.Remove(this);
            Object.Destroy(gameObject);
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class HudUpdate
        {
            public static void Postfix() => Update();
        }
    }
}