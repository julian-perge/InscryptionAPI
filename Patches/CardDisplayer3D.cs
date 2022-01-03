using System.Linq;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace API.Patches
{
	[HarmonyPatch(typeof(CardDisplayer3D))]
	public class CardDisplayer3DPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(CardDisplayer3D.GetEmissivePortrait))]
		private static bool GetEmissiveSpritePrefix(Sprite mainPortrait, ref Sprite __result)
		{
			Sprite sprite;
			if (NewCard.emissions.TryGetValue(mainPortrait.name, out sprite))
			{
				__result = sprite;
				return false;
			}

			if (CustomCard.emissions.TryGetValue(mainPortrait.name, out sprite))
			{
				__result = sprite;
				return false;
			}

			return true;
		}


		[HarmonyPrefix, HarmonyPatch(nameof(CardDisplayer3D.UpdateTribeIcon))]
		public static bool SetTribeIconPrefix(CardInfo info, CardDisplayer3D __instance)
		{
			var cardOfTribe = NewTribe.Tribes
				.Where(tribeWithSprite => info.IsOfTribe(tribeWithSprite.Key));

			// there are 5 TribeIconRenderers
			// Amalgam is the only card that uses all 5 renders
			foreach (var iconRenderer in __instance.tribeIconRenderers)
			{
				iconRenderer.sprite = null;
			}

			foreach (var tribe in cardOfTribe)
			{
				Plugin.Log.LogDebug($"Card [{info.name}] Tribe is [{tribe.Key}]");
				foreach (var spriteRenderer in __instance.tribeIconRenderers.Where(renderer => renderer.sprite == null))
				{
					// Plugin.Log.LogDebug($"-> Setting spriteRenderer to tribe");
					spriteRenderer.sprite = tribe.Value;
					break;
				}
			}

			return false;
		}
	}
}