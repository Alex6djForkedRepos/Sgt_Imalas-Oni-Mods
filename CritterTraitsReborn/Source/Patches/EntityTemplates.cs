using HarmonyLib;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using static EntityTemplates;
using static FactionManager;

namespace CritterTraitsReborn.Patches
{
	[HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToBasicCreature), new Type[] { typeof(EntityTemplates.ExtendEntityToBasicCreatureData) })]
	class EntityTemplates_ExtendEntityToBasicCreature
	{
		static void Postfix(ref GameObject __result)
		{
			__result.AddOrGet<Components.CritterTraits>();
		}
	}
}
