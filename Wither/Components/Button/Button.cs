using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using InnerNet;
using Reactor;
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
        protected ConfigEntry<string> offsetConfig;
        protected SpriteRenderer spriteRenderer;
        protected BoxCollider2D collider;
        protected TextRenderer timerText;
        protected PassiveButton button;
        protected Vector2 offset;
        protected Sprite sprite;
        protected float timer;
        protected float maxTimer;
        protected bool isCoolingDown;
        protected bool hasLimitedUse = false;
        protected float maxUses = 0;
        protected float currentUses;
        protected bool overrideOffset = false;
        protected bool isSelectedForEdit = false;
        private static readonly int Desat = Shader.PropertyToID("_Desat");
        private static readonly int Percent = Shader.PropertyToID("_Percent");
        private static bool inEditMode;

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
            collider = gameObject.GetComponent<BoxCollider2D>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            button = gameObject.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityAction)OnClickListener);
            aspectPosition.updateAlways = true;
            allButtons.Add(this);
            offsetConfig = PluginSingleton<WitherPlugin>.Instance.Config.Bind("Button Positions", allButtons.IndexOf(this) + "B", "-1 -1");
        }

        protected void InitializePart2()
        {
            SetCoolDown(timer = maxTimer);
            aspectPosition.Alignment = edgeAlignment;
            spriteRenderer.sprite = sprite;
            currentUses = maxUses;
            if (offsetConfig.Value == "-1 -1" || overrideOffset)
            {
                offsetConfig.Value = $"{offset.x} {offset.y}";
                offsetConfig.BoxedValue = offsetConfig.Value;
                aspectPosition.DistanceFromEdge = offset;
                return;
            }

            float num1 = Convert.ToSingle(offsetConfig.Value.Substring(0, offsetConfig.Value.IndexOf(" ", StringComparison.Ordinal)));
            float num2 = Convert.ToSingle(offsetConfig.Value.Substring(offsetConfig.Value.IndexOf(" ", StringComparison.Ordinal)));
            offset = new Vector2(num1, num2);
            aspectPosition.DistanceFromEdge = offset;
        }

        public static TextRenderer taskCompleteOverlayTextRenderer;

        public static void SUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                inEditMode = !inEditMode;
                if (!inEditMode)
                {
                    HudManager.Instance.TaskCompleteOverlay.gameObject.SetActive(false);

                    foreach (var button in allButtons)
                    {
                        button.isSelectedForEdit = false;
                        button.offsetConfig.Value = $"{button.aspectPosition.DistanceFromEdge.x} {button.aspectPosition.DistanceFromEdge.y}";
                        button.offsetConfig.BoxedValue = button.offsetConfig.Value;
                    }
                }
                else
                {
                    HudManager.Instance.TaskCompleteOverlay.gameObject.SetActive(true);
                }
            }

            PlayerControl.LocalPlayer.MyPhysics.body.constraints = inEditMode ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
            
            taskCompleteOverlayTextRenderer.Text = inEditMode ? "In Edit Mode, Press F12 To Exit" : "Task Completed!";

            foreach (var button in allButtons)
            {
                if (inEditMode)
                {
                    button.EditUpdate();
                    continue;
                }
                button.ButtonUpdate();
            }
        }

        private void EditUpdate()
        {
            Vector2 mousePos = Input.mousePosition;
            if (isSelectedForEdit)
            {
                spriteRenderer.color = Palette.EnabledColor;
                spriteRenderer.material.SetFloat(Desat, 0f);
            }
            else
            {
                spriteRenderer.color = Palette.DisabledClear;
                spriteRenderer.material.SetFloat(Desat, 1f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(mousePos);
                cursorPosition.z = gameObject.transform.position.z;
                if (collider.bounds.Contains(cursorPosition))
                {
                    isSelectedForEdit = true;
                }
                else
                {
                    isSelectedForEdit = false;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isSelectedForEdit = false;
            }

            if (!isSelectedForEdit) return;

            mousePos = new Vector2(Mathf.Clamp(mousePos.x, 0, Screen.width), Mathf.Clamp(mousePos.y, 0, Screen.height));
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            pos.z = gameObject.transform.position.z;
            pos = new Vector3((float) Math.Round(pos.x, 1), (float) Math.Round(pos.y, 1), pos.z);
            pos = HudManager.Instance.transform.InverseTransformPoint(pos);
            var origin = HudManager.Instance.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f)));
            origin.z = pos.z;
            var distance = origin - pos;
            distance = new Vector3(Mathf.Abs(distance.x), Mathf.Abs(distance.y), pos.z);
            aspectPosition.DistanceFromEdge = Vector3.Lerp(aspectPosition.DistanceFromEdge, distance, 0.25f);
        }

        private void ButtonUpdate()
        {
            gameObject.SetActive(CouldUse() && CommonCanUse());
            if (!gameObject.active) return;
            if (PlayerControl.LocalPlayer.CanMove)
                timer -= Time.deltaTime;
            SetCoolDown(timer);
            spriteRenderer.sprite = sprite;
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
            if (inEditMode) return;
            if (!gameObject.active || !CanUse() || isCoolingDown) return;
            if (hasLimitedUse)
            {
                if (!(currentUses > 0)) return;
                currentUses--;
                SetCoolDown(timer = maxTimer);
                OnClick();
                return;
            }
            SetCoolDown(timer = maxTimer);
            OnClick();
        }

        private static bool CommonCanUse()
        {
            return (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started ||
                    AmongUsClient.Instance.GameMode == GameModes.FreePlay) && 
                   PlayerControl.LocalPlayer && ShipStatus.Instance && !MeetingHud.Instance;
        }

        public void SetCoolDown(float _timer)
        {
	        float num = Mathf.Clamp01(_timer / maxTimer);
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
}

