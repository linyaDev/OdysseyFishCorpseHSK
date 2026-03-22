using System.Collections.Generic;
using HarmonyLib;
using Verse;
using Verse.Sound;

namespace OdysseyFishCorpse;

/// <summary>
/// Глушит звуки, привязанные к временной рыбе-пешке (отложенный ambient после спавна и т.д.).
/// </summary>
internal static class FishCorpseSoundMute
{
	private static readonly HashSet<int> SuppressedThingIds = new();

	internal static void Register(Pawn fish)
	{
		SuppressedThingIds.Add(fish.thingIDNumber);
	}

	internal static void ScheduleUnregister(Pawn fish)
	{
		LongEventHandler.ExecuteWhenFinished(() => SuppressedThingIds.Remove(fish.thingIDNumber));
	}

	private static bool MatchesSuppressed(Thing? t)
	{
		if (t == null)
		{
			return false;
		}

		if (t is Pawn p && SuppressedThingIds.Contains(p.thingIDNumber))
		{
			return true;
		}

		if (t is Corpse c && c.InnerPawn != null && SuppressedThingIds.Contains(c.InnerPawn.thingIDNumber))
		{
			return true;
		}

		return false;
	}

	private static bool ShouldSuppress(SoundInfo info)
	{
		if (!info.Maker.HasThing)
		{
			return false;
		}

		return MatchesSuppressed(info.Maker.Thing);
	}

	[HarmonyPatch(typeof(SoundStarter), nameof(SoundStarter.PlayOneShot))]
	private static class Patch_SoundStarter_PlayOneShot
	{
		[HarmonyPrefix]
		private static bool Prefix(SoundDef def, SoundInfo info)
		{
			return !ShouldSuppress(info);
		}
	}

	[HarmonyPatch(typeof(SoundStarter), nameof(SoundStarter.TrySpawnSustainer))]
	private static class Patch_SoundStarter_TrySpawnSustainer
	{
		[HarmonyPrefix]
		private static bool Prefix(SoundDef def, SoundInfo info, ref Sustainer? __result)
		{
			if (ShouldSuppress(info))
			{
				__result = null;
				return false;
			}

			return true;
		}
	}
}
