using System;
using System.Collections;
using Assets.CoreScripts;
using HarmonyLib;
using PowerTools;
using Reactor;
using Reactor.Extensions;
using UnhollowerBaseLib;
using UnityEngine;
using Object = Il2CppSystem.Object;

namespace Wither.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class KillPatch
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
	        if (AmongUsClient.Instance.IsGameOver)
			{
				return false;
			}
			if (!target || __instance.Data.IsDead || !__instance.Data.IsImpostor || __instance.Data.Disconnected)
			{
				int num = target ? ((int)target.PlayerId) : -1;
				Debug.LogWarning(string.Format("Bad kill from {0} to {1}", __instance.PlayerId, num));
				return false;
			}
			GameData.PlayerInfo data = target.Data;
			if (data == null || data.IsDead)
			{
				// Debug.LogWarning("Missing target data for kill");
				return false;
			}
			if (__instance.AmOwner)
			{
				StatsManager instance = StatsManager.Instance;
				uint num2 = instance.ImpostorKills;
				instance.ImpostorKills = num2 + 1U;
				SoundManager.Instance.PlaySound(__instance.KillSfx, false, 0.8f);
			}
			__instance.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
			DestroyableSingleton<Telemetry>.Instance.WriteMurder();
			target.gameObject.layer = LayerMask.NameToLayer("Ghost");
			if (target.AmOwner)
			{
				StatsManager instance2 = StatsManager.Instance;
				uint num2 = instance2.TimesMurdered;
				instance2.TimesMurdered = num2 + 1U;
				if (Minigame.Instance)
				{
					try
					{
						Minigame.Instance.Close();
						Minigame.Instance.Close();
					}
					catch { }
				}
				DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowOne(__instance.Data, data);
				DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(false);
				target.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
				target.RpcSetScanner(false);
				ImportantTextTask importantTextTask = new GameObject("_Player").AddComponent<ImportantTextTask>();
				importantTextTask.transform.SetParent(__instance.transform, false);
				if (!PlayerControl.GameOptions.GhostsDoTasks)
				{
					for (int i = 0; i < target.myTasks.Count; i++)
					{
						PlayerTask playerTask = target.myTasks[i];
						playerTask.OnRemove();
						UnityEngine.Object.Destroy(playerTask.gameObject);
					}
					target.myTasks.Clear();
					
					importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GhostIgnoreTasks, (Il2CppReferenceArray<Object>) Array.Empty<object>());
				}
				else
				{
					importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GhostDoTasks, new Il2CppReferenceArray<Object>(0));
				}
				target.myTasks.Insert(0, importantTextTask);
			}
			Coroutines.Start(CoPerformKill(__instance, target, __instance.KillAnimations.Random<KillAnimation>()));
			return false;
	    }

        public static IEnumerator CoPerformKill(PlayerControl source, PlayerControl target, KillAnimation animation)
        {
	        FollowerCamera cam = Camera.main.GetComponent<FollowerCamera>();
	        bool isParticipant = PlayerControl.LocalPlayer == source || PlayerControl.LocalPlayer == target;
	        PlayerPhysics sourcePhys = source.MyPhysics;
	        KillAnimation.SetMovement(source, false);
	        KillAnimation.SetMovement(target, false);
	        if (isParticipant)
	        {
		        cam.Locked = true;
	        }
	        target.Die(DeathReason.Kill);
	        SpriteAnim sourceAnim = source.GetComponent<SpriteAnim>();
	        yield return new WaitForAnimationFinish(sourceAnim, animation.BlurAnim);
	        if (target.CurrentPet != null)
	        {
		        target.CurrentPet.SetMourning();
	        }
	        sourceAnim.Play(sourcePhys.IdleAnim, 1f);
	        KillAnimation.SetMovement(source, true);
	        DeadBody deadBody = UnityEngine.Object.Instantiate<DeadBody>(animation.bodyPrefab);
	        Vector3 vector = target.transform.position + animation.BodyOffset;
	        vector.z = vector.y / 1000f;
	        deadBody.transform.position = vector;
	        deadBody.ParentId = target.PlayerId;
	        target.SetPlayerMaterialColors(deadBody.GetComponent<Renderer>());
	        KillAnimation.SetMovement(target, true);
	        if (isParticipant)
	        {
		        cam.Locked = false;
	        }
        }
    }
}