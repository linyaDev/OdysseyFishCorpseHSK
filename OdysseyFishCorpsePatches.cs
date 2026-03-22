using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using SK;
using UnityEngine;
using Verse;

namespace OdysseyFishCorpse;

[StaticConstructorOnStartup]
public static class OdysseyFishCorpseHarmony
{
	static OdysseyFishCorpseHarmony()
	{
		new Harmony("local.odyssey.fishcorpse").PatchAll(typeof(OdysseyFishCorpseHarmony).Assembly);
	}
}

[HarmonyPatch(typeof(FishingUtility), nameof(FishingUtility.GetCatchesFor))]
public static class OdysseyFishCorpsePatches_FishingUtility_GetCatchesFor
{
	public static void Postfix(Pawn pawn, IntVec3 cell, bool animalFishing, ref bool rare, ref List<Thing> __result)
	{
		if (!ModsConfig.OdysseyActive || pawn?.Map == null || __result == null || __result.Count == 0)
		{
			return;
		}

		var species = Util_FishIndustry.GetFishSpeciesList(pawn.Map.Biome);
		if (species.NullOrEmpty())
		{
			return;
		}

		var output = new List<Thing>();
		foreach (Thing t in __result)
		{
			if (t is Corpse)
			{
				output.Add(t);
				continue;
			}

			if (!IsVanillaFishResource(t.def))
			{
				output.Add(t);
				continue;
			}

			int stack = Mathf.Max(1, t.stackCount);
			t.Destroy(DestroyMode.Vanish);

			// Один труп на стопку улова (без дублирования по stackCount).
			Corpse? corpse = TryMakeFishCorpse(pawn, cell, t.def);
			if (corpse != null)
			{
				if (corpse.def.stackLimit > 1)
				{
					corpse.stackCount = Mathf.Min(stack, corpse.def.stackLimit);
				}

				output.Add(corpse);
			}
		}

		__result.Clear();
		__result.AddRange(output);
	}

	private static bool IsVanillaFishResource(ThingDef def)
	{
		if (def == ThingDefOf.Fish_Toxfish)
		{
			return true;
		}

		if (def.thingCategories != null && def.thingCategories.Any(c => c.defName == "Fish"))
		{
			return true;
		}

		return false;
	}

	private static Corpse? TryMakeFishCorpse(Pawn fisher, IntVec3 cell, ThingDef fishItemDef)
	{
		Map map = fisher.Map;
		PawnKindDef? kind = SelectFishKind(map, cell, fishItemDef);
		if (kind == null)
		{
			return null;
		}

		Pawn fish = PawnGenerator.GeneratePawn(kind, null);
		Thing? spawned = GenSpawn.Spawn(fish, fisher.Position, map, WipeMode.Vanish);
		if (spawned == null)
		{
			fish.Destroy(DestroyMode.Vanish);
			return null;
		}

		KillFishForCorpse(fish, fisher);
		Corpse? corpse = fish.Corpse;
		if (corpse != null && corpse.Spawned)
		{
			corpse.DeSpawn();
		}

		return corpse;
	}

	private static void KillFishForCorpse(Pawn fish, Pawn fisher)
	{
		if (fish.Dead)
		{
			return;
		}

		BodyPartRecord? brain = fish.health.hediffSet.GetBrain();
		if (brain != null)
		{
			DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, 999f, 999f, -1f, fisher, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, instigatorGuilty: false);
			dinfo.SetIgnoreArmor(ignoreArmor: true);
			fish.TakeDamage(dinfo);
		}

		if (!fish.Dead)
		{
			fish.Kill(null);
		}
	}

	private static PawnKindDef? SelectFishKind(Map map, IntVec3 cell, ThingDef fishItemDef)
	{
		List<PawnKindDef_FishSpecies> list = Util_FishIndustry.GetFishSpeciesList(map.Biome);
		if (list.NullOrEmpty())
		{
			return null;
		}

		if (fishItemDef == ThingDefOf.Fish_Toxfish)
		{
			IEnumerable<PawnKindDef_FishSpecies> tox = list.Where(f => f.livesInMarsh);
			if (tox.Any())
			{
				return tox.RandomElementByWeight(f => f.commonality);
			}
		}

		bool ocean = TerrainCheck.IsWaterOcean(cell, map);
		bool marsh = TerrainCheck.IsMud(cell, map);
		IEnumerable<PawnKindDef_FishSpecies> q = ocean
			? list.Where(f => f.livesInOcean)
			: marsh
				? list.Where(f => f.livesInMarsh)
				: list.Where(f => f.livesInRiver);

		if (!q.Any())
		{
			q = list;
		}

		return q.RandomElementByWeight(f => f.commonality);
	}
}
