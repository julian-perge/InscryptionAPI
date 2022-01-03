using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using UnityEngine;

namespace APIPlugin
{
	public class NewTribe
	{
		private static string PrefabIconPath = "Art/Cards/TribeIcons/tribeicon_";

		// Total 8 Tribe enums, last index is 7
		private static readonly List<Tribe> OriginalTribes = Enum.GetValues(typeof(Tribe)).Cast<Tribe>().ToList();

		public static readonly Dictionary<Tribe, Sprite> Tribes
			= OriginalTribes
				// Filtering out NUM_TRIBES since it's not an actual tribe
				// Squirrel is also filtered out since it does not have a tribe icon and will cause NRE.
				// total count is now 6, last index is 5
				.Where(t => t != Tribe.NUM_TRIBES && t != Tribe.Squirrel)
				.ToDictionary(
					key => key,
					// the logic to load the Sprite resources is exactly the same how the original code did it
					_ => ResourceBank.Get<Sprite>(PrefabIconPath + _.ToString().ToLowerInvariant())
				);

		public Tribe tribe;
		private Sprite tribeIconSprite;
		private string tribeName;

		private NewTribe(Sprite tribeIconSprite, string tribeName)
		{
			// Since removing 2 tribes, starting number is 2
			// First tribe added will be: 2 + 6 = 8. 8 is the index after all the original tribes
			// Second tribe added will be: 2 + 7 = 9
			// and so on
			this.tribe = (Tribe)2 + Tribes.Count;
			this.tribeIconSprite = tribeIconSprite;
			this.tribeName = tribeName;
			Tribes.Add(this.tribe, this.tribeIconSprite);

			Plugin.Log.LogInfo($"Added custom tribe [{this.tribeName}]!");
		}

		public static NewTribe Add(Sprite tribeIcon, string tribeName)
		{
			return new NewTribe(tribeIcon, tribeName);
		}

		public static NewTribe Add(string tribeIcon, string tribeName)
		{
			return Add(ImageUtils.CreateSpriteFromPng(tribeIcon), tribeName);
		}
	}
}